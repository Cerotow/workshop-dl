
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class TrueVampiricArrow : ModProjectile
    {
        public Asset<Texture2D> Glow;
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
            Projectile.penetrate = -1;
            DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Ranged/TrueVampiricArrow_Glow");
            DDGlobalProjectile.ScaleGlow[Projectile.type] =4;
            DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255,255,255,0);
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 5;
                Dust dust = Main.dust[NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 5)];
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if(Projectile.velocity.Length()<1)
            {
                Projectile.Kill();
            }
            if(Projectile.DProj().Times[0]>20)
            {
                Projectile.DProj().Times[0] = 20;
            }
            if (Projectile.damage < 1) Projectile.damage = 1;
            Projectile.netUpdate = true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.velocity *= 0.85F;
            Projectile.damage = (int)(Projectile.damage *0.8F);
            Projectile.knockBack= (int)(Projectile.knockBack * 0.8F);
            Projectile.DProj().Times[0]++;
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30+30 * Projectile.DProj().Times[0]; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 235)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.8f;
            }
            if (Main.myPlayer != Projectile.owner)
            {
                return;
            }
            for (int A = 0; A < Projectile.DProj().Times[0]/5; A++)
            {
                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedBy(Main.rand.NextFloat(0,MathHelper.TwoPi))*Main.rand.NextFloat(6,10), ModContent.ProjectileType<SuckBloodPlayer2>(), 0, 0, Projectile.owner);
            } 
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(255, 40, 40, 0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(255, 40, 40, 0), (float)Math.Pow((double)completionRatio, 1.0)); ;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 10f * Projectile.scale, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["普通拖尾"]);
            }
            GameShaders.Misc["普通拖尾"].SetShaderTexture(DDTextures.MiniVoidStar);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2().PerfectNormalize()*(texture.Height/2-2), 104, null);

            Color color = Color.White;
            color.A = 0;
            Main.spriteBatch.Draw(DDGlobalProjectile.Glow[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, DDGlobalProjectile.Glow[Projectile.type].Size() / 2, Projectile.scale / DDGlobalProjectile.ScaleGlow[Projectile.type]* (0.2F+Projectile.DProj().Times[0]/10), 0, 0f);
            color.A = 255;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);

            return false;
        }
        internal Trailing TrailDrawer;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            float num2 = 0.75f;
            float LaserLength = MathHelper.Lerp(0, Projectile.height/2, num2);
            Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), 0, default);
            float num = 0f;
            return new bool?(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center + Pvelocity * LaserLength, projHitbox.Width, ref num));
        }
    }
}
