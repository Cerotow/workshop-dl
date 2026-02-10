using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DDmod.Content.Items.Series.王朝木
{
	[AutoloadEquip(EquipType.Legs)]
	public class 王朝木腿甲 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 0, 0, 20);
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.DynastyWood, 25).AddTile(TileID.WorkBenches).Register();
		}
	}
}