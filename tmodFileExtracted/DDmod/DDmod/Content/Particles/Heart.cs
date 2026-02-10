

using Terraria.GameContent.Drawing;
using Terraria.Graphics.Renderers;

namespace DDmod.Content.Particles
{
    public class HeartParticles : ABasicParticle
    {
        float Rot;
        int A = 0;
        float Sc;
        public static Asset<Texture2D> texture;
        public static Asset<Texture2D> Glow;

        public override void FetchFromPool()
        {
            base.FetchFromPool();
            Rot = Main.rand.NextFloat(0, MathHelper.TwoPi);
            A = 0;
            Sc = Main.rand.NextFloat(0.3f, 0.5f);
            if (texture == null)
            {
                texture = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/BossHeart");
            }
            if (Glow == null)
            {
                Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/爱心光效");
            }
        }
        public override void Update(ref ParticleRendererSettings settings)
        {
            base.Update(ref settings);
            Rot += 0.1f;
            A += 5;
            //Sc -= 0.02f;
            Vector2 value = settings.AnchorPosition + LocalPosition;
            if (A > 255 || Sc <= 0 || (value - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)).Length() > 1500)
            {
                ShouldBeRemovedFromRenderer = true;
            }
        }
        public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
        {
            Vector2 value = settings.AnchorPosition + LocalPosition;
            SpriteEffects spriteEffects = (SpriteEffects)1;
            Vector2 vector = new Vector2(texture.Width() / 2, texture.Height() / 2);

            spritebatch.Draw(Glow.Value, value, null, new Color(255, 0, 0, 0) * 0.5f, Rot, Glow.Size() / 2, Sc * 1.5f, spriteEffects, 0f);
            spritebatch.Draw(texture.Value, value, null, new Color(255 - A, 255 - A, 255 - A, 155 - A), Rot, vector, Sc, spriteEffects, 0f);

        }

        private static ParticlePool<HeartParticles> HeartPool = new(1, GetNewFlameParticle);
        private static HeartParticles GetNewFlameParticle()
        {
            return new HeartParticles();
        }
        public static void Heart(ParticleOrchestraSettings settings)
        {
            HeartParticles flameParticle = HeartPool.RequestParticle();
            flameParticle.LocalPosition = settings.PositionInWorld;
            flameParticle.Velocity = settings.MovementVector;
            Main.ParticleSystem_World_BehindPlayers.Add(flameParticle);

        }
    }
}