
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss大魔唾液 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft =60;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;

        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void AI()
        {
            if(Projectile.alpha==255)
            {
                Projectile.ai[2] = Main.rand.NextFloat(1.2F,2F);
            }
            Projectile.scale = Projectile.ai[2];
            Projectile.ProjScaleChange();
            if(Projectile.alpha>0)
            {
                Projectile.alpha -= 20;
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
            if (Projectile.frameCounter%6==0)
            {
                Projectile.frame++;
                Projectile.frame%=3;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 3)
            {
                if (Projectile.velocity.Y < 10)
                {
                    Projectile.velocity.Y += 0.2f;
                }
            }
            Projectile.velocity.X *= 0.98f;
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != 1)
            {
                if (Projectile.ai[1] == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(-Projectile.velocity.PerfectNormalize(), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                        Projectile proj = Main.projectile[NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, projDirection, ModContent.ProjectileType<Boss魔唾液>(), Projectile.damage / 2, 0, -1, 0, 0, Main.rand.NextFloat(0.6F, 1.5F))];
                        proj.alpha = 5;
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
                    if (Projectile.velocity == Vector2.Zero)
                    {
                        Pi = 0;

                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 projDirection = Utils.RotatedBy(-Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(Pi), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                            Projectile proj = Main.projectile[NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, projDirection, ModContent.ProjectileType<Boss魔唾液>(), Projectile.damage / 2, 0, -1, 0, 0, Main.rand.NextFloat(0.6F, 1.5F))];
                            proj.alpha = 5;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 projDirection = Utils.RotatedBy(-Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(Pi), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                            Projectile proj = Main.projectile[NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, projDirection, ModContent.ProjectileType<Boss魔唾液>(), Projectile.damage / 2, 0, -1, 0, 0, Main.rand.NextFloat(0.6F, 1.5F))];
                            proj.alpha = 5;
                        }
                    }
                }

            }
            if (Main.netMode != 2)
            {
                if (Projectile.ai[1] == 0)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(-Projectile.velocity.PerfectNormalize(), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) + Projectile.velocity.PerfectNormalize() * 12, 1, 1, 18)];
                        dust.velocity = projDirection;
                        dust.alpha = 100;
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
                    if (Projectile.velocity == Vector2.Zero)
                    {
                        Pi = 0;

                        for (int i = 0; i < 120; i++)
                        {
                            Vector2 projDirection = Utils.RotatedBy(-Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(Pi), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                            Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) + Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(Pi) * 12, 1, 1, 5)];
                            dust.velocity = projDirection;
                            dust.alpha = 100;
                            dust.scale = Projectile.scale;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 120; i++)
                        {
                            Vector2 projDirection = Utils.RotatedBy(-Projectile.velocity.PerfectNormalize().RotatedBy(Pi), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                            Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) + Projectile.velocity.PerfectNormalize().RotatedBy(Pi) * 12, 1, 1, 5)];
                            dust.velocity = projDirection;
                            dust.alpha = 100;
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
            if (Projectile.velocity.X > 0)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)1, 0f);
            }
            return false;
        }
    }
}
