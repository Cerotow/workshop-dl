using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Projectiles.Boss;
using DDmod.DDOn;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using Terraria;

namespace DDmod.Content.NPCs.Boss.星心守卫
{
    [AutoloadBossHead]
    public class 生命守卫替 : ModNPC
    {
        public Asset<Texture2D> Glow => Boss.LifeGuardLes.LifeGuard.Glow;
        public override void Load()
        { 
        }
        string Text = "";
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Life Guardian : Les");
           //DisplayName.AddTranslation(7, "生命守卫:莱斯");
            Main.npcFrameCount[NPC.type] = 5;
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
            if (!Main.dedServ) Music = DDSystem.Music(3, "星心对话");
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.Shatter;
            NPC.dontTakeDamage = true;
            NPC.localAI[0] = 0;
        }


        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.1;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override void BossLoot(ref int potionType)
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
            Lighting.AddLight(NPC.position, 2f, 0.5f, 0.5f);
            Player P = Main.player[NPC.target];
            //如果Boss没有目标,或者玩家距离较远,死亡刷新攻击目标
            Main.LocalPlayer.Dplayer().Bossperspective(P.Center - new Vector2(0, 300), 10, false,0.15F);
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || P.Distance(NPC.Center) > 300)
            {
                NPC.TargetClosest(true);
                if (P.Distance(NPC.Center) > 10000)
                {
                    NPC.timeLeft -= 10;
                }
            }
            NPC.damage = 0;
            NPC.rotation = NPC.velocity.X * 0.05f;
            TLTime++;
            void T()
            {
                NPC.localAI[0]++;
                TLTime = 0;
            }

            if (NPC.localAI[0] <=0)
            {
                Text = Main.LocalPlayer.name;
                if (TLTime > 300)
                {
                    T();
                }
                Vector2 vector = P.Center - new Vector2(300) - NPC.Center;
                float speed = vector.Length() / 20;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
            }
            else if (NPC.localAI[0] <= 3)
            {
                Vector2 vector = P.Center - new Vector2(300) - NPC.Center;
                float speed = vector.Length() / 20;
                NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                if (TLTime > 600)
                {
                    T();
                }
                if (NPC.localAI[0] == 1)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue14");
                }
                if (NPC.localAI[0] == 2)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue15");
                }
                if (NPC.localAI[0] == 3)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue16");
                }
                if (TLTime >= 300)
                {
                    Text = "";
                }
            }
            else
            {
                NPC npc = Main.npc[0];
                if(NPC.FindFirstNPC(ModContent.NPCType<星辰守卫替>())!=-1)
                {
                    npc = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<星辰守卫替>())];
                }
                Vector2 vector = npc.Center - NPC.Center;
                if (NPC.localAI[0] > 6)
                {
                    Text = "";
                    if (npc == null || npc.type != ModContent.NPCType<星辰守卫替>())
                    {
                        NPC.active = false;
                    }
                    if (npc.getRect().Intersects(NPC.getRect()))
                    {
                        if (!NPC.Dnpc().Bool[0])
                        {
                            for (int R = 1; R <= 3; R++)
                            {
                                int a = NewDust((NPC.Center + npc.Center) / 2 - new Vector2(4), 1, 1, ModContent.DustType<光芒粒子>(), 0, 0, -1000, new Color(255, 0, 0, 40), 0.1F + 0.3F * R);
                                Main.dust[a].customData = new Vector3(9, 6, 0);
                                Main.dust[a].velocity = Vector2.Zero;
                                Main.dust[a].alpha = -300;

                                int b = NewDust((NPC.Center + npc.Center) / 2 - new Vector2(4), 1, 1, ModContent.DustType<光芒粒子>(), 0, 0, -1000, new Color(0, 100, 255, 40), 0.1F + 0.3F * R);
                                Main.dust[b].customData = new Vector3(9, 6, 0);
                                Main.dust[b].velocity = Vector2.Zero;
                                Main.dust[b].rotation = 1;
                                Main.dust[b].alpha = -300;
                            }
                            NPC.Dnpc().Bool[0] = true;
                            NPC npc2 = new NPC();
                            npc2.SetDefaults(ModContent.NPCType<星心守卫>());
                            DNPC.NewNPCs(NPC.GetSource_FromAI(), (NPC.Center + npc.Center) /2+new Vector2(0,30), ModContent.NPCType<星心守卫>(), 0);
                        }
                        else
                        {
                            NPC.active = false;
                        }
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
                    vector = P.Center - new Vector2(300) - NPC.Center;
                    float speed = vector.Length() / 20;
                    NPC.velocity = (NPC.velocity * 20 + vector.PerfectNormalize() * speed) / 21;
                }

                if (TLTime > 180)
                {
                    T();
                }
                if (NPC.localAI[0] == 4)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue17");
                }
                if (NPC.localAI[0] == 5)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue18");
                }
                if (NPC.localAI[0] == 6)
                {
                    Text = Language.GetTextValue("Mods.DDmod.NPCDialogue.LifeGuard.Dialogue19");
                }

            }
            if (Text != "")
            {
                NPC.NPCText(Text, 生命);
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
                        Vector2 vector2 = NPC.oldPos[i] + NPC.Size/2- Main.screenPosition;
                        Color color = new Color(255 - NPC.alpha, 50 - NPC.alpha, 50 - NPC.alpha, 0) * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                        spriteBatch.Draw(Glow.Value, vector2, null, color * 0.5f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f * ((NPC.oldPos.Length - i) / (float)NPC.oldPos.Length), spriteEffects, 0f);
                    }
                }
            }
            Vector2 vector = new Vector2(texture.Width / 4, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            //绘制能量体大小
            spriteBatch.Draw(Glow.Value, NPC.Center - Main.screenPosition, null, new Color(255 - NPC.alpha, 50 - NPC.alpha, 50 - NPC.alpha, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f, spriteEffects, 0f);
            spriteBatch.Draw(Glow.Value, NPC.Center - Main.screenPosition, null, new Color(0, 105 - NPC.alpha, 105 - NPC.alpha, 0) * 1f, NPC.rotation, Glow.Size() / 2, NPC.scale * 1.15f/ 2, spriteEffects, 0f);

            //如果没被击败就绘制贴图
            if (NPC.localAI[1] != 2) spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, new Rectangle?(NPC.life <= NPC.lifeMax / 2 ? new Rectangle(texture.Width / 2, NPC.frame.Y, texture.Width / 2, texture.Height / 5) : new Rectangle(0, NPC.frame.Y, texture.Width / 2, texture.Height / 5)), Color.White, NPC.rotation, vector, NPC.scale, spriteEffects, 0f);
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