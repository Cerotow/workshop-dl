using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Content.Projectiles.GeneralProj;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 夜光蘑菇炸弹Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
            Projectile.timeLeft = 600;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X*0.03F;
            if(Projectile.velocity.X>0)
            {
                Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.03F;
            }
            else
            {
                Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.03F;
            }
            if (Projectile.ai[0] == 1)
            {
                Projectile.ai[0]++;
                Projectile.velocity.Y /= 2;
            }
            if (Projectile.velocity.Y < 10)
            {

                Projectile.velocity.Y += 0.1F;
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
            for (int A = 0; A < 30; A++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 20, 0f, 0f, 0, default(Color), 1f);
                if (Main.rand.NextBool(2))
                {
                    Main.dust[dust].scale = 0.1f;
                    Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.02f;
                }
            }
            for (int A = 0; A < 20; A++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<拉长粒子>(), 0f, 0f, 0, new Color(134, 172, 226, 0), 1.5f);
                if (Main.rand.NextBool(2))
                {
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.02f;
                }
                Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(2, 6), 0).RotatedBy(Main.rand.NextFloat(0,MathHelper.TwoPi));
            }
            if (Projectile.owner == Main.myPlayer)
            {
                for (int a = 0; a < 5; a++)
                {
                    int A = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(2, 6), 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), ModContent.ProjectileType<夜光蘑菇>(), Projectile.damage, 0.2f, Projectile.owner);
                    Main.projectile[A].DamageType = DamageClass.Ranged;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + texture.Size() / 2;
                Color color = new Color(34, 172, 226,0) *((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            Main.spriteBatch.Draw(texture, v, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            
            return false;
        }
    }
}