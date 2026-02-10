using DDmod.Content.Dusts;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩故障机器人Tile : ModTile
	{
		public const int NextStyleHeight = 38;

		public override void SetStaticDefaults()
        {
            Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;
            if(DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩故障机器人", out int GG))
			Main.tileGlowMask[Type] = (short)GG;

			DustType = ModContent.DustType<绿岩粒子>();

            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = new[] { 16,16,16,16};
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Origin = new Point16(3, 3);

            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.newAlternate.Origin = new Point16(0, 3);
            TileObjectData.addAlternate(1);

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return true;
        }
        public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
        {
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 3;
		}
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i,j)*16,16,16,ModContent.DustType<绿岩电光粒子>(),Main.rand.NextFloat(-4,4), Main.rand.NextFloat(-4, 4),Scale:Main.rand.NextFloat(0.75F,1.25F));
            return base.CreateDust(i, j, ref type);
        }

    }
}
