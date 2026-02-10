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
#pragma warning disable
    public static class 奖励袋
    {
        /// <summary>
        /// 药水
        /// </summary>
        /// <param name="Level">0地表,1地下,2洞穴,3原版药水全随机</param>
        public static void Medicament(ref List<Item> list, int Level = 0)
        {
            return;
            if (Level == 0)
            {
                //药水
                list.Add(Main.rand.Next(new Item[]{
                    new Item(28, Main.rand.Next(3, 8)),
                    new Item(2350, Main.rand.Next(3, 8)),
                    new Item(290, Main.rand.Next(2, 4)),
                    new Item(292, Main.rand.Next(2, 4)),
                    new Item(298, Main.rand.Next(2, 4)),
                    new Item(299, Main.rand.Next(2, 4)),
                    new Item(2322, Main.rand.Next(2, 4)),
                    new Item(2325, Main.rand.Next(2, 4)),
                }));
            }
            if (Level == 1)
            {
                //药水
                list.Add(Main.rand.Next(new Item[]{
                    new Item(296, Main.rand.Next(3, 8)),
                    new Item(295, Main.rand.Next(3, 8)),
                    new Item(302, Main.rand.Next(3, 8)),
                    new Item(299, Main.rand.Next(2, 4)),
                    new Item(303, Main.rand.Next(2, 4)),
                    new Item(305, Main.rand.Next(2, 4)),
                    new Item(301, Main.rand.Next(2, 4)),
                    new Item(297, Main.rand.Next(2, 4)),
                    new Item(304, Main.rand.Next(2, 4)),
                    new Item(2329, Main.rand.Next(2, 4)),
                    new Item(2351, Main.rand.Next(1, 2)),
                }));
            }
            if (Level == 2)
            {
                //药水
                list.Add(Main.rand.Next(new Item[]{
                    new Item(288, Main.rand.Next(3, 8)),
                    new Item(293, Main.rand.Next(3, 8)),
                    new Item(294, Main.rand.Next(3, 8)),
                    new Item(295, Main.rand.Next(3, 8)),
                    new Item(296, Main.rand.Next(3, 8)),
                    new Item(297, Main.rand.Next(3, 8)),
                    new Item(300, Main.rand.Next(3, 8)),
                    new Item(301, Main.rand.Next(3, 8)),
                    new Item(302, Main.rand.Next(3, 8)),
                    new Item(304, Main.rand.Next(3, 8)),
                    new Item(305, Main.rand.Next(3, 8)),
                    new Item(2323, Main.rand.Next(3, 8)),
                    new Item(2345, Main.rand.Next(3, 8)),
                    new Item(2348, Main.rand.Next(3, 8)),
                }));
            }
            if (Level == 3)
            {
                //药水
                list.Add(Main.rand.Next(new Item[]{
                    new Item(288, Main.rand.Next(3, 8)),
                    new Item(289, Main.rand.Next(3, 8)),
                    new Item(290, Main.rand.Next(3, 8)),
                    new Item(291, Main.rand.Next(3, 8)),
                    new Item(292, Main.rand.Next(3, 8)),
                    new Item(293, Main.rand.Next(3, 8)),
                    new Item(294, Main.rand.Next(3, 8)),
                    new Item(295, Main.rand.Next(3, 8)),
                    new Item(296, Main.rand.Next(3, 8)),
                    new Item(297, Main.rand.Next(3, 8)),
                    new Item(298, Main.rand.Next(3, 8)),
                    new Item(299, Main.rand.Next(3, 8)),
                    new Item(300, Main.rand.Next(3, 8)),
                    new Item(301, Main.rand.Next(3, 8)),
                    new Item(302, Main.rand.Next(3, 8)),
                    new Item(304, Main.rand.Next(3, 8)),
                    new Item(305, Main.rand.Next(3, 8)),
                    new Item(2322, Main.rand.Next(3, 8)),
                    new Item(2323, Main.rand.Next(3, 8)),
                    new Item(2324, Main.rand.Next(3, 8)),
                    new Item(2325, Main.rand.Next(3, 8)),
                    new Item(2326, Main.rand.Next(3, 8)),
                    new Item(2327, Main.rand.Next(3, 8)),
                    new Item(2328, Main.rand.Next(3, 8)),
                    new Item(2329, Main.rand.Next(3, 8)),
                    new Item(2344, Main.rand.Next(3, 8)),
                    new Item(2345, Main.rand.Next(3, 8)),
                    new Item(2346, Main.rand.Next(3, 8)),
                    new Item(2347, Main.rand.Next(3, 8)),
                    new Item(2348, Main.rand.Next(3, 8)),
                    new Item(2349, Main.rand.Next(3, 8)),
                    new Item(2350, Main.rand.Next(3, 8)),
                    new Item(2351, Main.rand.Next(3, 8)),
                    new Item(2359, Main.rand.Next(3, 8)),
                    new Item(4870, Main.rand.Next(3, 8)),
                }));
            }
        }
        /// <summary>
         /// 强化石
         /// </summary>
         /// <param name="list"></param>
         /// <param name="NextBool">概率</param>
         /// <param name="Level">等级</param>
         /// <param name="MinStack">最小数量</param>
         /// <param name="MaxStack">最大数量</param>
        public static void StrengtheningStone(ref List<Item> list, int NextBool = 1, int Level = 1, int MinStack = 1, int MaxStack = 1)
        {
            return;
            if (Main.rand.NextBool(NextBool))
            {
                if (Level == 1)
                {
                    list.Add(new Item(ModContent.ItemType<StrengtheningStone>(), Main.rand.Next(MinStack, MaxStack + 1)));
                }
                else if (Level == 2)
                {
                    list.Add(new Item(ModContent.ItemType<StrengtheningStone2>(), Main.rand.Next(MinStack, MaxStack + 1)));
                }
                else if (Level == 3)
                {
                    list.Add(new Item(ModContent.ItemType<StrengtheningStone3>(), Main.rand.Next(MinStack, MaxStack + 1)));
                }
            }
        }
        /// <summary>
        /// 矿锭
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Level">0铜锡铁铅
        /// 1铁铅银钨
        /// 2银钨金铂金
        /// 3陨石锭
        /// 4新三锭
        /// 5叶绿锭</param>
        public static void Orebar(ref List<Item> list, int Level = 0, int Stack = 1)
        {
            return;
            if (Level == 0)
            {
                if (Main.rand.NextBool(2))
                {
                    if (WorldGen.SavedOreTiers.Copper == TileID.Copper)
                    {
                        list.Add(new Item(20, Stack));
                    }
                    else
                    {
                        list.Add(new Item(703, Stack));
                    }
                }
                else
                {
                    if (WorldGen.SavedOreTiers.Iron == TileID.Iron)
                    {
                        list.Add(new Item(22, Stack));
                    }
                    else
                    {
                        list.Add(new Item(704, Stack));
                    }
                }
            }
            if (Level == 1)
            {
                if (Main.rand.NextBool(2))
                {
                    if (WorldGen.SavedOreTiers.Iron == TileID.Iron)
                    {
                        list.Add(new Item(22, Stack));
                    }
                    else
                    {
                        list.Add(new Item(704, Stack));
                    }
                }
                else
                {
                    if (WorldGen.SavedOreTiers.Silver == TileID.Silver)
                    {
                        list.Add(new Item(21, Stack));
                    }
                    else
                    {
                        list.Add(new Item(705, Stack));
                    }
                }
            }
            if (Level == 2)
            {
                if (Main.rand.NextBool(2))
                {
                    if (WorldGen.SavedOreTiers.Silver == TileID.Silver)
                    {
                        list.Add(new Item(21, Stack));
                    }
                    else
                    {
                        list.Add(new Item(705, Stack));
                    }
                }
                else
                {
                    if (WorldGen.SavedOreTiers.Gold == TileID.Gold)
                    {
                        list.Add(new Item(19, Stack));
                    }
                    else
                    {
                        list.Add(new Item(706, Stack));
                    }
                }
            }
            if (Level == 3)
            {
                list.Add(new Item(116, Stack));
            }
            if (Level == 4)
            {
                int A = Main.rand.Next(3);
                if (A == 0)
                {
                    if (WorldGen.SavedOreTiers.Cobalt == TileID.Cobalt)
                    {
                        list.Add(new Item(381, Stack));
                    }
                    else
                    {
                        list.Add(new Item(1184, Stack));
                    }
                }
                if (A == 1)
                {
                    if (WorldGen.SavedOreTiers.Mythril == TileID.Mythril)
                    {
                        list.Add(new Item(382, Stack));
                    }
                    else
                    {
                        list.Add(new Item(1191, Stack));
                    }
                }
                if (A == 2)
                {
                    if (WorldGen.SavedOreTiers.Adamantite == TileID.Adamantite)
                    {
                        list.Add(new Item(391, Stack));
                    }
                    else
                    {
                        list.Add(new Item(1198, Stack));
                    }
                }
            }
            if (Level == 5)
            {
                list.Add(new Item(1006, Stack));
            }
        }
    }
}
