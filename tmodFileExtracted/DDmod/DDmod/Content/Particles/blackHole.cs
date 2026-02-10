

using Terraria.GameContent.Drawing;
using Terraria.Graphics.Renderers;

namespace DDmod.Content.Particles
{
    public class blackHoleParticles : ABasicParticle
    {
        float Rot;
        int A = 0;
        float Sc;
        public static Asset<Texture2D> Black;
        public override void FetchFromPool()
        {
            base.FetchFromPool();
            Rot = 0;
            A = 0;
            Sc = Main.rand.NextFloat(0.3f, 0.42f);
            if (Black == null)
            {
                Black = ModContent.Request<Texture2D>("DDmod/Content/NPCs/BlackHole");
            }
        }
        public override void Update(ref ParticleRendererSettings settings)
        {
            base.Update(ref settings);
            if (Rot < 1)
            {
                Rot += 0.05f;
            }
            A += Main.rand.Next(4, 8);
            Sc -= 0.008f;
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
            Texture2D texture = DDTextures.VoidStar.Value;
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 2);
            spritebatch.Draw(texture, value, null, new Color(255, 0, 0, 0)* Rot, Rot, texture.Size() / 2, Sc, spriteEffects, 0f);
            spritebatch.Draw(Black.Value, value, null, Color.White * Rot, Rot, Black.Size() / 2, Sc * 0.4F, spriteEffects, 0f);

        }

        private static ParticlePool<blackHoleParticles> BlackHolePool = new(1, GetNewFlameParticle);
        private static blackHoleParticles GetNewFlameParticle()
        {
            return new blackHoleParticles();
        }
        public static void BlackHole(ParticleOrchestraSettings settings)
        {
            blackHoleParticles flameParticle = BlackHolePool.RequestParticle();
            flameParticle.LocalPosition = settings.PositionInWorld;
            flameParticle.Velocity = settings.MovementVector;

            Main.ParticleSystem_World_BehindPlayers.Add(flameParticle);

        }
    }
}