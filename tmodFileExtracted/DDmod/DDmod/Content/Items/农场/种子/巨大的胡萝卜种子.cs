using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.Tiles.农场;
using DDmod.Content.Items.Melee.Sword;

namespace DDmod.Content.Items.农场.种子
{
	public class 巨大的胡萝卜种子 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.height = 2;
			Item.width = 2;
			Item.createTile = ModContent.TileType<超级胡萝卜Tile>();
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Green;
			Item.consumable = true;
			Item.value = Item.buyPrice(0, 0, 5, 0);
            Item.GetGlobalItem<FoodGlobalItem>().Seed = true;
        }

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "Seed", Language.GetTextValue("Mods.DDmod.Tooltips.Farmland", ModContent.ItemType<超级胡萝卜>(), Lang.GetItemNameValue(ModContent.ItemType<超级胡萝卜>()))));
        }
        public override void AddRecipes()
		{
		}
	}
}