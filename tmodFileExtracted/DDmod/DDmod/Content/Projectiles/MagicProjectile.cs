using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Magic.Book;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Content.Projectiles.Melee.Sword;
using ReLogic.Text;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;

namespace DDmod.Content.Projectiles
{
    public class MagicProjectile : GlobalProjectile
    {
        public static Asset<Texture2D> MagicDaggerGlow;
        public bool[] NPCW = new bool[200];
        public static Asset<Texture2D> 鬼魂;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                MagicDaggerGlow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Magic/Staff/MagicDagger2");
                Proj_316_Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Proj_316_Glow");
                鬼魂 = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/鬼魂");
            }
        }
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == 294)
            {
                projectile.extraUpdates = 300;
                projectile.timeLeft = 300;
            }
                //魔刺
            if (projectile.type == 7)
            {
                NPCHit(projectile, 40);
            }
                //魔晶刺
            if (projectile.type == 493||projectile.type == 494)
            {
                NPCHit(projectile, 40);
            }
            //绿激光
            if (projectile.type == 20)
            {
                NPCHit(projectile, 30);
            }
            //水箭
            if (projectile.type == 27)
            {
                NPCHit(projectile, 10);
            }
            //魔法飞刀
            if (projectile.type == 93)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            }
            //魔法导弹
            if (projectile.type == 16)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 50;
                NPCHit(projectile, 20);
            }
            //恶魔锄刀
            if (projectile.type == 44 || projectile.type == 45)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
                NPCHit(projectile, 30);
            }
            //诅咒火焰
            if (projectile.type == 95 || projectile.type == 101)
            {
                NPCHit(projectile, 30);
            }
            //灵液
            if (projectile.type == 280)
            {
                NPCHit(projectile, 30);
            }
            //骷髅头
            if (projectile.type == 837)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
                NPCHit(projectile, 30);
            }
            //磁球
            if (projectile.type == 254)
            {
                projectile.scale *= 1.5f;
            }
            if (projectile.type == 255)
            {
                projectile.timeLeft = 400;
                projectile.extraUpdates = 40;
                projectile.DProj().Times[0] = 1;
                projectile.timeLeft *= 50;
            }
            //利刃台风
            if (projectile.type == 409)
            {
                NPCHit(projectile, 20);
            }
            //水
            if (projectile.type == 22)
            {
                NPCHit(projectile, 20);
                projectile.width = projectile.height = 26;
                projectile.extraUpdates = 5;
            }
            //雨云
            if (projectile.type == 239)
            {
                NPCHit(projectile, 20);
            }
            //邪恶三叉戟
            if (projectile.type == 114)
            {
                NPCHit(projectile, 20);
            }
            //亡魂
            if (projectile.type == 297)
            {
                projectile.width = projectile.height = 24;
                NPCHit(projectile, 20);
                projectile.penetrate = 5;
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
            }
            //蝙蝠
            if (projectile.type == 316)
            {
                projectile.penetrate = 3;
                NPCHit(projectile, 30);
            }
            //高温射线
            if (projectile.type == 260)
            {
                NPCHit(projectile, 20);
                projectile.penetrate = -1;
            }
            //高温射线
            if (projectile.type == 645)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
                projectile.extraUpdates = 2;
                projectile.penetrate = 1;
            }
        }
        public void NPCHit(Projectile projectile, int Hit)
        {
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = Hit;
            projectile.usesIDStaticNPCImmunity = false;
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.type == 79)
            {
                if(!projectile.Player().controlUseItem || projectile.ai[1] != 0 || projectile.Player().controlUseTile)
                {
                    projectile.Kill();
                }
            }
            if (projectile.type == 645)
            {
                projectile.Kill();
            }
                return base.OnTileCollide(projectile, oldVelocity);

        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
        }
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == 522)
            {
                projectile.velocity *= 0.92F;
                if (projectile.velocity.Length() < 1F)
                {
                    projectile.Kill();
                }
                for (float A = 0; A < 2; A += 1f)
                {
                    Dust dust = Main.dust[Dust.NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(248, 80, 228, 50))];
                    dust.noGravity = true;
                    dust.scale = 0.6F;
                    dust.velocity = -projectile.velocity / 10;
                    dust.customData = -1;
                }
                return false;

            }
            if (projectile.type == 521)
            {
                if (Collision.CanHitLine(projectile.DProj().vector[0], 1, 1, projectile.position, projectile.width, projectile.height) || Collision.CanHitLine(projectile.Player().position, projectile.Player().width, projectile.Player().height, projectile.position, projectile.width, projectile.height))
                {
                    projectile.DProj().Bool[0] = true;
                }
                projectile.tileCollide = projectile.DProj().Bool[0];

                for (float A = 0; A < 4; A += 1f)
                {
                    Dust dust = Main.dust[Dust.NewDust(projectile.Center - new Vector2(4) - (projectile.velocity / 4 * A), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(248, 80, 228, 50))];
                    dust.noGravity = true;
                    dust.scale = 1F;
                    dust.velocity = -projectile.velocity / 10;
                    dust.customData = -3;
                }
                projectile.Track(200, 10, 12, 5);
                return false;
            }
            //高温射线
            if (projectile.type == 260)
            {
                if (projectile.ai[1] > 0)
                {
                    projectile.scale = 0.4F;
                    if (projectile.timeLeft > 30)
                    {
                        projectile.timeLeft = 30;
                    }
                }
                projectile.ProjScaleChange();
                projectile.localAI[0] += 1f;
                if (projectile.ai[1] == 0)
                {
                    if (projectile.localAI[0] == 5)
                    {
                        for (float A = 0; A < 40; A += 1f)
                        {
                            Vector2 vector = new Vector2(0, 20).RotatedBy(MathHelper.TwoPi / 40 * A);
                            vector *= new Vector2(0.4F, 1);
                            vector = vector.RotatedBy(projectile.velocity.ToRotation());
                            Dust dust = Main.dust[Dust.NewDust(projectile.Center - new Vector2(4) + vector, 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(255, 181, 12, 0))];
                            dust.noGravity = true;
                            dust.scale = 0.3F;
                            dust.customData = dust.scale;
                        }
                    }
                    if (projectile.localAI[0] == 7)
                    {
                        for (float A = 0; A < 40; A += 1f)
                        {
                            Vector2 vector = new Vector2(0, 30).RotatedBy(MathHelper.TwoPi / 40 * A);
                            vector *= new Vector2(0.4F, 1);
                            vector = vector.RotatedBy(projectile.velocity.ToRotation());
                            Dust dust = Main.dust[Dust.NewDust(projectile.Center - new Vector2(4) + vector, 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(255, 181, 12, 0))];
                            dust.noGravity = true;
                            dust.scale = 0.6F;
                            dust.customData = dust.scale;
                        }
                    }
                    if (projectile.localAI[0] == 10)
                    {
                        for (float A = 0; A < 40; A += 1f)
                        {
                            Vector2 vector = new Vector2(0, 40).RotatedBy(MathHelper.TwoPi / 40 * A);
                            vector *= new Vector2(0.4F, 1);
                            vector = vector.RotatedBy(projectile.velocity.ToRotation());
                            Dust dust = Main.dust[Dust.NewDust(projectile.Center - new Vector2(4) + vector, 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(255, 181, 12, 0))];
                            dust.noGravity = true;
                            dust.scale = 1F;
                            dust.customData = dust.scale;
                        }
                    }
                }
                if (projectile.localAI[0] > 3f)
                {
                    Dust dust = Main.dust[Dust.NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<激光粒子>(), 0, 0, 100, new Color(255, 181, 20, 155))];
                    dust.noGravity = true;
                    dust.scale = projectile.scale;
                    dust.velocity = Vector2.Zero;
                    dust.rotation = projectile.velocity.ToRotation();
                    dust.customData = -dust.scale;
                }
                {
                    Dust dust = Main.dust[Dust.NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(255, 181, 0, 0))];
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(1, 3) * projectile.scale;
                    dust.velocity = new Vector2(Main.rand.NextFloat(1, 4) * projectile.scale, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                    dust.rotation = dust.velocity.ToRotation();
                    dust.customData = dust.scale;
                }
                return false;
            }
            //蝙蝠
            if (projectile.type == 316)
            {
                projectile.DProj().Times[0]++;
                if (projectile.DProj().Times[0] > 120)
                {
                    projectile.DProj().Times[0] = 0;
                }
                if (!projectile.DProj().Bool[0])
                {
                    projectile.scale = Main.rand.NextFloat(0.5F, 1.5F);
                    for (int A = 0; A < 5; A++)
                    {
                        int num454 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 195);
                        Main.dust[num454].scale = 0.85f * projectile.scale;
                        Main.dust[num454].noGravity = true;
                        Dust dust = Main.dust[num454];
                        dust.velocity += projectile.velocity * 0.5f;
                    }
                    for (int A = 0; A < 20; A++)
                    {
                        int num454 = Dust.NewDust(projectile.Center + new Vector2(4), 1, 1, 195);
                        Dust dust = Main.dust[num454];
                        dust.scale = 0.85f * projectile.scale;
                        dust.noGravity = true;
                        dust.velocity = new Vector2(Main.rand.NextFloat(10), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                    }
                    projectile.ProjScaleChange();
                    projectile.DProj().Bool[0] = true;
                }
                projectile.alpha = 0;
                NPC npc = NPCdirection.FindClosest(projectile.Center, 500, false);
                if (npc == null && projectile.DProj().Bool[1])
                {
                    if (Collision.SolidTiles(projectile.position, projectile.width, projectile.height))
                    {
                        projectile.Kill();
                    }
                    projectile.frameCounter++;
                    if (projectile.frameCounter >= 3)
                    {
                        projectile.frame++;
                        projectile.frameCounter = 0;
                    }

                    if (projectile.frame >= 3)
                        projectile.frame = 0;
                    projectile.position += projectile.Player().velocity;
                    projectile.spriteDirection = 0;
                    if (projectile.Player().velocity.X > 0)
                    {
                        projectile.spriteDirection = -1;
                    }
                    projectile.Track(500, 0, 15);
                    return false;
                }
                else
                {
                    projectile.DProj().Bool[1] = false;
                }
                if (npc == null)
                {
                    projectile.velocity *= 0.98F;
                    projectile.frameCounter++;
                    if (projectile.frameCounter >= 3)
                    {
                        projectile.frame++;
                        projectile.frameCounter = 0;
                    }

                    if (projectile.frame >= 3)
                        projectile.frame = 0;
                    projectile.spriteDirection = 0;
                    if (projectile.Player().velocity.X > 0)
                    {
                        projectile.spriteDirection = -1;
                    }
                    return false;
                }
            }
            //裂天剑
            if (projectile.type == 660)
            {
                if (projectile.alpha > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item9, projectile.Center);
                    projectile.alpha = 0;
                    projectile.scale = Main.rand.NextFloat(0.8F, 1.3F);
                    projectile.frame = Main.rand.Next(14);
                    projectile.netUpdate = true;
                }
                projectile.ai[1]++;
                projectile.tileCollide = projectile.ai[1] > 10;
                if (!Main.tile[(int)projectile.Center.X / 16, (int)projectile.Center.Y / 16].HasTile)
                {
                    projectile.ai[1] = 11;
                }
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
                return false;
            }
            //邪恶三叉戟
            if (projectile.type == 114)
            {
                if (projectile.alpha > 0)
                {
                    PlaySound(SoundID.Item8, projectile.Center);
                    projectile.alpha = 0;
                    projectile.netUpdate = true;
                }
                projectile.ai[2] -= projectile.velocity.Length();
                projectile.tileCollide = projectile.ai[2] < 0;
                projectile.ai[0]++;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
                return false;
            }
            //利刃台风
            if (projectile.type == 409)
            {
                if (projectile.DProj().track < 30)
                {
                    projectile.localAI[1]++;
                    if (projectile.localAI[1] > 10f && Main.rand.NextBool(3))
                    {
                        for (int D = 0; D < 6; D++)
                        {
                            Vector2 spinningpoint4 = Vector2.Normalize(projectile.velocity) * new Vector2(projectile.width, projectile.height) / 2f;
                            spinningpoint4 = spinningpoint4.RotatedBy((D - (6 / 2 - 1)) * Math.PI / 6) + projectile.Center;
                            Vector2 value19 = ((float)(Main.rand.NextDouble() * MathHelper.Pi) - (float)Math.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
                            Dust dust = Main.dust[NewDust(spinningpoint4 + value19, 0, 0, 217, value19.X * 2f, value19.Y * 2f, 100, default(Color), 1.4f)];
                            dust.noGravity = true;
                            dust.noLight = true;
                            dust.velocity /= 4f;
                            dust.velocity -= projectile.velocity;
                        }

                        projectile.alpha -= 5;
                        if (projectile.alpha < 50)
                            projectile.alpha = 50;

                        projectile.rotation += projectile.velocity.X * 0.1f;
                        projectile.frame = (int)(projectile.localAI[1] / 3f) % 3;
                        Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.1f, 0.4f, 0.6f);
                    }
                    return false;
                }
            }
            //血雨云
            if (projectile.type == 244)
            {
                /*
                bool target = false;
                NPC npc = projectile.FindTargetWithinRange(500);
                DDHelper.BackAndForth(30, 80, 0.2f, ref projectile.DProj().Times[0], ref projectile.DProj().Bool[0]);
                projectile.netUpdate = true;
                if (npc != null && npc.CanBeChasedBy(projectile, false))
                {
                    Vector2 vector4 = Vector2.Subtract(npc.Center - new Vector2(0, npc.height / 2 + projectile.DProj().Times[0]), projectile.Center);
                    vector4.X += npc.velocity.X * 3;
                    float Speed = vector4.Length() / 50;
                    DDHelper.MaxandMinF(ref Speed, 1, 12);
                    vector4.Normalize();
                    vector4 *= Speed;
                    projectile.velocity = (projectile.velocity * 9 + vector4) / 10;
                    target = true;
                }
                if (!target)
                {
                    projectile.velocity = Vector2.Zero;
                }*/
                bool flag18 = true;
                int num352 = (int)projectile.Center.X;
                int num353 = (int)(projectile.position.Y + (float)projectile.height);
                if (Collision.SolidTiles(new Vector2(num352, num353), 2, 20))
                    flag18 = false;

                projectile.frameCounter++;
                if (projectile.frameCounter > 8)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if ((!flag18 && projectile.frame > 2) || projectile.frame > 5)
                        projectile.frame = 0;
                }

                projectile.ai[1] += 1f;
                if (projectile.ai[1] >= 1800f)
                {
                    projectile.alpha += 5;
                    if (projectile.alpha > 255)
                    {
                        projectile.alpha = 255;
                        projectile.Kill();
                    }
                }
                else if (flag18)
                {
                    projectile.ai[0] += 1f;
                    if (projectile.ai[0] > 10f)
                    {
                        projectile.ai[0] = 0f;
                        if (projectile.owner == Main.myPlayer)
                        {
                            num352 += Main.rand.Next(-14, 15);
                            NewProjectile(projectile.GetSource_FromAI(), num352, num353, 0f, 5f, ModContent.ProjectileType<血雨>(), projectile.damage, 0f, projectile.owner);
                        }
                    }
                }

                projectile.localAI[0] += 1f;
                if (!(projectile.localAI[0] >= 10f))
                    return false;

                projectile.localAI[0] = 0f;
                //判定血云消失

                int num354 = 0;
                int num355 = 0;
                float num356 = 0f;
                for (int num358 = 0; num358 < 1000; num358++)
                {
                    if (Main.projectile[num358].active && Main.projectile[num358].owner == projectile.owner && Main.projectile[num358].type == projectile.type && Main.projectile[num358].ai[1] < 1800f)
                    {
                        num354++;
                        if (Main.projectile[num358].ai[1] > num356)
                        {
                            num355 = num358;
                            num356 = Main.projectile[num358].ai[1];
                        }
                    }
                }
                if (num354 > 3)
                {
                    Main.projectile[num355].netUpdate = true;
                    Main.projectile[num355].ai[1] = 1800f;
                }
                return false;
            }
            //雷雨云
            if (projectile.type == 238)
            {
                bool target = false;
                NPC npc = projectile.FindTargetWithinRange(500);
                DDHelper.BackAndForth(100, 200, 0.2f, ref projectile.DProj().Times[0], ref projectile.DProj().Bool[0]);
                projectile.netUpdate = true;
                if (npc != null && npc.CanBeChasedBy(projectile, false) && projectile.DProj().Times[1] < 20)
                {
                    Vector2 vector4 = Vector2.Subtract(npc.Center - new Vector2(0, npc.height / 2 + projectile.DProj().Times[0]), projectile.Center);
                    vector4.X += npc.velocity.X * 3;
                    float Speed = vector4.Length() / 50;
                    DDHelper.MaxandMinF(ref Speed, 1, 12);
                    vector4.Normalize();
                    vector4 *= Speed;
                    projectile.velocity = (projectile.velocity * 9 + vector4) / 10;
                    target = true;
                }
                if (!target)
                {
                    projectile.velocity = Vector2.Zero;
                }
                projectile.DProj().Times[1]--;
                if (projectile.DProj().Times[1] <= 0)
                {
                    if (projectile.owner == Main.myPlayer)
                    {
                        for (int W = -1; W <= 1; W++)
                        {
                            Vector2 vector = new Vector2(projectile.Center.X + Main.rand.NextFloat(0, 20) * W, projectile.Center.Y + 4);
                            int A = NewProjectile(projectile.GetSource_FromAI(), vector, ((vector - projectile.Center) * new Vector2(0.25F, 1)).PerfectNormalize() * 10, ModContent.ProjectileType<闪电>(), projectile.damage, 1f, projectile.owner, 0, 1F, 120);
                            Main.projectile[A].DamageType = DamageClass.Magic;
                        }
                    }
                    projectile.DProj().Times[1] = 100;
                }
                return true;
            }
            //骷髅头
            if (projectile.type == 837)
            {
                projectile.spriteDirection = projectile.direction;
                if (projectile.direction < 0)
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
                else
                    projectile.rotation = projectile.velocity.ToRotation();
                NPC npc = NPCdirection.FindClosest(projectile.Center, 500, false);
                projectile.alpha = 0;
                if (npc != null && npc.CanBeChasedBy(projectile, false) && projectile.DProj().track > 80)
                {
                    Vector2 vector4 = Vector2.Subtract(npc.Center, projectile.Center);
                    if (projectile.ai[0] == 0)
                    {
                        projectile.ai[0] = 1;
                        SoundStyle sound = SoundID.Item4;
                        sound.Pitch = -1f;
                        PlaySound(sound, projectile.position);
                        int Type = 6;
                        for (float A = 0; A < projectile.scale; A += 0.01f)
                        {
                            Dust dust = Main.dust[NewDust(projectile.position, 1, 1, Type, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default)];
                            dust.noGravity = true;
                            dust.scale = 2;
                            dust.velocity = Main.rand.NextVector2Unit(vector4.ToRotation() - MathHelper.PiOver4, MathHelper.PiOver2) * -Main.rand.NextFloat(3f, 6.5f);
                        }
                        projectile.velocity = vector4.PerfectNormalize() * 30;
                    }
                    vector4.Normalize();
                    vector4 *= 4;
                    projectile.velocity = (projectile.velocity * 20 + vector4) / 21;
                }
                projectile.frame++;
                if (projectile.frame > 2)
                {
                    projectile.frame = 0;
                }
                for (int num172 = 0; num172 < 2; num172++)
                {
                    int Posit = 4;
                    Dust dust = Main.dust[NewDust(projectile.position + new Vector2(Posit), projectile.width - Posit * 2, projectile.height - Posit * 2, 6, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default, 2f)];
                    dust.position -= projectile.velocity * 2f;
                    dust.noGravity = true;
                    dust.velocity.X *= 0.3f;
                    dust.velocity.Y *= 0.3f;
                }
                return false;
            }
            //恶魔锄刀
            if (projectile.type == 45)
            {
                projectile.ai[0] -= 0.25f;
                NPC npc = projectile.FindTargetWithinRange(500, true);
                if (npc != null && npc.CanBeChasedBy(projectile, false) && projectile.DProj().track > 80 && projectile.DProj().track < 1200)
                {
                    Vector2 vector4 = Vector2.Subtract(npc.Center, projectile.Center);
                    vector4.Normalize();
                    vector4 *= 12;
                    projectile.velocity = (projectile.velocity * 9 + vector4) / 10;
                }
            }
            //水
            if (projectile.type == 22)
            {
                Color color = new Color(0, 25, 155, 140);
                if (projectile.ai[1] >= 8f)
                {
                    Dust dust = Main.dust[NewDust(projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0f, 0f, 100, color, 1.5f)];
                    dust.noGravity = true;
                    dust.customData = 2;
                    dust.velocity *= 0.1f;
                    dust.velocity += projectile.velocity * 0.1f;
                    dust.noLight = true;


                    if (Main.rand.NextBool(5))
                    {
                        dust = Main.dust[NewDust(projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0f, 0f, 100, color, 1.5f)];
                        dust.velocity *= 5f;
                        dust.customData = dust.DustAI(9) + 1.2F;
                        dust.velocity += projectile.velocity * 0.5f;
                        dust.noLight = true;
                    }
                }
                if (projectile.ai[1] == 7f)
                {
                    for (int a = 0; a < 30; a++)
                    {
                        Dust dust = Main.dust[NewDust(projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0f, 0f, 100, color, Main.rand.NextFloat(0.4F, 1.3F))];
                        dust.customData = dust.DustAI(9) + 1.2F;
                        dust.velocity = projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.4F, 1.3F);
                        dust.noLight = true;
                    }
                }

                projectile.ai[1]++;
                if (projectile.ai[1] >= 80f)
                    projectile.velocity.Y += 0.05f;


                if (projectile.velocity.Y > 16f)
                    projectile.velocity.Y = 16f;
                if (projectile.wet)
                {
                    projectile.Kill();
                }

                return false;

            }
            //水弹
            if (projectile.type == 27)
            {
                for (int num92 = 0; num92 < 2; num92++)
                {
                    Vector2 vector = projectile.velocity / 3f * num92;
                    Dust dust = Main.dust[NewDust(projectile.position, projectile.width, projectile.height, DustID.DungeonWater, 0f, 0f, 100, default, 1.2f)];
                    dust.noGravity = true;
                    dust.velocity *= 0.1f;
                    dust.velocity += projectile.velocity * 0.1f;
                    dust.position -= vector;
                }

                if (Main.rand.NextBool(5))
                {
                    int Posit = 4;
                    Dust dust = Main.dust[NewDust(projectile.position + new Vector2(Posit), projectile.width - Posit * 2, projectile.height - Posit * 2, DustID.DungeonWater, 0f, 0f, 100, default, 0.6f)];
                    dust.velocity *= 0.25f;
                    dust.velocity += projectile.velocity * 0.5f;
                }

                if (projectile.ai[1] >= 20f)
                    projectile.velocity.Y += 0.2f;


                projectile.rotation += 0.3f * projectile.direction;

                if (projectile.velocity.Y > 16f)
                    projectile.velocity.Y = 16f;
                return false;
            }
            //暗影束
            if (projectile.type == 294)
            {
                projectile.localAI[0] += 1f;
                projectile.alpha = 255;
                Dust dust = Main.dust[NewDust(projectile.position, 1, 1, 173)];
                dust.scale = 1;
                dust.alpha = 255;
                dust.velocity = Vector2.Zero;
                if (projectile.DProj().vector[0] == Vector2.Zero)
                {
                    projectile.DProj().vector[0] = projectile.velocity;
                }
                NPC result = null;
                float num = 1000;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!NPCW[i])
                    {
                        if (npc.CanBeChasedBy())
                        {
                            if (Collision.CanHitLine(projectile.Center, 1, 1, npc.position, npc.width, npc.height))
                            {
                                float num2 = (projectile.Center - npc.Center).Length();
                                if (!(num <= num2))
                                {
                                    num = num2;
                                    result = npc;
                                }
                            }
                        }
                    }
                }
                if (result != null && projectile.DProj().track > 10)
                {
                    projectile.Chase(result, projectile.DProj().vector[0].Length(), 0);
                }
                return false;
            }
            //地狱火
            if (projectile.type == 295)
            {
                projectile.Track(500, 20, 12, 30);
            }
            //灵液
            if (projectile.type == 280)
            {
                projectile.scale -= 0.002f;
                if (projectile.scale <= 0f)
                    projectile.Kill();
                projectile.velocity.Y += 0.075f;

                Dust dust = Main.dust[NewDust(projectile.position, 1, 1, 170, 0f, 0f, 100)];
                dust.noGravity = true;
                dust.scale = 1.3f;
                dust.velocity = projectile.velocity * 0.5f;

                if (projectile.velocity.Y > 0 && projectile.ai[0] == 0)
                {
                    NPC npc = projectile.FindTargetWithinRange(500, true);
                    if (npc != null && npc.CanBeChasedBy(projectile, false))
                    {
                        Vector2 vector4 = Vector2.Subtract(npc.Center, projectile.Center);
                        if (vector4.Y > 0)
                        {
                            vector4.Normalize();
                            vector4 *= 7;
                            projectile.velocity.X = (projectile.velocity.X * 9 + vector4.X) / 10;
                        }
                    }
                }
                return false;
            }
            //水晶碎块
            if (projectile.type == 94)
            {
                projectile.ArmorPenetration = 10;
            }
            //磁球
            if (projectile.type == 254)
            {
                //旋转
                if (projectile.velocity.X > 0f)
                    projectile.rotation += (Math.Abs(projectile.velocity.Y) + Math.Abs(projectile.velocity.X)) * 0.001f;
                else
                    projectile.rotation -= (Math.Abs(projectile.velocity.Y) + Math.Abs(projectile.velocity.X)) * 0.001f;
                //帧图
                projectile.frameCounter++;
                if (projectile.frameCounter > 6)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if (projectile.frame > 4)
                        projectile.frame = 0;
                }

                if (Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y) > 2.0)
                    projectile.velocity *= 0.98f;

                for (int num347 = 0; num347 < 1000; num347++)
                {
                    if (num347 != projectile.whoAmI && Main.projectile[num347].active && Main.projectile[num347].owner == projectile.owner && Main.projectile[num347].type == projectile.type && projectile.timeLeft > Main.projectile[num347].timeLeft && Main.projectile[num347].timeLeft > 30)
                        Main.projectile[num347].timeLeft = 30;
                }
                if (projectile.timeLeft <= 30)
                {
                    projectile.alpha += 20;
                    return false;
                }
                bool flag14 = false;
                NPC npc = projectile.FindTargetWithinRange(400, true);
                if (npc != null && npc.CanBeChasedBy(projectile, false))
                {
                    flag14 = true;
                }
                if (Main.myPlayer == projectile.owner)
                {
                    float Speed = (projectile.Player().Dplayer().MouseWorld - projectile.Center).Length() / 10;
                    if (Speed < 1) Speed = 1;
                    if (Speed > 60) Speed = 60;
                    projectile.velocity = (Main.MouseWorld - projectile.Center).PerfectNormalize() * Speed;
                    projectile.netUpdate = true;
                }
                if (Main.projectile[(int)projectile.ai[1]].type == ModContent.ProjectileType<MagnetSphere>() && Main.projectile[(int)projectile.ai[1]].active)
                {
                    projectile.timeLeft = 600;
                }
                if (flag14)
                {
                    projectile.localAI[0] += 1f;
                    if (projectile.localAI[0] > 25f || projectile.ai[0] == 1)
                    {
                        Vector2 vector = projectile.Center;
                        int Type = ModContent.DustType<光球粒子>();
                        for (int A = 0; A < 30; A++)
                        {
                            Dust dust = Main.dust[NewDust(projectile.Center - new Vector2(4), 0, 0, Type, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, new Color(2, 254, 201, 0))];
                            dust.noGravity = true;
                            dust.scale = Main.rand.NextFloat(1F, 3F);
                            dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3, 6f);
                            dust.rotation = projectile.rotation;
                            dust.customData = -6;
                        }
                        projectile.localAI[0] = 0f;

                        if (projectile.ai[0] == 1)
                        {
                            for (int A = -2; A <= 2; A++)
                            {
                                Vector2 vector20 = Utils.RotatedBy(Vector2.Subtract(npc.Center, projectile.Center).PerfectNormalize() * projectile.velocity.Length(), Main.rand.NextFloat(-1.1f, 1.1f), default);
                                if (projectile.owner == Main.myPlayer)
                                {
                                    int a = NewProjectile(projectile.GetSource_FromThis(), vector, vector20.PerfectNormalize() * 5, ModContent.ProjectileType<闪电>(), projectile.damage, projectile.knockBack, projectile.owner, 1, 1, Main.rand.Next(120, 240));
                                    Main.projectile[a].DamageType = DamageClass.Magic;
                                }
                            }
                            projectile.ai[0] = 0;
                        }
                        else
                        {
                            Vector2 vector20 = Utils.RotatedBy(Vector2.Subtract(npc.Center, projectile.Center).PerfectNormalize() * projectile.velocity.Length(), Main.rand.NextFloat(-1.1f, 1.1f), default);
                            if (projectile.owner == Main.myPlayer)
                            {
                                int a = NewProjectile(projectile.GetSource_FromThis(), vector, vector20.PerfectNormalize() * 5, ModContent.ProjectileType<闪电>(), projectile.damage, projectile.knockBack, projectile.owner, 1, 1, Main.rand.Next(120, 240));
                                Main.projectile[a].DamageType = DamageClass.Magic;
                            }
                        }
                        projectile.ai[2] = 25;
                    }
                }
                else
                {
                    projectile.localAI[0] += 1f;
                    if (projectile.localAI[0] > 35f || projectile.ai[0] == 1)
                    {
                        Vector2 vector = projectile.Center;
                        int Type = ModContent.DustType<光球粒子>();
                        for (int A = 0; A < 30; A++)
                        {
                            Dust dust = Main.dust[NewDust(projectile.Center - new Vector2(4), 0, 0, Type, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, new Color(2, 254, 201, 0))];
                            dust.noGravity = true;
                            dust.scale = Main.rand.NextFloat(1F, 3F);
                            dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3, 6f);
                            dust.rotation = projectile.rotation;
                            dust.customData = -6;
                        }
                        projectile.localAI[0] = 0f;
                        if (projectile.ai[0] == 1)
                        {
                            for (int A = -2; A <= 2; A++)
                            {
                                Vector2 vector20 = Main.rand.NextVector2Unit();
                                if (projectile.owner == Main.myPlayer)
                                {
                                    int a = NewProjectile(projectile.GetSource_FromThis(), vector, vector20.PerfectNormalize() * 5, ModContent.ProjectileType<闪电>(), projectile.damage, projectile.knockBack, projectile.owner, 1, 1, Main.rand.Next(120, 240));
                                    Main.projectile[a].DamageType = DamageClass.Magic;
                                }
                            }
                            projectile.ai[0] = 0;
                        }
                        else
                        {
                            Vector2 vector20 = Main.rand.NextVector2Unit();
                            if (projectile.owner == Main.myPlayer)
                            {
                                int a = NewProjectile(projectile.GetSource_FromThis(), vector, vector20.PerfectNormalize() * 5, ModContent.ProjectileType<闪电>(), projectile.damage, projectile.knockBack, projectile.owner, 1, 1, Main.rand.Next(120, 240));
                                Main.projectile[a].DamageType = DamageClass.Magic;
                            }
                        }
                        projectile.ai[2] = 25;
                    }

                }
                if (projectile.ai[2] > 0)
                {
                    projectile.ai[2]--;
                    projectile.velocity = Vector2.Zero;
                }
                return false;
            }
            /*
            if (projectile.type == 255)
            {
                if (Main.rand.NextBool(5) && projectile.DProj().track > 3)
                {
                    NPC npc = projectile.FindTargetWithinRange(500, true);
                    if (npc != null && npc.CanBeChasedBy(projectile, false))
                    {
                        float A = Vector2.Subtract(npc.Center, projectile.Center).Length() / 300;
                        if (A > 1)
                        {
                            A = 1;
                        }
                        Vector2 vector1 = Utils.RotatedBy(Vector2.Subtract(npc.Center, projectile.Center).PerfectNormalize() * projectile.velocity.Length(), Main.rand.NextFloat(-A, A), default);
                        projectile.velocity = vector1;
                    }
                    else
                    {
                        Vector2 vector1 = Utils.RotatedBy(projectile.DProj().vector[0] * projectile.velocity.Length(), Main.rand.NextFloat(-1, 1), default);
                        projectile.velocity = vector1;
                    }
                }
                projectile.DProj().Times[0] -= 0.003F;

                projectile.alpha = 255;
                Dust dust = Main.dust[NewDust(projectile.position, 1, 1, 160)];
                dust.velocity = Vector2.Zero;
                dust.scale = projectile.DProj().Times[0];
                if (projectile.scale < 0.01F)
                {
                    projectile.active = false;
                }
                return false;
            }*/
            //魔法导弹
            if (projectile.type == 16)
            {
                Player player = projectile.Player();
                projectile.extraUpdates = 1;
                if (player.controlUseItem && projectile.ai[1] == 0 && !player.controlUseTile)
                {
                    if (projectile.ai[0] == 0)
                    {
                        projectile.velocity = (player.Dplayer().MouseWorld - projectile.Center).PerfectNormalize() * 10;
                        if ((player.Dplayer().MouseWorld - projectile.Center).Length() < 5)
                        {
                            projectile.velocity = Vector2.Zero;
                            projectile.Center = player.Dplayer().MouseWorld;
                        }
                    }
                    else
                    {
                        projectile.ownerHitCheck = true;
                        int Proj = 0;
                        for (int a = 0; a < projectile.whoAmI; a++)
                        {
                            Projectile Projectile = Main.projectile[a];
                            if (Projectile.active && projectile.owner == Projectile.owner && Projectile.type == 16 && Projectile.ai[1] == 0 && Projectile.ai[0] != 0)
                            {
                                Proj++;
                            }
                        }
                        projectile.penetrate = -1;
                        projectile.tileCollide = false;
                        projectile.velocity = (player.Center + new Vector2(0, 100 + (player.Dplayer().PlayerTimes % 50)).RotatedBy(MathHelper.TwoPi / 3 * Proj + player.Dplayer().PlayerTimes * player.direction * 0.2F) - projectile.Center).PerfectNormalize() * 10;
                    }
                }
                else
                {
                    projectile.ownerHitCheck = false;
                    if (projectile.ai[1] == 0)
                    {
                        if (projectile.ai[0] == 0)
                        {
                            projectile.velocity = (player.Dplayer().MouseWorld - projectile.Center).PerfectNormalize() * 10;
                            if (projectile.velocity.Length() < 1)
                            {
                                projectile.velocity = projectile.rotation.ToRotationVector2() * 10;
                            }
                        }
                        else
                        {
                            projectile.velocity = (projectile.Center - player.Center).PerfectNormalize() * 10;
                            projectile.penetrate = 1;
                        }
                        projectile.ai[1] = 1;
                        projectile.tileCollide = true;
                        projectile.netUpdate = true;
                    }
                    projectile.Track(500, 20, 10, 0);
                }
                projectile.rotation = projectile.velocity.ToRotation();

                return false;
            }
            //彩虹导弹
            if (projectile.type == 79)
            {
                Player player = projectile.Player();
                projectile.extraUpdates = 1;
                if (player.controlUseItem && projectile.ai[1] == 0 && !player.controlUseTile)
                {
                    if (projectile.ai[0] == 0)
                    {
                        projectile.velocity = (player.Dplayer().MouseWorld - projectile.Center).PerfectNormalize() * 20;
                        if ((player.Dplayer().MouseWorld - projectile.Center).Length() < 5)
                        {
                            projectile.velocity = Vector2.Zero;
                            projectile.Center = player.Dplayer().MouseWorld;
                        }
                    }
                    else
                    {
                        projectile.ownerHitCheck = true;
                        for (int a = 1; a < projectile.oldPos.Length; a++)
                        {
                            if (projectile.oldPos[a] != Vector2.Zero)
                                projectile.oldPos[a] += player.velocity / 2;
                        }
                        int Proj = 0;
                        bool A = false;
                        for (int a = 0; a < projectile.whoAmI; a++)
                        {
                            Projectile Projectile = Main.projectile[a];
                            if (Projectile.active && projectile.owner == Projectile.owner && Projectile.type == 79 && Projectile.ai[1] == 0 && Projectile.ai[0] != 0)
                            {
                                Proj++;
                            }
                        }
                        if (Proj > 1)
                        {
                            A = true;
                        }
                        projectile.penetrate = -1;
                        projectile.tileCollide = false;

                        if (Proj < 4)
                        {
                            projectile.velocity = (player.Center + new Vector2(0, 200 + (player.Dplayer().PlayerTimes % 50)).RotatedBy(MathHelper.TwoPi / 2 * Proj + player.Dplayer().PlayerTimes * player.direction * 0.15F) - projectile.Center).PerfectNormalize() * 14;

                            if (A)
                            {
                                projectile.velocity.X *= 0.5f;
                                projectile.velocity.Y *= 1f;
                            }
                            else
                            {
                                projectile.velocity.X *= 1f;
                                projectile.velocity.Y *= 0.5f;
                            }
                            projectile.position += player.Dplayer().PrePosition / 2;
                            projectile.velocity = projectile.velocity.RotatedBy((-MathHelper.PiOver4 + 0.1F) * player.direction);
                        }
                        else
                        {
                            projectile.extraUpdates = 2;
                            Vector2 vector = new Vector2(0, 180).RotatedBy(player.Dplayer().PlayerTimes / 2 + MathHelper.TwoPi / 2 * Proj);
                            if (Proj % 2 == 0)
                            {
                                projectile.velocity = (player.Dplayer().MouseWorld + vector - projectile.Center).PerfectNormalize() * 15;
                            }
                            else
                            {

                                projectile.velocity = (player.Dplayer().MouseWorld + vector - projectile.Center).PerfectNormalize() * 15;
                            }
                        }
                    }
                }
                else
                {
                    projectile.ownerHitCheck = false;
                    if (projectile.ai[1] == 0)
                    {
                        if (projectile.ai[0] == 0)
                        {
                            projectile.velocity = (player.Dplayer().MouseWorld - projectile.Center).PerfectNormalize() * 20;
                            if (projectile.velocity.Length() < 1)
                            {
                                projectile.velocity = projectile.rotation.ToRotationVector2() * 20;
                            }
                        }
                        else
                        {
                            projectile.velocity = (projectile.Center - player.Center).PerfectNormalize() * 20;
                            projectile.penetrate = 3;
                        }
                        projectile.ai[1] = 1;
                        projectile.tileCollide = true;
                        projectile.netUpdate = true;
                    }
                    projectile.Track(1000, 20, 20, 0);
                }
                projectile.rotation = projectile.velocity.ToRotation();

                return false;
            }
            //火焰弹
            if (projectile.type == 22)
            {

            }
            //火焰弹
            if (projectile.type == 34)
            {
                Player player = projectile.Player();
                projectile.extraUpdates = 1;
                if (player.controlUseItem && projectile.ai[1] == 0 && !player.controlUseTile)
                {
                    if (projectile.ai[0] == 0)
                    {
                        projectile.velocity = (player.Dplayer().MouseWorld - projectile.Center).PerfectNormalize() * 10;
                        if ((player.Dplayer().MouseWorld - projectile.Center).Length() < 5)
                        {
                            projectile.velocity = Vector2.Zero;
                            projectile.Center = player.Dplayer().MouseWorld;
                        }
                    }
                    else
                    {
                        int Proj = 0;
                        for (int a = 0; a < projectile.whoAmI; a++)
                        {
                            Projectile Projectile = Main.projectile[a];
                            if (Projectile.active && projectile.owner == Projectile.owner && Projectile.type == 34 && Projectile.ai[1] == 0 && Projectile.ai[0] != 0)
                            {
                                Proj++;
                            }
                        }
                        projectile.ownerHitCheck = true;
                        projectile.penetrate = -1;
                        projectile.tileCollide = false;
                        projectile.velocity = (player.Dplayer().MouseWorld + new Vector2(0, 100).RotatedBy(MathHelper.TwoPi / 3 * Proj + player.Dplayer().PlayerTimes * player.direction * 0.2F) - projectile.Center).PerfectNormalize() * 10;
                    }
                }
                else
                {
                    projectile.ownerHitCheck = false;
                    if (projectile.ai[1] == 0)
                    {
                        if (projectile.ai[0] == 0)
                        {
                            projectile.velocity = (player.Dplayer().MouseWorld - projectile.Center).PerfectNormalize() * 10;
                            if (projectile.velocity.Length() < 1)
                            {
                                projectile.velocity = projectile.rotation.ToRotationVector2() * 10;
                            }
                        }
                        else
                        {
                            projectile.velocity = (projectile.Center - player.Center).PerfectNormalize() * 10;
                        }
                        projectile.ai[1] = 1;
                        projectile.tileCollide = true;
                        projectile.netUpdate = true;
                    }
                    projectile.penetrate = 1;
                    projectile.Track(500, 20, 10, 0);
                }
                if (Main.rand.NextBool(20))
                {
                    NewDust(projectile.position, projectile.width, projectile.height, 6, 0, -2, 0, default, 2);
                }
                projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
                if (projectile.velocity.Length() < 1)
                {
                    projectile.rotation = 0;
                }

                return false;
            }
            if (projectile.type == 645)
            {
                projectile.tileCollide = projectile.Center.Y > projectile.ai[1];
                projectile.Track(400, 30, 8);
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
                return false;

            }
            //鬼魂
            if (projectile.type == 297)
            {
                //projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
                projectile.tileCollide = projectile.localAI[0] > 60;
                //帧图
                if (projectile.localAI[0] == 0)
                {
                    projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
                    projectile.localAI[0] = 1;
                }
                else
                {
                    projectile.localAI[0]++;
                }
                projectile.localAI[1]++;
                if (projectile.localAI[1] >= 4)
                {
                    projectile.frame++;
                    if (projectile.frame >= 3)
                    {
                        projectile.frame = 0;
                    }
                }
                projectile.velocity = (projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * projectile.ai[0];
                NPC npc = NPCdirection.FindClosest(projectile.Center, 1500);
                if (npc != null)
                {
                    Vector2 vector = npc.Center - projectile.Center;
                    projectile.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.04F);
                    if (DDHelper.SpecifyDirection(projectile.rotation, vector.ToRotation() - MathHelper.PiOver2, 0.04F))
                    {
                        if (projectile.ai[0] < 8)
                        {
                            projectile.ai[0] += 0.3F;
                        }
                    }
                    else
                    {

                        projectile.ai[0] *= 0.98f;
                    }
                }
                else
                {
                    if (projectile.ai[0] < 8)
                    {
                        projectile.ai[0] += 0.3F;
                    }
                }
                return false;
            }
            return base.PreAI(projectile);
        }
        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            //诅咒火
            if (projectile.type == 95)
            {
                fallThrough = projectile.localAI[0] == 1;
            }
            return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public void Eye2(Projectile projectile)
        {
            Vector2 position = projectile.Center;
            int radius = 4;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(y + position.Y / 16.0f);

                    Vector2 vector = new Vector2(xPosition, yPosition);
                    if (vector.X > 0 && vector.Y > 0 && vector.X < Main.maxTilesX && vector.Y < Main.maxTilesY)
                    {
                        if (Main.tile[(int)vector.X, (int)vector.Y].HasTile)
                        {
                            if (Math.Sqrt(x * x + y * y) <= radius + 0.5)
                            {
                                WorldGen.SquareTileFrame((int)vector.X, (int)vector.Y, true);
                                WorldGen.KillTile((int)vector.X, (int)vector.Y);
                            }
                        }
                        if (Main.tile[(int)vector.X, (int)vector.Y].WallType > 0)
                        {
                            if (Math.Sqrt(x * x + y * y) <= radius + 0.5)
                            {
                                WorldGen.SquareTileFrame((int)vector.X, (int)vector.Y, true);
                                WorldGen.KillWall((int)vector.X, (int)vector.Y);
                            }
                        }
                    }
                }
            }
        }
        public override bool? CanDamage(Projectile projectile)
        {
            return base.CanDamage(projectile);
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            if (target.HasBuff(BuffID.Bleeding))
            {
                //血雨
                if (projectile.type == 245)
                {
                    modifiers.SourceDamage += 0.2F;
                }
            }
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            if (projectile.type == 645)
            {
                NewDustChange4(40,projectile.Center-new Vector2(4),Vector2.Zero,ModContent.DustType<光球粒子>(),0,4,true,0.4F,1.2F,100,0,new Color(20, 233, 201,0),-2);
                NewDustChange4(10,projectile.Center-new Vector2(4),Vector2.Zero,ModContent.DustType<速度粒子>(),0,12,true,1.4F,3.2F,100,1000,new Color(20, 233, 201,0),-2);
                return false;
            }
            if (projectile.type == 22)
            {
                NewDustChange4(40,projectile.Center-new Vector2(4),Vector2.Zero,ModContent.DustType<光球粒子>(),0,4,true,0.8F,2F,100,0,new Color(0, 15, 155,140),new Dust().DustAI(9)+2);
                return false;
            }
                return base.PreKill(projectile, timeLeft);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.type == 476)
            {
                if (Main.projectile[(int)projectile.ai[0]].localAI[0] < 4&&player.ownedProjectileCounts[ModContent.ProjectileType<SuckBloodPlayer>()]<8&&!target.Dnpc().Properties.Iron)
                {
                    NewProjectile(projectile.GetSource_FromThis(), target.Center, Main.rand.NextVector2Circular(0, MathHelper.TwoPi), ModContent.ProjectileType<SuckBloodPlayer>(), 10, 0, projectile.owner, projectile.ai[0], projectile.ai[1]);
                }
            }
            //高温射线
            if (projectile.type == 260)
            {
                for (int a = 0; a < 6; a++)
                {
                    Dust dust = Main.dust[Dust.NewDust(projectile.Center + projectile.velocity.PerfectNormalize() * 20 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(255, 181, 0, 0))];
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(2, 6);
                    dust.velocity = new Vector2(Main.rand.NextFloat(4, 12), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                    dust.rotation = dust.velocity.ToRotation();
                    dust.customData = dust.scale;
                }
                if (projectile.ai[1] == 0)
                {
                    for (int a = 0; a < 3; a++)
                        NewProjectile(projectile.GetSource_FromAI(), projectile.Center + projectile.velocity.PerfectNormalize() * 20, new Vector2(Main.rand.NextFloat(3, 7), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), projectile.type, projectile.damage / 3, 0, -1, 0, 1);
                    projectile.Kill();
                }
            }
            //暴雪法杖
            if (projectile.type == 337)
            {
                if (!target.Dnpc().BossPhysique && target.realLife < 0)
                {
                    target.AddBuff(ModContent.BuffType<Freeze>(), (int)Main.rand.NextFloat(120, 300));
                }
                else
                {
                    target.AddBuff(ModContent.BuffType<Frozen>(), (int)Main.rand.NextFloat(120, 300));
                }
            }
            //利刃台风
            if (projectile.type == 409)
            {
                projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
                Main.npc[target.whoAmI].immune[projectile.owner] = 1;
            }
            //魔法飞刀
            if (projectile.type == 93)
            {
                int Type = 57;
                for (float A = 0; A < 20; A += 1f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, Type, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 1.3F;
                    dust.velocity = Main.rand.NextVector2Unit(projectile.velocity.ToRotation() - MathHelper.PiOver4, 1.57F) * Main.rand.NextFloat(projectile.velocity.Length() / 4, projectile.velocity.Length() / 2);
                }
            }
            //暗影束
            if (projectile.type == 294)
            {
                projectile.DProj().Bool[0] = true;
                NPCW[target.whoAmI] = true;
                projectile.netUpdate = true;
            }
            //水箭
            if (projectile.type == 27)
            {
                NPC npc = NPCdirection.FindClosest(projectile.Center, 600, true, target);
                if (npc != null)
                {
                    projectile.velocity = (npc.Center - projectile.Center).PerfectNormalize() * projectile.velocity.Length();
                }
                int Type = 172;
                for (float A = 0; A < 1; A += 0.05f)
                {
                    Dust dust = Main.dust[NewDust(projectile.position, 1, 1, Type, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 1;
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2.5f, 3.5f);
                }
                projectile.damage = (int)(projectile.damage * 0.85f);
            }
            //恶魔锄刀
            if (projectile.type == 45)
            {
                if(projectile.DProj().track > 80)
                projectile.DProj().track = 1200;
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                int A = NewProjectile(projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, projectile.owner, target.whoAmI, 1);
                Main.projectile[A].DProj().color = new Color(90, 10, 255, 0);
                Main.projectile[A].localAI[0] = 1F;
                Main.projectile[A].scale = 1f;
            }
            //骷髅头
            if (projectile.type == 837)
            {
                target.AddBuff(24, 300);
            }
            //诅咒火
            if (projectile.type == 101)
            {
                target.AddBuff(39, 420);
            }
            //血雨
            if (projectile.type == 245)
            {
                if (Main.rand.NextBool(5)) target.AddBuff(BuffID.Bleeding, 120);
            }
            //魔刺
            if (projectile.type == 150 || projectile.type == 151 || projectile.type == 152)
            {
                target.immune[projectile.owner] = 5;
            }
            if (projectile.type == 632||projectile.type == 461)
            {
                projectile.numHits = 2;
            }
            
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            //魔法飞刀
            if (projectile.type == 93)
            {
                int Type = 57;
                for (float A = 0; A < 80; A += 1f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, Type, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 1.3F;
                    dust.velocity = Main.rand.NextVector2Unit(projectile.velocity.ToRotation() - MathHelper.PiOver4, 1.57F) * Main.rand.NextFloat(projectile.velocity.Length() / 4, projectile.velocity.Length() / 2);
                }
            }
            //磁球
            if (projectile.type == 255)
            {
                int Type = 160;
                for (float A = 0; A < 20; A++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, Type, projectile.oldVelocity.X, projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 1.2F;
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1, 10);
                }
                SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
                //SoundStyle sound = SoundID.Thunder;
                sound.Pitch = 0.5f;
                sound.Volume = 0.2f;
                PlaySound(sound, projectile.position);
            }
            if(projectile.type == 34)
            {
                if (projectile.ai[0] == 0)
                {
                    for (int a = 0; a < 1000; a++)
                    {
                        Projectile Projectile = Main.projectile[a];
                        if (Projectile.active && projectile.owner == Projectile.owner && Projectile.type == 34 && Projectile.ai[1] == 0 && Projectile.ai[0] != 0)
                        {
                            Projectile.ai[1] = 1;
                        }
                    }
                }
            }
            if(projectile.type == 297)
            {
                SoundStyle sound = SoundID.NPCDeath39;
                sound.MaxInstances = 50;
                sound.Volume = 0.1F;
                sound.Pitch = -1;
                PlaySound(sound, projectile.Center);
                for (int A = 0; A < 4; A++)
                {
                    Dust dust = Main.dust[NewDust(projectile.Center + new Vector2(4), 1, 1, ModContent.DustType<光圈粒子>(), projectile.oldVelocity.X, projectile.oldVelocity.Y, 0, new Color(214,241,241,0))];
                    dust.noGravity = true;
                    dust.scale = 0.1f;
                    dust.alpha = -5;
                    dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                    dust.velocity = Vector2.Zero;
                    dust.noLightEmittence = false;
                    dust.customData = new Vector4(0.1F, 40 * projectile.scale, 0, 1F);
                }
            }
        }
        public static Asset<Texture2D> Proj_316_Glow;
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            SpriteEffects sprite = 0;
            if (projectile.spriteDirection < 0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            if (projectile.type == 20)
            {
                lightColor = projectile.GetAlpha(Color.White);
                if (projectile.timeLeft < 30)
                {
                    lightColor *= projectile.timeLeft / 30f;
                }
                Main.spriteBatch.Draw(texture, projectile.position+new Vector2(projectile.width/2,0) - Main.screenPosition, null, lightColor, projectile.rotation,new Vector2(texture.Width/2,0), projectile.scale, 0, 0f);
                return false;
            }
            if (projectile.type == 316)
            {
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 4 * projectile.frame, texture.Width, texture.Height / 4)), lightColor, projectile.rotation, new Vector2(texture.Width, texture.Height / 4) / 2, projectile.scale, sprite, 0f);
                Main.spriteBatch.Draw(Proj_316_Glow.Value, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, Proj_316_Glow.Height() / 4 * projectile.frame, Proj_316_Glow.Width(), Proj_316_Glow.Height() / 4)), new Color(255, 0, 0, 0), projectile.rotation, new Vector2(Proj_316_Glow.Width(), Proj_316_Glow.Height() / 4) / 2, projectile.scale, sprite, 0f);
                if(projectile.DProj().Times[0]>110)
                {
                    Main.spriteBatch.Draw(Proj_316_Glow.Value, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, Proj_316_Glow.Height() / 4 * projectile.frame, Proj_316_Glow.Width(), Proj_316_Glow.Height() / 4)), Lighting.GetColor((int)projectile.Center.X/16, (int)projectile.Center.Y/16, new Color(33, 33, 33, 255)), projectile.rotation, new Vector2(Proj_316_Glow.Width(), Proj_316_Glow.Height() / 4) / 2, projectile.scale, sprite, 0f);
                }
                return false;
            }
            //魔法飞刀
            if (projectile.type == 93)
            {
                Vector2 vector = new Vector2(projectile.width, projectile.height) / 2;
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    for (int a = 0; a < 3; a++)
                    {
                        Vector2 vector2 = projectile.oldPos[i] + vector - Main.screenPosition;
                        Color color = new Color(236, 236, 51, 0) * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length / 2f);
                        Main.spriteBatch.Draw(MagicDaggerGlow.Value, vector2, null, color, projectile.oldRot[i], MagicDaggerGlow.Size() / 2, projectile.scale * 1.2F * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length * 1.2f), 0, 0f);
                        color = new Color(20, 20, 201, 0) * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length / 2f);
                        Main.spriteBatch.Draw(MagicDaggerGlow.Value, vector2, null, color, projectile.oldRot[i], MagicDaggerGlow.Size() / 2, projectile.scale * 0.5F * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length * 1.2f), 0, 0f);
                    }
                }
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale, 0, 0f);
                return false;
            }
            //恶魔锄刀
            if (projectile.type == 44 || projectile.type == 45)
            {
                Vector2 vector = new Vector2(projectile.width, projectile.height) / 2;
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    for (int a = 0; a < 3; a++)
                    {
                        Vector2 vector2 = projectile.oldPos[i] + vector - Main.screenPosition;
                        Color color = new Color(132, 2, 253, 0) * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length) * 0.5f;
                        Main.spriteBatch.Draw(texture, vector2, null, color, projectile.oldRot[i], texture.Size() / 2, projectile.scale, 0, 0f);
                    }
                }
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), new Color(255, 255, 255, 0), projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), new Color(255, 255, 255, 0), projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), new Color(255, 255, 255, 0), projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale, 0, 0f);
                return false;
            }
            //骷髅头
            if (projectile.type == 837)
            {
                if (projectile.ai[0] == 0)
                {
                    return true;
                }
                sprite = 0;
                if (projectile.direction < 0)
                {
                    sprite = (SpriteEffects)(1);
                }
                Vector2 vector = new Vector2(projectile.width, projectile.height) / 2;
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    for (int a = 0; a < 3; a++)
                    {
                        Vector2 vector2 = projectile.oldPos[i] + vector - Main.screenPosition;
                        Color color = new Color(253, 62, 3, 0) * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length) * 0.5f;
                        Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, texture.Height / 3 * projectile.frame, texture.Width, texture.Height / 3)), color, projectile.oldRot[i], new Vector2(texture.Width, texture.Height / 3) / 2, projectile.scale * 1.3f, sprite, 0f);
                        Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, texture.Height / 3 * projectile.frame, texture.Width, texture.Height / 3)), color, projectile.oldRot[i], new Vector2(texture.Width, texture.Height / 3) / 2, projectile.scale * 1.3f, sprite, 0f);
                    }
                }
                return false;
            }
            //鬼魂
            if (projectile.type == 645)
            {
                texture = DDTextures.MiniVoidStar.Value;
                Vector2 vector = projectile.Size / 2;
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    if (projectile.oldPos[i] != projectile.position)
                    {
                        Vector2 vector2 = projectile.oldPos[i] + vector - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, vector2, null, new Color(20, 233, 201, 0), projectile.rotation, texture.Size() / 2, projectile.scale / 4 * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length) + 0.1F, 0, 0f);
                        Main.spriteBatch.Draw(texture, vector2, null, new Color(20, 233, 201, 0).Opposite() * 0.25F, projectile.rotation, texture.Size() / 2, projectile.scale / 8 * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length) + 0.1F, 0, 0f);
                        //Main.spriteBatch.Draw(texture, vector2, null, new Color(71, 233, 60, 0).Opposite() * 0.5F, projectile.rotation, texture.Size() / 2, projectile.scale / 10 + 0.025F + (float)i / 675 * 0.26f, spriteEffects, 0f);
                    }
                }
                return false;
            }
            //鬼魂
            if (projectile.type == 297)
            {
                texture = 鬼魂.Value;
                Vector2 vector = projectile.Size/ 2;
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(255, 255, 255, 0) * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length) * 0.5f;
                    Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, texture.Height / 3 * projectile.frame, texture.Width, texture.Height / 3)), color, projectile.oldRot[i], new Vector2(texture.Width, texture.Height / 3) / 2, projectile.scale * ((projectile.oldPos.Length - i) / (float)projectile.oldPos.Length), sprite, 0f);

                }
                Main.spriteBatch.Draw(texture, projectile.Center- Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * projectile.frame, texture.Width, texture.Height / 3)), Color.White, projectile.rotation, new Vector2(texture.Width, texture.Height / 3) / 2, projectile.scale, sprite, 0f);
                Main.spriteBatch.Draw(texture, projectile.Center- Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * projectile.frame, texture.Width, texture.Height / 3)), new Color(255, 255, 255, 0), projectile.rotation, new Vector2(texture.Width, texture.Height / 3) / 2, projectile.scale, sprite, 0f);

                return false;
            }
            //磁球
            if (projectile.type == 254)
            {
                Vector2 vector = new Vector2(projectile.width, projectile.height) / 2;
                Main.spriteBatch.Draw(texture, projectile.position + vector - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 5 * projectile.frame, texture.Width, texture.Height / 5)), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 10), projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(VoidStar, projectile.position + vector - Main.screenPosition, null, new Color(2, 254, 201, 0) * 0.8F, projectile.rotation, VoidStar.Size() / 2, projectile.scale / 2F, 0, 0f);
                Main.spriteBatch.Draw(VoidStar, projectile.position + vector - Main.screenPosition, null, new Color(253, 1, 54, 0) * 0.8F, projectile.rotation, VoidStar.Size() / 2, projectile.scale / 3F, 0, 0f);
                return false;
            }
            //邪恶三叉戟
            if (projectile.type == 114)
            {
                    Vector2 vector = projectile.Size / 2;
                    Main.spriteBatch.Draw(texture, projectile.position + vector - Main.screenPosition,null, new Color(181, 16, 233, 100), projectile.rotation, texture.Size() / 2, projectile.scale, 0, 0f);
                    Main.spriteBatch.Draw(texture, projectile.position + vector - Main.screenPosition, null, new Color(181, 16, 233, 100), projectile.rotation, texture.Size()/2, projectile.scale, 0, 0f);

                
                return false;
            }
            //魔法导弹
            if (projectile.type == 16)
            {
                Color ColorFunction(float completionRatio)
                {
                    float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
                    return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
                    {
                    new Color(69, 189, 249, 0),
                    }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(169, 89, 249, 0), (float)Math.Pow((double)completionRatio, 1.0)); ;
                }
                float WidthFunction(float completionRatio)
                {
                    float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
                    return MathHelper.Lerp(0, 40f, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
                }
                if (TrailDrawer == null)
                {
                    TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
                }
                GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.FireEffect2);
                GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
                TrailDrawer.Draw(projectile.oldPos, projectile.Size * 0.5f - Main.screenPosition, 104, null);
                DDHelper.BackAndForth(0.4F, 0.8F, 0.04F, ref projectile.DProj().Times[0], ref projectile.DProj().Bool[0]);
                float S = 1 - (projectile.velocity.Length() / 10);
                if(S<0.5F)
                {
                    S = 0.5F;
                }
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color(69, 189, 249, 0), projectile.rotation, texture.Size() / 2, new Vector2(2 - S, S), 0, 0f);
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color(69, 189, 249, 0), projectile.rotation, texture.Size() / 2, new Vector2(2-S, S) * 2  * projectile.DProj().Times[0], 0, 0f);
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color(69, 189, 249, 0), projectile.rotation, texture.Size() / 2, new Vector2(2-S, S) * 2  * projectile.DProj().Times[0], 0, 0f);
                return false;
            } 
            //火焰弹
            if (projectile.type == 34)
            {
                Color ColorFunction(float completionRatio)
                {
                    float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
                    return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
                    {
                    new Color(253, 62, 3, 0),
                    }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(253, 62, 3, 0), (float)Math.Pow((double)completionRatio, 1.0)); ;
                }
                float WidthFunction(float completionRatio)
                {
                    float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
                    return MathHelper.Lerp(0, 20f, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
                }
                if (TrailDrawer == null)
                {
                    TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
                }
                GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.FireEffect);
                GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
                TrailDrawer.Draw(projectile.oldPos, projectile.Size * 0.5f - Main.screenPosition, 104, null);
                TrailDrawer.Draw(projectile.oldPos, projectile.Size * 0.5f - Main.screenPosition, 104, null);
                projectile.frameCounter++;
                if (projectile.frameCounter % 2 == 0)
                {
                    projectile.frame++;
                    projectile.frame %= 6;
                }
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0,texture.Height/Main.projFrames[projectile.type]*projectile.frame,texture.Width, texture.Height / Main.projFrames[projectile.type])), Color.White, projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[projectile.type]) / 2, projectile.scale, 0, 0f);
                return false;
            }
            return base.PreDraw(projectile, ref lightColor);
        }
        public override void PostDraw(Projectile projectile, Color lightColor)
        {
        }
        public override void DrawBehind(Projectile projectile, int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
        }
        internal Trailing TrailDrawer;
    }
}