using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.MeteorAnnihilatorItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Content.Tiles.农场;
using DDmod.Sync;
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.海幽浮王
{
    [AutoloadBossHead]
    public class 海幽浮 : ModNPC
    {
        public static int Head;
        public override void Load()
        {
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            DDSystem.HBar(NPC.type, "海幽浮王", Vector2.Zero, "Mini");
        }

        public override void SetDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPC.damage = 30;
            NPC.width = 42;
            NPC.height = 42;
            NPC.defense = 8;
            NPC.lifeMax = 1800;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.scale = 1.3F;
            NPC.value = 0f;
            NPC.NPCHB().MiniBoss = true;
            NPC.Dnpc().BossPhysique = true;
            NPC.NPCHB().Child = true;
            NPC.Dnpc().Properties.Water = true;
            NPC.Dnpc().Properties.Meat= true;
            NPC.Dnpc().Properties.BossLife = 1.125F;
        }
        public int moveSpeed;
        public int moveSpeedY;
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.海幽浮"))
            });
        }
        public override void BossLoot( ref int potionType)
        {
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            /*
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
            */
        }
        public override void OnKill()
        {
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            Vector2 vector = player.Center - NPC.Center;
            NPC.localAI[0]++;
            if (NPC.localAI[0] >= 6 * 8)
            {
                NPC.localAI[0] = 0;
            }
            NPC.velocity = (NPC.rotation - MathHelper.PiOver2).ToRotationVector2() * NPC.ai[2];

            NPC.ai[2] *= 0.94F;
            if (NPC.localAI[0] == 4 * 8)
            {
                NPC.ai[2] = Main.rand.NextFloat(8,12);
            }
            if (NPC.ai[0]++ >= 180)
            {
                NPC.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, 0.03F);
            }
            else
            {
                if (NPC.localAI[0] == 5 * 8)
                {
                    NPC.ai[2] = 6;
                }

            }
            if (NPC.ai[0] == 2 && Main.netMode != 1)
            {
                DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center - new Vector2(100, 100), ModContent.NPCType<小海幽浮>(), 0);
                DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center - new Vector2(50, 120), ModContent.NPCType<小海幽浮>(), 0);
                DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 140), ModContent.NPCType<小海幽浮>(), 0);
                DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center - new Vector2(-50, 120), ModContent.NPCType<小海幽浮>(), 0);
                DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center - new Vector2(-100, 100), ModContent.NPCType<小海幽浮>(), 0);
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
                for (int i = 0; i < 100; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, 33, Main.rand.NextFloat(-12, 12), Main.rand.NextFloat(-8, 8), 100, default, NPC.scale * 1.3F);
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    int GoreType = Mod.Find<ModGore>("海幽浮1").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8)), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("海幽浮2").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8)), GoreType, NPC.scale);
                    GoreType = Mod.Find<ModGore>("海幽浮3").Type;
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
            drawColor = NPC.GetAlpha(drawColor);
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Rectangle rectangle = NPC.frame;
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, drawColor, NPC.rotation, new Vector2(texture.Width, texture.Height / 6) / 2, NPC.scale, sprite, 0f);
            for (int a = 0; a < NPC.oldPos.Length; a++)
            {
                Vector2 vector = NPC.oldPos[a] + NPC.Size / 2;
                spriteBatch.Draw(texture, vector - screenPos, rectangle, drawColor*0.4f * (1 - (float)a / NPC.oldPos.Length), NPC.rotation, new Vector2(texture.Width, texture.Height / 6) / 2, NPC.scale, sprite, 0f);
            }
            return false;
        }
    }
}