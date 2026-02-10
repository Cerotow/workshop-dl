using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using DDmod.Content.Dusts;

namespace DDmod.Content.Items.Series.GlowingMushroom
{
	[AutoloadEquip(EquipType.Head)]
	public class GlowingMushroomHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 1;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<GlowingMushroomShirt>() && legs.type == ModContent.ItemType<GlowingMushroomLeggings>();
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			DDSystem.Instance.DDEquipGlow.TryGetValue("蘑菇头", out int GG);
			glowMask = GG;
			glowMaskColor = Color.White;
		}
		public override void UpdateVanitySet(Player player)
		{

		}
		public override void UpdateEquip(Player player)
		{
			player.manaRegen += 5;
			Lighting.AddLight(player.Center - new Vector2(0, 20), new Color(95,99,255).ToVector3());
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.GlowingMushroomSet");
			player.statManaMax2 += 50;
			player.Aplayer().GlowingMushroomSet = true;

		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(183, 30).AddTile(18).Register();
		}
	}
}