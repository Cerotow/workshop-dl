using DDmod.Content.Dusts;
using DDmod.Modkey;
using Terraria;

namespace DDmod.Content.Projectiles.Talisman
{
    public class MiniSlimeCrown : Talismans
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
            if (!Projectile.DProj().Bool[1])
            {
                player.Center = Projectile.Center + new Vector2(0, 20);
                player.immune = true;
                player.immuneNoBlink = true;
                player.immuneTime = 10;
                player.hurtCooldowns[1] = 10;
                player.noFallDmg = true;
                player.itemTime = 8;
                player.itemAnimation = 2;
            }
            Projectile.tileCollide = true;
            //使用计时器
            if (Projectile.velocity.Y == 0f)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter % 12 == 0)
                {
                    Projectile.frame++;
                }
                Projectile.frame %= 4;
            }
            else
            {
                Projectile.frame = 5;
            }
            if (Projectile.scale < 1)
            {
                Projectile.DProj().Bool[1] = false;
                Projectile.scale += 0.03f;
                Projectile.velocity.Y = 0.01f;
                Projectile.velocity.X = 0f;
                //粒子
                if (Projectile.scale < 0.75f)
                {
                    for (int a = 0; a < 10; a++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(Main.rand.NextFloat(100, 200)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 1, 1, ModContent.DustType<光球粒子>(), Projectile.velocity.X, Projectile.velocity.Y, 150, new Color(78, 136, 255, 0), 2f)];
                        dust.noGravity = true;
                        dust.customData = 5;
                        dust.scale = Main.rand.NextFloat(1.8F, 2.5F);
                        dust.velocity = (Projectile.Center - dust.position) / Main.rand.NextFloat(30, 34) / dust.scale * 5;
                    }
                }
            }
            else
            {
                Projectile.velocity.X = 0f;
                Projectile.velocity.Y = 18f;
                //粒子
                if (!Projectile.DProj().Bool[1])
                {
                    for (int a = 0; a < 4; a++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<光球粒子>(), Projectile.velocity.X, Projectile.velocity.Y, 150, new Color(78, 136, 255, 80), Main.rand.NextFloat(0.8F, 1.6F))];
                        dust.noGravity = true;
                        dust.customData = 3;
                        dust.velocity *= 0.5f;
                    }
                }
            }
            Projectile.spriteDirection = 0;
        }
        public override bool MobileAI()
        {
            if (NewGore)
            {
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 58), new Vector2(0, -8).RotatedBy(Main.rand.NextFloat(-1, 1)), 734);
                for (int a = 0; a < 300; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<光球粒子>(), 0, 0, 150, new Color(78, 136, 255, 80), 2f)];
                    dust.noGravity = true;
                    dust.velocity = new Vector2(0, Main.rand.NextFloat(1, 10)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi, 0));
                    dust.scale = Main.rand.NextFloat(2F, 3F);
                    dust.customData = 3;
                }
                Projectile.width = 20;
                Projectile.height = 20;
                Projectile.netUpdate = true;
                NewGore = false;
            }
            return base.MobileAI();
        }
        public override bool Use()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = player.direction;
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD && ModkeySetup.TalismanKey.JustPressed && Projectile.owner == Main.myPlayer && Projectile.Player().velocity.Y != 0)
            {
                Projectile.DProj().Bool[0] = true;
                Projectile.DProj().Bool[1] = false;
                player.TPlayer().TalismanTimes = player.TPlayer().MaxTalismanTimes;
                Projectile.width = 162;
                Projectile.height = 108;
                Projectile.Center = Projectile.Player().Center + new Vector2(10 * Projectile.Player().direction, 0);
                Projectile.netUpdate = true;
                Projectile.scale = 0.1F;
            }
            return false;
        }
        public override bool PreEnd()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.Center.Y < 0 || Projectile.Center.X < 0 || Projectile.Center.Y > Main.maxTilesY * 16 || Projectile.Center.X > Main.maxTilesX * 16)
            {
                player.TPlayer().TalismanTimes = 0;
            }
            if (Projectile.owner != Main.myPlayer)
            {
                return false;
            }
            if (player.TPlayer().TalismanTimes <= 0)
            {
                Projectile.DProj().Bool[0] = false;
                player.TPlayer().TalismanTimes = 0;
                Projectile.DProj().Bool[1] = false;
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
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if(Projectile.DProj().Bool[1])
            {
                modifiers.SourceDamage *= 0.05F;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y != oldVelocity.Y && !Projectile.DProj().Bool[1] && Projectile.DProj().Bool[0])
            {
                for (int a = 0; a < 220; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position + new Vector2(0,Projectile.height), Projectile.width, 4, ModContent.DustType<光球粒子>(), Projectile.velocity.X, Projectile.velocity.Y, 150, new Color(78, 136, 255, 80), 2f)];
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(2F,3F);
                    dust.velocity = new Vector2(0,-Main.rand.NextFloat(1,8)).RotatedBy(Main.rand.NextFloat(-1.5F,1.5F));
                    dust.customData = 2;
                }
                Projectile.DProj().Bool[1] = true;
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.DProj().MouseWorld = Main.MouseWorld;
                }
                Projectile.netUpdate = true;
                Projectile.Player().velocity = (Projectile.DProj().MouseWorld - Projectile.Player().Center).PerfectNormalize() * 10;
                Projectile.Player().velocity.Y = -14;
            }
            Projectile.velocity *= 0.1f;
            if(Projectile.velocity.Length()<=0.01F)
            {
                Projectile.velocity = Vector2.Zero;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;
            if (Projectile.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.None;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Size / 2;
            if (Projectile.DProj().Bool[0])
            {
                Main.instance.LoadNPC(50);
                texture = TextureAssets.Npc[50].Value;
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 6 * Projectile.frame, texture.Width, texture.Height / 6)), new Color(78, 136, 255, 155), Projectile.rotation, new Vector2(texture.Width, texture.Height / 6) / 2, Projectile.scale, spriteEffects, 0f);

                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 6 * Projectile.frame, texture.Width, texture.Height / 6)), new Color(78, 136, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2), Projectile.rotation, new Vector2(texture.Width, texture.Height / 6) / 2, Projectile.scale, spriteEffects, 0f);
                }
                texture = TextureAssets.Extra[39].Value;
                Vector2 PO = new Vector2(0, 58);
                if (Projectile.frame == 1)
                {
                    PO = new Vector2(0, 66);
                }
                if (Projectile.frame == 2)
                {
                    PO = new Vector2(0, 58);
                }
                if (Projectile.frame == 3)
                {
                    PO = new Vector2(0, 50);
                }
                if (Projectile.frame == 4)
                {
                    PO = new Vector2(0, 56);
                }
                if (Projectile.frame == 5)
                {
                    PO = new Vector2(0, 54);
                }
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition - PO * Projectile.scale, null, Color.White, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, 1, spriteEffects, 0f);
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + vector - Main.screenPosition - PO * Projectile.scale, null, Color.White * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, 1, spriteEffects, 0f);
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