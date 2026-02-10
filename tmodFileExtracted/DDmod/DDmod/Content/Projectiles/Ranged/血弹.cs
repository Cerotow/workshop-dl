
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 血弹 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;

        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void AI()
        {
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
            if (Main.netMode != 2)
            {
                if (Projectile.ai[1] == 0)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(-Projectile.velocity.PerfectNormalize(), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) + Projectile.velocity.PerfectNormalize() * 12, 1, 1, 5)];
                        dust.velocity = projDirection;
                        dust.alpha = 100;
                        dust.scale = 1f;
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
                            Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) + Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(Pi) * 12, 1, 1, 5)];
                            dust.velocity = projDirection;
                            dust.alpha = 100;
                            dust.scale = 1f;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 60; i++)
                        {
                            Vector2 projDirection = Utils.RotatedBy(-Projectile.velocity.PerfectNormalize().RotatedBy(Pi), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 8.2F);
                            Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) + Projectile.velocity.PerfectNormalize().RotatedBy(Pi) * 12, 1, 1, 5)];
                            dust.velocity = projDirection;
                            dust.alpha = 100;
                            dust.scale = 1f;
                        }
                        for (int i = 0; i < 12; i++)
                        {
                            Vector2 projDirection = Utils.RotatedBy(-Projectile.velocity.PerfectNormalize().RotatedBy(Pi), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0, 2.2F);
                            Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) + Projectile.velocity.PerfectNormalize().RotatedBy(Pi) * 12, 1, 1, ModContent.DustType<冰雾>(),0,0,0,new Color(101,13,13,100))];
                            dust.velocity = projDirection;
                            dust.noGravity = false;
                            dust.alpha = -3000;
                            dust.scale = 1f;
                        }
                    }
                }
                PlaySound(SoundID.NPCDeath1, Projectile.Center);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.NextBool(10))
            {
                float lifeStoled = damageDone * 0.01f;
                if (lifeStoled < 1)
                {
                    lifeStoled = 1;
                }
                if (target.HasBuff(30)) lifeStoled *= 2;
                if ((int)lifeStoled > 0 && !player.moonLeech && target.CanBeChasedBy())
                {
                    NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, lifeStoled);
                }
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
            Color color = lightColor;
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
