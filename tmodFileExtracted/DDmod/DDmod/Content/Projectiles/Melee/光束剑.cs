using DDmod.Content.Dusts;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class 光束剑 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] =5;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 320;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.alpha += 255;
            Projectile.scale = 1f;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Player player = Main.player[Projectile.owner];
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.scale *= player.GetAdjustedItemScale(player.ActiveItem());
                Projectile.DProj().Bool[0] = true;
                Projectile.velocity *= player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.ProjScaleChange();
            if (Projectile.ai[0] == 0)
            {
                if (Main.rand.NextBool(5))
                {
                    for (int a = 0; a < 3; a++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<光球粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 255, new Color(255, 175, 60, 100))];
                        dust.noGravity = true;
                        dust.scale = Projectile.scale * 0.2F;
                        dust.velocity = -Projectile.velocity * Main.rand.NextFloat(0.1f, 0.3f);
                    }
                    Dust dust3 = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 255, new Color(255, 175, 60, 100))];
                    dust3.noGravity = true;
                    dust3.scale = Projectile.scale * 3;
                    dust3.velocity = -Projectile.velocity / 8;
                    dust3.rotation = Projectile.velocity.ToRotation();
                }
            }
            if (Projectile.ai[0] == 1)
            {
                Projectile.ai[0]++;
                int Type = ModContent.DustType<光球粒子>();
                for (int A = 0; A < 30; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 175, 60, 100))];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale *1f;
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.2f, 1.3f);
                    dust.noLightEmittence = false;
                }
                for (int A = 0; A < 5; A++)
                {
                    for (int a = 0; a < Projectile.oldPos.Length; a++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.oldPos[a] + Projectile.Size / 2 - new Vector2(2), 4, 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 175, 60, 100))];
                        dust.noGravity = true;
                        dust.scale = Projectile.scale * 1f;
                        dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.2f, 1.3f);
                        dust.noLightEmittence = false;
                    }
                }
                for (int A = 0; A < 3; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<星光粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 175, 60, 100))];
                    dust.noGravity = true;
                    dust.scale = 16;
                    dust.customData = 8;
                    dust.velocity = Vector2.Zero;
                }
                Projectile.position -= Projectile.velocity.PerfectNormalize() * 1500;
                Projectile.velocity = Projectile.velocity.PerfectNormalize() * 15;
                Projectile.timeLeft = 200;
                Projectile.extraUpdates = 100;
                Projectile.scale *= 2;
                for (int A = 0; A < 15; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 175, 60, 100))];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale * 1.2f;
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.2f, 1f);
                    dust.noLightEmittence = false;
                }
                Projectile.netUpdate = true;
                Projectile.tileCollide = false;
            }
            if (Projectile.ai[0] == 2)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<激光粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 255, new Color(255, 175, 60, 100))];
                dust.noGravity = true;
                dust.rotation = Projectile.velocity.ToRotation();
                dust.scale = Projectile.scale * 0.5F;
                dust.velocity = Vector2.Zero;
                dust.customData = -1.3F;
                if (Main.rand.NextBool(5))
                {
                    Dust dust3 = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 255, new Color(255, 175, 60, 100))];
                    dust3.noGravity = true;
                    dust3.scale = Projectile.scale * 3;
                    dust3.velocity = Projectile.velocity;
                    dust3.customData = 2F;
                    dust3.rotation = dust3.velocity.ToRotation();
                }
            }
            Lighting.AddLight(Projectile.Center, new Color(100, 255, 100).ToVector3() * 0.3f);
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 0)
            {
                int Type = ModContent.DustType<光球粒子>();
                for (int A = 0; A < 30; A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 175, 60, 100))];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale * 0.8f;
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.2f, 2f);
                    dust.noLightEmittence = false;
                }
                for (int A = 0; A < 3; A++)
                {
                    for (int a = 0; a < Projectile.oldPos.Length; a++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.oldPos[a] + Projectile.Size / 2 - new Vector2(2), 4, 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 175, 60, 100))];
                        dust.noGravity = true;
                        dust.scale = Projectile.scale * 0.6f;
                        dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.2f, 2f);
                        dust.noLightEmittence = false;
                    }
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] != 0)
            {
                Projectile.damage = (int)(Projectile.damage * 0.8F);
                Projectile.netUpdate = true;
            }

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return base.GetAlpha(lightColor);
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            if (Projectile.ai[0] == 0)
                TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 10, 204, null);

            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(255,215,30,100);
            Vector2 vector = Projectile.Size / 2;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, spriteEffects, 0f);
            return false;
        }
        internal static Trailing TrailDrawer;
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(255,55,30,100),
                new Color(255,115,30,100),
                new Color(255,165,30,100)
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(255, 255, 194, 100), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                20,
            }) * Projectile.scale, 10 * Projectile.scale, (float)Math.Pow((double)completionRatio, 1.0));
        }
    }
}