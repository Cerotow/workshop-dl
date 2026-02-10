using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class Boss夜光蘑菇 : ModProjectile
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
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation += 0.1f;
            if (Projectile.ai[0] == 1)
            {
                Projectile.ai[0]++;
                Projectile.velocity.Y /= 2;
            }
            if (Projectile.ai[0] == 2)
            {
                if (Projectile.velocity.Y < 5)
                {
                    Projectile.velocity.Y += 0.05F;

                }
            }
            else
            {
                if (Projectile.velocity.Y < 10)
                {

                    Projectile.velocity.Y += 0.1F;
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
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + texture.Size() / 2;
                Color color = new Color(34, 172, 226,0) *((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            Main.spriteBatch.Draw(texture, v, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            if (Projectile.ai[0] == 2)
            {
                Main.spriteBatch.Draw(texture, v, null, new Color(34, 172, 226, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            
            return false;
        }
    }
    public class Boss蘑菇弹 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 1;
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                int A = NewDust(Projectile.Center - new Vector2(4), 0, 0, 20, Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1), Scale: 1.8F);
                Main.dust[A].noGravity = true;
                Projectile.ai[1] += 0.05f;
                Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                if (Projectile.ai[1] < 4)
                {
                    Vector2 vector = (player.Center - Projectile.Center).PerfectNormalize() * Projectile.ai[1];
                    Projectile.velocity = (Projectile.velocity * 20 + vector) / (21);
                }
            }
            else if (Projectile.ai[0] == 1)
            {
                if(Projectile.DProj().vector[0]==Vector2.Zero)
                {
                    Projectile.DProj().vector[0] = Projectile.velocity;
                }
                int A = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<激光粒子>(),0, 0, newColor: new Color(74, 189, 226, 0), Scale: Projectile.scale/4);
                Main.dust[A].noGravity = true;
                Main.dust[A].velocity = Vector2.Zero;
                Main.dust[A].rotation = Projectile.velocity.ToRotation();
                DDHelper.BackAndForth(1F, 10, 0.5F, ref Projectile.ai[1], ref Projectile.DProj().Bool[0]);
                Projectile.scale = (10-Projectile.ai[1])/3+1;
                Projectile.ProjScaleChange();
                Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize()* Projectile.ai[1];
            }
            else if (Projectile.ai[0] == 2)
            {
                if(Projectile.rotation==0)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
                Projectile.DProj().vector[0] = Projectile.rotation.ToRotationVector2();
                Projectile.rotation += Main.rand.NextFloat(-0.2F, 0.2F);
                int A = NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<激光粒子>(),0, 0, newColor: new Color(74, 189, 226, 0), Scale: Projectile.scale/4);
                Main.dust[A].noGravity = true;
                Main.dust[A].velocity = Vector2.Zero;
                Main.dust[A].rotation = Projectile.velocity.ToRotation();
                DDHelper.BackAndForth(1F, 10, 0.5F, ref Projectile.ai[1], ref Projectile.DProj().Bool[0]);
                Projectile.scale = (10-Projectile.ai[1])/3+1;
                Projectile.ProjScaleChange();
                Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize()* Projectile.ai[1];
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
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}