using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss星星 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/星星光效");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 500;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.01F;
            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.01F;
            }
            else
            {
                Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.01F;
            }
            if (Projectile.ai[0] == -4)
            {
                Projectile.position += Projectile.DProj().vector[0] *= 0.98F;
                if (Projectile.velocity.Length() < 26)
                {
                    Projectile.velocity *= 1.04F;
                }
            }
            else
            if (Projectile.ai[0] == -3)
            {
                Projectile.scale = Projectile.ai[2];
                if (Projectile.ai[1] == 0)
                {
                    Projectile.velocity *= Projectile.ai[2];
                }
                Projectile.ai[1]++;
                if (Projectile.ai[1] > 120 && Projectile.velocity.Length() < 24 * Projectile.ai[2])
                {
                    Projectile.velocity *= 1.01F;
                }
            }
            else
            if (Projectile.ai[0] == -2)
            {
                Projectile.position += Projectile.DProj().vector[0] *= 0.98F;
                Projectile.velocity.X *= 0.92F;
                if (Projectile.scale < 1.2F)
                {
                    Projectile.scale += 0.01F;
                }
                else
                {
                    Projectile.scale = 1.2F;
                }
                if (Projectile.velocity.Y < 24)
                {
                    Projectile.velocity.Y += 0.4F;
                }
                if (Projectile.DProj().vector[0].Y < 0 && Projectile.position.Y < -Projectile.DProj().vector[0].Y + (-Projectile.velocity.Y))
                {
                    Projectile.position.Y = -Projectile.DProj().vector[0].Y + (-Projectile.velocity.Y);
                }

            }
            else
            if (Projectile.ai[0] == -1)
            {
                Projectile.position += Projectile.DProj().vector[0] *= 0.99F;
                if (Projectile.velocity.Length() < 26)
                {
                    Projectile.velocity *= 1.04F;
                }

            }
            else
            if (Projectile.ai[0] == 0)
            {
                if (!Projectile.DProj().Bool[0])
                {
                    Projectile.scale = 0.05F;
                    Projectile.DProj().vector[0] = Projectile.velocity;
                    Projectile.DProj().Bool[0] = true;
                }
                else
                {
                    Projectile.DProj().Times[0]++;
                    if (Projectile.scale < 1F)
                    {
                        Projectile.scale += 0.002F;
                        if (Projectile.DProj().Times[0] > 120)
                        {
                            Projectile.scale += 0.05F;
                        }
                    }
                    else
                    {
                        Projectile.scale = 1;
                    }
                    Projectile.ProjScaleChange();
                }
                if (Projectile.DProj().Times[0] > 120)
                {
                    Projectile.velocity = Projectile.DProj().vector[0] * Projectile.scale;
                }
                else
                {

                    Projectile.velocity = Projectile.DProj().vector[0] * Projectile.scale / 10;
                }
            }
            else
            if (Projectile.ai[0] == 1)
            {
                if (!Projectile.DProj().Bool[0])
                {
                    Projectile.scale = 0.05F;
                    Projectile.DProj().vector[0] = Projectile.velocity;
                    Projectile.DProj().Bool[0] = true;
                    Projectile.DProj().Times[1] = 1;
                }
                else
                {
                    Projectile.DProj().Times[0]++;
                    if (Projectile.scale < 1F)
                    {
                        Projectile.scale += 0.002F;
                        if (Projectile.DProj().Times[0] > 40)
                        {
                            Projectile.scale += 0.05F;
                        }
                    }
                    else
                    {
                        Projectile.scale = 1;
                    }
                }
                if (Projectile.DProj().Times[0] > 360)
                {
                    Projectile.DProj().Times[1] *= 1.02F;
                }
                if (Projectile.DProj().Times[0] > 40)
                {
                    Projectile.velocity = Projectile.DProj().vector[0] * Projectile.scale * Projectile.DProj().Times[1];
                }
                else
                {
                    Projectile.velocity = Projectile.DProj().vector[0] * Projectile.scale / 10;
                }
            }
            else if (Projectile.ai[0] == 2)
            {

                if (!Projectile.DProj().Bool[0])
                {
                    Projectile.scale = 0.05F;
                    Projectile.DProj().vector[0] = Projectile.velocity * 5 + new Vector2(3, 0);
                    Projectile.DProj().Bool[0] = true;
                }
                else
                {
                    if (Projectile.scale < 1F)
                    {
                        Projectile.scale += 0.02F;
                    }
                    else
                    {
                        Projectile.scale = 1;
                    }
                    Projectile.Center = (Main.npc[(int)Projectile.ai[1]].Center + Projectile.DProj().vector[0].RotatedBy(Main.npc[(int)Projectile.ai[1]].rotation));
                }
                Projectile.velocity = Vector2.Zero;

                Projectile.DProj().Times[0]++;
                if (Projectile.DProj().Times[0] < 90)
                    Projectile.DProj().vector[0] *= 1.02F;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 6; a++)
            {
                DDParticle.RequestParticleSpawn(ParticleType.Star, new ParticleOrchestraSettings
                {
                    PositionInWorld = Projectile.Center,
                    MovementVector = Main.rand.NextVector2Unit() * 3
                });
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D textureGlow = DDTextures.VoidStar.Value;
            Color c = new Color(0, 100, 255, 0);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                Color color = c * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                Main.spriteBatch.Draw(textureGlow, vector2, null, c * 0.4f, 1, textureGlow.Size() / 2, Projectile.scale * 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, v, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);

            Main.spriteBatch.Draw(textureGlow, v, null, c, 1, textureGlow.Size() / 2, Projectile.scale * 0.6f, SpriteEffects.None, 0);
            return false;
        }
    }
}