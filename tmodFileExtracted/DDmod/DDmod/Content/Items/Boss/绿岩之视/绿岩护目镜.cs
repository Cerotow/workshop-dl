using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.绿岩;

namespace DDmod.Content.Items.Boss.绿岩之视
{
	[AutoloadEquip(EquipType.Head)]
	public class 绿岩护目镜 : ModItem
	{
		public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        }
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 50);
            Item.rare = 4;
            Item.defense = 1;
        }
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<绿岩盔甲>() && legs.type == ModContent.ItemType<绿岩裤>();
        }
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩护目镜", out int GG);
            glowMask = GG;
            glowMaskColor = Color.White;
        }
		public override void UpdateVanitySet(Player player)
        {
            if (player.velocity.Length() > 1)
            {
                int a=  NewDust(player.position, player.width, player.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(180,255,180),Main.rand.NextFloat(1F,2F));
                Main.dust[a].velocity = -player.velocity/3;
                Main.dust[a].rotation = player.velocity.ToRotation();
            }
		}
		public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
        }
		public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.召唤绿岩套");
            player.maxMinions++;
            player.GetDamage(DamageClass.Summon) += 0.1F;
            player.whipRangeMultiplier += 0.1f;
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.1f;
			player.Aplayer().绿岩套 = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 16).AddIngredient(ModContent.ItemType<绿岩电池>(), 3).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
    }
}