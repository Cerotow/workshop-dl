using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DDmod.Content.Items.Boss.克苏鲁之眼
{
	[AutoloadEquip(EquipType.Legs)]
	public class 眼肉裤 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 1;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.1f;
		}
	}
}