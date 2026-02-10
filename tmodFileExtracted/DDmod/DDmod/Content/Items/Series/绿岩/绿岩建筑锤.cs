using DDmod.Content.Tiles.Mine;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Tiles.绿岩.家具;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.绿岩
{
    public class 绿岩建筑锤 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = 1;
            Item.rare = ItemRarityID.Orange;
            Item.consumable = false;
            Item.value = Item.buyPrice(0, 0, 0, 0);
        }

        public override void SetStaticDefaults()
        {
        }

        public override void AddRecipes()
        {
        }
        public override void HoldItem(Player player)
        {
            if (Item.createTile == ModContent.TileType<绿岩井锁Tile>() || Item.createTile == ModContent.TileType<绿岩门禁Tile>() ||
                Item.createTile == ModContent.TileType<绿岩胶化舱Tile>() || Item.createTile == ModContent.TileType<绿岩机器打印机Tile>() ||
                Item.createTile == ModContent.TileType<绿岩监控机床Tile>()|| Item.createTile == ModContent.TileType<绿岩活塞门锁Tile>()|| Item.createTile == ModContent.TileType<绿岩测验机Tile>())
                player.InfoAccMechShowWires = true;
        }
        public override bool? UseItem(Player player)
        {
            if (Item.createTile == ModContent.TileType<绿岩井锁Tile>())
            {
                int i = (int)(player.Dplayer().MouseWorld.X / 16);
                int j = (int)(player.Dplayer().MouseWorld.Y / 16);
                Tile tile = Main.tile[i, j];
                if (tile.TileType == Item.createTile)
                {
                    int left = i - (int)tile.TileFrameX % (6 * 16) / 16;
                    int top = j - (int)tile.TileFrameY % (2 * 16) / 16;
                    for (int a = 0; a < 6; a++)
                    {
                        tile = Main.tile[left + a, top];
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
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendTileSquare(player.whoAmI, left, top, 6, 2);
                    }
                }
            }
            return base.UseItem(player);
        }
        int A = -1;
        public override bool AltFunctionUse(Player player)
        {
            switch (A)
            {
                case 0:
                    Item.createTile = ModContent.TileType<绿岩吞噬者Tile>();
                    break;
                case 1:
                    Item.createTile = ModContent.TileType<绿岩之视凹槽Tile>();
                    break;
                case 2:
                    Item.createTile = ModContent.TileType<报废的绿岩机器人Tile>();
                    break;
                case 3:
                    Item.createTile = ModContent.TileType<绿岩锭预览图Tile>();
                    break;
                case 4:
                    Item.createTile = ModContent.TileType<绿岩培养皿Tile>();
                    break;
                case 5:
                    Item.createTile = ModContent.TileType<绿岩胶化舱Tile>();
                    break;
                case 6:
                    Item.createTile = ModContent.TileType<绿岩机器打印机Tile>();
                    break;
                case 7:
                    Item.createTile = ModContent.TileType<绿岩监控机床Tile>();
                    break;
                case 8:
                    Item.createTile = ModContent.TileType<绿岩炮台Tile>();
                    break;
                case 9:
                    Item.createTile = ModContent.TileType<上锁绿岩门Tile>();
                    break;
                case 10:
                    Item.createTile = ModContent.TileType<绿岩井锁Tile>();
                    break;
                case 11:
                    Item.createTile = ModContent.TileType<绿岩门禁Tile>();
                    break;
                case 12:
                    Item.createTile = ModContent.TileType<绿岩活塞门锁Tile>();
                    break;
                case 13:
                    Item.createTile = ModContent.TileType<绿岩测验机Tile>();
                    break;
                case 14:
                    Item.createTile = ModContent.TileType<绿岩电刺Tile>();
                    Main.NewText("绿岩电网");
                    break;
                case 15:
                    Item.createTile = ModContent.TileType<绿岩存储仓Tile>();
                    break;
                case 16:
                    Item.createTile = ModContent.TileType<绿岩干扰器Tile>();
                    break;
                default:
                    Item.createTile = ModContent.TileType<绿岩吞噬者Tile>();
                    A = 0;
                    break;

            }
            A++;
            //Item.createTile = ModContent.TileType<ElixirFurnaceTile>();
            return false;
        }
    }
}
