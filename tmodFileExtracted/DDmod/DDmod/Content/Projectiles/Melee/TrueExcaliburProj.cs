using DDmod.Content.Dusts;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class TrueExcaliburProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
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
            Projectile.scale =1f;
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
            Projectile.Track(1000, 10, 30, 10);
            Projectile.ProjScaleChange();
            Dust dust;
            if (Projectile.ai[1] % 3 == 0)
            {
                dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<星光粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, default)];
                dust.noGravity = true;
                dust.scale = Projectile.scale * Main.rand.NextFloat(0.5f, 1f);
                dust.color = new Color(255, 243, 124, 120);
                if (Main.rand.NextBool(2))
                {
                    dust.color = new Color(243, 115, 153, 120);
                }
                dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-1, 1)).PerfectNormalize() * Main.rand.NextFloat(0.5f, 2f);
            }
            /*
            if (Main.rand.NextBool(4))
            {
                dust = Main.dust[Dust.NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * (Projectile.height / 4) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 243, 124, 0) * 0.6F, 2)];
                dust.velocity = -Projectile.velocity.PerfectNormalize().RotatedBy(-Main.rand.NextFloat(0.2F,0.4F)) * 5;
                dust.customData = 1;
                dust.noGravity = false;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;
            }
            if (Main.rand.NextBool(4))
            {
                dust = Main.dust[Dust.NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize().RotatedBy(-MathHelper.PiOver2) * (Projectile.height / 4) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 243, 124, 0) * 0.6F, 2)];
                dust.velocity = -Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(0.2F, 0.4F)) * 5;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;
                dust.customData = 1;
                dust.noGravity = false;
            }*/
        }
        public override void OnKill(int timeLeft)
        {
            int Type = ModContent.DustType<星光粒子>();
            for (int A = 0; A < 4; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, default)];
                dust.noGravity = true;
                dust.scale = Projectile.scale * Main.rand.NextFloat(1f, 3f);
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.5f, 3f);
                dust.color = new Color(255, 243, 124, 120);
                if (Main.rand.NextBool(2))
                {
                    dust.color = new Color(243, 115, 153, 120);
                }
            }
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 0.4f;
            PlaySound(sound, Projectile.position);
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
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition - Projectile.velocity.PerfectNormalize() *(26*Projectile.scale), 88, null,Projectile.scale);
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 vector = Projectile.Size/2;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(100,100,100,255), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(255,255,255,100), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);

            return false;
        }
        internal static Trailing TrailDrawer;
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(255, 243, 124 ),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(243, 115, 153), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                30,
                20,
            }), 10 , (float)Math.Pow((double)completionRatio, 1.0));
        }
    }
}