using DDmod.Content.Tiles.Mine;
using DDmod.Content.Tiles.绿岩;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.绿岩
{
	public class 绿岩故障机器人 : ModItem
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
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.createTile = ModContent.TileType<绿岩故障机器人Tile>();
		}

		public override void SetStaticDefaults()
		{
		}

		public override void AddRecipes()
		{
		}
	}
}
