using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using DDmod.Content.Dusts;

namespace DDmod.Content.Items.Series.王朝木
{
	[AutoloadEquip(EquipType.Head)]
	public class 王朝木头盔 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 0, 20);
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<王朝木甲>() && legs.type == ModContent.ItemType<王朝木腿甲>();
		}
		public override void UpdateVanitySet(Player player)
		{

		}
		public override void UpdateEquip(Player player)
		{
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.王朝木套");
			player.statDefense += 2;

		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.DynastyWood, 20).AddTile(TileID.WorkBenches).Register();
		}
	}
}