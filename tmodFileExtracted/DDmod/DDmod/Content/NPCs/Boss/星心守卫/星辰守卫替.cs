using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Projectiles.Boss;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using Terraria;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.Content.NPCs.Boss.星心守卫
{
    [AutoloadBossHead]
    public class 星辰守卫替 : ModNPC
    {
        public  Asset<Texture2D> Glow => Boss.StarGuardBulan.StarGuard.Glow;
        public override void Load()
        {
        }
        string Text = "";
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Mana Guardian: Bulan");
           //DisplayName.AddTranslation(7, "星辰守卫:布兰");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 5;
            NPC.knockBackResist = 0f;
            NPC.width = (int)(48 * NPC.scale);
            NPC.height = (int)(48 * NPC.scale);
            NPC.scale = 1.3f;
            NPC.value = Item.buyPrice(0, 3, 0, 0);
            NPC.npcSlots = 111f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            if (!Main.dedServ) Music = DDSystem.Music(3, "星心对话");
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.Shatter;
            NPC.dontTakeDamage = true;
            NPC.localAI[0] = 0;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.66f * (1 + (numPlayers - 1) * 0.15f) * balance) * bossAdjustment);;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override void OnKill()
        {
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        Vector2 PlayerC;
        int TLTime = 0;
        public override void AI()
        {
            NPC.rotation += 0.03F;
            Lighting.AddLight(NPC.position, 0.5f, 2f, 2f);
            Player P = Main.player[NPC.target];
            //如果Boss没有目标,或者玩家距离较远,死亡刷新攻击目标
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || P.Distance(NPC.Center) > 300)
            {
                NPC.TargetClosest(true);
                if (P.Distance(NPC.Center) > 10000)
                {
                    NPC.timeLeft -= 10;
                }
            }
            NPC.damage = 0;

            TLTime++;
            void T()
            {
                NPC.localAI[0]++;
                TLTime = 0;
            }
            if (NPC.localAI[0] <= 0)
            {
                Text = Main.LocalPlayer.name;
                if (TLTime > 300)
                {
                    T();
                }
                Vector2 vector = P.Center - new Vector2(-300, 300) - NPC.Center;
                float speed = vector.Length() / 20;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
            }
            else
            if (NPC.localAI[0] <= 3)
            {
                Vector2 vector = P.Center - new Vector2(-300,300) - NPC.Center;
                float speed = vector.Length() / 20;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                if (TLTime > 600)
                {
                    T();
                }
                if (NPC.localAI[0] == 1)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue14");
                }
                if (NPC.localAI[0] == 2)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue15");
                }
                if (NPC.localAI[0] == 3)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue16");
                }
                if (TLTime < 300)
                {
                    Text = "";
                }
            }
            else
            {
                NPC npc = Main.npc[0];
                if (NPC.FindFirstNPC(ModContent.NPCType<生命守卫替>()) != -1)
                {
                    npc = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<生命守卫替>())];
                }
                Vector2 vector = npc.Center - NPC.Center;
                if (NPC.localAI[0] > 6)
                {
                    Text = "";
                    if (npc == null || npc.type != ModContent.NPCType<生命守卫替>())
                    {
                        NPC.active = false;
                    }
                    float speed = vector.Length() / 20;
                    if (speed > 10)
                    {
                        speed = 10;
                    }
                    NPC.velocity = vector.PerfectNormalize() * speed;
                }
                else
                {
                    vector = P.Center - new Vector2(-300,300) - NPC.Center;
                    float speed = vector.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                }

                if (TLTime > 180)
                {
                    T();
                }
                if (NPC.localAI[0] == 4)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue17");
                }
                if (NPC.localAI[0] == 5)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue18");
                }
                if (NPC.localAI[0] == 6)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.Dialogue19");
                }
            }
            if (Text != "")
            {
                NPC.NPCText(Text, 魔力);
            }
        }
        public override void HitEffect(HitInfo hit)
        {
            for (int i = 0; i < 2; i++)
            {
                NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<星星粒子>(), hit.HitDirection, -1f, 0, new Color(155, 155, 155, 0), 1f);
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<星星粒子>(), hit.HitDirection, -1f, 0, new Color(155, 155, 155, 0), 1f);
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
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (item.DamageType != DamageClass.Magic)
            {
                modifiers.SourceDamage *= 0.66f;
            }
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (projectile.DamageType != DamageClass.Magic)
            {
                modifiers.SourceDamage *= 0.66f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = (Texture2D)TextureAssets.Npc[NPC.type];
            Texture2D glow = Glow.Value;
            //绘制光效残影
            if (NPC.localAI[1] != 2)
            {
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    Vector2 vector2 = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2 - Main.screenPosition;
                    Color color = new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                    spriteBatch.Draw(glow, vector2, null, color, NPC.rotation, glow.Size() / 2, NPC.scale * 1.15f  * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);

                }
            }
            Vector2 vector = new Vector2(texture.Width / 4, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            //绘制能量体大小
            spriteBatch.Draw(glow, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - Main.screenPosition, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, glow.Size() / 2, NPC.scale * 1.15f , spriteEffects, 0f);
            spriteBatch.Draw(glow, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - Main.screenPosition, null, new Color(255 - NPC.alpha, 155 - NPC.alpha, 0, 0) * 1f, NPC.rotation, glow.Size() / 2, NPC.scale * 1.15f  / 2, spriteEffects, 0f);

            spriteBatch.Draw(texture, NPC.position + new Vector2(NPC.width, NPC.height) / 2 - Main.screenPosition, new Rectangle?(((float)NPC.life / NPC.lifeMax <= 0.5F) ? new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height) : new Rectangle(0, 0, texture.Width / 2, texture.Height)), Color.White, NPC.rotation, vector, NPC.scale, spriteEffects, 0f);

            return false;
        }
    }
}