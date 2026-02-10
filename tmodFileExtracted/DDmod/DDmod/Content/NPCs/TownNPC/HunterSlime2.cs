using DDmod.Content.Biome;
using DDmod.SubworldLibraryWorld;
using DDmod.UI.HunterQuests;
using DDmod.Worlds;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.NPCs.TownNPC
{
    public class HunterSlime2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Hunter Slime");
           //DisplayName.AddTranslation(7, "猎手史莱姆");
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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.HunterSlime"))
            });
        }
        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Reds"
            };
        }


        public override string GetChat()
        {
            return Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.Chat2");
        }

        public override void AI()
        {
            for(int a = 0;a<Main.player.Length;a++)
            {
                if(Main.player[a].talkNPC == NPC.whoAmI)
                {
                    int life = NPC.life;
                    NPC.Transform(ModContent.NPCType<HunterSlime>());
                    NPC.life = life;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NPC.life = life;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
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
                            NPCs2[a] = 300;
                            if (Main.dayTime)
                            {
                                if (Main.time < 21600)
                                {
                                    CText2 = Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.say1") + Main.npc[a].FullName;
                                }
                                else
                                {
                                    CText2 = Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.say2") + Main.npc[a].FullName;
                                }
                            }
                            else
                            {
                                CText2 = Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.say3") + Main.npc[a].FullName;
                            }
                            TL = 0;
                            TLTime = 0;
                            NPCs[a] = 10000;
                        }
                    }
                }
            }
            if (Main.rand.NextBool(5000) && CText <= 0)
            {
                CText = 300;
                if (Main.dayTime)
                {
                    if (Main.time < 21600)
                    {
                        CText2 = Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.say4");
                    }
                    else
                    {
                        CText2 = Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.say5");
                    }
                    if (Main.raining)
                    {
                        CText2 = Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.say6");
                    }
                    if (Main.raining && Main.newMusic == 52)
                    {
                        CText2 = Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.say7");
                    }
                }
                else
                {
                    CText2 = Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.say8");
                    if (Main.raining)
                    {
                        CText2 = Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.say9");
                    }
                }
                TL = 0;
                TLTime = 0;
            }
        }
        int[] NPCs = new int[200];
        int[] NPCs2 = new int[200];
        int CText;
        string CText2;


        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (SWSystem.TrueSubworld || Main.LocalPlayer.talkNPC == NPC.whoAmI || CText > 0)
            {
                CText--;
                string Text = Language.GetTextValue("Mods.DDmod.NPCs.HunterSlime.say10");
                if (Main.LocalPlayer.GetModPlayer<SWPlayer>().SWDead && !DDSystem.English)
                {
                    Text = "菜就多练练";
                }
                if (CText > 0)
                {
                    Text = CText2;
                }
                if (Main.LocalPlayer.talkNPC == NPC.whoAmI)
                {
                    Text = Main.npcChatText;
                }
                if (Text == "")
                {
                    return true;
                }
                if (Text.Length > TL)
                {
                    TLTime++;
                    if (TLTime % 5 == 0)
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
}