
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 魔唾液 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;

        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
        }
        public override void AI()
        {
            Projectile.scale = Projectile.ai[2];
            Projectile.ProjScaleChange();
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 20;
            }
            else
            {
                if (Main.rand.NextBool(3))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(-Projectile.velocity.PerfectNormalize(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4), default) * Main.rand.NextFloat(0, 2.2F);
                        Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 18, 0, 0, 100)];
                        dust.velocity = projDirection;
                        dust.scale = Projectile.scale*2;
                        dust.noGravity = true;
                    }
                }
            }
            if (Projectile.ai[1] != 0)
            {
                Projectile.ai[1]++;
                Projectile.extraUpdates = 4;
                Projectile.velocity *= 0.1f;
                if (Projectile.ai[1] > 2)
                {
                    Projectile.Kill();
                }
                return;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 6 == 0)
            {
                Projectile.frame++;
                Projectile.frame %= 3;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.ai[0]++;
            if (Projectile.ai[0] > 30)
            {
                if (Projectile.velocity.Y < 10)
                {
                    Projectile.velocity.Y += 0.2f;
                }
                Projectile.tileCollide = true;
            }
            Projectile.velocity.X *= 0.98f;
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != 2)
            {
                if (Projectile.ai[1] == 0)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(-Projectile.velocity.PerfectNormalize(), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) + Projectile.velocity.PerfectNormalize() * 12, 1, 1, 18, 0, 0, 100)];
                        dust.velocity = projDirection;
                        dust.scale = Projectile.scale;
                    }
                }
                else
                {
                    float Pi = 0;
                    if (Projectile.velocity.X > 0)
                    {
                        Pi = MathHelper.PiOver2;
                        if (Projectile.DProj().vector[0].Y < 0)
                        {
                            Pi = -Pi;
                        }
                    }
                    else if (Projectile.velocity.X < 0)
                    {
                        Pi = -MathHelper.PiOver2;
                        if (Projectile.DProj().vector[0].Y < 0)
                        {
                            Pi = -Pi;
                        }
                    }
                    else
                    if (Projectile.velocity.Y > 0)
                    {
                        Pi = -MathHelper.PiOver2;
                        if (Projectile.DProj().vector[0].X < 0)
                        {
                            Pi = -Pi;
                        }
                    }
                    else if (Projectile.velocity.Y < 0)
                    {
                        Pi = MathHelper.PiOver2;
                        if (Projectile.DProj().vector[0].X < 0)
                        {
                            Pi = -Pi;
                        }
                    }
                    if(Projectile.velocity==Vector2.Zero)
                    {
                        Pi = 0;

                        for (int i = 0; i < 60; i++)
                        {
                            Vector2 projDirection = Utils.RotatedBy(-Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(Pi), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                            Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) + Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(Pi) * 12, 1, 1, 18, 0, 0, 100)];
                            dust.velocity = projDirection;
                            dust.scale = Projectile.scale;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 60; i++)
                        {
                            Vector2 projDirection = Utils.RotatedBy(-Projectile.velocity.PerfectNormalize().RotatedBy(Pi), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                            Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) + Projectile.velocity.PerfectNormalize().RotatedBy(Pi) * 12, 1, 1, 18,0,0,100)];
                            dust.velocity = projDirection;
                            dust.scale = Projectile.scale;
                        }
                    }
                }
                PlaySound(SoundID.NPCDeath1, Projectile.Center);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (!target.HasBuff(ModContent.BuffType<蠕虫毒牙Buff>()))
            {
                target.AddBuff(ModContent.BuffType<蠕虫毒牙Buff>(), 20, false);
            }
            else
            {
                target.AddBuff(ModContent.BuffType<蠕虫毒牙Buff>(), 20 + target.buffTime[target.FindBuffIndex(ModContent.BuffType<蠕虫毒牙Buff>())], false);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = 1;
                Projectile.DProj().vector[0] = oldVelocity;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Color color = Projectile.GetAlpha(lightColor);
            Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]));
            if (Projectile.velocity.X > 0)
            {  for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color2 = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                    Main.spriteBatch.Draw(texture, vector2, rectangle, color2, Projectile.rotation, rectangle.Value.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangle, color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);

            }
            else
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size/2;
                    Color color2 = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                    Main.spriteBatch.Draw(texture, vector2, rectangle, color2, Projectile.rotation, rectangle.Value.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), (SpriteEffects)1, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)1, 0f);
            }
            return false;
        }
    }
}
