using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Tiles.Mine;
using DDmod.Content.Tiles.绿岩;
using DDmod.Worlds;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Tiles.绿岩
{
	public class 绿岩格网块 : ModItem
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
			Item.createTile = ModContent.TileType<绿岩格网块Tile>();
		}

		public override void SetStaticDefaults()
		{
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            float height = DDWorld.GreenRockLab.Y + 61;

            tooltips.Add(new TooltipLine(Mod, "绿岩提示", Language.GetTextValue("Mods.DDmod.Tooltips.Placementhints", Language.GetTextValue("Mods.DDmod.NPCs.绿岩之视.DisplayName"), DDHelper.Deep(height)))
            {
                OverrideColor = new Color(100, 255, 100)
            });
        }

        public override void AddRecipes()
		{
            CreateRecipe(10).AddIngredient(ModContent.ItemType<绿岩电池>(), 1).AddIngredient(ModContent.ItemType<绿岩砖>(), 10).AddTile(TileID.WorkBenches).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩格网墙>(), 4).AddTile(TileID.WorkBenches).Register();
		}
	}
}