using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss心心 : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/爱心光效");
        }
        public float TelegraphDelay
        {
            get
            {
                return Projectile.DProj().Times[0];
            }
            set
            {
                Projectile.DProj().Times[0] = value;
            }
        }
        public override void SetStaticDefaults()
        {
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
            Projectile.timeLeft =500;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y * -1, Projectile.velocity.X * -1) - MathHelper.Pi / 2;
            if (Projectile.ai[0] == -3)
            {
                Projectile.scale = Projectile.ai[2];
                if(Projectile.ai[1]==0)
                {
                    Projectile.velocity *= Projectile.ai[2];
                }
                Projectile.ai[1]++;
                if (Projectile.ai[1] >120&& Projectile.velocity.Length() < 24 * Projectile.ai[2])
                {
                    Projectile.velocity *= 1.01F;
                }
            }
            else
            if (Projectile.ai[0] == -2)
            {
                Projectile.position += Projectile.DProj().vector[0] *= 0.98F;
                if (Projectile.scale < 1.2F)
                {
                    Projectile.scale += 0.01F;
                }
                else
                {
                    Projectile.scale = 1.2F;
                }
                if (Projectile.velocity.Length() < 24)
                {
                    Projectile.velocity *= 1.01F;
                }
            }
            else
            if (Projectile.ai[0] == -1)
            {
                if (Projectile.scale < 2F)
                {
                    Projectile.scale += 0.02F;
                }
                else
                {
                    Projectile.scale = 2F;
                }
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
                    Projectile.DProj().Times[1] *=1.02F;
                }
                if (Projectile.DProj().Times[0] > 40)
                {
                    Projectile.velocity = Projectile.DProj().vector[0] * Projectile.scale * Projectile.DProj().Times[1];
                }
                else
                {
                    Projectile.velocity = Projectile.DProj().vector[0] * Projectile.scale / 10 ;
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
            Projectile.ProjScaleChange();
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
                DDParticle.RequestParticleSpawn(ParticleType.Heart, new ParticleOrchestraSettings
                {
                    PositionInWorld = Projectile.Center,
                    MovementVector = Main.rand.NextVector2Unit(),

                });
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D Glow = DDTextures.VoidStar.Value;
            Color c = new Color(255, 100, 100, 0);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Color color = c * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(Glow, vector2, null, color, Projectile.rotation, Glow.Size() / 2, Projectile.scale * 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }

            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 2);
            Main.spriteBatch.Draw(Glow, Projectile.Center - Main.screenPosition, null, c, Projectile.rotation, Glow.Size() / 2, Projectile.scale * 0.6f, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, vector, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}