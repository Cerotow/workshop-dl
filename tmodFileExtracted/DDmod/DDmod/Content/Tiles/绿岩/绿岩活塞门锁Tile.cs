using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Content.Tiles.杂物块;
using FullSerializer.Internal;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩活塞门锁Tile : ModTile
	{
		public const int NextStyleHeight = 38;

		public override void SetStaticDefaults()
		{
            DDGlobalTile.TopInvincible[Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;
			TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileHammeringIfOnTopOfIt[Type] = true;
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩活塞门锁", out int GG);
			Main.tileGlowMask[Type] = (short)GG;


			DustType = ModContent.DustType<绿岩粒子>();

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new[] { 16,16,16};
            //TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            //TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorTop = new AnchorData();
            TileObjectData.newTile.AnchorBottom = new AnchorData();
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.DrawYOffset = 0;
            TileObjectData.newTile.Origin = new Point16(0, 2);

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(0, 255, 0), CreateMapEntryName());
        }
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num =3;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
        public override void HitWire(int i, int j)
        {
            Tile Wiredens = Main.tile[DDSystem.Wiredens];
            if (Wiredens.TileType == ModContent.TileType<绿岩门禁Tile>())
            {
                Tile tile = Main.tile[i, j];
                int left = i - (int)tile.TileFrameX % (1 * 18) / 18;
                int top = j - (int)tile.TileFrameY % (3 * 18) / 18;

                for (int a = 0; a < 3; a++)
                {
                    tile = Main.tile[left, top+a];
                    tile.TileType = (ushort)ModContent.TileType<绿岩活塞门关Tile>();
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, left, top, 1, 3);
                }

                if (Wiring.running)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        Wiring.SkipWire(left, top + n);
                    }
                }
            }
        }

        public override bool RightClick(int i, int j)
        {
            return false;
        }
    }
}
