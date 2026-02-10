using DDmod.Content.Particles;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Summon
{
    public class SummonStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation += 0.1f;

            Projectile.ProjScaleChange();
            Projectile.tileCollide = Projectile.Center.Y > Projectile.ai[0];
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < 6; a++)
            {
                DDParticle.RequestParticleSpawn(ParticleType.Star, new ParticleOrchestraSettings
                {
                    PositionInWorld = Projectile.Center,
                    MovementVector = Main.rand.NextVector2Unit() * 3
                });
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D textureGlow = DDTextures.VoidStar.Value;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size/2;
                Color color = Utils.MultiplyRGBA(new Color(255, 255, 255, 0), new Color(0, 150, 255)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                Main.spriteBatch.Draw(textureGlow, vector2, null, Utils.MultiplyRGBA(new Color(255, 255, 255, 0), new Color(0, 100, 255)) * 0.4f, 1, textureGlow.Size() / 2, Projectile.scale * 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, v, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);

            Main.spriteBatch.Draw(textureGlow, v, null, Utils.MultiplyRGBA(new Color(255, 255, 255, 0), new Color(0, 100, 255)) * 0.4f, 1, textureGlow.Size() / 2,Projectile.scale* 0.6f, SpriteEffects.None, 0);
            return false;
        }
    }
}