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

namespace DDmod.Content.Items.Series.Venture.奖励袋
{
    public class 奖励袋Lv9 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 200;
            Item.rare = 11;
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
            for (int a = 0; a < Main.LocalPlayer.Dplayer().Bpets.Length; a++)
            {
                if (Main.LocalPlayer.Dplayer().Bpets[a].Type == 0)
                {
                    Main.LocalPlayer.Dplayer().Bpets[a] = new Players.BattlePets(2);
                    break;
                }
            }
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<海幽浮钓竿>()));
        }
    }
}
