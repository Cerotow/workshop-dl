using DDmod.Content.Tiles.Mine;
using DDmod.Content.Tiles.绿岩;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.绿岩
{
	public class 绿岩晶石 : ModItem
	{
		public override void SetDefaults()
		{

			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 5;
			Item.useTime = 5;
			Item.useStyle = 1;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(0, 0, 0, 50); 

            Item.createTile = ModContent.TileType<绿岩晶块Tile>();

        }

		public override void SetStaticDefaults()
		{
		}
	}
}
