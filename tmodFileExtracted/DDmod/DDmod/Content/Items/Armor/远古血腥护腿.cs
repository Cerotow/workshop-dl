using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DDmod.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class 远古血腥护腿 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 6;
            Item.value = 50000;
            Item.rare = 1;
        }
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic)+=0.03F;
		}
	}
}