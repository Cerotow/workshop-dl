using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.晶凝;
using DDmod.Content.Items.Tiles.花岗岩基地;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.花岗岩基地
{
    public class 花岗岩科技床Tile : DDBed
    {
        public override Color Color => new Color(100, 155, 255);
        public override int Icon => ModContent.ItemType<花岗岩科技床>();
        public override int Dust => ModContent.DustType<花岗岩电光粒子>();

		float a = 0;
		bool b = false;
		public override void ModifySleepingTargetInfo(int i, int j, ref TileRestingInfo info) {
            DDHelper.BackAndForth(16,24,0.2F,ref a,ref b);
        }

        public Asset<Texture2D> TileTexture;

        public string T => "DDmod/Content/Tiles/花岗岩基地/花岗岩科技床_Glow";

        public override void Load()
        {
            if (!Main.dedServ)
            {
                TileTexture = ModContent.Request<Texture2D>(T);
            }
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = (tile.TileFrameY == 36) ? 18 : 16;
            Main.spriteBatch.Draw(TileTexture.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
        }
    }
}
