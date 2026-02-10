using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using DDmod.Players;
using System.Collections.ObjectModel;

namespace DDmod.Content.Items.Series.Venture.Level_1
{
	[AutoloadEquip(EquipType.Shield)]
	public class 木盾 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 1250;
			Item.rare = 2;
			Item.accessory = true;
			Item.defense = 1;
			Item.DItem().UpdatesRequired = true;
			AdventureGearGlobalItem.AccessoriesQualityAttribute(Item, new Random().Next(6), new Random().Next(6), 0, new Random().Next(2, 4), 0, 0);
		}

		public override void SetStaticDefaults()
		{
		}
        public override void UpdateInventory(Player player)
		{
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!hideVisual)
			{
				player.PlayerAction().ThereShield = true;
			}
			player.Aplayer().Resist += 10;
		}
		public override void UpdateVanity(Player player)
		{
			player.PlayerAction().ThereShield = true;
		}
	}
}
