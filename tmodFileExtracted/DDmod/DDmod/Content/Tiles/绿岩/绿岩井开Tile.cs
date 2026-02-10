using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.UI.PlaystationUI;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;
using Terraria.ID;
using DDmod.Content.Tiles.杂物块;

namespace DDmod.Content.Tiles.绿岩
{
    public class 绿岩井开Tile : ModTile
    {
        public const int NextStyleHeight = 38;

        public override void SetStaticDefaults()
        {
            DDGlobalTile.LeftInvincible[Type] = true;
            DDGlobalTile.RightInvincible[Type] = true;

            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.IgnoredByNpcStepUp[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            RegisterItemDrop(ModContent.ItemType<绿岩井>(), 0);


            DustType = ModContent.DustType<绿岩粒子>();

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.AnchorBottom = new AnchorData();
            TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Height, 0);
            TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Height, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.DrawYOffset = 0;
            TileObjectData.newTile.Origin = new Point16(3, 1);

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(0, 255, 0), CreateMapEntryName());
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 3;
        }
        public override void HitWire(int i, int j)
        {
            Tile Wiredens = Main.tile[DDSystem.Wiredens];
            if (Wiredens.TileType == ModContent.TileType<门禁按钮Tile>()&&Wiredens.TileFrameY==0)
            {
                Tile tile = Main.tile[i, j];
                int left = i - (int)tile.TileFrameX % (6 * 18) / 18;
                int top = j - (int)tile.TileFrameY % (2 * 18) / 18;
                for (int a = 0; a < 6; a++)
                {
                    tile = Main.tile[left + a, top];
                    tile.TileType = (ushort)ModContent.TileType<绿岩井关Tile>();
                    if (a >= 1 && a <= 4)
                    {
                        tile.IsHalfBlock = true;
                    }
                    if (a == 0)
                    {
                        tile.Slope = SlopeType.SlopeDownLeft;
                    }
                    if (a == 5)
                    {
                        tile.Slope = SlopeType.SlopeDownRight;
                    }
                    tile = Main.tile[left + a, top + 1];
                    tile.TileType = (ushort)ModContent.TileType<绿岩井关Tile>();
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, left, top, 6, 2);
                }
                if (Wiring.running)
                {
                    for (int m = 0; m < 6; m++)
                    {
                        for (int n = 0; n < 2; n++)
                        {
                            Wiring.SkipWire(left + m, top + n);
                        }
                    }
                }
            }
        }
        public override bool Slope(int i, int j)
        {
            return false;
        }
        public override void MouseOver(int i, int j)
        {
        }
    }
}