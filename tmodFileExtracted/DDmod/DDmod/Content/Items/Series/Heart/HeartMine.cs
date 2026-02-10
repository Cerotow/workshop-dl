using DDmod.Content.Tiles.Mine;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.Heart
{
	public class HeartMine : ModItem
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
			Item.rare = ItemRarityID.Green;
			Item.consumable = true;
			Item.value = Item.buyPrice(0, 0, 0, 50);
			Item.createTile = ModContent.TileType<HeartMineTile>();
		}

		public override void SetStaticDefaults()
		{
		}
	}
}
