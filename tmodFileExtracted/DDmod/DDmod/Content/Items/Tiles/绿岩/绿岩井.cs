using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.Trophy;
using DDmod.Content.Tiles.晶凝;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Tiles.绿岩.家具;
using Terraria.ID;

namespace DDmod.Content.Items.Tiles.绿岩
{
    public class 绿岩井 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 5, 0);
            Item.createTile = ModContent.TileType<绿岩井关Tile>();
            Item.placeStyle = 0;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override bool? UseItem(Player player)
        {
            int i = (int)(player.Dplayer().MouseWorld.X/16);
            int j = (int)(player.Dplayer().MouseWorld.Y/16);
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
            return base.UseItem(player);
        }
        public override void HoldItem(Player player)
        {
            player.InfoAccMechShowWires = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩砖>(), 15).AddIngredient(ModContent.ItemType<绿岩电池>(), 1).AddTile(TileID.Anvils).Register();
        }
    }
}