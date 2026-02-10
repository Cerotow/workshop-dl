using System;
using DDmod.Content.Items.Series.钢;
using DDmod.Content.Projectiles.Ranged.Gun;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.Venture.奖励袋.特别奖励
{
	public class 白切精华 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 38;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
        }

		public override void SetStaticDefaults()
		{
		}
        public override void AddRecipes()
        {
        }
    }
}