using DDmod.Content.Dusts;
using DDmod.Worlds;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace DDmod.Content.NPCs.Boss.LifeGuardLes
{
    public class LifeWarrior : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Life Warrior");
            //DisplayName.AddTranslation(7, "大地勇士");
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
        }
        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPC.aiStyle = -1;
            float Strengthen = 1 + (NPCDowned.downedLifeGuard ? 1 : 0) + (NPCDowned.downedLifeGuard2 ? 1 : 0);
            if (Main.expertMode)
            {
                Strengthen *= 1.25F;
            }
            if (Main.masterMode)
            {
                Strengthen *= 1.25F;
            }
            NPC.lifeMax = (int)(30 * Strengthen);
            NPC.damage = (int)(12 * Strengthen);
            NPC.defense = (int)(2 * Strengthen);

            NPC.Dnpc().LifeUP = false;

            NPC.knockBackResist = 1f;
            NPC.width = 38;
            NPC.height = 36;
            NPC.value = Item.buyPrice(0, 0, 0, 50);
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.Shatter;
            NPC.buffImmune[31] = true;
            NPC.netAlways = true;
            NPC.Dnpc().Properties.Stone = true;
            if (NPCDowned.downedLifeGuard)
            {
                NPC.Dnpc().Properties.BossLife = 1.05F;
            }
            if (NPCDowned.downedLifeGuard2)
            {
                NPC.Dnpc().Properties.BossLife = 1.1F;
            }

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,

                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.DDmod.NPCBestiary.LifeWarrior"))
            });
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.pick > 0)
            {

                modifiers.SourceDamage *= item.pick / 10;
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void AI()
        {
            //普普通通的冲刺形仆从
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = 10;
                NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<爱心粒子>(), 0f, 0f, 0, new Color(255, 0, 0, 0), 1f);
            }
            Lighting.AddLight(NPC.position, 1f, 0.3f, 0.3f);
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();
            NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
            Vector2 vector = Vector2.Subtract(player.Center, NPC.Center);
            if (vector != Vector2.Zero) vector.Normalize();

            NPC.ai[0]++;
            if (NPC.ai[0] < 60)
            {
                NPC.velocity *= 0.96F;
                int d = NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<爱心粒子>(), 0f, 0f, 0, new Color(255, 0, 0, 0), 1f);
                GlobalDust.DustNPCOwner[d] = NPC.whoAmI;
                NPC.localAI[2] = NPC.ai[0]/60F;
            }
            else if (NPC.ai[0] == 60)
            {
                NPC.velocity = vector * 16;

            }
            else
            {
                NPC.velocity = (NPC.velocity * 40 + vector * 8) / 41;
                NPC.localAI[2] = 1-(NPC.ai[0]-60) / 120F;
                if (NPC.ai[0] > 180)
                {
                    NPC.ai[0] = 0;
                }
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 2; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, DustID.HeartCrystal, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, DustID.HeartCrystal, hit.HitDirection, -1f, 0, default, 1f);
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
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;

            spriteBatch.Draw(texture, NPC.Center - screenPos, null, Color.White, NPC.rotation, texture.Size()/2, NPC.scale, 0, 0f);
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                spriteBatch.Draw(texture, NPC.oldPos[i] +NPC.Size/2 - screenPos, null, new Color(255,0,0,0)* NPC.localAI[2] * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), NPC.rotation, texture.Size() / 2, NPC.scale, 0, 0f);

            }

                return false;
        }
    }
}