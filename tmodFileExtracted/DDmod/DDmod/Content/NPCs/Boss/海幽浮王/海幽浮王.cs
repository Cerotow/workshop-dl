using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MeteorAnnihilatorItems;
using DDmod.Content.Items.Boss.海幽浮王;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Content.Tiles.Trophy;
using DDmod.Content.Tiles.农场;
using DDmod.Sync;
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.海幽浮王
{
    [AutoloadBossHead]
    public class 海幽浮王 : ModNPC
    {
        public static int Head;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 7;
            DDSystem.HBar(NPC.type, "海幽浮王", new Vector2(-1146, 0));
        }

        public override void SetDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPC.damage = 40;
            NPC.width = 92;
            NPC.height = 92;
            NPC.defense = 12;
            NPC.lifeMax = 4150;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.canGhostHeal = false;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.scale = 1.3F;
            NPC.value = 12000f;
            NPC.boss = true;
            NPC.NPCHB().Multiple = true;
            NPC.alpha = 255;
            NPC.Dnpc().Properties.Water = true;
            NPC.Dnpc().Properties.Meat = true;
            if (!Main.dedServ) Music = DDSystem.Music(2, "水母王");
            NPC.Dnpc().Properties.BossLife = 1.125F;
            /*
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.Dnpc().Properties = NPCProperties.Iron;*/
        }
        public int moveSpeed;
        public int moveSpeedY;
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.海幽浮王"))
            });
        }
        public override void BossLoot( ref int potionType)
        {
            potionType = 188;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<海幽浮王宝藏袋>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<海幽浮王纪念章物品>(), 10));
            //面具
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<海幽浮王面具>(), 10));
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<海幽浮王圣物>()));
            //大师掉落物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<幽灵海螺>(), 4));
            
            //特别引用,普通模式
            int[] A = new int[2];
            A[0] = ModContent.ItemType<海幽浮法杖>();
            A[1] = ModContent.ItemType<水球炮>();

            npcLoot.NormalLoot(1,A);
            
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.海幽浮王, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            bool fury = !player.ZoneBeach;
            Vector2 vector = player.Center - NPC.Center;
            NPC.localAI[0]++;
            if (NPC.localAI[0] >= 7 * 8)
            {
                NPC.localAI[0] = 0;
            }
            if(NPC.localAI[1]++<120)
            {
                if (NPC.localAI[1]==1)
                {
                    NPC.position.Y +=80;
                }
                NPC.velocity = new Vector2(0, -1);
                NPC.alpha -= 5;
                NPC.netUpdate = true;
                return;
            }
            float rot = 0.02f;
            if(fury)
            {
                NPC.ai[2] *= 1.05F;
                rot = 0.06F;
            }
            NPC.ai[0]++;
            if (NPC.Dnpc().Stage == 0)
            {
                if (NPC.ai[0] < 300)
                {
                    NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * NPC.ai[2];

                    NPC.ai[2] *= 0.94F;
                    if (NPC.localAI[0] == 6 * 8)
                    {
                        NPC.ai[2] = Main.rand.NextFloat(8, 12);
                    }
                    NPC.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, rot);
                    if (NPC.ai[0] % 30 == 0&&Main.netMode!=1)
                    {
                        NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.NextFloat(-300, 300), Main.rand.NextFloat(-300, 300)), Vector2.Zero, ModContent.ProjectileType<Boss水球>(), 15, 1, -1, NPC.whoAmI);
                    }
                }
                else
                if (NPC.ai[0] < 600)
                {
                    NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * NPC.ai[2];

                    NPC.ai[2] *= 0.94F;
                    if (NPC.localAI[0] == 6 * 8)
                    {
                        NPC.ai[2] = Main.rand.NextFloat(8, 12);
                    }
                    NPC.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, rot);
                    if (NPC.ai[0] == 400 && Main.netMode != 1)
                    {
                        for (int a = 0; a < 6; a++)
                        {
                            int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.NextFloat(-300, 300), Main.rand.NextFloat(-300, 300)), Vector2.Zero, ModContent.ProjectileType<Boss水球>(), 15, 1, -1, NPC.whoAmI, 1, 100);
                            Main.projectile[Proj].DProj().vector[0] = new Vector2(a, 6);
                        }
                    }
                    if (NPC.ai[0] == 500 && Main.netMode != 1)
                    {
                        for (int a = 0; a < 6; a++)
                        {
                            int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.NextFloat(-300, 300), Main.rand.NextFloat(-300, 300)), Vector2.Zero, ModContent.ProjectileType<Boss水球>(), 15, 1, -1, NPC.whoAmI, 2, 200);
                            Main.projectile[Proj].DProj().vector[0] = new Vector2(a, 6);
                        }
                    }
                }
                else
                if (NPC.ai[0] < 800)
                {
                    NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * NPC.ai[2];

                    NPC.ai[2] *= 0.94F;
                    if (NPC.localAI[0] == 6 * 8)
                    {
                        NPC.ai[2] = Main.rand.NextFloat(8, 12);
                    }
                    NPC.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, rot);
                }
                else
                if (NPC.ai[0] < 1100)
                {
                    NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * NPC.ai[2];

                    NPC.ai[2] *= 0.94F;
                    if (NPC.localAI[0] == 6 * 8)
                    {
                        NPC.ai[2] = Main.rand.NextFloat(16, 20);
                    }
                    NPC.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, rot);
                }
                else
                {
                    NPC.ai[0] = 0;
                }
                if (NPC.life < NPC.lifeMax / 2)
                {
                    NPC.Dnpc().Stage = 1;
                    NPC.ai[0]=0;
                }
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.ai[2] *= 0.94F;
                NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * NPC.ai[2];
                NPC.dontTakeDamage = true;
                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 10, false, 0.1F);
                NPC.RotationSpeed(0, rot);
                NPC.ai[0]++;
                if(NPC.ai[0]== 120 && Main.netMode != 1)
                {
                    DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 800), ModContent.NPCType<海幽浮>(), 0);
                }
                if(NPC.ai[0]>=300)
                {
                    for(int a= 0;a<100;a++)
                    {
                        int D = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), Main.rand.NextFloat(-10,10), Main.rand.NextFloat(-10, 10), 100, new Color(100, 150, 255,100), Main.rand.NextFloat(1, 3));
                        Main.dust[D].velocity = new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5));
                        Main.dust[D].customData = Main.dust[D].DustAI(1);
                    }
                    SoundStyle sound = SoundID.NPCDeath1;
                    sound.Pitch = -0.4F;
                    PlaySound(sound, NPC.position);
                    NPC.Dnpc().Stage = 2;
                    NPC.ai[0] = 0;
                }
            }
            else if (NPC.Dnpc().Stage == 2)
            {
                NPC.dontTakeDamage = false;
                if (NPC.ai[0] < 300)
                {
                    NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * NPC.ai[2];

                    NPC.ai[2] *= 0.94F;
                    if (NPC.localAI[0] == 6 * 8)
                    {
                        NPC.ai[2] = Main.rand.NextFloat(14, 18);
                    }
                    NPC.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, rot);
                    if (NPC.ai[0] % 30 == 0 && Main.netMode != 1)
                    {
                        int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.NextFloat(-300, 300), Main.rand.NextFloat(-300, 300)), Vector2.Zero, ModContent.ProjectileType<Boss水球>(), 21, 1, -1, NPC.whoAmI,3);
                    }
                }
                else
                if (NPC.ai[0] < 600)
                {
                    NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * NPC.ai[2];

                    NPC.ai[2] *= 0.94F;
                    if (NPC.localAI[0] == 6 * 8)
                    {
                        NPC.ai[2] = Main.rand.NextFloat(14, 18);
                    }
                    NPC.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, rot);
                    if (NPC.ai[0] == 400 && Main.netMode != 1)
                    {
                        for (int a = 0; a < 6; a++)
                        {
                            int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.NextFloat(-300, 300), Main.rand.NextFloat(-300, 300)), Vector2.Zero, ModContent.ProjectileType<Boss水球>(),21, 1, -1, NPC.whoAmI, 4, 100);
                            Main.projectile[Proj].DProj().vector[0] = new Vector2(a, 6);
                        }
                    }
                    if (NPC.ai[0] == 500 && Main.netMode != 1)
                    {
                        for (int a = 0; a < 6; a++)
                        {
                            int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.NextFloat(-300, 300), Main.rand.NextFloat(-300, 300)), Vector2.Zero, ModContent.ProjectileType<Boss水球>(), 21, 1, -1, NPC.whoAmI, 5, 200);
                            Main.projectile[Proj].DProj().vector[0] = new Vector2(a, 6);
                        }
                    }
                }
                else
                if (NPC.ai[0] < 800)
                {
                    NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * NPC.ai[2];

                    NPC.ai[2] *= 0.94F;
                    if (NPC.localAI[0] == 6 * 8)
                    {
                        NPC.ai[2] = Main.rand.NextFloat(14, 18);
                    }
                    NPC.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, rot);
                }
                else
                if (NPC.ai[0] < 1100)
                {
                    NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * NPC.ai[2];

                    NPC.ai[2] *= 0.94F;
                    if (NPC.localAI[0] == 6 * 8)
                    {
                        NPC.ai[2] = Main.rand.NextFloat(24, 28);
                    }
                    NPC.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, rot);
                }
                else
                {
                    NPC.ai[0] = 0;
                }
            }
            return;
            if (Main.rand.NextBool(60))
            {
                // for(int a =0;a<20;a++)
                //NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector.PerfectNormalize() * 8, 466, 30, 1,-1,2,2);
                for (int a = 0; a < 4; a++)
                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -Main.rand.NextFloat(4, 10)) + vector.PerfectNormalize() * Main.rand.NextFloat(20, 30), ModContent.ProjectileType<Boss水球>(), 30, 1);

                for (int a = 0; a < 10; a++)
                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -10) + vector.PerfectNormalize() * 12, ModContent.ProjectileType<Boss水球>(), 30, 1, -1, 1, 10, a);
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 200; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, 33, Main.rand.NextFloat(-12, 12), Main.rand.NextFloat(-8, 8), 100, default, NPC.scale * 1.3F);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    int GoreType = Mod.Find<ModGore>("海幽浮王1").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8)), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("海幽浮王2").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8)), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("海幽浮王3").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, GoreType, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8)), Vector2.Zero, GoreType, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8)), Vector2.Zero, GoreType, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8)), Vector2.Zero, GoreType, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8)), Vector2.Zero, GoreType, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center + new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8)), Vector2.Zero, GoreType, NPC.scale);
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            NPC.frame.Y = frameHeight * ((int)NPC.localAI[0] / 8);
            //Main.NewText(((int)NPC.localAI[0] / 8));
        }
        Vector4[] vectors = new Vector4[4];
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player player = Main.player[NPC.target];
            SpriteEffects sprite = 0;
            if (NPC.spriteDirection == 1)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            if (!NPC.IsABestiaryIconDummy)
            {
                drawColor = NPC.GetAlpha(drawColor);
            }
                Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Rectangle rectangle = NPC.frame;
            if (NPC.Dnpc().Stage == 2)
            {
                Color color = NPC.GetAlpha(new Color(10, 100, 255, 0));
                color.A = 0;
                spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, NPC.GetAlpha(Color.White), NPC.rotation, new Vector2(texture.Width, texture.Height / 7) / 2, NPC.scale, sprite, 0f);
                spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, color * 0.4f, NPC.rotation, new Vector2(texture.Width, texture.Height / 7) / 2, NPC.scale, sprite, 0f);
                for (int a = 0; a < NPC.oldPos.Length; a++)
                {
                    Vector2 vector = NPC.oldPos[a] + NPC.Size / 2;
                    spriteBatch.Draw(texture, vector - screenPos, rectangle, color * 0.4f * (1 - (float)a / NPC.oldPos.Length), NPC.rotation, new Vector2(texture.Width, texture.Height / 7) / 2, NPC.scale, sprite, 0f);
                }
                return false;
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, drawColor, NPC.rotation, new Vector2(texture.Width, texture.Height / 7) / 2, NPC.scale, sprite, 0f);
            for (int a = 0; a < NPC.oldPos.Length; a++)
            {
                Vector2 vector = NPC.oldPos[a] + NPC.Size / 2;
                spriteBatch.Draw(texture, vector - screenPos, rectangle, drawColor*0.4f * (1 - (float)a / NPC.oldPos.Length), NPC.rotation, new Vector2(texture.Width, texture.Height / 7) / 2, NPC.scale, sprite, 0f);
            }
            return false;
        }
    }
}