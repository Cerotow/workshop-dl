using DDmod.Content;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Potion;
using DDmod.Content.Items.Series.Venture;
using DDmod.Content.Items.Sundries;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Players;
using DDmod.SubworldLibraryWorld;
using DDmod.SubworldLibraryWorld.草原;
using log4net.Repository.Hierarchy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using static DDmod.Helper.DDHelper;

namespace DDmod.UI.HunterQuests
{

    public class EntrustID
    {
        public const int 无任务 = 0;
        public const int 任务失败 = 1;
        public const int 任务完成 = 2;
        public const int 收集任务 = 3;
        public const int 战斗任务 = 4;
    }
    public partial class Entrust
    {
        //任务类型
        public int type = EntrustID.无任务;
        public Item EntrustItem;
        public NPC EntrustNPC;
        public List<Item> Loot;
        public int Level;
        public int EntrustNPCStack;
        public int MaxEntrustNPCStack;
        public int value;
        public string Text;
        public bool EntrustFinish;
        /// <summary>
        /// 有没有接受任务
        /// </summary>
        public bool Accept;
        public int EntrustNPCType(NPC npc)
        {
            //属于史莱姆
            if (npc.type == ModContent.NPCType<GrassSlime>() || npc.type == 147 || npc.type == 302 || npc.type == 333 || npc.type == 334 || npc.type == 335 || npc.type == 336)
            {
                return 1;
            }
            //属于恶魔眼
            if (NPCID.Sets.DemonEyes[npc.type])
            {
                return 2;
            }
            //属于蘑菇战士
            if (npc.type == 254 || npc.type == 255)
            {
                return 635;
            }
            //属于僵尸
            if (npc.type == 161||npc.type == 431)
            {
                return 161;
            }
            //属于僵尸
            if (NPCID.Sets.Zombies[npc.type])
            {
                return 3;
            }
            //属于骷髅
            if (npc.type == 201 || npc.type == 202 || npc.type == 203 || npc.type == 322 || npc.type == 323 || npc.type == 324 || npc.type == 449 || npc.type == 450 || npc.type == 451 || npc.type == 452)
            {
                return 21;
            }
            //属于水母
            if (npc.type == 64|| npc.type == 103)
            {
                return 63;
            }
            //属于血蜘蛛
            if (npc.type ==240)
            {
                return 239;
            }
            //属于大蜜蜂
            if (npc.type ==231||npc.type ==232||npc.type ==233||npc.type ==234||npc.type ==235)
            {
                return 42;
            }
            //属于食人花
            if (npc.type ==56)
            {
                return 43;
            }
            //属于蚁狮马
            if (npc.type ==508)
            {
                return 580;
            }
            //属于蚁狮蜂
            if (npc.type ==509)
            {
                return 581;
            }
            //恶魔
            if (npc.type ==66)
            {
                return 62;
            }
            //愤怒骷髅
            if (npc.type ==294|| npc.type == 295|| npc.type == 296)
            {
                return 31;
            }
            //蓝盔甲骷髅
            if (npc.type ==274|| npc.type == 275|| npc.type == 276)
            {
                return 273;
            }
            //烂盔甲骷髅
            if (npc.type ==270|| npc.type == 271|| npc.type == 272)
            {
                return 269;
            }
            //地狱骷髅
            if (npc.type ==278|| npc.type == 279|| npc.type == 280)
            {
                return 277;
            }
            //死灵法师
            if (npc.type == 284)
            {
                return 283;
            }
            //褴褛邪教徒法师
            if (npc.type ==282)
            {
                return 281;
            }
            //地狱骷髅
            if (npc.type ==286)
            {
                return 285;
            }
            //蜥蜴
            if (npc.type ==199)
            {
                return 198;
            }
            return npc.type;
        }
        public void Update()
        {
            if (Main.time == 0 && Main.dayTime)
            {
                if (type > 2)
                {
                    type = EntrustID.任务失败;
                    Main.LocalPlayer.GetModPlayer<EntrustPlayer>().EntrustNPCTypes.Clear();
                    //DDmod.SyncData(DDType.PlayersEntrust, Main.LocalPlayer.whoAmI, -1, Main.LocalPlayer.whoAmI);
                }
                if (type == 2)
                {
                    type = EntrustID.无任务;
                }
            }
            if (type == EntrustID.收集任务)
            {
                int s = 0;
                for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
                {
                    if (Main.LocalPlayer.inventory[i].type == EntrustItem.type)
                    {
                        s += Main.LocalPlayer.inventory[i].stack;
                    }
                }
                if (s >= EntrustItem.stack)
                {
                    EntrustFinish = true;
                }
                else
                {
                    EntrustFinish = false;
                }
            }
            else if (type == EntrustID.战斗任务)
            {
                if (EntrustNPCStack >= MaxEntrustNPCStack)
                {
                    EntrustFinish = true;
                }
                else
                {
                    EntrustFinish = false;
                }
            }
            if (!Accept)
            {
                EntrustFinish = false;
            }
        }
        /// <summary>
        /// 任务类型,请使用EntrustID
        /// T文本类型
        /// item需要收集的物品
        /// npc需要击杀的怪物
        /// NPCStack需要击杀的怪物数量
        /// ILoot任务奖励
        /// </summary>
        public void UpdateEntrust(int Etype, string T, Item item, NPC npc, int NPCStack, List<Item> ILoot, int Level, int Value)
        {
            if (type == 0 || type == 1)
            {
                EntrustNPC = null;
                EntrustItem = null;
                MaxEntrustNPCStack = 0;
                EntrustNPCStack = 0;
                type = Etype;
                if (type == EntrustID.收集任务)
                {
                    EntrustItem = item;
                    Text = Language.GetTextValue("Mods.DDmod.EntrustText." + T, new string[] { EntrustItem.stack.ToString(), EntrustItem.Name });
                    //Text = "商人:我需要" + EntrustItem.stack + "个" + EntrustItem.Name + ",请把他带给我,我会给你报酬的.";
                }
                else if (type == EntrustID.战斗任务)
                {
                    //value = Item.buyPrice(0, 1, 20, 20);
                    value = Value;
                    MaxEntrustNPCStack = NPCStack;
                    EntrustNPC = npc;
                    Text = Language.GetTextValue("Mods.DDmod.EntrustText." + T, new string[] { MaxEntrustNPCStack.ToString(), npc.FullName });
                    if (npc.type == 1)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.EntrustText." + T, new string[] { MaxEntrustNPCStack.ToString(), Language.GetTextValue("Mods.DDmod.Entrust.史莱姆") });
                    }
                    if (npc.type == 63)
                    {
                        Text = Language.GetTextValue("Mods.DDmod.EntrustText." + T, new string[] { MaxEntrustNPCStack.ToString(), Language.GetTextValue("Mods.DDmod.Entrust.水母") });
                    }
                }
                Accept = false;
                Loot = ILoot;
                this.Level = Level;
            }
        }
        public void ELoot()
        {
            for (int a = 0; a < Loot.Count; a++)
            {
                if (Loot[a].type != ModContent.ItemType<Exp>())
                {
                    Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), Loot[a].type, Loot[a].stack);
                }
                else
                {
                    Main.LocalPlayer.GetModPlayer<EntrustPlayer>().Experience += Loot[a].stack;
                }
            }
            int v = value / 1000000;
            if(v>0)
            {
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), 74, v);
            }
            v = value / 10000;
            v %= 100;
            if (v > 0)
            {
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), 73, v);
            }
            v = value / 100;
            v %= 100;
            if (v > 0)
            {
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), 72, v);
            }
            v = value;
            v %= 100;
            if (v > 0)
            {
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), 71, v);
            }



            type = EntrustID.任务完成;
        }
        public void AcceptEntrust()
        {
            Accept = true;
            Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AddEntrust(EntrustNPC.type);
            if (EntrustNPC != null && (EntrustNPC.type == 224 || EntrustNPC.type == 225))
            {
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), ModContent.ItemType<潮汐之泪>(), 1);
            }
            if (Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel < 4)
            {
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), 300, 1);
            }
            else if (Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel < 6)
            {

                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), ModContent.ItemType<战争药水>(), 1);
            }
            else
            {

                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), ModContent.ItemType<纷争药水>(), 1);
            }
        }
        public void SaveData(int T, TagCompound tag)
        {
            tag.Add("EntrustsType" + T, type);
            tag.Add("EntrustsItem" + T, EntrustItem);
            if (EntrustNPC != null)
            {
                tag.Add("EntrustsNPC" + T, EntrustNPC.type);
            }
            tag.Add("EntrustsNPCStack" + T, EntrustNPCStack);
            tag.Add("MaxEntrustsNPCStack" + T, MaxEntrustNPCStack);
            tag.Add("EntrustsText" + T, Text);
            tag.Add("EntrustsLoot" + T, Loot);

            if (Accept) tag["EntrustsAccept" + T] = true;
            tag.Add("EntrustsLevel" + T, Level);
            tag.Add("Entrustsvalue" + T, value);
        }
        public void LoadData(int T, TagCompound tag)
        {
            type = tag.Get<int>("EntrustsType" + T);
            EntrustItem = tag.Get<Item>("EntrustsItem" + T);
            NPC npc = new NPC();
            npc.SetDefaults(tag.Get<int>("EntrustsNPC" + T));
            EntrustNPC = npc;
            EntrustNPCStack = tag.Get<int>("EntrustsNPCStack" + T);
            MaxEntrustNPCStack = tag.Get<int>("MaxEntrustsNPCStack" + T);
            Text = tag.Get<string>("EntrustsText" + T);
            Loot = tag.Get<List<Item>>("EntrustsLoot" + T);

            Accept = tag.ContainsKey("EntrustsAccept" + T);
            Level = tag.Get<int>("EntrustsLevel" + T);
            value = tag.Get<int>("Entrustsvalue" + T);
        }
    }
}
