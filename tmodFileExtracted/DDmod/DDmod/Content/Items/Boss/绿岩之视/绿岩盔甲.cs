using DDmod.Content.Items.Series.绿岩;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Boss.绿岩之视
{
	[AutoloadEquip(EquipType.Body)]
	public class 绿岩盔甲 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
            Item.value = Item.buyPrice(0, 0, 50);
            Item.rare = 4;
            Item.defense = 6;
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩盔甲", out int GG);
            glowMask = GG;
            glowMaskColor = Color.White;
        }
        public override void ArmorArmGlowMask(Player drawPlayer, float shadow, ref int glowMask, ref Color color)
        {
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩盔甲", out int GG);
            glowMask = GG;
            color = Color.White;
        }
        public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic)+=0.12F;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 24).AddIngredient(ModContent.ItemType<绿岩电池>(), 5).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
    }
}