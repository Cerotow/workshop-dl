using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.NPCs.Boss.StarGuardBulan
{
    public class ServantOfTheStars : ModNPC
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/StarGuardBulan/星仆光效");
        }
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Mana Servant");
           //DisplayName.AddTranslation(7, "星辰仆从");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "DDmod/Content/NPCs/Boss/StarGuardBulan/ServantOfTheStars",
                Scale = 1f,
                PortraitScale = 0.8f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 200;
            NPC.damage = 38;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.width = (int)(34 * NPC.scale);
            NPC.height = (int)(36 * NPC.scale);
            NPC.aiStyle = -1;
            NPC.scale = 1f;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.Shatter;
            NPC.noGravity = true;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPC.Dnpc().Properties.Stone = true;
                NPC.Dnpc().Properties.BossLife = 1.1F;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.ServantOfTheStars"))
            });
        }
        public override bool PreAI()
        {
            //让他知道他的主人
            NPC parent = Main.npc[(int)NPC.ai[0]];
            //如果他的主人是星辰守卫就执行
            if (parent.type == ModContent.NPCType<StarGuard>() && parent.active)
            {
                NPC.target = parent.target;
                //数量
                int Proj = 0;
                //排序
                int G = 0;
                //遍历一遍NPC,知道这个星辰守卫有了多少仆从和仆从顺序
                for (int T = 0; T < 200; T++)
                {
                    if (Main.npc[T].ai[0] == NPC.ai[0])
                    {
                        if (Main.npc[T].type == ModContent.NPCType<ServantOfTheStars>() && Main.npc[T].active)
                        {
                            Proj++;
                            if (Main.npc[T].whoAmI < NPC.whoAmI)
                            {
                                G++;
                            }
                        }
                    }
                }
                //用数量与Boss的调整距离,以免太过密集
                //限制最小距离,以免和Boss重叠
                NPC.ai[3]++;
                if (NPC.ai[3] > 140)
                {
                    if (NPC.ai[2] < 1.2f)
                    {
                        NPC.ai[2] += 0.02f;
                    }
                }
                else
                {
                    if (NPC.ai[2] > 0.5f)
                    {
                        NPC.ai[2] -= 0.02f;
                    }
                }
                if (NPC.ai[3] < 200)
                {
                    NPC.damage = 0;
                    //让仆从围着Boss并且让Boss控制仆从旋转速度
                    Vector2 vector2 = parent.Center + Utils.RotatedBy(new Vector2(0f, 50), (Math.PI * 2 / Proj * G) + parent.localAI[2], default) - NPC.Center;

                    float a = vector2.Length() / 1.5f;
                    if (a < NPC.ai[1] / 10)
                    {
                        a = NPC.ai[1] / 10;
                    }
                    if (a > 30)
                    {
                        a = 30;
                    }
                    NPC.velocity = vector2.PerfectNormalize() * a;
                }
                else if (NPC.ai[3] == 200)
                {
                    Vector2 vector2 = Main.player[NPC.target].Center - (NPC.Center - Utils.RotatedBy(new Vector2(0f, 50), (Math.PI * 2 / Proj * G) + parent.localAI[2], default));

                    NPC.velocity = vector2.PerfectNormalize() * 13;
                }
                else if (NPC.ai[3] < 260)
                {
                    NPC.damage = NPC.defDamage;
                }
                else
                {
                    NPC.ai[3] = 0;
                }
                NPC.rotation += 0.08F;
                //防止他脱战
                NPC.timeLeft = 20;
                //如果他的主人正在结束动画
                if (parent.localAI[0] != 0)
                {
                    NPC.active = false;
                }
            }
            else
            {
                //如果他的主人不是星辰守卫
                NPC.active = false;
            }
            return false;
        }

        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<魔力水晶粒子>(), hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int a = 0; a < 8; a++)
                {
                    DDParticle.RequestParticleSpawn(ParticleType.Star, new ParticleOrchestraSettings
                    {
                        PositionInWorld = NPC.Center,
                        MovementVector = Main.rand.NextVector2Unit()
                    });
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = (Texture2D)TextureAssets.Npc[NPC.type];
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            //if (NPC.ai[3] > 140)
            {
                //绘制光效残影
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    Vector2 vector2 = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2 - Main.screenPosition;
                    Color color = new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length/2);
                    spriteBatch.Draw(Glow.Value, vector2, null, color, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f * NPC.ai[2] * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);

                }
                //绘制能量体大小
                spriteBatch.Draw(Glow.Value, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - Main.screenPosition, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f * NPC.ai[2], spriteEffects, 0f);
                spriteBatch.Draw(Glow.Value, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - Main.screenPosition, null, new Color(255 - NPC.alpha, 155 - NPC.alpha, 0, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f * NPC.ai[2] / 2, spriteEffects, 0f);
            }
            spriteBatch.Draw(texture, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, NPC.rotation, vector, NPC.scale, spriteEffects, 0f);
            return false;
        }
    }
}