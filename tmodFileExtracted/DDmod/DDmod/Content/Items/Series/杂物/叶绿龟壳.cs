using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Tiles.Mine;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.杂物
{
	public class 叶绿龟壳 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = 7;
			Item.value = Item.buyPrice(0, 0, 10, 0);
		}

		public override void SetStaticDefaults()
		{
		}

		public override void AddRecipes()
		{
            CreateRecipe(1).AddIngredient(1328, 1).AddIngredient(1006, 20).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
