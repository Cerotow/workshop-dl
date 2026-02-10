using DDmod.Content.Biome;
using DDmod.NoContent.Config;
using DDmod.Players;
using DDmod.SubworldLibraryWorld;
using DDmod.UI;
using DDmod.UI.HunterQuests;
using DDmod.Worlds;
using Microsoft.Xna.Framework;
using Stubble.Core.Classes;
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
    public class 宠物管家史莱姆 : ModNPC
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
        public static Asset<Texture2D> Tail;
        public override void Load()
        {
            Tail = ModContent.Request<Texture2D>(Texture+ "_Tail");
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
                .SetNPCAffection(ModContent.NPCType<委托兑换商史莱姆>(), AffectionLevel.Love)
                .SetNPCAffection(20, AffectionLevel.Like)
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

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.宠物管家史莱姆"))
            });
        }
        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return ModContent.GetInstance<DDConfigServer>().Battlepet;
        }
        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Fett"
            };
        }


        public override string GetChat()
        {
            return Language.GetTextValue("Mods.DDmod.NPCs.宠物管家史莱姆.Chat1");
        }

        public override void AI()
        {
            if (!ModContent.GetInstance<DDConfigServer>().Battlepet)
            {
                NPC.Kill();
            }
            if (Tailframe2 > 0)
            {
                Tailframe2++;
                if (Tailframe2 % 4==0)
                {
                    Tailframe++;
                }

                if (Tailframe >= 4)
                {
                    Tailframe = 0;
                }

                if (Tailframe == 2)
                {
                    Tailframe2 = -4;
                }
            }
            else if (Tailframe2 < 0)
            {
                Tailframe2++;

            }
            else
            {
                if (Main.rand.NextBool(100))
                {
                    Tailframe2 = 1;
                    Tailframe++;
                }
                if (NPC.velocity.X != 0)
                {
                    Tailframe2 = 1;
                    Tailframe++;
                }
            }
            if (NPCs ==null)
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
                        if ((Main.npc[a].Center - NPC.Center).Length() < 300&&Main.npc[a].type!= 19 && Main.npc[a].type != 38)
                        {
                            CText = 300;
                            NPCs2[a] = 3000;
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
                CText = 3000;
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
        int Life = 0;
        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("Mods.DDmod.NPCs.宠物管家史莱姆.Options1");
            string T = "";
            Life = 0;
            for (int a = 0; a < Main.LocalPlayer.Dplayer().Bpets.Length; a++)
            {
                if(Main.LocalPlayer.Dplayer().Bpets[a].LifeMax - Main.LocalPlayer.Dplayer().Bpets[a].Life>0)
                {
                    Life += Main.LocalPlayer.Dplayer().Bpets[a].LifeMax - Main.LocalPlayer.Dplayer().Bpets[a].Life;
                }
            }
            int Value = Life/10;
            int 铂金 = Value / 1000000;

            int 金 = Value / 10000;
            金 %= 100;
            int 银 = Value / 100;
            银 %= 100;
            int 铜 = Value;
            铜 %= 100;
            if (Value > 100)
            {
                T = (铂金 > 0 ? (铂金.ToString() + Language.GetTextValue("Currency.Platinum")) : "") + (金 > 0 ? (金.ToString() + Language.GetTextValue("Currency.Gold")) : "") + (银 > 0 ? (银.ToString() + Language.GetTextValue("Currency.Silver")) : "") + (铜 > 0 ? (铜.ToString() + Language.GetTextValue("Currency.Copper")) : "");
            }
            else
            {
                T = Language.GetTextValue("Mods.DDmod.NPCs.宠物管家史莱姆.Options3");
            }
            button2 = Language.GetTextValue("Mods.DDmod.NPCs.宠物管家史莱姆.Options2",T);
        }

        int A = 30;
        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                DDUISystem.BattlePetsUION = true;
                Main.npcChatText = "";
                if (Main.LocalPlayer.Dplayer().Bpets == null || Main.LocalPlayer.Dplayer().Bpets.Length != 20)
                    Main.LocalPlayer.Dplayer().Bpets = new BattlePets[20];
                for (int a = 0; a < Main.LocalPlayer.Dplayer().Bpets.Length; a++)
                {
                    if (Main.LocalPlayer.Dplayer().Bpets[a] == null)
                    {
                        Main.LocalPlayer.Dplayer().Bpets[a] = new BattlePets(0);
                    }
                }
                if (Main.LocalPlayer.Dplayer().Bpets[0] == null || Main.LocalPlayer.Dplayer().Bpets[0].Type == 0)
                {
                    Main.npcChatText = Language.GetTextValue("Mods.DDmod.NPCs.宠物管家史莱姆.Chat2");
                }
            }
            else
            {
                if (Life > 0)
                {
                    if (Life / 10 <= 100)
                    {
                        Main.npcChatText = Language.GetTextValue("Mods.DDmod.NPCs.宠物管家史莱姆.Chat6");
                        for (int a = 0; a < Main.LocalPlayer.Dplayer().Bpets.Length; a++)
                        {
                            Main.LocalPlayer.Dplayer().Bpets[a].Life = Main.LocalPlayer.Dplayer().Bpets[a].LifeMax;

                        }
                    }
                    else if (Main.LocalPlayer.BuyItem(Life / 10))
                    {
                        for (int a = 0; a < Main.LocalPlayer.Dplayer().Bpets.Length; a++)
                        {
                            Main.LocalPlayer.Dplayer().Bpets[a].Life = Main.LocalPlayer.Dplayer().Bpets[a].LifeMax;

                        }
                        Main.npcChatText = Language.GetTextValue("Mods.DDmod.NPCs.宠物管家史莱姆.Chat3");
                    }
                    else
                    {
                        Main.npcChatText = Language.GetTextValue("Mods.DDmod.NPCs.宠物管家史莱姆.Chat4");
                    }
                }
                else
                {

                    Main.npcChatText = Language.GetTextValue("Mods.DDmod.NPCs.宠物管家史莱姆.Chat5");
                }
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
            if (Main.LocalPlayer.talkNPC == NPC.whoAmI|| CText>0)
            {
                CText--;
                string Text = "";
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
            Texture2D texture = Tail.Value;
            Vector2 A = Vector2.Zero;
            int F =NPC.frame.Y / NPC.frame.Height;
            if(F%6==1)
            {
                A.X = -2;
            }
            if(F%6==2)
            {
                A.X = -4;
                A.Y = -2;
            }
            if(F%6==3)
            {
                A.Y = -2;
            }
            if(F%6==4)
            {
                A.X = 2;
            }
            Main.EntitySpriteDraw(texture, NPC.Center - screenPos + new Vector2((8+ A.X) * (NPC.spriteDirection == 1 ? -1 : 1), -6+ A.Y), new Rectangle?(new Rectangle(0, texture.Height / 4 * Tailframe, texture.Width, texture.Height / 4)), drawColor, NPC.rotation, new Vector2(texture.Width, texture.Height / 4) / 2, NPC.scale, (SpriteEffects)(NPC.spriteDirection == -1 ? 0 : 1), 0);

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        int Tailframe;
        int Tailframe2;

        int TL = 0;
        int TLTime = 0;
    }
}