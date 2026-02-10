using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Tiles.Mine;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.杂物
{
	public class 金毛鸟蛋 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = 3;
			Item.value = Item.buyPrice(0, 3, 0, 0);
			Item.useTime = Item.useAnimation = 20;
			Item.useStyle = 1;
            Item.consumable = true;
        }

		public override void SetStaticDefaults()
		{
		}
        public override bool CanUseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                for (int a = 0; a < player.Dplayer().Bpets.Length; a++)
                {
                    if (player.Dplayer().Bpets[a].Type == 0)
                    {
                        return true;
                    }
                }
            }
            return base.CanUseItem(player);
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                for (int a = 0; a < player.Dplayer().Bpets.Length; a++)
                {
                    if (player.Dplayer().Bpets[a].Type == 0)
                    {
                        player.Dplayer().Bpets[a] = new Players.BattlePets(Players.BattlePets.金毛幼鸟);
                        break;
                    }
                }
            }
            return true;
        }

    }
}
