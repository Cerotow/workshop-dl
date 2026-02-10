using DDmod.Content.Dusts;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
	public class ElixirFurnaceTile : ModTile
	{

		public override void SetStaticDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

			DustType = 0;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 7;
            TileObjectData.newTile.Height = 7;
            TileObjectData.newTile.CoordinateHeights = new[] { 16,16,16,16,16,16,16,};
            TileObjectData.newTile.DrawYOffset = 6;
            TileObjectData.newTile.Origin = new Point16(3, 6);

            TileObjectData.addTile(Type);

			//ModTranslationname = CreateMapEntryName();
			//name.SetDefault("Solidified crystal bathtub");
			//name.AddTranslation(7,"晶凝浴缸");
			AddMapEntry(new Color(200, 20, 20), CreateMapEntryName());
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

	}
}
