using DDmod.Content.Particles;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Boss
{
    public class BossStar2 : ModProjectile
    {
        public float TelegraphDelay
        {
            get
            {
                return Projectile.ai[0];
            }
            set
            {
                Projectile.ai[0] = value;
            }
        }
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation += 0.1f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
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
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D textureGlow = DDTextures.VoidStar.Value;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + texture.Size() / 2;
                Color color = Utils.MultiplyRGBA(new Color(255, 255, 255, 0), new Color(0, 150, 255)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                Main.spriteBatch.Draw(textureGlow, vector2, null, Utils.MultiplyRGBA(new Color(255, 255, 255, 0), new Color(0, 100, 255)) * 0.4f, 1, textureGlow.Size() / 2, 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, v, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);

            Main.spriteBatch.Draw(textureGlow, v, null, Utils.MultiplyRGBA(new Color(255, 255, 255, 0), new Color(0, 100, 255)) * 0.4f, 1, textureGlow.Size() / 2, 0.6f, SpriteEffects.None, 0);
            return false;
        }
    }
}