using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MeteorAnnihilatorItems;
using DDmod.Content.Items.Boss.蘑菇王;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Magic;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.NPCs.Boss.MeteorAnnihilator
{
    [AutoloadBossHead]
    public class MeteorAnnihilator : ModNPC
    {
        public static Asset<Texture2D> NPCTexture;
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> 尾气;
        public static int Head;
        public override void BossHeadSlot(ref int index)
        {
            Head = ModContent.GetModBossHeadSlot("DDmod/Content/NPCs/Boss/MeteorAnnihilator/MeteorAnnihilator2_Head_Boss");
            if (NPC.Dnpc().Stage >= 1)
            {
                index = Head;
            }
        }
        public override void Load()
        {
            if (!Main.dedServ)
            {
                NPCTexture = ModContent.Request<Texture2D>(Texture);
                Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
                尾气 = ModContent.Request<Texture2D>(Texture + "_尾气");

            }
        }
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                DDSystem.HBar(NPC.type, "流星血条", new Vector2(4, -2));
                NPCHealthBar.Head[NPC.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星血条/歼灭者Head");
                NPCHealthBar.Tail[NPC.type] = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星血条/Tail2");
            }
        }

        public override void SetDefaults()
        {
            NPC.damage = 50;
            NPC.width = 92;
            NPC.height = 92;
            NPC.defense = 12;
            NPC.lifeMax = 7000;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            Main.npcFrameCount[Type] = 4;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.Item14;
            NPC.netAlways = true;
            NPC.scale = 1.1F;
            NPC.value = 12000f;
            NPC.boss = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            if (!Main.dedServ) Music = MusicID.Boss3 ;
            NPC.Dnpc().Properties.Iron = true;
            if (Main.netMode != 2)
            {
                NPC.NPCHB().HealthBarFrame(new Vector2(1, 0), new Vector2(1, 0), new Vector2(4, 4), new Vector2(1, 0));
                //NPC.NPCHB().HealthBarFrame(new Vector2(1, 0), new Vector2(1, 0), new Vector2(4, 4), new Vector2(1, 0));
            }
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 4;
            NPC.Dnpc().Properties.BossLife = 1.15F;
        }
        public int moveSpeed;
        public int moveSpeedY;
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.MeteorAnnihilator"))
            });
        }
        public override void BossLoot( ref int potionType)
        {
            potionType = 188;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //宝藏袋掉落
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<流星箱>()));
            //纪念章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<流星歼灭者纪念章>(), 10));
            //面具
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MeteorDiggerMask>(), 10));
            //大师圣物
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<流星歼灭者圣物>()));
            //大师掉落物
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<流星遥控手环>(), 4));

            //特别引用,普通模式
            int[] A = new int[2];
            A[0] = ModContent.ItemType<流星飞盘物品>();
            A[1] = ModContent.ItemType<流星Y形无人机控制器>();

            npcLoot.NormalLoot(1,A);
            npcLoot.SpecialLoot(ModContent.ItemType<哈迪斯之刃物品>(), 1);
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.downedMeteorAnnihilator, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
            base.BossHeadRotation(ref rotation);
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void AI()
        {
            NPC.oldRot[0] = NPC.rotation;

            for (int a = NPC.oldRot.Length - 1; a > 0; a--)
            {
                NPC.oldRot[a] = NPC.oldRot[a - 1];
            }
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            Vector2 direction = player.Center - NPC.Center;
            direction.DirectPerfectNormalize();
            DGlobalNPC NPCAI = NPC.Dnpc();
            NPCAI.Times[4]--;
            if (NPC.Dnpc().Stage > 0)
            {
                if (!Main.dedServ)
                {
                    Music = DDSystem.Music(2, "流星歼灭者");
                    NPC.NPCHB().Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar+ "流星血条/歼灭者Head2");
                    NPC.NPCHB().Mid2 = ModContent.Request<Texture2D>(DDSystem.HealthBar+ "流星血条/Mid2");
                    NPC.NPCHB().Tail2 = ModContent.Request<Texture2D>(DDSystem.HealthBar+ "流星血条/Tail3");
                    NPC.NPCHB().Fill2 = ModContent.Request<Texture2D>(DDSystem.HealthBar+ "流星血条/Fill2");
                    NPC.NPCHB().Lock2 = ModContent.Request<Texture2D>(DDSystem.HealthBar+ "流星血条/Lock2");
                    NPC.NPCHB().HealthBarFrame(new Vector2(1, 0), new Vector2(1, 0), new Vector2(4, 4), new Vector2(1, 0));
                }
            }
            if (!Main.player[NPC.target].dead)
            {
                NPC.damage = 0;
                if (NPC.Dnpc().Stage == 0)
                {
                    if (NPC.life > NPC.lifeMax *0.75F)
                    {
                        if (WYE < 1)
                        {
                            WYE += 0.05F;
                        }
                        else
                        {
                            WYE = 1;
                        }
                        DDHelper.BackAndForth(-1, 1, 0.01F, ref NPCAI.Times[2], ref NPCAI.Bool[2]);
                        Vector2 vector4 = Vector2.Subtract(player.Center - new Vector2(0, 400).RotatedBy(NPCAI.Times[2])/*调整npc要去的位置*/, NPC.Center);
                        float Q = vector4.Length() / 2;
                        float E = Q;
                        if (E > 30)
                        {
                            E = 30;
                        }
                        if (E < 0.5F)
                        {
                            E = 0;
                        }
                        vector4.DirectPerfectNormalize();
                        vector4 *= E;//速度
                        NPC.velocity = (NPC.velocity * 9 + vector4) / 10;
                        NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;
                        if (player.Center.Y-NPC.Center.Y>0)
                        {
                            switch (NPC.ai[0])
                            {
                                case 0:
                                    NPCAI.Times[0]++;
                                    if (NPCAI.Times[0] % 25 == 0)
                                    {
                                        NPCAI.Times[1]++;
                                        int type = ModContent.ProjectileType<Boss机械橙激光>();
                                        for (int a = -1; a <= 1; a++)
                                        {
                                            NPC.NewNPCProj(NPC.Center + (NPC.rotation).ToRotationVector2() * (10 * a) + (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * 80, direction * 6, type, 12, 0, -1, 0, 0.5F);
                                        }
                                        NPCAI.Times[4] = 5;
                                        PlaySound(SoundID.Item12, NPC.position);
                                    }
                                    if (NPCAI.Times[0] > 300)
                                    {
                                        NPCAI.Times[0] = 0;
                                        NPC.ai[0]++;
                                    }
                                    break;
                                case 1:
                                    NPCAI.Times[0]++;
                                    if (NPCAI.Times[0] % 50 == 0 && NPCAI.Times[0] <= 150)
                                    {
                                        NPCAI.Times[1]++;
                                        if (Main.netMode != 1)
                                        {
                                            for (int a = -1; a <= 1; a++)
                                            {
                                                Vector2 vector = new Vector2(30 * a, 0).RotatedBy(NPC.rotation);
                                                if (a != 0)
                                                {
                                                    NPC npc = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center + vector, ModContent.NPCType<歼灭者导弹>(), 0)];
                                                    npc.velocity = direction.RotatedBy(-1 * a) * 15;
                                                }
                                            }
                                        }
                                        PlaySound(SoundID.Item12, NPC.position);
                                    }
                                    if (NPCAI.Times[0] > 300)
                                    {
                                        NPCAI.Times[0] = 0;
                                        NPC.ai[0] = 0;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (WYE > 0)
                        {
                            WYE -= 0.1F;
                        }
                        else
                        {
                            WYE = 0;
                        }
                        NPC.ai[1]++;
                        NPC.noTileCollide = false;
                        NPC.noGravity = false;
                        NPC.dontTakeDamage = true;
                        NPC.rotation += NPC.velocity.X*0.05F;
                        if (NPC.velocity.Y == 0)
                            NPC.velocity.X *= 0.98F;
                        if (NPC.ai[1] > 150)
                        {
                            if (!Main.dedServ)
                            {
                                Music = DDSystem.Music(2, "流星歼灭者");
                                NPC.NPCHB().Head2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星血条/歼灭者Head2");
                                NPC.NPCHB().Mid2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星血条/Mid2");
                                NPC.NPCHB().Tail2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星血条/Tail3");
                                NPC.NPCHB().Fill2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星血条/Fill2");
                                NPC.NPCHB().Lock2 = ModContent.Request<Texture2D>(DDSystem.HealthBar + "流星血条/Lock2");
                                NPC.NPCHB().HealthBarFrame(new Vector2(1, 0), new Vector2(1, 0), new Vector2(4, 4), new Vector2(1, 0));
                            }
                        }
                        if (NPC.ai[1]>200)
                        {
                            NPC.Dnpc().Stage = 1;
                            NPC.ai[0] = 0;
                            NPCAI.Bool[0] = true;
                        }
                    }
                }
                else if (NPC.Dnpc().Stage == 1)
                {
                    if (WYE < 1)
                    {
                        WYE += 0.2F;
                    }
                    else
                    if (WYE > 1.03F)
                    {
                        WYE -= 0.02F;
                    }
                    else
                    {
                        WYE = 1;
                    }
                    NPC.noTileCollide = true;
                    NPC.noGravity = true;
                    NPC.dontTakeDamage = false;
                    if (NPCAI.Bool[0])
                    {
                        NPC.ai[1] -= 5;
                        if (NPC.ai[1] < 0)
                        {
                            NPC.ai[1] = 0;
                            NPCAI.Bool[0] = false;
                            NPC.ai[1] = Main.rand.Next(1,4);
                        }
                        NPC.dontTakeDamage = true;
                        //要去的位置
                        Vector2 vector4 = Vector2.Subtract(player.Center + new Vector2(-500, 0).RotatedBy(NPC.ai[1] * MathHelper.PiOver4)/*调整npc要去的位置*/, NPC.Center);
                        float Q = vector4.Length() / 10;
                        float E = Q;
                        if (E > 20)
                        {
                            E = 20;
                        }
                        if (E < 0.1F)
                        {
                            E = 0;
                        }
                        if (E > 0 && E < 2)
                        {
                            E = 2;
                        }
                        vector4.Normalize();
                        vector4 *= E;//速度

                        if (NPC.ai[1] < 120)
                        {
                            NPC.velocity = (NPC.velocity * 120 + vector4) / 121;
                            NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;
                        }
                        else
                        {
                            NPC.velocity += (NPC.rotation-MathHelper.PiOver2).ToRotationVector2()*2;
                        }
                        return;
                    }
                    if (NPC.life < NPC.lifeMax * 0.5F)
                    {
                        NPC.Dnpc().Stage = 2;
                    }
                    switch (NPC.ai[0])
                    {
                        case 0:
                            //要去的位置
                            Vector2 vector4 = Vector2.Subtract(player.Center + new Vector2(-500, 0).RotatedBy(NPC.ai[1]*MathHelper.PiOver4)/*调整npc要去的位置*/, NPC.Center);
                            float Q = vector4.Length() / 10;
                            float E = Q;
                            if (E > 20)
                            {
                                E = 20;
                            }
                            if (E < 0.1F)
                            {
                                E = 0;
                            }
                            if (E > 0 && E < 2)
                            {
                                E = 2;
                            }
                            vector4.Normalize();
                            vector4 *= E;//速度

                            NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;
                            NPC.velocity = (NPC.velocity * 20 + vector4) / 21;
                            NPCAI.Times[0]++;
                            if (NPCAI.Times[0] % 20 == 0&& NPCAI.Times[4]>40)
                            {
                                NPCAI.Times[1]++;
                                int type = ModContent.ProjectileType<BossGreenLaser2>();
                                
                                NPC.NewNPCProj(NPC.Center+direction*76, direction * 6, type, 12, 0, -1);
                                PlaySound(SoundID.Item12, NPC.position);
                            }
                            if (NPCAI.Times[0] > 300)
                            {
                                NPCAI.Times[0] = 0;
                                NPC.ai[0]++;
                                NPC.ai[1] = Main.rand.Next(1, 4);

                            }
                            if (NPCAI.Times[4] < 50)
                            {
                                NPCAI.Times[4] +=10;
                            }
                            break;
                        case 1:
                            //要去的位置
                            vector4 = Vector2.Subtract(player.Center + new Vector2(-500, 0).RotatedBy(NPC.ai[1] * MathHelper.PiOver4)/*调整npc要去的位置*/, NPC.Center);
                            Q = vector4.Length() / 10;
                            E = Q;
                            if (E > 20)
                            {
                                E = 20;
                            }
                            if (E < 0.1F)
                            {
                                E = 0;
                            }
                            if (E > 0 && E < 2)
                            {
                                E = 2;
                            }
                            vector4.Normalize();
                            vector4 *= E;//速度

                            NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;
                            NPC.velocity = (NPC.velocity * 20 + vector4) / 21;
                            NPCAI.Times[0]++;
                            if (NPCAI.Times[0] % 50 == 0 && NPCAI.Times[0] <= 150)
                            {
                                NPCAI.Times[1]++;
                                if (Main.netMode != 1)
                                {
                                    for (int a = -1; a <= 1; a++)
                                    {
                                        Vector2 vector = new Vector2(30 * a, 0).RotatedBy(NPC.rotation);
                                        if (a != 0)
                                        {
                                            NPC npc = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center + vector, ModContent.NPCType<歼灭者导弹2>(), 0)];
                                            npc.velocity = direction.RotatedBy(1 * a) * 25;
                                        }
                                    }
                                }
                                PlaySound(SoundID.Item12, NPC.position);
                            }
                            if (NPCAI.Times[0] > 300)
                            {
                                NPCAI.Times[0] = 0;
                                NPC.ai[0]++;
                                NPC.ai[1] = Main.rand.Next(1, 4);
                                NPC.velocity = (NPC.rotation-MathHelper.PiOver2).ToRotationVector2();
                            }
                            break;
                        case 2:
                            NPC.damage = NPC.defDamage;
                            if (NPCAI.Times[0]>240)
                            {
                                NPCAI.Times[0]++;
                            }
                            if (DDHelper.SpecifyDirection(NPC.velocity.ToRotation(), direction.ToRotation(), 0.5F))
                            {
                                NPCAI.Times[0]++;
                                if (NPCAI.Times[0] % 60 == 0)
                                {
                                    NPC.velocity = NPC.velocity.PerfectNormalize() * 40;
                                    NPC.ai[2] = 60;
                                    WYE = 1.5f;
                                }
                            }
                            vector4 = Vector2.Subtract(player.Center/*调整npc要去的位置*/, NPC.Center);
                            vector4.DirectPerfectNormalize();
                            NPC.velocity = (NPC.velocity * 10 + vector4*6) / 11;
                            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
                            if (NPCAI.Times[0] > 300)
                            {
                                NPCAI.Times[0] = 0;
                                NPC.ai[0]=0;
                                NPC.ai[1] = Main.rand.Next(1, 4);

                            }
                            break;
                    }
                }
                else if (NPC.Dnpc().Stage == 2)
                {
                    if (WYE < 1)
                    {
                        WYE += 0.2F;
                    }
                    else
                    if (WYE > 1.03F)
                    {
                        WYE -= 0.02F;
                    }
                    else
                    {
                        WYE = 1;
                    }

                    if (NPC.life < NPC.lifeMax * 0.25F)
                    {
                        NPC.Dnpc().Stage = 3;
                    }
                    NPC.noTileCollide = true;
                    NPC.noGravity = true;
                    NPC.dontTakeDamage = false;
                    switch (NPC.ai[0])
                    {
                        case 0:
                            //要去的位置
                            Vector2 vector4 = Vector2.Subtract(player.Center + new Vector2(-500, 0).RotatedBy(NPC.ai[1] * MathHelper.PiOver4)/*调整npc要去的位置*/, NPC.Center);
                            float Q = vector4.Length() / 10;
                            float E = Q;
                            if (E > 20)
                            {
                                E = 20;
                            }
                            if (E < 0.1F)
                            {
                                E = 0;
                            }
                            if (E > 0 && E < 2)
                            {
                                E = 2;
                            }
                            vector4.Normalize();
                            vector4 *= E;//速度

                            NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;
                            NPC.velocity = (NPC.velocity * 20 + vector4) / 21;
                            NPCAI.Times[0]++;
                            if (NPCAI.Times[0] % 20 == 0 && NPCAI.Times[4] > 40)
                            {
                                NPCAI.Times[1]++;
                                int type = ModContent.ProjectileType<BossGreenLaser2>();
                                NPC.NewNPCProj(NPC.Center + direction * 76, direction * 4, type, 12, 0, -1);
                                if (Main.netMode != 1)
                                {
                                    for (int a = -1; a <= 1; a++)
                                    {
                                        Vector2 vector = new Vector2(30 * a, 0).RotatedBy(NPC.rotation);
                                        if (a != 0)
                                        {
                                            NPC npc = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center + vector, ModContent.NPCType<歼灭者导弹2>(), 0)];
                                            npc.velocity = direction.RotatedBy(1.5 * a) * 35;
                                        }
                                    }
                                }
                                PlaySound(SoundID.Item12, NPC.position);
                            }
                            if (NPCAI.Times[0] > 300)
                            {
                                NPCAI.Times[0] = 0;
                                NPC.ai[0]++;
                                NPC.ai[1] = Main.rand.Next(1, 4);

                            }
                            if (NPCAI.Times[4] < 50)
                            {
                                NPCAI.Times[4] += 10;
                            }
                            break;
                        case 1:
                            //要去的位置
                            vector4 = Vector2.Subtract(player.Center + new Vector2(0, -500)/*调整npc要去的位置*/, NPC.Center);
                            Q = vector4.Length() / 10;
                            E = Q;
                            if (E > 20)
                            {
                                E = 20;
                            }
                            if (E < 0.1F)
                            {
                                E = 0;
                            }
                            if (E > 0 && E < 2)
                            {
                                E = 2;
                            }
                            vector4.Normalize();
                            vector4 *= E;//速度

                            NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;
                            NPC.velocity = (NPC.velocity * 20 + vector4) / 21;
                            NPCAI.Times[0]++;
                            if (NPCAI.Times[0] % 5 == 0 && NPCAI.Times[0] <= 150)
                            {
                                NPCAI.Times[1]++;
                                if (Main.netMode != 1)
                                {
                                    for (int a = -1; a <= 1; a++)
                                    {
                                        Vector2 vector = new Vector2(30 * a, 0).RotatedBy(NPC.rotation);
                                        if (a != 0)
                                        {
                                            Vector2 PO = player.Center + direction.RotatedBy(Main.rand.NextFloat(-1.2F, 1.2F)) * Main.rand.NextFloat(200, 400);
                                            NPC npc = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center + vector, ModContent.NPCType<歼灭者导弹2>(), 0, PO.X, (int)PO.Y)];
                                            npc.velocity = direction.RotatedBy(1 * a) * 25;

                                        }
                                    }
                                }
                                PlaySound(SoundID.Item12, NPC.position);
                            }
                            if (NPCAI.Times[0] > 300)
                            {
                                NPCAI.Times[0] = 0;
                                NPC.ai[0]++;
                                NPC.ai[1] = Main.rand.Next(1, 4);
                                NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2();

                            }
                            break;
                        case 2:
                            NPC.damage = NPC.defDamage;
                            if (NPCAI.Times[0] > 240)
                            {
                                NPCAI.Times[0]++;
                            }
                            if (DDHelper.SpecifyDirection(NPC.velocity.ToRotation(), direction.ToRotation(), 0.5F))
                            {
                                NPCAI.Times[0]++;
                                if (NPCAI.Times[0] % 60 == 0)
                                {
                                    NPC.velocity = NPC.velocity.PerfectNormalize() * 40;
                                    NPC.ai[2] = 60;
                                    WYE = 1.5f;
                                    for (int a = -1; a <= 1; a++)
                                    {
                                        Vector2 vector = new Vector2(30 * a, 0).RotatedBy(NPC.rotation);
                                        if (a != 0)
                                        {
                                            NPC npc = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center + vector, ModContent.NPCType<歼灭者导弹2>(), 0)];
                                            npc.velocity = direction.RotatedBy(1 * a) * -25;
                                        }
                                    }
                                    PlaySound(SoundID.Item12, NPC.position);
                                }
                            }
                            vector4 = Vector2.Subtract(player.Center/*调整npc要去的位置*/, NPC.Center);
                            vector4.DirectPerfectNormalize();
                            NPC.velocity = (NPC.velocity * 10 + vector4 * 6) / 11;
                            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
                            if (NPCAI.Times[0] > 300)
                            {
                                NPCAI.Times[0] = 0;
                                NPC.ai[0] = 0;
                                NPC.ai[1] = Main.rand.Next(1, 4);

                            }
                            break;
                    }
                }
                else if (NPC.Dnpc().Stage == 3)
                {
                    if (WYE < 1)
                    {
                        WYE += 0.2F;
                    }
                    else
                    if (WYE > 1.03F)
                    {
                        WYE -= 0.02F;
                    }
                    else
                    {
                        WYE = 1;
                    }

                    NPC.noTileCollide = true;
                    NPC.noGravity = true;
                    NPC.dontTakeDamage = false;
                    for (int a = 0; a < 4; a++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<绿激光粒子>(), 0f, 0f, 0, default, 0.2f)];
                        Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, (Math.PI * 2 / a) + a, default);
                        dust.velocity *= vector;
                        dust.noGravity = true;
                    }
                    if (Main.rand.NextBool(10))
                    {

                        for (int a = 0; a < 4; a++)
                        {
                            Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<冰雾>(), 0f, 0f, 0, new Color(10, 148, 10, 155), Main.rand.NextFloat(1F, 1.5F))];
                            Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, Main.rand.NextFloat(MathHelper.TwoPi), default);
                            dust.velocity = vector * 0.5F;
                            dust.alpha = -Main.rand.Next(500, 1500);
                        }
                    }
                    if (Main.rand.NextBool(60))
                    {
                        NPC.velocity.Y += Main.rand.NextFloat(5, 10);
                        NPC.velocity.X += Main.rand.NextFloat(-10, 10);
                        
                    }
                    switch (NPC.ai[0])
                    {
                        case 0:
                            //要去的位置
                            Vector2 vector4 = Vector2.Subtract(player.Center + new Vector2(-500, 0).RotatedBy(NPC.ai[1] * MathHelper.PiOver4)/*调整npc要去的位置*/, NPC.Center);
                            float Q = vector4.Length() / 10;
                            float E = Q;
                            if (E > 20)
                            {
                                E = 20;
                            }
                            if (E < 0.1F)
                            {
                                E = 0;
                            }
                            if (E > 0 && E < 2)
                            {
                                E = 2;
                            }
                            vector4.Normalize();
                            vector4 *= E;//速度

                            NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;
                            NPC.velocity = (NPC.velocity * 20 + vector4) / 21;
                            NPCAI.Times[0]++;
                            if (NPCAI.Times[0] % 20 == 0 && NPCAI.Times[4] > 40)
                            {
                                NPCAI.Times[1]++;
                                if (Main.netMode != 1)
                                {
                                    int type = ModContent.ProjectileType<BossGreenLaser2>();
                                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center + direction * 76, direction * 2, type, 12, 0, -1);
                                    for (int a = -1; a <= 1; a++)
                                    {
                                        Vector2 vector = new Vector2(30 * a, 0).RotatedBy(NPC.rotation);
                                        if (a != 0 && Main.rand.NextBool(4))
                                        {
                                            NPC npc = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center + vector, ModContent.NPCType<歼灭者导弹2>(), 0)];
                                            npc.velocity = direction.RotatedBy(1.5 * a) * 35;
                                        }
                                    }
                                }
                                PlaySound(SoundID.Item12, NPC.position);
                            }
                            if (NPCAI.Times[0] > 300)
                            {
                                NPCAI.Times[0] = 0;
                                NPC.ai[0]++;
                                NPC.ai[1] = Main.rand.Next(1, 4);

                            }
                            if (NPCAI.Times[4] < 50)
                            {
                                NPCAI.Times[4] += Main.rand.NextFloat(0,10);
                                
                            }
                            break;
                        case 1:
                            //要去的位置
                            vector4 = Vector2.Subtract(player.Center + new Vector2(0, -500)/*调整npc要去的位置*/, NPC.Center);
                            Q = vector4.Length() / 10;
                            E = Q;
                            if (E > 20)
                            {
                                E = 20;
                            }
                            if (E < 0.1F)
                            {
                                E = 0;
                            }
                            if (E > 0 && E < 2)
                            {
                                E = 2;
                            }
                            vector4.Normalize();
                            vector4 *= E;//速度

                            NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;
                            NPC.velocity = (NPC.velocity * 20 + vector4) / 21;
                            NPCAI.Times[0]++;
                            if (NPCAI.Times[0] % 5 == 0 && NPCAI.Times[0] <= 150)
                            {
                                NPCAI.Times[1]++;
                                if (Main.netMode != 1)
                                {
                                    for (int a = -1; a <= 1; a++)
                                    {
                                        Vector2 vector = new Vector2(30 * a, 0).RotatedBy(NPC.rotation);
                                        if (a != 0 && Main.rand.NextBool(4))
                                        {
                                            Vector2 PO = player.Center + direction.RotatedBy(Main.rand.NextFloat(-1.2F, 1.2F)) * Main.rand.NextFloat(200, 400);
                                            NPC npc = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center + vector, ModContent.NPCType<歼灭者导弹2>(), 0, PO.X, (int)PO.Y)];
                                            npc.velocity = direction.RotatedBy((1 + Main.rand.NextFloat(1)) * a) * 25;
                                        }
                                    }
                                }
                                PlaySound(SoundID.Item12, NPC.position);
                            }
                            if (NPCAI.Times[0] > 300)
                            {
                                NPCAI.Times[0] = 0;
                                NPC.ai[0]++;
                                NPC.ai[1] = Main.rand.Next(1, 4);
                                NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2();

                            }
                            break;
                        case 2:
                            NPC.damage = NPC.defDamage;
                            if (NPCAI.Times[0] > 240)
                            {
                                NPCAI.Times[0]++;
                            }
                            if (DDHelper.SpecifyDirection(NPC.velocity.ToRotation(), direction.ToRotation(), 0.5F))
                            {
                                NPCAI.Times[0]++;
                                if (NPCAI.Times[0] % 60 == 0)
                                {
                                    NPC.velocity = NPC.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1,1)) * 40;
                                    NPC.ai[2] = 60;
                                    WYE = 1.5f;
                                    if (Main.netMode != 1)
                                    {
                                        for (int a = -1; a <= 1; a++)
                                        {
                                            Vector2 vector = new Vector2(30 * a, 0).RotatedBy(NPC.rotation);
                                            if (a != 0 && Main.rand.NextBool(4))
                                            {
                                                NPC npc = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center + vector, ModContent.NPCType<歼灭者导弹2>(), 0)];
                                                npc.velocity = direction.RotatedBy(1 * a) * -25;
                                            }
                                        }
                                    }
                                    PlaySound(SoundID.Item12, NPC.position);
                                    
                                }
                            }
                            vector4 = Vector2.Subtract(player.Center/*调整npc要去的位置*/, NPC.Center);
                            vector4.DirectPerfectNormalize();
                            NPC.velocity = (NPC.velocity * 10 + vector4 * 6) / 11;
                            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
                            if (NPCAI.Times[0] > 300)
                            {
                                NPCAI.Times[0] = 0;
                                NPC.ai[0] = 0;
                                NPC.ai[1] = Main.rand.Next(1, 4);

                            }
                            break;
                    }
                }
            }
            else
            {
                NPC.velocity.Y -= 0.2f;
                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
            }
            if(NPC.ai[2]>0)
            {

                for (int a = 0; a < 4; a++)
                {
                    Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<速度粒子>(), 0f, 0f, 0, new Color(60, 148, 60, 0), Main.rand.NextFloat(2F, 3.5F))];
                    dust.velocity = -NPC.velocity.RotatedBy(Main.rand.NextFloat(-0.4F,0.4F)).PerfectNormalize()*12;
                    dust.rotation = dust.velocity.ToRotation();
                }
                NPC.ai[2]-=2;
            }
            //NPC.Dnpc().netUpdate = true;
        }

        public override void HitEffect(HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }
            if (NPC.life <= 0)
            {
                for (int a = 0; a < 300; a++)
                {
                    Dust dust = Main.dust[NewDust(NPC.Center,1, 1, ModContent.DustType<绿激光粒子>(), 0f, 0f, 0, default, 1.5f)];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 23), Main.rand.NextFloat(2, 117)) / 4, (Math.PI * 2 / a) + a, default);
                    dust.velocity *= vector;
                    dust.position += vector.PerfectNormalize()*Main.rand.NextFloat(2,12);
                    dust.noGravity = true;
                }
                for (int a = 0; a < 120; a++)
                {
                    Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<冰雾>(), 0f, 0f, 0, new Color(10, 148, 10, 155), Main.rand.NextFloat(2F, 4.5F))];
                    Vector2 vector = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(2, 4), Main.rand.NextFloat(2, 4)) / 4, Main.rand.NextFloat(MathHelper.TwoPi), default);
                    dust.velocity = vector * Main.rand.NextFloat(4F);
                    dust.position += vector.PerfectNormalize() * Main.rand.NextFloat(2, 42);
                    dust.alpha = -Main.rand.Next(3000, 6000);
                }
                int GoreType = Mod.Find<ModGore>("MeteorAnnihilator1").Type;
                int GoreType2 = Mod.Find<ModGore>("MeteorAnnihilator2").Type;
                int GoreType3 = Mod.Find<ModGore>("MeteorAnnihilator3").Type;
                int GoreType4 = Mod.Find<ModGore>("MeteorAnnihilator4").Type;
                int GoreType5 = Mod.Find<ModGore>("MeteorAnnihilator5").Type;
                int GoreType6 = Mod.Find<ModGore>("MeteorAnnihilator6").Type;

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation).ToRotationVector2() * 8, GoreType3, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation + MathHelper.Pi).ToRotationVector2() * 8, GoreType4, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation).ToRotationVector2() * 8, GoreType5, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, (NPC.rotation + MathHelper.Pi).ToRotationVector2() * 8, GoreType6, NPC.scale);

                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-3, 4), Main.rand.NextFloat(-3, 4)), GoreType, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-3, 4), Main.rand.NextFloat(-3, 4)), GoreType2, NPC.scale);
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Width = 24;
            NPC.frame.Height = 40;
            NPC.frameCounter++;
            if(NPC.frameCounter%5==0)
            {
                NPC.frame.X += 24;
            }
            if(NPC.frame.X>=120)
            {
                NPC.frame.X = 0;
            }
            int F = 0;
            if (NPC.Dnpc().Stage == 0)
            {
                if (NPC.life < NPC.lifeMax * 0.75F)
                {
                    F = 1;
                }
            }
            else if (NPC.Dnpc().Stage < 3)
            {
                F = 2;
            }
            else
            {
                F = 3;
            }
            NPC.frame.Y = 40*F;
            /*if (NPC.localAI[0] < 0f)
            {
                NPC.localAI[0] = 0f;
                NPC.localAI[1] = 1;
            }
            if (NPC.localAI[0] > 0.3f)
            {
                NPC.localAI[0] = 0.3f;
                NPC.localAI[1] = 0;
            }
            if (NPC.localAI[1] == 1)
            {
                NPC.localAI[0] += 0.05f;
            }
            else
            {
                NPC.localAI[0] -= 0.05f;
            }*/
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(252, 160,28 ),
                new Color(252, 160,28 ),
                new Color(252, 160,28 ),
                new Color(248, 66,5 ),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(248, 66, 5), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal Color ColorFunction2(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(50, 255,57 ),
                new Color(50, 210,57 ),
                new Color(50, 190,57 ),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(50, 160, 57), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                30,
                30,
                60,
                50,
                40,
                30,
            }), 5, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal static Trailing TrailDrawer;
        internal static Trailing TrailDrawer2;
        internal static Trailing TrailDrawer3;
        float WYE = 1;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float WYE = this.WYE*0.75F*NPC.scale;
            int F = 0;
            if (NPC.Dnpc().Stage == 0)
            {
                if (NPC.life < NPC.lifeMax * 0.75F)
                {
                    F = 1;
                }
            }
            else if (NPC.Dnpc().Stage <3)
            {
                F = 2;
            }
            else
            {
                F = 3;
            }
            //Vector2 vector = NPC.Center - screenPos + ((NPC.rotation - MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * -20);

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            if (TrailDrawer2 == null)
            {
                TrailDrawer2 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction2), null, GameShaders.Misc["贴图拖尾"]);
            }
            if (TrailDrawer3== null)
            {
                TrailDrawer3 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"],1);
            }
            Vector2 vector = new Vector2(0, 44 * 4).RotatedBy(NPC.rotation) * WYE- new Vector2(0, 44 * 4).RotatedBy(NPC.oldRot[1]) * WYE+NPC.velocity;
            vector *= 3;
            Vector2 vector2 = new Vector2(25 * 2.5F, 38 * 3F).RotatedBy(NPC.rotation) * WYE- new Vector2(25 * 2.5F, 38 * 3F).RotatedBy(NPC.oldRot[1]) * WYE+NPC.velocity;
            vector2 *= 3;
            Vector2 vector3 = new Vector2(-25 * 2.5F, 38 * 3F).RotatedBy(NPC.rotation) * WYE- new Vector2(-25 * 2.5F, 38 * 3F).RotatedBy(NPC.oldRot[1]) * WYE+NPC.velocity;
            vector3 *= 3;
            if (NPC.Dnpc().Defaults)
            {
                GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
                GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(3f);
                Vector2[] vectors = [NPC.Center + new Vector2(0, 44 * NPC.scale).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(0, 44 * 4).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(0, 44 * 4).RotatedBy(NPC.rotation) * WYE - NPC.velocity * 3];
                TrailDrawer3.Draw(vectors, -screenPos, 104, null, NPC.scale, 1, spriteBatch);

                vectors = [NPC.Center + new Vector2(25, 38).RotatedBy(NPC.rotation) * NPC.scale * WYE, NPC.Center + new Vector2(25 * 2.5F, 38 * 3F).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(25 * 3F, 38 * 3F).RotatedBy(NPC.rotation) * WYE - NPC.velocity * 3];
                TrailDrawer3.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.3F, 1, spriteBatch);

                vectors = [NPC.Center + new Vector2(-25, 38).RotatedBy(NPC.rotation) * NPC.scale * WYE, NPC.Center + new Vector2(-25 * 2.5F, 38 * 3F).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(-25 * 3F, 38 * 3F).RotatedBy(NPC.rotation) * WYE - NPC.velocity*3];
                TrailDrawer3.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.3F, 1, spriteBatch);
            }
            else
            {
                if (F <= 1)
                {
                    GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
                    GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(3f);
                    Vector2[] vectors = [NPC.Center + new Vector2(0, 44 * NPC.scale).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(0, 44 * 4).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(0, 44 * 4).RotatedBy(NPC.rotation) * WYE - vector];
                    TrailDrawer.Draw(vectors, -screenPos, 104, null, NPC.scale, 1, spriteBatch);

                    vectors = [NPC.Center + new Vector2(25, 38).RotatedBy(NPC.rotation) * NPC.scale * WYE, NPC.Center + new Vector2(25 * 2.5F, 38 * 3F).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(25 * 3F, 38 * 3F).RotatedBy(NPC.rotation) * WYE - vector2];
                    TrailDrawer.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.3F, 1, spriteBatch);

                    vectors = [NPC.Center + new Vector2(-25, 38).RotatedBy(NPC.rotation) * NPC.scale * WYE, NPC.Center + new Vector2(-25 * 2.5F, 38 * 3F).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(-25 * 3F, 38 * 3F).RotatedBy(NPC.rotation) * WYE - vector3];
                    TrailDrawer.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.3F, 1, spriteBatch);
                }
                else
                {
                    GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
                    GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(3f);
                    Vector2[] vectors = [NPC.Center + new Vector2(0, 44 * NPC.scale).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(0, 44 * 4).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(0, 44 * 4).RotatedBy(NPC.rotation) * WYE - vector];
                    TrailDrawer2.Draw(vectors, -screenPos, 104, null, NPC.scale, 1, spriteBatch);

                    vectors = [NPC.Center + new Vector2(25, 38).RotatedBy(NPC.rotation) * NPC.scale * WYE, NPC.Center + new Vector2(25 * 2.5F, 38 * 3F).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(25 * 3F, 38 * 3F).RotatedBy(NPC.rotation) * WYE - vector2];
                    TrailDrawer2.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.3F,1, spriteBatch);

                    vectors = [NPC.Center + new Vector2(-25, 38).RotatedBy(NPC.rotation) * NPC.scale * WYE, NPC.Center + new Vector2(-25 * 2.5F, 38 * 3F).RotatedBy(NPC.rotation) * WYE, NPC.Center + new Vector2(-25 * 3F, 38 * 3F).RotatedBy(NPC.rotation) * WYE - vector3];
                    TrailDrawer2.Draw(vectors, -screenPos, 104, null, NPC.scale * 1.3F, 1, spriteBatch);

                }
            }
            if (NPC.Dnpc().Stage >= 1)
            {
                for (int a = 0; a < NPC.oldPos.Length; a++)
                {
                    spriteBatch.Draw(NPCTexture.Value, NPC.oldPos[a] + NPC.Size / 2 - screenPos, new Rectangle?(new Rectangle(0, NPCTexture.Height() / 4 * F, NPCTexture.Width(), NPCTexture.Height() / 4)), new Color(1, 155, 7, 0) * (1 - (float)a / NPC.oldPos.Length), NPC.oldRot[a], new Vector2(NPCTexture.Width() / 2, NPCTexture.Height() / 8), NPC.scale, 0, 0f);
                }

            }
            spriteBatch.Draw(NPCTexture.Value, NPC.Center - screenPos, new Rectangle?(new Rectangle(0, NPCTexture.Height() / 4 * F, NPCTexture.Width(), NPCTexture.Height() / 4)), drawColor, NPC.rotation, new Vector2(NPCTexture.Width() / 2, NPCTexture.Height() / 8), NPC.scale, 0, 0f);


            //spriteBatch.Draw(尾气.Value, NPC.Center - screenPos+ (NPC.rotation-MathHelper.PiOver2).ToRotationVector2()*0, new Rectangle?(NPC.frame), Color.White, NPC.rotation, new Vector2(尾气.Width() / 10, 尾气.Height() / 8) - new Vector2(0, 44), NPC.scale, 0, 0f);

            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int F = 0;
            if (NPC.Dnpc().Stage == 0)
            {
                if (NPC.life < NPC.lifeMax * 0.75F)
                {
                    F = 1;
                }
            }
            else if (NPC.Dnpc().Stage < 3)
            {
                F = 2;
            }
            else
            {
                F = 3;
            }
            Texture2D Scanning = DDTextures.Scanning.Value;
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(new Rectangle(0, Glow.Height() / 4 * F, Glow.Width(), Glow.Height() / 4)), Color.White, NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 8), NPC.scale, 0, 0f);
            if (NPC.Dnpc().Stage == 2)
            {
                spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, new Rectangle?(new Rectangle(0, Glow.Height() / 4 * F, Glow.Width(), Glow.Height() / 4)), new Color(255, 255, 255, 0), NPC.rotation, new Vector2(Glow.Width() / 2, Glow.Height() / 8), NPC.scale, 0, 0f);
            }
            DGlobalNPC NPCAI = NPC.Dnpc();
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Texture2D Starlight = DDTextures.Starlight.Value;
            if (F >= 2 && NPCAI.Times[4] > 0)
            {
                Vector2 vector = NPC.Center - Main.screenPosition + ((NPC.rotation - MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * 44);
                Main.EntitySpriteDraw(VoidStar, vector, null, new Color(0, 255, 0, 0) * (NPCAI.Times[4] / (float)50), NPC.rotation - MathHelper.PiOver2, VoidStar.Size() / 2, new Vector2(NPC.scale, NPC.scale / 2) * 1.5F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(0, 255, 0, 0) * (NPCAI.Times[4] / (float)50), NPC.rotation - MathHelper.PiOver2, Starlight.Size() / 2, new Vector2(NPC.scale, NPC.scale / 2) * 1.5F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(255, 0, 255, 0) * (NPCAI.Times[4] / (float)50), NPC.rotation - MathHelper.PiOver2, Starlight.Size() / 2, new Vector2(NPC.scale, NPC.scale / 2) * 0.75F, 0, 0);
            }
            else
            if (F == 0 && NPCAI.Times[4] > 0)
            {
                Vector2 vector = NPC.Center - Main.screenPosition + ((NPC.rotation - MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * 44);
                Main.EntitySpriteDraw(VoidStar, vector, null, new Color(224, 82, 0, 0), NPC.rotation - MathHelper.PiOver2, VoidStar.Size() / 2, new Vector2(NPC.scale, NPC.scale / 2) * 1.5F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(224, 82, 0, 0), NPC.rotation - MathHelper.PiOver2, Starlight.Size() / 2, new Vector2(NPC.scale, NPC.scale / 2) * 1.5F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(31, 200, 255, 0), NPC.rotation - MathHelper.PiOver2, Starlight.Size() / 2, new Vector2(NPC.scale, NPC.scale / 2) * 0.75F, 0, 0);
            }
            if (NPC.life <= NPC.lifeMax * 0.75F)
            {
                if (NPC.ai[1] > 150)
                {
                    VoidStar = DDTextures.GlowEffect.Value;
                    Vector2 vector = NPC.Center - Main.screenPosition;
                    Main.EntitySpriteDraw(VoidStar, vector, null, new Color(20, 205, 20, 0)*0.75f, (float)(Main.GameUpdateCount*0.1), VoidStar.Size() / 2, new Vector2(NPC.scale, NPC.scale) * ((NPC.ai[1] - 150) / 50) * 10, 0, 0);
                    Main.EntitySpriteDraw(Starlight, vector, null, new Color(20, 205, 20, 0), (float)(Main.GameUpdateCount * 0.1), Starlight.Size() / 2, new Vector2(NPC.scale, NPC.scale) * ((NPC.ai[1] - 150) / 50) * 10, 0, 0);
                    Main.EntitySpriteDraw(Starlight, vector, null, new Color(235, 50, 235, 0), (float)(Main.GameUpdateCount * 0.1), Starlight.Size() / 2, new Vector2(NPC.scale, NPC.scale) * ((NPC.ai[1] - 150) / 50) * 5, 0, 0);
                }
            }
            if (NPC.Dnpc().Stage == 2)
            {
                VoidStar = DDTextures.GlowEffect.Value;
                Vector2 vector = NPC.Center + new Vector2(0, 8 * NPC.scale).RotatedBy(NPC.rotation) - Main.screenPosition;
                Main.EntitySpriteDraw(VoidStar, vector, null, new Color(20, 205, 20, 0), 0 * 0.75f, VoidStar.Size() / 2, new Vector2(NPC.scale, NPC.scale) * 0.3F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(20, 205, 20, 0), 0, Starlight.Size() / 2, new Vector2(NPC.scale, NPC.scale) * 0.3F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(235, 50, 235, 0), 0, Starlight.Size() / 2, new Vector2(NPC.scale, NPC.scale) * 0.3F, 0, 0);
            }
            if (NPC.Dnpc().Stage == 3)
            {
                VoidStar = DDTextures.GlowEffect.Value;
                Vector2 vector = NPC.Center+new Vector2(0,8*NPC.scale).RotatedBy(NPC.rotation) - Main.screenPosition;
                Main.EntitySpriteDraw(VoidStar, vector, null, new Color(20, 205, 20, 0)*0.4F, 0, VoidStar.Size() / 2, new Vector2(NPC.scale, NPC.scale) * 0.3F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(20, 205, 20, 0) * 0.5F, 0, Starlight.Size() / 2, new Vector2(NPC.scale, NPC.scale) * 0.3F, 0, 0);
                Main.EntitySpriteDraw(Starlight, vector, null, new Color(235, 50, 235, 0) * 0.5F, 0, Starlight.Size() / 2, new Vector2(NPC.scale, NPC.scale) * 0.3F, 0, 0);
            }
        }
    }
}