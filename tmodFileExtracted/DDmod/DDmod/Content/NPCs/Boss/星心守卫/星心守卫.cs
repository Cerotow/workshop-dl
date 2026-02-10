using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Boss;
using DDmod.DDOn;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace DDmod.Content.NPCs.Boss.星心守卫
{
    [AutoloadBossHead]
    public class 星心守卫 : ModNPC
    {
        public static Asset<Texture2D> LGlow;
        public static Asset<Texture2D> MGlow;
        public static Asset<Texture2D> LGlow2;
        public static Asset<Texture2D> MGlow2;
        public static Asset<Texture2D> MLGlow;
        public override void Load()
        {
            LGlow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/星心守卫/心心守卫_Glow");
            MGlow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/星心守卫/星星守卫_Glow");
            LGlow2 = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/星心守卫/心心守卫2_Glow");
            MGlow2 = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/星心守卫/星星守卫2_Glow");
            MLGlow = ModContent.Request<Texture2D>("DDmod/Content/NPCs/Boss/星心守卫/星心守卫_Glow");
        }
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Life Guardian : Les");
           //DisplayName.AddTranslation(7, "生命守卫:莱斯");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;

        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 30;
            NPC.lifeMax =120000;
            NPC.defense = 12;
            NPC.knockBackResist = 0f;
            NPC.width = (int)(68 * NPC.scale);
            NPC.height = (int)(68 * NPC.scale);
            NPC.scale = 1f;
            NPC.value = Item.buyPrice(0, 5, 0, 0);
            NPC.npcSlots = 111f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            if (!Main.dedServ) Music = DDSystem.Music(2, "星心守卫");
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.Shatter;
            NPC.Dnpc().vector[0] = new Vector2(0, 300);
            NPC.Dnpc().Times[1] = 1;
            NPC.Dnpc().ArmorReduction = 0.75f;
            NPC.Dnpc().Properties.Stone = true;
            NPC.Dnpc().Properties.BossLife = 1.4F;

        }


        public override void FindFrame(int frameHeight)
        {
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override void BossLoot(ref int potionType)
        {
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        public static byte[] LifeTextures =
        [
            0,0,1,1,1,0,1,1,1,0,0,
            0,1,0,0,0,1,0,0,0,1,0,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,1,
            0,1,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,0,0,1,0,0,
            0,0,0,1,0,0,0,1,0,0,0,
            0,0,0,0,1,0,1,0,0,0,0,
            0,0,0,0,0,1,0,0,0,0,0,

        ];
        public byte[] LifeTextures2 =
        [
            0,0,1,1,1,0,0,0,1,1,1,0,0,
            0,1,0,0,0,1,0,1,0,0,0,1,0,
            1,0,0,0,0,0,1,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,0,0,1,
            0,1,0,0,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,0,0,0,0,1,0,0,
            0,0,0,1,0,0,0,0,0,1,0,0,0,
            0,0,0,0,1,0,0,0,1,0,0,0,0,
            0,0,0,0,0,1,0,1,0,0,0,0,0,
            0,0,0,0,0,0,1,0,0,0,0,0,0,

        ];
        public byte[] LifeTextures3 =
        [
            0,1,0,1,0,
            1,0,1,0,1,
            1,0,0,0,1,
            0,1,0,1,0,
            0,0,1,0,0,

        ];
        public static byte[] ManaTextures =
        [
            0,0,0,0,0,1,0,0,0,0,0,
            0,0,0,0,1,0,1,0,0,0,0,
            0,0,0,0,1,0,1,0,0,0,0,
            0,0,0,1,0,0,0,1,0,0,0,
            1,1,1,0,0,0,0,0,1,1,1,
            1,0,0,0,0,0,0,0,0,0,1,
            0,1,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,0,0,1,0,0,
            0,1,0,0,0,1,0,0,0,1,0,
            1,0,0,0,1,0,1,0,0,0,1,
            1,1,1,1,0,0,0,1,1,1,1,

        ];
        public byte[] ManaTextures2 =
        [
            0,0,0,0,0,0,1,0,0,0,0,0,0,
            0,0,0,0,0,1,0,1,0,0,0,0,0,
            0,0,0,0,0,1,0,1,0,0,0,0,0,
            0,0,0,0,1,0,0,0,1,0,0,0,0,
            1,1,1,1,0,0,0,0,0,1,1,1,1,
            1,0,0,0,0,0,0,0,0,0,0,0,1,
            0,1,0,0,0,0,0,0,0,0,0,1,0,
            0,0,1,0,0,0,0,0,0,0,1,0,0,
            0,0,0,1,0,0,0,0,0,1,0,0,0,
            0,0,1,0,0,0,1,0,0,0,1,0,0,
            0,1,0,0,0,1,0,1,0,0,0,1,0,
            1,0,0,0,1,0,0,0,1,0,0,0,1,
            1,1,1,1,0,0,0,0,0,1,1,1,1,

        ];
        public byte[] ManaTextures3 =
        [
            0,0,0,1,0,0,0,
            0,0,1,0,1,0,0,
            1,1,0,0,0,1,1,
            1,0,0,0,0,0,1,
            0,1,0,1,0,1,0,
            1,0,1,0,1,0,1,
            1,1,0,0,0,1,1,
        ];
        public override void AI()
        {
            int ProjDamage = 42;
            DDWorld.SunLight = 0.2F;
            Player P = Main.player[NPC.target];
            //如果Boss没有目标,或者玩家距离较远,死亡刷新攻击目标
            //Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 10, false, 0.15F);
            NPC.damage = 0;
            NPC.Dnpc().Bool[0] = false;
            if (NPC.target < 0 || NPC.target == 255 || P.dead || !P.active)
            {
                NPC.TargetClosest(true);
                if (P.Distance(NPC.Center) > 10000)
                {
                    NPC.timeLeft -= 10;
                }
                NPC.velocity += NPC.velocity.PerfectNormalize() * 0.4F;
            }
            else
            {
                if (NPC.Dnpc().Stage == 0)
                {
                    Vector2 vector = P.Center - NPC.Dnpc().vector[0] - NPC.Center;
                    float speed = vector.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                    if (NPC.ai[0]++ > 180)
                    {
                        NPC.Dnpc().Stage = 1;
                        NPC.ai[0] = 0;
                    }

                }
                else
                if (NPC.Dnpc().Stage == 1)
                {
                    NPC.ai[0]++;
                    if (NPC.ai[0] < 700)
                    {
                        Vector2 vector = P.Center - NPC.Dnpc().vector[0] - NPC.Center;
                        float speed = vector.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                        if (Main.netMode != 1)
                        {
                            if (NPC.ai[0] % 240 == 0)
                            {
                                NPC.Dnpc().vector[0] = new Vector2(Main.rand.NextFloat(-500, 500), Main.rand.NextFloat(-500, 500));
                                for (int a = 0; a < ManaTextures.Length; a++)
                                {
                                    if (ManaTextures[a] == 1)
                                    {
                                        vector = new Vector2(a % 11, a / 11);
                                        vector = vector.RotatedBy(NPC.rotation);
                                        Vector2 velocity = (vector - (new Vector2(10).RotatedBy(NPC.rotation) / 2));
                                        NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(11).RotatedBy(NPC.rotation) / 2 + vector + velocity * 4, velocity * 3, ModContent.ProjectileType<Boss星星>(), ProjDamage, 1, -1);
                                    }
                                    /*
                                    if (ManaTextures[a] == 1)
                                    {
                                        vector = new Vector2(a % 11, a / 11);
                                        Vector2 velocity = (vector - new Vector2(11) / 2);
                                        NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(11) / 2 + vector+ velocity*4, velocity, ModContent.ProjectileType<Boss星星>(), 12, 12, -1,1,NPC.whoAmI);
                                    }*/
                                }
                            }
                            if (NPC.ai[0] % 240 == 120)
                            {
                                NPC.Dnpc().vector[0] = new Vector2(Main.rand.NextFloat(-500, 500), Main.rand.NextFloat(-500, 500));
                                for (int a = 0; a < LifeTextures.Length; a++)
                                {
                                    if (LifeTextures[a] == 1)
                                    {
                                        vector = new Vector2(a % 11, a / 11);
                                        vector = vector.RotatedBy(NPC.rotation);
                                        Vector2 velocity = (vector - (new Vector2(10).RotatedBy(NPC.rotation) / 2));
                                        NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(11).RotatedBy(NPC.rotation) / 2 + vector + velocity * 4, velocity * 3, ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1);
                                    }
                                    /*
                                    if (LifeTextures[a] == 1)
                                    {
                                        vector = new Vector2(a % 11, a / 11);
                                        vector = vector;
                                        Vector2 velocity = (vector - new Vector2(11) / 2);
                                        NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(11) / 2 + vector + velocity *4, velocity, ModContent.ProjectileType<Boss心心>(), 12, 12, -1,1, NPC.whoAmI);
                                    }
                                    */
                                }
                            }
                        }
                    }
                    else
                    if (NPC.ai[0] < 900)
                    {
                        Vector2 vector = P.Center - NPC.Dnpc().vector[0] - NPC.Center;
                        float speed = vector.Length() / 10;
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                        if (Main.netMode != 1)
                        {
                            if (NPC.ai[0] % 60 == 0)
                            {
                                NPC.Dnpc().vector[0] = new Vector2(Main.rand.NextFloat(-500, 500), Main.rand.NextFloat(-500, 500));
                                for (int a = 0; a < ManaTextures.Length; a++)
                                {
                                    if (ManaTextures[a] == 1)
                                    {
                                        vector = new Vector2(a % 11, a / 11);
                                        vector = vector.RotatedBy(NPC.rotation);
                                        Vector2 velocity = (vector - (new Vector2(10).RotatedBy(NPC.rotation) / 2));
                                        NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(11).RotatedBy(NPC.rotation) / 2 + vector + velocity * 4, (vector - new Vector2(11).RotatedBy(NPC.rotation) / 2) * 1, ModContent.ProjectileType<Boss星星>(), ProjDamage, 1, -1, 1);
                                    }
                                }
                            }
                            if (NPC.ai[0] % 60 == 30)
                            {
                                NPC.Dnpc().vector[0] = new Vector2(Main.rand.NextFloat(-500, 500), Main.rand.NextFloat(-500, 500));
                                for (int a = 0; a < LifeTextures.Length; a++)
                                {

                                    if (LifeTextures[a] == 1)
                                    {
                                        vector = new Vector2(a % 11, a / 11);
                                        vector = vector.RotatedBy(NPC.rotation);
                                        Vector2 velocity = (vector - (new Vector2(10).RotatedBy(NPC.rotation) / 2));
                                        NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(11).RotatedBy(NPC.rotation) / 2 + vector + velocity * 4, (vector - new Vector2(11).RotatedBy(NPC.rotation) / 2) * 1, ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, 1);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        NPC.Dnpc().vector[0] = new Vector2(0, 300);
                        Vector2 vector = P.Center - NPC.Dnpc().vector[0] - NPC.Center;
                        float speed = vector.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                        if (NPC.ai[0] > 1000)
                        {
                            NPC.ai[0] = 0;
                        }
                    }
                    if (NPC.lifeMax * 0.66F > NPC.life)
                    {
                        NPC.Dnpc().Stage = 2;
                        NPC.ai[0] = -120;
                    }
                }
                else
                if (NPC.Dnpc().Stage == 2)
                {
                    Vector2 vector = P.Center - NPC.Center;
                    float speed = vector.Length() / 20;
                    if (vector.Length() < 800 && speed > 12)
                    {
                        speed = 12;
                    }
                    NPC.rotation = NPC.velocity.X * 0.03F;
                    NPC.ai[0]++;
                    if (NPC.ai[0] >= 0 && NPC.ai[0] < 300)
                    {
                        if (NPC.ai[0] < 200)
                        {
                            if (Main.netMode != 1)
                            {
                                if (NPC.ai[1] == 0)
                                {
                                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                                    if (NPC.ai[0] % 10 == 0)
                                    {
                                        NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector.PerfectNormalize() * 15, ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, -1);
                                        NPC.velocity -= vector.PerfectNormalize() * 14;
                                    }
                                }
                                else
                                {
                                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed / 10) / 21;
                                    if (NPC.ai[0] % 60 == 0)
                                    {
                                        for (int a = 0; a < LifeTextures3.Length; a++)
                                        {
                                            if (LifeTextures3[a] == 1)
                                            {
                                                Vector2 PO = new Vector2(a % 5, a / 5).RotatedBy(vector.ToRotation() - MathHelper.PiOver2);
                                                Vector2 velocity = (PO - (new Vector2(4).RotatedBy(vector.ToRotation() - MathHelper.PiOver2) / 2));

                                                int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector.PerfectNormalize() * 8, ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, -2);
                                                Main.projectile[Proj].DProj().vector[0] = velocity * 10;

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                        }
                        if (NPC.ai[0] == 200)
                        {
                            if (Main.netMode != 1)
                            {
                                if (NPC.ai[1] == 0)
                                {
                                    for (int a = 0; a < LifeTextures3.Length; a++)
                                    {
                                        if (LifeTextures3[a] == 1)
                                        {
                                            Vector2 PO = new Vector2(a % 5, a / 5).RotatedBy(vector.ToRotation() - MathHelper.PiOver2);
                                            Vector2 velocity = (PO - (new Vector2(4).RotatedBy(vector.ToRotation() - MathHelper.PiOver2) / 2));

                                            int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector.PerfectNormalize() * 8, ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, -2);
                                            Main.projectile[Proj].DProj().vector[0] = velocity * 10;

                                        }
                                    }
                                }
                                if (NPC.ai[1] == 2)
                                {

                                    for (int a = 0; a < LifeTextures.Length; a++)
                                    {
                                        if (LifeTextures[a] == 1)
                                        {
                                            Vector2 PO = new Vector2(a % 11, a / 11).RotatedBy(vector.ToRotation() - MathHelper.PiOver2);
                                            Vector2 velocity = (PO - (new Vector2(10).RotatedBy(vector.ToRotation() - MathHelper.PiOver2) / 2));

                                            int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector.PerfectNormalize() * 3, ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, -2);
                                            Main.projectile[Proj].DProj().vector[0] = velocity * 4;

                                        }
                                    }
                                }
                            }
                            NPC.ai[1]++;
                        }
                    }
                    else if (NPC.ai[0] == 300)
                    {
                        if (NPC.ai[1] < 3)
                        {
                            NPC.ai[0] = 0;
                        }
                        else
                        {
                            NPC.ai[1] = 0;
                            if (vector.X > 0)
                            {
                                NPC.Dnpc().Times[0] = 1;
                            }
                            else
                            {
                                NPC.Dnpc().Times[0] = -1;
                            }
                        }
                    }
                    //>300
                    else if (NPC.ai[0] < 900)
                    {
                        if (NPC.ai[0] > 800)
                        {
                            NPC.Dnpc().vector[0] = new Vector2(0, 400);
                            vector = P.Center - NPC.Dnpc().vector[0] - NPC.Center;
                            speed = vector.Length() / 10;
                            if (vector.Length() < 800 && speed > 18)
                            {
                                speed = 18;
                            }
                            NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                        }
                        else
                        {

                            if (NPC.ai[0] < 640)
                            {
                                Vector2 po = P.Center - new Vector2(300 * NPC.Dnpc().Times[0], 300) - NPC.Center;
                                speed = po.Length() / 10;
                                if (po.Length() < 800 && speed > 18)
                                {
                                    speed = 18;
                                }
                                NPC.velocity = (NPC.velocity * 20 + po.PerfectNormalize() * speed) / 21;
                            }
                            if (NPC.ai[0] > 390 && NPC.ai[0] < 600)
                            {
                                if (NPC.ai[0] % 10 == 0)
                                {
                                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector.PerfectNormalize() * 15, ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, -1);
                                    NPC.velocity -= vector.PerfectNormalize() * 5;
                                }
                            }
                            if (NPC.ai[0] >= 640)
                            {
                                NPC.rotation = vector.ToRotation() - MathHelper.PiOver2;
                                NPC.Dnpc().Bool[0] = true;
                                if (NPC.Dnpc().Times[1] < 3.5F)
                                {
                                    NPC.Dnpc().Times[1] += 0.2F;
                                }
                                else
                                {
                                    NPC.Dnpc().Times[1] = 3.5f;
                                }
                                if (NPC.ai[0] < 700)
                                {
                                    NPC.velocity = -vector.PerfectNormalize() * 10;
                                }
                            }
                            if (NPC.ai[0] == 700)
                            {
                                NPC.velocity = vector.PerfectNormalize() * 50;
                                for (int a = 0; a < LifeTextures.Length; a++)
                                {
                                    if (LifeTextures[a] == 1)
                                    {
                                        Vector2 PO = new Vector2(a % 11, a / 11).RotatedBy(vector.ToRotation() - MathHelper.PiOver2);
                                        Vector2 velocity = (PO - (new Vector2(10).RotatedBy(vector.ToRotation() - MathHelper.PiOver2) / 2));

                                        int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector.PerfectNormalize() * 9, ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, -2);
                                        Main.projectile[Proj].DProj().vector[0] = velocity * 4;

                                    }
                                }
                            }
                            if (NPC.ai[0] > 700)
                            {
                                NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                                int W = (int)(NPC.width * NPC.Dnpc().Times[1]);
                                int H = (int)(NPC.height * NPC.Dnpc().Times[1]);
                                if (new Rectangle((int)NPC.Center.X - W / 2, (int)NPC.Center.Y - H / 2, W, H).Intersects(P.getRect()))
                                {
                                    if (!P.noKnockback && !P.immune)
                                    {
                                        P.velocity = vector.PerfectNormalize() * 30;
                                    }
                                    P.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), (int)(NPC.defDamage * 2f), 0);
                                }
                                if (NPC.ai[0] % 5 == 0)
                                {
                                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity.RotatedBy(MathHelper.PiOver2).PerfectNormalize(), ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, -1);
                                    NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity.RotatedBy(-MathHelper.PiOver2).PerfectNormalize(), ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, -1);
                                }
                            }
                        }
                        if (NPC.ai[0] == 800)
                        {
                            if (NPC.ai[1] < 2)
                            {
                                NPC.ai[1]++;
                                NPC.Dnpc().Times[0] *= -1;
                                NPC.ai[0] = 301;
                            }
                            else
                            {
                                NPC.ai[1] = 0;
                            }
                        }
                    }
                    else if (NPC.ai[0] < 1500)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(0, -400);
                        vector = P.Center - NPC.Dnpc().vector[0] - NPC.Center;
                        speed = vector.Length() / 10;
                        if (vector.Length() < 800 && speed > 18)
                        {
                            speed = 18;
                        }
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                        if (NPC.ai[0] < 1400)
                        {
                            if (NPC.ai[0] % 120 == 0)
                            {
                                NPC.Dnpc().vector[0].X = Main.rand.Next(-600, 600);
                                for (int a = 0; a < LifeTextures3.Length; a++)
                                {
                                    if (LifeTextures3[a] == 1)
                                    {
                                        Vector2 PO = new Vector2(a % 5, a / 5).RotatedBy((P.Center - NPC.Center).ToRotation() - MathHelper.PiOver2);
                                        Vector2 velocity = (PO - (new Vector2(4).RotatedBy((P.Center - NPC.Center).ToRotation() - MathHelper.PiOver2) / 2));

                                        int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center, (P.Center - NPC.Center).PerfectNormalize() * 8, ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, -2);
                                        Main.projectile[Proj].DProj().vector[0] = velocity * 10;

                                    }
                                }
                            }

                            if (NPC.ai[0] % 10 == 0)
                            {
                                NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.Next(-1000, 1000), 1000), new Vector2(0, -10), ModContent.ProjectileType<Boss心心>(), ProjDamage, 1, -1, -3, 0, Main.rand.NextFloat(0.7F, 1.3F));
                            }

                        }

                    }
                    else
                    {
                        NPC.ai[0] = 0;
                        NPC.ai[1] = 0;
                    }
                    if (NPC.lifeMax * 0.33F > NPC.life)
                    {
                        NPC.Dnpc().Stage = 3;
                        NPC.ai[0] = -120;
                        NPC.ai[1] = 0;
                    }
                }
                else
                if (NPC.Dnpc().Stage == 3)
                {
                    NPC.ai[0]++;
                    if (NPC.ai[0] <= 0)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(0, 300);
                        Vector2 vector = P.Center - NPC.Dnpc().vector[0] - NPC.Center;
                        float speed = vector.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                    }
                    else if (NPC.ai[0] >= 0 && NPC.ai[0] < 500)
                    {
                        Vector2 vector = P.Center - NPC.Dnpc().vector[0] - NPC.Center;
                        if (NPC.ai[0] < 300)
                        {
                            float speed = vector.Length() / 20;
                            NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;

                            if (NPC.ai[0] % 120 == 0)
                            {
                                NPC.Dnpc().vector[0] = new Vector2(Main.rand.NextFloat(-400, 400), 300);
                                for (int a = 0; a < ManaTextures3.Length; a++)
                                {
                                    if (ManaTextures3[a] == 1)
                                    {
                                        Vector2 PO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                        Vector2 velocity = (PO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                        int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -30) + P.Dplayer().PrePosition * 3, ModContent.ProjectileType<Boss星星>(), ProjDamage, 1, -1, -2);
                                        Main.projectile[Proj].DProj().vector[0] = velocity * 5;
                                    }

                                }
                            }
                        }
                        else
                        {
                            vector = P.Center - NPC.Center;
                            if (NPC.ai[0] < 500)
                            {
                                NPC.Dnpc().Bool[0] = true;
                                if (NPC.Dnpc().Times[1] < 2.5F)
                                {
                                    NPC.Dnpc().Times[1] += 0.2F;
                                }
                                else
                                {
                                    NPC.Dnpc().Times[1] = 2.5f;
                                }
                                int W = (int)(NPC.width * NPC.Dnpc().Times[1]);
                                int H = (int)(NPC.height * NPC.Dnpc().Times[1]);
                                if (new Rectangle((int)NPC.Center.X - W / 2, (int)NPC.Center.Y - H / 2, W, H).Intersects(P.getRect()))
                                {
                                    if (!P.noKnockback && !P.immune)
                                    {
                                        P.velocity = vector.PerfectNormalize() * 16;
                                    }
                                    P.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), (int)(NPC.defDamage), 0);
                                }
                            }

                            if (NPC.ai[0] == 300)
                            {
                                for (int a = 0; a < ManaTextures3.Length; a++)
                                {
                                    if (ManaTextures3[a] == 1)
                                    {
                                        Vector2 PO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                        Vector2 velocity = (PO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                        int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(0, -30) + P.Dplayer().PrePosition * 3, ModContent.ProjectileType<Boss星星>(), ProjDamage, 1, -1, -2);
                                        Main.projectile[Proj].DProj().vector[0] = velocity * 5;
                                    }

                                }
                            }
                            if (NPC.ai[0] == 400)
                            {
                                NPC.velocity = vector.PerfectNormalize() * 50;
                                for (int a = 0; a < ManaTextures3.Length; a++)
                                {
                                    if (ManaTextures3[a] == 1)
                                    {
                                        Vector2 PO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                        Vector2 velocity = (PO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                        int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector.PerfectNormalize() * 5, ModContent.ProjectileType<Boss星星>(), ProjDamage, 1, -1, -4);
                                        Main.projectile[Proj].DProj().vector[0] = velocity * 7;
                                    }
                                }
                            }
                            if (NPC.ai[0] > 380 && NPC.ai[0] < 400)
                            {
                                NPC.velocity = -vector.PerfectNormalize() * 40;
                            }
                            if (NPC.ai[0] <= 380)
                            {
                                NPC.velocity = vector.PerfectNormalize() * 3;
                            }
                        }
                    }
                    else if (NPC.ai[0] == 500)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(0, 300);
                        if (NPC.ai[1] < 2)
                        {
                            NPC.ai[0] = 0;
                            NPC.ai[1]++;
                        }
                        else
                        {
                            NPC.ai[1] = 0;
                        }
                    }
                    else if (NPC.ai[0] < 800)
                    {
                        Vector2 vector = P.Center - NPC.Center;
                        NPC.Dnpc().Bool[0] = true;
                        if (NPC.Dnpc().Times[1] < 2.5F)
                        {
                            NPC.Dnpc().Times[1] += 0.2F;
                        }
                        else
                        {
                            NPC.Dnpc().Times[1] = 2.5f;
                        }
                        int W = (int)(NPC.width * NPC.Dnpc().Times[1]);
                        int H = (int)(NPC.height * NPC.Dnpc().Times[1]);
                        if (new Rectangle((int)NPC.Center.X - W / 2, (int)NPC.Center.Y - H / 2, W, H).Intersects(P.getRect()))
                        {
                            if (!P.noKnockback && !P.immune)
                            {
                                P.velocity = vector.PerfectNormalize() * 16;
                            }
                            P.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), (int)(NPC.defDamage), 0);
                        }
                        if (NPC.ai[0] <= 680 && NPC.ai[1] <= 4)
                        {
                            if (NPC.ai[1] == 0)
                            {
                                NPC.Dnpc().vector[1] = new Vector2(P.Center.X, P.Center.Y - 300);
                            }
                            if (NPC.ai[1] == 1)
                            {
                                NPC.Dnpc().vector[1] = new Vector2(P.Center.X + 300, P.Center.Y);
                            }
                            if (NPC.ai[1] == 2)
                            {
                                NPC.Dnpc().vector[1] = new Vector2(P.Center.X + 300, P.Center.Y + 300);
                            }
                            if (NPC.ai[1] == 3)
                            {
                                NPC.Dnpc().vector[1] = new Vector2(P.Center.X - 300, P.Center.Y + 300);
                            }
                            if (NPC.ai[1] == 4)
                            {
                                NPC.Dnpc().vector[1] = new Vector2(P.Center.X - 300, P.Center.Y);
                            }
                            if (NPC.ai[0] % 5 == 0)
                            {
                                for (int a = 0; a < ManaTextures3.Length; a++)
                                {
                                    if (ManaTextures3[a] == 1)
                                    {
                                        Vector2 PO = new Vector2(a % 7, a / 7);
                                        Vector2 velocity = (PO - (new Vector2(6) / 2));

                                        int Proj = NewDust(NPC.Dnpc().vector[1] - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 0.8F);
                                        Main.dust[Proj].velocity = velocity*2;
                                        Main.dust[Proj].customData = 3;

                                    }
                                }
                            }
                            if (NPC.ai[0] == 680)
                            {
                                NPC.Center = NPC.Dnpc().vector[1];
                                for (int a = 0; a < NPC.oldPos.Length; a++)
                                {
                                    NPC.oldPos[a] = Vector2.Zero;
                                }
                                for (int a = 0; a < ManaTextures3.Length; a++)
                                {
                                    if (ManaTextures3[a] == 1)
                                    {
                                        Vector2 PO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                        Vector2 velocity = (PO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));
                                        vector = P.Center - NPC.Center;
                                        int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vector.PerfectNormalize() * 2, ModContent.ProjectileType<Boss星星>(), ProjDamage, 1, -1, -4);
                                        Main.projectile[Proj].DProj().vector[0] = velocity * 3;
                                    }
                                }
                                for (int a = 0; a < ManaTextures.Length; a++)
                                {
                                    if (ManaTextures[a] == 1)
                                    {
                                        Vector2 PO = new Vector2(a % 11, a / 11);
                                        Vector2 velocity = (PO - (new Vector2(10) / 2));

                                        int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 0.5F);
                                        Main.dust[Proj].velocity = velocity;
                                        Main.dust[Proj].customData = 5;

                                    }
                                }
                            }
                        }
                        if (NPC.ai[0] < 700)
                        {
                            NPC.velocity = -vector.PerfectNormalize() * 30;
                        }
                        if (NPC.ai[0] == 700)
                        {
                            NPC.velocity = vector.PerfectNormalize() * 50;
                        }
                    }
                    else if (NPC.ai[0] == 800)
                    {
                        NPC.Dnpc().vector[0] = new Vector2(0, 300);
                        if (NPC.ai[1] < 4)
                        {
                            NPC.ai[0] = 501;
                            NPC.ai[1]++;
                        }
                        else
                        {
                            NPC.Center = NPC.Dnpc().vector[1];
                            for (int a = 0; a < NPC.oldPos.Length; a++)
                            {
                                NPC.oldPos[a] = Vector2.Zero;
                            }
                            for (int a = 0; a < ManaTextures.Length; a++)
                            {
                                if (ManaTextures[a] == 1)
                                {
                                    Vector2 PO = new Vector2(a % 11, a / 11);
                                    Vector2 velocity = (PO - (new Vector2(10) / 2));

                                    int Proj = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<星星粒子>(), Scale: 0.5F);
                                    Main.dust[Proj].velocity = velocity;
                                    Main.dust[Proj].customData = 5;

                                }
                            }
                            NPC.Dnpc().vector[0] = new Vector2(0, 500);
                            NPC.ai[1] = 0;
                        }
                    }
                    else if (NPC.ai[0] < 1500)
                    {
                        Vector2 vector = P.Center - NPC.Dnpc().vector[0] - NPC.Center;
                        float speed = vector.Length() / 20;
                        NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;

                        if (NPC.ai[0] % 120 == 0)
                        {
                            NPC.Dnpc().vector[0] = new Vector2(Main.rand.NextFloat(-400, 400), 500);
                            for (int a = 0; a < ManaTextures3.Length; a++)
                            {
                                if (ManaTextures3[a] == 1)
                                {
                                    Vector2 PO = new Vector2(a % 7, a / 7).RotatedBy(NPC.rotation);
                                    Vector2 velocity = (PO - (new Vector2(6).RotatedBy(NPC.rotation) / 2));

                                    int Proj = NewProjectile(NPC.GetSource_FromAI(), NPC.Center,(P.Center - NPC.Center).PerfectNormalize() * 1, ModContent.ProjectileType<Boss星星>(), ProjDamage, 1, -1, -1);
                                    Main.projectile[Proj].DProj().vector[0] = velocity * 2;
                                }

                            }
                        }
                        if (NPC.ai[0] % 10 == 0)
                        {
                            NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.Next(-1000, 1000), -1000), new Vector2(Main.rand.Next(-6, 6), 16), ModContent.ProjectileType<Boss星星>(), ProjDamage, 1, -1, -3, 0, Main.rand.NextFloat(0.7F, 1.3F));
                        }
                    }
                    else
                    {
                        NPC.Dnpc().vector[0] = new Vector2(0, 300);
                        NPC.ai[0] = 0;
                        NPC.ai[1] = 0;
                    }
                }
            }
            if (NPC.Dnpc().Stage != 2)
            {
                NPC.rotation += NPC.velocity.X * 0.01F;
                if (NPC.velocity.X > 0)
                {
                    NPC.rotation += Math.Abs(NPC.velocity.Y) * 0.01F;
                }
                else
                {
                    NPC.rotation -= Math.Abs(NPC.velocity.Y) * 0.01F;
                }
            }
            if(!NPC.Dnpc().Bool[0])
            {
                if (NPC.Dnpc().Times[1] > 1F)
                {
                    NPC.Dnpc().Times[1] -= 0.2F;
                }
                else if (NPC.Dnpc().Times[1] < 0.94F)
                {
                    NPC.Dnpc().Times[1] += 0.05f;
                }
                else
                {
                    NPC.Dnpc().Times[1] = 1;
                }
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 2; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<生命水晶粒子>(), hit.HitDirection, -1f, 0, default, 1f);
                NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<魔力水晶粒子>(), hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<生命水晶粒子>(), hit.HitDirection, -1f, 0, default, 1f);
                    NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<魔力水晶粒子>(), hit.HitDirection, -1f, 0, default, 1f);
                }
                if (NPC.localAI[1] == 1)
                {
                    NPC.localAI[1] = 2;
                }
                NPC.dontTakeDamage = true;
                if (NPC.Dnpc().Deathrattle)
                {
                    NPC.life = 5;
                }
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            if (NPC.Dnpc().Stage < 2)
            {
                float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
                return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
                {
                new Color(255, 100, 100, 0),
                }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(0, 100, 255, 0), (float)Math.Pow((double)completionRatio, 1.0));
            }
            else if (NPC.Dnpc().Stage == 2)
            {

                float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
                return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
                {
                new Color(255,100, 100, 0),
                }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(255, 100, 100, 0), (float)Math.Pow((double)completionRatio, 1.0));
            }
            else
            {

                float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
                return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
                {
                new Color(0, 100, 255, 0),
                }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(0, 100, 255, 0), (float)Math.Pow((double)completionRatio, 1.0));
            }

        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                50,
                50,
                60,
                50,
                50,
            }) * MathHelper.Lerp(0f, 1.4f, widthRatio), 20, (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal Trailing TrailDrawer;
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(NPC.velocity.Length() / 20);
            TrailDrawer.Draw(NPC.oldPos, NPC.Size * 0.5f - Main.screenPosition, 204, null,NPC.scale * NPC.Dnpc().Times[1]);

            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (NPC.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            /*
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                if (i != 0)
                {
                    Vector2 vector2 = NPC.oldPos[i] + NPC.Size / 2 - Main.screenPosition;
                    Color color = new Color(255 - NPC.alpha, 50 - NPC.alpha, 50 - NPC.alpha, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                    spriteBatch.Draw(LGlow.Value, vector2, null, color, NPC.rotation, LGlow.Size() / 2, NPC.scale * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);
                    spriteBatch.Draw(LGlow.Value, vector2, null, color, NPC.rotation, LGlow.Size() / 2, NPC.scale * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);
                    color = new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                    spriteBatch.Draw(MGlow.Value, vector2, null, color, NPC.rotation, MGlow.Size() / 2, NPC.scale * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);
                    spriteBatch.Draw(MGlow.Value, vector2, null, color, NPC.rotation, MGlow.Size() / 2, NPC.scale * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);
                }
            }*/
            Vector2 vector = texture.Size() / 2;
            //绘制能量体大小
            
            spriteBatch.Draw(LGlow.Value, NPC.Center - Main.screenPosition, null, new Color(255 - NPC.alpha, 100 - NPC.alpha, 100 - NPC.alpha, 0) * 1f, NPC.rotation, LGlow.Size() / 2, NPC.scale, spriteEffects, 0f);
            spriteBatch.Draw(LGlow.Value, NPC.Center - Main.screenPosition, null, new Color(255 - NPC.alpha, 100 - NPC.alpha, 100 - NPC.alpha, 0) * 1f, NPC.rotation, LGlow.Size() / 2, NPC.scale, spriteEffects, 0f);

            spriteBatch.Draw(MGlow.Value, NPC.Center - Main.screenPosition, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, MGlow.Size() / 2, NPC.scale, spriteEffects, 0f);
            spriteBatch.Draw(MGlow.Value, NPC.Center - Main.screenPosition, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, MGlow.Size() / 2, NPC.scale, spriteEffects, 0f);


            //spriteBatch.Draw(MGlow2.Value, NPC.Center - Main.screenPosition, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, MGlow2.Size() / 2, NPC.scale, spriteEffects, 0f);
            //spriteBatch.Draw(MGlow2.Value, NPC.Center - Main.screenPosition, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, MGlow2.Size() / 2, NPC.scale, spriteEffects, 0f);
            if (NPC.Dnpc().Stage == 2)
            {
                spriteBatch.Draw(LGlow2.Value, NPC.Center + new Vector2(0, 10).RotatedBy(NPC.rotation) - Main.screenPosition, null, new Color(50 - NPC.alpha, 50 - NPC.alpha, 50 - NPC.alpha, 255 - NPC.alpha) * 1f, NPC.rotation, LGlow2.Size() / 2, NPC.scale / 4 * 1.25F * NPC.Dnpc().Times[1], spriteEffects, 0f);
                spriteBatch.Draw(LGlow2.Value, NPC.Center + new Vector2(0, 10).RotatedBy(NPC.rotation) - Main.screenPosition, null, new Color(255 - NPC.alpha, 100 - NPC.alpha, 100 - NPC.alpha, 0) * 1f, NPC.rotation, LGlow2.Size() / 2, NPC.scale / 4 * 1.25F * NPC.Dnpc().Times[1], spriteEffects, 0f);
                spriteBatch.Draw(LGlow2.Value, NPC.Center + new Vector2(0, 10).RotatedBy(NPC.rotation) - Main.screenPosition, null, new Color(255 - NPC.alpha, 100 - NPC.alpha, 100 - NPC.alpha, 0) * 1f, NPC.rotation, LGlow2.Size() / 2, NPC.scale / 4 * 1.25F * NPC.Dnpc().Times[1], spriteEffects, 0f);
            }
            if (NPC.Dnpc().Stage == 3)
            {
                spriteBatch.Draw(MGlow2.Value, NPC.Center - Main.screenPosition, null, new Color(50 - NPC.alpha, 50 - NPC.alpha, 50 - NPC.alpha, 255 - NPC.alpha) * 1f, NPC.rotation, MGlow2.Size() / 2, NPC.scale / 4 * 1.35F * NPC.Dnpc().Times[1], spriteEffects, 0f);
                spriteBatch.Draw(MGlow2.Value, NPC.Center - Main.screenPosition, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, MGlow2.Size() / 2, NPC.scale / 4 * 1.35F * NPC.Dnpc().Times[1], spriteEffects, 0f);
                spriteBatch.Draw(MGlow2.Value, NPC.Center - Main.screenPosition, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, MGlow2.Size() / 2, NPC.scale / 4 * 1.35F * NPC.Dnpc().Times[1], spriteEffects, 0f);
            }
            /*
            */
            //spriteBatch.Draw(MLGlow.Value, NPC.Center - Main.screenPosition, null, new Color(255 - NPC.alpha, 0 , 0, 0) * 1f, NPC.rotation, MLGlow.Size() / 2, NPC.scale/4*2, spriteEffects, 0f);

            //如果没被击败就绘制贴图
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, null, Color.White, NPC.rotation, vector, NPC.scale, spriteEffects, 0f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return false;
        }
        public override bool CheckDead()
        {
            return true;
        }
    }
}