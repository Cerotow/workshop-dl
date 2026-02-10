using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Tiles.Mine;
using DDmod.Content.Tiles.绿岩;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Tiles.绿岩
{
	public class 绿岩 : ModItem
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
			Item.createTile = ModContent.TileType<绿岩Tile>();
		}

		public override void SetStaticDefaults()
		{
		}

		public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<SoulOfNature>(), 1).AddIngredient(3, 1).AddTile(TileID.LivingLoom).Register();
            CreateRecipe(4).AddIngredient(ModContent.ItemType<绿岩墙>(), 1).AddTile(TileID.WorkBenches).Register();
        }
	}
}
