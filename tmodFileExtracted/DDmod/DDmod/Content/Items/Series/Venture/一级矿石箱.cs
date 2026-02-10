using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using DDmod.Players;
using System.Collections.ObjectModel;
using DDmod.UI.ItemUI.背包;
using DDmod.UI.抽奖UI;
using DDmod.Content.Items.Talisman;

namespace DDmod.Content.Items.Series.Venture
{
    public class 一级矿石箱 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 200;
            Item.rare = 2;
            Item.maxStack = Item.CommonMaxStack;
            Item.GetGlobalItem<BackpackUIItem>().Loot.Clear();
            Item.GetGlobalItem<BackpackUIItem>().LootProbability = new int[]{30,24,20,10,2 };

            Item.GetGlobalItem<BackpackUIItem>().Loot.Add(1, new Item[] { new Item(12,80), new Item(699,80), new Item(424,20), new Item(1103,20), new Item(3081,40), new Item(3086,40) });
            
            Item.GetGlobalItem<BackpackUIItem>().Loot.Add(2, new Item[] { new Item(12, 200), new Item(699, 200), new Item(11,80), new Item(700,80),new Item(424,50), new Item(1103,50), new Item(3347,20), });
            
            Item.GetGlobalItem<BackpackUIItem>().Loot.Add(3, new Item[] { new Item(11, 200), new Item(700, 200), new Item(14,100), new Item(701,100), new Item(13,20), new Item(702,20), new Item(3347, 100) });
            
            Item.GetGlobalItem<BackpackUIItem>().Loot.Add(4, new Item[] { new Item(14, 200), new Item(701, 200),new Item(13, 100), new Item(702, 100), new Item(3347, 300) });

            Item.GetGlobalItem<BackpackUIItem>().Loot.Add(5, new Item[] { new Item(ModContent.ItemType<GhostFireLanternItem>())});
        }

        public override void SetStaticDefaults()
        {
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
                int r = Item.GetGlobalItem<BackpackUIItem>().RandNext(Item.GetGlobalItem<BackpackUIItem>().LootProbability) +1;
                抽奖UI.Loot(A, a, Item.GetGlobalItem<BackpackUIItem>().Loot[r][Main.rand.Next(Item.GetGlobalItem<BackpackUIItem>().Loot[r].Length)], r);
            }
            抽奖UI.Visible = true;
            Item.stack -= A;
            int s = Item.stack;
            if (s > 0)
            {
                Item.SetDefaults(Item.type);
                Item.stack = s;
            }
            else
            {
                Item.SetDefaults(0);

            }
            return false;
        }
        public override bool CanRightClick()
        {

            return true;
        }
    }
}
