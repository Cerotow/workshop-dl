using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using DDmod.Content.Items.Series.仙人掌;
using DDmod.Content.Dusts;
using Terraria.ModLoader.Utilities;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Boss;
using Terraria.WorldBuilding;
using DDmod.Content.Biome;

namespace DDmod.Content.NPCs.Boss.绿岩之视
{
	public class 绿岩炮 : ModNPC
	{
		public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "DDmod/Content/NPCs/Boss/绿岩之视/绿岩炮Texture",
                Scale = 1,
                Position = new Vector2(-10, 10),
                PortraitScale = 1f,
                PortraitPositionXOverride = -10,
                PortraitPositionYOverride = 36
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
            Main.npcFrameCount[NPC.type] = 1;
            DDSystem.HBar(NPC.type, "绿岩之视", Vector2.Zero, "手");
        }

		public override void SetDefaults()
		{
			NPC.damage = 40;
			NPC.width = 40;
			NPC.height = 40;
			NPC.aiStyle = -1;
			NPC.defense = 18;
			NPC.scale = 1f;
			NPC.lifeMax = 2100;
			NPC.knockBackResist = 0.3f;
			NPC.value = 0;
			NPC.alpha = 200;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.NPCHB().MiniBoss = true;
            NPC.NPCHB().Child = true;
            NPC.Dnpc().Properties.Iron = true;
            NPC.localAI[0] = -1;
            NPC.localAI[1] = -1;
            SpawnModBiomes = new int[] { ModContent.GetInstance<绿岩实验室>().Type };
            NPC.Dnpc().PenetrationProtection = 0.33F;
            NPC.Dnpc().MaxPenetrationProtection = 0.33F;
            NPC.Dnpc().Properties.BossLife = 1.125F;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void AI()
        {
            NPC.timeLeft = 100;
            //主人
            NPC Master = Main.npc[NPC.Dnpc().Master];
            NPC.target = Master.target;
            NPC.Center = Master.Center - new Vector2(-1 * Master.direction * Master.scale, 64 * Master.scale).RotatedBy(Master.rotation);
            NPC.dontTakeDamage = !Master.Dnpc().Bool[1];
            NPC.boss = !NPC.dontTakeDamage;
            if (NPC.ai[2] > 0)
            {
                NPC.dontTakeDamage = Master.dontTakeDamage;
                NPC.boss = false;
                if (NPC.Dnpc().MasterBool || !NPC.AnyNPCs(ModContent.NPCType<绿岩之视>()))
                {
                    if (!Master.active || Master.type != NPC.type)
                    {
                        NPC.Kill(false);
                    }
                }
                for (int a = 0; a < 255; a++)
                {
                    if (Master.immune[a] > 0)
                    {
                        NPC.immune[a] = Master.immune[a];
                    }
                    if (NPC.immune[a] > 0)
                    {
                        Master.immune[a] = NPC.immune[a];
                    }
                }
                NPC M = Main.npc[Master.Dnpc().Master];
                NPC.Center = M.Center - new Vector2(-1 * M.direction * M.scale, 64 * M.scale).RotatedBy(M.rotation) + M.Dnpc().vector[1].PerfectNormalize() * (40 * NPC.ai[2]);
                return;
            }
            if (NPC.realLife == -1)
                NPC.realLife = NPC.whoAmI;

            if (Main.netMode != 1)
            {
                if (NPC.localAI[0] == -1 || Main.npc[(int)NPC.localAI[0]].type != NPC.type)
                {
                    NPC.localAI[0] = NewNPC(NPC.GetSource_FromAI(), 0, 0, NPC.type, ai2: 1);
                    Main.npc[(int)NPC.localAI[0]].realLife = NPC.whoAmI;
                    NPC.netUpdate = true;

                }
                if (NPC.localAI[1] == -1 || Main.npc[(int)NPC.localAI[1]].type != NPC.type)
                {
                    NPC.localAI[1] = NewNPC(NPC.GetSource_FromAI(), 0, 0, NPC.type, ai2: 2);
                    Main.npc[(int)NPC.localAI[1]].realLife = NPC.whoAmI;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.Dnpc().Times[1] > 0)
            {
                NPC.Dnpc().Times[1]--;

            }
            NPC.ai[0]++;
            if (NPC.ai[1] > 0)
            {
                NPC.ai[1]--;
                Master.Dnpc().Times[2] -= Master.direction * 0.4F;
                for (int a = 0; a < 4; a++)
                {
                    int dust = NewDust(NPC.Center + Master.Dnpc().vector[1].PerfectNormalize() * 100 - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(119, 237, 130, 50), 1);
                    Main.dust[dust].velocity = Master.Dnpc().vector[1].PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(10, 12);
                    Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                    Main.dust[dust].customData = 1002;
                    Main.dust[dust].noGravity = true;
                }
                if (Main.netMode != 1)
                {
                    int proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Master.Dnpc().vector[1].PerfectNormalize() * 100, Master.Dnpc().vector[1].PerfectNormalize() * 8, ModContent.ProjectileType<绿岩弹>(), 20, 0, -1);

                    Main.projectile[proj].scale = 0.3f;
                }
                for (int a = 0; a < 12; a++)
                {
                    Dust dust = Main.dust[NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<冰雾>(), 0f, 0f, -Main.rand.Next(200, 500), new Color(119, 237, 130, 50), Main.rand.NextFloat(0.3F, 0.75F))];
                    Vector2 vector = -Master.Dnpc().vector[1].PerfectNormalize() * Main.rand.NextFloat(8, 24);
                    dust.velocity = vector;
                    dust.noGravity = false;
                }
            }
            if (NPC.ai[0] % 120 == 0 && !NPC.dontTakeDamage)
            {
                NPC.ai[1] = 3;
                if (Main.netMode != 1)
                {
                    int proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center + Master.Dnpc().vector[1].PerfectNormalize() * 100, Master.Dnpc().vector[1].PerfectNormalize() * 22, ModContent.ProjectileType<绿岩弹>(), 30, 0, -1);
                    Main.projectile[proj].scale = 1.2f;
                    NPC.netUpdate = true;
                }
                SoundStyle sound = new SoundStyle(DDHelper.Sound(1, "大炮"));
                sound.Pitch = -0.5f;
                PlaySound(sound, NPC.Center);

                for (int a = 0; a < 30; a++)
                {
                    int dust = NewDust(NPC.Center + Master.Dnpc().vector[1].PerfectNormalize() * 100 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(119, 237, 130, 50), 3);
                    Main.dust[dust].velocity = Master.Dnpc().vector[1].PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(10, 28);
                    Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                    Main.dust[dust].noGravity = true;
                }
            }
            if (NPC.Dnpc().MasterBool || !NPC.AnyNPCs(ModContent.NPCType<绿岩之视>()))
            {
                if (!Master.active || Master.type != ModContent.NPCType<绿岩之视>())
                {
                    NPC.Kill(false);
                }
            }
            //Player player = Main.player[NPC.target];
        }
        public override bool? CanFallThroughPlatforms()
		{
			return true;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BestiaryPortraitBackgroundProviderPreferenceInfoElement(ModContent.GetInstance<绿岩实验室>().ModBiomeBestiaryInfoElement),
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.绿岩炮")),
            });
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
		}
		//1-5生气
		//6-8转换微笑
		//9-14微笑
		//15-18转换生气
		//19挨打
        public override void FindFrame(int frameHeight)
        {
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if(NPC.ai[2] > 0)
            {
                return false;
            }
            return base.DrawHealthBar(hbPosition, ref scale, ref position);
        }
        public override void OnHitByProjectile(Projectile projectile, HitInfo hit, int damageDone)
        {
            /*
            if (NPC.ai[2] == 0)
            {
                projectile.localNPCImmunity[(int)Main.npc[NPC.realLife].localAI[0]] = projectile.localNPCHitCooldown;
                projectile.localNPCImmunity[(int)Main.npc[NPC.realLife].localAI[1]] = projectile.localNPCHitCooldown;
            }
            if (NPC.ai[2] == 1)
            {
                projectile.localNPCImmunity[NPC.realLife] = projectile.localNPCHitCooldown;
                projectile.localNPCImmunity[(int)Main.npc[NPC.realLife].localAI[1]] = projectile.localNPCHitCooldown;
            }
            if (NPC.ai[2] == 2)
            {
                projectile.localNPCImmunity[NPC.realLife] = projectile.localNPCHitCooldown;
                projectile.localNPCImmunity[(int)Main.npc[NPC.realLife].localAI[0]] = projectile.localNPCHitCooldown;
            }*/
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return false;
		}
        public override void HitEffect(HitInfo hit)
        {
            if (NPC.ai[2] == 0)
            {
                NPC.Dnpc().Times[1] = 6;

                NPC.Dnpc().Bool[2] = true;

                NPC.Dnpc().Times[0] = 10;
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<绿岩粒子>(), hit.HitDirection, -1f, 0, Color.White, 1f);
                }
                for (int i = 0; i < 2; i++)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0f, 0f, 10, new Color(119, 237, 130, 255), 1f);
                    Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 4);
                }

                if (NPC.life <= 0&& !NPC.Dnpc().Bool[3])
                {
                    NPC.Dnpc().Bool[3] = true;
                    if (Main.netMode != 2)
                    {
                        int GoreType = Mod.Find<ModGore>("绿岩炮1").Type;
                        Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, new Vector2(0, -2), GoreType, NPC.scale);
                    }
                    for (int A = 0; A < 30; A++)
                    {
                        int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0f, 0f, 10, new Color(119, 237, 130, 255), 1f);
                        Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 6);
                    }
                }
            }
            else
            {
                NPC Master = Main.npc[NPC.Dnpc().Master];
                Master.Dnpc().Times[1] = 6;

                Master.Dnpc().Bool[2] = true;

                Master.Dnpc().Times[0] = 10;
                NPCLoader.HitEffect(Master,hit);
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<绿岩粒子>(), hit.HitDirection, -1f, 0, Color.White, 1f);
                }
                for (int i = 0; i < 2; i++)
                {
                    int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0f, 0f, 10, new Color(119, 237, 130, 255), 1f);
                    Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0, 4);
                }

            }
        }
	}
}
