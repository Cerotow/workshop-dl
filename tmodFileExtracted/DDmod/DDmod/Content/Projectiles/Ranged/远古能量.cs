
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 远古能量 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
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
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void AI()
        {
            if(!Main.tile[(int)Projectile.Center.X/16,(int)Projectile.Center.Y/16].HasTile&&
                !Main.tile[(int)Projectile.Center.X/16+1,(int)Projectile.Center.Y/16].HasTile&&
                !Main.tile[(int)Projectile.Center.X/16-1,(int)Projectile.Center.Y/16].HasTile&&
                !Main.tile[(int)Projectile.Center.X/16,(int)Projectile.Center.Y/16+1].HasTile&&
                !Main.tile[(int)Projectile.Center.X/16,(int)Projectile.Center.Y/16-1].HasTile)
            {
                Projectile.tileCollide = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.ai[0] == 2)
            {
                Projectile.Track(400, 20, 20, 10);
            }
            for (int a = 0; a < Projectile.velocity.Length()/4; a++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center+ Projectile.velocity - new Vector2(4)-Projectile.velocity.PerfectNormalize()*(a*4)+new Vector2(0, Projectile.ai[1]).RotatedBy(Projectile.velocity.ToRotation()), 1, 1, 172)];
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.5f;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 100; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.8F, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 172)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(0, 55, 255, 0)*0.5F,
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(0, 55, 255, 0) * 0.5F, (float)Math.Pow((double)completionRatio, 1.0)); ;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 10f * Projectile.scale, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.VoidStar.Value;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(0,55,255,0),0, texture.Size()/2, Projectile.scale/3, 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(0,55,255,0),0, texture.Size()/2, Projectile.scale/3, 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(0,55,255,0).Opposite(),0, texture.Size()/2, Projectile.scale/5, 0, 0f);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, Projectile.Size/2, 0, 0f);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return null;
        }
    }
}
