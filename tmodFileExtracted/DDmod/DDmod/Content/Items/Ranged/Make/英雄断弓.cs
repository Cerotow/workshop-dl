using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Ranged.Make
{
	public class 英雄断弓 : ModItem
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