using DDmod.Content.Items.Melee.Sword;
using Microsoft.Xna.Framework.Graphics;

namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class 猩红弹Proj : ModProjectile
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
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
            AIType = 14;
            Projectile.GetGlobalProjectile<RangedProjectile>().BulletProj = true;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                if (Projectile.ai[2] < Projectile.velocity.Length() * (Projectile.extraUpdates + 1))
                {
                    Projectile.ai[2] += Projectile.velocity.Length() / 10 * (Projectile.extraUpdates + 1);
                }
                else
                {
                    Projectile.ai[2] = Projectile.velocity.Length() * (Projectile.extraUpdates + 1);
                }
            }
            else
            {
                Projectile.ai[2] += Projectile.velocity.Length();
                if (Projectile.ai[2] > 50)
                {
                    for (int a = 0; a < 1; a++)
                    {
                        int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(165, 37, 62, 255),3F);
                        Main.dust[dust].velocity = Vector2.Zero;
                        Main.dust[dust].rotation = Projectile.velocity.ToRotation();
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].customData = 2;
                    }
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            float lifeStoled = damageDone * 0.1f;
            if (lifeStoled < 1)
            {
                return;
            }
            if ((int)lifeStoled > 0 && !player.moonLeech && target.type != NPCID.TargetDummy && target.lifeMax > 2 && (Main.rand.NextBool(15) || Projectile.ai[1] != 0))
            {
                if (Projectile.ai[1] != 0)
                {
                    lifeStoled *= 1.75F;
                    NewProjectile(Projectile.GetSource_FromThis(), target.Center, -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)), ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, lifeStoled);
                    NewProjectile(Projectile.GetSource_FromThis(), target.Center, -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)), ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, lifeStoled);
                }
                NewProjectile(Projectile.GetSource_FromThis(), target.Center, -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)), ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, lifeStoled);
            }
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.oldPosition;
            float A = Projectile.oldVelocity.Length() * 0.75F * (Projectile.extraUpdates + 1);
            if (A > 45)
            {
                A = 45;
            }
            if (Projectile.ai[1] == 0)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(165, 37, 62, 255), 1);
                    Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            else
            {

                for (int a = 0; a < 12; a++)
                {
                    int dust = NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(165, 37, 62, 255), Main.rand.NextFloat(2f, 3f));
                    Main.dust[dust].velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0F, 0.5f) * A;
                    Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                    Main.dust[dust].noGravity = true;
                }
            }
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
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(100, 0, 0, 255)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2, null, Projectile.GetAlpha(new Color(255, 0, 0, 255)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

            return false;
        }
    }
}