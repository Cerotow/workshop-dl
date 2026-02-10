using DDmod.Content.Tiles.Mine;
using DDmod.Content.Tiles.流星;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Tiles.流星
{
	public class 流星模块主机 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = 1;
			Item.rare = ItemRarityID.Orange;
			Item.consumable = true;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.createTile = ModContent.TileType<流星模块主机Tile>();
		}

		public override void SetStaticDefaults()
		{
		}
	}
}
