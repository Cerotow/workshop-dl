
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 飞鱼镖Proj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;

        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter%6==0)
            {
                Projectile.frame++;
                Projectile.frame%=4;
            }
            if(Projectile.velocity.Length()<2)
            {
                Projectile.velocity = Projectile.velocity.PerfectNormalize() * 2;
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != 2)
            {
                for (int i = 0; i < 30; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 4.2F);
                    Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, 5)];
                    dust.velocity = projDirection;
                    dust.noGravity = true;
                    dust.alpha = 100;
                    dust.scale = 1.8f;
                }
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity, 312, Projectile.scale);
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity, 313, Projectile.scale);
                PlaySound(SoundID.NPCDeath1, Projectile.Center);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y*0.8F;
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.8F;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Color color = lightColor;
            float SP = Projectile.velocity.Y*0.3F;
            if(SP>0.1F)
            {
                SP = 0.1F;
            }    
            if(SP<-0.1F)
            {
                SP = -0.1F;
            }    
            if (Projectile.velocity.X > 0)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation+ SP, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation- SP, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)1, 0f);
            }
            return false;
        }
    }
}
