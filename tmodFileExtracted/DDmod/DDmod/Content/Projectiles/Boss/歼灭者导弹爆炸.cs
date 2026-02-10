using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class 歼灭者导弹爆炸 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 0;
            Projectile.scale = 1F;
            Projectile.timeLeft = 3;
        }
        int A;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                for (int a = 0; a <20; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 0, default, 1.5f)];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, (Math.PI * 2 / 20) * a, default);
                    dust.velocity *= vector;
                }
                for (int a = 0; a < 8; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<冰雾>(), 0f, 0f, 0,new Color(100,100,100,155), Main.rand.NextFloat(0.6F,1.5F))];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, (Math.PI * 2 / 8) * a, default);
                    dust.velocity = vector*1.5F;
                    dust.alpha = -Main.rand.Next(500,1500);
                }
            }
            else
            {
                for (int a = 0; a < 20; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<绿激光粒子>(), 0f, 0f, 0, default, 0.8f)];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, (Math.PI * 2 / 20) * a, default);
                    dust.velocity *= vector;
                    dust.noGravity = true;
                }
                for (int a = 0; a < 8; a++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<冰雾>(), 0f, 0f, 0, new Color(10, 148, 10, 155), Main.rand.NextFloat(0.6F, 1.5F))];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, (Math.PI * 2 / 8) * a, default);
                    dust.velocity = vector * 1.5F;
                    dust.alpha = -Main.rand.Next(500, 1500);
                }

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
        }
    }
}