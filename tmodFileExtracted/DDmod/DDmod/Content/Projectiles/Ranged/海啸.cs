
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Ranged.Ammo;
using DDmod.DrawPlayer;
using DDmod.Players;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 海啸 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 162;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;

        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 8 == 0)
            {
                Projectile.frame++;
                Projectile.frame %= 6;
            }

            if (Projectile.ai[1] == 0)
            {
                if (Projectile.ai[0] < 240)
                {
                    Projectile.ai[0] += 2;
                }
            }
            else
            {
                if (Projectile.ai[0] < 300)
                {
                    Projectile.ai[0] += 3;
                }

            }
            for (int i = 0; i < 3; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(new Vector2(0, -1), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), default) * Main.rand.NextFloat(0.3F, 8.2F);
                Dust dust = Main.dust[NewDust(Projectile.position - new Vector2(-(int)(Projectile.width * 0.25F) + 4, -Projectile.height * 0.75F + 4), (int)(Projectile.width * 0.5F), 2, 33)];
                dust.velocity = projDirection;
                dust.noLightEmittence = false;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(0.8F, 1.8F);
            }
            Projectile.velocity *= 0.99F;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float height = 0;
            for (int a = 0; a < Projectile.ai[0] / 30; a++)
            {
                float Scale = 0.5F + a * 0.04F;
                Vector2 vector = Projectile.Center - new Vector2((int)(Projectile.width * Scale), (int)(Projectile.height * Scale)) / 2 - new Vector2(0, height);
                projHitbox = new Rectangle((int)vector.X, (int)vector.Y, (int)(Projectile.width * Scale), (int)(Projectile.height * Scale));
                if (projHitbox.Intersects(targetHitbox))
                {
                    return true;
                }
                height += Projectile.height * (Scale);
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Color color = lightColor;
            float height = 0;
            int R = (int)(Projectile.ai[0] / 30);
            for (int a = 0; a < Projectile.ai[0] / 30; a++)
            {

                float Scale = 0.5F + a * 0.04F;
                //vector = Projectile.Center- new Vector2((int)(Projectile.width * Scale), (int)(Projectile.height * Scale)) / 2 - new Vector2(0, height);
                if (R == a)
                {
                    color = lightColor * ((Projectile.ai[0] % 30) / 30F);
                    Main.spriteBatch.Draw(texture, Projectile.Center - new Vector2(0, height) - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * ((Projectile.frame + a) % 6), texture.Width, texture.Height / Main.projFrames[Projectile.type])), color * 0.6f, 0, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Scale, 0, 0f);
                }
                else
                {
                    if (Projectile.timeLeft < 50)
                    {
                        color = lightColor * (Projectile.timeLeft / 50F);
                    }
                    Main.spriteBatch.Draw(texture, Projectile.Center - new Vector2(0, height) - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * ((Projectile.frame + a) % 6), texture.Width, texture.Height / Main.projFrames[Projectile.type])), color * 0.6f, 0, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Scale, 0, 0f);

                }
                //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, vector - Main.screenPosition,null, Color.White*0.4F, 0, Vector2.Zero, new Vector2((int)(Projectile.width * Scale), (int)(Projectile.height * Scale))/2, 0, 0f);
                height += Projectile.height * (Scale);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
