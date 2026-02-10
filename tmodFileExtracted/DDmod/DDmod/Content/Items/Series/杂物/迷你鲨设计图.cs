using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Tiles.Mine;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.杂物
{
	public class 迷你鲨设计图 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = 3;
			Item.value = Item.buyPrice(0, 20, 0, 0);
		}

		public override void SetStaticDefaults()
		{
		}

		public override void AddRecipes()
		{
		}
	}
}
