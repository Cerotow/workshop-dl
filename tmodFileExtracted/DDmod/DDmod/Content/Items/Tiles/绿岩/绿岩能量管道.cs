using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Tiles.Mine;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Tiles.花岗岩基地;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Tiles.绿岩
{
	public class 绿岩能量管道 : ModItem
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
			Item.value = Item.buyPrice(0, 0, 0, 3);
			Item.createTile = ModContent.TileType<绿岩能量管道Tile>();
		}

		public override void SetStaticDefaults()
		{
		}

		public override void AddRecipes()
        {
            CreateRecipe(10).AddIngredient(530, 1).AddIngredient(ModContent.ItemType<绿岩电池>(), 1).AddIngredient(ModContent.ItemType<绿岩砖>(), 10).Register();
        }
	}
}
