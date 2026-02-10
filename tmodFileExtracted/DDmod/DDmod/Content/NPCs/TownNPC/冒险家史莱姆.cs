using DDmod.Content.Biome;
using DDmod.NoContent.Config;
using DDmod.Players;
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
    [AutoloadHead]
    public class 冒险家史莱姆 : ModNPC
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
                .SetNPCAffection(ModContent.NPCType<委托兑换商史莱姆>(), AffectionLevel.Love)
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

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.冒险家史莱姆"))
            });
        }
        public override bool CanTownNPCSpawn(int numTownNPCs)/* tModPorter Suggestion: Copy the implementation of NPC.SpawnAllowed_Merchant in vanilla if you to count money, and be sure to set a flag when unlocked, so you don't count every tick. */
        {
            return ModContent.GetInstance<DDConfigServer>().AdventureSlime;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Criss"
            };
        }


        public override string GetChat()
        {
            return Language.GetTextValue("Mods.DDmod.NPCs.冒险家史莱姆.Chat1");
        }

        public override void AI()
        {
            if(!ModContent.GetInstance<DDConfigServer>().AdventureSlime)
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
                                    CText2 = Language.GetTextValue("Mods.DDmod.NPCs.冒险家史莱姆.say1") + Main.npc[a].FullName;
                                }
                                else
                                {
                                    CText2 = Language.GetTextValue("Mods.DDmod.NPCs.冒险家史莱姆.say2") + Main.npc[a].FullName;
                                }
                            }
                            else
                            {
                                CText2 = Language.GetTextValue("Mods.DDmod.NPCs.冒险家史莱姆.say3") + Main.npc[a].FullName;
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
            button = Language.GetTextValue("Mods.DDmod.NPCs.冒险家史莱姆.Options1");
            button2 = Language.GetTextValue("Mods.DDmod.NPCs.冒险家史莱姆.Options2");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                if (Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel == 0)
                {
                    Main.npcChatText = Language.GetTextValue("Mods.DDmod.NPCs.冒险家史莱姆.Chat2");
                }
                else
                {
                    EntrustUI.Visible = true;
                    EntrustUI.Location = NPC.Center;
                    EntrustUI.Synthesis = NPC.whoAmI;
                    EntrustPlayer EntrustPlayer = Main.LocalPlayer.GetModPlayer<EntrustPlayer>();
                    for (int A = 0; A < EntrustPlayer.MaxEntrust; A++)
                    {
                        任务.战斗任务(out NPC npc, out List<Item> Loot, out string Text, out int Stack, out int Level, out int Value);
                        EntrustPlayer.entrust[A].UpdateEntrust(EntrustID.战斗任务, Text, null, npc, Stack, Loot, Level, Value);
                        /*
                        if (Main.rand.NextBool(2))
                        {
                            任务.收集任务(out Item item, out List<Item> Loot, out string Text);
                            EntrustPlayer.entrusts[A].UpdateEntrust(EntrustID.收集任务, Text, item, null, 0, Loot);
                        }
                        else
                        {
                            任务.战斗任务(out NPC npc,out List<Item> Loot, out string Text);
                            EntrustPlayer.entrusts[A].UpdateEntrust(EntrustID.战斗任务, Text, null, npc, 100, Loot);
                        }*/
                    }
                    Main.npcChatText = "";
                }
            }
            else
            {
                EntrustLevelUI.Visible = true;
                EntrustLevelUI.Location = NPC.Center;
                EntrustLevelUI.Synthesis = NPC.whoAmI;
                Main.npcChatText = "";
            }
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
}