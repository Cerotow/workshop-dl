using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class FlowerofFire : AMagicStaff
    {
        public override int ProjShoot => 15;
        public override float Distance => 28;
        public override float ShootDistance => 24;
        public override byte AIStyle => 4;
        public override void Set()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 35;
            Projectile.width = 20;
            Projectile.height = 24;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.extraUpdates = 8;
            Projectile.DProj().Times[4] = 2F;
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.localAI[1] >= 0)
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
                {
                    if (Main.rand.NextBool(20))
                    {
                        int Type = 6;
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 4;
                        dust.scale = 1f;
                    }
                }
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            for (int A = 0; A < 20; A++)
            {
                int Type = 6;
                Dust dust = Main.dust[NewDust(target.Center, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.5F;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 12);
            }
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override SoundStyle Sound()
        {
            SoundStyle sound = SoundID.Item1;
            return sound;
        }
        Color color = new Color(253, 62, 3, 0);
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1]++;
                return false;
            }
            Texture2D texture = Glow.Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (i > 0)
                {
                    if (Math.Abs(Projectile.oldRot[i] - Projectile.oldRot[i - 1]) > 0.01F)
                    {
                        Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                        Color color = new Color(253, 62, 3, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);

                            if (Projectile.spriteDirection == 0)
                            {
                                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale / 4, (SpriteEffects)Projectile.spriteDirection, 0f);
                            }
                            else
                            {
                                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i] + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale / 4, (SpriteEffects)Projectile.spriteDirection, 0f);
                            }
                        
                    }
                }

            }
            if (Projectile.spriteDirection == 0)
            {
                Main.spriteBatch.Draw(texture, Center, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4, (SpriteEffects)Projectile.spriteDirection, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Center, null, color, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale / 4, (SpriteEffects)Projectile.spriteDirection, 0f);
            }
            texture = TextureAssets.Item[112].Value;
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, null, lightColor, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
            }
            else
            {
                Projectile.alpha += 10;
            }
            texture = Glow.Value;
            if (Projectile.spriteDirection == 0)
            {
                Main.spriteBatch.Draw(texture, Center, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4, (SpriteEffects)Projectile.spriteDirection, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Center, null, color, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale / 4, (SpriteEffects)Projectile.spriteDirection, 0f);
            }
            return false;
        }
    }
}