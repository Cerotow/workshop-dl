using DDmod.Content.Dusts;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩警告牌Tile : ModTile
	{
		public const int NextStyleHeight = 38;

		public override void SetStaticDefaults()
		{
			Main.tileTable[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;


			DustType = ModContent.DustType<绿岩粒子>();

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 6;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Origin = new Point16(1, 5);

			TileObjectData.addTile(Type);

			AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 3;
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			return base.CreateDust(i, j, ref type);
		}

	}
}