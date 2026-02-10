
using DDmod.Content.Dusts;
using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Boss.MiniBoss;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Boss.MiniBoss;
using DDmod.Content.Tiles.Trophy;
using DDmod.Helper;
using DDmod.Worlds;
using System.Linq;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.Utilities;

namespace DDmod.Content.NPCs.EliteMonster.四柱护卫
{
    [AutoloadBossHead]
    public class 星云护卫 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            DDSystem.HBar(NPC.type, "四柱/星云柱", new Vector2(-38, 0), Shield: true);
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = -20,
                PortraitPositionXOverride = -0F,
                PortraitScale = 1f,
                Scale = 1F,
                Position = new Vector2(0, 0),
                Rotation = 0F,
                Direction = 1,
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;

        }
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Zu;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/四柱护卫/星云护卫_Glow");
            Zu = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/四柱护卫/星云护卫玉足");
        }


        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPC.damage = 60;
            NPC.width = 72;
			NPC.height = 48;
			NPC.aiStyle = -1;
			NPC.defense = 20;
			NPC.lifeMax = 52000;
			NPC.knockBackResist = 0;
			NPC.value = Item.buyPrice(0, 50, 0, 0);
			NPC.alpha = 255;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.noGravity = true;
			NPC.scale = 1.3F;
            NPC.noTileCollide = true;
            NPC.Dnpc().Properties.Meat = true;
            NPC.boss = true;
            NPC.localAI[2] = 2;
            if (!Main.dedServ) Music = 34;
            NPC.Dnpc().Properties.BossLife = 1.325F;
        }
        public override void ModifyTypeName(ref string typeName)
        {
        }
        public override void BossHeadRotation(ref float rotation)
        {
            //rotation = NPC.rotation;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        int Landing = 0;
        public override void AI()
        {
            NPC.Dnpc().Bool[3] = false;
            SoundStyle sound = SoundID.Roar;
            sound.Pitch = -0.8F;
            sound.MaxInstances = 20;
            NPC.TargetClosest();
            Main.player[Main.myPlayer].nebulaMonolithShader = true;
            Player player = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || !player.active || player.dead)
            {
                NPC.velocity.Y -= 1;
                if (NPC.timeLeft > 5)
                {
                    NPC.timeLeft = 5;
                }
                return;
            }
            Vector2 vector = player.Center - NPC.Center;
            NPC.localAI[1] -= 0.03F;
            if (NPC.localAI[2] < 2)
            {
                NPC.localAI[2] += 0.06F;
            }
            NPC.spriteDirection = 0;
            if (vector.X < 0)
            {
                NPC.spriteDirection = 1;
            }
            if (NPC.Dnpc().Stage == 0)
            {
                NPC.velocity.X = 0;
                NPC.velocity.Y = -0.4F;
                if (NPC.alpha > 0)
                {
                    NPC.alpha -= 2;
                    for (int A = 0; A < 5; A++)
                    {
                        int D = NewDust(NPC.position, NPC.width, NPC.height, 255, 0f, 0f, 100, new Color(255, 31, 174, 55), 1f);
                        Main.dust[D].noGravity = true;
                        Main.dust[D].scale *= 0.5f + Main.rand.NextFloat(1, 2);
                        vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(3, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                        Main.dust[D].velocity = vector / 2;
                    }
                    Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 50, false, 0.1F);
                }
                else
                {
                    NPC.localAI[2] = 1;

                    Main.LocalPlayer.Dplayer().PlayerShake(30, 12);
                    sound.Pitch = 0.9F;
                    PlaySound(sound);
                    PlaySound(sound);
                    PlaySound(sound);
                    for (int t = 0; t < 4; t++)
                    {
                        for (int r = 0; r < 4; r++)
                        {
                            Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(255, 31, 174, 55))];
                            dust.noGravity = true;
                            dust.scale = 0.01f;
                            dust.alpha = -1;
                            dust.velocity = Vector2.Zero;
                            dust.customData = new Vector4(NPC.scale * 3, 40, r * 16, 1F);
                        }
                    }
                    NPC.Dnpc().Stage = 1;

                    if (!NPC.Dnpc().Bool[4])
                    {
                        NPC.Dnpc().Bool[4] = true;
                        for (int A = -5; A <= 5; A++)
                        {
                            if (A != 0)
                            {
                                float Rand = 0;
                                int Rand2 = Math.Abs(A);
                                int R = NPC.NewNPCProj(NPC.Center, new Vector2(0, -1).RotatedBy(Rand), ModContent.ProjectileType<星云触手>(), 50, 0, -1, NPC.whoAmI + 1, Rand2, 1.75F);
                                if (R != -1)
                                {
                                    if (A > 0)
                                    {
                                        Main.projectile[R].DProj().Times[0] = A / 5F*1.5F;
                                    }
                                    else
                                    {
                                        Main.projectile[R].DProj().Times[0] = A / 5F*1.5F;
                                    }
                                    Main.projectile[R].DProj().Bool[0] = A<0;
                                }
                            }
                        }
                    }
                    NPC.netUpdate = true;
                }
                return;
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.ai[0]++;
                if (NPC.localAI[2] >= 2 && NPC.ai[0] > 30)
                {
                    NPC.ai[0] = 0;
                        PreTP(player.Center + Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.Next(300, 500));
                        TP();

                    NPC.Dnpc().Stage = 2;
                    NPC.netUpdate = true;
                }
                return;
            }
            Vector2 P = player.Center - NPC.Center;
            //NPC.SmoothVelocity(P.PerfectNormalize() * (P.Length() / 2 > 50 ? 50 : P.Length() / 2), 20);
            if (NPC.ai[2] == 0)
            {
                NPC.ai[0]++;

                if (NPC.ai[0] == 100)
                {
                    PreTP(player.Center + Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.Next(300, 500));
                }
                if (NPC.ai[0] > 100 && NPC.ai[0] < 130)
                {
                    TPDust();
                }
                if (NPC.ai[0] == 130)
                {
                    TP();
                }
                if (NPC.ai[0] >= 230)
                {
                    NPC.ai[1]++;
                    NPC.ai[0] = 60;
                    if (NPC.ai[1] > 3)
                    {
                        AI0();
                    }
                }
                else
                if (NPC.ai[0] > 130)
                {
                    if (NPC.ai[0] % 3 == 0)
                    {
                        if (Main.netMode != 1)
                        {
                            Main.projectile[NPC.NewNPCProj(NPC.Center, P.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-2, 2)) * Main.rand.NextFloat(12, 20), 573, 30, 0)].tileCollide = false;
                        }
                    }
                }
            }
            else if (NPC.ai[2] == 1)
            {
                NPC.ai[0]++;

                if (NPC.ai[0] == 100)
                {
                    PreTP(player.Center + Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.Next(300, 500));
                }
                if (NPC.ai[0] > 100 && NPC.ai[0] < 130)
                {
                    TPDust();
                }
                if (NPC.ai[0] == 130)
                {
                    TP();
                }
                if (NPC.ai[0] >= 300)
                {
                    if (NPC.ai[0] >= 360)
                    {
                        NPC.ai[1]++;
                        NPC.ai[0] = 40;
                        if (NPC.ai[1] > 3)
                        {
                            AI0();
                        }
                    }
                }
                else
                if (NPC.ai[0] > 130)
                {
                    if (NPC.ai[0] % 3 == 0)
                    {
                        if (Main.netMode != 1)
                        {
                            Main.projectile[NPC.NewNPCProj(NPC.Center, Vector2.One.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(10, 16), ModContent.ProjectileType<Boss星云弹>(), 30, 0)].tileCollide = false;
                        }
                    }
                    if (NPC.ai[0] % 6 == 0)
                    {
                        if (Main.netMode != 1)
                        {
                            Main.projectile[NPC.NewNPCProj(NPC.Center, Vector2.One.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 10, ModContent.ProjectileType<Boss星云光束>(), 40, 0)].tileCollide = false;
                        }
                    }
                }
            }
            else if (NPC.ai[2] == 2)
            {
                NPC.ai[0]++;

                if (NPC.ai[0] > 100 && NPC.ai[0] < 130)
                {
                    PreTP(player.Center - new Vector2(0, 300) + player.velocity * 22);
                    TPDust();
                }
                if (NPC.ai[0] == 130)
                {
                    TP();
                    if (Main.netMode != 1)
                    {
                        Main.projectile[NPC.NewNPCProj(NPC.Center, new Vector2(0, 10), ModContent.ProjectileType<Boss星云光束>(), 100, 0, -1, 3,40,0)].tileCollide = false;
                    }
                }
                if (NPC.ai[0] >= 140)
                {
                    NPC.ai[1]++;
                    NPC.ai[0] = 70;
                    if (NPC.ai[1] > 3)
                    {
                        AI0();
                    }
                }
            }
            else if (NPC.ai[2] == 3)
            {
                NPC.ai[0]++;

                if (NPC.ai[0] > 100 && NPC.ai[0] < 130)
                {
                    PreTP(player.Center + Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.Next(300, 500));
                    TPDust();
                }
                if (NPC.ai[0] == 130)
                {
                    TP();
                }
                if (NPC.ai[0] >= 130 && NPC.ai[0] < 230)
                {
                    if (NPC.ai[0] % 3 == 0)
                    {
                        if (Main.netMode != 1)
                        {
                            Main.projectile[NPC.NewNPCProj(NPC.Center, Vector2.One.PerfectNormalize().RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(10, 16), ModContent.ProjectileType<Boss星云弹>(), 30, 0)].tileCollide = false;
                        }
                    }
                    if (NPC.ai[0] >= 200)
                    {
                        PreTP(player.Center - new Vector2(0, 300) + player.velocity * 22);
                        TPDust();
                    }
                }
                if(NPC.ai[0]==230)
                {
                    TP();
                    NPC.localAI[2] = 1;
                    NPC.frameCounter = 0;
                }
                if (NPC.ai[0] >= 250)
                {
                    NPC.Dnpc().Bool[3] = true;
                    if (NPC.frameCounter < 3)
                    {
                        NPC.frameCounter += 0.3f;
                    }
                    else
                    {
                        NPC.frameCounter = 3;
                    }
                    if (vector.Y < 120 && vector.Y > 0)
                    {
                        NPC.frameCounter = 4;
                    }
                    if (NPC.Dnpc().Times[1] == 0 && NPC.getRect().Intersects(player.getRect()))
                    {
                        NPC.Dnpc().Times[1] = player.whoAmI + 1;
                        NPC.netUpdate = true;
                    }
                }
                if (NPC.ai[0] == 270)
                {
                    sound.Pitch = 0.9F;
                    PlaySound(sound);
                    PlaySound(sound);
                    NPC.velocity.Y = 40;
                    NPC.velocity.X = 0;
                }
                if(NPC.Dnpc().Times[1]>0)
                {
                    NPC.velocity *= 0.96F;
                    Main.player[(int)(NPC.Dnpc().Times[1] - 1)].AddBuff(163,10);
                    Main.player[(int)(NPC.Dnpc().Times[1] - 1)].statLife-=2;
                    Main.player[(int)(NPC.Dnpc().Times[1] - 1)].Center = NPC.Center +new Vector2(0,100)+NPC.velocity;
                    if(Main.player[(int)(NPC.Dnpc().Times[1] - 1)].statLife<=0)
                    {
                        Main.player[(int)(NPC.Dnpc().Times[1] - 1)].KillMe(PlayerDeathReason.ByNPC(NPC.whoAmI),1,1);
                    }
                    if(NPC.ai[0]>400)
                    {
                        NPC.ai[1]++;
                        NPC.ai[0] = 70;
                        NPC.velocity.Y= -20;
                        DNPC.NewNPCs(NPC.GetSource_FromAI(), Main.player[(int)(NPC.Dnpc().Times[1] - 1)].Center,421,0);
                        NPC.Dnpc().Times[1] = 0;
                        if (NPC.ai[1] > 3)
                        {
                            AI0();
                        }
                    }
                }
                else
                if (NPC.ai[0] >= 290)
                {
                    NPC.ai[1]++;
                    NPC.ai[0] = 70;
                    NPC.velocity = Vector2.Zero;
                    if (NPC.ai[1] > 3)
                    {
                        AI0();
                    }
                }
            }
            else
            {
                NPC.ai[0]++;

                if (NPC.ai[0] == 100)
                {
                    PreTP(player.Center + new Vector2(Main.rand.NextBool(3)?0:Main.rand.NextFloat(-400,400), -Main.rand.NextFloat(300, 400)));
                    TP();
                    if (Main.netMode != 1)
                        Main.projectile[NPC.NewNPCProj(NPC.Center, new Vector2(0, 10), ModContent.ProjectileType<Boss星云光束>(), 60, 0, -1, 1.5F, 30, 0)].tileCollide = false;
                }
                if (NPC.ai[0] >= 100)
                {
                    NPC.ai[1]++;
                    NPC.ai[0] = 80;
                    NPC.velocity = Vector2.Zero;
                    if (NPC.ai[1] > 25)
                    {
                        AI1();
                    }
                }
            }
            NPC.alpha = 0;
            NPC.rotation = 0;
            void AI0()
            {
                NPC.ai[2]++;
                NPC.ai[0] = 0;
                NPC.ai[1] = 0;
            }
            void AI1(int A = -1)
            {
                NPC.ai[0] = 0;
                NPC.ai[1] = 0;
                if (A != -1) NPC.ai[2] = A;
            }
            void PreTP(Vector2 vector)
            {
                NPC.Dnpc().vector[0] = vector;
                NPC.netUpdate = true;
            }
            void TPDust()
            {
                for (int A = 0; A < 5; A++)
                {
                    int D = NewDust(NPC.Dnpc().vector[0]-new Vector2(4), 1, 1, 255, 0f, 0f, 100, new Color(255, 31, 174, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 0.5f + Main.rand.NextFloat(1, 2);
                    vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(3, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                    Main.dust[D].velocity = vector / 2;
                }
            }
            void TP()
            {
                for (int A = 0; A < 50; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, 255, 0f, 0f, 100, new Color(255, 31, 174, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 1.5f + Main.rand.NextFloat(1, 2);
                    vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(3, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                    Main.dust[D].velocity = vector;
                }
                NPC.Center = NPC.Dnpc().vector[0];
                NPC.velocity = Vector2.Zero;
                NPC.netUpdate = true;
                if (Main.netMode == 2)
                {
                    DDmod.SyncData(DDType.NPCCenter, NPC.whoAmI, -1, -1);
                }
                for (int A = 0; A < 50; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, 255, 0f, 0f, 100, new Color(255, 31, 174, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 1.5f + Main.rand.NextFloat(1, 2);
                    vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(3, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                    Main.dust[D].velocity = vector;
                }
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.NebulaPillar,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.星云护卫"))
            ]);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            DropOneByOne.Parameters parameters = default(DropOneByOne.Parameters);
            parameters.MinimumItemDropsCount = 8;
            parameters.MaximumItemDropsCount = 12;
            parameters.ChanceNumerator = 1;
            parameters.ChanceDenominator = 1;
            parameters.MinimumStackPerChunkBase = 2;
            parameters.MaximumStackPerChunkBase = 4;
            parameters.BonusMinDropsPerChunkPerPlayer = 1;
            parameters.BonusMaxDropsPerChunkPerPlayer = 2;
            DropOneByOne.Parameters parameters2 = parameters;
            DropOneByOne.Parameters parameters3 = parameters2;
            DropOneByOne.Parameters parameters4 = parameters3;
            parameters3.BonusMinDropsPerChunkPerPlayer = 2;
            parameters3.BonusMaxDropsPerChunkPerPlayer = 3;
            parameters4.BonusMinDropsPerChunkPerPlayer = 3;
            parameters4.BonusMaxDropsPerChunkPerPlayer = 4;
            npcLoot.Add(new DropBasedOnCompleteMode(new DropOneByOne(3457, parameters2), new DropOneByOne(3457, parameters3), new DropOneByOne(3457, parameters4)));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<星云遗物>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<星云护卫纪念章>(), 10));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<星云护卫圣物>()));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                NPC.alpha = 0;
                NPC.spriteDirection = 0;
            }
            SpriteEffects spriteEffects = (SpriteEffects)NPC.spriteDirection;
            float C = 1- NPC.alpha / 255F;
            Texture2D texture = Glow.Value;
            Rectangle rectangle = NPC.frame;
            rectangle.Y = (int)(NPC.Dnpc().Times[0] / 5) % 6 * NPC.frame.Height;
            Vector2 origin = NPC.frame.Size() / 2 - new Vector2(0,20);
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, new Color(255, 255, 255, 0)* C, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
            int L = NPC.oldPos.Length;
            if (NPC.Dnpc().Stage > 1)
            {
                for (int i = 0; i < L; i++)
                {
                    Vector2 vector2 = NPC.oldPos[i] - screenPos + NPC.Size / 2;
                    Color color = new Color(255, 31, 174, 55) * ((L - i) / (float)L)*0.5F;
                    spriteBatch.Draw(Zu.Value, vector2, rectangle, color * C, NPC.oldRot[i], origin, NPC.scale, spriteEffects, 0f);
                    spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, vector2, NPC.frame, color * C, NPC.oldRot[i], origin, NPC.scale, spriteEffects, 0f);
                    color = new Color(255, 255, 255, 100) * ((L - i) / (float)L) * 0.1F;
                    spriteBatch.Draw(texture, vector2, NPC.frame, color * C, NPC.oldRot[i], origin, NPC.scale, spriteEffects, 0f);
                }
            }
            /*
            if (NPC.localAI[1] > 0)
            {
                Main.spriteBatch.Draw(DDTextures.Wire.Value, NPC.Center - screenPos, null, new Color(34, 221, 151, 55) * (NPC.localAI[1] * 3), spriteEffects == 0 ? 0 : MathHelper.Pi, new Vector2(0, 1), new Vector2(5, 18) * (1 - NPC.localAI[1]), 0, 0f);
            }*/
            texture = TextureAssets.Npc[NPC.type].Value;
            if (NPC.localAI[2] < 2)
            {
                for (int A = 0; A < 3; A++)
                {
                    spriteBatch.Draw(Zu.Value, NPC.Center - screenPos, rectangle, new Color(255, 31, 174, 55) * (2 - NPC.localAI[2]) * 2 * C, NPC.rotation, origin, NPC.scale * NPC.localAI[2], spriteEffects, 0f);
                    spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, new Color(255, 31, 174, 55) * (2 - NPC.localAI[2]) * 2 * C, NPC.rotation, origin, NPC.scale * NPC.localAI[2], spriteEffects, 0f);
                    spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, NPC.frame, new Color(255, 255, 255, 0) * C, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                }
            }
            spriteBatch.Draw(Zu.Value, NPC.Center  - screenPos, rectangle, drawColor * C, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
            spriteBatch.Draw(texture, NPC.Center  - screenPos, NPC.frame,drawColor * C, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
            texture = Glow.Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, Color.White * C, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);



 
            //spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position - screenPos, null, Color.White * 0.5F, 0, Vector2.Zero, NPC.Size/2, 0, 0f);

            //spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position+new Vector2(0,110) - screenPos, null, Color.White * 0.5F, 0, Vector2.Zero, new Vector2(NPC.width/2,20), 0, 0f);
            return false;
        }
        float speed = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Width = 222;
            NPC.frame.X = NPC.Dnpc().Bool[3]?222:0;
            NPC.oldRot[0] = NPC.rotation;

            for (int a = NPC.oldRot.Length - 1; a > 0; a--)
            {
                NPC.oldRot[a] = NPC.oldRot[a - 1];
            }
            NPC.localAI[3]-=2;
            if (!NPC.Dnpc().Bool[3])
            {
                NPC.frameCounter++;
                if (NPC.frameCounter % 5 == 0)
                {
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y >= frameHeight * 6)
                {
                    NPC.frame.Y = frameHeight;
                }
            }
            else
            {
                NPC.frame.Y = (int)(NPC.frameCounter* frameHeight);
            }
                NPC.Dnpc().Times[0]++;
        }
        
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.星云护卫, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
		{
			for (int i = 0; i < 12; i++)
            {
                int D = NewDust(NPC.position, NPC.width, NPC.height,255, 0f, 0f, 100, new Color(255, 31, 174, 55), 1f);
                Main.dust[D].noGravity = true;
                Main.dust[D].scale *= 1.7f + Main.rand.Next(1);
                Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                Main.dust[D].velocity = vector;
            }
			if (NPC.life <= 0)
			{
                PlaySound(NPC.DeathSound, NPC.Center);
				for (int A = 0; A < 220; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height,255, 0f, 0f, 100, new Color(255, 31, 174, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 2.7f + Main.rand.NextFloat(1,3);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(3, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                    Main.dust[D].velocity = vector * 2;
                }
                if (Main.netMode != 2)
                    for (int A = 1; A <= 9; A++)
                {
                    int GoreType = Mod.Find<ModGore>("星云护卫" + A).Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(hit.HitDirection, -1), GoreType, NPC.scale);
                }
            }
		}
	}
}
