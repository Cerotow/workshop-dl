
using DDmod.Content.Dusts;
using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Boss.MiniBoss;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Projectiles.Boss.MiniBoss;
using DDmod.Content.Tiles.Trophy;
using DDmod.Helper;
using DDmod.Worlds;
using System.Linq;
using Terraria;
using Terraria.ModLoader.Utilities;

namespace DDmod.Content.NPCs.EliteMonster
{
    [AutoloadBossHead]
    public class 炼狱头颅 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 0;

        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/EliteMonster/炼狱头颅_Glow");

        }


        public override void SetDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPC.damage = 45;
            NPC.width = 68;
			NPC.height = 68;
			NPC.aiStyle = -1;
			NPC.defense = 25;
			NPC.lifeMax = 4000;
			NPC.knockBackResist = 0;
			NPC.value = Item.buyPrice(0, 10, 0, 0);
			NPC.alpha = 0;
            NPC.HitSound = SoundID.Tink;
            SoundStyle sound = SoundID.NPCDeath14;
            sound.Pitch = -1;
            sound.MaxInstances = 5;
            NPC.DeathSound = sound;
            NPC.noGravity = true;
			NPC.scale = 1.3F;
            NPC.noTileCollide = true;
            NPC.Dnpc().Properties.Fire = true;
            NPC.Dnpc().Neutrality = true;
            NPC.boss = true;
            NPC.NPCHB().MiniBoss = true;
            NPC.Dnpc().Properties.BossLife = 1.15f;
        }
        public override void ModifyTypeName(ref string typeName)
        {
        }
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
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
            if (NPC.Dnpc().Stage != 0)
            {
                NPC.noGravity = true;
                NPC.noTileCollide = true;
                if (NPC.localAI[2] >= 1)
                {
                    NPC.damage = NPC.defDamage;
                    NPC.Dnpc().Neutrality = false;
                    NPC.chaseable = true;
                    NPC.boss = true;
                    if (!Main.dedServ && Music == -1) Music = DDSystem.MiniBossMusic;
                }
            }
            if (NPC.Dnpc().Stage == 0)
            {
                NPC.damage = 0;
                NPC.localAI[2] = 0;

                NPC.noGravity = false;
                NPC.noTileCollide = false;
                NPC.boss = false;
                if (NPC.localAI[1] == 0)
                {

                    NPC.localAI[1] = Main.rand.Next(-1, 2);
                }

                NPC.rotation = 1.16F * NPC.localAI[1];
                if (NPC.life < NPC.lifeMax)
                {
                    NPC.Dnpc().Stage = 1;
                }
            }
            else if (NPC.localAI[2] < 1)
            {
                NPC.localAI[2] += 0.01F;
                NPC.velocity.X = 0;
                if (NPC.localAI[2] >= 0.2F)
                {
                    if (NPC.velocity.Y > -4F)
                        NPC.velocity.Y -= 0.1F;
                    NPC.RotationSpeed(NPC.velocity.X * 0.03F, Math.Abs(NPC.velocity.Y) / 200);
                }

                NPC.position = NPC.Center;
                NPC.width = (int)(94 * (NPC.scale));
                NPC.height = (int)(94 * (NPC.scale));
                NPC.Center = NPC.position;

                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 60, false, 0.1F);
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.velocity = Vector2.Zero;
                NPC.localAI[2] = 1;
                NPC.RotationSpeed(NPC.velocity.X * 0.03F, 0.02F);
                NPC.Dnpc().Stage = 2;
                NPC.ai[2] = 60;
                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 60, false, 0.1F);
                for (int t = 0; t < 6; t++)
                    PlaySound(sound, NPC.Center);
                Main.LocalPlayer.Dplayer().PlayerShake(60, 10);
                for (int t = 0; t < 12; t++)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4, -32 * (NPC.scale) + 4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(254, 86, 4, 0))];
                        dust.noGravity = true;
                        dust.scale = 0.01f;
                        dust.alpha = -1;
                        dust.velocity = Vector2.Zero;
                        dust.customData = new Vector4(NPC.scale, 40, r * 16, 1F);
                    }
                }
            }
            else if (NPC.Dnpc().Stage == 2)
            {
                NPC.velocity = Vector2.Zero;
                if (NPC.ai[2] > 0)
                {
                    NPC.ai[2]--;
                }
                else
                {

                    NPC.Dnpc().Stage = 3;
                    NPC.ai[2] = 0;
                }
                NPC.RotationSpeed(NPC.velocity.X * 0.03F, 0.02F);
            }
            else
            {
                NPC.TargetClosest(false);
                Player player = Main.player[NPC.target];
                Vector2 vector = player.Center - NPC.Center;

                Vector2 p = player.Center - new Vector2(0, 350) - NPC.Center;

                NPC.ai[0]++;
                if (NPC.ai[0] < 600)
                {
                    if (NPC.ai[0] > 120)
                    {
                        if (NPC.ai[0] % 40 == 0)
                        {
                            sound.Volume = 0.1F;
                            PlaySound(sound, NPC.Center);
                            NPC.ai[2] = 12;
                            NPC.NewNPCProj(NPC.Center + new Vector2(0, 32 * NPC.scale), vector.PerfectNormalize() * 12, ModContent.ProjectileType<Boss炼狱弹>(), 20, 0, -1, 1.6F);
                            for (int t = 0; t < 4; t++)
                            {
                                Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4, -32 * (NPC.scale) + 4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(254, 86, 4, 0))];
                                dust.noGravity = true;
                                dust.scale = 0.01f;
                                dust.alpha = -1;
                                dust.velocity = Vector2.Zero;
                                dust.customData = new Vector4(NPC.scale / 4, 40, 0, 1F);
                            }
                        }
                    }
                }
                else if (NPC.ai[0] < 1200)
                {
                    if (NPC.ai[0] % 120 == 0)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(20, 10);
                        for (int t = 0; t < 12; t++)
                        {
                            float damage = Main.rand.NextFloat(0.8F, 1.8F);
                            NPC.NewNPCProj(NPC.Center + new Vector2(0, 32 * NPC.scale), new Vector2(0, 6 * damage).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<Boss炼狱弹>(), (int)(20 * damage), 0, -1, damage);
                        }
                        PlaySound(sound, NPC.Center);
                        NPC.ai[2] = 20;
                        NPC.velocity = Vector2.Zero;
                        for (int t = 0; t < 12; t++)
                        {
                            Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4, -32 * (NPC.scale) + 4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(254, 86, 4, 0))];
                            dust.noGravity = true;
                            dust.scale = 0.01f;
                            dust.alpha = -1;
                            dust.velocity = Vector2.Zero;
                            dust.customData = new Vector4(NPC.scale/2, 20, 0, 1F);
                        }
                    }
                }
                else if (NPC.ai[0] < 1800)
                {
                    NPC.Dnpc().Bool[0] = true;
                    NPC.Dnpc().Bool[1] = true;
                    if (NPC.ai[0] == 1200)
                    {
                        PlaySound(sound, NPC.Center);
                    }
                    if (NPC.ai[0] < 1500)
                    {
                        float speed = vector.Length() / 20;
                        if (speed > 40)
                        {
                            speed = 40;
                        }
                        if (speed < 3)
                        {
                            speed = 3;
                        }
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                        NPC.rotation += 0.3F;
                    }
                    else if (NPC.ai[0] < 1580)
                    {
                        NPC.velocity *= 0.96F;
                        NPC.RotationSpeed(NPC.velocity.X * 0.03F, 0.1F);
                    }
                    else if (NPC.ai[0] < 1600)
                    {
                        NPC.rotation -= NPC.velocity.Length() * 0.04F;
                        NPC.velocity = (NPC.velocity * 20 + -vector.PerfectNormalize() * 18) / 21;
                    }
                    else if (NPC.ai[0] == 1600)
                    {
                        NPC.velocity = vector.PerfectNormalize() * 28;

                        for (int t = 0; t < 24; t++)
                        {
                            float damage = Main.rand.NextFloat(0.8F, 1.8F);
                            NPC.NewNPCProj(NPC.Center + new Vector2(0, 32 * NPC.scale), new Vector2(0, 10 * damage).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<Boss炼狱弹>(), (int)(20 * damage), 0, -1, damage);
                        }
                        PlaySound(sound, NPC.Center);
                        NPC.ai[2] = 20;
                        for (int t = 0; t < 12; t++)
                        {
                            Dust dust = Main.dust[NewDust(NPC.Center - new Vector2(4, -32 * (NPC.scale) + 4), 1, 1, ModContent.DustType<光圈粒子>(), NPC.oldVelocity.X, NPC.oldVelocity.Y, 0, new Color(254, 86, 4, 0))];
                            dust.noGravity = true;
                            dust.scale = 0.01f;
                            dust.alpha = -1;
                            dust.velocity = Vector2.Zero;
                            dust.customData = new Vector4(NPC.scale, 40, 0, 1F);
                        }
                    }
                    else
                    {
                        NPC.rotation += NPC.velocity.Length()*0.04F;
                        if (NPC.ai[0] > 1700)
                        {
                            if (vector.Length()>200)
                            {
                                NPC.velocity *= 0.92F;
                            }
                        }
                    }
                }
                else
                {
                    NPC.ai[0] = 0;
                }
                if (NPC.ai[2] > 0)
                {
                    NPC.ai[2]--;
                }




                if (!NPC.Dnpc().Bool[1])
                {
                    float speed = p.Length() / 20;
                    if (speed > 40)
                    {
                        speed = 40;
                    }
                    if(player.dead)
                    {
                        NPC.velocity.X *= 0.92f;
                        NPC.velocity.Y += 0.2F;
                        if(NPC.timeLeft>5)
                        {
                            NPC.timeLeft = 5;
                        }
                    }
                    else
                    {
                        NPC.velocity = (NPC.velocity * 20 + p.PerfectNormalize() * speed) / 21;

                    }
                }
                else
                {
                    NPC.Dnpc().Bool[1] = false;
                }
                if (!NPC.Dnpc().Bool[0])
                {
                    NPC.rotation %= MathHelper.TwoPi;
                    NPC.RotationSpeed(NPC.velocity.X * 0.03F, 0.1F);
                }
                else
                {
                    NPC.Dnpc().Bool[0] = false;
                }
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.炼狱头颅"))
            ]);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPCdirection.Incident(spawnInfo) || NPC.AnyNPCs(Type))
            {
                return 0;
            }
            if ((NPC.downedBoss3 || spawnInfo.Player.Aplayer().恶魔头颅))
            {
                if (spawnInfo.Player.Aplayer().恶魔头颅)
                {
                    return SpawnCondition.Underworld.Chance * 0.1f;
                }
                else if (!NPCDowned.炼狱头颅)
                {
                    return SpawnCondition.Underworld.Chance * 0.02f;
                }
                else
                {
                    return SpawnCondition.Underworld.Chance * 0.001f;
                }
            }
            return 0f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(new DropLocalPerClient(ModContent.ItemType<火山戒指>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<恶魔之血>(),1,1,4));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<炼狱头颅纪念章物品>(),10));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<炼狱头颅圣物>()));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            
            Texture2D texture = Glow.Value;
            Rectangle rectangle = new Rectangle(NPC.frame.X, NPC.frame.Y * 4, NPC.frame.Width * 4, NPC.frame.Height * 4);
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, new Color(254, 86, 4, 0) * NPC.localAI[2], NPC.rotation, rectangle.Size() / 2, NPC.scale / 4, 0, 0f);

            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 vector2 = NPC.oldPos[i] - screenPos + NPC.Size/2;
                Color color = new Color(254, 86, 4, 100) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length / 2f) * NPC.localAI[2];
                spriteBatch.Draw(texture, vector2, new Rectangle?(rectangle), color, NPC.oldRot[i], rectangle.Size() / 2, NPC.scale / 4, 0, 0f);
            }
            texture = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.Center  - screenPos, NPC.frame,Color.White, NPC.rotation, NPC.frame.Size()/2, NPC.scale, 0, 0f);
            texture = Glow.Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, new Color(254, 86, 4, 100)*0.1F* NPC.localAI[2], NPC.rotation, rectangle.Size() / 2, NPC.scale / 4, 0, 0f);
            return false;
        }
        float speed = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.oldRot[0] = NPC.rotation;

            for (int a = NPC.oldRot.Length-1; a >0;a--)
            {
                NPC.oldRot[a] = NPC.oldRot[a - 1];
            }
                NPC.frameCounter++;
            if(NPC.frameCounter%8==0)
            {
                NPC.frame.Y += frameHeight;  
            }
            if (NPC.frame.Y>=frameHeight*3)
            {
                NPC.frame.Y = frameHeight;
            }
            if (NPC.Dnpc().Stage == 0)
            {
                NPC.frame.Y = 0;

            }
            if (NPC.Dnpc().Stage != 0)
            {
                NPC.frame.Y = frameHeight*2;
            }
            if (NPC.ai[2]>0)
            {
                NPC.frame.Y = frameHeight;
            }
        }
        public override void OnKill()
        {
            SetEventFlagCleared(ref NPCDowned.炼狱头颅, -1);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void HitEffect(HitInfo hit)
		{
			for (int i = 0; i < 2; i++)
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
                PlaySound(NPC.DeathSound, NPC.Center);
                PlaySound(NPC.DeathSound, NPC.Center);
                PlaySound(NPC.DeathSound, NPC.Center);
				for (int A = 0; A < 120; A++)
                {
                    int D = NewDust(NPC.position, NPC.width, NPC.height, 6, 0f, 0f, 100, new Color(40, 251, 13, 55), 1f);
                    Main.dust[D].noGravity = true;
                    Main.dust[D].scale *= 1.7f + Main.rand.NextFloat(1,3);
                    Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(3, 8)), Main.rand.NextFloat(MathHelper.TwoPi), default);
                    Main.dust[D].velocity = vector * 2;
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    Asset<Texture2D> L = 丛林暴食怪根.ML;
                    Asset<Texture2D> L2 = 丛林暴食怪根.ML2;
                    int GoreType = Mod.Find<ModGore>("炼狱头颅1").Type;
                    int GoreType2 = Mod.Find<ModGore>("炼狱头颅2").Type;
                    int GoreType3 = Mod.Find<ModGore>("炼狱头颅3").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(-7, 0).RotatedBy(NPC.rotation), GoreType, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 7).RotatedBy(NPC.rotation), GoreType2, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(7, 0).RotatedBy(NPC.rotation), GoreType3, NPC.scale);
                    Vector2 vector = NPC.Size / 2;
                }
            }
		}
	}
}
