using DDmod.Content.Items.Melee.Sword;
using Microsoft.Xna.Framework.Graphics;

namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class 受潮弹Proj : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Blood Bullet");
           //DisplayName.AddTranslation(7, "血猩弹");
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 90;
            Projectile.aiStyle = 1;
            AIType = 14;
            Projectile.GetGlobalProjectile<RangedProjectile>().BulletProj = true;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            if (Projectile.ai[2] < Projectile.velocity.Length() * (Projectile.extraUpdates + 1))
            {
                Projectile.ai[2] += Projectile.velocity.Length() / 10 * (Projectile.extraUpdates + 1);
            }
            else
            {
                Projectile.ai[2] = Projectile.velocity.Length() * (Projectile.extraUpdates + 1);
            }
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = Projectile.damage;
            }
            if (Projectile.timeLeft < 2)
            {
                Projectile.timeLeft = 2;
                Projectile.damage = (int)Projectile.ai[1];

                Projectile.alpha += 30;
                if (Projectile.alpha >= 255 || Projectile.velocity.Length()<0.1F)
                {
                    Projectile.Kill();
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if(Projectile.DProj().track>70)
            {
                Projectile.velocity.Y += 0.1F;
            }
            if(Projectile.DProj().track>60)
            {
                Projectile.velocity *= 0.96F;
                Projectile.ai[1] *= 0.99f;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
            if (A > 45)
            {
                A = 45;
            }
            for (int a = 0; a < 5; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(186, 61, 1, 155), 1);
                Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                Main.dust[dust].noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.position = Projectile.oldPosition;
            float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
            if (A > 45)
            {
                A = 45;
            }
            for (int a = 0; a < 5; a++)
            {
                int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(186, 61, 1, 155), 1);
                Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                Main.dust[dust].noGravity = true;
            }
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //子弹
            float A = Projectile.ai[2];
            if (A > 90)
            {
                A = 90;
            }
            Texture2D texture = DDTextures.WhitePng.Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(186, 61, 1, 155)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(50, 50, 50, 155)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

            return false;
        }
    }
}