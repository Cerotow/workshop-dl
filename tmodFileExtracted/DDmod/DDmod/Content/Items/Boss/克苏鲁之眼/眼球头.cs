using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using DDmod.Content.Dusts;

namespace DDmod.Content.Items.Boss.克苏鲁之眼
{
	[AutoloadEquip(EquipType.Head)]
	public class 眼球头 : ModItem
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
			Item.defense = 3;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<血嘴衣>() && legs.type == ModContent.ItemType<眼肉裤>();
		}
		public override void UpdateVanitySet(Player player)
		{

		}
		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Generic) += 5;
		}
		public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.DDmod.ItemArmorSet.眼球套");
			if(player.statLife>player.statLifeMax2/2)
			{
				player.statDefense += 5;
			}
			else
			{
                player.statDefense -= 5;
				player.GetCritChance(DamageClass.Generic) += 10;
				player.GetDamage(DamageClass.Generic) += 0.1F;
				player.moveSpeed += 0.1f;
            }
        }
	}
}