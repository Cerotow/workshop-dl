using DDmod.Content.Items.Boss.LifeGuardItems;
using Terraria.Enums;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.Relic
{
    public class LifeGuardRelicTile : Relic
    {
        public override RelicType RelicType => RelicType.PreHardmode;
        public override string RelicTextureName2 => "DDmod/Content/Tiles/Relic/LifeGuardRelicTile2";
        public override Vector2 OffsetPosition => new Vector2(0f, -60f);

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 offScreen = new Vector2(Main.offScreenRange);
            Texture2D texture2 = RelicTexture2.Value;
            Point p = new Point(i, j);
            Color color = Lighting.GetColor(p.X, p.Y);
            const float TwoPi = (float)Math.PI * 2f;
            float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * TwoPi / 5f);
            int eyeCount = 5;
            for (int T = 0; T < eyeCount; T++)
            {
                if (Main.tile[i, j].TileFrameX == 0 && (Main.tile[i, j].TileFrameY == 0 || Main.tile[i, j].TileFrameY == 72))
                {
                    Vector2 rotatedPos = (Vector2.UnitY * Sc).RotatedBy(T / (float)eyeCount * MathHelper.TwoPi + R) + new Vector2(5, 2f);
                    Vector2 drawPos = (new Vector2(i * 16, j * 16) + offScreen) - Main.screenPosition + new Vector2(20f, 0f) + rotatedPos + new Vector2(0f, offset * 4f);
                    Main.EntitySpriteDraw(texture2, drawPos, null, color, rotatedPos.ToRotation() + MathHelper.PiOver2, texture2.Size() / 2, 0.8f, SpriteEffects.None, 0);
                }
            }
        }
    }
}