using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using DDmod.Players;
using System.Collections.ObjectModel;
using DDmod.UI.ItemUI.背包;
using DDmod.UI.抽奖UI;
using DDmod.Content.Items.Talisman;

namespace DDmod.Content.Items.Series.Venture.任务奖励
{
    public class 蘑菇时装箱 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 200;
            Item.rare = 2;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void UpdateInventory(Player player)
        {
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(4779, 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(4780, 1, 1, 1));
            itemLoot.Add(ItemDropRule.Common(4781, 1, 1, 1));
        }
        public override bool CanRightClick()
        {

            return true;
        }
    }
}
