using DDmod.Content.Dusts;
using DDmod.Modkey;

namespace DDmod.Content.Projectiles.Talisman
{
    public class EyeTeeth : Talismans
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
        }
        public override void Set()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.hide = true;
        }
        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            player.itemTime = 5;
            player.itemAnimation = 5;
            player.Center = Projectile.Center + new Vector2(0, 20);
            player.immune = true;
            player.immuneNoBlink = true;
            player.immuneTime = 10;
            player.Dplayer().PDraw = true;
            player.hurtCooldowns[1] = 10;
            player.noFallDmg = true;
            player.immuneAlpha = 255;
            Projectile.tileCollide = true;
            Projectile.DProj().Times[0]++;
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 9 == 0)
            {
                Projectile.frame++;
            }
            Projectile.frame %= 3;

            if (!Projectile.DProj().Bool[1])
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.rotation += 0.8F;
                Projectile.scale = (Projectile.DProj().Times[0] / 30);
            }
            else
            {
                Projectile.frame += 3;
            }
            if (Projectile.DProj().Times[0] > 30 && !Projectile.DProj().Bool[1])
            {
                Projectile.DProj().Bool[1] = true;
                SoundStyle sound = SoundID.ForceRoar;
                sound.Pitch = 0.5f;
                PlaySound(sound, Projectile.position);
                Projectile.velocity = (Projectile.DProj().MouseWorld - player.Center).PerfectNormalize() * 30;
                Projectile.rotation = Projectile.velocity.ToRotation() - 1.57F;
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 58), new Vector2(0, -8).RotatedBy(Main.rand.NextFloat(-1, 1)), 8);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 58), new Vector2(0, -8).RotatedBy(Main.rand.NextFloat(-1, 1)), 8);
                Projectile.scale = 1;
            }
            //粒子
            if (Projectile.DProj().Bool[1])
            {
                for (int a = 0; a < 10; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position + Vector2.UnitX * -20f, Projectile.width + 40, Projectile.height, ModContent.DustType<光球粒子>(), Projectile.velocity.X, Projectile.velocity.Y)];
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(0.6F, 1F);
                    dust.velocity *= 0.5f;
                    dust.color = new Color(255, 50, 50);
                }
            }
            Projectile.spriteDirection = 0;
        }
        public override bool Use()
        {
               Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = player.direction;
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD && ModkeySetup.TalismanKey.JustPressed && Projectile.owner == Main.myPlayer && !player.HasBuff(23))
            {
                Projectile.DProj().Bool[0] = true;
                Projectile.DProj().Bool[1] = false;
                Projectile.width = 110;
                Projectile.height = 110;
                Projectile.Center = Projectile.Player().Center + new Vector2(10 * Projectile.Player().direction, 0);
                Projectile.DProj().MouseWorld = Main.MouseWorld;
                Projectile.DProj().Times[0] = 0;
                player.TPlayer().TalismanTimes = player.TPlayer().MaxTalismanTimes;
                player.TPlayer().TalismanCD = 0;
                Projectile.netUpdate = true;
            }
            return false;
        }
        public override bool MobileAI()
        {
            if(NewGore)
            {
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 58), new Vector2(0, -8).RotatedBy(Main.rand.NextFloat(-1, 1)), 9);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 58), new Vector2(0, -8).RotatedBy(Main.rand.NextFloat(-1, 1)), 9);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 58), new Vector2(0, -8).RotatedBy(Main.rand.NextFloat(-1, 1)), 10);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 58), new Vector2(0, -8).RotatedBy(Main.rand.NextFloat(-1, 1)), 10);
                for (int a = 0; a < 300; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<光球粒子>())];
                    dust.noGravity = true;
                    dust.velocity = new Vector2(0, Main.rand.NextFloat(1, 20)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi, 0));
                    dust.scale = Main.rand.NextFloat(1F, 2F);
                    dust.color = new Color(255, 50, 50);
                }
                Projectile.width = 20;
                Projectile.height = 20;
                Projectile.rotation = 0;
                Projectile.netUpdate = true;
                NewGore = false;
            }
            return base.MobileAI();
        }
        public override bool PreEnd()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.Center.Y < 0 || Projectile.Center.X < 0 || Projectile.Center.Y > Main.maxTilesY * 16 || Projectile.Center.X > Main.maxTilesX * 16)
            {
                Projectile.Tproj().TalismanTimes = 0;
            }
            if (player.TPlayer().TalismanTimes <= 0&& Projectile.owner == Main.myPlayer)
            {
                Projectile.DProj().Bool[0] = false;
                player.TPlayer().TalismanTimes = 0;
                NewGore = true;
                Projectile.ClearInvincibleFrame();
                Projectile.netUpdate = true;
            }
            return false;
        }
        public override bool? CanDamage()
        {
            return Projectile.DProj().Bool[0];
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 20;
            height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.DProj().Bool[0])
            {
                player.TPlayer().TalismanTimes = 0;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            if(Projectile.spriteDirection==1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(23, 147, 234, 0);
            Vector2 vector = Projectile.Size / 2;
            if (Projectile.DProj().Bool[0])
            {
                Main.instance.LoadNPC(4);
                texture = TextureAssets.Npc[4].Value;

                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 6 * Projectile.frame, texture.Width, texture.Height / 6)), Color.White * (Projectile.DProj().Times[0] / 30), Projectile.rotation, new Vector2(texture.Width/2, texture.Height / 6*0.7F), Projectile.scale, spriteEffects, 0f);
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 6 * Projectile.frame, texture.Width, texture.Height / 6)), new Color(255,0,0,0) * (Projectile.DProj().Times[0] / 30) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length/2), Projectile.rotation, new Vector2(texture.Width/2, texture.Height / 6 * 0.7F), Projectile.scale, spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 6 * Projectile.frame, texture.Width, texture.Height / 6)), new Color(255,0,0,0) * (Projectile.DProj().Times[0] / 30) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length/2), Projectile.rotation, new Vector2(texture.Width/2, texture.Height / 6 * 0.7F), Projectile.scale, spriteEffects, 0f);
                }
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            }
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}