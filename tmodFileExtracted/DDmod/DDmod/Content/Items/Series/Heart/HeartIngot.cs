using DDmod.Content.Tiles.Mine;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.Heart
{
	public class HeartIngot : ModItem
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
			Item.createTile = ModContent.TileType<HeartIngotTile>();
			Item.GetGlobalItem<StrengthenGlobalItem>().Enchantments(StrengthenGlobalItem.防御, 1, 3, 10, StrengthenGlobalItem.生命, 10, 20, 10);
		}

		public override void SetStaticDefaults()
		{
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<HeartMine>(),3).AddTile(TileID.Furnaces).Register();
		}
	}
}
