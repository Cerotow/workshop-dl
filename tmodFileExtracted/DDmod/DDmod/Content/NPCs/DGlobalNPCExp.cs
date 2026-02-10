using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Items.Boss.蘑菇王;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.Projectiles;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Projectiles.Summon.Whip;
using DDmod.Content.Tiles;
using DDmod.Content.Tiles.农场;
using DDmod.Players;
using DDmod.UI.HunterQuests;
using Microsoft.Xna.Framework.Input;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;

namespace DDmod.Content.NPCs
{
    public class DGlobalNPCExp : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool[] PlayerHitNPC = new bool[255];
        public override void SetDefaultsFromNetId(NPC npc)
        {
        }
        public void GetExp(NPC npc)
        {
            if (npc.lifeMax > 10 && !NPCID.Sets.ProjectileNPC[npc.type])
            {
                Exp = (int)(npc.lifeMax*0.5F);
            }
            else
            {
                Exp = 0;
                return;
            }
            if (npc.aiStyle == 6)
            {
                Exp /= 3;
            }
            if (npc.type is 87 or 134)
            {
                Exp /= 4;
            }
            /*
            if (npc.Dnpc().BossPhysique)
            {
                Exp /= 2;
            }*/
            if (npc.ModNPC != null && npc.ModNPC.Mod != Mod)
            {
                Exp /= 100;
            }
            if (npc.SpawnedFromStatue || npc.friendly || npc.type is 70 or 72 or 249 or 263 or 328 or 384 or 388 or 395 or 396 or 397 or 400 or 401 or 408 or 437
                or 491 or 516 or 519 or 522 or 523 or 548 or 549)
            {
                Exp = 0;
            }
        }
        public override void SetDefaults(NPC npc)
        {
            
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            Player player = projectile.Player();
            if ((npc.realLife != -1 && Main.npc[npc.realLife].Exp().PlayerHitNPC[player.whoAmI]) || PlayerHitNPC[player.whoAmI])
            {
                return;
            }
            if (Main.netMode == 1)
            {
                ModPacket packet = Mod.GetPacket(256);
                packet.Write((byte)DDType.PlayerNPC);
                packet.Write((byte)player.whoAmI);
                if (npc.realLife != -1)
                {
                    packet.Write((byte)npc.realLife);
                }
                else
                {
                    packet.Write((byte)npc.whoAmI);
                }
                packet.Send(player.whoAmI, -1);
            }
            else
            {
                if (npc.realLife != -1)
                {
                    Main.npc[npc.realLife].Exp().PlayerHitNPC[player.whoAmI] = true;
                }
                else
                {
                    PlayerHitNPC[player.whoAmI] = true;
                }
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if ((npc.realLife != -1 && Main.npc[npc.realLife].Exp().PlayerHitNPC[player.whoAmI]) || PlayerHitNPC[player.whoAmI])
            {
                return;
            }
            if (Main.netMode == 1)
            {
                ModPacket packet = Mod.GetPacket(256);
                packet.Write((byte)DDType.PlayerNPC);
                packet.Write((byte)player.whoAmI);
                if (npc.realLife != -1)
                {
                    packet.Write((byte)npc.realLife);
                }
                else
                {
                    packet.Write((byte)npc.whoAmI);
                }
                packet.Send(player.whoAmI, -1);
            }
            else
            {
                if (npc.realLife != -1)
                {
                    Main.npc[npc.realLife].Exp().PlayerHitNPC[player.whoAmI] = true;
                }
                else
                {
                    PlayerHitNPC[player.whoAmI] = true;
                }
            }
        }
        public static void PlayerNPC(Mod mod, BinaryReader reader)
        {
            byte player = reader.ReadByte();
            byte npc = reader.ReadByte();

            Main.npc[npc].Exp().PlayerHitNPC[player] = true;

            if (Main.netMode == 2)
            {
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.PlayerNPC);
                packet.Write(player);
                packet.Write(npc);
                packet.Send(player, -1);
            }
        }
        public override void HitEffect(NPC npc, HitInfo hit)
        {
            
        }
        public static void PlayerExp(Mod mod, BinaryReader reader)
        {
            byte player = reader.ReadByte();
            byte npc = reader.ReadByte();
            int Exp = Main.npc[npc].GetGlobalNPC<DGlobalNPCExp>().Exp;
            EntrustPlayer Entrust = Main.player[player].GetModPlayer<EntrustPlayer>();
            Entrust.Experience += Exp;
            if (Main.player[player].Dplayer().FightPets >= 0)
            {
                Main.player[player].Dplayer().Bpets[Main.player[player].Dplayer().FightPets].Exp += Exp * 3;
            }
            Entrust.npc.Add(Main.npc[npc]);
            if (Main.netMode == 2)
            {
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.PlayerExp);
                packet.Write(player);
                packet.Write(npc);
                packet.Send(player, -1);
            }
        }

        public override void OnKill(NPC npc)
        {

            if (Main.netMode == 2)
            {
                for (byte a = 0; a < 255; a++)
                {
                    Player player = Main.player[a];
                    if (player.active && PlayerHitNPC[a])
                    {
                        ModPacket packet = Mod.GetPacket(256);
                        packet.Write((byte)DDType.PlayerExp);
                        packet.Write(a);
                        packet.Write((byte)npc.whoAmI);
                        packet.Send(a, -1);
                    }
                }
            }
            else
            {
                EntrustPlayer Entrust = Main.LocalPlayer.GetModPlayer<EntrustPlayer>();
                if (PlayerHitNPC[Main.myPlayer])
                {
                    Entrust.Experience += (int)(Exp);
                    if(Main.LocalPlayer.Dplayer().FightPets>=0)
                    Main.LocalPlayer.Dplayer().Bpets[Main.LocalPlayer.Dplayer().FightPets].Exp += (int)(Exp*3);
                    Entrust.npc.Add(npc);
                }
            }
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            return;
            HashSet<int> typesToAdjust = new HashSet<int>();
            for (int A = 0; A < spawnInfo.Player.GetModPlayer<EntrustPlayer>().EntrustNPCTypes.Count; A++)
            {
                typesToAdjust.Add(spawnInfo.Player.GetModPlayer<EntrustPlayer>().EntrustNPCTypes[A]);
            }
        }
        public void EntrustNPCType(int type, IDictionary<int, float> pool)
        {
            void P(int key)
            {
                pool[key] = 50;
            }

            foreach (int key in pool.Keys)
            {
                if (key == type)
                {
                    P(key);
                }

                //属于史莱姆
                if (type == 1)
                {
                    if (key == ModContent.NPCType<GrassSlime>() || key == 147 || key == 302 || key == 333 || key == 334 || key == 335 || key == 336)
                    {
                        P(key);
                    }
                }
                //属于恶魔眼
                if (type == 2)
                {
                    if (NPCID.Sets.DemonEyes[key])
                    {
                        P(key);
                    }
                }
                //属于蘑菇战士
                if (type == 635)
                {
                    if (key == 254 || key == 255)
                    {
                        P(key);
                    }
                }
                //属于僵尸
                if (type == 161)
                {
                    if (key == 161 || key == 431)
                    {
                        P(key);
                    }
                }
                //属于僵尸
                if (type == 3)
                {
                    if (NPCID.Sets.Zombies[key])
                    {
                        P(key);
                    }
                }
                //属于骷髅
                if (type == 21)
                {
                    if (key == 201 || key == 202 || key == 203 || key == 322 || key == 323 || key == 324 || key == 449 || key == 450 || key == 451 || key == 452)
                    {
                        P(key);
                    }
                }
                //属于水母
                if (type == 63)
                {
                    if (key == 64 || key == 103)
                    {
                        P(key);
                    }
                }
                //属于血蜘蛛
                if (type == 239)
                {
                    if (key == 240)
                    {
                        P(key);
                    }
                }
                //属于大蜜蜂
                if (type == 42)
                {
                    if (key == 231 || key == 232 || key == 233 || key == 234 || key == 235)
                    {
                        P(key);
                    }
                }
                //属于食人花
                if (type == 43)
                {
                    if (key == 56)
                    {
                        P(key);
                    }
                }
                //属于蚁狮马
                if (type == 580)
                {
                    if (key == 508)
                    {
                        P(key);
                    }
                }
                //属于蚁狮蜂
                if (type == 581)
                {
                    if (key == 509)
                    {
                        P(key);
                    }
                }
                //恶魔
                if (type == 62)
                {
                    if (key == 66)
                    {
                        P(key);
                    }
                }
                //愤怒骷髅
                if (type == 31)
                {
                    if (key == 294 || key == 295 || key == 296)
                    {
                        P(key);
                    }
                }
                //蓝盔甲骷髅
                if (type == 273)
                {
                    if (key == 274 || key == 275 || key == 276)
                    {
                        P(key);
                    }
                }
                //烂盔甲骷髅
                if (type == 269)
                {
                    if (key == 270 || key == 271 || key == 272)
                    {
                        P(key);
                    }
                }
                //地狱骷髅
                if (type == 277)
                {
                    if (key == 278 || key == 279 || key == 280)
                    {
                        P(key);
                    }
                }
                //死灵法师
                if (type == 283)
                {
                    if (key == 284)
                    {
                        P(key);
                    }
                }
                //褴褛邪教徒法师
                if (type == 281)
                {
                    if (key == 282)
                    {
                        P(key);
                    }
                }
                //地狱骷髅
                if (type == 285)
                {
                    if (key == 286)
                    {
                        P(key);
                    }
                }
                //蜥蜴
                if (type == 198)
                {
                    if (key == 199)
                    {
                        P(key);
                    }
                }
            }
        }
        bool SP;
        public override bool PreAI(NPC npc)
        {
            if (!SP)
            {
                SP = true;
            }
            return base.PreAI(npc);
        }
        public int Exp = 0;
        public override void ResetEffects(NPC npc)
        {
        }
        public static void SendScale(NPC npc)
        {
        }
        public static void ReceiveScale(BinaryReader binaryReader)
        {
        }
        public static void Receive(BinaryReader binaryReader)
        {
        }
        public override bool CheckActive(NPC npc)
        {
            return true;
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
        }
        public override bool CanHitNPC(NPC npc, NPC target)
        {
            return base.CanHitNPC(npc, target);
        }
        public override void ModifyIncomingHit(NPC npc, ref HitModifiers modifiers)
        {
        }
        
        public override void AI(NPC npc)
        {
        }
        public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        {
            return base.CanBeHitByProjectile(npc, projectile);
        }
        public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        {
            return base.CanBeHitByItem(npc, player, item);
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, HitInfo hit, int damageDone)
        {
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
        }
        public override void DrawBehind(NPC npc, int index)
        {
        }
        public override void ModifyShop(NPCShop shop)
        {
        }
    }

}