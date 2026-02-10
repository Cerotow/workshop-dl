
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
    public class 日耀护卫 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.TrailCacheLength[NPC.type] = 8;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            DDSystem.HBar(NPC.type, "四柱/日耀柱", new Vector2(-38, 0), Shield: true);
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers npcbestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitPositionYOverride = 0,
                PortraitPositionXOverride = -0F,
                PortraitScale = 0.75f,
                Scale = 0.5F,
                Position = new Vector2(0, 0),
                Rotation = 0F,
                Direction = 1,
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = npcbestiaryDrawModifiers;

        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/四柱护卫/日耀护卫_Glow");

        }


        public override void SetDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 16;
            NPC.damage = 120;
            NPC.width = 112;
			NPC.height = 112;
			NPC.aiStyle = -1;
			NPC.defense = 62;
			NPC.lifeMax = 39000;
			NPC.knockBackResist = 0;
			NPC.value = Item.buyPrice(0, 50, 0, 0);
			NPC.alpha = 255;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCDeath3;
            NPC.noGravity = true;
			NPC.scale = 1.3F;
            NPC.noTileCollide = true;
            NPC.Dnpc().Properties.Fire = true;
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
            SoundStyle sound = SoundID.Roar;
            sound.Pitch = -0.4F;
            sound.MaxInstances = 20;
            NPC.TargetClosest();
            Main.player[Main.myPlayer].solarMonolithShader = true;
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
            Vector2 vector = player.Center- NPC.Center;
            if (NPC.localAI[2] < 2)
            {
                NPC.localAI[2] += 0.06F;
                if (!NPC.Dnpc().Bool[2])
                    Main.LocalPlayer.Dplayer().PlayerShake(30, 12);
            }
            NPC.spriteDirection = 1;
            if (NPC.velocity.X < 0)
            {
                NPC.spriteDirection = -1;
            }
            if (NPC.Dnpc().Stage == 0)
            {
                NPC.velocity.X=0;
                NPC.velocity.Y=-0.4F;

                if (NPC.alpha>0)
                {
                    NPC.alpha -= 2;
                    for (int A = 0; A < 5; A++)
                    {
                        int D = NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, new Color(40, 251, 13, 55), 1f);
                        Main.dust[D].noGravity = true;
                        Main.dust[D].scale *= 1.2f + Main.rand.NextFloat(1, 2);
                        vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(3, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                        Main.dust[D].velocity = vector/2;
                    }

                    Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center,50,false,0.1F);
                }
                else
                {
                    NPC.localAI[2] = 1;

                    sound.Pitch = -1F;
                    PlaySound(sound);
                    PlaySound(sound);
                    for (int t = 0; t < 4; t++)
                    {
                        for (int r = 0; r < 4; r++)
                        {
                            Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(253, 180, 100, 0))];
                            dust.noGravity = true;
                            dust.scale = 0.01f;
                            dust.alpha = -1;
                            dust.velocity = Vector2.Zero;
                            dust.customData = new Vector4(NPC.scale * 3, 40, r * 16, 1F);
                        }
                    }
                    NPC.Dnpc().Stage = 1;
                    NPC.netUpdate = true;
                }
                return;
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.ai[0]++;
                if (NPC.localAI[2]>=2&& NPC.ai[0]>30)
                {
                    NPC.ai[0] = 0;
                    NPC.Dnpc().Stage = 2;
                    NPC.netUpdate = true;
                }
                    return;

            }
            NPC.alpha = 0;
            if(!NPC.Dnpc().Bool[2])
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.ai[0]--;
            if (NPC.ai[0] < 0)
            {
                Vector2 P = player.Center - NPC.Center;
                P.Y -= 300;
                if (vector.X > 0)
                {
                    P.X -= 400;
                }
                else
                {
                    P.X += 400;
                }
                if (NPC.ai[1] < 5)
                {
                    if (!NPC.Dnpc().Bool[2])
                    {
                        NPC.SmoothVelocity(P.PerfectNormalize() * 50, 20);
                        if (Math.Abs(P.Y) < 70 && Math.Abs(P.X) < 200)
                        {
                            NPC.Dnpc().Bool[2] = true;

                            NPC.localAI[2] = 1;
                        }
                    }
                    else
                    {
                        NPC.localAI[1]++;
                        if(NPC.localAI[1]<20)
                        {
                            //NPC.position -= vector.PerfectNormalize()* 5;
                            NPC.velocity *= 0.91F;

                            NPC.RotationSpeed(vector.ToRotation(),0.1F);
                        }
                        else
                        {
                            NPC.localAI[1] = 0;
                            NPC.Dnpc().Bool[2] = false;
                            NPC.velocity = vector.PerfectNormalize() * 42;
                            sound.Pitch = -0.5F;
                            PlaySound(sound);
                            NPC.ai[0] = 60;
                            NPC.Dnpc().Bool[0] = false;
                            NPC.Dnpc().Bool[1] = false;
                            NPC.ai[1]++;
                        }
                    }
                }
                else
                {
                    if (NPC.ai[2] == 0)
                    {
                        if (vector.Length() < 500)
                        {
                            NPC.Dnpc().Bool[1] = true;
                        }
                        if (NPC.Dnpc().Bool[1])
                        {
                            NPC.SmoothVelocity(vector.PerfectNormalize() * 5, 20);
                            NPC.localAI[0]++;
                            if (Main.rand.NextBool(10))
                            {
                                NPC.NewNPCProj(NPC.Center + new Vector2(40, 40 * NPC.spriteDirection).RotatedBy(NPC.rotation), (NPC.rotation + 0.3F * NPC.spriteDirection + Main.rand.NextFloat(-0.4F, 0.4F)).ToRotationVector2() * Main.rand.NextFloat(12, 22), ModContent.ProjectileType<BossHellfireball>(), 50, 0, -1, 0, Main.rand.NextFloat(1.75F, 2.5F), 2);
                            }
                            if (NPC.localAI[0] > 300)
                            {
                                NPC.localAI[0] = 0;
                                NPC.ai[1] = 0;
                                NPC.ai[2]++;
                            }
                        }
                        else
                        {

                            NPC.SmoothVelocity(vector.PerfectNormalize() * 20, 20);
                        }
                    }
                    else if (NPC.ai[2] == 1)
                    {
                        NPC.SmoothVelocity(P.PerfectNormalize() * P.Length() / 2, 20);
                        if ((Math.Abs(P.Y) < 20 && Math.Abs(P.X) < 200) || NPC.localAI[0] > 0)
                        {
                            NPC.localAI[0]++;
                            if (NPC.localAI[0] % 30 == 0)
                            {
                                NPC N = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center, 418, 0)];
                                N.velocity = new Vector2(0, -Main.rand.NextFloat(12, 22)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                            }
                            NPC.velocity *= 0.8F;
                            NPC.rotation = vector.ToRotation();
                            NPC.spriteDirection = 1;
                            if (vector.X < 0)
                            {
                                NPC.spriteDirection = -1;
                            }
                        }
                        if (NPC.localAI[0] > 120)
                        {
                            NPC.localAI[0] = 0;
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                        }
                    }
                    else if (NPC.ai[2] == 2)
                    {
                        P = player.Center - NPC.Center;
                        if (vector.X > 0)
                        {
                            P.X -= 500;
                        }
                        else
                        {
                            P.X += 500;
                        }
                        NPC.rotation = vector.ToRotation();
                        NPC.spriteDirection = 1;
                        if (vector.X < 0)
                        {
                            NPC.spriteDirection = -1;
                        }
                        NPC.SmoothVelocity(P.PerfectNormalize() * P.Length() / 2, 20);
                        NPC.localAI[0]++;
                        if (NPC.localAI[0] > 0 && NPC.localAI[0] <= 360 && NPC.localAI[0] % 120 == 60)
                        {
                            for (int A = -12; A <= 12; A++)
                            {
                                NPC.NewNPCProj(player.Center + new Vector2(120 * A + Main.rand.Next(-30, 30), -1200), new Vector2(0, 10).RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)), ModContent.ProjectileType<Boss烈阳光束>(), 80, 0, -1, 0, 0, (A + 8) * 3);

                            }
                        }

                        if (NPC.localAI[0] > 0 && NPC.localAI[0] <= 360 && NPC.localAI[0] % 120 == 0)
                        {
                            NPC.localAI[2] = 1;
                            sound.Pitch = -1F;
                            PlaySound(sound);
                            PlaySound(sound);
                            for (int t = 0; t < 4; t++)
                            {
                                for (int r = 0; r < 4; r++)
                                {
                                    Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(253, 180, 100, 0))];
                                    dust.noGravity = true;
                                    dust.scale = 0.01f;
                                    dust.alpha = -1;
                                    dust.velocity = Vector2.Zero;
                                    dust.customData = new Vector4(NPC.scale * 3, 40, r * 16, 1F);
                                }
                            }
                        }
                        if (NPC.localAI[0] > 500)
                        {
                            NPC.localAI[0] = 0;
                            NPC.ai[1] = 0;
                            NPC.ai[2]++;
                        }
                    }
                    else if (NPC.ai[2] == 3)
                    {
                        P = player.Center - NPC.Center;
                        if (vector.X > 0)
                        {
                            P.X -= 500;
                        }
                        else
                        {
                            P.X += 500;
                        }
                        vector += player.velocity * 18;
                        NPC.rotation = vector.ToRotation();
                        NPC.spriteDirection = 1;
                        if (vector.X < 0)
                        {
                            NPC.spriteDirection = -1;
                        }
                        NPC.rotation = vector.ToRotation() - 0.3F * NPC.spriteDirection;
                        NPC.SmoothVelocity(P.PerfectNormalize() * P.Length() / 2, 20);
                        NPC.localAI[0]++;
                        if (NPC.localAI[0] >45 && NPC.localAI[0] <= 360 && NPC.localAI[0] % 15 == 0)
                        {
                            NPC.NewNPCProj(NPC.Center + new Vector2(40, 40 * NPC.spriteDirection).RotatedBy(NPC.rotation), (NPC.rotation + 0.3F * NPC.spriteDirection + Main.rand.NextFloat(-0.4F, 0.4F)).ToRotationVector2() * Main.rand.NextFloat(18, 24), ModContent.ProjectileType<Boss炼狱弹>(), 50, 0, -1, Main.rand.NextFloat(1.75F, 2.5F), 0);
                        }
                        if (NPC.localAI[0] > 500)
                        {
                            NPC.localAI[0] = 0;
                            NPC.ai[1] = 0;
                            NPC.ai[2] =0;
                        }
                    }
                }
            }
            else
            {
                for (int a = 0; a < 3; a++)
                {
                    int A = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(253, 159, 66, 50), NPC.scale * Main.rand.NextFloat(1F, 1.6F));
                    Main.dust[A].velocity = -NPC.velocity.PerfectNormalize() * 2;
                    Main.dust[A].rotation = NPC.velocity.ToRotation();
                    Main.dust[A].customData = 1F;
                    Main.dust[A].noGravity = true;
                }
                if (!NPC.Dnpc().Bool[0] && Math.Abs(vector.Y) < 100)
                {
                    NPC.Dnpc().Bool[0] = true;
                }
                if (NPC.ai[0] % 10 == 0 && Main.netMode != 1)
                {
                    NPC N = Main.npc[NewNPCs(NPC.GetSource_FromAI(), NPC.Center, 519, 0)];
                    N.velocity = new Vector2(0, -Main.rand.NextFloat(12, 22)).RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F));
                }
                if (NPC.Dnpc().Bool[0] && (Math.Abs(vector.Y) > 200)&& (Math.Abs(vector.X) > 300))
                {
                    NPC.ai[0] = 0;
                }
            }
            //NPC.ai[2] = 2;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.SolarPillar,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.日耀护卫"))
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
            npcLoot.Add(new DropBasedOnCompleteMode(new DropOneByOne(3458, parameters2), new DropOneByOne(3458, parameters3), new DropOneByOne(3458, parameters4)));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<日耀遗物>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<日耀护卫纪念章>(), 10));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<日耀护卫圣物>()));
        }
        internal Color ColorFunction(float completionRatio)
        {
            // 定义颜色数组
            Color[] colorArray = [
    new Color(253, 221,3, 0),
   new Color(254, 121,2, 0),
    new Color(154, 61,0, 155),
    new Color(154, 61,0, 155),
    new Color(0, 0,0, 0),
    new Color(0, 0,0, 0),
];

            // 根据completionRatio选择颜色
            float segment = 1f / (colorArray.Length - 1);
            int index = (int)(completionRatio / segment);
            float lerpFactor = (completionRatio % segment) / segment;

            Color drawColor = Color.Lerp(colorArray[index], colorArray[index + 1], lerpFactor);
            return drawColor;
        }
        internal float WidthFunction(float completionRatio)
        {
            // 定义颜色数组
            float[] colorArray = [80,80,80,80,40,20,10,5,2,1];

            // 根据completionRatio选择颜色
            float segment = 1f / (colorArray.Length - 1);
            int index = (int)(completionRatio / segment);
            float lerpFactor = (completionRatio % segment) / segment;

            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(colorArray[index], colorArray[index + 1], lerpFactor);
        }
        internal static Trailing TrailDrawer;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾2"]);
            }
            if (NPC.IsABestiaryIconDummy)
            {
                NPC.alpha = 0;
                NPC.spriteDirection = 0;
            }
            else
            {
                GameShaders.Misc["贴图拖尾2"].SetShaderTexture(DDTextures.GlowTrail2);
                GameShaders.Misc["贴图拖尾2"].Shader.Parameters["uSpeed"].SetValue(1.2f);
                TrailDrawer.Draw(NPC.oldPos, NPC.Size / 2 - screenPos, 120, null, NPC.scale, 1F);
            }
            SpriteEffects spriteEffects = 0;
            if(NPC.spriteDirection==-1)
            {
                spriteEffects = SpriteEffects.FlipVertically;
            }
            float C = 1- NPC.alpha / 255F;
            Texture2D texture = Glow.Value;
            Rectangle rectangle = new Rectangle(0, NPC.frame.Y * 2, NPC.frame.Width * 2, NPC.frame.Height * 2);
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, new Color(255, 255, 255, 0)* C, NPC.rotation, rectangle.Size() / 2, NPC.scale / 2, spriteEffects, 0f);
            int L = 12;
            if (NPC.Dnpc().Stage > 1)
            {
                for (int i = 0; i < L; i++)
                {
                    Vector2 vector2 = NPC.oldPos[i] - screenPos + NPC.Size / 2;
                    Color color = new Color(255, 255, 255, 100) * ((L - i) / (float)L);
                    spriteBatch.Draw(texture, vector2, rectangle, color * C, NPC.oldRot[i], rectangle.Size() / 2, NPC.scale / 2, spriteEffects, 0f);
                }
            }
            texture = TextureAssets.Npc[NPC.type].Value;
            if (NPC.localAI[2] < 2)
            {
                for (int A = 0; A < 3; A++)
                {
                    spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, new Color(253, 129, 4, 0) * (2 - NPC.localAI[2]) * 2 * C, NPC.rotation, NPC.frame.Size() / 2, NPC.scale * NPC.localAI[2], spriteEffects, 0f);
                    spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, rectangle, new Color(255, 255, 255, 0) * C, NPC.rotation, rectangle.Size() / 2, NPC.scale / 2, spriteEffects, 0f);
                }
            }
            spriteBatch.Draw(texture, NPC.Center  - screenPos, NPC.frame,drawColor * C, NPC.rotation, NPC.frame.Size()/2, NPC.scale, spriteEffects, 0f);
            texture = Glow.Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, new Color(255, 255, 255, 0) * C, NPC.rotation, rectangle.Size() / 2, NPC.scale / 2, spriteEffects, 0f);
            return false;
        }
        float speed = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.oldRot[0] = NPC.rotation;

            for (int a = NPC.oldRot.Length - 1; a > 0; a--)
            {
                NPC.oldRot[a] = NPC.oldRot[a - 1];
            }
            NPC.localAI[3]-=2;
            NPC.frameCounter++;
            if (NPC.frameCounter % 8 == 0)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 5)
            {
                NPC.frame.Y = frameHeight;
            }
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.日耀护卫, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
		{
			for (int i = 0; i < 12; i++)
            {
                int D = NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, new Color(40, 251, 13, 55), 1f);
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
                    int D = NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, new Color(40, 251, 13, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 2.7f + Main.rand.NextFloat(1,3);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(3, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                    Main.dust[D].velocity = vector * 2;
                }
                if (Main.netMode != 2)
                    for (int A = 1; A <= 4; A++)
                {
                    int GoreType = Mod.Find<ModGore>("日耀护卫"+A).Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(hit.HitDirection, -1), GoreType, NPC.scale);
                }
                
            }
		}
	}
}
