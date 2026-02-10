
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class TrueShadowArrows : ModProjectile
    {
        public Asset<Texture2D> Glow;
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Ranged/TrueShadowArrows_Glow");
            DDGlobalProjectile.ScaleGlow[Projectile.type] =4;
            DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255,255,255,0);
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void AI()
        {
            if (Projectile.ai[0] > 0)
            {
                Projectile.penetrate = 1;
                if (Projectile.localAI[0]==0)
                {
                    Projectile.localAI[0] = Projectile.velocity.Length();
                }
                Projectile.Track(400, 20, Projectile.localAI[0], 10);
            }
            Dust dust = Main.dust[NewDust(Projectile.position+Projectile.velocity.PerfectNormalize()*14, Projectile.width, Projectile.height, 27)];
            dust.noGravity = true;
            dust.alpha = 100;
            dust.scale = 1.3f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame,180);
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 200; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 6F);
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 27)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
            SoundStyle sound = SoundID.Item14;
            sound.Pitch = -0.5F;
            PlaySound(sound, Projectile.Center);
            NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity*0.001F, ModContent.ProjectileType<ShadowExplosion>(), Projectile.damage, Projectile.knockBack,Projectile.owner);
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(81, 6, 233, 0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(81, 6, 2332, 0), (float)Math.Pow((double)completionRatio, 1.0)); ;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 40f * Projectile.scale, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.FireEffect2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2().PerfectNormalize()*(texture.Height/2-2), 1104, null);

            DDHelper.绘制偏移头部(texture, Projectile, Color.White, MathHelper.Pi, DDGlobalProjectile.Glow[Projectile.type].Value, 10, 4);
            return false;
        }
        internal static Trailing TrailDrawer;
    }
}
