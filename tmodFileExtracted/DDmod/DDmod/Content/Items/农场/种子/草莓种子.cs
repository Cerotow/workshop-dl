using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.Tiles.农场;
using DDmod.Content.Items.农场.食物;

namespace DDmod.Content.Items.农场.种子
{
	public class 草莓种子 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.height = 2;
			Item.width = 2;
			Item.createTile = ModContent.TileType<草莓植株>();
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Green;
			Item.consumable = true;
			Item.value = Item.buyPrice(0, 0, 0, 50);
			Item.GetGlobalItem<FoodGlobalItem>().Seed = true;

        }

		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "Seed", Language.GetTextValue("Mods.DDmod.Tooltips.Farmland", ModContent.ItemType<草莓>(), Lang.GetItemNameValue(ModContent.ItemType<草莓>()))));
        }
        public override void AddRecipes()
		{
		}
	}
}