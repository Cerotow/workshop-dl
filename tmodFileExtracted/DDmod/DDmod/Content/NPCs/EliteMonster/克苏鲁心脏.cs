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
using Microsoft.Xna.Framework.Graphics;
using DDmod.Content.Items.Ranged.NPCLoot;
using static AssGen.Assets;

namespace DDmod.Content.NPCs.EliteMonster
{
    public class 克苏鲁心脏 : ModNPC
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
            NPC.lifeMax = 6750;
            NPC.damage = 40;
            NPC.defense = 20;
            NPC.knockBackResist = 0;
            NPC.width = 92;
            NPC.height = 92;
            NPC.value = Item.buyPrice(0, 14, 0, 0);
            NPC.npcSlots = 80f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.netAlways = true;
            NPC.alpha = 0;
            NPC.boss = true;
            NPC.NPCHB().MiniBoss = true;
            Main.npcFrameCount[NPC.type] = 6;
            NPC.Dnpc().Properties.Meat = true;
            NPC.Dnpc().Properties.BossLife = 1.2f;
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
                Biomes.UndergroundCrimson,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.克苏鲁心脏"))
            ]);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            DropOneByOne.Parameters parameters = default(DropOneByOne.Parameters);
            parameters.MinimumItemDropsCount = 6;
            parameters.MaximumItemDropsCount = 9;
            parameters.ChanceNumerator = 1;
            parameters.ChanceDenominator = 1;
            parameters.MinimumStackPerChunkBase = 2;
            parameters.MaximumStackPerChunkBase = 4;
            parameters.BonusMinDropsPerChunkPerPlayer = 1;
            parameters.BonusMaxDropsPerChunkPerPlayer = 2;
            DropOneByOne.Parameters parameters2 = parameters;
            DropOneByOne.Parameters parameters3 = parameters2;
            DropOneByOne.Parameters parameters4 = parameters3;
            parameters3.BonusMinDropsPerChunkPerPlayer =2;
            parameters3.BonusMaxDropsPerChunkPerPlayer = 3;
            parameters4.BonusMinDropsPerChunkPerPlayer = 3;
            parameters4.BonusMaxDropsPerChunkPerPlayer = 4;
            npcLoot.Add(new DropBasedOnCompleteMode( new DropOneByOne(521, parameters2), new DropOneByOne(521, parameters3), new DropOneByOne(521, parameters4)));

            parameters2.MinimumItemDropsCount = 10;
            parameters2.MaximumItemDropsCount = 12;
            parameters3.MinimumItemDropsCount = 10;
            parameters3.MaximumItemDropsCount = 12;
            parameters4.MinimumItemDropsCount = 10;
            parameters4.MaximumItemDropsCount = 12;
            npcLoot.Add(new DropBasedOnCompleteMode( new DropOneByOne(1332, parameters2), new DropOneByOne(1332, parameters3), new DropOneByOne(1332, parameters4)));
        }
        public override bool CheckActive()
        {
            return true;
        }
        public override bool CheckDead()
        {
            return true;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.RotationSpeed(NPC.velocity.X * 0.06F,0.04F);
            Vector2 vector = player.Center - NPC.Center;
            DDHelper.BackAndForth(-1, 1, 0.05F, ref NPC.ai[3], ref NPC.Dnpc().Bool[0]);
            NPC.velocity.Y = NPC.velocity.Y * 0.98f+NPC.ai[3] / 10;
            if (player.dead)
            {
                if (NPC.timeLeft > 5)
                {
                    NPC.timeLeft = 5;
                }
                NPC.spriteDirection = 0;
                if (NPC.velocity.X < 0)
                {
                    NPC.spriteDirection = 1;
                }
                NPC.SmoothVelocity(-vector.PerfectNormalize() * 8);
                return;
            }
            if (NPC.Dnpc().Stage == 0)
            {
                    NPC.localAI[2] =15;
                NPC.spriteDirection = 0;
                if (NPC.ai[0]++%120==0)
                {
                    CombatText.NewText(new Rectangle((int)NPC.Center.X,(int)NPC.Center.Y-10,1,1),new Color(155,0,0,255),"ZZZ",false,false);
                }
                NPC.boss = false;
                if (!Main.dedServ) Music = -1;
                if (NPC.life<NPC.lifeMax)
                {
                    NPC.Dnpc().Stage = 1;
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.Dnpc().Stage == 1)
            {
                NPC.boss = true;
                if (!Main.dedServ) Music = DDSystem.MiniBossMusic;
                int W = 14;
                if (NPC.localAI[1] ==0)
                {
                    NPC.NewNPCProj(NPC.Center, new Vector2(1, 0), ModContent.ProjectileType<猩红触手>(), 22, 0, -1, NPC.whoAmI + 1,6, 1.75F);
                    NPC.NewNPCProj(NPC.Center, new Vector2(-1, 0), ModContent.ProjectileType<猩红触手>(), 22, 0, -1, NPC.whoAmI + 1, 6,1.75F);
                }
                if (NPC.localAI[1] == W)
                {
                    NPC.NewNPCProj(NPC.Center, new Vector2(1, 0), ModContent.ProjectileType<猩红触手>(), 22, 0, -1, NPC.whoAmI + 1,5, 1.75F);
                    NPC.NewNPCProj(NPC.Center, new Vector2(-1, 0), ModContent.ProjectileType<猩红触手>(), 22, 0, -1, NPC.whoAmI + 1, 5,1.75F);
                }
                if (NPC.localAI[1] == W*2)
                {
                    NPC.NewNPCProj(NPC.Center, new Vector2(1, 0), ModContent.ProjectileType<猩红触手>(), 22, 0, -1, NPC.whoAmI + 1,3, 1.75F);
                    NPC.NewNPCProj(NPC.Center, new Vector2(-1, 0), ModContent.ProjectileType<猩红触手>(), 22, 0, -1, NPC.whoAmI + 1, 3,1.75F);
                }
                if (NPC.localAI[1] == W*3)
                {
                    NPC.NewNPCProj(NPC.Center, new Vector2(1, 0), ModContent.ProjectileType<猩红触手>(), 22, 0, -1, NPC.whoAmI + 1,4, 1.75F);
                    NPC.NewNPCProj(NPC.Center, new Vector2(-1, 0), ModContent.ProjectileType<猩红触手>(), 22, 0, -1, NPC.whoAmI + 1, 4,1.75F);
                }
                if (NPC.localAI[1] == W*4)
                {
                    NPC.NewNPCProj(NPC.Center, new Vector2(1, 0), ModContent.ProjectileType<猩红触手>(), 22, 0, -1, NPC.whoAmI + 1,3, 1.75F);
                    NPC.NewNPCProj(NPC.Center, new Vector2(-1, 0), ModContent.ProjectileType<猩红触手>(),22, 0, -1, NPC.whoAmI + 1, 3,1.75F);
                }
                NPC.localAI[1]++;
                if (NPC.localAI[2]++ > 120)
                {
                    NPC.localAI[2] = 0;
                }
                int Direction = 1;
                NPC.spriteDirection = 0;
                if (vector.X < 0)
                {
                    NPC.spriteDirection = 1;
                    Direction = -1;
                }
                Vector2 Center = NPC.Center - new Vector2(-14 * Direction, 8).RotatedBy(NPC.rotation);
                vector = player.Center - Center;
                if (NPC.ai[0]++ < 300)
                {
                    NPC.SmoothVelocity(vector.PerfectNormalize() * 5);
                    if (NPC.ai[0] % 60 == 0)
                    {
                        NPC.NewNPCProj(Center, vector.PerfectNormalize() *6, ModContent.ProjectileType<神血>(), 28, 0);
                        Eze();
                    }
                }
                else if (NPC.ai[0] < 500)
                {
                    NPC.velocity *= 0.92F;
                    if (NPC.ai[0] % 15 == 0)
                    {
                        NPC.NewNPCProj(Center, vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F)) * Main.rand.NextFloat(4F, 8F), ModContent.ProjectileType<神血>(), 28, 0);
                        Eze();
                    }

                }
                else if (NPC.ai[0] < 800)
                {
                    if (NPC.ai[0] % 90 == 0)
                    {
                        NPC.velocity = vector.PerfectNormalize() * 16;
                    }
                    if (NPC.ai[0] % 90 > 45)
                    {
                        NPC.velocity *= 0.92F;
                    }
                }
                else
                {

                    NPC.ai[0] = 0;
                }
                Find();
                void Eze()
                {

                    if (NPC.localAI[2] >= 41)
                    {
                        NPC.localAI[2] -= 5;
                    }
                    else
                    {
                        NPC.localAI[2] = 36;
                    }
                }
            }
        }
        public void Find()
        {
            int dust = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 0, 100, default, Main.rand.NextFloat(0.2F, 0.8F));
            Main.dust[dust].velocity = new Vector2(0, Main.rand.NextFloat(0.2F, 0.8F));
            Main.dust[dust].noGravity = false;

            int Direction = 1;
            if (NPC.spriteDirection ==1)
            {
                NPC.spriteDirection = 1;
                Direction = -1;
            }
            dust = NewDust(NPC.Center - new Vector2(2* Direction, 66).RotatedBy(NPC.rotation) * NPC.scale-new Vector2(10), 12, 12, 5, 0, 0, 100, default, Main.rand.NextFloat(0.5F, 1.2F));
            Main.dust[dust].velocity = new Vector2(2 * Direction, -4).RotatedBy(NPC.rotation) * Main.rand.NextFloat(0.6F, 1F) + NPC.velocity;
            Main.dust[dust].noGravity = false;
            dust = NewDust(NPC.Center - new Vector2(-26 * Direction, 56).RotatedBy(NPC.rotation) * NPC.scale - new Vector2(10), 12, 12, 5, 0, 0, 100, default, Main.rand.NextFloat(0.75F, 1.4F));
            Main.dust[dust].velocity = new Vector2(4 * Direction, -2).RotatedBy(NPC.rotation) * Main.rand.NextFloat(0.6F, 1F)+NPC.velocity;
            Main.dust[dust].noGravity = false;
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int a = 0; a <5; a++)
            {
                int A = NewDust(NPC.position, NPC.width, NPC.height, 5, 0, 0, 100, default, Main.rand.NextFloat(0.3F, 2F));
                Main.dust[A].noGravity = true;
            }
            if (NPC.life <= 0)
            {
                //NPC.life = 1;
                {

                    for (int num829 = 0; num829 < 350; num829++)
                    {
                        Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, 2.5f * (float)hit.HitDirection, -2.5f,0,default, Main.rand.NextFloat(1F, 2.8F));
                    }
                    /*

                    Gore.NewGore(NPC.GetSource_FromAI(),NPC.position, NPC.velocity, 675, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromAI(),new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, 676, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromAI(),new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, 677, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromAI(),new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, 678, NPC.scale);
                    Gore.NewGore(NPC.GetSource_FromAI(),new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, 678, NPC.scale);*/
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            if (NPC.IsABestiaryIconDummy)
            {
                NPC.spriteDirection = 1;
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0);

            }
            else
            {
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0);
                Rectangle rectangle = NPC.frame;
                rectangle.X = NPC.frame.Width;
                if(NPC.localAI[2]<36)
                {
                    rectangle.Y = ((int)(NPC.localAI[2] / 5 % 6)) * NPC.frame.Height;
                }
                else
                {
                    rectangle.Y = 0;
                }
                spriteBatch.Draw(texture, NPC.Center - screenPos, rectangle, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, (SpriteEffects)NPC.spriteDirection, 0);


            }
            //spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position - screenPos, null, Color.White*0.5F, 0, Vector2.Zero, NPC.Size/2, (SpriteEffects)NPC.spriteDirection, 0); 
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Width = 112;
            NPC.frame.X = 0;
            NPC.frameCounter++;
            NPC.frame.Y = ((int)(NPC.frameCounter / 5 % 6)) * frameHeight;
        }
    }
}