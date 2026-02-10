using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;

namespace DDmod.Content.Projectiles.Pet.MasterPet
{
    public class Devil_Slime : ModProjectile
    {

        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Devil Slime");
           //DisplayName.AddTranslation(7, "Devil Slime");
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            Main.projFrames[Projectile.type] = 11;
            Projectile.netImportant = true;
            Projectile.width = 46;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.scale = 1;
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.frame = reader.ReadInt32();
            SJ = reader.ReadInt32();
            FL = reader.ReadBoolean();
            JX = reader.ReadInt32();
            Texts = reader.ReadInt32();
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.frame);
            writer.Write(SJ);
            writer.Write(FL);
            writer.Write(JX);
            writer.Write(Texts);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/MasterPet/Devil_Slime");
            //lightColor.A = 150;
            Main.EntitySpriteDraw(texture, Projectile.position + new Vector2(Projectile.width / 2, Projectile.height + 2) - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] - 2), Projectile.scale, (SpriteEffects)(Projectile.spriteDirection == 0 ? 0 : 1), 0);

            //texture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/MasterPet/Devil_Slime_E");
            //lightColor.A = 255;
            //Main.EntitySpriteDraw(texture, Projectile.position + new Vector2(Projectile.width / 2, Projectile.height + 2) - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] - 2), Projectile.scale, (SpriteEffects)(Projectile.spriteDirection == 0 ? 0 : 1), 0);

            texture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/MasterPet/Devil_Slime_Glow");

            /*if (Projectile.ai[1] != 1)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition;
                    Color color = new Color(132, 0, 255, 150) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture, vector2 - new Vector2(0, 2), new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), color, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, (SpriteEffects)(Projectile.spriteDirection == 0 ? 0 : 1), 0f);
                }
            }*/
            Main.EntitySpriteDraw(texture, Projectile.position+new Vector2(Projectile.width/2,Projectile.height+2) - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type])), Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type]-2), Projectile.scale, (SpriteEffects)(Projectile.spriteDirection == 0 ? 0 : 1), 0);
            //Main.EntitySpriteDraw(DDTextures.WhitePng.Value, Projectile.position- Main.screenPosition, null, Color.White*0.5F, Projectile.rotation,Vector2.Zero, Projectile.Size / 2, (SpriteEffects)(Projectile.spriteDirection == 0 ? 0 : 1), 0);
        }
        //开场白
        public bool HB;
        //起飞绘制
        public float T1 = 0;
        public float T2 = 0.33f;
        public float T3 = 0.66f;
        public float H2 = 1;
        //跳跃
        int dt;
        bool dtq;

        //睡觉
        int SJ;
        //愤怒
        bool FL;
        //惊醒
        int JX;
        //说话冷却
        int Texts;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] == 1)
            {
                T1 += 0.033f;
                T2 += 0.033f;
                T3 += 0.033f;
                if (T1 > 1f) T1 = 0;
                if (T2 > 1f) T2 = 0;
                if (T3 > 1f) T3 = 0;
                for (int a = 0; a < 3; a++)
                {
                    Vector2 projDirection = Utils.RotatedBy(new Vector2(0, Projectile.height / 2), Projectile.rotation, default);
                    Main.EntitySpriteDraw(DDTextures.Circle[0].Value, Projectile.Center + projDirection - Main.screenPosition, null, new Color(132, 0, 255, 150) * (1 - T1), Projectile.rotation, DDTextures.Circle[0].Size() / 2, new Vector2(Projectile.scale * 1.1f, 0.15f) * T1, 0, 0);
                    Main.EntitySpriteDraw(DDTextures.Circle[0].Value, Projectile.Center + projDirection - Main.screenPosition, null, new Color(132, 0, 255, 150) * (1 - T2), Projectile.rotation, DDTextures.Circle[0].Size() / 2, new Vector2(Projectile.scale * 1.1f + 0.01F, 0.17f) * T2, 0, 0);
                    Main.EntitySpriteDraw(DDTextures.Circle[0].Value, Projectile.Center + projDirection - Main.screenPosition, null, new Color(132, 0, 255, 150) * (1 - T3), Projectile.rotation, DDTextures.Circle[0].Size() / 2, new Vector2(Projectile.scale * 1.1f + 0.02F, 0.19f) * T3, 0, 0);
                }
            }
            return false;
        }
        public override void AI()
        {
            if(Texts>0)
            {
                Texts--;
            }
            Lighting.AddLight(Projectile.Center - new Vector2(0, 4), new Color(210,55,25).ToVector3()*0.5F);
            Player player = Main.player[Projectile.owner];
            CheckActive(player);

            Vector2 vector = (player.Center - new Vector2(100 * player.direction, 0)) - Projectile.Center;
            Vector2 vector2 = vector;

            if (Projectile.frameCounter == 0)
            {
                Projectile.frame = 0;
            }
            else if (Projectile.velocity.Y < 0)
            {
                Projectile.frame = 1;
            }
            else if (Projectile.velocity.Y > 0)
            {
                Projectile.frame = 3;
            }
            else if (Projectile.frameCounter > 4)
            {
                Projectile.frame = 4;
            }
            else
            {
                Projectile.frame = 5;
            }
            Projectile.tileCollide = (Projectile.ai[1] == 0 && Projectile.velocity.Y > 0) || Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, player.position, player.width, player.height) && player.Center.Y - Projectile.Center.Y < 100;
            Projectile.rotation = 0;
            if (Projectile.ai[1] == 1 || Projectile.velocity == Vector2.Zero)
            {
                Projectile.ai[0]++;
                if (Projectile.ai[0] < 16)
                {
                    Projectile.frame = 4;
                }
                else if (Projectile.ai[0] < 24)
                {
                    Projectile.frame = 0;
                }
                else
                {
                    Projectile.frame = 1;
                }
                if (Projectile.ai[0] >= 32) Projectile.ai[0] = 16;
            }
            else
            {
                Projectile.ai[0] = 0;
            }
            if (Projectile.velocity == Vector2.Zero)
            {
                if(Projectile.DProj().Times[0]>0)
                {
                    Projectile.frame = 7;
                    if(FL)
                    {
                        Projectile.frame = 8;
                    }
                }
                if (Projectile.owner == Main.myPlayer && (Projectile.Center - Main.MouseWorld).Length() < 20 && player.controlUseTile&& Projectile.DProj().Times[0]<-120)
                {
                    Projectile.DProj().Times[0]=60;
                    string text;
                    if (SJ >= 300)
                    {
                        text = Utils.SelectRandom(Main.rand, new string[]
                        {
                      "别在这理发店",
                      "烦捏"
                        });
                        FL = true;
                    }
                    else
                    {
                        text = Utils.SelectRandom(Main.rand, new string[]
                        {
                      "啊呜~",
                      "咩嘿嘿~"
                        });
                        for (int A = 0; A < 5; A++)
                        {
                            int Dust = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<爱心粒子>());
                            Main.dust[Dust].velocity = new Vector2(0, -2).RotatedBy(Main.rand.NextFloat(-0.4F, 0.4F));
                        }
                    }
                    SJ = 0;
                    CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 150) , text, false ,false);
                    Texts = 300;
                    Projectile.netUpdate = true;
                }
                else
                {
                    Projectile.DProj().Times[0]--;
                    if(Projectile.DProj().Times[0]==0)
                    {
                        Projectile.frame = 0;
                        FL = false;
                        Projectile.netUpdate = true;
                    }
                }
                SJ++;
                if (SJ == 300 && Projectile.frame != 7)
                {
                    string text = Utils.SelectRandom(Main.rand, new string[]
                    {
                      "没什么事的话我先睡一觉了",
                      "眠了眠了",
                      "好困..睡大觉"
                    });
                    CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 150) , text, false ,false);
                    Texts = 300;
                }
                if (SJ >= 300 && Projectile.frame != 7)
                {
                    Projectile.frame = 6;
                    if (SJ % 300 == 0)
                    {
                        CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 0) * 0.1f, "ZZZ..", false ,false);
                        Texts = 120;
                    }
                }
            }
            else
            {
                if (Projectile.ai[1] == 0)
                {
                    if (SJ >= 300)
                    {
                        string text = Utils.SelectRandom(Main.rand, new string[]
                        {
                         "嘿！你想去哪？",
                         "....好想睡觉",
                         "能让我再睡会吗?"
                        });
                        CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 150) , text, false ,false);
                        Texts = 300;
                    }
                    SJ = 0;
                }
                else if (SJ >= 300)
                {
                    SJ++;
                    Projectile.frame = 6;
                    if (SJ % 100 == 0)
                    {
                        CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 0) * 0.1f, "ZZZ..", false ,false);
                        Texts = 120;
                    }
                }
            }
            if (JX > 0)
            {
                JX--;
                SJ = 0;
                if (JX > 80)
                {
                    Projectile.frame = 9;
                }
                else  if (JX < 40)
                {
                    Projectile.frame = 10;
                }
            }
            if (Projectile.ai[1] == 1)
            {
                if (player.dead || !player.HasBuff(ModContent.BuffType<Devil_SlimeBuff>())) return;
                vector = player.Center - new Vector2(0, Projectile.height / 2 + player.height / 2 + 50) - Projectile.Center;
                vector2 = vector;
                if (vector != Vector2.Zero) vector.Normalize();
                float sp = vector2.Length() / 10;
                if (sp > 15) sp = 15;
                if (sp < 0.5F) sp = 0;
                vector *= sp;
                Projectile.velocity = vector;
                if (Projectile.velocity.X > 0)
                {
                    Projectile.spriteDirection = 0;
                }
                else
                {
                    Projectile.spriteDirection = -1;
                }
                if (player.velocity.Y == 0 && Projectile.ai[1] == 1)
                {
                    for (int a = 0; a < 7; a++)
                    {
                        if (Main.tile[(int)((player.Center.X - 100 * player.direction) / 16), (int)((player.Center.Y + Projectile.height) / 16) + a].HasTile)
                        {
                            Projectile.ai[1] = 0;
                        }
                    }
                }
                Projectile.rotation = Projectile.velocity.X * 0.03f;
            }

            float Center = (player.Center.X - 100 * player.direction) - Projectile.Center.X;

            if (Center < 0) Center = -Center;

            if (vector != Vector2.Zero) vector.Normalize();

            if (((vector2.X > 50 || vector2.X < -50) || (vector2.Y > 140 || vector2.Y < -140))&&((player.Center-Projectile.Center).Length()>150|| (player.Center - Projectile.Center).Length() <50))
            {
                if (Projectile.velocity.Y == 0 && Projectile.velocity.X == 0 && Projectile.ai[0] > 16)
                {
                    Projectile.velocity.Y = -8 - dt;
                    if (Center > 30)
                    {
                        Projectile.velocity.X = vector.X  * (4+ (Center/30));
                    }
                }
            }

            if (Projectile.velocity.Y != 0)
            {
                Projectile.frameCounter = 8;
                if (Projectile.ai[1] == 0)
                {
                    Projectile.velocity.Y += 0.4f;
                }
                if (Projectile.velocity.X > 0)
                {
                    Projectile.spriteDirection = 0;
                }
                else
                {
                    Projectile.spriteDirection = -1;
                }
            }
            else
            {
                if (Projectile.ai[0] < 8)
                {
                    Projectile.frame = 4;
                }
                else if (Projectile.ai[0] < 16)
                {
                    Projectile.frame = 5;
                }
                if ((vector2.X > 50 || vector2.X < -50 || vector2.Y > 140 || vector2.Y < -140) && Projectile.velocity.X == 0 && ((player.Center - Projectile.Center).Length() > 150 || (player.Center - Projectile.Center).Length() < 50))
                {
                    if (Projectile.ai[0] > 16)
                    {
                        dt++;
                    }
                }
                else
                {
                    Projectile.velocity.X = 0;
                    dt = 0;
                }
                if (Projectile.frameCounter > 0)
                {
                    Projectile.frameCounter--;
                }
                if (player.Center.X > Projectile.Center.X)
                {
                    Projectile.spriteDirection = 0;
                }
                else
                {
                    Projectile.spriteDirection = -1;
                }
            }

            if (Center <= 50)
            {
                Projectile.velocity.X *= 0.9f;
            }
            if (vector2.Length() > 500 || vector2.Y < -140)
            {
                Projectile.ai[1] = 1;
            }
            if (vector2.Length() > 2000)
            {
                Projectile.Center = player.Center;
            }
            //Projectile.rotation = velocity5.ToRotation() + (float)Math.PI / 2f;
        }
        private void CheckActive(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Devil_SlimeBuff>()))
            {
                Projectile.timeLeft = 2;
            }
            else if (!player.dead)
            {
                Projectile.timeLeft = 2;
                Projectile.ai[1] = 1;
                Projectile.velocity.Y -= 0.2f;
                Projectile.localAI[0]++;
                if (Projectile.localAI[0] == 1)
                {
                    if (Projectile.localAI[1] < 4)
                    {
                        CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 150), "拜拜了您捏", false, false);
                        Texts = 300;
                    }
                    Projectile.localAI[1]++;
                }
                if (Projectile.localAI[0] > 120)
                {
                    Projectile.active = false;
                }
                if (Projectile.localAI[1] >= 5)
                {
                    if (!player.dead)
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.DDmod.PlayerKill.Kill2", player.name)), 1000000, 0);
                        CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 150), "你个大傻逼！", false, false);
                        Texts = 300;
                    }
                }
            }
            if (player.dead)
            {
                Projectile.timeLeft = 2;
                Projectile.ai[1] = 1;
                if (!player.dead)
                {
                    Projectile.velocity.Y -= 0.2f;
                }
                else if (Projectile.localAI[0] > 60)
                {
                    Projectile.velocity.Y -= 0.2f;
                }
                Projectile.localAI[0]++;
                if (Projectile.localAI[0] > (player.dead ? 180 : 120))
                {
                    Projectile.active = false;
                }
            }
            else if (player.HasBuff(ModContent.BuffType<Devil_SlimeBuff>()))
            {
                Projectile.timeLeft = 2;
                if (Projectile.localAI[0] > 0)
                {
                    CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 150), "你什么问题？", false, false);
                    Texts = 300;
                    Projectile.localAI[0] = 0;
                }
            }
            if (!HB)
            {
                CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 150), "我TM莱纳!", false, false);
                Texts = 300;
                HB = true;
            }
            if (BossCD2 > 0)
            {
                BossCD2--;
            }
            if (BossCD > 0)
            {
                BossCD--;
            }
            for (int b = 0; b < Boss.Length; b++)
            {
                if (!player.dead && BossCD == 0)
                {
                    if (Main.npc[b].boss && Main.npc[b].active)
                    {
                        SJ = 0;
                        if (!Boss[b])
                        {
                            CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 25), Main.npc[b].FullName + "你就是歌姬吧!", false, false);
                            Texts = 300;
                            BossCD = 300;
                            Boss[b] = true;
                        }
                    }
                }
                if (Boss[b] && BossCD2 == 0)
                {
                    if (player.dead && Main.npc[b].active)
                    {
                        string text = "土地," + Main.npc[b].FullName + "我跟你没完你等着熬";
                        CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 25), text, false, false);
                        Texts = 300;
                        BossCD2 = 300;
                        Boss[b] = false;

                    }
                    else if (Main.npc[b].boss && !Main.npc[b].active)
                    {
                        NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -5), ModContent.ProjectileType<Devil_SlimeFireworks>(), 0, 0, Projectile.owner);
                        CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 25), Main.npc[b].FullName + "这个弟中之弟", false, false);
                        Texts = 300;
                        BossCD2 = 300;
                        Boss[b] = false;
                    }
                    else if (!Main.npc[b].boss)
                    {
                        Boss[b] = false;
                    }
                }
            }
            if (!player.dead && Main.rand.NextBool(1000)&& Texts<=0)
            {
                string text;
                if (SJ < 300)
                {
                    text = Utils.SelectRandom(Main.rand, new string[]
                    {
                    "我是个傻逼",
                    "......",
                    "寄汤来咯",
                    "SlimeMod...已经是过去了..",
                    "我...貌似是全国产模组最菜吧",
                    "感谢游玩本模组OvO",
                    "笙箫有点怕怕",
                    "夏夏~",
                    "我爱夏夏!",
                    "夏夏是夏璃夜!",
                    });
                }
                else
                {

                    text = Utils.SelectRandom(Main.rand, new string[]
                    {
                    "什么时候才能火呢?",
                    "woc,我火了!",
                    });
                }
                CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 150), text, false, false);
                Texts = 300;
                if (text == "woc,我火了!")
                {
                    JX = 120;
                }
            }
            if (JX == 80)
            {
                CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 150), "。。。", false, false);
                Texts = 300;
            }
            if (JX == 40)
            {
                CombatText.NewText(new Rectangle((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1), new Color(132, 0, 255, 150), "原来是梦啊。。。", false, false);
                Texts = 300;
            }
        }
        bool[] Boss = new bool[200];
        //Boss弹窗CD
        int BossCD;
        //玩家死亡BossCD
        int BossCD2;
        private bool Movement(Player player)
        {
            return true;
        }

        private void Animate(bool movesFast)
        {
        }
    }
}
