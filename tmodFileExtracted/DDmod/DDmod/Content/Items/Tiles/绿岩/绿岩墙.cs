using DDmod.Content.Tiles.Mine;
using DDmod.Content.Tiles.绿岩;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Tiles.绿岩
{
	public class 绿岩墙 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 5;
			Item.useTime = 5;
			Item.useStyle = 1;
			Item.rare = ItemRarityID.Orange;
			Item.consumable = true;
			Item.value = Item.buyPrice(0, 0, 0, 0);
			Item.createWall = ModContent.WallType<绿岩墙Tile>();
		}

		public override void SetStaticDefaults()
		{
		}

		public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient(ModContent.ItemType<绿岩>(), 1).AddTile(TileID.WorkBenches).Register();
        }
	}
}
