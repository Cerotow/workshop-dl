using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class TerraEdge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
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
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 12)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 100;
                }
                else if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
            Projectile.Track(1000, 10, 30, 30);
            Projectile.ProjScaleChange();
            Lighting.AddLight(Projectile.Center, new Color(100, 255, 100).ToVector3() * 0.3f);
        }
        public override void OnKill(int timeLeft)
        {
            /*
            int Type = 107;
            for (int A = 0; A < 30; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.8f;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2f, 3f);
                dust.noLightEmittence = false;
            }
            for (int A = 0; A < 3; A++)
            {
                for (int a = 0; a < Projectile.oldPos.Length; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.oldPos[a] + Projectile.Size / 2 - new Vector2(2), 4, 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 0.9f;
                    dust.velocity = Projectile.velocity.PerfectNormalize() / 2;
                    dust.noLightEmittence = false;
                }
            }*/
            Vector2 positionInWorld = Projectile.Center;
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, settings, Projectile.owner);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {

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
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 14, 88, null,Projectile.scale);

            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Color.White*0.5f;
            color.A = 0;
            Vector2 vector = Projectile.Size / 2;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, spriteEffects, 0f);
            texture = DDTextures.Starlight3.Value;
            color = new Color(173, 255, 170, 0) * Projectile.DProj().Times[0]*0.5f;
            vector += Projectile.velocity.PerfectNormalize() * 26;
            DDHelper.BackAndForth(0.4F, 1F, 0.05F, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0], true);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, 0, new Vector2(texture.Width, texture.Height) / 2, new Vector2(0.5F, 1.5F) * Projectile.DProj().Times[0] * 1, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, MathHelper.PiOver2, new Vector2(texture.Width, texture.Height) / 2, new Vector2(0.75F, 3.5F) * Projectile.DProj().Times[0] * 1, spriteEffects, 0f);

            return false;
        }
        internal static Trailing TrailDrawer;
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(83, 255,40 ,0),
                new Color(0, 144, 217,0),
                new Color(19,201,122,0),
                new Color(54,249,152,0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade)*0.5f, new Color(10, 204, 164, 0), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                10,
                20,
                30,
                40,
                30,
                20,
            }), 10, (float)Math.Pow((double)completionRatio, 1.0));
        }
    }
}