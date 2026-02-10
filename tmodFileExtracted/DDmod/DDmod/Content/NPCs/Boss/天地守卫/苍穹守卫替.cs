using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Boss;
using DDmod.DDOn;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.天地守卫
{
    [AutoloadBossHead]
    public class 苍穹守卫替 : ModNPC
    {
        public Asset<Texture2D> Glow => Boss.StarGuardBulan.StarGuard.Glow;
        public override void Load()
        { 
        }
        string Text = "";
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Life Guardian : Les");
           //DisplayName.AddTranslation(7, "生命守卫:莱斯");
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;

        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 5;
            NPC.defense = 12;
            NPC.knockBackResist = 0f;
            NPC.width = (int)(48 * NPC.scale);
            NPC.height = (int)(48 * NPC.scale);
            NPC.scale = 1f;
            NPC.value = Item.buyPrice(0, 5, 0, 0);
            NPC.npcSlots = 111f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            //if (!Main.dedServ) Music = MusicLoader.GetMusicSlot(Mod, "NoContent/Music/星心守卫");
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.Shatter;
            NPC.dontTakeDamage = true;
            if (NPCDowned.星心对话)
            {
                NPC.localAI[0] = 10;
            }
            if (NPCDowned.觉醒星心双子)
            {
                NPC.localAI[0] = 11;
            }
            NPC.Dnpc().Properties.Stone = true;
        }


        public override void FindFrame(int frameHeight)
        {
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override void BossLoot( ref int potionType)
        {
        }
        public bool title;
        public bool brothers;
        public bool RecognizeX;
        public bool RecognizeY;
        public int brothersTimer;
        public int brothersTimer2;
        public int 次数;
        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
        }
        int TL = 0;
        int TLTime = 0;
        public override void AI()
        {
            Lighting.AddLight(NPC.position, 0.5f, 1f, 2f);
            Player P = Main.player[NPC.target];
            //如果Boss没有目标,或者玩家距离较远,死亡刷新攻击目标
            //Main.LocalPlayer.Dplayer().Bossperspective(NPC.Center, 10, false,0.15F);
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || P.Distance(NPC.Center) > 300)
            {
                NPC.TargetClosest(true);
                if (P.Distance(NPC.Center) > 10000)
                {
                    NPC.timeLeft -= 10;
                }
            }
            NPC.damage = 0;
            NPC.rotation += NPC.velocity.X * 0.01F;
            if (NPC.velocity.X > 0)
            {
                NPC.rotation += Math.Abs(NPC.velocity.Y) * 0.01F;
            }
            else
            {
                NPC.rotation -= Math.Abs(NPC.velocity.Y) * 0.01F;
            }
            TLTime++;
            void T()
            {
                NPC.localAI[0]++;
                TLTime = 0;
            }

            if (NPC.localAI[0] <= 9)
            {
                if (!Main.dedServ) Music = DDSystem.Music(3, "星心对话");
            }
            if (NPC.localAI[0] <=0)
            {
                Text = Main.LocalPlayer.name;
                if (TLTime > 300)
                {
                    T();
                }
                Vector2 vector = P.Center - new Vector2(-300,300) - NPC.Center;
                float speed = vector.Length() / 20;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
            }
            else if (NPC.localAI[0] <= 3)
            {
                Vector2 vector = P.Center - new Vector2(-300, 300) - NPC.Center;
                float speed = vector.Length() / 20;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                if (TLTime > 600)
                {
                    T();
                }
                Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.ArousalDialogue" + NPC.localAI[0]);
                if (TLTime < 300)
                {
                    Text = "";
                }
            }
            else if (NPC.localAI[0] <= 8)
            {
                Vector2 vector = P.Center - new Vector2(-300, 300) - NPC.Center;
                float speed = vector.Length() / 20;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                if (TLTime > 300)
                {
                    T();
                }
                Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.ArousalDialogue" + NPC.localAI[0]);
            }
            else if (NPC.localAI[0] ==10)
            {
                Vector2 vector = P.Center - new Vector2(-300, 300) - NPC.Center;
                float speed = vector.Length() / 20;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                if (TLTime > 300)
                {
                    T();
                    NPC.localAI[0] = 9;
                }
                Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.StarGuard.ArousalDialogue" + 9);
            }
            else if (NPC.localAI[0] == 9)
            {
                if (!NPC.Dnpc().Bool[0])
                {
                    for (int r = 0; r < 5; r++)
                    {
                        for (int R = 0; R < 4; R++)
                        {
                            int a = NewDust(NPC.Center - new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), 0, 0, -100, new Color(0, 100, 255, 40), 0.1F + 0.3F);
                            Main.dust[a].customData = new Vector3(1, 20, 4 * R);
                            Main.dust[a].velocity = Vector2.Zero;
                            Main.dust[a].alpha = 0;
                        }
                    }
                    NewDustChange3(100, NPC.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 8, 30, true, 3, 8, 100, 1000, new Color(0, 100, 255, 40));

                    NPC.Dnpc().Bool[0] = true;
                    SoundStyle sound = DDHelper.SoundStyle(0, "变身");
                    sound.MaxInstances = 10;
                    sound.Pitch = -1;
                    PlaySound(sound,NPC.Center);
                    DNPC.NewNPCs(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, NPC.height / 2), ModContent.NPCType<苍穹守卫>(), 0);
                }
                else
                {
                    NPC.active = false;
                }
            }
            else
            {
                Vector2 vector = P.Center - new Vector2(-300, 300) - NPC.Center;
                float speed = vector.Length() / 20;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                if (TLTime > 120)
                {
                    NPC.localAI[0] = 9;
                }
            }
            if (Text != "")
            {
                NPC.NPCText(Text, 魔力);
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
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
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (NPC.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Npc[NPC.type];
            //绘制光效残影
            if (NPC.localAI[1] != 2)
            {
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    if (i != 0)
                    {
                        Vector2 vector2 = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2 - Main.screenPosition;
                        Color color = new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                        spriteBatch.Draw(Glow.Value, vector2, null, color * 0.5f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);
                    }
                }
            }
            Vector2 vector = new Vector2(texture.Width / 4, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            //绘制能量体大小
            spriteBatch.Draw(Glow.Value, NPC.Center - Main.screenPosition, null, new Color(0, 100 - NPC.alpha, 255 - NPC.alpha, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f, spriteEffects, 0f);
            spriteBatch.Draw(Glow.Value, NPC.Center - Main.screenPosition, null, new Color(255 - NPC.alpha, 155 - NPC.alpha, 0, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f / 2, spriteEffects, 0f);

            //如果没被击败就绘制贴图
            if (NPC.localAI[1] != 2) spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, new Rectangle?(NPC.life <= NPC.lifeMax / 2 ? new Rectangle(texture.Width / 2, NPC.frame.Y, texture.Width / 2, texture.Height) : new Rectangle(0, NPC.frame.Y, texture.Width / 2, texture.Height)), Color.White, NPC.rotation, vector, NPC.scale, spriteEffects, 0f);
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