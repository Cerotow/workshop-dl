using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.GlowingMushroom
{
	[AutoloadEquip(EquipType.Body)]
	public class GlowingMushroomShirt : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 1;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			DDSystem.Instance.DDEquipGlow.TryGetValue("蘑菇衣", out int GG);
			glowMask = GG;
			glowMaskColor = Color.White;
		}
        public override void ArmorArmGlowMask(Player drawPlayer, float shadow, ref int glowMask, ref Color color)
		{
			DDSystem.Instance.DDEquipGlow.TryGetValue("蘑菇衣", out int GG);
			glowMask = GG;
			color = Color.White;
		}
        public override void UpdateEquip(Player player)
		{
			player.manaRegen += 5;
			Lighting.AddLight(player.Center, new Color(95, 99, 255).ToVector3() * 0.75F);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(183, 50).AddTile(18).Register();
		}
	}
}