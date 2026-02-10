using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using DDmod.Content.Dusts;

namespace DDmod.Content.Items.Series.Star
{
	[AutoloadEquip(EquipType.Head)]
	public class MagicStarHelmet : ModItem
	{
		public override void SetStaticDefaults()
        {
        }
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<MagicStarBreastplate>() && legs.type == ModContent.ItemType<MagicStarShoes>();
		}
		int A;
		public override void UpdateVanitySet(Player player)
        {
            if (player.velocity.Length() > 0.5F&& player.velocity.Y==0)
			{
				A++;
				if(A%5==0)
				{
					Dust dust = Main.dust[NewDust(player.position+new Vector2(0, player.height*0.8f), player.width, 1, ModContent.DustType<星星粒子>())];
					dust.velocity =Vector2.Zero;
					dust.scale = 0.5f;
					dust.noGravity = true;
					dust.noLight = true;
					dust.shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
				}
			}
		}

		public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.07f;
			player.GetDamage(DamageClass.Summon) += 0.07f;
			player.maxMinions += 1;
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.MagicStar");
			int A = player.Aplayer().ManaMax / 200;
			player.maxMinions += A;
			player.statManaMax2 += A*25;

		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<StarIngot>(), 10).AddTile(TileID.Anvils).Register();
		}
	}
}