using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.绿岩;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩之视凹槽Tile : ModTile
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
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩之视凹槽", out int GG);
			Main.tileGlowMask[Type] = (short)GG;
            MineResist = 10;


            DustType = ModContent.DustType<绿岩粒子>();

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 16;
            TileObjectData.newTile.Height = 16;
            TileObjectData.newTile.CoordinateHeights = new[] { 16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16};
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Origin = new Point16(7, 15);

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());
        }
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num =3;
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
            int left = t.TileFrameX % (16 * 18) / 18;
            int top = t.TileFrameY % (16 * 18) / 18;
            if((left==0|| left==15) && (top==0|| top == 1|| top == 2|| top == 3|| top == 12|| top == 13|| top == 14|| top == 15))
            {
                return false;
            }
            if((left == 1 || left == 14) && (top==0|| top == 1|| top == 2|| top == 13|| top == 14|| top == 15))
            {
                return false;
            }
            if((left == 2 || left == 13) && (top==0|| top == 1|| top == 14|| top == 15))
            {
                return false;
            }
            if((left == 3 || left == 12) && (top==0|| top == 15))
            {
                return false;
            }
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }

    }
}
