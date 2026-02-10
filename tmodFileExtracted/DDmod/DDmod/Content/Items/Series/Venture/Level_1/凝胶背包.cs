using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using DDmod.Players;
using System.Collections.ObjectModel;
using DDmod.UI.ItemUI.背包;

namespace DDmod.Content.Items.Series.Venture.Level_1
{
	public class 凝胶背包 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 200;
			Item.rare = 2;
			Item.GetGlobalItem<BackpackUIItem>().ContainedItems = 8;
		}

		public override void SetStaticDefaults()
		{
		}
        public override void UpdateInventory(Player player)
		{
		}
	}
}
