global using DDmod.Content.NPCs;
global using DDmod.DDLoot;
global using DDmod.Helper;
global using DDmod.Textures;
global using DDmod.BossHealthBar;
global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using ReLogic.Content;
global using System;
global using System.Collections.Generic;
global using System.IO;
global using Terraria;
global using Terraria.Audio;
global using Terraria.DataStructures;
global using Terraria.GameContent;
global using Terraria.GameContent.Bestiary;
global using Terraria.GameContent.Creative;
global using Terraria.GameContent.ItemDropRules;
global using Terraria.ID;
global using Terraria.Localization;
global using Terraria.ModLoader;
global using ReLogic.Graphics;
global using Terraria.UI.Chat;
global using DDmod.Content.Buffs.PlayerBuffs;
global using DDmod.UI.ItemUI;
global using DDmod.Content.Projectiles;
global using static DDmod.Helper.DNPC;
global using static DDmod.Helper.DDProj;
global using static DDmod.Helper.DDust;
global using static DDmod.Content.Projectiles.DDGlobalProjectile;
global using static Terraria.Audio.SoundEngine;
global using static Terraria.Projectile;
global using static Terraria.NPC;
global using static Terraria.Dust;
global using static StructureHelper.API.Generator;
using DDmod.Content.NPCs.BossAI;
using DDmod.Content;
using DDmod.Effects;
using DDmod.Players;
using Terraria.UI;
using Terraria.ModLoader.IO;
using DDmod.UI.ItemUI.背包;
using DDmod.Worlds;
using DDmod.Content.Tiles.农场;
using DDmod.Content.Items.农场;
using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.流星;
using DDmod.Content.Tiles;
using DDmod.UI.PlaystationUI;
using Terraria.Chat;

namespace DDmod
{
    public class DDmod : Mod
    {
        //同步
        public static DDmod Instance;

        public static Item NewItem = new Item();
        public override uint ExtraPlayerBuffSlots => 200;
        //战宠
        public static void SyncBattlepets(Player player, int I)
        {
            if (Main.netMode == 1 && I >= 0)
            {
                Mod mod = Instance;
                BattlePets battle = player.Dplayer().Bpets[I];
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.Battlepets);
                packet.Write((byte)player.whoAmI);
                packet.Write((byte)I);
                packet.Write((byte)battle.Type);
                packet.Write((byte)battle.Level);
                packet.Write(battle.Fight);
                packet.Write(battle.Life);
                packet.Write((byte)battle.Variant);
                packet.Send(-1, player.whoAmI);
            }
        }
        public static void SyncBattlepets2(Mod mod, BinaryReader reader, int whoAmI)
        {
            Player player = Main.player[reader.ReadByte()];
            int I = reader.ReadByte();
            int Type = reader.ReadByte();
            int Level = reader.ReadByte();
            if (player.Dplayer().Bpets[I] == null)
            {
                player.Dplayer().Bpets[I] = new BattlePets(Type);
            }
            bool Fight = reader.ReadBoolean();
            int Life = reader.ReadInt32();
            byte Variant = reader.ReadByte();
            player.Dplayer().Bpets[I].Level = Level;
            player.Dplayer().Bpets[I].Fight = Fight;
            player.Dplayer().Bpets[I].Life = Life;
            player.Dplayer().Bpets[I].Variant = Variant;
            player.Dplayer().FightPets = I;
            if (Main.netMode == 2)
            {
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.Battlepets);
                packet.Write((byte)player.whoAmI);
                packet.Write((byte)I);
                packet.Write((byte)player.Dplayer().Bpets[I].Type);
                packet.Write((byte)player.Dplayer().Bpets[I].Level);
                packet.Write(player.Dplayer().Bpets[I].Fight);
                packet.Write(player.Dplayer().Bpets[I].Life);
                packet.Write((byte)player.Dplayer().Bpets[I].Variant);
                packet.Send(-1, player.whoAmI);
            }
        }
        public static void PlayersGame(Mod mod, BinaryReader reader, int whoAmI)
        {
            Player player = Main.player[reader.ReadByte()];
            Point16 Po = new(reader.ReadInt16(), reader.ReadInt16());
            player.GetModPlayer<PlaystationPlayer>().point = Po;
            if (Main.netMode == 2)
            {
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.PlayersGame);
                packet.Write((byte)player.whoAmI);
                packet.Write(Po.X);
                packet.Write(Po.Y);
                packet.Send(-1, player.whoAmI);
            }
        }
        public static void PlayersEntrust(Mod mod, BinaryReader reader, int whoAmI)
        {
            Player player = Main.player[reader.ReadInt32()];
            int Count = reader.ReadInt32();
            var modPlayer = player.GetModPlayer<EntrustPlayer>();
            modPlayer.EntrustNPCTypes.Clear();
            for (int A = 0; A < Count; A++)
            {
                modPlayer.AddEntrust(reader.ReadInt32());
            }
            if (Main.netMode == 2)
            {
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.PlayersEntrust);
                packet.Write(player.whoAmI);
                packet.Write(Count);
                for (int A = 0; A < Count; A++)
                {
                    packet.Write(modPlayer.EntrustNPCTypes[A]);
                }
                packet.Send(-1, player.whoAmI);
            }
        }
        public static void NPCMaster(Mod mod, BinaryReader reader)
        {
            byte N = reader.ReadByte();
            byte Master = reader.ReadByte();
            if (N <= 200)
            {
                NPC npc = Main.npc[N];
                if (npc.type > 0)
                {
                    npc.Dnpc().Master = Master;
                    if (Main.npc[Master].type > 0)
                    {
                        Main.npc[Master].Dnpc().Servant[npc.whoAmI] = true;
                    }
                    npc.Dnpc().MasterBool = true;
                }
            }
            if (Main.netMode == 2)
            {
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.NPCMaster);
                packet.Write((byte)N);
                packet.Write(Master);
                packet.Send(-1, -1);
            }
        }
        public static void PlayersWorld(Mod mod, BinaryReader reader, int whoAmI)
        {
            Player player = Main.player[reader.ReadByte()];
            byte T = reader.ReadByte();
            NPCDowned.绿岩刷怪 = reader.ReadBoolean();
            DDWorld.诅咒之火 = reader.ReadBoolean();
            if (Main.netMode == 2)
            {
                if (T == 1)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.DDmod.ItemTips.诅咒之火"), new Color(50, 255, 130));
                    DDWorld.诅咒之火 = true;
                }
                NetMessage.TrySendData(7);
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.PlayersWorld);
                packet.Write((byte)player.whoAmI);
                packet.Write(T);
                packet.Write(NPCDowned.绿岩刷怪);
                packet.Write(DDWorld.诅咒之火);
                packet.Send(-1, player.whoAmI);
            }
        }
        public static void PlayersSuit(Mod mod, BinaryReader reader, int whoAmI)
        {
            Player player = Main.player[reader.ReadByte()];
            player.Aplayer().神圣戒指CD = reader.ReadInt16();
            player.Aplayer().GuardianOfTheStarCD = reader.ReadInt32();
            player.Aplayer().PalladiumSetCD = reader.ReadInt16();
            player.Aplayer().RefinedGoldSetCD = reader.ReadInt16();
            if (Main.netMode == 2)
            {
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.PlayersSuit);
                packet.Write((byte)player.whoAmI);
                packet.Write(player.Aplayer().神圣戒指CD);
                packet.Write(player.Aplayer().GuardianOfTheStarCD);
                packet.Write(player.Aplayer().PalladiumSetCD);
                packet.Write(player.Aplayer().RefinedGoldSetCD);
                packet.Send(-1, player.whoAmI);
            }
        }
        /// <summary>
        /// 黎明同步数据
        /// </summary>
        /// <param name="type">同步类型</param>
        /// <param name="whoAmI">谁要同步</param>
        /// <param name="toClient">指定发给谁</param>
        /// <param name="ignoreClient">不发给谁</param>
        public static void SyncData(DDType type, int whoAmI, int toClient = -1, int ignoreClient = -1, object value = null, object value2 = null, object value3 = null)
        {
            if (Main.netMode == 0)
            {
                return;
            }
            Mod Mod = DDmod.Instance;
            ModPacket packet;
            try
            {
                switch (type)
                {
                    case DDType.PlayerCenter:
                        Player player = Main.player[whoAmI];
                        packet = Mod.GetPacket(256);
                        packet.Write((byte)DDType.PlayerCenter);
                        packet.Write((byte)player.whoAmI);
                        packet.WriteVector2(player.Center);
                        packet.WriteVector2(player.velocity);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.NPCCenter:
                        packet = Mod.GetPacket(256);
                        NPC npc = Main.npc[whoAmI];
                        packet.Write((byte)DDType.NPCCenter);
                        packet.Write((byte)whoAmI);
                        packet.Write(npc.Dnpc().Control);
                        packet.WriteVector2(npc.Center);
                        packet.WriteVector2(npc.velocity);
                        packet.Write(ignoreClient);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.MouseWorld:
                        //要和上面对齐
                        packet = Mod.GetPacket(256);
                        packet.Write((byte)DDType.MouseWorld);
                        packet.Write((byte)whoAmI);
                        packet.WriteVector2(Main.player[whoAmI].Dplayer().MouseWorld);
                        packet.Write(Main.player[whoAmI].controlUseTile);
                        //前者是发给所有玩家,后者是不用发给我自己
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.PlayerData:
                        player = Main.player[whoAmI];
                        packet = Mod.GetPacket(256);
                        packet.Write((byte)DDType.PlayerData);
                        packet.Write((byte)player.whoAmI);
                        packet.Write((short)player.statMana);
                        packet.Write(player.Dplayer().PlayerTimes);
                        packet.Write((byte)Main.player[whoAmI].GetModPlayer<EntrustPlayer>().Level);
                        BitsByte actionFlags2 = (byte)0;
                        actionFlags2[0] = player.Aplayer().Stand!=0;
                        actionFlags2[1] = player.Aplayer().Stand2!=0;
                        actionFlags2[2] = player.Aplayer().Stand3!=0;
                        actionFlags2[3] = player.Dplayer().STPosition.X != 0;
                        actionFlags2[4] = player.Dplayer().STPosition.Y!=0;
                        packet.Write(actionFlags2);
                        if(actionFlags2[0])
                        packet.Write((byte)player.Aplayer().Stand);
                        if (actionFlags2[1])
                            packet.Write((byte)player.Aplayer().Stand2);
                        if (actionFlags2[2])
                            packet.Write((byte)player.Aplayer().Stand3);
                        if (actionFlags2[3])
                            packet.Write(player.Dplayer().STPosition.X);
                        if (actionFlags2[4])
                            packet.Write(player.Dplayer().STPosition.Y);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.NPCLife:
                        packet = Mod.GetPacket(256);
                        npc = Main.npc[whoAmI];
                        packet.Write((byte)DDType.NPCLife);
                        packet.Write((byte)whoAmI);
                        packet.Write(npc.life);
                        packet.Write(npc.lifeMax);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.Config:
                        packet = Mod.GetPacket(256);
                        PlayerAction playerAction = Main.player[whoAmI].PlayerAction();
                        packet.Write((byte)DDType.Config);
                        packet.Write((byte)whoAmI);
                        //填入玩家要同步的内容
                        BitsByte actionFlags = (byte)0;
                        actionFlags[0] = playerAction.headRotation;
                        actionFlags[1] = playerAction.Somersault;
                        actionFlags[2] = playerAction.Swim;
                        actionFlags[3] = playerAction.Action;
                        actionFlags[4] = playerAction.MoveEffects;
                        actionFlags[5] = playerAction.ShowKey;
                        actionFlags[6] = playerAction.ShowWeapons;
                        packet.Write(actionFlags);
                        //前者是发给所有玩家,后者是不用发给我自己
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.Delay:
                        packet = Mod.GetPacket(256);
                        packet.Write((byte)DDType.Delay);
                        packet.Write((byte)whoAmI);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.TalismanTE:
                        packet = Mod.GetPacket(256);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.TalismanTE2:
                        packet = Mod.GetPacket(256);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.锄地:
                        packet = Mod.GetPacket(256);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.菜TE:
                        packet = Mod.GetPacket(256);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.NPCTE:
                        packet = Mod.GetPacket(256);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.Talisman:
                        packet = Mod.GetPacket(128);
                        TalismanPlayer DDPlayer = Main.player[whoAmI].TPlayer();

                        packet.Write((byte)DDType.Talisman);
                        packet.Write((byte)whoAmI);

                        // 简单直接，所有字段都发
                        packet.Write(DDPlayer.UseTalisman);
                        packet.Write(DDPlayer.Shield);
                        packet.Write(DDPlayer.oldShield);
                        packet.Write(DDPlayer.MaxShield);
                        packet.Write(DDPlayer.MaxTalismanCD);
                        packet.Write(DDPlayer.TalismanCD);
                        packet.Write(DDPlayer.TalismanCD2);
                        packet.Write(DDPlayer.TalismanTimes);
                        packet.Write(DDPlayer.TalismanTimes2);
                        packet.Write(DDPlayer.MaxTalismanTimes);

                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.PlayerFood:
                        Main.player[whoAmI].GetModPlayer<FoodPlayer>().Send();
                        break;
                    case DDType.PlayerHeal:
                        packet = Mod.GetPacket(256);
                        packet.Write((byte)DDType.PlayerHeal);
                        packet.Write((byte)whoAmI);
                        packet.Write((short)value);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.PlayerExp:
                        packet = Mod.GetPacket(256);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.PlayerNPC:
                        packet = Mod.GetPacket(256);
                        packet.Send(toClient, ignoreClient);
                        break;
                    case DDType.Battlepets:
                        DDmod.SyncBattlepets(Main.player[whoAmI], Main.player[whoAmI].Dplayer().FightPets);
                        break;
                    case DDType.PlayersGame:
                        Main.player[whoAmI].GetModPlayer<PlaystationPlayer>().Sync();
                        break;
                    //玩家发送世界更新内容
                    case DDType.PlayersWorld:
                        Main.player[whoAmI].GetModPlayer<DDPlayer>().Sync((byte)value);
                        break;
                    //玩家发送世界更新内容
                    case DDType.PlayersSuit:
                        Main.player[whoAmI].Aplayer().Sync();
                        break;
                    //玩家发送世界更新内容
                    case DDType.PlayersEntrust:
                        Main.player[whoAmI].GetModPlayer<EntrustPlayer>().Sync();
                        break;
                    //玩家发送世界更新内容
                    case DDType.NPCMaster:
                        npc = Main.npc[whoAmI];
                        packet = Mod.GetPacket(256);
                        packet.Write((byte)DDType.NPCMaster);
                        packet.Write((byte)whoAmI);
                        packet.Write((byte)npc.Dnpc().Master);
                        packet.Send(toClient, ignoreClient);
                        break;
                    default:
                        DDmod.Instance.Logger.Error(string.Format("无法解析发包：不存在的包ID为{0}", type));
                        throw new Exception("无法解析同步数据包：无效的同步数据包ID.");
                }
            }
            catch (Exception e)
            {
                EndOfStreamException eose;
                ObjectDisposedException ode;
                if ((eose = (e as EndOfStreamException)) != null)
                {
                    DDmod.Instance.Logger.Error("无法解析同步数据包,可能数据包太短,损坏,丢失.", eose);
                }
                else if ((ode = (e as ObjectDisposedException)) != null)
                {
                    DDmod.Instance.Logger.Error("无法解析同步数据包,可能数据包太短,损坏,丢失.", ode);
                }
                else
                {
                    IOException ioe;
                    if ((ioe = (e as IOException)) == null)
                    {
                        throw;
                    }
                    DDmod.Instance.Logger.Error("无法解析同步数据包,可能数据包太短,损坏,丢失", ioe);
                }
            }
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            try
            {
                DDType SType = (DDType)reader.ReadByte();
                switch (SType)
                {
                    case DDType.PlayerCenter:
                        SyncPlayer(this, reader, whoAmI);
                        break;
                    case DDType.NPCCenter:
                        Sync.SNPC.SyncNPC(this, reader);
                        break;
                    case DDType.MouseWorld:
                        //你要同步的包
                        DDPlayer.SyncMouseWorld(this, reader, whoAmI);
                        break;
                    case DDType.PlayerData:
                        //你要同步的包
                        DDPlayer.PlayerData(this, reader, whoAmI);
                        break;
                    case DDType.NPCLife:
                        //你要同步的包
                        Sync.SNPC.SyncNPCLife(this, reader);
                        break;
                    case DDType.Config:
                        //在服务器同步玩家的配置信息
                        PlayerAction.PlayerConfig(this, reader);
                        break;
                    case DDType.Delay:
                        //在服务器同步玩家的配置信息
                        DDPlayer.PlayerDelay(this, reader);
                        break;
                    case DDType.TalismanTE:
                        //在服务器同步TalismanTE
                        StrengthenUI.ReadTalismanTE(this, reader);
                        break;
                    case DDType.TalismanTE2:
                        //在服务器同步TalismanTE
                        StrengthenUI.ReadTalismanTE2(this, reader);
                        break;
                    case DDType.锄地:
                        //发送锄地数据
                        DDWorld.Read(reader);
                        break;
                    case DDType.菜TE:
                        //发送锄地数据
                        菜TE.Read(reader);
                        break;
                    case DDType.NPCTE:
                        //发送锄地数据
                        NPCSpawnTE.Read(reader);
                        break;
                    case DDType.Talisman:
                        TalismanPlayer.PlayerTalisman(this, reader);
                        break;
                    case DDType.PlayerFood:
                        //发送锄地数据
                        FoodPlayer.PlayerFood(this, reader);
                        break;
                    case DDType.PlayerHeal:
                        //同步治疗
                        DDOn.PlayerOn.HandleHeal(this, reader);
                        break;
                    case DDType.PlayerExp:
                        //同步经验
                        DGlobalNPCExp.PlayerExp(this, reader);
                        break;
                    case DDType.PlayerNPC:
                        //同步命中
                        DGlobalNPCExp.PlayerNPC(this, reader);
                        break;
                    case DDType.Battlepets:
                        //同步宠物
                        SyncBattlepets2(this, reader, whoAmI);
                        break;
                    case DDType.PlayersGame:
                        //同步游戏
                        PlayersGame(this, reader, whoAmI);
                        break;
                    case DDType.PlayersWorld:
                        //同步游戏
                        PlayersWorld(this, reader, whoAmI);
                        break;
                    case DDType.PlayersSuit:
                        //同步游戏
                        PlayersSuit(this, reader, whoAmI);
                        break;
                    case DDType.PlayersEntrust:
                        //同步游戏
                        //PlayersWorld(this, reader, whoAmI);
                        break;
                    case DDType.NPCMaster:
                        //同步游戏
                        NPCMaster(this, reader);
                        break;
                    default:
                        Logger.Error(string.Format("无法解析发包：不存在的包ID为{0}", SType));
                        throw new Exception("无法解析同步数据包：无效的同步数据包ID.");
                }
            }
            catch (Exception e)
            {
                EndOfStreamException eose;
                ObjectDisposedException ode;
                if ((eose = (e as EndOfStreamException)) != null)
                {
                    Logger.Error("无法解析同步数据包,可能数据包太短,损坏,丢失.", eose);
                }
                else if ((ode = (e as ObjectDisposedException)) != null)
                {
                    Logger.Error("无法解析同步数据包,可能数据包太短,损坏,丢失.", ode);
                }
                else
                {
                    IOException ioe;
                    if ((ioe = (e as IOException)) == null)
                    {
                        throw;
                    }
                    Logger.Error("无法解析同步数据包,可能数据包太短,损坏,丢失", ioe);
                }
            }
        }
        public override void Load()
        {
            DDShaders.LoadShaders();
            Instance = this;
            if (!Main.dedServ)
            {
                Instance.AddBossHeadTexture("DDmod/Content/NPCs/Boss/MeteorAnnihilator/MeteorAnnihilator2_Head_Boss", -1);
                Instance.AddBossHeadTexture("DDmod/Content/NPCs/Boss/流星破坏者/流星破坏者2_Head_Boss", -1);
            }
            base.Load();
        }
        public static void SyncPlayer(Mod mod, BinaryReader reader, int whoAmI)
        {
            byte player = reader.ReadByte();
            Vector2 Center = reader.ReadVector2();
            Vector2 Velocityr = reader.ReadVector2();
            Main.player[player].Center = Center;
            Main.player[player].velocity = Velocityr;
            if (Main.netMode == NetmodeID.Server)
            {
                SyncData(DDType.PlayerCenter, player, -1, player);
            }
        }
    }
    public enum DDType : byte
    {
        /// <summary> 同步玩家位置 </summary>
        PlayerCenter,
        /// <summary> 同步NPC位置 </summary>
        NPCCenter,
        /// <summary> 同步鼠标位置 </summary>
        MouseWorld,
        /// <summary> 同步玩家数据 </summary>
        PlayerData,
        NPCLife,
        Config,
        Delay,
        TalismanTE,
        TalismanTE2,
        锄地,
        菜TE,
        NPCTE,
        Talisman,
        PlayerFood,
        PlayerHeal,
        PlayerExp,
        PlayerNPC,
        Battlepets,
        /// <summary> 同步玩家打游戏 </summary>
        PlayersGame,
        /// <summary> 玩家发送世界更新内容 </summary>
        PlayersWorld,
        /// <summary> 套装奖励计时器同步 </summary>
        PlayersSuit,
        /// <summary> 任务系统 </summary>
        PlayersEntrust,
        /// <summary> 同步NPC的主人和杂鱼 </summary>
        NPCMaster,
    }
}
