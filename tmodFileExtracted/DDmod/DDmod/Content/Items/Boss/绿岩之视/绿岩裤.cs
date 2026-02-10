using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DDmod.Content.Items.Series.绿岩;

namespace DDmod.Content.Items.Boss.绿岩之视
{
	[AutoloadEquip(EquipType.Legs)]
	public class 绿岩裤 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 50);
            Item.rare = 4;
            Item.defense = 3;
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩裤", out int GG);
            glowMask = GG;
            glowMaskColor = Color.White;
        }
        public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.18f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 12).AddIngredient(ModContent.ItemType<绿岩电池>(), 1).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
    }
}