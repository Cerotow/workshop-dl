

using DDmod.Content.Buffs.DeBuffs;
using DDmod.Players;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class Katana : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            Projectile.width = 20;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 50;
            Projectile.extraUpdates = 6;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        int proj = 0;
        //特殊攻击
        bool Special;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(Special);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
            Special = reader.ReadBoolean();
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Special && Projectile.scale == 1 && !player.HasBuff(ModContent.BuffType<SpecialAttackCD>()))
            {
                if (player.controlUseTile)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Projectile.DProj().MouseWorld = Main.MouseWorld;
                        Projectile.DProj().vector[2] = Projectile.DProj().MouseWorld - player.Center;
                        Projectile.netUpdate = true;
                    }
                    player.AddBuff(ModContent.BuffType<SpecialAttackCD>(), 300);
                    Projectile.damage = (int)(Projectile.damage * 2.25f);
                    Projectile.extraUpdates = 30;
                    Special = true;
                    Projectile.netUpdate = true;
                }
            }
            Projectile.scale = 1.01f;
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

            if (!Special)
            {
                Projectile.HoldProj(player, 40 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, 2.1f, false);
            }
            else
            {
                int A = proj / 10;
                if (A > 20)
                {
                    A = 20;
                }

                if (proj >= 60)
                {
                    Projectile.HoldProj(player, (40 - A) * Projectile.scale, Projectile.ai[0], new Vector2(0, 1).RotatedBy(0.8f * player.direction), MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                }
                else
                {
                    Projectile.HoldProj(player, (40 - A) * Projectile.scale, Projectile.ai[0], -player.velocity, MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                }
                Projectile.localAI[1] = 1;
            }
            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                
                proj = 0;
            }
            else
            {
                if (Special&& proj < 60)
                {
                    player.dashDelay = 5;
                    player.velocity.Y = -0.001f;
                    player.velocity.X = -0.001f;
                    player.position += Collision.TileCollision(player.Center-new Vector2(4), Projectile.DProj().vector[2].PerfectNormalize() * 6, 8, 8, true, true);
                    DDPlayer.移动玩家(player, 30, true);
                    if (Collision.TileCollision(player.Center - new Vector2(4), Projectile.DProj().vector[2].PerfectNormalize() * 40 / (Projectile.extraUpdates + 1), 8, 8, false, false)!= Projectile.DProj().vector[2].PerfectNormalize() * 40 / (Projectile.extraUpdates + 1))
                    {
                        proj = 59;
                    }
                    player.Aplayer().NoGravity = 2;
                    player.dashDelay = 5;
                }
                if (proj == 0)
                {
                    if (Special)
                    {
                        SoundStyle sound = SoundID.Item71;
                        sound.Pitch = 0.2F;
                        PlaySound(sound, Projectile.position);
                        int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center + Projectile.DProj().vector[2]/2.75F, Projectile.DProj().vector[2], ModContent.ProjectileType<Slash>(), 0, 0, Projectile.owner, 0, 0, 2);
                        Main.projectile[A].scale = 1.8f;
                    }
                    else
                    {
                        PlaySound(SoundID.Item1, Projectile.position);
                    }
                    proj++;
                    Projectile.netUpdate = true;
                }
            }
            if (Special)
            {
                if (proj != 0)
                {
                    proj++;
                    if (proj >= 60)
                    {
                        Projectile.Player().velocity = new Vector2(0, -0.01F);
                        player.Aplayer().NoGravity = 2;
                        bool TileCollision3 = Collision.SolidCollision(player.position, player.width, player.height / 2);
                        bool TileCollision4 = Collision.SolidCollision(player.position + new Vector2(0, player.height / 2), player.width, player.height / 2);
                        if (TileCollision3)
                        {
                            player.position.Y +=  1;
                        }
                        if (TileCollision4)
                        {
                            player.position.Y -=  1;
                        }
                        Projectile.extraUpdates = 6;
                    }
                    if (proj / 10 == 20)
                    {
                        SoundStyle sound = SoundID.Item71;
                        sound.Pitch = 0.3F;
                        PlaySound(sound, Projectile.position);
                        for (int a = 0; a < 1000; a++)
                        {
                            if (Main.projectile[a].active && Main.projectile[a].type == ModContent.ProjectileType<Slash>() && Main.projectile[a].owner == Projectile.owner && Main.projectile[a].ai[1] == 2)
                            {
                                Main.projectile[a].ai[1] = 3;
                            }
                        }
                        int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center + new Vector2(4 * -player.direction, 6), new Vector2(1, 0), ModContent.ProjectileType<Slash>(), 0, 0, Projectile.owner, 0, 1);
                        Main.projectile[A].scale = 0.6f;
                    }
                    if (proj > 300)
                    {
                        player.itemAnimation = 0;
                        player.itemTime = 0;
                        Projectile.Kill();
                    }
                }
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Special)
            {
                for(int a = -1;a<=1;a+=2)
                {
                    Vector2 vector = new Vector2(a, 1);
                    int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<Slash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, target.whoAmI, 2);
                    Main.projectile[A].scale = 1.4f;
                }
                if (Projectile.Player().HasBuff(ModContent.BuffType<SpecialAttackCD>()) && target.life<=0)
                {
                    Projectile.Player().ClearBuff(ModContent.BuffType<SpecialAttackCD>());
                }
            }
            else
            {
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<Slash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, target.whoAmI, 1);
            }
        }
        Color color = new Color(100, 100, 100, 0);
        public float TWidth()
        {
            return 24 * Projectile.scale;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1]++;
                return false;
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }
            DDHelper.BladeTrail(DDTextures.WhitePng, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, TWidth());
            DDHelper.BladeTrail(DDTextures.Wave, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, TWidth());
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D texture = TextureAssets.Item[2273].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
    }
    public class Slash : ModProjectile
    {
        public override string Texture => "DDmod/Image/VoidStar";
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Slash");
           //DisplayName.AddTranslation(7, "斩");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.width = 8;
            Projectile.height = 90;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 20;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 1f;
            Projectile.ArmorPenetration = 100000;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[1]<2;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[1] != 0)
            {
                return target.whoAmI == (int)Projectile.ai[0];
            }
            return null;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * (0.2f + Main.npc[(int)Projectile.ai[0]].Size.Length() / 100)));
            if (Projectile.velocity.Length() > 0.1f)
                Projectile.velocity = Projectile.velocity.PerfectNormalize()*0.09F;
            Projectile.timeLeft = 20;
            if (Projectile.ai[1] < 2)
            {
                if (Projectile.ai[1] == 1 && Projectile.damage <= 0)
                {
                    Projectile.scale -= 0.02f;
                }
                else
                {
                    Projectile.scale -= 0.04f;
                }
                if (Projectile.scale < 0.05F)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
                Projectile.active = Main.npc[(int)Projectile.ai[0]].active;
                if (Projectile.ai[1] == 3)
                {
                    Projectile.ai[1] = 1;
                    Projectile.scale = 1.2f;
                }
                else
                {
                    Projectile.scale = 0.1f;
                }
            }
            if (Projectile.ai[1] == 1 && Projectile.damage <= 0)
            {
                Projectile.Center += Projectile.Player().velocity / 2;
            }
        }
        public override void OnKill(int timeLeft)
        {

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.Player().HasBuff(ModContent.BuffType<SpecialAttackCD>()) && Projectile.ai[1] == 1 && target.life <= 0)
            {
                Projectile.Player().ClearBuff(ModContent.BuffType<SpecialAttackCD>());
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(155, 155, 155, 0);
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            if (Projectile.ai[1] >= 2)
            {
                return false;
            }
            for (int a = 0; a < 3; a++)
            {
                if (Projectile.damage > 0)
                {
                    if (Projectile.ai[1] == 1)
                    {
                        Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.4f, Projectile.rotation, texture.Size() / 2, new Vector2(0.2f + Main.npc[(int)Projectile.ai[0]].Size.Length() / 100, Projectile.scale / 16) * 1.5f, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 4.4f, Projectile.rotation, texture.Size() / 2, new Vector2(0.2f + Main.npc[(int)Projectile.ai[0]].Size.Length() / 100, Projectile.scale / 16), spriteEffects, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.4f, Projectile.rotation, texture.Size() / 2, new Vector2(0.6f, Projectile.scale / 16) * 1.5f, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 4.4f, Projectile.rotation, texture.Size() / 2, new Vector2(0.6f, Projectile.scale / 16), spriteEffects, 0f);
                    }
                }
                else
                {
                    if (Projectile.ai[1] == 1)
                    {
                        Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.4f, Projectile.rotation, texture.Size() / 2, new Vector2(0.2f, Projectile.scale / 16) * 1.5f, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 4.4f, Projectile.rotation, texture.Size() / 2, new Vector2(0.2f, Projectile.scale / 16), spriteEffects, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.4f, Projectile.rotation, texture.Size() / 2, new Vector2(2f, Projectile.scale / 16) * 1.5f, spriteEffects, 0f);
                        Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 4.4f, Projectile.rotation, texture.Size() / 2, new Vector2(2f, Projectile.scale / 16), spriteEffects, 0f);
                    }
                }
            }

            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num2 = 0.75f;
            float LaserLength = MathHelper.Lerp(0, Projectile.height, num2);
            Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), 0, default);
            float num = 0f;
            bool T = false;
            if(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center + Pvelocity * LaserLength, projHitbox.Width, ref num))
            {
                T = true;
            }
            if(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center - Pvelocity * LaserLength, projHitbox.Width, ref num))
            {
                T = true;
            }
            return new bool?(T);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.ai[1] == 1&&Projectile.damage<=0)
            {
                overPlayers.Add(index);
            }
        }
    }
}
