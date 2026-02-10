using DDmod.Content.Biome;
using DDmod.Content.Items.Melee.FlyingKnife.Make;
using DDmod.Content.Items.Melee.TwinSwords;
using DDmod.Content.Items.Potion;
using DDmod.Content.Items.Ranged;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Ranged.Make.Gun;
using DDmod.Content.Items.Ranged.NPCLoot;
using DDmod.Content.Items.Series.Venture;
using DDmod.Content.Items.Series.Venture.奖励袋.特别奖励;
using DDmod.Content.Items.Series.杂物;
using DDmod.Content.Items.Sundries;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Items.Tiles;
using DDmod.Content.NPCs.BattlePetNPC;
using DDmod.NoContent.Config;
using DDmod.Players;
using DDmod.SubworldLibraryWorld;
using DDmod.UI;
using DDmod.UI.HunterQuests;
using DDmod.Worlds;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using static Terraria.GameContent.Animations.Actions.NPCs;
using static Terraria.ModLoader.NPCShop;

namespace DDmod.Content.NPCs.TownNPC
{
    public enum DDShop : byte
    {
        /// <summary>
        /// 白1
        /// </summary>
        white1,
        /// <summary>
        /// 白2
        /// </summary>
        white2,
        /// <summary>
        /// 白3
        /// </summary>
        white3,
        /// <summary>
        /// 白4
        /// </summary>
        white4,
        /// <summary>
        /// 白5
        /// </summary>
        white5,
        /// <summary>
        /// 绿1
        /// </summary>
        green1,
        /// <summary>
        /// 绿2
        /// </summary>
        green2,
        /// <summary>
        /// 绿3
        /// </summary>
        green3,
        /// <summary>
        /// 绿4
        /// </summary>
        green4,
        /// <summary>
        /// 绿5
        /// </summary>
        green5,
        /// <summary>
        /// 蓝1
        /// </summary>
        blue1,
        /// <summary>
        /// 蓝2
        /// </summary>
        blue2,
        /// <summary>
        /// 蓝3
        /// </summary>
        blue3,
        /// <summary>
        /// 蓝4
        /// </summary>
        blue4,
        /// <summary>
        /// 蓝5
        /// </summary>
        blue5,
    }

    [AutoloadHead]
    public class 委托兑换商史莱姆 : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Adventurer Slime");
           //DisplayName.AddTranslation(7, "冒险家史莱姆");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 5;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 500;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 35;
            NPCID.Sets.AttackAverageChance[NPC.type] = 10;
            NPCID.Sets.HatOffsetY[NPC.type] = 100000000;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 14;
            NPC.height = 32;
            NPC.aiStyle = 7;
            NPC.damage = 25;
            NPC.defense = 35;
            NPC.lifeMax = 114514;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.4f;
            AnimationType = NPCID.GoblinTinkerer;
            NPC.scale = 1f;
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Like)
                .SetBiomeAffection<SkyBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<UnderworldBiome>(AffectionLevel.Hate)
                .SetNPCAffection(ModContent.NPCType<HunterSlime>(), AffectionLevel.Love)
                .SetNPCAffection(ModContent.NPCType<冒险家史莱姆>(), AffectionLevel.Love)
                .SetNPCAffection(22, AffectionLevel.Like)
                .SetNPCAffection(19, AffectionLevel.Dislike)
                .SetNPCAffection(38, AffectionLevel.Hate);
        }

        public override void HitEffect(HitInfo hit)
        {
            
            int num = NPC.life > 0 ? 4 : 25;
            for (int k = 0; k < num; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.t_Cactus);
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.委托兑换商史莱姆"))
            });
        }
        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return ModContent.GetInstance<DDConfigServer>().AdventureCoinDealerSlime;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Gila"
            };
        }


        public override string GetChat()
        {
            NPC npc = new NPC();
            NPC npc2 = new NPC();
            npc.SetDefaults(ModContent.NPCType<HunterSlime>());
            npc2.SetDefaults(ModContent.NPCType<冒险家史莱姆>());
            return Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Chat1",new object[] {npc.FullName,npc2.FullName });
        }

        public override void AI()
        {
            if (!ModContent.GetInstance<DDConfigServer>().AdventureCoinDealerSlime)
            {
                NPC.Kill();
            }
            if (SWSystem.TrueSubworld)
            {
                NPC.position = new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16 - NPC.height);
                NPC.direction = 1;
                NPC.dontTakeDamage = true;
            }
            if(NPCs ==null)
            {
                NPCs = new int[200];
            }
            if(NPCs2 == null)
            {
                NPCs2 = new int[200];
            }
            for (int a = 0; a < 200; a++)
            {
                NPCs[a]--;
                NPCs2[a]--;
                if (!Main.npc[a].active)
                {
                    NPCs[a] = 0;
                }
                else
                {
                    if (NPCs2[a] > 0)
                    {
                        NPC.velocity = Vector2.Zero;
                        NPC.direction = (NPC.Center.X - Main.npc[a].Center.X) > 0 ? -1 : 1;
                        break;
                    }
                }
            }
            if (Main.rand.NextBool(200) && CText <= 0)
            {
                for (int a = 0; a < 200; a++)
                {
                    if (Main.npc[a].active && Main.npc[a].whoAmI != NPC.whoAmI && NPCs[a] < 0)
                    {
                        if ((Main.npc[a].Center - NPC.Center).Length() < 300 && Main.npc[a].type != 19 && Main.npc[a].type != 38)
                        {
                            CText = 300;
                            NPCs2[a] = 3000;
                            if (Main.dayTime)
                            {
                                if (Main.time < 21600)
                                {
                                    CText2 = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.say1") + Main.npc[a].FullName;
                                }
                                else
                                {
                                    CText2 = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.say2") + Main.npc[a].FullName;
                                }
                            }
                            else
                            {
                                CText2 = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.say3") + Main.npc[a].FullName;
                            }
                            TL = 0;
                            TLTime = 0;
                            NPCs[a] = 10000;
                        }
                    }
                }
            }
        }
        int[] NPCs = new int[200];
        int[] NPCs2 = new int[200];
        int CText;
        string CText2;
        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (ShopType == DDShop.white1)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<白色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop1") + ")" });
            }
            else
            if (ShopType == DDShop.white2)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<白色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop2") + ")" });
            }
            else
            if (ShopType == DDShop.white3)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<白色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop3") + ")" });
            }
            else
            if (ShopType == DDShop.white4)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<白色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop4") + ")" });
            }
            else
            if (ShopType == DDShop.white5)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<白色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop5") + ")" });
            }
            else if (ShopType == DDShop.green1)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<绿色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop1") + ")" });
            }
            else if (ShopType == DDShop.green2)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<绿色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop2") + ")" });
            }
            else if (ShopType == DDShop.green3)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<绿色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop3") + ")" });
            }
            else if (ShopType == DDShop.green4)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<绿色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop4") + ")" });
            }
            else if (ShopType == DDShop.green5)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<绿色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop5") + ")" });
            }
            else if (ShopType == DDShop.blue1)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<蓝色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop1") + ")" });
            }
            else if (ShopType == DDShop.blue2)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<蓝色委托币>()).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop2") + ")" });
            }
            else if (ShopType == DDShop.blue3)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<蓝色委托币>()).Name , "("+ Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop3") + ")" });
            }
            else if (ShopType == DDShop.blue4)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<蓝色委托币>()).Name , "("+ Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop4") + ")" });
            }
            else if (ShopType == DDShop.blue5)
            {
                button = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ModContent.ItemType<蓝色委托币>()).Name , "("+ Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop5") + ")" });
            }
            button2 = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options2");
        }

        public static DDShop ShopType =  0;
        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = ShopType.ToString();
            }
            else
            {
                DDUISystem.SelectStore = true;
            }
        }
        public override void AddShops()
        {
            // Boss前,刚进入接任务阶段
            Condition condition = new Condition(Language.GetTextValue("Mods.DDmod.NPCs.Shop.Condition1"), () => Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel >= 1);
            // Boss前,有一定的冒险经历后
            Condition condition2 = new Condition(Language.GetTextValue("Mods.DDmod.NPCs.Shop.Condition2"), () => Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel >= 2);
            // 史莱姆王后
            Condition condition3 = new Condition(Language.GetTextValue("Mods.DDmod.NPCs.Shop.Condition3"), () => Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel >= 3);
            // 世界吞噬者/脑后
            Condition condition4 = new Condition(Language.GetTextValue("Mods.DDmod.NPCs.Shop.Condition4"), () => Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel >= 4);
            // 骷髅王后
            Condition condition5 = new Condition(Language.GetTextValue("Mods.DDmod.NPCs.Shop.Condition5"), () => Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel >= 5);
            // 肉后
            Condition condition6 = new Condition(Language.GetTextValue("Mods.DDmod.NPCs.Shop.Condition6"), () => Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel >= 6);
            // 三王后
            Condition condition7 = new Condition(Language.GetTextValue("Mods.DDmod.NPCs.Shop.Condition7"), () => Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel >= 7);
            // 花后
            Condition condition8 = new Condition(Language.GetTextValue("Mods.DDmod.NPCs.Shop.Condition8"), () => Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel >= 8);
            NPCShop shop = new NPCShop(NPC.type,DDShop.white1.ToString());


            //白色币
            //武器
            shop.Commodity(ModContent.ItemType<白色委托币>(), 1, DDSystem.AdventureCoins2);
            shop.Commodity(ModContent.ItemType<白色委托币>(), 5000, -1);
            shop.Commodity(4281, 10, DDSystem.AdventureCoins,condition);
            shop.Commodity(280, 10, DDSystem.AdventureCoins,condition);
            shop.Commodity(281, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(284, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(946, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(3069, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<叶木双剑>(), 10, DDSystem.AdventureCoins, condition);

            shop.Commodity(4764, 15, DDSystem.AdventureCoins, condition);

            shop.Commodity(5011, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(55, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(930, 15, DDSystem.AdventureCoins, condition);

            shop.Commodity(ModContent.ItemType<飞鱼镖>(), 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(1166, 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(96, 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(162, 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(64, 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(800, 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(802, 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(1256, 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(4463, 15, DDSystem.AdventureCoins, condition2);

            shop.Commodity(670, 20, DDSystem.AdventureCoins, condition);
            shop.Commodity(724, 20, DDSystem.AdventureCoins, condition);
            shop.Commodity(1319, 20, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<寒霜飞刀>(), 20, DDSystem.AdventureCoins, condition);

            shop.Commodity(186, 12, DDSystem.AdventureCoins, condition);
            shop.Commodity(277, 25, DDSystem.AdventureCoins, condition);

            shop.Commodity(213, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(964, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(989, 25, DDSystem.AdventureCoins, condition3);

            shop.Commodity(4061, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(4062, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(65, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(ModContent.ItemType<星空>(), 25, DDSystem.AdventureCoins, condition3);

            shop.Commodity(ModContent.ItemType<樱之弓>(), 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(ModContent.ItemType<白切精华>(), 40, DDSystem.AdventureCoins, condition3);


            shop.Register();
            //护甲/法宝
            shop = new NPCShop(NPC.type, DDShop.white2.ToString());
            shop.Commodity(ModContent.ItemType<白色委托币>(), 1, DDSystem.AdventureCoins2);
            shop.Commodity(ModContent.ItemType<白色委托币>(), 5000, -1);
            shop.Commodity(954, 8, DDSystem.AdventureCoins, condition);
            shop.Commodity(955, 12, DDSystem.AdventureCoins, condition);
            shop.Commodity(879, 12, DDSystem.AdventureCoins, condition);
            shop.Commodity(3109, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(238, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(88, 12, DDSystem.AdventureCoins, condition);
            shop.Commodity(410, 12, DDSystem.AdventureCoins, condition);
            shop.Commodity(411, 12, DDSystem.AdventureCoins, condition);
            shop.Commodity(1135, 8, DDSystem.AdventureCoins, condition);
            shop.Commodity(1136, 8, DDSystem.AdventureCoins, condition);
            shop.Commodity(3187, 28, DDSystem.AdventureCoins, condition);
            shop.Commodity(3188, 28, DDSystem.AdventureCoins, condition);
            shop.Commodity(3189, 28, DDSystem.AdventureCoins, condition);
            shop.Commodity(803, 20, DDSystem.AdventureCoins, condition);
            shop.Commodity(804, 20, DDSystem.AdventureCoins, condition);
            shop.Commodity(805, 20, DDSystem.AdventureCoins, condition);
            shop.Commodity(960, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(961, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(962, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(ModContent.ItemType<奇异的云>(), 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<WoodSpiritSwordItem>(), 25, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<永恒冰晶>(), 30, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<宣花葫芦>(), 10, DDSystem.AdventureCoins, condition2);
            shop.Register();

            //饰品
            shop = new NPCShop(NPC.type, DDShop.white3.ToString());
            shop.Commodity(ModContent.ItemType<白色委托币>(), 1, DDSystem.AdventureCoins2);
            shop.Commodity(ModContent.ItemType<白色委托币>(), 5000, -1);
            shop.Commodity(216, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(285, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(953, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(3068, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(3084, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(4341, 10, DDSystem.AdventureCoins, condition);

            shop.Commodity(18, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(393, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(49, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(53, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(54, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(975, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(3199, 15, DDSystem.AdventureCoins, condition);

            shop.Commodity(187, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(863, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(4404, 15, DDSystem.AdventureCoins, condition);

            shop.Commodity(158, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(159, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(2219, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(4978, 30, DDSystem.AdventureCoins, condition);

            shop.Commodity(111, 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(1290, 15, DDSystem.AdventureCoins, condition2);

            shop.Commodity(950, 20, DDSystem.AdventureCoins, condition);
            shop.Commodity(987, 20, DDSystem.AdventureCoins, condition);
            shop.Commodity(1579, 20, DDSystem.AdventureCoins, condition);

            shop.Commodity(212, 25, DDSystem.AdventureCoins,condition3);
            shop.Commodity(211, 25, DDSystem.AdventureCoins,condition3);
            shop.Commodity(3017, 25, DDSystem.AdventureCoins,condition3);

            shop.Commodity(4055, 25, DDSystem.AdventureCoins,condition3);
            shop.Commodity(4056, 25, DDSystem.AdventureCoins,condition3);

            shop.Register();

            //药剂
            shop = new NPCShop(NPC.type,DDShop.white4.ToString());
            shop.Commodity(ModContent.ItemType<白色委托币>(), 1, DDSystem.AdventureCoins2);
            shop.Commodity(ModContent.ItemType<白色委托币>(), 5000, -1);
            shop.Commodity(28, 1, DDSystem.AdventureCoins, condition);
            shop.Commodity(110, 1, DDSystem.AdventureCoins, condition);
            shop.Commodity(2350, 1, DDSystem.AdventureCoins, condition);
            shop.Commodity(289, 1, DDSystem.AdventureCoins, condition);
            shop.Commodity(290, 1, DDSystem.AdventureCoins, condition);
            shop.Commodity(292, 1, DDSystem.AdventureCoins, condition);
            shop.Commodity(298, 1, DDSystem.AdventureCoins, condition);
            shop.Commodity(299, 1, DDSystem.AdventureCoins, condition);
            shop.Commodity(2325, 2, DDSystem.AdventureCoins, condition);
            shop.Commodity(297, 2, DDSystem.AdventureCoins, condition);
            shop.Commodity(291, 3, DDSystem.AdventureCoins, condition);
            shop.Commodity(295, 3, DDSystem.AdventureCoins, condition);
            shop.Commodity(301, 3, DDSystem.AdventureCoins, condition);
            shop.Commodity(302, 3, DDSystem.AdventureCoins, condition);
            shop.Commodity(304, 3, DDSystem.AdventureCoins, condition);
            shop.Commodity(2327, 3, DDSystem.AdventureCoins, condition);
            shop.Commodity(2329, 3, DDSystem.AdventureCoins, condition);
            shop.Commodity(293, 5, DDSystem.AdventureCoins, condition);
            shop.Commodity(294, 5, DDSystem.AdventureCoins, condition);
            shop.Commodity(296, 5, DDSystem.AdventureCoins, condition);
            shop.Commodity(300, 5, DDSystem.AdventureCoins, condition);
            shop.Commodity(303, 5, DDSystem.AdventureCoins, condition);
            shop.Commodity(305, 5, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<弱效飞翔药水>(), 6, DDSystem.AdventureCoins, condition);
            shop.Commodity(4477, 10, DDSystem.AdventureCoins, condition);
            shop.Register();

            //杂物
            shop = new NPCShop(NPC.type, DDShop.white5.ToString());
            shop.Commodity(ModContent.ItemType<白色委托币>(), 1, DDSystem.AdventureCoins2);
            shop.Commodity(ModContent.ItemType<白色委托币>(), 5000, -1);
            shop.Commodity(ModContent.ItemType<StrengthenPlatformItem>(), 50, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<重铸币>(), 5, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<StrengtheningStone>(), 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<金毛鸟蛋>(), 50, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<魔眼蛋>(), 100, DDSystem.AdventureCoins, condition);
            shop.Commodity(3093, 6, DDSystem.AdventureCoins, condition);
            shop.Commodity(4345, 6, DDSystem.AdventureCoins, condition);
            shop.Commodity(29, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(109, 10, DDSystem.AdventureCoins, condition);
            shop.Commodity(50, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(997, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(75, 2, DDSystem.AdventureCoins, condition);
            shop.Commodity(2198, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(859, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(2197, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(ModContent.ItemType<潮汐之泪>(), 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(4271, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(361, 15, DDSystem.AdventureCoins, condition);
            shop.Commodity(115, 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(3062, 15, DDSystem.AdventureCoins, condition2);
            shop.Commodity(2292, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(2204, 25, DDSystem.AdventureCoins, condition3);

            shop.Commodity(4262, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(4263, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(4276, 25, DDSystem.AdventureCoins, condition3);
            shop.Commodity(5391, 25, DDSystem.AdventureCoins, condition3);
            shop.Register();

            //绿色币
            //武器
            shop = new NPCShop(NPC.type, DDShop.green1.ToString());
            shop.Commodity(ModContent.ItemType<绿色委托币>(), 1, DDSystem.AdventureCoins3);
            shop.Commodity(ModContent.ItemType<绿色委托币>(), 50000, -1);
            shop.Commodity(272, 25, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(165, 12, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(3317, 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(157, 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(164, 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(113, 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(163, 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(155, 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(ModContent.ItemType<远古短刀>(), 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(ModContent.ItemType<远古弓>(), 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(220, 30, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(274, 30, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(218, 30, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(112, 30, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(3019, 30, DDSystem.AdventureCoins2, condition5);
            shop.Register();
            //护甲/法宝
            shop = new NPCShop(NPC.type, DDShop.green2.ToString());
            shop.Commodity(ModContent.ItemType<绿色委托币>(), 1, DDSystem.AdventureCoins3);
            shop.Commodity(ModContent.ItemType<绿色委托币>(), 50000, -1);
            shop.Commodity(959,10, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(ModContent.ItemType<玉净瓶>(),20, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(ModContent.ItemType<地狱葫芦>(), 30, DDSystem.AdventureCoins2, condition5);
            shop.Register();
            //饰品
            shop = new NPCShop(NPC.type, DDShop.green3.ToString());
            shop.Commodity(ModContent.ItemType<绿色委托币>(), 1, DDSystem.AdventureCoins3);
            shop.Commodity(ModContent.ItemType<绿色委托币>(), 50000, -1);
            shop.Commodity(3212, 10, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(906, 10, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(223, 10, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(887, 10, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(1303, 10, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(1322, 20, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(1323, 20, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(891, 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(3095, 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(156, 20, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(5010, 12, DDSystem.AdventureCoins2, condition5);
            shop.Register();

            //药剂
            shop = new NPCShop(NPC.type, DDShop.green4.ToString());
            shop.Commodity(ModContent.ItemType<绿色委托币>(), 1, DDSystem.AdventureCoins3);
            shop.Commodity(ModContent.ItemType<绿色委托币>(), 50000, -1);
            shop.Commodity(188, 1, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(189, 1, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(288, 3, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(4870, 5, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2322, 2, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2323, 2, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2324, 2, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2354, 2, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2355, 2, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2356, 2, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2359, 2, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2326, 5, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2328, 5, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2344, 5, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2346, 5, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2347, 5, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2348, 5, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2349, 5, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(5211, 5, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(ModContent.ItemType<战争药水>(), 8, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(4478, 10, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2345, 15, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2351, 20, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2352, 10, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2353, 10, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(2756, 30, DDSystem.AdventureCoins2, condition4);
            shop.Register();
            //杂物
            shop = new NPCShop(NPC.type, DDShop.green5.ToString());
            shop.Commodity(ModContent.ItemType<绿色委托币>(), 1, DDSystem.AdventureCoins3);
            shop.Commodity(ModContent.ItemType<绿色委托币>(), 50000, -1);
            shop.Commodity(ModContent.ItemType<优秀重铸币>(), 5, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(ModContent.ItemType<StrengtheningStone2>(), 10, DDSystem.AdventureCoins2, condition4);
            shop.Commodity(327, 4, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(932, 10, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(2192, 10, DDSystem.AdventureCoins2, condition5);
            shop.Commodity(329, 15, DDSystem.AdventureCoins2, condition5);
            shop.Register();


            //蓝色币
            //武器
            shop = new NPCShop(NPC.type, DDShop.blue1.ToString());
            shop.Commodity(ModContent.ItemType<蓝色委托币>(), 500000, -1);
            shop.Commodity(676, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(1306, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(1314, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(1324, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(1264, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(726, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(1244, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(1264, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(1308, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(725, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(1265, 25, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(517, 12, DDSystem.AdventureCoins3, condition6);

            shop.Commodity(1327, 25, DDSystem.AdventureCoins3, condition7);
            shop.Commodity(683, 18, DDSystem.AdventureCoins3, condition7);
            shop.Commodity(3107, 25, DDSystem.AdventureCoins3, condition7);
            shop.Commodity(ModContent.ItemType<迷你鲨2>(), 100, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(ModContent.ItemType<手枪2>(), 50, DDSystem.AdventureCoins3, condition7);


            shop.Commodity(1513, 40, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(4789, 40, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(1266, 40, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(1444, 40, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(1445, 40, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(1446, 40, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(759, 40, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(1254, 40, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(4679, 40, DDSystem.AdventureCoins3, condition8);


            shop.Commodity(1569, 100, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(1571, 100, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(1260, 100, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(1156, 100, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(1572, 100, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(4607, 100, DDSystem.AdventureCoins3, condition8);
            shop.Register();
            //护甲/法宝
            shop = new NPCShop(NPC.type, DDShop.blue2.ToString());
            shop.Commodity(ModContent.ItemType<蓝色委托币>(), 500000, -1);
            shop.Commodity(ModContent.ItemType<HunyuanPearlUmbrellaItem>(), 30, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(ModContent.ItemType<Items.Talisman.药王葫芦>(), 40, DDSystem.AdventureCoins3, condition7);
            shop.Register();
            //饰品
            shop = new NPCShop(NPC.type, DDShop.blue3.ToString());
            shop.Commodity(ModContent.ItemType<蓝色委托币>(), 500000, -1);
            shop.Commodity(485, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(497, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(532, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(535, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(536, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(554, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(900, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(885, 12, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(886, 12, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(888, 12, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(889, 12, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(890, 12, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(892, 12, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(893, 12, DDSystem.AdventureCoins3, condition6);

            shop.Commodity(1321, 20, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(1253, 20, DDSystem.AdventureCoins3, condition6);


            shop.Commodity(977, 40, DDSystem.AdventureCoins3, condition8);
            shop.Commodity(1300, 40, DDSystem.AdventureCoins3, condition8);


            shop.Register();
            //药剂
            shop = new NPCShop(NPC.type, DDShop.blue4.ToString());
            shop.Commodity(ModContent.ItemType<蓝色委托币>(), 500000, -1);
            shop.Commodity(499, 1, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(500, 1, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2326, 3, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2328, 3, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2344, 3, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2346, 3, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2347, 3, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2348, 3, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2349, 3, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(5211, 3, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2345, 8, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(ModContent.ItemType<飞翔药水>(), 8, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(ModContent.ItemType<滞空药水>(), 10, DDSystem.AdventureCoins3, condition7);
            shop.Commodity(ModContent.ItemType<纷争药水>(), 10, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2351, 10, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(4479, 15, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2756, 15, DDSystem.AdventureCoins3, condition6);
            shop.Register();
            //杂物
            shop = new NPCShop(NPC.type, DDShop.blue5.ToString());
            shop.Commodity(ModContent.ItemType<蓝色委托币>(), 500000, -1);
            shop.Commodity(ModContent.ItemType<完美重铸币>(), 5, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(ModContent.ItemType<StrengtheningStone3>(), 10, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(527, 2, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(528, 2, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(2161, 6, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(3783, 6, DDSystem.AdventureCoins3, condition6);
            shop.Commodity(1315, 20, DDSystem.AdventureCoins3, condition6);
            shop.Register();

        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            if (Main.hardMode)
            {
                damage = 100;
            }
            else
            {
                damage = 50;
            }
            knockback = 0;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 10;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = 0;
            if (Main.hardMode)
            {
                attackDelay = 10;
            }
            else
            {
                attackDelay = 30;
            }
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 14f;
            gravityCorrection = 16f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (SWSystem.TrueSubworld || Main.LocalPlayer.talkNPC == NPC.whoAmI|| CText>0)
            {
                CText--;
                string Text = "";
                //string Text = DDSystem.English? "Warriors, you can!" : "上啊勇士,你可以的!";
                if (Main.LocalPlayer.GetModPlayer<SWPlayer>().SWDead)
                {
                    //Text = DDSystem.English ? "" : "菜就多练练";
                }
                if(CText>0)
                {
                    Text = CText2;
                }
                if (Main.LocalPlayer.talkNPC == NPC.whoAmI)
                {
                    Text = Main.npcChatText;
                }
                if(Text=="")
                {
                    return true;
                }
                if(Text.Length>TL)
                {
                    TLTime++;
                    if(TLTime%5==0)
                    {
                        TL++;
                    }
                }
                else if (Text.Length < TL)
                {
                    TL = 0;
                    TLTime = 0;
                }
                Text = Text.Remove(TL);
                SpriteEffects spriteEffects = (SpriteEffects)1;
                if (NPC.direction == 1)
                {
                    spriteEffects = 0;
                }
                NPC.NPCText(Text);
            }
            else
            {
                TL = 0;
                TLTime = 0;
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        int TL = 0;
        int TLTime = 0;
    }
    //白色委托币
    public class AdventureCoins : CustomCurrencySingleCoin
    {
        public AdventureCoins(int coinItemID1, long currencyCap) : base(coinItemID1, currencyCap)
        {
            Include(coinItemID1, 10000000);
            SetCurrencyCap(currencyCap);
        }
        public override long CountCurrency(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
        {
            long num = Main.LocalPlayer.Dplayer().AdventureCoins;

            overFlowing = false;
            return num;
        }
        public override bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
        {
            if (Main.LocalPlayer.Dplayer().AdventureCoins >= price)
            {
                Main.LocalPlayer.Dplayer().AdventureCoins -= (int)price;
                return true;
            }
            return false;
        }
        public override void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
        {
            Texture2D texture2D = TextureAssets.Item[ModContent.ItemType<白色委托币>()].Value;
            if (horizontal)
            {
                Vector2 position = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).X + 45f, shopy + 50f);
                sb.Draw(texture2D, position, null, Color.White, 0f, texture2D.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, "" + Main.LocalPlayer.Dplayer().AdventureCoins, position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
            else
            {
                int num2 = (totalCoins > 99) ? (-6) : 0;
                sb.Draw(texture2D, new Vector2(shopx + 11f, shopy + 75f), null, Color.White, 0f, texture2D.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, "" + Main.LocalPlayer.Dplayer().AdventureCoins, shopx + (float)num2, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
        }
        public override void GetPriceText(string[] lines, ref int currentLine, long price)
        {
            int A = ModContent.ItemType<白色委托币>();
            Color color = coinColor * (Main.mouseTextColor / 255f);
            
            lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3}{4}{5}]", new object[]
            {
                color.R,
                color.G,
                color.B,
                Language.GetTextValue("LegacyTooltip.50"),
                price,
                // "][i: " + A,
                new Item(A).Name
            });
            //lines[currentLine++] = string.Format("[c/01FF01:我是绿色]");
        }

        public Color coinColor = new Color(255, 255, 255);
    }
    //绿色委托币
    public class AdventureCoins2 : CustomCurrencySingleCoin
    {
        public AdventureCoins2(int coinItemID1, long currencyCap) : base(coinItemID1, currencyCap)
        {
            Include(coinItemID1, 10000000);
            SetCurrencyCap(currencyCap);
        }
        public override long CountCurrency(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
        {
            long num = Main.LocalPlayer.Dplayer().AdventureCoins2;

            overFlowing = false;
            return num;
        }
        public override bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
        {
            if (Main.LocalPlayer.Dplayer().AdventureCoins2 >= price)
            {
                Main.LocalPlayer.Dplayer().AdventureCoins2 -= (int)price;
                return true;
            }
            return false;
        }
        public override void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
        {
            Texture2D texture2D = TextureAssets.Item[ModContent.ItemType<绿色委托币>()].Value;
            if (horizontal)
            {
                Vector2 position = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).X + 45f, shopy + 50f);
                sb.Draw(texture2D, position, null, Color.White, 0f, texture2D.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, "" + Main.LocalPlayer.Dplayer().AdventureCoins2, position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
            else
            {
                int num2 = (totalCoins > 99) ? (-6) : 0;
                sb.Draw(texture2D, new Vector2(shopx + 11f, shopy + 75f), null, Color.White, 0f, texture2D.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, "" + Main.LocalPlayer.Dplayer().AdventureCoins2, shopx + (float)num2, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
        }
        public override void GetPriceText(string[] lines, ref int currentLine, long price)
        {
            int A = ModContent.ItemType<绿色委托币>();
            Color color = coinColor * (Main.mouseTextColor / 255f);
            
            lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3}{4}{5}]", new object[]
            {
                color.R,
                color.G,
                color.B,
                Language.GetTextValue("LegacyTooltip.50"),
                price,
                // "][i: " + A,
                new Item(A).Name
            });
            //lines[currentLine++] = string.Format("[c/01FF01:我是绿色]");
        }

        public Color coinColor = new Color(100, 255, 100);
    }
    //蓝色委托币
    public class AdventureCoins3 : CustomCurrencySingleCoin
    {
        public AdventureCoins3(int coinItemID1, long currencyCap) : base(coinItemID1, currencyCap)
        {
            Include(coinItemID1, 10000000);
            SetCurrencyCap(currencyCap);
        }
        public override long CountCurrency(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
        {
            long num = Main.LocalPlayer.Dplayer().AdventureCoins3;

            overFlowing = false;
            return num;
        }
        public override bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
        {
            if (Main.LocalPlayer.Dplayer().AdventureCoins3 >= price)
            {
                Main.LocalPlayer.Dplayer().AdventureCoins3 -= (int)price;
                return true;
            }
            return false;
        }
        public override void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
        {
            Texture2D texture2D = TextureAssets.Item[ModContent.ItemType<蓝色委托币>()].Value;
            if (horizontal)
            {
                Vector2 position = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).X + 45f, shopy + 50f);
                sb.Draw(texture2D, position, null, Color.White, 0f, texture2D.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, "" + Main.LocalPlayer.Dplayer().AdventureCoins3, position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
            else
            {
                int num2 = (totalCoins > 99) ? (-6) : 0;
                sb.Draw(texture2D, new Vector2(shopx + 11f, shopy + 75f), null, Color.White, 0f, texture2D.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, "" + Main.LocalPlayer.Dplayer().AdventureCoins3, shopx + (float)num2, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
        }
        public override void GetPriceText(string[] lines, ref int currentLine, long price)
        {
            int A = ModContent.ItemType<蓝色委托币>();
            Color color = coinColor * (Main.mouseTextColor / 255f);
            
            lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3}{4}{5}]", new object[]
            {
                color.R,
                color.G,
                color.B,
                Language.GetTextValue("LegacyTooltip.50"),
                price,
                // "][i: " + A,
                new Item(A).Name
            });
            //lines[currentLine++] = string.Format("[c/01FF01:我是绿色]");
        }

        public Color coinColor = new Color(0, 150, 255);
    }
    //紫色委托币
    public class AdventureCoins4 : CustomCurrencySingleCoin
    {
        public AdventureCoins4(int coinItemID1, long currencyCap) : base(coinItemID1, currencyCap)
        {
            Include(coinItemID1, 10000000);
            SetCurrencyCap(currencyCap);
        }
        public override long CountCurrency(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
        {
            long num = Main.LocalPlayer.Dplayer().AdventureCoins4;

            overFlowing = false;
            return num;
        }
        public override bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
        {
            if (Main.LocalPlayer.Dplayer().AdventureCoins4 >= price)
            {
                Main.LocalPlayer.Dplayer().AdventureCoins4 -= (int)price;
                return true;
            }
            return false;
        }
        public override void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
        {
            Texture2D texture2D = TextureAssets.Item[ModContent.ItemType<紫色委托币>()].Value;
            if (horizontal)
            {
                Vector2 position = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).X + 45f, shopy + 50f);
                sb.Draw(texture2D, position, null, Color.White, 0f, texture2D.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, "" + Main.LocalPlayer.Dplayer().AdventureCoins4, position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
            else
            {
                int num2 = (totalCoins > 99) ? (-6) : 0;
                sb.Draw(texture2D, new Vector2(shopx + 11f, shopy + 75f), null, Color.White, 0f, texture2D.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, "" + Main.LocalPlayer.Dplayer().AdventureCoins4, shopx + (float)num2, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
        }
        public override void GetPriceText(string[] lines, ref int currentLine, long price)
        {
            int A = ModContent.ItemType<紫色委托币>();
            Color color = coinColor * (Main.mouseTextColor / 255f);
            
            lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3}{4}{5}]", new object[]
            {
                color.R,
                color.G,
                color.B,
                Language.GetTextValue("LegacyTooltip.50"),
                price,
                // "][i: " + A,
                new Item(A).Name
            });
            //lines[currentLine++] = string.Format("[c/01FF01:我是绿色]");
        }

        public Color coinColor = new Color(180, 0, 255);
    }
    //橙色委托币
    public class AdventureCoins5 : CustomCurrencySingleCoin
    {
        public AdventureCoins5(int coinItemID1, long currencyCap) : base(coinItemID1, currencyCap)
        {
            Include(coinItemID1, 10000000);
            SetCurrencyCap(currencyCap);
        }
        public override long CountCurrency(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
        {
            long num = Main.LocalPlayer.Dplayer().AdventureCoins5;

            overFlowing = false;
            return num;
        }
        public override bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
        {
            if (Main.LocalPlayer.Dplayer().AdventureCoins5 >= price)
            {
                Main.LocalPlayer.Dplayer().AdventureCoins5 -= (int)price;
                return true;
            }
            return false;
        }
        public override void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
        {
            Texture2D texture2D = TextureAssets.Item[ModContent.ItemType<橙色委托币>()].Value;
            if (horizontal)
            {
                Vector2 position = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).X + 45f, shopy + 50f);
                sb.Draw(texture2D, position, null, Color.White, 0f, texture2D.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, "" + Main.LocalPlayer.Dplayer().AdventureCoins5, position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
            else
            {
                int num2 = (totalCoins > 99) ? (-6) : 0;
                sb.Draw(texture2D, new Vector2(shopx + 11f, shopy + 75f), null, Color.White, 0f, texture2D.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
                Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, "" + Main.LocalPlayer.Dplayer().AdventureCoins5, shopx + (float)num2, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
            }
        }
        public override void GetPriceText(string[] lines, ref int currentLine, long price)
        {
            int A = ModContent.ItemType<橙色委托币>();
            Color color = coinColor * (Main.mouseTextColor / 255f);
            
            lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3}{4}{5}]", new object[]
            {
                color.R,
                color.G,
                color.B,
                Language.GetTextValue("LegacyTooltip.50"),
                price,
                // "][i: " + A,
                new Item(A).Name
            });
            //lines[currentLine++] = string.Format("[c/01FF01:我是绿色]");
        }

        public Color coinColor = new Color(255, 150, 0);
    }
}