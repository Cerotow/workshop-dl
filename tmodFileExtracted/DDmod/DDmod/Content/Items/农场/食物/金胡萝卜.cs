using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace DDmod.Content.Items.农场.食物
{
	public class 金胡萝卜 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 30;
			Item.rare = ItemRarityID.Orange;
			Item.consumable = true;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item2;
			Item.value = Item.buyPrice(0, 10, 0, 0);
		}

		public override void SetStaticDefaults()
		{
		}
	}
}