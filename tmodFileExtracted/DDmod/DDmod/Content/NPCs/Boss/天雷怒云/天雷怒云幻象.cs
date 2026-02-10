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
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.天雷怒云
{
    [AutoloadBossHead]
    public class 天雷怒云幻象 : ModNPC
    {
        public static int Head;
        public static Asset<Texture2D> Glow;
        public static Asset<Texture2D> Pointer;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
        }

        public override void SetDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPC.damage = 30;
            NPC.width = 140;
            NPC.height = 84;
            NPC.defense = 0;
            NPC.lifeMax = 100;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit30;
            NPC.DeathSound = SoundID.NPCDeath33;
            NPC.netAlways = true;
            NPC.dontCountMe = true;
            NPC.scale = 1F;
            NPC.value = 0f;
            NPC.alpha = 255;
            NPC.Dnpc().Properties.Light= true;
            NPC.dontTakeDamage = true;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override void BossLoot(ref int potionType)
        {
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        public override void OnKill()
        {
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
        }
        public override void AI()
        {
            if (!AnyNPCs(ModContent.NPCType<天雷怒云>()))
            {
                NPC.Kill(false);
                return;
            }
            NPC.damage = 0;
            NPC.TargetClosest();
            NPC npc = Main.npc[FindFirstNPC(ModContent.NPCType<天雷怒云>())];
            Player player = Main.player[NPC.target];
            Vector2 vector = npc.Center - NPC.Center;
            vector.Y += 50;
            vector.X += NPC.ai[2];
            DDHelper.BackAndForth(-500, 500, 10, ref NPC.ai[2], ref NPC.Dnpc().Bool[3]);

            float R = vector.Length() /2;
            if (R > 30)
            {
                R = 30;
            }
            if (NPC.alpha > 0)
            {
                NPC.alpha -= 5;
            }
            NPC.ai[1]++;
            NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * R) / 21;
            if (NPC.ai[0] == 2)
            {
                if (NPC.ai[1] % 60 > 50 || NPC.ai[1] % 60 < 10)
                {
                    NPC.velocity = Vector2.Zero;
                }
            }
            if (NPC.ai[1] > 540)
            {
                if (NPC.ai[1] >= 600)
                {
                    if (NPC.ai[0] < 2)
                    {
                        for (int A = 0; A < 18; A++)
                        {
                            Vector2 P = NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, NPC.height / 2 - 10);
                            NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 0, Main.rand.Next(150, 200));
                            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                            sound.Pitch = 1f;
                            sound.MaxInstances = 20;
                            sound.Volume = .1f;
                            PlaySound(sound, P);
                        }
                    }
                    NPC.velocity = Vector2.Zero;
                    NPC.ai[0]++;
                    NPC.ai[1] = 0;
                    if(NPC.ai[0]>2)
                    {
                        NPC.ai[0] = 0;
                    }
                }
                else
                {
                    if (NPC.ai[0] < 2)
                    {
                        if (NPC.ai[1] % 6 == 0)
                        {
                            Vector2 P = NPC.Center + new Vector2(Main.rand.Next(-NPC.width / 2 * 10, NPC.width / 2 * 10) / 10, NPC.height / 2 - 10);
                            NPC.NewNPCProj(P, (P - NPC.Center).PerfectNormalize() * 4, ModContent.ProjectileType<Boss闪电>(), 30, 1, -1, 0, 1, 120);
                            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                            sound.Pitch = 1f;
                            sound.MaxInstances = 20;
                            sound.Volume = .1f;
                            PlaySound(sound, P);
                        }
                    }
                }
            }
            else
            {
                if (NPC.ai[0] < 2)
                {
                    if (NPC.ai[1] < 30)
                    {
                        NPC.velocity = Vector2.Zero;
                    }
                }
            }
        }
        public override bool CheckActive()
        {
            return false;
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
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Width = 180;
            NPC.frameCounter++;
            if (NPC.Dnpc().Bool[4])
            {
                NPC.frame.X = 180;
                NPC.frame.Y = frameHeight * ((int)(NPC.frameCounter / 6) % 3);
            }
            else
            {

                NPC.frame.X = 0;
                NPC.frame.Y = frameHeight * ((int)(NPC.frameCounter / 6) % Main.npcFrameCount[Type]);
            }
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
            Color color = NPC.GetAlpha(new Color(0,15,15));
            color.A = 0;
            drawColor = NPC.GetAlpha(Color.White) * 0.4F;
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Rectangle rectangle;
            if (NPC.Dnpc().Times[4] > 0)
            {
                rectangle = new Rectangle(0, Pointer.Height() / 6 * ((int)(NPC.frameCounter / 6) % 6), Pointer.Width(), Pointer.Height() / 6);
                spriteBatch.Draw(Pointer.Value, NPC.Center - screenPos, rectangle, Color.White * NPC.Dnpc().Times[4], NPC.Dnpc().Times[3], rectangle.Size() / 2, NPC.scale, sprite, 0f);
            }
            rectangle = NPC.frame;
            for (int a = 0; a < NPC.oldPos.Length; a++)
            {
                Vector2 vector = NPC.oldPos[a] + NPC.Size / 2;
                spriteBatch.Draw(texture, vector - screenPos, rectangle, color * 0.4f * (1 - (float)a / NPC.oldPos.Length), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, sprite, 0f);
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, sprite, 0f);
            spriteBatch.Draw(Glow.Value, NPC.Center - screenPos, rectangle, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, sprite, 0f);
            return false;
        }
    }
}