
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class Boss叶箭 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.ArmorPenetration = 15;

        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            if (!Projectile.DProj().Bool[0])
            {

                for (int i = 0; i < 10; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 4.2F);
                    Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 40)];
                    dust.velocity = projDirection;
                    dust.noGravity = true;
                    dust.alpha = 100;
                    dust.scale = 0.8f;
                }
                Projectile.DProj().Bool[0] = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ProjScaleChange();
            Projectile.frameCounter++;
            if (Projectile.frameCounter%4==0)
            {
                Projectile.frame++;
                Projectile.frame%=7;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 40)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.2f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Color color = lightColor;

            if (Projectile.ai[0] == 0)
            {
                if (Projectile.velocity.X + Projectile.Player().velocity.X > 0)
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation + MathHelper.Pi, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)1, 0f);
                }
            }
            else
            {
                if (Projectile.velocity.X > 0)
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation + MathHelper.Pi, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)1, 0f);
                }
            }
            return false;
        }
    }
}
