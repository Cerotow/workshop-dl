using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using DDmod.Players;
using System.Collections.ObjectModel;
using DDmod.UI.ItemUI.背包;
using DDmod.UI.抽奖UI;

namespace DDmod.Content.Items.Series.Venture.Level_1
{
    public class 抽奖测试 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 200;
            Item.rare = 2;
            Item.maxStack = Item.CommonMaxStack;
        }

        public override void SetStaticDefaults()
        {
           //DisplayName.AddTranslation(7, "抽奖测试");
        }
        public override void UpdateInventory(Player player)
        {
        }
        public override bool ConsumeItem(Player player)
        {
            if (抽奖UI.Visible)
            {
                return false;
            }
            for (int a = 0;a<Main.item.Length;a++)
            {

            }
            Dictionary<int, Item[]> Loot = new Dictionary<int, Item[]>();
            //白
            Loot.Add(1, new Item[] { new Item(11), new Item(12), new Item(13), new Item(14), new Item(15, 40) });
            //蓝
            Loot.Add(2, new Item[] { new Item(111, 40), new Item(112), new Item(113), new Item(114, 40), new Item(115) });
            //紫
            Loot.Add(3, new Item[] { new Item(121), new Item(122, 40), new Item(123), new Item(124), new Item(125) });
            //橙
            Loot.Add(4, new Item[] { new Item(131, 40), new Item(132), new Item(133), new Item(134), new Item(135) });
            //红
            Loot.Add(5, new Item[] { new Item(141), new Item(142), new Item(143), new Item(144, 40), new Item(145) });
            int A = Item.stack;
            if (A > 10)
            {
                A = 10;
            }
            抽奖UI.instance.Item = new Item[A];
            抽奖UI.instance.Quality = new int[A];
            for (int a = 0; a < A; a++)
            {
                //随机一个品质
                int r = Main.rand.Next(5) + 1;
                抽奖UI.Loot(A, a, Loot[r][Main.rand.Next(Loot[r].Length)], r);
            }
            抽奖UI.Visible = true;
            Item.stack -= A;
            return false;
        }
        public override bool CanRightClick()
        {

            return true;
        }
    }
}
