using DDmod.Content.Tiles.Mine;
using DDmod.Content.Tiles.绿岩;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.钢
{
	public class 钢锭 : ModItem
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
			Item.value = Item.buyPrice(0, 0, 1, 50);
			Item.createTile = ModContent.TileType<钢锭Tile>();
			Item.GetGlobalItem<StrengthenGlobalItem>().Enchantments(StrengthenGlobalItem.防御, 3, 5, 10, StrengthenGlobalItem.减伤, 3, 5, 5);
		}

		public override void SetStaticDefaults()
		{
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddRecipeGroup(RecipeGroupID.IronBar,2).AddIngredient(1922, 2).AddTile(ModContent.TileType<绿岩电弧炉Tile>()).Register();
		}
	}
}
