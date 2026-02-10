using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using DDmod.Players;
using System.Collections.ObjectModel;
using DDmod.UI.ItemUI.背包;
using DDmod.UI.抽奖UI;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Items.Series.Venture.奖励袋.特别奖励;
using DDmod.Content.Items.Boss.海幽浮王;
using DDmod.Content.Items.Sundries;

namespace DDmod.Content.Items.Series.Venture.奖励袋
{
    public class 奖励袋Lv5 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 200;
            Item.rare = 7;
            Item.maxStack = Item.CommonMaxStack;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void UpdateInventory(Player player)
        {
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<蓝色委托币>(), 1, 6, 12));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<完美重铸币>(), 1, 3, 6));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StrengtheningStone2>(), 1, 1, 2));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StrengtheningStone3>(), 4, 1, 2));
            itemLoot.Add(ItemDropRule.Common(499, 1, 3, 5));
        }
    }
}
