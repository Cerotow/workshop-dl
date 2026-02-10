
using DDmod.Content.Items.Melee.FlyingKnife.Make;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Ranged.NPCLoot;
using DDmod.Content.Items.Series.Venture;
using DDmod.Content.Items.Series.Venture.任务奖励;
using DDmod.Content.Items.Series.Venture.奖励袋;
using DDmod.Content.Items.Series.仙人掌;
using DDmod.Content.Items.Sundries;
using DDmod.Content.Items.Talisman;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Players;

namespace DDmod.Content.NPCs.TownNPC
{
    public class 任务
    {
        #region 战斗
        public const short 小粉 = -1;

        public const short 森林 = 1;
        public const short 下雨 = 2;
        public const short 洞穴 = 3;
        public const short 发光蘑菇 = 4;
        public const short 水中 = 5;
        public const short 天空 = 6;
        public const short 雪地 = 7;
        public const short 血猩 = 8;
        public const short 腐化 = 9;
        public const short 血月 = 10;
        public const short 丛林 = 11;
        public const short 沙漠 = 12;
        public const short 地狱 = 13;
        public const short 地牢 = 14;
        public const short 神圣 = 15;
        public const short 困难 = 16;
        public const short 三王 = 17;
        public const short 花后 = 18;
        //暂时不用事件
        /*
        public const short 哥布林 = 18;
        public const short 海盗 = 19;
        public const short 南瓜月 = 20;
        public const short 霜月 = 21;
        public const short 火星人 = 22;
        public const short 四柱事件 = 23;
        */
        public static string Text(int type, out int S)
        {
            if (type == 0)
            {
                S = 1;
                return "送";
            }
            //史莱姆,眼球.僵尸
            if (type == 1 || type == 2 || type == 3)
            {
                //有五个条件
                S = 5;
                return "森林";
            }
            //史莱姆之母,骷髅,蝙蝠
            if (type == 16 || type == 21 || type == 49)
            {
                //有三个条件
                S = 3;
                return "地下";
            }
            //蘑菇蝙蝠
            if (type == 634)
            {
                //有一个条件
                S = 1;
                return "发光蘑菇蝙蝠";
            }
            //蘑菇骷髅
            if (type == 635)
            {
                //有一个条件
                S = 1;
                return "发光蘑菇亡灵";
            }
            //真菌球怪
            if (type == 259)
            {
                //有一个条件
                S = 1;
                return "发光蘑菇";
            }
            //飞鱼,雨伞史莱姆
            if (type == 224||type == 225)
            {
                //有三个条件
                S = 3;
                return "下雨";
            }
            //水母鲨鱼螃蟹
            if (type == 63||type == 65||type == 67)
            {
                //有三个条件
                S = 3;
                return "水下";
            }
            //鸟妖
            if (type == 48)
            {
                //有三个条件
                S = 2;
                return "天空";
            }
            //冰雪蝙蝠,雪人僵尸,尖刺冰雪史莱姆
            if (type == 150|| type == 161 || type == 184)
            {
                //有三个条件
                S = 3;
                return "雪地";
            }
            //噬魂怪,腐虫,腐败爬行者
            if (type == 6|| type == 7 || type == ModContent.NPCType<腐败爬行者>())
            {
                //有三个条件
                S = 3;
                return "腐化";
            }
            //血虫,脸怪,血蜘蛛
            if (type == 173|| type == 181 || type == 239)
            {
                //有三个条件
                S = 3;
                return "血猩";
            }
            //蜜蜂,抓人草,丛林蝙蝠
            if (type == 42|| type == 43 || type == 51)
            {
                //有三个条件
                S = 3;
                return "丛林";
            }
            //蚁狮马,蚁狮蜂,仙人掌史莱姆
            if (type == 580|| type == 581 || type == ModContent.NPCType<仙人掌史莱姆>())
            {
                //有三个条件
                S = 3;
                return "沙漠";
            }
            //愤怒骷髅,骷髅头,骷髅法师
            if (type == 31|| type == 32 || type == 34)
            {
                //有三个条件
                S = 3;
                return "地牢";
            }
            //恶魔,地狱蝙蝠,妖精
            if (type == 62|| type ==60 || type == 24)
            {
                //有三个条件
                S = 3;
                return "地狱";
            }
            //精灵,独角兽,荧光蜗牛,荧光史莱姆,混沌精
            if (type == 75 || type == 86 || type == 122||type == 138||type==120)
            {
                //有三个条件
                S = 3;
                return "神圣";
            }
            //骷髅弓箭手,装甲骷髅,徘徊之眼,魔化盔甲,木乃伊,神圣木乃伊
            if (type == 110 || type == 77 || type == 133 || type == 140 || type == 78|| type == 80||
                //沙漠幽魂,冰雪精,冰雪乌龟,丛林乌龟,愤怒捕手,灵液怪
                type == 533 || type == 169 || type == 154 || type == 153 || type == 175 || type == 268||
                //恶心浮游怪,血木乃伊,爬藤怪,腐化史莱姆,暗影木乃伊
                type == 182 || type == 630 || type == 101 || type == 81 || type == 79|| type == 151|| type == 156)
            {
                //有五个条件
                S = 5;
                return "困难";
            }
            //蓝盔甲骷髅,烂盔甲骷髅,地狱骷髅,暗影骷髅法师,幽灵法师,火叉法师
            if (type == 273 || type == 269 || type == 277 || type == 283 || type == 281 || type == 285)
            {
                //有三个条件
                S = 3;
                return "困难地牢";
            }
            //蜥蜴,飞蛇
            if (type == 198 || type == 226)
            {
                //有三个条件
                S = 3;
                return "神庙";
            }
            S = 0;
            return "";
        }
        public static void 战斗任务(out NPC npc, out List<Item> Loot, out string Text, out int Stack, out int ELevel,out int EValue)
        {
            npc = new NPC();
            npc.SetDefaults(目标(out int Ecological, out int stack,out int Level,out int Value));
            Stack = stack;
            Text = 任务.Text(npc.type, out int S) + "击杀委托" + Main.rand.Next(1, S + 1);

            Loot = 战斗奖励(Ecological, npc.type, Stack);
            ELevel = Level;
            EValue = Value;
        }
        public static List<int[]> 随机目标(int Level)
        {
            //可能随机一个全等级任务
            bool B = Main.rand.NextBool(20);
            if (B)
            {
                //冒险等级
                Level = Main.rand.Next(1, Level + 1);
            }
            List<int[]> list = new List<int[]>();

            //等级小于等于3的任务
            int Loot = 0;

            #region 肉前
            #region 一阶段任务
            if (Level <= 3)
            {
                //史莱姆
                list.Add(new int[] { 1, 森林, Main.rand.Next(6, 12), 1, Main.rand.Next(500, 1000) });
                //眼球
                list.Add(new int[] { 2, 森林, Main.rand.Next(6, 12), 2, Main.rand.Next(1000, 2000) });
                //僵尸
                list.Add(new int[] { 3, 森林, Main.rand.Next(6, 12), 2, Main.rand.Next(1000, 2000) });
            }
            #endregion

            #region 二阶段任务
            if (Level >= 2 && Level <= 4)
            {
                //雨伞史莱姆
                list.Add(new int[] { 225, 下雨, Main.rand.Next(2, 5), 3, Main.rand.Next(2000, 5000) });
                //飞鱼
                list.Add(new int[] { 224, 下雨, Main.rand.Next(6, 12), 3, Main.rand.Next(2000, 5000) });

                //史莱姆之母
                list.Add(new int[] { 16, 洞穴, Main.rand.Next(2, 5), 5, Main.rand.Next(2000, 5000) });
                //骷髅
                list.Add(new int[] { 21, 洞穴, Main.rand.Next(6, 12), 5, Main.rand.Next(2000, 5000) });
                //蝙蝠
                list.Add(new int[] { 49, 洞穴, Main.rand.Next(10, 20), 4, Main.rand.Next(2000, 5000) });

                //蘑菇蝙蝠
                list.Add(new int[] { 634, 发光蘑菇, Main.rand.Next(10, 20), 5, Main.rand.Next(2000, 5000) });
                //蘑菇骷髅
                list.Add(new int[] { 635, 发光蘑菇, Main.rand.Next(6, 12), 6, Main.rand.Next(2000, 5000) });
                //真菌球怪
                list.Add(new int[] { 259, 发光蘑菇, Main.rand.Next(1, 3), 6, Main.rand.Next(2000, 5000) });
            }

            #endregion

            #region 三阶段任务
            if (Level >= 3 && Level <= 5)
            {
                //鸟妖
                list.Add(new int[] { 48, 天空, Main.rand.Next(5, 10), 6, Main.rand.Next(3000, 8000) });
                //水母
                list.Add(new int[] { 63, 水中, Main.rand.Next(4, 8), 6, Main.rand.Next(3000, 8000) });
                //鲨鱼
                list.Add(new int[] { 65, 水中, Main.rand.Next(1, 3), 9, Main.rand.Next(3000, 8000) });
                //螃蟹
                list.Add(new int[] { 67, 水中, Main.rand.Next(2, 5), 6, Main.rand.Next(3000, 8000) });

                //冰雪蝙蝠
                list.Add(new int[] { 150, 雪地, Main.rand.Next(10, 20), 7, Main.rand.Next(3000, 8000) });
                //雪人僵尸
                list.Add(new int[] { 161, 雪地, Main.rand.Next(6, 12), 4, Main.rand.Next(3000, 8000) });
                //尖刺冰雪史莱姆
                list.Add(new int[] { 184, 雪地, Main.rand.Next(3, 8), 8, Main.rand.Next(3000, 8000) });

                if (WorldGen.crimson)
                {
                    //血虫
                    list.Add(new int[] { 173, 血猩, Main.rand.Next(8, 15), 7, Main.rand.Next(5000, 10000) });
                    //脸怪
                    list.Add(new int[] { 181, 血猩, Main.rand.Next(8, 15), 4, Main.rand.Next(5000, 10000) });
                    //血蜘蛛
                    list.Add(new int[] { 239, 血猩, Main.rand.Next(8, 15), 8, Main.rand.Next(5000, 10000) });
                }
                else
                {
                    //噬魂怪
                    list.Add(new int[] { 6, 腐化, Main.rand.Next(10, 20), 9, Main.rand.Next(5000, 10000) });
                    //腐虫
                    list.Add(new int[] { 7, 腐化, Main.rand.Next(1, 2), 10, Main.rand.Next(5000, 10000) });
                    //腐败爬行者
                    list.Add(new int[] { ModContent.NPCType<腐败爬行者>(), 腐化, Main.rand.Next(3, 8), 10, Main.rand.Next(5000, 10000) });
                }
                
                //抓人草
                list.Add(new int[] { 43, 丛林, Main.rand.Next(6, 12), 9, Main.rand.Next(8000, 12000) });
                //蜜蜂
                list.Add(new int[] { 42, 丛林, Main.rand.Next(10, 15), 12, Main.rand.Next(8000, 12000) });
                //丛林蝙蝠
                list.Add(new int[] { 51, 丛林, Main.rand.Next(10, 20), 8, Main.rand.Next(8000, 12000) });

                //蚁狮马
                list.Add(new int[] { 580, 沙漠, Main.rand.Next(10, 20), 9, Main.rand.Next(8000, 12000) });
                //蚁狮蜂
                list.Add(new int[] { 581, 沙漠, Main.rand.Next(6, 12), 9, Main.rand.Next(8000, 12000) });
                //仙人掌史莱姆
                list.Add(new int[] { ModContent.NPCType<仙人掌史莱姆>(), 沙漠, Main.rand.Next(8, 16), 6, Main.rand.Next(8000, 12000) });
            }
            #endregion

            #region 四阶段任务
            if (Level >= 4 && Level <= 5)
            {
                if (downedBoss3)
                {
                    //骷髅法师
                    list.Add(new int[] { 32, 地牢, Main.rand.Next(5, 10), 10, Main.rand.Next(30000, 40000) });
                    //骷髅头
                    list.Add(new int[] { 34, 地牢, Main.rand.Next(4, 8), 11, Main.rand.Next(30000, 40000) });
                    //骷髅
                    list.Add(new int[] { 31, 地牢, Main.rand.Next(15, 20), 12, Main.rand.Next(30000, 40000) });
                }
                //恶魔
                list.Add(new int[] { 62, 地狱, Main.rand.Next(5, 10), 13, Main.rand.Next(30000, 40000) });
                //地狱蝙蝠
                list.Add(new int[] { 60, 地狱, Main.rand.Next(15, 20), 11, Main.rand.Next(30000, 40000) });
                //妖精
                list.Add(new int[] { 24, 地狱, Main.rand.Next(3, 8), 12, Main.rand.Next(30000, 40000) });
            }
            #endregion
            #endregion
            #region 肉后
            #region 五阶段任务
            if (Level == 6)
            {
                //骷髅弓箭手
                list.Add(new int[] { 110, 困难, Main.rand.Next(8, 16), 18, Main.rand.Next(100000, 150000) });
                //装甲骷髅
                list.Add(new int[] { 77, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                //徘徊之眼
                list.Add(new int[] { 133, 困难, Main.rand.Next(8, 16), 17, Main.rand.Next(100000, 150000) });
                //魔化盔甲
                list.Add(new int[] { 140, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                //木乃伊
                list.Add(new int[] { 78, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                //神圣木乃伊
                list.Add(new int[] { 80, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                //沙漠幽魂
                list.Add(new int[] { 533, 困难, Main.rand.Next(1, 4), 16, Main.rand.Next(100000, 150000) });
                //冰雪精
                list.Add(new int[] { 169, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                //冰雪乌龟
                list.Add(new int[] { 154, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                //丛林乌龟
                list.Add(new int[] { 153, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                //愤怒捕手
                list.Add(new int[] { 175, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                if (WorldGen.crimson)
                {
                    //灵液怪
                    list.Add(new int[] { 268, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                    //恶心浮游怪
                    list.Add(new int[] { 182, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                    //血木乃伊
                    list.Add(new int[] { 630, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                }
                else
                {
                    //爬藤怪
                    list.Add(new int[] { 101, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                    //腐化史莱姆
                    list.Add(new int[] { 81, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                    //暗影木乃伊
                    list.Add(new int[] { 79, 困难, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                }
                //小精灵
                list.Add(new int[] { 75, 神圣, Main.rand.Next(14, 24), 15, Main.rand.Next(100000, 150000) });
                //独角兽
                list.Add(new int[] { 86, 神圣, Main.rand.Next(4, 8), 17, Main.rand.Next(100000, 150000) });
                //荧光蜗牛
                list.Add(new int[] { 122, 神圣, Main.rand.Next(8, 13), 16, Main.rand.Next(100000, 150000) });
                //荧光史莱姆
                list.Add(new int[] { 138, 神圣, Main.rand.Next(8, 16), 16, Main.rand.Next(100000, 150000) });
                //混沌精
                list.Add(new int[] { 120, 神圣, Main.rand.Next(3, 8), 17, Main.rand.Next(100000, 150000) });
            }
            #endregion

            #region 六阶段任务
            if (Level >= 7 && Level <= 8)
            {
                //骷髅弓箭手
                list.Add(new int[] { 110, 三王, Main.rand.Next(12, 20), 18, Main.rand.Next(200000, 300000) });
                //装甲骷髅
                list.Add(new int[] { 77, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                //徘徊之眼
                list.Add(new int[] { 133, 三王, Main.rand.Next(12, 20), 17, Main.rand.Next(200000, 300000) });
                //魔化盔甲
                list.Add(new int[] { 140, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                //木乃伊
                list.Add(new int[] { 78, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                //神圣木乃伊
                list.Add(new int[] { 80, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                //沙漠幽魂
                list.Add(new int[] { 533, 三王, Main.rand.Next(2, 6), 16, Main.rand.Next(200000, 300000) });
                //冰雪精
                list.Add(new int[] { 169, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                //冰雪乌龟
                list.Add(new int[] { 154, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                //丛林乌龟
                list.Add(new int[] { 153, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                //愤怒捕手
                list.Add(new int[] { 175, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                //熔岩蝙蝠
                list.Add(new int[] { 151, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                //红恶魔
                list.Add(new int[] { 156, 三王, Main.rand.Next(6, 12), 19, Main.rand.Next(200000, 300000) });
                if (WorldGen.crimson)
                {
                    //灵液怪
                    list.Add(new int[] { 268, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                    //恶心浮游怪
                    list.Add(new int[] { 182, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                    //血木乃伊
                    list.Add(new int[] { 630, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                }
                else
                {
                    //爬藤怪
                    list.Add(new int[] { 101, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                    //腐化史莱姆
                    list.Add(new int[] { 81, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                    //暗影木乃伊
                    list.Add(new int[] { 79, 三王, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                }
                //小精灵
                list.Add(new int[] { 75, 神圣, Main.rand.Next(20, 30), 15, Main.rand.Next(200000, 300000) });
                //独角兽
                list.Add(new int[] { 86, 神圣, Main.rand.Next(6, 11), 17, Main.rand.Next(200000, 300000) });
                //荧光蜗牛
                list.Add(new int[] { 122, 神圣, Main.rand.Next(10, 17), 16, Main.rand.Next(200000, 300000) });
                //荧光史莱姆
                list.Add(new int[] { 138, 神圣, Main.rand.Next(12, 20), 16, Main.rand.Next(200000, 300000) });
                //混沌精
                list.Add(new int[] { 120, 神圣, Main.rand.Next(5, 11), 17, Main.rand.Next(200000, 300000) });
            }
            #endregion

            #region 七阶段任务
            if (Level == 8)
            {
                //蓝盔甲骷髅
                list.Add(new int[] { 273, 花后, Main.rand.Next(12, 20), 19, Main.rand.Next(300000, 500000) });
                //烂盔甲骷髅
                list.Add(new int[] { 269, 花后, Main.rand.Next(12, 20), 18, Main.rand.Next(300000, 500000) });
                //地狱骷髅
                list.Add(new int[] { 277, 花后, Main.rand.Next(12, 20), 18, Main.rand.Next(300000, 500000) });
                //暗影骷髅法师
                list.Add(new int[] { 283, 花后, Main.rand.Next(4, 9), 20, Main.rand.Next(300000, 500000) });
                //幽灵法师
                list.Add(new int[] { 281, 花后, Main.rand.Next(4, 9), 20, Main.rand.Next(300000, 500000) });
                //火叉法师
                list.Add(new int[] { 285, 花后, Main.rand.Next(4, 9), 20, Main.rand.Next(300000, 500000) });
                //蜥蜴
                list.Add(new int[] { 198, 花后, Main.rand.Next(12, 20), 24, Main.rand.Next(300000, 500000) });
                //飞蛇
                list.Add(new int[] { 226, 花后, Main.rand.Next(12, 20), 22, Main.rand.Next(300000, 500000) });
            }
            #endregion
            #endregion

            //送奖励
            //list.Add(new int[] { 0, Loot, 0 ,0});
            return list;
        }
        public static int 目标(out int Ecological, out int Stack, out int Level,out int Value)
        {
            Level = 0;
            //设置击杀目标和奖励等级
            List<int[]> list = 随机目标(Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel);


            //随机一条击杀目标和奖励等级
            int Rand = Main.rand.Next(list.Count);
            //奖励环境
            Ecological = list[Rand][1];
            //击杀数量
            Stack = list[Rand][2];
            //任务等级
            Level = list[Rand][3];
            //任务价值
            Value = list[Rand][4];
            //选好的npc
            return list[Rand][0];
        }
        public static List<Item> 战斗奖励(int Level, int type, int Stack)
        {
            List<Item> list = new List<Item>();

            int L = Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel;
            #region 肉前小怪
            #region 一阶段小怪
            //经验值
            NPC npc = new NPC();
            npc.SetDefaults(type);
            NPCLoader.PreAI(npc);
            int Exp = npc.Exp().Exp;
            Exp *= Stack*3;

            //史莱姆
            if (type == 1)
            {
                list.Add(new Item(23, Main.rand.Next(10, 20)));
                if (Main.rand.NextBool(100))
                {
                    list.Add(new Item(1309));
                }
            }
            //恶魔眼
            if (type == 2)
            {
                list.Add(new Item(38, Main.rand.Next(3, 6)));
                if (Main.rand.NextBool(10))
                {
                    list.Add(new Item(236));
                }
            }
            //僵尸
            if (type == 3)
            {
                if (Main.rand.NextBool(10))
                {
                    list.Add(new Item(1304));
                }
            }
            #endregion
            #region 二阶段小怪
            //史莱姆之母
            if (type == 16)
            {
                list.Add(new Item(23, Main.rand.Next(20, 40)));
                if (Main.rand.NextBool(20))
                {
                    list.Add(new Item(1309));
                }
            }
            //骷髅
            if (type == 21)
            {
                int Item = 0;
                if (Main.rand.NextBool(10))
                {
                    Item = 118;
                }
                if (Main.rand.NextBool(20))
                {
                    Item = 954;
                }
                if (Main.rand.NextBool(30))
                {
                    Item = 955;
                }
                if (Main.rand.NextBool(30))
                {
                    Item = 1166;
                }
                if (Main.rand.NextBool(50))
                {
                    Item = 1274;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //蝙蝠
            if (type == 49)
            {
                int Item = 0;
                if (Main.rand.NextBool(10))
                {
                    Item = 18;
                }
                if (Main.rand.NextBool(30))
                {
                    Item = Main.rand.NextBool(2) ? 5097 : 1325;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //真菌蝙蝠
            if (type == 634)
            {
                int Item = 0;
                if (Main.rand.NextBool(10))
                {
                    Item = 18;
                }
                if (Main.rand.NextBool(6))
                {
                    Item = 4764;
                }
                if (Main.rand.NextBool(30))
                {
                    Item = 5097;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //真菌骷髅或者僵尸
            if (type == 635)
            {
                int Item = 0;
                if (Main.rand.NextBool(20))
                {
                    Item = 954;
                }
                if (Main.rand.NextBool(30))
                {
                    Item = 955;
                }
                if (Main.rand.NextBool(30))
                {
                    Item = 1166;
                }
                if (Main.rand.NextBool(50))
                {
                    Item = 1274;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //真菌球怪
            if (type == 259)
            {
            }
            //飞鱼
            if (type == 224)
            {
                int Item = 0;
                if (Main.rand.NextBool(10))
                {
                    Item = 4057;
                }
                if (Main.rand.NextBool(6))
                {
                    Item = ModContent.ItemType<飞鱼镖>();
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //雨伞史莱姆
            if (type == 225)
            {
                int Item = 0;
                if (Main.rand.NextBool(10))
                {
                    Item = 1243;
                }
                if (Main.rand.NextBool(6))
                {
                    Item = 946;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            #endregion
            #region 三阶段小怪
            //鸟妖
            if (type == 48)
            {
                if (Main.rand.NextBool(30))
                {
                    list.Add(new Item(1516));
                }
                else
                {
                    list.Add(new Item(320, Main.rand.Next(2, 8)));
                }
            }
            //水母
            if (type == 63)
            {
                int Item = 0;
                if (Main.rand.NextBool(10))
                {
                    Item = 1303;
                }
                else
                {
                    list.Add(new Item(282,Main.rand.Next(2,8)));
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //鲨鱼
            if (type == 65)
            {
                if (Main.rand.NextBool(2))
                {
                    list.Add(new Item(268));
                }
                else
                {
                    Item item = new Item(319, Main.rand.Next(4, 11));
                    list.Add(item);
                }
            }
            //螃蟹
            if (type == 63)
            {
            }
            //血虫
            if (type == 173)
            {
                int Item = 1330;
                if (Main.rand.NextBool(100))
                {
                    Item = 5094;
                }
                if (Main.rand.NextBool(1000))
                {
                    Item = 5091;
                }
                if (Item > 0)
                {
                    if (Item == 1330)
                    {
                        list.Add(new Item(Item, Main.rand.Next(6, 12)));
                    }
                    else
                        list.Add(new Item(Item));
                }
            }
            //脸怪
            if (type == 181)
            {
                int Item = 1330;
                if (Main.rand.NextBool(100))
                {
                    Item = 5094;
                }
                if (Main.rand.NextBool(1000))
                {
                    Item = 5091;
                }
                if (Item > 0)
                {
                    if (Item == 1330)
                    {
                        list.Add(new Item(Item, Main.rand.Next(6, 12)));
                    }
                    else
                        list.Add(new Item(Item));
                }
            }
            //血蜘蛛
            if (type == 239)
            {
                int Item = 1330;
                if (Main.rand.NextBool(100))
                {
                    Item = 5094;
                }
                if (Main.rand.NextBool(1000))
                {
                    Item = 5091;
                }
                if (Item > 0)
                {
                    if (Item == 1330)
                    {
                        list.Add(new Item(Item, Main.rand.Next(6, 12)));
                    }
                    else
                        list.Add(new Item(Item));
                }
            }
            //噬魂怪
            if (type == 6)
            {
                int Item = 68;
                if (Main.rand.NextBool(100))
                {
                    Item = 5094;
                }
                if (Main.rand.NextBool(1000))
                {
                    Item = 5091;
                }
                if (Item > 0)
                {
                    if (Item == 68)
                    {
                        list.Add(new Item(Item, Main.rand.Next(6, 12)));
                    }
                    else
                    list.Add(new Item(Item));
                }
            }
            //腐虫
            if (type == 7)
            {
                int Item = 68;
                if (Main.rand.NextBool(2))
                {
                    Item = 69;
                }
                if (Main.rand.NextBool(100))
                {
                    Item = 5094;
                }
                if (Main.rand.NextBool(1000))
                {
                    Item = 5091;
                }
                if (Item > 0)
                {
                    if (Item == 68 || Item == 69)
                    {
                        list.Add(new Item(Item,Main.rand.Next(6,12)));
                    }
                    else
                    {
                        list.Add(new Item(Item));
                    }
                }
            }
            //腐败爬行者
            if (type == ModContent.NPCType<腐败爬行者>())
            {
                int Item = 68;
                if (Main.rand.NextBool(2))
                {
                    Item = 69;
                }
                list.Add(new Item(Item, Main.rand.Next(6, 12)));
            }
            //冰雪蝙蝠
            if (type == 150)
            {
                int Item = 0;
                if (Main.rand.NextBool(10))
                {
                    Item = 18;
                }
                if (Main.rand.NextBool(30))
                {
                    Item = Main.rand.NextBool(2) ? 5097 : 1325;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //雪人僵尸
            if (type == 161)
            {
                int Item = 0;
                if (Main.rand.NextBool(4))
                {
                    int R = Main.rand.Next(3);
                    if (R == 0)
                    {
                        Item = 803;
                    }
                    if (R == 1)
                    {
                        Item = 804;
                    }
                    if (R == 2)
                    {
                        Item = 805;
                    }

                }
                if (Main.rand.NextBool(10))
                {
                    Item = 216;
                }
                if (Main.rand.NextBool(20))
                {
                    Item = 1304;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //尖刺冰雪史莱姆
            if (type == 184)
            {
                int Item = 0;
                if (Main.rand.NextBool(100))
                {
                    Item = 1309;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //蜜蜂
            if (type == 42)
            {
                int Item = 209;
                int S = Main.rand.Next(8, 15);
                if (Main.rand.NextBool(10))
                {
                    Item = 887;
                    S = 1;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item, S));
                }
            }
            //抓人草
            if (type == 43)
            {
                list.Add(new Item(210, Main.rand.Next(2, 6)));
            }
            //丛林蝙蝠
            if (type == 51)
            {
                int Item = 0;
                if (Main.rand.NextBool(10))
                {
                    Item = 18;
                }
                if (Main.rand.NextBool(30))
                {
                    Item = Main.rand.NextBool(2) ? 5097 : 1325;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //蚁狮马
            if (type == 580)
            {
                if (Main.rand.NextBool(10))
                {
                    list.Add(new Item(323,Main.rand.Next(2,6)));
                }
                else
                {

                    list.Add(new Item(3772));
                }
            }
            //蚁狮蜂
            if (type == 581)
            {
                if (Main.rand.NextBool(10))
                {
                    list.Add(new Item(323,Main.rand.Next(2,6)));
                }
                else
                {

                    list.Add(new Item(3772));
                }
            }
            //仙人掌史莱姆
            if (type == ModContent.NPCType<仙人掌史莱姆>())
            {
                int R = Main.rand.Next(3);
                if (R == 0)
                {
                    list.Add(new Item(23, Main.rand.Next(20, 60)));
                }
                if (R == 1)
                {
                    list.Add(new Item(276, Main.rand.Next(40, 100)));
                }
                if (R == 2)
                {
                    list.Add(new Item(ModContent.ItemType<仙人掌之魂>(), Main.rand.Next(4, 12)));
                }
            }
            #endregion
            #region 四阶段小怪
            //骷髅
            if (type == 31)
            {
                int Item = 154;
                int S = Main.rand.Next(10,20);
                if (Main.rand.NextBool(10))
                {
                    Item = 327;
                    S = 1;
                }
                if (Main.rand.NextBool(10))
                {
                    Item = 3095;
                    S = 1;
                }
                if (Main.rand.NextBool(40))
                {
                    Item = 1307;
                    S = 1;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item,S));
                }
            }
            //骷髅法师
            if (type == 32)
            {
                int Item = 154;
                int S = Main.rand.Next(10, 20);
                if (Main.rand.NextBool(6))
                {
                    Item = 165;
                    S = 1;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item, S));
                }
            }
            //骷髅头
            if (type == 34)
            {
                int Item = 154;
                int S = Main.rand.Next(10, 20);
                if (Main.rand.NextBool(10))
                {
                    Item = 327;
                    S = 1;
                }
                if (Main.rand.NextBool(10))
                {
                    Item = 3095;
                    S = 1;
                }
                if (Main.rand.NextBool(40))
                {
                    Item = 1307;
                    S = 1;
                }
                if (Main.rand.NextBool(6))
                {
                    Item = 891;
                    S = 1;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item, S));
                }
            }
            
            //恶魔
            if (type == 62)
            {
                int Item =0;
                if (Main.rand.NextBool(8))
                {
                    Item = 272;
                }
                if (Item > 0)
                {
                        list.Add(new Item(Item));
                }
            }
            //地狱蝙蝠
            if (type == 60)
            {
                int Item = 0;
                if (Main.rand.NextBool(10))
                {
                    Item = 1322;
                }
                if (Main.rand.NextBool(100))
                {
                    Item = 5097;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            //妖精
            if (type == 24)
            {
                int Item = 0;
                if (Main.rand.NextBool(5))
                {
                    Item = 1323;
                }
                if (Main.rand.NextBool(100))
                {
                    Item =244;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
            }
            #endregion
            #endregion
            if (L== 6)
            {
                #region 五阶段小怪
                //小精灵
                if (type == 75)
                {
                    int Item = 501;
                    int S = Main.rand.Next(30, 60);
                    if (Main.rand.NextBool(10))
                    {
                        Item = 889;
                        S = 1;
                    }
                    if (Main.rand.NextBool(10))
                    {
                        Item = 890;
                        S = 1;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item, S));
                    }
                    list.Add(new Item(520, Main.rand.Next(12, 24)));
                }
                //独角兽
                if (type == 86)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(10))
                    {
                        Item = 856;
                    }
                    if (Main.rand.NextBool(10))
                    {
                        Item = 282;
                    }
                    if (Main.rand.NextBool(10))
                    {
                        Item = 3260;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(526, Main.rand.Next(10, 20)));
                    list.Add(new Item(520, Main.rand.Next(12, 24)));
                }
                //荧光蜗牛
                if (type == 122)
                {
                    int Item = 23;
                    int S = Main.rand.Next(40, 100);
                    if (Item > 0)
                    {
                        list.Add(new Item(Item, S));
                    }
                    list.Add(new Item(520, Main.rand.Next(12, 24)));
                }

                //荧光史莱姆
                if (type == 138)
                {
                    int Item = 23;
                    int S = Main.rand.Next(40, 100);
                    if (Main.rand.NextBool(10))
                    {
                        Item = 1309;
                        S = 1;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item, S));
                    }
                    list.Add(new Item(520, Main.rand.Next(12, 24)));
                }
                //混沌精
                if (type == 120)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(20))
                    {
                        Item = 1326;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(520, Main.rand.Next(12, 24)));
                }

                //骷髅弓箭手
                if (type == 110)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(8))
                    {
                        Item = 682;
                    }
                    if (Main.rand.NextBool(20))
                    {
                        Item = 1321;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //装甲骷髅
                if (type == 77)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(10))
                    {
                        Item = 886;
                    }
                    if (Main.rand.NextBool(15))
                    {
                        Item = 723;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //徘徊之眼
                if (type == 133)
                {
                    int Item = 38;
                    int S = Main.rand.Next(4, 15);
                    if (Main.rand.NextBool(10))
                    {
                        Item = 236;
                        S = 1;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item, S));
                    }
                }
                //魔化盔甲
                if (type == 140)
                {
                    int Item = 0;
                    int S = 0;
                    if (Item > 0)
                    {
                        list.Add(new Item(Item, S));
                    }
                }
                //木乃伊
                if (type == 78)
                {
                    int Item = Main.rand.Next(870, 873);
                    if (Main.rand.NextBool(10))
                    {
                        Item = 889;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //神圣木乃伊
                if (type == 80)
                {
                    int Item = Main.rand.Next(870, 873);
                    if (Main.rand.NextBool(10))
                    {
                        Item = 893;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(528, Main.rand.Next(2, 6)));
                    list.Add(new Item(520, Main.rand.Next(12, 24)));
                }
                //邪恶木乃伊
                if (type == 630 || type == 79)
                {
                    int Item = Main.rand.Next(870, 873);
                    if (Main.rand.NextBool(10))
                    {
                        if (Main.rand.NextBool(10))
                        {
                            Item = 888;
                        }
                        else
                        {
                            Item = 890;
                        }
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(527, Main.rand.Next(2, 6)));
                    list.Add(new Item(521, Main.rand.Next(12, 24)));
                }
                //沙漠幽魂
                if (type == 533)
                {
                    int Item = 3795;
                    if (Main.rand.NextBool(10))
                    {
                        Item = 3770;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(521, Main.rand.Next(12, 24)));
                }
                //冰雪精
                if (type == 169)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(6))
                    {
                        if (Main.rand.NextBool(3))
                        {

                            Item = 1306;
                        }
                        else
                        {
                            Item = 726;
                        }
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //冰雪乌龟
                if (type == 154)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(6))
                    {
                        if (Main.rand.NextBool(3))
                        {

                            Item = 1306;
                        }
                        else
                        {
                            Item = 1253;
                        }
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //丛林陆龟
                if (type == 153)
                {
                    list.Add(new Item(1328, Main.rand.Next(2, 6)));
                }
                //愤怒捕手
                if (type == 175)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(10))
                    {
                        Item = 1265;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //灵液怪
                if (type == 268)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(100))
                    {
                        Item = 5091;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(1332, Main.rand.Next(20, 60)));
                    list.Add(new Item(521, Main.rand.Next(12, 24)));
                }
                //恶心浮游怪
                if (type == 182)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(10))
                    {
                        Item = 892;
                    }
                    if (Main.rand.NextBool(20))
                    {
                        Item = 996;
                    }
                    if (Main.rand.NextBool(100))
                    {
                        Item = 5091;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(1330, Main.rand.Next(20, 60)));
                    list.Add(new Item(521, Main.rand.Next(12, 24)));
                }
                //爬藤怪
                if (type == 101)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(10))
                    {
                        Item = 892;
                    }
                    if (Main.rand.NextBool(20))
                    {
                        Item = 996;
                    }
                    if (Main.rand.NextBool(100))
                    {
                        Item = 5091;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(522, Main.rand.Next(20, 60)));
                    list.Add(new Item(521, Main.rand.Next(12, 24)));
                }
                //腐化史莱姆
                if (type == 81)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(10))
                    {
                        Item = 888;
                    }
                    if (Main.rand.NextBool(20))
                    {
                        Item = 996;
                    }
                    if (Main.rand.NextBool(100))
                    {
                        Item = 5091;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(23, Main.rand.Next(50, 120)));
                    list.Add(new Item(521, Main.rand.Next(12, 24)));
                }
                #endregion
            }
            if (L >= 7 && L <= 8)
            {
                #region 六阶段小怪
                //小精灵
                if (type == 75)
                {
                    int Item = 501;
                    int S = Main.rand.Next(50, 80);
                    if (Main.rand.NextBool(5))
                    {
                        Item = 889;
                        S = 1;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        Item = 890;
                        S = 1;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item, S));
                    }
                    list.Add(new Item(520, Main.rand.Next(20, 30)));
                }
                //独角兽
                if (type == 86)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(5))
                    {
                        Item = 856;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        Item = 282;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        Item = 3260;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(526, Main.rand.Next(20, 40)));
                    list.Add(new Item(520, Main.rand.Next(20, 30)));
                }
                //荧光蜗牛
                if (type == 122)
                {
                    int Item = 23;
                    int S = Main.rand.Next(100, 200);
                    if (Item > 0)
                    {
                        list.Add(new Item(Item, S));
                    }
                    list.Add(new Item(520, Main.rand.Next(20, 30)));
                }

                //荧光史莱姆
                if (type == 138)
                {
                    int Item = 23;
                    int S = Main.rand.Next(100, 200);
                    if (Main.rand.NextBool(5))
                    {
                        Item = 1309;
                        S = 1;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item, S));
                    }
                    list.Add(new Item(520, Main.rand.Next(20, 30)));
                }
                //混沌精
                if (type == 120)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(10))
                    {
                        Item = 1326;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(520, Main.rand.Next(20, 30)));
                }

                //骷髅弓箭手
                if (type == 110)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(4))
                    {
                        Item = 682;
                    }
                    if (Main.rand.NextBool(10))
                    {
                        Item = 1321;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //装甲骷髅
                if (type == 77)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(5))
                    {
                        Item = 886;
                    }
                    if (Main.rand.NextBool(8))
                    {
                        Item = 723;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //徘徊之眼
                if (type == 133)
                {
                    int Item = 38;
                    int S = Main.rand.Next(7, 20);
                    if (Main.rand.NextBool(5))
                    {
                        Item = 236;
                        S = 1;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item, S));
                    }
                }
                //魔化盔甲
                if (type == 140)
                {
                    int Item = 0;
                    int S = 0;
                    if (Item > 0)
                    {
                        list.Add(new Item(Item, S));
                    }
                }
                //木乃伊
                if (type == 78)
                {
                    int Item = Main.rand.Next(870, 873);
                    if (Main.rand.NextBool(5))
                    {
                        Item = 889;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //神圣木乃伊
                if (type == 80)
                {
                    int Item = Main.rand.Next(870, 873);
                    if (Main.rand.NextBool(5))
                    {
                        Item = 893;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(528, Main.rand.Next(4, 8)));
                    list.Add(new Item(520, Main.rand.Next(20, 30)));
                }
                //邪恶木乃伊
                if (type == 630 || type == 79)
                {
                    int Item = Main.rand.Next(870, 873);
                    if (Main.rand.NextBool(5))
                    {
                        if (Main.rand.NextBool(5))
                        {
                            Item = 888;
                        }
                        else
                        {
                            Item = 890;
                        }
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(527, Main.rand.Next(4, 8)));
                    list.Add(new Item(521, Main.rand.Next(20, 30)));
                }
                //沙漠幽魂
                if (type == 533)
                {
                    int Item = 3795;
                    if (Main.rand.NextBool(5))
                    {
                        Item = 3770;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(521, Main.rand.Next(12, 24)));
                }
                //冰雪精
                if (type == 169)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(3))
                    {
                        if (Main.rand.NextBool(2))
                        {

                            Item = 1306;
                        }
                        else
                        {
                            Item = 726;
                        }
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //冰雪乌龟
                if (type == 154)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(3))
                    {
                        if (Main.rand.NextBool(2))
                        {

                            Item = 1306;
                        }
                        else
                        {
                            Item = 1253;
                        }
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //丛林陆龟
                if (type == 153)
                {
                    list.Add(new Item(1328, Main.rand.Next(4, 8)));
                }
                //愤怒捕手
                if (type == 175)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(5))
                    {
                        Item = 1265;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                }
                //灵液怪
                if (type == 268)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(50))
                    {
                        Item = 5091;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(1332, Main.rand.Next(40, 80)));
                    list.Add(new Item(521, Main.rand.Next(20, 30)));
                }
                //恶心浮游怪
                if (type == 182)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(5))
                    {
                        Item = 892;
                    }
                    if (Main.rand.NextBool(10))
                    {
                        Item = 996;
                    }
                    if (Main.rand.NextBool(50))
                    {
                        Item = 5091;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(1330, Main.rand.Next(40, 80)));
                    list.Add(new Item(521, Main.rand.Next(20, 30)));
                }
                //爬藤怪
                if (type == 101)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(5))
                    {
                        Item = 892;
                    }
                    if (Main.rand.NextBool(10))
                    {
                        Item = 996;
                    }
                    if (Main.rand.NextBool(50))
                    {
                        Item = 5091;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(522, Main.rand.Next(30, 80)));
                    list.Add(new Item(521, Main.rand.Next(20, 30)));
                }
                //腐化史莱姆
                if (type == 81)
                {
                    int Item = 0;
                    if (Main.rand.NextBool(5))
                    {
                        Item = 888;
                    }
                    if (Main.rand.NextBool(10))
                    {
                        Item = 996;
                    }
                    if (Main.rand.NextBool(50))
                    {
                        Item = 5091;
                    }
                    if (Item > 0)
                    {
                        list.Add(new Item(Item));
                    }
                    list.Add(new Item(23, Main.rand.Next(100, 200)));
                    list.Add(new Item(521, Main.rand.Next(20, 30)));
                }
                #endregion
            }
            #region 七阶段小怪
            //蓝盔甲骷髅
            if (type == 273)
            {
                int Item = 0;
                if (Main.rand.NextBool(5))
                {
                    Item = 886;
                }
                if (Main.rand.NextBool(8))
                {
                    if (Main.rand.NextBool(2))
                    {
                        Item = 671;
                    }
                    else
                    {
                        Item = 4679;
                    }
                }
                if (Main.rand.NextBool(10))
                {
                    Item = 1266;
                }
                if (Main.rand.NextBool(15))
                {
                    Item = 1183;
                }
                if (Main.rand.NextBool(20))
                {
                    Item = 1517;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
                list.Add(new Item(154, Main.rand.Next(150, 200)));
            }
            //烂盔甲骷髅
            if (type == 269)
            {
                int Item = 0;
                if (Main.rand.NextBool(5))
                {
                    Item = 885;
                }
                if (Main.rand.NextBool(8))
                {
                    if (Main.rand.NextBool(2))
                    {
                        Item = 671;
                    }
                    else
                    {
                        Item = 4679;
                    }
                }
                if (Main.rand.NextBool(10))
                {
                    Item = 1266;
                }
                if (Main.rand.NextBool(15))
                {
                    Item = 1183;
                }
                if (Main.rand.NextBool(20))
                {
                    Item = 1517;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
                list.Add(new Item(154, Main.rand.Next(150, 200)));
            }
            //地狱盔甲骷髅
            if (type == 277)
            {
                int Item = 0;
                if (Main.rand.NextBool(8))
                {
                    if (Main.rand.NextBool(2))
                    {
                        Item = 671;
                    }
                    else
                    {
                        Item = 4679;
                    }
                }
                if (Main.rand.NextBool(10))
                {
                    Item = 1266;
                }
                if (Main.rand.NextBool(15))
                {
                    Item = 1183;
                }
                if (Main.rand.NextBool(20))
                {
                    Item = 1517;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
                list.Add(new Item(154, Main.rand.Next(150, 200)));
            }
            //暗影法师
            if (type == 283)
            {
                int Item = 0;
                if (Main.rand.NextBool(2))
                {
                    Item =1444;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
                list.Add(new Item(154, Main.rand.Next(150, 200)));
            }
            //幽灵法师
            if (type == 281)
            {
                int Item = 0;
                if (Main.rand.NextBool(2))
                {
                    Item =1446;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
                list.Add(new Item(154, Main.rand.Next(150, 200)));
            }
            //火叉法师
            if (type == 285)
            {
                int Item = 0;
                if (Main.rand.NextBool(2))
                {
                    Item =1445;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
                list.Add(new Item(154, Main.rand.Next(150, 200)));
            }
            //蜥蜴
            if (type == 198)
            {
                int Item = 0;
                if (Main.rand.NextBool(70))
                {
                    Item = 1172;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
                list.Add(new Item(1293, Main.rand.Next(2, 6)));
                list.Add(new Item(2766, Main.rand.Next(10, 30)));
            }
            //飞蛇
            if (type == 226)
            {
                int Item = 0;
                if (Main.rand.NextBool(70))
                {
                    Item =1172;
                }
                if (Item > 0)
                {
                    list.Add(new Item(Item));
                }
                list.Add(new Item(1293, Main.rand.Next(2, 6)));
                list.Add(new Item(2766, Main.rand.Next(10, 30)));
            }
            /*
            //蓝盔甲骷髅,烂盔甲骷髅,地狱骷髅,暗影骷髅法师,幽灵法师,火叉法师
            if (type == 273 || type == 269 || type == 277 || type == 283 || type == 281 || type == 285)
            {
                //有三个条件
                S = 3;
                return "困难地牢";
            }
            //蜥蜴,飞蛇
            if (type == 198 || type == 226)
            {
                //有三个条件
                S = 3;
                return "神庙";
            }*/
            #endregion

            #region 肉后小怪

            #region 环境
            //任务奖励
            if (Level == 森林)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv1>(), 1));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(279, Main.rand.Next(200, 400)),
                        new Item(280),
                        new Item(281),
                        new Item(282, Main.rand.Next(50, 80)),
                        new Item(284),
                        new Item(285),
                        new Item(953),
                        new Item(946),
                        new Item(3068),
                        new Item(3069),
                        new Item(3084),
                        new Item(4341),
                        new Item(ModContent.ItemType<WoodSpiritSwordItem>())
                    };

                    list.Add(Main.rand.Next(items));
                }
                //常驻物品
                //list.Add(new Item(9, Main.rand.Next(50, 150)));
                //list.Add(new Item(5, Main.rand.Next(3, 5)));
                Medicament(ref list, 0);
                
                //矿
                 Orebar(ref list,0, Main.rand.Next(10, 15));
                //强化石
                StrengtheningStone(ref list, 5);
            }
            if (Level == 下雨)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv1>(), 1));
                //常驻物品
                //list.Add(new Item(9, Main.rand.Next(50, 150)));
                //list.Add(new Item(28, Main.rand.Next(3, 5)));
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
                //强化石
                StrengtheningStone(ref list, 5, 1, 1, 2);
            }
            if (Level == 洞穴)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv2>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(49),
                        new Item(50),
                        new Item(53),
                        new Item(54),
                        new Item(5011),
                        new Item(989),
                        new Item(975),
                        new Item(930),
                        new Item(997),
                    };
                    Item item = Main.rand.Next(items);
                    list.Add(item);
                    if (item.type == 930)
                    {
                        list.Add(new Item(931, Main.rand.Next(50, 150)));
                    }
                }
                //常驻物品
                //list.Add(new Item(166, Main.rand.Next(10, 20)));
                //list.Add(new Item(188, Main.rand.Next(2, 5)));
                //药水
                Medicament(ref list, 1);
                //矿
                if (Main.rand.NextBool(2))
                {
                    Orebar(ref list, 1, Main.rand.Next(10, 20));
                }
                else
                {
                    Orebar(ref list, 2, Main.rand.Next(6, 12));
                }
                //强化石
                StrengtheningStone(ref list, 3, 1, 1, 3);
            }
            if (Level == 发光蘑菇)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv2>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(49),
                        new Item(50),
                        new Item(53),
                        new Item(54),
                        new Item(5011),
                        new Item(989),
                        new Item(975),
                        new Item(930),
                        new Item(997),
                        new Item(ModContent.ItemType<蘑菇时装箱>()),
                    };
                    Item item = Main.rand.Next(items);
                    list.Add(item);
                    if (item.type == 930)
                    {
                        list.Add(new Item(931, Main.rand.Next(50, 150)));
                    }
                }
                //常驻物品
                if (Main.rand.NextBool(3))
                {
                    list.Add(new Item(183, Main.rand.Next(50, 100)));
                }
                else
                {
                    list.Add(new Item(194, Main.rand.Next(2, 5)));
                }
                //药水
                Medicament(ref list, 1);
                //矿
                if (Main.rand.NextBool(2))
                {
                    Orebar(ref list, 1, Main.rand.Next(10, 20));
                }
                else
                {
                    Orebar(ref list, 2, Main.rand.Next(6, 12));
                }
                //强化石
                StrengtheningStone(ref list, 3, 1, 1, 3);
            }
            if (Level == 水中)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv2>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(186),
                        new Item(187),
                        new Item(277),
                        new Item(4404),
                        new Item(863),
                        new Item(859),
                        new Item(4460),
                        new Item(4425),
                    };
                    Item item = Main.rand.Next(items);
                    list.Add(item);
                }
                //常驻物品
                //药水
                Medicament(ref list, 1);
                //矿
                if (Main.rand.NextBool(2))
                {
                    Orebar(ref list, 1, Main.rand.Next(10, 20));
                }
                else
                {
                    Orebar(ref list, 2, Main.rand.Next(6, 12));
                }
                //强化石
                StrengtheningStone(ref list, 3, 1, 2, 5);
            }
            if (Level == 天空)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv2>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(159),
                        new Item(65),
                        new Item(158),
                        new Item(2219),
                        new Item(4978),
                    };
                    Item item = Main.rand.Next(items);
                    list.Add(item);
                }
                //常驻物品
                //药水
                Medicament(ref list, 1);
                //矿
                if (Main.rand.NextBool(2))
                {
                    Orebar(ref list, 1, Main.rand.Next(10, 20));
                }
                else
                {
                    Orebar(ref list, 2, Main.rand.Next(6, 12));
                }
                //强化石
                StrengtheningStone(ref list, 3, 1, 2, 5);
            }
            if (Level == 雪地)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv2>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(670),
                        new Item(724),
                        new Item(950),
                        new Item(1319),
                        new Item(987),
                        new Item(1579),
                        new Item(669),
                        new Item(997),
                    };
                    Item item = Main.rand.Next(items);
                    list.Add(item);
                }
                //常驻物品

                //药水
                Medicament(ref list, 1);
                //矿
                if (Main.rand.NextBool(2))
                {
                    Orebar(ref list, 1, Main.rand.Next(10, 20));
                }
                else
                {
                    Orebar(ref list, 2, Main.rand.Next(6, 12));
                }
                //强化石
                StrengtheningStone(ref list, 3, 1, 2, 5);
            }
            if (Level == 腐化)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv3>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(96),
                        new Item(162),
                        new Item(111),
                        new Item(115),
                        new Item(64),
                        new Item(56,Main.rand.Next(20, 50)),
                    };
                    Item item = Main.rand.Next(items);
                    list.Add(item);
                }
                //常驻物品

                //药水
                Medicament(ref list, 1);
                //强化石
                StrengtheningStone(ref list, 3, 1, 2, 5);
            }
            if (Level == 血猩)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv3>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(802),
                        new Item(1256),
                        new Item(800),
                        new Item(1290),
                        new Item(3062),
                        new Item(880,Main.rand.Next(20, 50)),
                    };
                    Item item = Main.rand.Next(items);
                    list.Add(item);
                }
                //常驻物品

                //药水
                Medicament(ref list, 1);
                //强化石
                StrengtheningStone(ref list, 3, 1, 2, 5);
            }
            if (Level == 沙漠)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv3>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(4061),
                        new Item(4062),
                        new Item(4263),
                        new Item(4262),
                        new Item(4056),
                        new Item(4055),
                        new Item(4276),
                    };
                    Item item = Main.rand.Next(items);
                    list.Add(item);
                }
                //常驻物品
                list.Add(new Item(4423, Main.rand.Next(3, 5)));

                //药水
                Medicament(ref list, 1);
                //矿
                if (Main.rand.NextBool(2))
                {
                    Orebar(ref list, 1, Main.rand.Next(10, 20));
                }
                else
                {
                    Orebar(ref list, 2, Main.rand.Next(6, 12));
                }
                //强化石
                StrengtheningStone(ref list, 3, 1, 2, 5);
            }
            if (Level == 丛林)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv3>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(211),
                        new Item(212),
                        new Item(964),
                        new Item(213),
                        new Item(3017),
                        new Item(2292),
                        new Item(753),
                    };
                    Item item = Main.rand.Next(items);
                    list.Add(item);
                }
                //常驻物品

                //药水
                Medicament(ref list, 1);
                //矿
                if (Main.rand.NextBool(2))
                {
                    Orebar(ref list, 1, Main.rand.Next(10, 20));
                }
                else
                {
                    Orebar(ref list, 2, Main.rand.Next(6, 12));
                }
                //强化石
                StrengtheningStone(ref list, 3, 1, 2, 5);
            }
            if (Level == 地牢)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv4>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    List<Item> items = new List<Item>
                    {
                        new Item(164),
                        new Item(157),
                        new Item(113),
                        new Item(163),
                        new Item(156),
                        new Item(155),
                        new Item(3317),
                        new Item(ModContent.ItemType<远古短刀>()),
                        new Item(ModContent.ItemType<远古弓>()),
                    };
                    Item item = Main.rand.Next(items);
                    list.Add(item);
                }
                //常驻物品
                if (Main.rand.NextBool(10))
                {
                    list.Add(new Item(329));
                }
                else
                {
                    list.Add(new Item(2350, Main.rand.Next(3, 5)));
                }
                //药水
                Medicament(ref list, 1);
                //矿
                if (Main.rand.NextBool(2))
                {
                    Orebar(ref list, 1, Main.rand.Next(10, 20));
                }
                else
                {
                    Orebar(ref list, 2, Main.rand.Next(6, 12));
                }
                //强化石
                StrengtheningStone(ref list, 1, 1, 3, 7);
            }
            if (Level == 地狱)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv4>()));
                //稀有物品
                if (Main.rand.NextBool(3))
                {
                    if (NPC.downedBoss3)
                    {
                        List<Item> items = new List<Item>
                    {

                        new Item(3019),
                        new Item(220),
                        new Item(112),
                        new Item(218),
                        new Item(274),
                        new Item(174,Main.rand.Next(40, 80)),
                    };
                        Item item = Main.rand.Next(items);
                        list.Add(item);
                    }
                    else
                    {
                        List<Item> items = new List<Item>
                    {

                        new Item(4879),
                        new Item(174,Main.rand.Next(40, 80)),
                    };
                        Item item = Main.rand.Next(items);
                        list.Add(item);
                    }
                }
                //常驻物品

                //药水
                Medicament(ref list, 2);
                //矿
                if (Main.rand.NextBool(2))
                {
                    Orebar(ref list, 2, Main.rand.Next(20, 40));
                }
                else
                {
                    Orebar(ref list, 3, Main.rand.Next(20, 40));
                }
                //强化石
                StrengtheningStone(ref list, 1, 1, 3, 7);
            }
            if (Level == 神圣)
            {
                list.Add(new Item(ModContent.ItemType<奖励袋Lv5>()));
                //常驻物品
                list.Add(new Item(502, Main.rand.Next(10, 30)));

                //药水
                Medicament(ref list, 3);
                //矿
                Orebar(ref list, 4, Main.rand.Next(10, 40));
                //强化石
                StrengtheningStone(ref list, 3, 2, 1, 3);
            }
            if (Level == 困难)
            {
                //常驻物品
                list.Add(new Item(ModContent.ItemType<奖励袋Lv5>()));
                //药水
                Medicament(ref list, 3);
                //矿
                Orebar(ref list, 4, Main.rand.Next(10, 40));
                //强化石
                StrengtheningStone(ref list, 3, 2, 1, 3);
            }
            if (Level == 三王)
            {
                //常驻物品
                list.Add(new Item(ModContent.ItemType<奖励袋Lv5>()));
                //药水
                Medicament(ref list, 3);
                if (Main.rand.NextBool(2))
                {
                    //矿
                    Orebar(ref list, 4, Main.rand.Next(20, 50));
                }
                else
                {
                    Orebar(ref list, 5, Main.rand.Next(10, 30));
                }
                //强化石
                StrengtheningStone(ref list, 2, 2, 2, 5);
            }
            if (Level == 花后)
            {
                //常驻物品
                list.Add(new Item(ModContent.ItemType<奖励袋Lv6>()));
                //矿
                Orebar(ref list, 5, Main.rand.Next(20, 40));
                //强化石
                StrengtheningStone(ref list, 1, 2, 3, 8);
            }
            #endregion
            #endregion


            #region 特殊战斗

            if (Level == 小粉)
            {
                list.Add(new Item(114, 2));
                if (Main.rand.NextBool(10))
                {
                    list.Add(new Item(ModContent.ItemType<StrengtheningStone>(), 1));
                }
            }
            if (Exp > 0)
            {
                list.Add(new Item(ModContent.ItemType<Exp>(), Exp));
            }
            #endregion
            return list;
        }
        #endregion
        /// <summary>
        /// 药水
        /// </summary>
        /// <param name="Level">0地表,1地下,2洞穴,3原版药水全随机</param>
        public static void Medicament(ref List<Item> list,int Level = 0)
        {
            if(Level==0)
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
            if(Level==1)
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
            if(Level==2)
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
            if(Level==3)
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
        }/// <summary>
        /// 强化石
        /// </summary>
        /// <param name="list"></param>
        /// <param name="NextBool">概率</param>
        /// <param name="Level">等级</param>
        /// <param name="MinStack">最小数量</param>
        /// <param name="MaxStack">最大数量</param>
        public static void StrengtheningStone(ref List<Item> list,int NextBool = 1, int Level = 1,int MinStack = 1, int MaxStack = 1)
        {
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
                if (A==0)
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
                if (A ==2)
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
        #region 收集
        public static void 收集任务(out Item item, out List<Item> Loot, out string Text)
        {
            Loot = new List<Item>();
            int R = Main.rand.Next(3);
            Text = "";
            item = DDmod.NewItem.Clone();
            if (R == 0)
            {
                Text = "物块委托" + Main.rand.Next(1, 5);
                item = new Item(2, 10);
            }
            if (R == 1)
            {
                Text = "材料委托" + Main.rand.Next(1, 3);
                item = new Item(23, 100);
            }
            if (R == 2)
            {
                Text = "武器委托" + Main.rand.Next(1, 3);
                item = new Item(4);
            }
            if (R == 2)
            {
                Text = "食材委托" + Main.rand.Next(1, 3);
                item = new Item(4);
            }
            Loot.Add(new Item(29, 2));
            Loot.Add(new Item(19, 5));
            Loot.Add(new Item(20, 5));
        }
        #endregion
    }
}
