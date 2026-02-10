using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Worlds;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩吞噬者Tile : ModTile
	{
		public const int NextStyleHeight = 38;

		public override void SetStaticDefaults()
		{
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩吞噬者", out int GG);
            Main.tileGlowMask[Type] = (short)GG;
            MineResist = 10;

            DustType = ModContent.DustType<绿岩粒子>();

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 13;
            TileObjectData.newTile.Height = 15;
            TileObjectData.newTile.CoordinateHeights = new[] { 16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,};
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.Origin = new Point16(12, 14);

            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.newAlternate.Origin = new Point16(0, 14);
            TileObjectData.addAlternate(1);

            TileObjectData.addTile(Type);

            //ModTranslationname = CreateMapEntryName();W
            //name.SetDefault("Solidified crystal bathtub");
            //name.AddTranslation(7,"晶凝浴缸");
            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());
        }
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 3;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
            {
                if (Main.rand.NextBool(2))
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, j * 16 + 8), ModContent.ItemType<绿岩砖>(), Main.rand.Next(4, 8));
                }
                else
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, j * 16 + 8), ModContent.ItemType<绿岩格网块>(), Main.rand.Next(4, 8));

                }
            }
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            Tile t = Main.tile[i, j];
            int left = t.TileFrameX % (13 * 18) / 18;
            int top = t.TileFrameY % (15 * 18) / 18;
            if ((top >= 3 && top <= 12)&&(left==0|| left == 1||left==11|| left == 12))
            {
                return false;
            }
            if ((top >= 4 && top <= 12)&&(left==2|| left == 10))
            {
                return false;
            }
            if ((top >= 5 && top <= 12)&&(left==3|| left ==9))
            {
                return false;
            }
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
        public override bool Slope(int i, int j)
        {
            return base.Slope(i, j);
        }
    }
}
