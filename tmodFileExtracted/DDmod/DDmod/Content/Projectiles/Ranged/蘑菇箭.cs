using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.GeneralProj;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 蘑菇箭 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.arrow = true;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < 30; A++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height - 4, ModContent.DustType<蘑菇粒子>(), Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2), 0, default(Color), 1f);

            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            float Pi = 0;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                if (oldVelocity.X > 0)
                {
                    Pi = MathHelper.PiOver2;
                }
                if (oldVelocity.X < 0)
                {
                    Pi = -MathHelper.PiOver2;
                }
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                if (oldVelocity.Y > 0)
                {
                    Pi = MathHelper.Pi;
                }
                if (oldVelocity.Y < 0)
                {
                    Pi = 0;
                }
            }
            if (Projectile.owner == Main.myPlayer)
            {
                int A = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center+Collision.TileCollision(Projectile.position,Projectile.velocity,14,14), Vector2.Zero, ModContent.ProjectileType<蘑菇>(), Projectile.damage / 2, 0f, -1, 1, -1);
                Main.projectile[A].rotation = Pi;
            }
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner]; 
            NewProjectile(player.GetSource_FromAI(), Projectile.Center,Vector2.Zero , ModContent.ProjectileType<蘑菇>(), Projectile.damage/2, 0f, -1, 1,target.whoAmI);
            Projectile.ai[2]++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            texture.DrawCentre(Projectile, null, lightColor,Projectile.scale, 0);

            return false;
        }
    }
}