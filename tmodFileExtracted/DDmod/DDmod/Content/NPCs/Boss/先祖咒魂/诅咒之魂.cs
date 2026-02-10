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
using static System.Net.Mime.MediaTypeNames;
using DDmod.Content.Items.Melee.Sword;

namespace DDmod.Content.NPCs.Boss.先祖咒魂
{
    public class 诅咒之魂 : ModNPC
    {
        public override void SetStaticDefaults()
        {

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }
        public static Asset<Texture2D> Chains;
        public override void Load()
        {
            Chains = ModContent.Request<Texture2D>(Texture+"_Chains");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 2250;
            NPC.damage = 50;
            NPC.defense = 0;
            NPC.knockBackResist = 0;
            NPC.width = 40;
            NPC.height = 60;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.npcSlots = 80f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            SoundStyle sound = SoundID.NPCHit30;
            sound.Pitch = -0.5F;
            sound.Volume = 0.1F;
            NPC.HitSound = sound;
            sound = SoundID.NPCDeath33;
            sound.Pitch = -0.5F;
            sound.Volume = 0.1F;
            NPC.DeathSound = sound;
            NPC.netAlways = true;
            NPC.alpha = 0;
            NPC.dontTakeDamage = true;
            NPC.Dnpc().Properties.ShadowFire = true;
            NPC.Dnpc().Properties.Control = false;
            NPC.hide = true;
            NPC.Dnpc().Properties.BossLife = 1.25F;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public override bool? CanFallThroughPlatforms()
        {
            return false;
        }
        public override void OnKill()
        {
            Player closestPlayer = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)];

            for (int A = 0; A <3; A++)
            {
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.Heart);
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(
            [
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.诅咒之魂"))
            ]);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override bool CheckDead()
        {
            return true;
        }
        SoundStyle sound => Main.rand.Next([SoundID.Zombie61, SoundID.Zombie62]);
        public override void AI()
        {
            if (NPC.localAI[0] == 0)
            {
                NewDustChange4(50, NPC.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 9, true, 1F, 1.8F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 2);
                NPC.localAI[0] = 1;
            }
            if (NPC.ai[2] == 0)
            {
                NPC.dontTakeDamage = false;
                if (NPC.ai[1] < 1)
                {
                    NPC.ai[1] += 0.02F;
                }
                else
                {
                    NPC.ai[0]++;
                    Player player = Main.LocalPlayer;
                    Vector2 vector = player.Center - NPC.Center;
                    if (NPC.ai[0] > 600)
                    {
                        player.FixedDamage(NPC.damage, PlayerDeathReason.ByNPC(NPC.whoAmI));
                        NewDustChange4(100, player.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 4);
                        NPC.ai[0] = 0;
                    }
                    else
                    {
                        NewDustSector3(1, player.Center - new Vector2(8), new Vector2(4), new Vector3(-vector.PerfectNormalize(), MathHelper.Pi), ModContent.DustType<速度粒子>(), 2, 10, true, 1, 3, 100, 1000, new Color(151, 35, 221, 0), null, Main.myPlayer);
                    }
                    if (!Collision.CanHitLine(NPC.position - new Vector2(0, 40), 1, 1, NPC.position + new Vector2(0, NPC.height), 1, 1))
                    {
                        NPC.velocity = new Vector2(0, -1);
                    }
                    else
                    {
                        NPC.velocity *= 0.98F;
                    }
                    NPC.ai[1] = 1;
                }
            }
            else
            {
                NPC npc = Main.npc[(int)NPC.ai[2] - 1];

                if (NPC.ai[1] < 1)
                {
                    NPC.ai[1] += 0.02F;
                }
                else
                {
                    NPC.ai[0]++;
                    Vector2 vector = npc.Center - NPC.Center;
                    if (NPC.ai[0] > 600)
                    {
                        NewDustChange4(100, npc.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 18, true, 1F, 2.4F, 0, 0, new Color(151, 35, 221, 0), new Dust().DustAI(1) + 4);
                        NPC.ai[0] = 0;
                        NPC.Kill(false);
                    }
                    else
                    {
                        NewDustSector(1, npc.Center - new Vector2(8), new Vector2(4), new Vector3(-vector.PerfectNormalize(), MathHelper.Pi), ModContent.DustType<速度粒子>(), 2, 10, true, 1, 3, 100, 1000, new Color(151, 35, 221, 0), null);
                    }
                    NPC.ai[1] = 1;
                }
            }
        }
        public void Find()
        {
            int dust = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(151, 35, 221, 0), Main.rand.NextFloat(0.3F, 2.8F));
            Main.dust[dust].velocity = new Vector2(0, -Main.rand.NextFloat(3, 6));
            Main.dust[dust].customData = 2;
            Main.dust[dust].noGravity = true;
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int a = 0; a < 5; a++)
            {
                int dust = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(151, 35, 221, 0), Main.rand.NextFloat(0.3F, 2.8F));
                Main.dust[dust].velocity = new Vector2(0, -Main.rand.NextFloat(3, 6));
                Main.dust[dust].customData = 2;
                Main.dust[dust].noGravity = true;
            }
            if (NPC.life <= 0)
            {

                for (int num829 = 0; num829 < 50; num829++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<光球粒子>(), 2.5f * (float)hit.HitDirection, -2.5f, 0, new Color(151, 35, 221, 0));
                }
            }
        }
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCsOverPlayers.Add(index);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects sprite = 0;
            Texture2D texture = Chains.Value;
            if (NPC.IsABestiaryIconDummy)
            {
                spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position - screenPos - new Vector2(100, 50), null, new Color(50, 12, 74, 255), 0, Vector2.Zero, new Vector2(200, 100), 0, 0f);
            }
            else
            {
                Vector2 vector = Main.LocalPlayer.Center - NPC.Center;
                if (NPC.ai[2] > 0)
                {
                    vector = Main.npc[(int)NPC.ai[2] - 1].Center - NPC.Center;
                }
                if (vector.X > 0)
                {
                    sprite = SpriteEffects.FlipHorizontally;
                }
                if ((NPC.ai[0] / 600) < 0.85F)
                {
                    DDHelper.BackAndForth(1f, 1.5f, (NPC.ai[0] / 600) / 20, ref NPC.Dnpc().Times[0], ref NPC.Dnpc().Bool[0], true);
                }
                else
                {
                    DDHelper.BackAndForth(1f, 1.5f, (NPC.ai[0] / 600) / 4, ref NPC.Dnpc().Times[0], ref NPC.Dnpc().Bool[0], true);
                }
                spriteBatch.Draw(DDTextures.VoidStar.Value, NPC.Center + vector * (NPC.ai[0] / 600) - screenPos, null, new Color(100, 100, 100, 255), 0, DDTextures.VoidStar.Size() / 2, 0.4F * NPC.Dnpc().Times[0], 0, 0);
                spriteBatch.Draw(DDTextures.VoidStar.Value, NPC.Center + vector * (NPC.ai[0] / 600) - screenPos, null, new Color(151, 35, 221, 0), 0, DDTextures.VoidStar.Size() / 2, 0.4F * NPC.Dnpc().Times[0], 0, 0);
                for (int a = 0; a < vector.Length() / texture.Width * NPC.ai[1]; a++)
                {
                    spriteBatch.Draw(texture, NPC.Center + vector.PerfectNormalize() * texture.Width * a - screenPos, null, new Color(151, 35, 221, 0) * 0.3F, vector.ToRotation(), texture.Size() / 2, NPC.scale, 0, 0);
                }
                if (NPC.ai[1] >= 1)
                {
                    if (NPC.ai[2] > 0)
                    {

                        spriteBatch.Draw(DDTextures.VoidStar.Value, Main.npc[(int)NPC.ai[2] - 1].Center - screenPos, null, new Color(100, 100, 100, 255), 0, DDTextures.VoidStar.Size() / 2, 0.4F, 0, 0);
                        spriteBatch.Draw(DDTextures.VoidStar.Value, Main.npc[(int)NPC.ai[2] - 1].Center - screenPos, null, new Color(151, 35, 221, 0), 0, DDTextures.VoidStar.Size() / 2, 0.4F, 0, 0);
                    }
                    else
                    {

                        spriteBatch.Draw(DDTextures.VoidStar.Value, Main.LocalPlayer.Center - screenPos, null, new Color(100, 100, 100, 255), 0, DDTextures.VoidStar.Size() / 2, 0.4F, 0, 0);
                        spriteBatch.Draw(DDTextures.VoidStar.Value, Main.LocalPlayer.Center - screenPos, null, new Color(151, 35, 221, 0), 0, DDTextures.VoidStar.Size() / 2, 0.4F, 0, 0);
                    }
                }
            }
            texture = TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.Center + new Vector2(0, 2) - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, sprite, 0);

            //spriteBatch.Draw(DDTextures.WhitePng.Value, NPC.position - screenPos, null, Color.White*0.5F, 0, Vector2.Zero, NPC.Size/2, (SpriteEffects)NPC.spriteDirection, 0); 
            return false;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            NPC.frame.Y = ((int)(NPC.frameCounter / 5 % 5)) * frameHeight;
        }
    }
}