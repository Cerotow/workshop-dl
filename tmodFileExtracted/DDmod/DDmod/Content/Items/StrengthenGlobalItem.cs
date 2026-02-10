
using DDmod;
using DDmod.Content.Items;
using DDmod.Content.Items.Boss.Boss特殊;
using DDmod.Content.Items.Sundries;
using DDmod.Content.Prefixes;
using DDmod.NoContent.Config;
using Terraria.GameContent.Prefixes;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

public class StrengthenGlobalItem : GlobalItem
{
    public override bool InstancePerEntity => true;

    public int Level;
    /// <summary>
    /// 强化石
    /// </summary>
    public int[] Probability = new int[11];
    public const int 伤害 = 1;
    public const int 防御 = 2;
    public const int 生命 = 3;
    public const int 生命回复 = 4;
    public const int 魔力 = 5;
    public const int 魔力回复 = 6;
    public const int 攻击速度 = 7;
    public const int 移动速度 = 8;
    public const int 跳跃速度 = 9;
    public const int 飞翔时间 = 10;
    public const int 暴击 = 11;
    public const int 破甲 = 12;
    public const int 召唤栏 = 13;
    public const int 免疫火焰 = 14;
    public const int 挖掘速度 = 15;
    public const int 魔力消耗 = 16;
    public const int 给予火焰 = 17;
    public const int 减伤 = 18;
    public const int 哥布林减伤 = 19;
    public const int 不动伤害 = 20;
    public const int 不动防御 = 21;
    public const int 不动减伤 = 22;
    public const int 幽灵 = 23;
    public const int 免疫咒火 = 24;
    public const int 免疫灵液 = 25;
    public const int 近战大小 = 26;
    public const int 幸运 = 27;
    public void Probabilitys(int P1 = 0, int P2 = 0, int P3 = 0, int P4 = 0, int P5 = 0, int P6 = 0, int P7 = 0, int P8 = 0, int P9 = 0, int P10 = 0, int P11 = 0)
    {
        Probability[0] = P1;
        Probability[1] = P2;
        Probability[2] = P3;
        Probability[3] = P4;
        Probability[4] = P5;
        Probability[5] = P6;
        Probability[6] = P7;
        Probability[7] = P8;
        Probability[8] = P9;
        Probability[9] = P10;
        Probability[10] = P11;
    }
    ///<summary> 附魔类型 </summary>
    public int EnchantmentType;
    ///<summary> 附魔类型 </summary>
    public int Value;
    ///<summary> 附魔类型 </summary>
    public int[] EnchantmentItemType = new int[2];
    ///<summary> 最小值 </summary>
    public int[] MinE = new int[2];
    ///<summary> 最大值 </summary>
    public int[] MaxE = new int[2];
    ///<summary> 附魔概率 </summary>
    public int[] Enchantment = new int[2];
    public void Enchantments(int type = 0, int Min = 0, int Max = 0, int P1 = 0, int type2 = 0, int Min2 = 0, int Max2 = 0, int P2 = 0)
    {
        EnchantmentItemType[0] = type;
        Enchantment[0] = P1;
        MinE[0] = Min;
        MaxE[0] = Max + 1;
        EnchantmentItemType[1] = type2;
        Enchantment[1] = P2;
        MinE[1] = Min2;
        MaxE[1] = Max2 + 1;
    }
    public override void Load()
    {
        On_Item.AffixName += Item_Text;
        On_Item.Prefix += Prefix;
    }
    public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand)
    {
        return base.PrefixChance(item, pre, rand);
    }
    public override void SetDefaults(Item item)
    {
        //蠕虫毒牙
        if (item.type == 161)
        {
            PrefixLegacy.ItemSets.GunsBows[item.type] = true;
            item.AllowReforgeForStackableItem = true;

        }
        //蠕虫毒牙
        if (item.type == 69)
        {
            Enchantments(破甲, 1, 1, 10, 挖掘速度, 2, 6, 5);
        }
        //晶状体
        if (item.type == 38)
        {
            Enchantments(暴击, 1, 2, 5, 跳跃速度, 3, 6, 2);
        }
        //黑晶状体
        if (item.type == 236)
        {
            Enchantments(暴击, 2, 5, 20, 跳跃速度, 5, 10, 10);
        }
        //铜 / 锡锭
        if (item.type == 20 || item.type == 703)
        {
            Enchantments(伤害, 1, 1, 10, 防御, 1, 1, 10);
        }
        //铁 / 铅锭
        if (item.type == 22 || item.type == 704)
        {
            Enchantments(伤害, 1, 2, 10, 防御, 1, 2, 10);
        }
        //银 / 钨锭
        if (item.type == 21 || item.type == 705)
        {
            Enchantments(伤害, 1, 2, 15, 防御, 1, 2, 15);
        }
        //金 / 铂金锭
        if (item.type == 19 || item.type == 706)
        {
            Enchantments(伤害, 1, 3, 15, 防御, 1, 3, 15);
        }
        //生命水晶
        if (item.type == 29)
        {
            Enchantments(生命, 4, 10, 20, 生命回复, 1, 3, 10);
        }
        //魔力水晶
        if (item.type == 109)
        {
            Enchantments(魔力, 8, 20, 20, 魔力回复, 1, 3, 10);
        }
        //猩红锭
        if (item.type == 1257)
        {
            Enchantments(防御, 2, 3, 15, 生命回复, 1, 4, 5);
        }
        //魔金锭
        if (item.type == 57)
        {
            Enchantments(防御, 2, 3, 15, 移动速度, 2, 5, 5);
        }
        //腐肉
        if (item.type == 68)
        {
            Enchantments(生命, 2, 7, 10, 生命, 5, 12, 3);
        }
        //椎骨
        if (item.type == 1330)
        {
            Enchantments(防御, 1, 3, 10, 生命回复, 1, 2, 3);
        }
        //蚁狮牙
        if (item.type == 323)
        {
            Enchantments(破甲, 1, 2, 8, 挖掘速度, 2, 6, 12);
        }
        //毒刺
        if (item.type == 209)
        {
            Enchantments(破甲, 1, 2, 10, 伤害, 1, 2, 5);
        }
        //陨石锭
        if (item.type == 117)
        {
            Enchantments(防御, 2, 3, 20, 魔力消耗, 2, 5, 10);
        }
        //藤曼
        if (item.type == 210)
        {
            Enchantments(攻击速度, 1, 4, 10, 移动速度, 1, 4, 10);
        }
        //羽毛
        if (item.type == 320)
        {
            Enchantments(移动速度, 3, 6, 5, 飞翔时间, 5, 15, 10);
        }
        //狱石锭
        if (item.type == 175)
        {
            Enchantments(防御, 2, 4, 15, 给予火焰, -1, 0, 3);
        }
        //黑曜石
        if (item.type == 173)
        {
            Enchantments(减伤, 1, 2, 5, 免疫火焰, -1, 0, 3);
        }
        //鲨鱼鳍
        if (item.type == 319)
        {
            Enchantments(移动速度, 2, 8, 5, 伤害, 2, 4, 5);
        }
        //丛林孢子
        if (item.type == 331)
        {
            Enchantments(生命回复, 2, 3, 5, 魔力回复, 2, 3, 5);
        }
        //橡果
        if (item.type == 27)
        {
            Enchantments(生命, 1, 2, 15, 生命, 2, 4, 15);
        }
        //骨头
        if (item.type == 154)
        {
            Enchantments(生命, 3, 8, 10, 攻击速度, 2, 6, 8);
        }
        //破布
        if (item.type == 362)
        {
            Enchantments(防御, 1, 3, 3, 哥布林减伤, 10, 20, 10);
        }
        //钴锭钯金锭
        if (item.type == 381 || item.type == 1184)
        {
            Enchantments(伤害, 2, 4, 10, 防御, 3, 5, 10);
        }
        //秘银锭山铜锭
        if (item.type == 382 || item.type == 1191)
        {
            Enchantments(伤害, 3, 4, 10, 防御, 4, 5, 10);
        }
        //精金锭钛金锭
        if (item.type == 391 || item.type == 1198)
        {
            Enchantments(伤害, 3, 5, 10, 防御, 4, 6, 10);
        }
        //叶绿锭
        if (item.type == 1006)
        {
            Enchantments(防御, 5, 8, 10, 生命回复, 3, 5, 4);
        }
        //神圣锭
        if (item.type == 1225)
        {
            Enchantments(防御, 4, 7, 20, 减伤, 4, 6, 8);
        }
        //蘑菇锭
        if (item.type == 1552)
        {
            Enchantments(防御, 6, 9, 10, 不动伤害, 5, 12, 8);
        }
        //龟壳
        if (item.type == 1328)
        {
            Enchantments(不动防御, 15, 25, 10, 不动减伤, 8, 16, 8);
        }
        //甲虫壳
        if (item.type == 2218)
        {
            Enchantments(防御, 8, 9, 10, 减伤, 5, 10, 8);
        }
        //幽灵锭
        if (item.type == 3261)
        {
            Enchantments(防御, 6, 9, 10, 幽灵, -1, 0, 6);
        }
        //远古皮布
        if (item.type == 3794)
        {
            Enchantments(防御, 3, 6, 5, 减伤, 3, 7, 5);
        }
        //丝绸
        if (item.type == 225)
        {
            Enchantments(防御, 1, 3, 5, 减伤, 1, 3, 5);
        }
        //精灵尘
        if (item.type == 501)
        {
            Enchantments(生命, 1, 12, 15, 魔力, 1, 24, 15);
        }
        //独角兽角
        if (item.type == 526)
        {
            Enchantments(伤害, 1, 5, 10, 破甲, 1, 3, 10);
        }
        //黑暗碎片
        if (item.type == 527)
        {
            Enchantments(伤害, -7, 7, 10, 防御, -7, 7, 10);
        }
        //光明碎片
        if (item.type == 528)
        {
            Enchantments(生命, 4, 6, 10, 移动速度, 5, 14, 10);
        }
        //咒火
        if (item.type == 522)
        {
            Enchantments(免疫咒火, -1, 0, 10, 伤害, 3, 5, 10);
        }
        //灵液
        if (item.type == 1332)
        {
            Enchantments(免疫灵液, -1, 0, 10, 破甲, 2, 4, 10);
        }
        //夜明锭
        if (item.type == 3467)
        {
            Enchantments(防御, 10, 10, 10, 近战大小, 6, 12, 10);
        }
        //蜘蛛牙
        if (item.type == 2607)
        {
            Enchantments(召唤栏, 1, 1, 4, 伤害, 1, 6, 10);
        }
        /*
        蠕虫毒牙：破甲1～3，伤害1 %～3 %
晶状体：暴击1～2 %
铜 / 锡锭：伤害1 %～2 %，防御1～2
铁 / 铅锭：伤害1 %～3 %，防御1～3
银 / 钨锭：伤害2 %～4 %，防御2～4
金 / 铂金锭：伤害3 %～5 %，防御3～5
魔金锭：防御3～7，移动速度2 %～5 %
 血猩锭：防御3～7，生命回复2～4
生命水晶：生命4～10，生命回复1～3
魔力水晶：魔力4～10，魔力回复1～3
陨石锭：魔力消耗2～5 %
腐肉：生命2～7，防御1～5
颈骨：防御2～4，生命回复1～2
蚁狮牙 / 毒刺：破甲1～4，伤害1 %～4 %
   小雪怪皮毛：召唤栏1～1，防御2～5
藤蔓：攻击速度1～4 %，移动速度1 %～4 %
  丛林孢子：生命回复2～3，魔力回复2～3
羽毛：移动速度3 %～6 %，飞行时间增加0.5～1.5秒
  狱石锭：免疫火焰，防御增加3～7
骨头：生命3～8，攻速2 %～6 %
 鲨鱼鳍：移动速度2 %～8 %，伤害3 %～7 %*/
    }
    int R114 = 0;
    public bool Prefix(On_Item.orig_Prefix orig, Item item, int P)
    {
        int R = item.maxStack;

        //item.maxStack = 1;
        bool o = orig(item, P);
        //item.maxStack = R;

        if (item.type == ModContent.ItemType<星辰炮>())
        {
            item.useAnimation = item.DItem().OriginaluseAnimation(item);
            item.useTime = item.DItem().OriginaluseTime(item);
        }
        if (P == -1 && item.OriginalDamage >= 10)
        {
            item.GetGlobalItem<StrengthenGlobalItem>().Level = Main.rand.Next(-DDConfigServer.Instance.MinStrengthen, DDConfigServer.Instance.MaxStrengthen + 1);
        }

        return o;
    }
    public static string Item_Text(On_Item.orig_AffixName orig, Item item)
    {
        string En = "";
        if (!Main.gameMenu && item.type > 0 && item.stack > 0 && item.GetGlobalItem<StrengthenGlobalItem>().EnchantmentType > 0)
        {
            En = Language.GetTextValue("Mods.DDmod.properties.融合") + " ";
        }
        string Level = "";
        if (!Main.gameMenu && item.type > 0 && item.stack > 0)
        {
            if (item.GetGlobalItem<StrengthenGlobalItem>().Level > 0)
            {
                Level += "+" + item.GetGlobalItem<StrengthenGlobalItem>().Level;
            }
            else if (item.GetGlobalItem<StrengthenGlobalItem>().Level < 0)
            {
                Level += item.GetGlobalItem<StrengthenGlobalItem>().Level;
            }
        }
        if (item.prefix < 0 || item.prefix >= Lang.prefix.Length)
            return En + item.Name + Level;

        string text = Lang.prefix[item.prefix].Value;
        if (text == "")
            return En + item.Name + Level;

        if (text.StartsWith("("))
            return En + item.Name + " " + text + Level;

        return En + text + " " + item.Name + Level;
    }
    public override void OnCreated(Item item, ItemCreationContext context)
    {
        Level = 0;
    }
    public override void OnSpawn(Item item, IEntitySource source)
    {
    }
    //重铸
    public override bool CanReforge(Item item)
    {

        return base.CanReforge(item);
    }
    public override void PostReforge(Item item)
    {
        int W = 0;
        for (int A = 0; A < Main.LocalPlayer.inventory.Length - 1; A++)
        {
            if (Main.LocalPlayer.inventory[A].type == ModContent.ItemType<重铸币>())
            {
                Main.LocalPlayer.inventory[A].stack--;
                W = 5;
                break;
            }
            if (Main.LocalPlayer.inventory[A].type == ModContent.ItemType<优秀重铸币>())
            {
                Main.LocalPlayer.inventory[A].stack--;
                W = 10;
                break;
            }
            if (Main.LocalPlayer.inventory[A].type == ModContent.ItemType<完美重铸币>())
            {
                Main.LocalPlayer.inventory[A].stack--;
                W = 15;
                break;
            }
        }
        if (Main.rand.Next(100) < W)
        {
            bool Acc = false;
            int Weapon = 0;
            for (int A = 0; A < item.GetPrefixCategories().Count; A++)
            {
                if (item.GetPrefixCategories()[A] == PrefixCategory.Accessory)
                {
                    Acc = true;
                    break;
                }
                if (item.GetPrefixCategories()[A] == PrefixCategory.Melee)
                {
                    Weapon = 1;
                    break;
                }
                if (item.GetPrefixCategories()[A] == PrefixCategory.Ranged)
                {
                    Weapon = 2;
                    break;
                }
                if (item.GetPrefixCategories()[A] == PrefixCategory.Magic)
                {
                    if (item.DItem().OriginalMana(item) > 3&&item.knockBack>0)
                    {
                        Weapon = 3;
                        break;
                    }
                }
            }
            if (!Acc)
            {
                item.prefix = ModContent.PrefixType<神级>();
                if (item.DamageType == DamageClass.Summon)
                {
                    item.prefix = ModContent.PrefixType<神级2>();
                }
                if (item.knockBack == 0)
                {
                    item.prefix = ModContent.PrefixType<恶魔>();
                }
                if (Main.rand.NextBool(2))
                {
                    if (Weapon == 1)
                    {
                        item.prefix = ModContent.PrefixType<传奇>();
                    }
                    else if (Weapon == 2)
                    {

                        item.prefix = ModContent.PrefixType<虚幻>();
                    }
                    else if (Weapon == 3)
                    {
                        item.prefix = ModContent.PrefixType<神话>();
                    }
                }
            }
            else
            {
                item.prefix = Main.rand.Next([ModContent.PrefixType<猛烈>(), ModContent.PrefixType<疯狂>(), ModContent.PrefixType<洞悉>(), ModContent.PrefixType<庇佑>(), ModContent.PrefixType<奔流>()]);

            }
        }
        Strengthen(item, true);
    }
    public override void PreReforge(Item item)
    {
        base.PreReforge(item);
    }
    public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
    {
        if (canApplyDiscount)
        {
            float F = 1;
            for (int A = 0; A < Main.LocalPlayer.inventory.Length - 1; A++)
            {
                if (Main.LocalPlayer.inventory[A].type == ModContent.ItemType<重铸币>())
                {
                    Main.LocalPlayer.noThrow = 2;
                    Main.LocalPlayer.cursorItemIconEnabled = true;
                    Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<重铸币>();
                    F -= 0.2F;
                    break;
                }
                if (Main.LocalPlayer.inventory[A].type == ModContent.ItemType<优秀重铸币>())
                {
                    Main.LocalPlayer.noThrow = 2;
                    Main.LocalPlayer.cursorItemIconEnabled = true;
                    Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<优秀重铸币>();
                    F -= 0.4F;
                    break;
                }
                if (Main.LocalPlayer.inventory[A].type == ModContent.ItemType<完美重铸币>())
                {
                    Main.LocalPlayer.noThrow = 2;
                    Main.LocalPlayer.cursorItemIconEnabled = true;
                    Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<完美重铸币>();
                    F -= 0.6F;
                    break;
                }
            }
            reforgePrice = (int)(reforgePrice * F);
        }

        return base.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
    }
    public override void UpdateEquip(Item item, Player player)
    {
        int T = EnchantmentType;
        if (T == StrengthenGlobalItem.伤害)
        {
            player.GetDamage(DamageClass.Generic) += (float)Value / 100;
        }
        if (T == StrengthenGlobalItem.防御)
        {
            player.statDefense += Value;
        }
        if (T == StrengthenGlobalItem.生命)
        {
            player.statLifeMax2 += Value;
        }
        if (T == StrengthenGlobalItem.生命回复)
        {
            player.lifeRegen += Value;
        }
        if (T == StrengthenGlobalItem.魔力)
        {
            player.statManaMax2 += Value;
        }
        if (T == StrengthenGlobalItem.魔力回复)
        {
            player.manaRegen += Value;
        }
        if (T == StrengthenGlobalItem.攻击速度)
        {
            player.GetAttackSpeed(DamageClass.Generic) += (float)Value / 100;
        }
        if (T == StrengthenGlobalItem.移动速度)
        {
            player.moveSpeed += (float)Value / 100;
        }
        if (T == StrengthenGlobalItem.跳跃速度)
        {
            player.jumpSpeedBoost += (float)Value / 100;
        }
        if (T == StrengthenGlobalItem.飞翔时间)
        {
            player.Dplayer().wingTimeMax += (float)Value / 100;
        }
        if (T == StrengthenGlobalItem.暴击)
        {
            player.GetCritChance(DamageClass.Generic) += Value;
        }
        if (T == StrengthenGlobalItem.破甲)
        {
            player.GetArmorPenetration(DamageClass.Generic) += Value;
        }
        if (T == StrengthenGlobalItem.召唤栏)
        {
            player.maxMinions += Value;
        }
        if (T == StrengthenGlobalItem.免疫火焰)
        {
            player.buffImmune[24] = true;
        }
        if (T == StrengthenGlobalItem.挖掘速度)
        {
            player.pickSpeed -= (float)Value / 100;
        }
        if (T == StrengthenGlobalItem.魔力消耗)
        {
            player.manaCost -= (float)Value / 100;
        }
        if (T == StrengthenGlobalItem.给予火焰)
        {
            player.Dplayer().HitBuff.Add(24);
        }
        if (T == StrengthenGlobalItem.减伤)
        {
            player.endurance += (float)Value / 100;
        }
        if (T == StrengthenGlobalItem.哥布林减伤)
        {
            player.Dplayer().GoblinsEndurance += (float)Value / 100;
        }
        if (player.velocity.Length() < 0.01F)
        {
            if (T == StrengthenGlobalItem.不动伤害)
            {
                player.GetDamage(DamageClass.Generic) += (float)Value / 100;
            }
            if (T == StrengthenGlobalItem.不动减伤)
            {
                player.Dplayer().GoblinsEndurance += (float)Value / 100;
            }
            if (T == StrengthenGlobalItem.不动防御)
            {
                player.statDefense += Value;
            }
        }
        if (T == StrengthenGlobalItem.幽灵)
        {
            player.Dplayer().phantom = true;
        }
        if (T == StrengthenGlobalItem.免疫咒火)
        {
            player.buffImmune[39] = true;
        }
        if (T == StrengthenGlobalItem.免疫灵液)
        {
            player.buffImmune[69] = true;
        }
        if (T == StrengthenGlobalItem.近战大小)
        {
            player.Dplayer().MeleeScale += (float)Value / 100;
        }
    }
    public void Strengthen(Item item, bool NoStrengthen)
    {
        item.damage = item.OriginalDamage;
        item.useAnimation = item.DItem().OriginaluseAnimation(item);
        item.useTime = item.DItem().OriginaluseTime(item);
        item.mana = item.DItem().OriginalMana(item);
        item.crit = item.DItem().OriginalCrit(item);
        item.scale = item.DItem().OriginalScale(item);
        item.shootSpeed = item.DItem().OriginalShootSpeed(item);
        item.knockBack = item.DItem().OriginalKnockBack(item);
        item.value = item.DItem().OriginalValue(item);
        item.rare = item.DItem().OriginalRare(item);
        item.Prefix(item.prefix);
        int damage = (int)(item.OriginalDamage * 0.05F);
        if (damage < 1)
        {
            damage = 1;
        }
        if (NoStrengthen)
        {
            //for (int a = 0; a < Level; a++)
            {
                item.damage += damage * Level;
            }
        }
        else
        {
            Level++;
            //for (int a = 0; a < Level; a++)
            {
                item.damage += damage * Level;
            }
        }
        int Damage = 0;

        if (damage < 1)
        {
            damage = 1;
        }
        //for (int a = 0; a < Level; a++)
        {
            Damage += damage * Level;
        }
        item.value = (int)(item.value * (1 + (Level * 0.25F)));
    }
    public override void LoadData(Item item, TagCompound tag)
    {
        Level = tag.Get<int>("Level");
        EnchantmentType = tag.Get<int>("EnchantmentType");
        Value = tag.Get<int>("Value");
        Strengthen(item, true);
    }
    public override void SaveData(Item item, TagCompound tag)
    {
        tag["Level"] = Level;
        tag["EnchantmentType"] = EnchantmentType;
        tag["Value"] = Value;
    }

    public override void NetSend(Item item, BinaryWriter writer)
    {
        writer.Write(Level);
        writer.Write(item.damage);
        writer.Write(EnchantmentType);
        writer.Write(Value);
    }
    public override void NetReceive(Item item, BinaryReader reader)
    {
        Level = reader.ReadInt32();
        item.damage = reader.ReadInt32();
        EnchantmentType = reader.ReadInt32();
        Value = reader.ReadInt32();
    }


    public int GetWeaponDamage(Item sItem, bool forTooltip = false)
    {
        StatModifier modifier = Main.LocalPlayer.GetTotalDamage(sItem.DamageType);

        if (AmmoID.Sets.IsArrow[sItem.useAmmo])
            modifier = modifier.CombineWith(Main.LocalPlayer.arrowDamage);

        if (AmmoID.Sets.IsBullet[sItem.useAmmo])
            modifier = modifier.CombineWith(Main.LocalPlayer.bulletDamage);

        if (AmmoID.Sets.IsSpecialist[sItem.useAmmo])
            modifier = modifier.CombineWith(Main.LocalPlayer.specialistDamage);

        CombinedHooks.ModifyWeaponDamage(Main.LocalPlayer, sItem, ref modifier);
        int baseDamage = forTooltip ? (int)(sItem.damage * ItemID.Sets.ToolTipDamageMultiplier[sItem.type]) : sItem.damage;
        return Math.Max(0, (int)(modifier.ApplyTo(baseDamage) + 5E-06f));
    }
    public static string EnchantmentText(int T, out bool B, out bool R)
    {
        B = false;
        R = false;
        if (T == StrengthenGlobalItem.伤害)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.伤害");
        }
        if (T == StrengthenGlobalItem.防御)
        {
            return Language.GetTextValue("Mods.DDmod.properties.防御");
        }
        if (T == StrengthenGlobalItem.生命)
        {
            return Language.GetTextValue("Mods.DDmod.properties.生命");
        }
        if (T == StrengthenGlobalItem.生命回复)
        {
            return Language.GetTextValue("Mods.DDmod.properties.生命回复");
        }
        if (T == StrengthenGlobalItem.魔力)
        {
            return Language.GetTextValue("Mods.DDmod.properties.魔力");
        }
        if (T == StrengthenGlobalItem.魔力回复)
        {
            return Language.GetTextValue("Mods.DDmod.properties.魔力回复");
        }
        if (T == StrengthenGlobalItem.攻击速度)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.攻击速度");
        }
        if (T == StrengthenGlobalItem.移动速度)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.移动速度");
        }
        if (T == StrengthenGlobalItem.跳跃速度)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.跳跃速度");
        }
        if (T == StrengthenGlobalItem.飞翔时间)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.飞翔时间");
        }
        if (T == StrengthenGlobalItem.暴击)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.暴击");
        }
        if (T == StrengthenGlobalItem.破甲)
        {
            return Language.GetTextValue("Mods.DDmod.properties.破甲");
        }
        if (T == StrengthenGlobalItem.召唤栏)
        {
            return Language.GetTextValue("Mods.DDmod.properties.召唤栏");
        }
        if (T == StrengthenGlobalItem.免疫火焰)
        {
            return Language.GetTextValue("Mods.DDmod.properties.免疫") + "'" + Language.GetTextValue("BuffName.OnFire") + "'";
        }
        if (T == StrengthenGlobalItem.挖掘速度)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.挖掘速度");
        }
        if (T == StrengthenGlobalItem.魔力消耗)
        {
            B = true;
            R = true;
            return Language.GetTextValue("Mods.DDmod.properties.魔力消耗");
        }
        if (T == StrengthenGlobalItem.给予火焰)
        {
            return Language.GetTextValue("Mods.DDmod.properties.受到伤害给予敌人", Language.GetTextValue("BuffName.OnFire"));
        }
        if (T == StrengthenGlobalItem.减伤)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.减伤");
        }
        if (T == StrengthenGlobalItem.哥布林减伤)
        {
            B = true;
            R = true;
            return Language.GetTextValue("Mods.DDmod.properties.哥布林减伤");
        }
        if (T == StrengthenGlobalItem.不动伤害)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.不动", Language.GetTextValue("Mods.DDmod.properties.伤害"));
        }
        if (T == StrengthenGlobalItem.不动减伤)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.不动", Language.GetTextValue("Mods.DDmod.properties.减伤"));
        }
        if (T == StrengthenGlobalItem.不动防御)
        {
            return Language.GetTextValue("Mods.DDmod.properties.不动", Language.GetTextValue("Mods.DDmod.properties.防御"));
        }
        if (T == StrengthenGlobalItem.幽灵)
        {
            return Language.GetTextValue("Mods.DDmod.properties.幽灵");
        }
        if (T == StrengthenGlobalItem.免疫咒火)
        {
            return Language.GetTextValue("Mods.DDmod.properties.免疫") + "'" + Language.GetTextValue("BuffName.CursedInferno") + "'";
        }
        if (T == StrengthenGlobalItem.免疫灵液)
        {
            return Language.GetTextValue("Mods.DDmod.properties.免疫") + "'" + Language.GetTextValue("BuffName.Ichor") + "'";
        }
        if (T == StrengthenGlobalItem.近战大小)
        {
            B = true;
            return Language.GetTextValue("Mods.DDmod.properties.大小");
        }
        return "";
    }
    public string Text(byte type)
    {
        return "\n" + StrengthenGlobalItem.EnchantmentText(EnchantmentItemType[type], out bool B, out bool R) + ((MaxE[type] - 1 == 0) ? "" : (R ? " -" : " +")) + "(" + ((MaxE[type] - 1 == 0) ? Language.GetTextValue("Mods.DDmod.properties.唯一") : ((MaxE[type] - 1 == MinE[type] && false) ? (MinE[type] + (B ? "%" : "")) : (MinE[type] + (B ? "%" : "") + "~" + (MaxE[type] - 1) + (B ? "%" : "")))) + ")";
    }
    public string NewText()
    {
        return EnchantmentText(EnchantmentType, out bool B, out bool R) + ((Value == 0) ? "(" + Language.GetTextValue("Mods.DDmod.properties.唯一") + ")" : ((R ? " -" : " +") + Value)) + (B ? "%" : "");
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (!item.social && EnchantmentType > 0)
        {
            if (Value >= 0)
            {
                tooltips.Add(new TooltipLine(Mod, "装备前缀", EnchantmentText(EnchantmentType, out bool B, out bool R) + ((Value == 0) ? "(" + Language.GetTextValue("Mods.DDmod.properties.唯一") + ")" : ((R ? " -" : " +") + Value)) + (B ? "%" : ""))
                {
                    IsModifier = true
                });
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod, "装备前缀", EnchantmentText(EnchantmentType, out bool B, out bool R) + ((Value == 0) ? "(" + Language.GetTextValue("Mods.DDmod.properties.唯一") + ")" : ("" + Value)) + (B ? "%" : ""))
                {
                    OverrideColor = new Color(255, 100, 100)
                });
            }
        }

        if (EnchantmentItemType[0] > 0)
        {
            tooltips.Add(new TooltipLine(Mod, "熔炼材料", Language.GetTextValue("Mods.DDmod.properties.融合材料") + Text(0) + Text(1))
            {
                OverrideColor = new Color(11, 200, 245)
            });
        }
        foreach (TooltipLine line in tooltips)
        {
            if (line.Mod == "Terraria")
            {
                string Text = line.Text;
                int Damage = 0;
                int damage = (int)(item.OriginalDamage * 0.05F);
                if (damage < 1)
                {
                    damage = 1;
                }
                //for (int a = 0; a < Level; a++)
                {
                    Damage += damage * Level;
                }
                if (line.Name == "PrefixDamage")
                {
                    Item item2 = Main.tooltipPrefixComparisonItem;
                    if (item2 == null || item2.netID != item.netID)
                    {
                        item2 = DDmod.DDmod.NewItem.Clone();
                        item2.netDefaults(item.netID);
                    }
                    if (item.GetGlobalItem<AdventureGearGlobalItem>().CanDamage > 0)
                    {
                        Damage += (int)(item.GetGlobalItem<AdventureGearGlobalItem>().CanDamage * ((float)item.GetGlobalItem<AdventureGearGlobalItem>().Quality / 2));
                    }
                    double num7 = (float)item.damage - Damage - (float)item2.damage;
                    num7 = num7 / (double)item2.damage * 100.0;
                    num7 = Math.Round(num7);
                    if (num7 > 0.0)
                        line.Text = "+" + num7 + Lang.tip[39].Value;
                    else if (num7 != 0.0)
                    {
                        line.Text = "" + num7 + Lang.tip[39].Value;
                        line.IsModifierBad = true;
                    }
                    else
                    {
                        line.Text = "";
                    }
                }
                if (line.Name == "PrefixCritChance")
                {
                    Item item2 = Main.tooltipPrefixComparisonItem;
                    if (item2 == null || item2.netID != item.netID)
                    {
                        item2 = DDmod.DDmod.NewItem.Clone();
                        item2.netDefaults(item.netID);
                    }
                    int Crit = 0;
                    if (item.GetGlobalItem<AdventureGearGlobalItem>().CanCrit > 0)
                    {
                        Crit += (int)(item.GetGlobalItem<AdventureGearGlobalItem>().CanCrit * ((float)item.GetGlobalItem<AdventureGearGlobalItem>().Quality / 2));
                    }
                    double num7 = (float)item.crit - Crit - (float)item2.crit;
                    num7 = Math.Round(num7);
                    if (num7 > 0.0)
                        line.Text = "+" + num7 + Lang.tip[41].Value;
                    else if (num7 != 0.0)
                    {
                        line.Text = "" + num7 + Lang.tip[41].Value;
                        line.IsModifierBad = true;
                    }
                    else
                    {
                        line.Text = "";
                    }
                }
                StatModifier modifier = Main.LocalPlayer.GetTotalDamage(item.DamageType);

                if (AmmoID.Sets.IsArrow[item.useAmmo])
                    modifier = modifier.CombineWith(Main.LocalPlayer.arrowDamage);

                if (AmmoID.Sets.IsBullet[item.useAmmo])
                    modifier = modifier.CombineWith(Main.LocalPlayer.bulletDamage);

                if (AmmoID.Sets.IsSpecialist[item.useAmmo])
                    modifier = modifier.CombineWith(Main.LocalPlayer.specialistDamage);

                CombinedHooks.ModifyWeaponDamage(Main.LocalPlayer, item, ref modifier);
                Damage = 0;
                //for (int a = 0; a < Level; a++)
                {
                    Damage += damage* Level;
                }
                int IDamage = (int)modifier.ApplyTo(item.damage - Damage);
                int IDamage2 = GetWeaponDamage(item, true) - (int)modifier.ApplyTo(item.damage - Damage);
                if (line.Name == "Damage")
                {
                    if (Level > 0)
                    {
                        line.Text = Text.Replace(GetWeaponDamage(item, true).ToString(), "(" + IDamage + "+" + IDamage2 + ")");
                    }
                    else if (Level < 0)
                    {
                        line.Text = Text.Replace(GetWeaponDamage(item, true).ToString(), "(" + IDamage + IDamage2 + ")");

                    }
                }

            }
        }
    }
}