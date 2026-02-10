
using System.Xml.Linq;
using Terraria.ID;

namespace DDmod.BossHealthBar
{
    public class NPCHealthBar : GlobalNPC
    {
        //头
        public static Asset<Texture2D>[] Head = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
        //身
        public static Asset<Texture2D>[] Mid = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
        //尾
        public static Asset<Texture2D>[] Tail = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
        //条
        public static Asset<Texture2D>[] Fill = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
        //额外条
        public static Asset<Texture2D>[] FillE = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
        //内
        public static Asset<Texture2D>[] End = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
        //盾
        public static Asset<Texture2D>[] Shield = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
        //无敌
        public static Asset<Texture2D>[] Lock = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
        //额外材质
        public static Asset<Texture2D> DD2E;
        //重置npc最大总数
        public static bool MaxNPCReset;

        //子头
        public Asset<Texture2D> Head2;
        //子身
        public Asset<Texture2D> Mid2;
        //子尾
        public Asset<Texture2D> Tail2;
        //子条
        public Asset<Texture2D> Fill2;
        //子内
        public Asset<Texture2D> End2;
        //子盾
        public Asset<Texture2D> Shield2;
        //子无敌
        public Asset<Texture2D> Lock2;
        //文本位置
        public Vector2 TextPosition;
        //位置
        public float position;
        //绑定一个NPC
        public int[] HealthNPCType;
        //头像
        public static Asset<Texture2D>[] BossHead = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
        //头像偏移
        public static Vector2[] Headoffset = NPCID.Sets.Factory.CreateCustomSet(Vector2.Zero);
        public int damage;
        public int damageTime;
        /// <summary>
        /// X：帧的数量
        /// Y: 动画速度
        /// </summary>
        /// <param name="Head"></param>
        /// <param name="Mid"></param>
        /// <param name="Tail"></param>
        /// <param name="Lock"></param>
        public void HealthBarFrame(Vector2 Head, Vector2 Mid, Vector2 Tail, Vector2 Lock)
        {
            Headframe = (int)Head.X;
            Midframe = (int)Mid.X;
            Tailframe = (int)Tail.X;
            Lockframe = (int)Lock.X;
            HeadframeCounter = (int)Head.Y;
            MidframeCounter = (int)Mid.Y;
            TailframeCounter = (int)Tail.Y;
            LockframeCounter = (int)Lock.Y;
        }
        //开启血条(Boss默认开启)
        public bool NeedBlood;
        //帧图计时器
        public int frame;
        //帧图数量
        public int Headframe =1;
        public int Midframe = 1;
        public int Tailframe = 1;
        public int Lockframe = 1;
        //帧图速度
        public int HeadframeCounter = 1;
        public int MidframeCounter = 1;
        public int TailframeCounter = 1;
        public int LockframeCounter = 1;
        //当前帧
        public int HeadframeCurrent;
        public int MidframeCurrent;
        public int TailframeCurrent;
        public int LockframeCurrent;
        //框内长度
        public float LeftLength;
        public float RightLength;
        //需要重合血条
        public bool Multiple;
        //子血条
        public bool Child;
        //集合Boss血条
        public int[] Gather;
        //记录最大血量
        public long multiNPCLifeMax;
        //血条大小
        public float Scale = 1;
        //文本大小
        public float TextScale = 1;
        //小boss
        public bool MiniBoss = false;
        //假血条长度
        public float FakeLength;
        //假血条长度计时器
        public float FakeLengthTime;

        public override bool InstancePerEntity => true;
        internal int[] multiNPCType = null;
        public override void Load()
        {
        }
        public override void Unload()
        {
            Head = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
            //身
            Mid = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
            //尾
            Tail = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
            //条
            Fill = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
            //额外条
            FillE = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
            //内
            End = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
            //盾
            Shield = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
            //无敌
            Lock = NPCID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
        }
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                int type;
                type = 4;
                string name = "克苏鲁之眼";
                //DDSystem.HBar(type, name, new Vector2(-12, 2));
                DDSystem.HBar(type, name, new Vector2(-12000, 2));
                type = 126;
                name = "魔焰眼";
                DDSystem.HBar(type, name, new Vector2(-24000, 2));
                FillE[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Fill_Glow");
                //激光眼
                type = 125;
                name = "激光眼";
                DDSystem.HBar(type, name, new Vector2(-24000, 2));
                FillE[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Fill_Glow");
                //史莱姆王
                type = 50;
                name = "史莱姆王";
                //DDSystem.HBar(type, name, new Vector2(-29, 18));
                DDSystem.HBar(type, name, new Vector2(-11129, 18));
                //蜂王
                type = 222;
                name = "蜂王";
                DDSystem.HBar(type, name, new Vector2(-10000, 30));
                //骷髅王
                type = 35;
                name = "骷髅王";
                DDSystem.HBar(type, name, new Vector2(-10000, 30));
                type = 36;
                DDSystem.HBar(type, name, new Vector2(-10000, 30), "手");
                //肉山
                type = 113;
                name = "肉山";
                DDSystem.HBar(type, name, new Vector2(-499992, 0));
                //毁灭者
                type = 134;
                name = "毁灭者";
                DDSystem.HBar(type, name, new Vector2(40000, 0));
                FillE[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Fill_Glow");
                //机械骷髅王
                type = 127;
                name = "机械骷髅王";
                DDSystem.HBar(type, name, new Vector2(40000, 0));
                FillE[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Fill_Glow");
                //机械骷髅王手
                for (int A = 0; A < 4; A++)
                {
                    type = 128 + A;
                    DDSystem.HBar(type, name, new Vector2(-2, 0), "手");
                    FillE[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/手Fill_Glow");
                }
                //石巨人
                type = 246;
                BossHead[type] = TextureAssets.NpcHeadBoss[5];
                //月球领主心脏
                type = 398;
                name = "月球领主";
                DDSystem.HBar(type, name);
                //月球领主手
                type = 397;
                name = "月球领主";
                DDSystem.HBar(type, name, default, "手");
                //月球领主手
                type = 396;
                name = "月球领主";
                DDSystem.HBar(type, name, new Vector2(10000, 0), "头");


                //BossHead[type] = ModContent.Request<Texture2D>("DDmod/Textures/月球领主心脏");
                //塔防

                for (int A = 0; A < 5; A++)
                {
                    if (A == 0) type = 551;
                    if (A == 1) type = 564;
                    if (A == 2) type = 565;
                    if (A == 3) type = 576;
                    if (A == 4) type = 577;
                    name = "地牢守护者";
                    DDSystem.HBar(type, name, new Vector2(-2, 0));
                    FillE[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Fill_Glow");
                    DD2E = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/E");
                    if (type == 551)
                        Headoffset[type] = new Vector2(-68, 12);
                    if (type == 564 || type == 565)
                        Headoffset[type] = new Vector2(-62, 12);
                    if (type == 576 || type == 577)
                        Headoffset[type] = new Vector2(-60, 12);
                }
                //哀木和南瓜王
                type = 325;
                name = "南瓜月";
                DDSystem.HBar(type, name, new Vector2(-52, -8));
                type = 327;
                DDSystem.HBar(type, name, new Vector2(-52, -8));

                type = 344;
                name = "霜月";
                DDSystem.HBar(type, name, new Vector2(-52, -8));
                type = 345;
                DDSystem.HBar(type, name, new Vector2(-52, -8));
                type = 346;
                DDSystem.HBar(type, name, new Vector2(-52, -8));

                type = 216;
                name = "海盗船";
                DDSystem.HBar(type, name, new Vector2(-52, -8));
                type = 492;
                DDSystem.HBar(type, name, new Vector2(-52, -8));

                type = 395;
                name = "火星飞碟";
                DDSystem.HBar(type, name, new Vector2(-52000, -8), "", true);

                type = 657;
                name = "史莱姆皇后";
                DDSystem.HBar(type, name, new Vector2(-52000, -8));

                name = "世界吞噬者";
                DDSystem.HBar(13, name, new Vector2(-52000, -8));
                DDSystem.HBar(14, name, new Vector2(-52000, -8));
                DDSystem.HBar(15, name, new Vector2(-52000, -8));
                type = 266;
                name = "克苏鲁之脑";
                DDSystem.HBar(type, name, new Vector2(-52000, -8), "", true);
                type = 668;
                name = "巨鹿";
                DDSystem.HBar(type, name, new Vector2(-52000, -8));
                type = 262;
                name = "世纪之花";
                DDSystem.HBar(type, name, new Vector2(-52000, -8));

                type = 245;
                name = "石巨人";
                DDSystem.HBar(type, name, new Vector2(-52000, -8),"",true);

                type = 247;
                name = "石巨人";
                DDSystem.HBar(type, name, new Vector2(-52000, -8),"手");
                type = 248;
                name = "石巨人";
                DDSystem.HBar(type, name, new Vector2(-52000, -8), "手");
                NPCHealthBar.Head[type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/手Head2");

                type = 370;
                name = "猪鲨";
                DDSystem.HBar(type, name, new Vector2(-52000, -8));
                type = 439;
                name = "教徒";
                DDSystem.HBar(type, name, new Vector2(-52000, -8));
                type = 636;
                name = "光之女皇";
                DDSystem.HBar(type, name, new Vector2(-52, -8));

                //四柱
                type = 517;
                name = "四柱/日耀柱";
                DDSystem.HBar(type, name, new Vector2(-38, 0),Shield:true);
                type = 422;
                name = "四柱/星璇柱";
                DDSystem.HBar(type, name, new Vector2(-38, 0), Shield: true);
                type = 507;
                name = "四柱/星云柱";
                DDSystem.HBar(type, name, new Vector2(-38, 0), Shield: true);
                type = 493;
                name = "四柱/星尘柱";
                DDSystem.HBar(type, name, new Vector2(-38, 0), Shield: true);


            }
        }
        public override void SetDefaults(NPC npc)
        {
            //npc.dontTakeDamage = true;

            NPCHealthBar.Headoffset[636] = new Vector2(-24, 0);
            if (!Main.dedServ)
            {
                if (npc.boss)
                {
                    NeedBlood = true;
                }
                if (npc.type == 325 || npc.type == 327)
                {
                    TextScale = 0.6f;
                }
                if (npc.type == 344 || npc.type == 345 || npc.type == 346)
                {
                    TextScale = 0.6f;
                    TextPosition.Y = -6;
                }
                //飞碟
                if (npc.type == NPCID.MartianSaucerCore)
                {
                    TextScale = 0.8f;
                    HealthNPCType = new int[2];
                    HealthNPCType[0] = 393;
                    HealthNPCType[1] = 394;
                    multiNPCLifeMax = 0;
                }
                if (npc.type == NPCID.MartianSaucerTurret || npc.type == NPCID.MartianSaucerCannon)
                {
                    MiniBoss = false;
                }
                //石巨人
                if (npc.type == 247 || npc.type == 248)
                {
                    Child = true;
                    MiniBoss = true;
                }
                if (npc.type == NPCID.Golem)
                {
                    BossHead[NPCID.Golem] = TextureAssets.NpcHeadBoss[5];
                    HealthNPCType = new int[1];
                    HealthNPCType[0] = 246;
                    Multiple = true;
                }
                //骷髅王
                if (npc.type == 35)
                {
                    Multiple = true;
                    TextScale = 1f;
                    //TextPosition.Y = 10;
                }
                if (npc.type == 36)
                {
                    MiniBoss = true;
                    //TextPosition.Y = 6;
                    TextScale = 0.8f;
                    Child = true;
                }
                if (npc.type == 668)
                {
                    TextPosition.Y = 4;
                }
                if (npc.type == 222)
                {
                    TextPosition.Y = 4;
                }
                //幻影弓龙
                if (npc.type == 454)
                {
                    MiniBoss = true;
                }
                //机械骷髅王
                if (npc.type == 127)
                {
                    Multiple = true;
                }
                if (npc.type == 128 || npc.type == 129 || npc.type == 130 || npc.type == 131)
                {
                    MiniBoss = true;
                    Child = true;
                }
                //飞眼怪
                if (npc.type == NPCID.BrainofCthulhu)
                {
                    HealthNPCType = new int[1];
                    HealthNPCType[0] = 267;

                    multiNPCLifeMax = 0;
                }
                //海盗船
                if (npc.type == 492)
                {
                    BossHead[492] = TextureAssets.NpcHeadBoss[26];
                    TextScale = 0.6f;
                    NeedBlood = true;
                    if (!AnyNPCs(492))
                    {
                        Gather = new int[1];
                        Gather[0] = 492;
                        HealthBar hb = new HealthBar();
                        BossDisplayInfo.SetCustomHealthBarMultiple(hb, Gather);
                    }
                }
                //大boss血条
                //南瓜月
                if (npc.type == NPCID.MourningWood || npc.type == NPCID.Pumpking ||
                     //霜月
                     npc.type == 346 || npc.type == 345 || npc.type == 344 ||
                     //撒旦
                     npc.type == 564 || npc.type == 565 || npc.type == 576 || npc.type == 577 || npc.type == 551 ||
                     //四柱
                     npc.type == 517 || npc.type == 507 || npc.type == 493 || npc.type == 422)
                {
                    NeedBlood = true;
                }
                //小boss血条
                //海盗
                if (npc.type == 216 ||
                     //血月
                     npc.type == 618 || npc.type == 620 || npc.type == 621)
                {
                    MiniBoss = true;
                }
                //世界吞噬者
                if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
                {
                    NeedBlood = true;
                }
                if (npc.type == NPCID.EaterofWorldsHead)
                {
                    NeedBlood = true;
                    if ((!AnyNPCs(14) && !AnyNPCs(15)))
                    {
                        Gather = new int[3];
                        Gather[0] = 13;
                        Gather[1] = 14;
                        Gather[2] = 15;
                        HealthBar hb = new HealthBar();
                        BossDisplayInfo.SetCustomHealthBarMultiple(hb, Gather);
                    }
                }
                //克苏鲁心脏
                if (npc.type == 398)
                {
                    HealthBarFrame(new Vector2(6, 6), new Vector2(1, 0), new Vector2(6, 6), new Vector2(1, 0));
                }
                //月总
                if (npc.type == NPCID.MoonLordHead)
                {
                    Multiple = true;
                    HealthBarFrame(new Vector2(8, 5), new Vector2(1, 0), new Vector2(8, 5), new Vector2(1, 0));
                }
                if (npc.type == NPCID.MoonLordHand)
                {
                    MiniBoss = true;
                    Child = true;
                    HealthBarFrame(new Vector2(4, 0), new Vector2(1, 0), new Vector2(9, 5), new Vector2(1, 0));
                }
                //当判定位大boss
                if (NeedBlood && !MiniBoss)
                {
                    if (Head[npc.type] == null)
                        Head[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认血条Head");
                    if (Fill[npc.type] == null)
                        Fill[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认血条Fill");
                    if (Mid[npc.type] == null)
                        Mid[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认血条Mid");
                    if (Tail[npc.type] == null)
                        Tail[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认血条Tail");
                    if (End[npc.type] == null)
                        End[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认血条End");
                    if (Shield[npc.type] == null)
                        Shield[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认血条Shield");
                    if (Lock[npc.type] == null)
                        Lock[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认血条Lock");
                }
                //当判定位小boss
                if (MiniBoss)
                {
                    NeedBlood = true;
                    TextScale = 0.6f;
                    if (Head[npc.type] == null)
                        Head[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认小血条Head");
                    if (Fill[npc.type] == null)
                        Fill[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认小血条Fill");
                    if (Mid[npc.type] == null)
                        Mid[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认小血条Mid");
                    if (Tail[npc.type] == null)
                        Tail[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认小血条Tail");
                    if (End[npc.type] == null)
                        End[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认小血条End");
                    if (Shield[npc.type] == null)
                        Shield[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认小血条Shield");
                    if (Lock[npc.type] == null)
                        Lock[npc.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "默认小血条Lock");
                }
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.MartianSaucerTurret || npc.type == NPCID.MartianSaucerCannon)
            {
                npc.TargetClosest();
            }
            if (!Main.dedServ)
            {
                if (npc.type == 551)
                    Headoffset[npc.type] = new Vector2(-68, 12);
                if (npc.type == 564 || npc.type == 565)
                    Headoffset[npc.type] = new Vector2(-62, 12);
                if (npc.type == 576 || npc.type == 577)
                    Headoffset[npc.type] = new Vector2(-60, 12);

                string name;
                if (Head[npc.type] != null)
                {
                    damageTime++;
                }
                //月球领主心脏
                if (npc.type == NPCID.MoonLordCore)
                {
                    NeedBlood = !npc.dontTakeDamage;
                }
                if (npc.type == NPCID.MoonLordHand)
                {
                    NeedBlood = npc.ai[0] != -2;
                    MiniBoss = npc.ai[0] != -2;

                    HeadframeCurrent = npc.frame.Height == 0 ? 0 : npc.frame.Y / npc.frame.Height;
                    if (npc.ai[2] == 1)
                    {
                        name = "月球领主";
                        if (Head2 == null)
                        {
                            Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/手Head2");
                        }
                        if (Tail2 == null)
                        {
                            Tail2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/手Tail2");
                        }
                    }
                }
                
                if (npc.type == NPCID.MoonLordHead)
                {
                    NeedBlood = npc.ai[0] > -2;
                }
                //帧图计时器
                frame++;

                //框框帧图
                if (MidframeCounter > 0)
                {
                    if (frame % MidframeCounter == 0)
                    {
                        MidframeCurrent++;
                    }
                    if (MidframeCurrent >= Midframe) MidframeCurrent = 0;
                }
                //头帧图
                if (HeadframeCounter > 0)
                {
                    if (frame % HeadframeCounter == 0)
                    {
                        HeadframeCurrent++;
                    }
                    if (HeadframeCurrent >= Headframe) HeadframeCurrent = 0;
                }
                //尾帧图
                if (TailframeCounter > 0)
                {
                    if (frame % TailframeCounter == 0)
                    {
                        TailframeCurrent++;
                    }
                    if (TailframeCurrent >= Tailframe) TailframeCurrent = 0;
                }
                //锁帧图
                if (LockframeCounter > 0)
                {
                    if (frame % LockframeCounter == 0)
                    {
                        LockframeCurrent++;
                    }
                    if (LockframeCurrent >= Lockframe) LockframeCurrent = 0;
                }
                //克苏鲁之眼
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    /*
                    if (npc.ai[0] > 1)
                    {
                        if (Head2 == null)
                        {
                            name = "克苏鲁之眼";
                            Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Head2");
                        }
                    }*/
                    if (npc.frame.Y >= 498)
                    {
                        if (Head2 == null)
                        {
                            name = "克苏鲁之眼";
                            Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Head2");
                        }
                    }
                }
                //克苏鲁之脑
                if (npc.type == 266)
                {
                    if (npc.frame.Y >= 728)
                    {
                        if (Head2 == null)
                        {
                            name = "克苏鲁之脑";
                            Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Head2");
                        }
                    }
                }
                //魔焰眼
                if (npc.type == 126)
                {
                    if (npc.frame.Y >= 600)
                    {
                        if (Head2 == null)
                        {
                            name = "魔焰眼";
                            Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Head2");
                        }
                    }
                }
                //激光眼
                if (npc.type == 125)
                {
                    if (npc.frame.Y >= 600)
                    {
                        if (Head2 == null)
                        {
                            name = "激光眼";
                            Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Head2");
                        }
                    }
                }
                //世纪之花
                if (npc.type == 262)
                {
                    if (npc.frame.Y >= 616)
                    {
                        if (Head2 == null)
                        {
                            name = "世纪之花";
                            Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Head2");
                        }
                    }
                }
                //机械骷髅王
                if (npc.type == NPCID.SkeletronPrime)
                {
                    if (npc.ai[1]==1)
                    {
                        if (Head2 == null)
                        {
                            name = "机械骷髅王";
                            Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + name + "/Head2");
                        }
                    }
                    else
                    {
                        if (Head2 != null)
                        {
                            Head2 = null;
                        }
                    }
                }
            }
            return base.PreAI(npc);
        }
    }
}