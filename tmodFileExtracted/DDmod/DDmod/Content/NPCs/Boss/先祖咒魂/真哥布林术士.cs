using DDmod.Content.Dusts;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Projectiles.Boss.MiniBoss;
using DDmod.Content.Projectiles.Summon;
using DDmod.Worlds;
using static Terraria.GameContent.Bestiary.BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;
using DDmod.SubworldLibraryWorld;
using SubworldLibrary;
using DDmod.Content.Items.Boss.MiniBoss;
using Terraria.ModLoader.Config;
using DDmod.Content.Items.Magic.Staff.NPCLoot;
using System.Linq;
using Terraria.ModLoader.Utilities;
using DDmod.Content.Items.Accessory;
using Terraria;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Items.Boss.先祖咒魂;

namespace DDmod.Content.NPCs.Boss.先祖咒魂
{
    public class 真哥布林术士 : ModNPC
    {
        public override void SetStaticDefaults()
        {

            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 6250;
            NPC.damage = 50;
            NPC.defense = 34;
            NPC.knockBackResist = 0;
            NPC.width = 34;
            NPC.height = 46;
            NPC.value = Item.buyPrice(0, 14, 0, 0);
            NPC.npcSlots = 80f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit40;
            NPC.DeathSound = SoundID.NPCDeath42;
            NPC.netAlways = true;
            NPC.alpha = 0;
            NPC.boss = true;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPC.NPCHB().MiniBoss = true;
            Main.npcFrameCount[NPC.type] = 4;
            NPC.Dnpc().Properties.Meat = true;
            NPC.Dnpc().Properties.ShadowFire = true;
            NPC.Dnpc().Goblins = true;
            if (!Main.dedServ) Music = 39;
            NPC.Dnpc().Properties.BossLife = 1.25F;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return false;
        }
        public override void OnKill()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(
            [
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.真哥布林术士"))
            ]);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<被诅咒的替死玩偶>()));
        }
        public override bool CheckActive()
        {
            return true;
        }
        public override bool CheckDead()
        {
            return true;
        }
        SoundStyle sound => Main.rand.Next([SoundID.Zombie61, SoundID.Zombie62]);
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            Vector2 vector = player.Center - NPC.Center;
            if (player.dead)
            {
                if(NPC.timeLeft>5)
                {
                    NPC.timeLeft = 5;
                }
                NPC.SmoothVelocity(-vector.PerfectNormalize()*8);
                
            }
            int Direction = 1;
            bool NoV = NPC.localAI[2]-- > 0;
            if (NoV)
            {
                NPC.velocity *= 0.92F;
            }
            NPC.spriteDirection = 1;
            if (vector.X < 0)
            {
                NPC.spriteDirection = 0;
                Direction = -1;
            }
            if (NPC.ai[3] == 1)
            {
                NPC.Dnpc().Stage = 1;
            }
            if (NPC.Dnpc().Stage == 0)
            {
                if (Main.rand.NextBool(600))
                {
                    PlaySound(sound, NPC.Center);
                }
                if (NPC.ai[0] == 0)
                {
                    NPC.ai[1]++;

                    Vector2 vector2 = (player.Center - new Vector2(400 * Direction, 0).RotatedBy(NPC.ai[2])) - NPC.Center;
                    float Sp = vector2.Length() / 10;
                    if (Sp > 12)
                    {
                        Sp = 12;
                    }
                    if (!NoV)
                        NPC.velocity = (NPC.velocity * 20 + vector2.PerfectNormalize() * Sp) / 21;
                    if (NPC.ai[1] > 120 && NPC.ai[1] < 240 && NPC.ai[1] % 10 == 0)
                    {
                        NPC.velocity *= 0.92F;
                        if (NPC.ai[1] % 10 == 0)
                        {
                            NPC.NewNPCProj(NPC.Center, new Vector2(10, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<术士召唤术>(), 30, 0, -1, 0, 10, Main.rand.Next(30, 60));
                        }
                    }
                    if (NPC.ai[1] > 360 && NPC.ai[1] < 480 && NPC.ai[1] % 10 == 0)
                    {
                        if (NPC.ai[1] % 10 == 0)
                        {
                            NPC.NewNPCProj(NPC.Center, new Vector2(10, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<术士召唤术>(), 30, 0, -1, 0, 10, Main.rand.Next(30, 60));
                        }
                    }
                    if (NPC.ai[1] > 600 && NPC.ai[1] < 720)
                    {
                        if (NPC.ai[1] % 10 == 0)
                        {
                            NPC.NewNPCProj(NPC.Center, new Vector2(10, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<术士召唤术>(), 30, 0, -1, 0, 10, Main.rand.Next(30, 60));
                        }
                    }
                    if (NPC.ai[1] > 720)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[0]++;
                        Change();
                    }
                }
                else if (NPC.ai[0] == 1)
                {
                    NPC.ai[1]++;
                    Vector2 vector2 = (player.Center - new Vector2(400 * Direction, 0).RotatedBy(NPC.ai[2])) - NPC.Center;
                    float Sp = vector2.Length() / 10;
                    if (Sp > 12)
                    {
                        Sp = 12;
                    }
                    if (!NoV)
                        NPC.velocity = (NPC.velocity * 20 + vector2.PerfectNormalize() * Sp) / 21;
                    if (NPC.ai[1] % 60 == 0 && NPC.ai[1] <= 180)
                    {
                        for (int A = 0; A < 4; A++)
                        {
                            NPC.NewNPCProj(NPC.Center, new Vector2(0, 4).RotatedBy(MathHelper.TwoPi / 4 * A), ModContent.ProjectileType<混沌球>(), 30, 0, -1, Direction, 0.4F);
                        }

                    }
                    if (NPC.ai[1] > 720)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[0]++;
                        Change();
                    }
                }
                else if (NPC.ai[0] == 2)
                {
                    NPC.ai[1]++;
                    Vector2 vector2 = (player.Center - new Vector2(400 * Direction, 0).RotatedBy(NPC.ai[2])) - NPC.Center;
                    float Sp = vector2.Length() / 10;
                    if (Sp > 12)
                    {
                        Sp = 12;
                    }
                    if (!NoV)
                        NPC.velocity = (NPC.velocity * 20 + vector2.PerfectNormalize() * Sp) / 21;
                    if (NPC.ai[1] % 40 == 0 && NPC.ai[1] <= 120)
                    {
                        vector2 = player.Center + new Vector2(Main.rand.Next(200, 400), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));

                        NPC.NewNPCProj(vector2, Vector2.Zero, ModContent.ProjectileType<暗影触手基座>(), 30, 0, -1, 0, 0, Main.rand.NextFloat(MathHelper.TwoPi));
                    }
                    if (NPC.ai[1] > 160 && NPC.localAI[0] < 3)
                    {
                        NPC.ai[1] = 0;
                        NPC.localAI[0]++;
                    }
                    if (NPC.ai[1] > 360 && NPC.localAI[0] >= 3)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[0]++;
                        Change();
                    }
                }
                else
                {
                    NPC.localAI[0] = 0;
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                }
            }
            else
            {
                Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center,30,false,0.1F);
                if (NPC.velocity.Y == 0)
                {
                    NPC.ai[3]++;

                    NewDustChange2((int)(NPC.ai[3]/10), NPC.Center - new Vector2(4), Vector2.Zero, 27, 0, NPC.ai[3]/10, true, 0.3F, 2F, 0);
                    if (NPC.ai[3] > 120)
                    {
                        NPC.NewNPCProj(NPC.Center, new Vector2(0, -10), ModContent.ProjectileType<术士召唤术>(), 30, 0, -1, 100, 50, 30);
                        NPC.Kill();
                    }
                    SoundStyle sound = this.sound;
                    sound.MaxInstances = 10;
                    sound.Pitch = Main.rand.NextFloat(-1, 1);
                    PlaySound(sound, NPC.Center);
                }
                else
                {
                    if (NPC.velocity.Y < 10)
                    {
                        NPC.velocity.Y += 0.2F;
                    }
                    NPC.noTileCollide = false;
                }
                NPC.dontTakeDamage = true;
                NPC.velocity.X *= 0.92F;
            }
            Find();
            NPC.rotation = NPC.velocity.X * 0.03F;

            void Change()
            {
                NPC.ai[2] = Main.rand.NextFloat(MathHelper.PiOver2) - MathHelper.PiOver4;
            }
        }
        public void Find()
        {

            int dust = NewDust(NPC.position, NPC.width, NPC.height, 27, 0, 0, 100, default, Main.rand.NextFloat(0.2F, 0.8F));
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].noGravity = false;
            Lighting.AddLight(NPC.Center, new Vector3(84, 107, 221) * (0.001F));
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int a = 0; a <5; a++)
            {
                int A = NewDust(NPC.position, NPC.width, NPC.height, 27, 0, 0, 100, default, Main.rand.NextFloat(0.3F, 2F));
                Main.dust[A].velocity = new Vector2(Main.rand.NextFloat(8), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                Main.dust[A].noGravity = true;
                A = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 0, 100, default, Main.rand.NextFloat(0.3F, 2F));
                Main.dust[A].noGravity = true;
            }
            if (NPC.life <= 0)
            {
                NPC.life = 1;
                if (NPC.Dnpc().Stage == 0)
                {
                    NPC.ai[3] = 1;
                }
                else
                {

                    for (int num829 = 0; num829 < 50; num829++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * (float)hit.HitDirection, -2.5f);
                    }

                    Gore.NewGore(NPC.GetSource_FromAI(),NPC.position, NPC.velocity, 675, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromAI(),new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, 676, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromAI(),new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, 677, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromAI(),new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, 678, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromAI(),new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, 678, NPC.scale);
                }
                NPC.dontTakeDamage = true;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            if (NPC.IsABestiaryIconDummy)
            {
                NPC.rotation = MathHelper.Pi;
                for (int a = 0; a < NPC.oldPos.Length; a++)
                {
                    spriteBatch.Draw(texture, NPC.oldPos[a] + NPC.Size / 2 + new Vector2(0, 2) - screenPos, NPC.frame, new Color(84, 107, 221, 0) * ((NPC.oldPos.Length - a) / (float)NPC.oldPos.Length), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0);
                }
                spriteBatch.Draw(texture, NPC.Center + new Vector2(0, 2) - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0);

            }
            else
            {

                for (int a = 0; a < NPC.oldPos.Length; a++)
                {
                    spriteBatch.Draw(texture, NPC.oldPos[a] + NPC.Size / 2 + new Vector2(0, 2) - screenPos, NPC.frame, new Color(84, 107, 221, 0) * ((NPC.oldPos.Length - a) / (float)NPC.oldPos.Length), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0);
                }
                spriteBatch.Draw(texture, NPC.Center + new Vector2(0, 2) - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0);

            }
            //spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position - screenPos, null, Color.White*0.5F, 0, Vector2.Zero, NPC.Size/2, (SpriteEffects)NPC.spriteDirection, 0); 
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            if(NPC.velocity.Y==0&&!NPC.IsABestiaryIconDummy)
            {
                NPC.frame.Y = 0;
            }
            else
            {
                NPC.frameCounter++;
                NPC.frame.Y = ((int)(NPC.frameCounter / 5 % 3) + 1) * frameHeight;
            }
        }
    }
}