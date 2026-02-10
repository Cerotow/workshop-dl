using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Content.NPCs.Boss.先祖咒魂;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.DDOn;
using Stubble.Core.Classes;
using Terraria.Chat;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Filters = Terraria.Graphics.Effects.Filters;

namespace DDmod.Worlds
{
    public class NPCDowned : ModSystem
    {
        //NPC击败判定

        /// <summary> 生命守卫击败判定 </summary>
        public static bool downedLifeGuard = false;
        /// <summary> 第二次击败生命守卫 </summary>
        public static bool downedLifeGuard2 = false;
        /// <summary> 第三次击败生命守卫 </summary>
        public static bool downedLifeGuard3 = false;

        /// <summary> 星辰守卫击败判定 </summary>
        public static bool downedStarGuard = false;
        /// <summary> 第二次击败星辰守卫 </summary>
        public static bool downedStarGuard2 = false;
        /// <summary> 第三次击败星辰守卫 </summary>
        public static bool downedStarGuard3 = false;

        /// <summary> 流星掘地者击败判定 </summary>
        public static bool downedMeteorDigger = false;
        /// <summary> 橡果之灵击败判定 </summary>
        public static bool downedWitheredAcornSpirit = false;
        /// <summary> 流星歼灭者击败判定 </summary>
        public static bool downedMeteorAnnihilator = false;
        /// <summary> 哥布林军团可召唤哥布林巫师首领判定 </summary>
        public static bool GoblinLegion = false;
        /// <summary> 哥布林军团可召唤真哥布林术士判定 </summary>
        public static bool GoblinWarlock = false;
        public static bool GoblinWarlock2 = false;
        /// <summary> 哥布林巫师首领生成计时器 </summary>
        public static int GoblinLegionTime;
        /// <summary> 哥布林巫师首领生成计时器 </summary>
        public static bool GoblinLegionText;
        /// <summary> 击败哥布林巫师首领判定 </summary>
        public static bool downedGoblinSorcererChieftain = false;
        /// <summary>
        /// 流星塔生成
        /// </summary>
        public static bool MeteorTower = false;

        public static bool 欲火蛇 = false;
        public static bool 夜光蘑菇王 = false;
        public static bool 蘑菇王 = false;
        public static bool 超级蓝史莱姆 = false;
        public static bool 矿洞幽魂 = false;
        public static bool 远古幽魂 = false;
        public static bool 恐惧缝合体 = false;
        public static bool 鬼牙 = false;
        public static bool 绿岩之视 = false;
        public static bool 海幽浮王 = false;
        public static bool 觉醒星心双子 = false;
        public static bool 星心对话 = false;
        public static bool 星心守卫 = false;
        public static bool 邪神史莱姆 = false;
        public static bool 树妖 = false;
        public static bool 丛林暴食怪 = false;
        public static bool 炼狱头颅 = false;
        public static bool 突变喀迈拉 = false;
        public static bool 突变噬魂怪 = false;
        public static bool 妖精王 = false;
        public static bool 流星破坏者 = false;
        public static bool 天雷怒云 = false;
        public static bool 先祖咒魂 = false;
        public static bool 克苏鲁心脏 = false;
        public static bool 日耀护卫 = false;
        public static bool 星旋护卫 = false;
        public static bool 星云护卫 = false;
        public static bool 星尘护卫 = false;

        /// <summary>
        /// 绿岩刷怪
        /// </summary>
        public static bool 绿岩刷怪 = false;
        public override void Load()
        {
        }
        public override void OnWorldLoad()
        {
            downedLifeGuard = false;
            downedLifeGuard2 = false;
            downedLifeGuard3 = false;
            downedStarGuard = false;
            downedStarGuard2 = false;
            downedStarGuard3 = false;
            downedMeteorDigger = false;
            downedWitheredAcornSpirit = false;
            downedMeteorAnnihilator = false;
            GoblinWarlock = false;
            GoblinWarlock2 = false;
            GoblinLegion = false;
            GoblinLegionText = false;
            downedGoblinSorcererChieftain = false;
            MeteorTower = false;
            欲火蛇 = false;
            夜光蘑菇王 = false;
            蘑菇王 = false;
            超级蓝史莱姆 = false;
            矿洞幽魂 = false;
            远古幽魂 = false;
            恐惧缝合体 = false;
            鬼牙 = false;
            绿岩之视 = false;
            海幽浮王 = false;
            觉醒星心双子 = false;
            星心对话 = false;
            星心守卫 = false;
            邪神史莱姆 = false;
            树妖 = false;
            丛林暴食怪 = false;
            炼狱头颅 = false;
            突变喀迈拉 = false;
            突变噬魂怪 = false;
            妖精王 = false;
            流星破坏者 = false;
            天雷怒云 = false;
            绿岩刷怪 = false;
            先祖咒魂 = false;
            克苏鲁心脏 = false;
            日耀护卫 = false;
            星旋护卫 = false;
            星云护卫 = false;
            星尘护卫 = false;
        }
        public override void OnWorldUnload()
        {
            OnWorldLoad();
        }
        public override void SaveWorldData(TagCompound tag)
        {
            if (downedLifeGuard) tag["downedLifeGuard"] = true;
            if (downedLifeGuard2) tag["downedLifeGuard2"] = true;
            if (downedLifeGuard3) tag["downedLifeGuard3"] = true;
            if (downedStarGuard) tag["downedStarGuard"] = true;
            if (downedStarGuard2) tag["downedStarGuard2"] = true;
            if (downedStarGuard3) tag["downedStarGuard3"] = true;
            if (downedMeteorDigger) tag["downedMeteorDigger"] = true;
            if (downedWitheredAcornSpirit) tag["downedWitheredAcornSpirit"] = true;
            if (downedMeteorAnnihilator) tag["downedMeteorAnnihilator"] = true;
            if (GoblinWarlock) tag["GoblinWarlock"] = true;
            if (GoblinWarlock2) tag["GoblinWarlock2"] = true;
            if (GoblinLegion) tag["GoblinLegion"] = true;
            if (GoblinLegionText) tag["GoblinLegionText"] = true;
            if (downedGoblinSorcererChieftain) tag["downedGoblinSorcererChieftain"] = true;
            if (欲火蛇) tag["downed欲火蛇"] = true;
            if (夜光蘑菇王) tag["downed夜光蘑菇王"] = true;
            if (蘑菇王) tag["downed蘑菇王"] = true;
            if (超级蓝史莱姆) tag["downed超级蓝史莱姆"] = true;
            if (矿洞幽魂)tag["downed洞穴幽魂"] = true;
            if (远古幽魂) tag["downed远古幽魂"] = true;
            if (恐惧缝合体) tag["downed恐惧缝合体"] = true;
            if (鬼牙) tag["downed鬼牙"] = true;
            if (绿岩之视) tag["downed绿岩之视"] = true;
            if (海幽浮王) tag["downed海幽浮王"] = true;
            if (觉醒星心双子) tag["downed觉醒星心双子"] = true;
            if (星心对话) tag["downed星心对话"] = true;
            if (星心守卫) tag["downed星心守卫"] = true;
            if (邪神史莱姆) tag["downed邪神史莱姆"] = true;
            if (树妖) tag["downed树妖"] = true;
            if (丛林暴食怪) tag["downed丛林暴食怪"] = true;
            if (炼狱头颅) tag["downed炼狱头颅"] = true;
            if (突变喀迈拉) tag["downed突变喀迈拉"] = true;
            if (突变噬魂怪) tag["downed突变噬魂怪"] = true;
            if (妖精王) tag["downed妖精王"] = true;
            if (流星破坏者) tag["downed流星破坏者"] = true;
            if (天雷怒云) tag["downed天雷怒云"] = true;
            if (先祖咒魂) tag["downed先祖咒魂"] = true;
            if (克苏鲁心脏) tag["downed克苏鲁心脏"] = true;
            if (日耀护卫) tag["downed日耀护卫"] = true;
            if (星旋护卫) tag["downed星旋护卫"] = true;
            if (星云护卫) tag["downed星云护卫"] = true;
            if (星尘护卫) tag["downed星尘护卫"] = true;
            if (绿岩刷怪) tag["绿岩刷怪"] = true;
            if (MeteorTower) tag["MeteorTower"] = true;
        }
        
        public override void LoadWorldData(TagCompound tag)
        {
            downedLifeGuard = tag.ContainsKey("downedLifeGuard");
            downedLifeGuard2 = tag.ContainsKey("downedLifeGuard2");
            downedLifeGuard3 = tag.ContainsKey("downedLifeGuard3");
            downedStarGuard = tag.ContainsKey("downedStarGuard");
            downedStarGuard2 = tag.ContainsKey("downedStarGuard2");
            downedStarGuard3 = tag.ContainsKey("downedStarGuard3");
            downedMeteorDigger = tag.ContainsKey("downedMeteorDigger");
            downedWitheredAcornSpirit = tag.ContainsKey("downedWitheredAcornSpirit");
            downedMeteorAnnihilator = tag.ContainsKey("downedMeteorAnnihilator");
            GoblinWarlock = tag.ContainsKey("GoblinWarlock");
            GoblinWarlock2 = tag.ContainsKey("GoblinWarlock2");
            GoblinLegion = tag.ContainsKey("GoblinLegion");
            GoblinLegionText = tag.ContainsKey("GoblinLegionText");
            downedGoblinSorcererChieftain = tag.ContainsKey("downedGoblinSorcererChieftain");
            欲火蛇 = tag.ContainsKey("downed欲火蛇");
            夜光蘑菇王 = tag.ContainsKey("downed夜光蘑菇王");
            蘑菇王 = tag.ContainsKey("downed蘑菇王");
            超级蓝史莱姆 = tag.ContainsKey("downed超级蓝史莱姆");
            矿洞幽魂 = tag.ContainsKey("downed洞穴幽魂");
            远古幽魂 = tag.ContainsKey("downed远古幽魂");
            恐惧缝合体 = tag.ContainsKey("downed恐惧缝合体");
            鬼牙 = tag.ContainsKey("downed鬼牙");
            绿岩之视 = tag.ContainsKey("downed绿岩之视");
            海幽浮王 = tag.ContainsKey("downed海幽浮王");
            觉醒星心双子 = tag.ContainsKey("downed觉醒星心双子");
            星心守卫 = tag.ContainsKey("downed星心守卫");
            星心对话 = tag.ContainsKey("downed星心对话");
            邪神史莱姆 = tag.ContainsKey("downed邪神史莱姆");
            树妖 = tag.ContainsKey("downed树妖");
            丛林暴食怪 = tag.ContainsKey("downed丛林暴食怪");
            炼狱头颅 = tag.ContainsKey("downed炼狱头颅");
            突变喀迈拉 = tag.ContainsKey("downed突变喀迈拉");
            突变噬魂怪 = tag.ContainsKey("downed突变噬魂怪");
            妖精王 = tag.ContainsKey("downed妖精王");
            流星破坏者 = tag.ContainsKey("downed流星破坏者");
            天雷怒云 = tag.ContainsKey("downed天雷怒云");
            先祖咒魂 = tag.ContainsKey("downed先祖咒魂");
            克苏鲁心脏 = tag.ContainsKey("downed克苏鲁心脏");
            日耀护卫 = tag.ContainsKey("downed日耀护卫");
            星旋护卫 = tag.ContainsKey("downed星旋护卫");
            星云护卫 = tag.ContainsKey("downed星云护卫");
            星尘护卫 = tag.ContainsKey("downed星尘护卫");
            绿岩刷怪 = tag.ContainsKey("绿岩刷怪");
            MeteorTower = tag.ContainsKey("MeteorTower");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedLifeGuard;
            flags[1] = downedLifeGuard2;
            flags[2] = downedLifeGuard3;
            flags[3] = downedStarGuard;
            flags[4] = downedStarGuard2;
            flags[5] = downedStarGuard3;
            flags[6] = downedMeteorDigger;
            flags[7] = downedWitheredAcornSpirit;
            writer.Write(flags);

            flags = new BitsByte();
            flags[0] = GoblinLegion;
            flags[1] = GoblinLegionText;
            flags[2] = downedGoblinSorcererChieftain;
            flags[3] = downedMeteorAnnihilator;
            flags[4] = 欲火蛇;
            flags[5] = 夜光蘑菇王;
            flags[6] = 蘑菇王;
            flags[7] = 超级蓝史莱姆;
            writer.Write(flags);

            flags = new BitsByte();
            flags[0] = 矿洞幽魂;
            flags[1] = 恐惧缝合体;
            flags[2] = 鬼牙;
            flags[3] = 绿岩之视;
            flags[4] = 海幽浮王;
            flags[5] = 觉醒星心双子;
            flags[6] = 星心守卫;
            flags[7] = 邪神史莱姆;
            writer.Write(flags);
            flags = new BitsByte();
            flags[0] = 树妖;
            flags[1] = 星心对话;
            flags[2] = 远古幽魂;
            flags[3] = 丛林暴食怪;
            flags[4] = 炼狱头颅;
            flags[5] = 突变喀迈拉;
            flags[6] = 突变噬魂怪;
            flags[7] = 妖精王;
            writer.Write(flags);
            flags = new BitsByte();
            flags[0] = 流星破坏者;
            flags[1] = 天雷怒云;
            flags[2] = 绿岩刷怪;
            flags[3] = 先祖咒魂;
            flags[4] = GoblinWarlock;
            flags[5] = GoblinWarlock2;
            flags[6] = 克苏鲁心脏;
            flags[7] = MeteorTower;
            writer.Write(flags);
            flags = new BitsByte();
            flags[0] = 日耀护卫;
            flags[1] = 星旋护卫;
            flags[2] = 星云护卫;
            flags[3] = 星尘护卫;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedLifeGuard = flags[0];
            downedLifeGuard2 = flags[1];
            downedLifeGuard3 = flags[2];
            downedStarGuard = flags[3];
            downedStarGuard2 = flags[4];
            downedStarGuard3 = flags[5];
            downedMeteorDigger = flags[6];
            downedWitheredAcornSpirit = flags[7];

            flags = reader.ReadByte();
            GoblinLegion = flags[0];
            GoblinLegionText = flags[1];
            downedGoblinSorcererChieftain = flags[2];
            downedMeteorAnnihilator = flags[3];
            欲火蛇 = flags[4];
            夜光蘑菇王 = flags[5];
            蘑菇王 = flags[6];
            超级蓝史莱姆 = flags[7];

            flags = reader.ReadByte();
            矿洞幽魂 = flags[0];
            恐惧缝合体 = flags[1];
            鬼牙 = flags[2];
            绿岩之视 = flags[3];
            海幽浮王 = flags[4];
            觉醒星心双子 = flags[5];
            星心守卫 = flags[6];
            邪神史莱姆 = flags[7];

            flags = reader.ReadByte();
            树妖 = flags[0];
            星心对话 = flags[1];
            远古幽魂 = flags[2];
            丛林暴食怪 = flags[3];
            炼狱头颅 = flags[4];
            突变喀迈拉 = flags[5];
            突变噬魂怪 = flags[6];
            妖精王 = flags[7];

            flags = reader.ReadByte();
            流星破坏者 = flags[0];
            天雷怒云 = flags[1];
            绿岩刷怪 = flags[2];
            先祖咒魂 = flags[3];
            GoblinWarlock = flags[4];
            GoblinWarlock2 = flags[5];
            克苏鲁心脏 = flags[6];
            MeteorTower = flags[7];

            flags = reader.ReadByte();
            日耀护卫 = flags[0];
            星旋护卫 = flags[1];
            星云护卫 = flags[2];
            星尘护卫 = flags[3];
        }
        public override void PreUpdateInvasions()
        {
            //哥布林入侵
            //GoblinLegionTime = 300;
            if (Main.invasionSize <= 0&&(GoblinLegion|| GoblinWarlock) && Main.invasionType == 1)
            {
                if (GoblinLegion)
                {
                    Main.invasionType = 0;
                }
                GoblinLegionText = true;
                GoblinLegionTime = 300;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }
            if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                if(Main.time==0&&Main.dayTime&& !GoblinWarlock2)
                {
                    if (Main.netMode != 1)
                    {
                        if (Main.invasionType == 0)
                        {
                            Main.invasionDelay = 0;
                            Main.StartInvasion();
                        }
                    }
                    GoblinWarlock2 = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData);
                    }
                }
            }
        }
        public override void PreUpdateWorld()
        {
            if (!树妖 && 觉醒星心双子)
            {
                if (NPC.AnyNPCs(20))
                {
                    NPC npc = Main.npc[NPC.FindFirstNPC(20)];
                    npc.Dnpc().Bool[0] = true;
                }
                else
                {
                    NPC npc = Main.npc[NewNPC(NPC.GetSource_TownSpawn(), 20, 20, 20)];
                    npc.homeless = false;
                    npc.homeTileX = (int)npc.position.X / 16;
                    npc.homeTileY = (int)npc.position.Y / 16;
                }

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }
        }

        public override void PreUpdateTime()
        {
            /*
            downedLifeGuard = true;
            downedLifeGuard2 = true;
            downedLifeGuard3 = false;
            downedStarGuard = true;
            downedStarGuard2 = true;
            downedStarGuard3 = false;*/
            //哥布林军团
            if (downedBoss2 && !GoblinLegion)
            {
                GoblinLegion = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Language.GetTextValue("Mods.DDmod.WorldTips.Goblin")), new Microsoft.Xna.Framework.Color(180, 0, 255));
                    NetMessage.SendData(MessageID.WorldData);
                }
                else if (Main.netMode == 0)
                {
                    Main.NewText(Language.GetTextValue("Mods.DDmod.WorldTips.Goblin"), 180, 0, 255);
                }
            }
            if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                if (!GoblinWarlock)
                {
                    GoblinWarlock = true;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Language.GetTextValue("Mods.DDmod.WorldTips.Goblin4")), new Microsoft.Xna.Framework.Color(180, 0, 255));
                        NetMessage.SendData(MessageID.WorldData);
                    }
                    else if (Main.netMode == 0)
                    {
                        Main.NewText(Language.GetTextValue("Mods.DDmod.WorldTips.Goblin4"), 180, 0, 255);
                    }
                }
            }
            if (GoblinLegionText)
            {
                if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                {
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Language.GetTextValue("Mods.DDmod.WorldTips.Goblin5")), new Microsoft.Xna.Framework.Color(180, 0, 255));
                        NetMessage.SendData(MessageID.WorldData);
                    }
                    else if (Main.netMode == 0)
                    {
                        Main.NewText(Language.GetTextValue("Mods.DDmod.WorldTips.Goblin5"), 180, 0, 255);
                    }
                }
                else
                {
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Language.GetTextValue("Mods.DDmod.WorldTips.Goblin2")), new Microsoft.Xna.Framework.Color(180, 0, 255));
                        NetMessage.SendData(MessageID.WorldData);
                    }
                    else if (Main.netMode == 0)
                    {
                        Main.NewText(Language.GetTextValue("Mods.DDmod.WorldTips.Goblin2"), 180, 0, 255);
                    }
                }
                GoblinLegionText = false;
            }
            if (GoblinLegionTime > 0)
            {
                GoblinLegionTime--;
                if (GoblinLegionTime == 1)
                {
                    if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    {
                        SpawnOnPlayer(Main.player[Player.FindClosest(new Vector2(Main.spawnTileX, Main.spawnTileY) * 16, 1, 1)].whoAmI, ModContent.NPCType<真哥布林术士>());
                    }
                    else
                    {
                        SpawnOnPlayer(Main.player[Player.FindClosest(new Vector2(Main.spawnTileX, Main.spawnTileY) * 16, 1, 1)].whoAmI, ModContent.NPCType<GoblinSorcererChieftain>());
                    }
                }
            }
        }
    }
}
