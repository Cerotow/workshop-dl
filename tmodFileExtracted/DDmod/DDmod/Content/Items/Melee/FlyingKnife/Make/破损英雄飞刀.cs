using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
	public class 破损英雄飞刀 : ModItem
	{
		public override void SetDefaults()
		{

			Item.width = 30;
			Item.height = 24;
			Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 8;
        }

		public override void SetStaticDefaults()
		{
		}
	}
}