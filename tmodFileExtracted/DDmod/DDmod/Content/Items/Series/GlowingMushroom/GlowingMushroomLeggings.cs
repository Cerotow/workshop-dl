using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DDmod.Content.Items.Series.GlowingMushroom
{
	[AutoloadEquip(EquipType.Legs)]
	public class GlowingMushroomLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 1;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			/*
			DDSystem.Instance.DDEquipGlow.TryGetValue("蘑菇腿", out int GG);
			glowMask = GG;
			glowMaskColor = Color.White * 0.5f;
		*/}
		public override void UpdateEquip(Player player)
		{
			player.manaRegen += 5;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(183, 20).AddTile(18).Register();
		}
	}
}