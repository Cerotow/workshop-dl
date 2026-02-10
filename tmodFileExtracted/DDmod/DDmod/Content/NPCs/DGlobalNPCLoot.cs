using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Armor;
using DDmod.Content.Items.Boss.Boss特殊;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Boss.MiniBoss.召唤物;
using DDmod.Content.Items.Boss.克苏鲁之眼;
using DDmod.Content.Items.Boss.特殊;
using DDmod.Content.Items.Magic.Book.NPCLoot;
using DDmod.Content.Items.Magic.Staff.Make;
using DDmod.Content.Items.Melee.FlyingKnife.Make;
using DDmod.Content.Items.Melee.FlyingKnife.NPCLoot;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Melee.Sword.NPCLoot;
using DDmod.Content.Items.Melee.SwordShield;
using DDmod.Content.Items.Pet;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Ranged.Make.Gun;
using DDmod.Content.Items.Ranged.NPCLoot;
using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Content.Items.Series.苦难;
using DDmod.Content.Items.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.NPCs
{
    public class LegacyHack_IsBossAndNotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (info.npc.boss)
                return !Main.expertMode;

            return false;
        }

        public bool CanShowItemDropInUI() => !Main.expertMode;
        public string GetConditionDescription() => Language.GetTextValue("Bestiary_ItemDropConditions.LegacyHack_IsBossAndNotExpert");
    }

    public class DGlobalNPCLoot : GlobalNPC
    {
        public override bool InstancePerEntity => true;


        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //陨石怪
            if (npc.type == NPCID.MeteorHead)
            {
                //npcLoot.Remove(ItemDropRule.Common(116, 50));
                npcLoot.RemoveWhere(rule => rule is CommonDrop normalDropRule && normalDropRule.itemId == 116);
                npcLoot.CompleteModeLoot(116, 4, 2, 1);
            }
            if (npc.type == 517)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<群星符日耀>()));
            if (npc.type == 422)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<群星符星旋>()));
            if (npc.type == 507)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<群星符星云>()));
            if (npc.type == 493)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<群星符星尘>()));

            //蛾怪
            if (npc.type == 477)
            {
                npcLoot.CompleteModeLoot(ModContent.ItemType<破损英雄飞刀>(), 8, 6, 4);
                npcLoot.CompleteModeLoot(ModContent.ItemType<英雄断弓>(), 8, 6, 4);
                npcLoot.CompleteModeLoot(ModContent.ItemType<英雄断杖>(), 8, 6, 4);
                npcLoot.CompleteModeLoot(ModContent.ItemType<英雄断戒>(), 8, 6, 4);
            }
            //肉山
            if (npc.type == 113)
            {
                npcLoot.SpecialLoot(ModContent.ItemType<血肉之舌>(), 1);
            }
            //机械骷髅王
            if (npc.type == 127)
            {
                npcLoot.SpecialLoot(ModContent.ItemType<机械激光加特林>(), 2);
            }
            //毁灭者
            if (npc.type == 134)
            {
                npcLoot.SpecialLoot(ModContent.ItemType<毁灭控制器>(), 2);
            }
            //双子魔眼
            if (npc.type == 125|| npc.type == 126)
            {
                npcLoot.SpecialLoot(ModContent.ItemType<机械魔眼剑盾>(), 2, true, [125,126]);
                npcLoot.SpecialLoot(ModContent.ItemType<咒焰瞬光>(), 1, true, [125,126]);
            }
            //世界吞噬者
            if (npc.type == 13|| npc.type == 14|| npc.type == 15)
            {
                npcLoot.SpecialLoot(ModContent.ItemType<蠕虫毒牙>(), 2,true);
            }
            //猪鲨
            if (npc.type == 370)
            {
                npcLoot.SpecialLoot(ModContent.ItemType<海爵之咬>(), 1);
            }
            //海盗船
            if (npc.type == 216 || npc.type == 491)
            {
                npcLoot.SpecialLoot(ModContent.ItemType<黄金大剑>(), 2);
            }
            //石巨人
            if (npc.type == 245)
            {
                npcLoot.SpecialLoot(ModContent.ItemType<石魔弓>(), 1);
                npcLoot.CompleteModeLoot(ModContent.ItemType<石像机器控制器>(), 4, 0, 0);
            }
            //蜂王
            if (npc.type == 222)
            {
                npcLoot.SpecialLoot(ModContent.ItemType<蜂巢剑盾>(), 2);
                npcLoot.SpecialLoot(ModContent.ItemType<蜜蜂加特林>(), 1);
            }
            //史莱姆王
            if (npc.type == 50)
            {
                npcLoot.SpecialLoot(ModContent.ItemType<史莱姆剑盾>(), 2);
                npcLoot.SpecialLoot(ModContent.ItemType<史莱姆链球>(), 1);
            }
            //克眼
            if (npc.type == 4)
            {
                npcLoot.CompleteModeLoot(ModContent.ItemType<眼球魔杖>(), 4, 0, 0);
                npcLoot.Add(new DropBasedOnCompleteMode(ItemDropRule.FewFromOptionsWithNumerator(1, 1, 1, ModContent.ItemType<眼球头>(), ModContent.ItemType<血嘴衣>(), ModContent.ItemType<眼肉裤>()), ItemDropRule.DropNothing(), ItemDropRule.DropNothing()));
                npcLoot.CompleteModeLoot(ModContent.ItemType<咬人的书>(), 4, 2, 1);
            }
            //哥布林盗贼
            if (npc.type ==73)
            {
                npcLoot.Add(ItemDropRule.Common(362, 1, 4, 8));
                npcLoot.CompleteModeLoot(362, 60, 40, 20);
            }
            //哥布林盗贼
            if (npc.type == 28)
            {
                npcLoot.CompleteModeLoot(ModContent.ItemType<GoblinDaggerItem>(), 60, 40, 20);
            }
            //哥布林战士
            if (npc.type == 28)
            {
                npcLoot.CompleteModeLoot(ModContent.ItemType<GoblinSword>(), 60, 40, 20);
            }
            //哥布林巫师
            if (npc.type == 29)
            {
                npcLoot.CompleteModeLoot(ModContent.ItemType<ShadowBookItem>(), 60, 40, 20);
            }
            //哥布林弓箭手
            if (npc.type == 111)
            {
                npcLoot.CompleteModeLoot(ModContent.ItemType<GoblinBow>(), 60, 40, 20);
            }
            //哥布林召唤师
            if (npc.type == NPCID.GoblinSummoner)
            {
                npcLoot.RemoveWhere(rule => rule is DropBasedOnExpertMode normalDropRule);
                //普通模式
                int[] A = new int[] { 3052, 3053, 3054, ModContent.ItemType<ShadowLantern>() };
                //ItemDropRule.OneFromOptions(1, 256, 257, 258);
                npcLoot.Add(ItemDropRule.NormalvsExpertOneFromOptions(1, 1, A));
            }
            //脸怪和奇美拉
            if (npc.type == 173 || npc.type == 181)
            {
                //普通模式
                int[] A = new int[] { ModContent.ItemType<远古血腥头盔>(), ModContent.ItemType<远古血腥胸甲>(), ModContent.ItemType<远古血腥护腿>() };
                npcLoot.Add(ItemDropRule.OneFromOptions(175, A));
                //npcLoot.CompleteModeLoot(ModContent.ItemType<远古血腥头盔>(), 526, 526, 526);
                //npcLoot.CompleteModeLoot(ModContent.ItemType<远古血腥胸甲>(), 526, 526, 526);
                //npcLoot.CompleteModeLoot(ModContent.ItemType<远古血腥护腿>(), 526, 526, 526);
                npcLoot.CompleteModeLoot(ModContent.ItemType<染血之眼>(), 620, 620, 620);
            }
            //噬魂怪
            if (npc.type == 6)
            {
                //普通模式
                npcLoot.CompleteModeLoot(ModContent.ItemType<噩梦降临>(), 620,620,620);
            }
            //哥布林弓箭手
            if (npc.type == 224)
            {
                npcLoot.CompleteModeLoot(ModContent.ItemType<飞鱼镖>(), 100, 50, 25);
            }
            //哥布林弓箭手
            if (npc.type == 490)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<苦难之魂>(), 50, 1, 1));
            }
            //骷髅
            if (npc.type is 21 or 449 or 201 or 450 or 202 or 451 or 203 or 452 or 322 or 323 or 324)
            {
                npcLoot.Add(ItemDropRule.Common(1922, 3, 1, 5));
            }
        }
    }
}