using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.Tiles.农场;
using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.家园塔;
using DDmod.Content.Tiles.农场.果树;

namespace DDmod.Content.Items.农场.种子
{
	public class 果树种子 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.height = 2;
			Item.width = 2;
			//Item.createTile = ModContent.TileType<荧光果植株>();
			Item.createTile = ModContent.TileType<森林果树>();
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Green;
			Item.consumable = true;
			Item.value = Item.buyPrice(0, 0, 0, 50);
		}

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
		}
        public override void HoldItem(Player player)
        {
            Item.createTile = ModContent.TileType<森林果树>();
            for (int a = 0; a < 5; a++)
            {
                Tile tile = Main.tile[(int)player.Dplayer().MouseWorld.X / 16, (int)player.Dplayer().MouseWorld.Y / 16 + a];
                if (tile.HasTile && DDHelper.SolidTile(tile,true,true))
                {
                    if (tile.TileType == 23 || tile.TileType == 661)
                    {
                        Item.createTile = ModContent.TileType<腐败果树>();
                    }
                    if (tile.TileType == 199 || tile.TileType == 662)
                    {
                        Item.createTile = ModContent.TileType<猩红果树>();
                    }
                    if (tile.TileType == 60)
                    {
                        Item.createTile = ModContent.TileType<丛林果树>();
                    }
                    if (tile.TileType == 53)
                    {
                        Item.createTile = ModContent.TileType<沙滩果树>();
                    }
                    if (tile.TileType == 109)
                    {
                        Item.createTile = ModContent.TileType<神圣果树>();
                    }
                    if (tile.TileType == 147)
                    {
                        Item.createTile = ModContent.TileType<雪地果树>();
                    }
                    if (tile.TileType == 633)
                    {
                        Item.createTile = ModContent.TileType<地狱果树>();
                    }
                    break;
                }
            }
        }
        public override bool CanUseItem(Player player)
        {
            /*
            Item.createTile = ModContent.TileType<森林果树>();
            for (int a = 0; a < 5; a++)
            {
                Tile tile = Main.tile[(int)player.Dplayer().MouseWorld.X / 16, (int)player.Dplayer().MouseWorld.Y / 16 + a];
                if (tile.HasTile)
                {
                    if (tile.TileType == 23 || tile.TileType == 661)
                    {
                        Item.createTile = ModContent.TileType<腐败果树>();
                    }
                    if (tile.TileType == 199 || tile.TileType == 662)
                    {
                        Item.createTile = ModContent.TileType<猩红果树>();
                    }
                    if (tile.TileType == 60)
                    {
                        Item.createTile = ModContent.TileType<丛林果树>();
                    }
                    if (tile.TileType == 53)
                    {
                        Item.createTile = ModContent.TileType<沙滩果树>();
                    }
                    if (tile.TileType == 109)
                    {
                        Item.createTile = ModContent.TileType<神圣果树>();
                    }
                    if (tile.TileType == 147)
                    {
                        Item.createTile = ModContent.TileType<雪地果树>();
                    }
                    if (tile.TileType == 633)
                    {
                        Item.createTile = ModContent.TileType<地狱果树>();
                    }
                    break;
                }
            }*/
            return base.CanUseItem(player);
        }
        public override void AddRecipes()
		{
		}
	}
}