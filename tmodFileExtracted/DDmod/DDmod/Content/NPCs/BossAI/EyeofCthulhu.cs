using DDmod.Content.Particles;
using DDmod.Content.Projectiles.Boss;
using DDmod.NoContent.Config;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.NPCs.BossAI
{
    //克苏鲁之眼
    public class EyeofCthulhu : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                if (Main.masterMode && false)
                {
                    npc.alpha = 15;
                }
                if (ModContent.GetInstance<DDConfigServer>().BossAnimation)
                {
                    npc.Dnpc().Deathrattle = true;
                    npc.dontTakeDamage = true;
                }
                else
                {
                    npc.Dnpc().Bool[4] = true;
                }
            }
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                if (npc.Dnpc().Deathrattle)
                {
                    if (npc.life <= 0)
                    {
                        npc.life = 100;
                        npc.Dnpc().Bool[3] = true;
                        npc.dontTakeDamage = true;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }
                }
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                if (npc.Dnpc().Bool[3])
                {
                    npc.netUpdate = true;
                    npc.ai[3] += 0.003F;
                    npc.rotation += npc.ai[3];
                    if (NPC.AnyNPCs(ModContent.NPCType<BlackHole>()))
                    {
                        Vector2 vector = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<BlackHole>())].Center - npc.Center;
                        float Le = vector.Length();
                        if (Le < 1) Le = 1;
                        if (Le < 5)
                        {
                            npc.scale -= 0.01F;
                        }
                        npc.velocity = vector.PerfectNormalize() * (Le / 30);
                        if (npc.scale <= 0.1F)
                        {
                            if (!npc.Dnpc().Bool[2])
                            {
                                npc.NPCLoot();
                            }
                            Main.npc[NPC.FindFirstNPC(ModContent.NPCType<BlackHole>())].ai[1] = 1;
                            npc.active = false;
                        }
                        if (npc.scale <= 0.5F)
                        {
                            npc.alpha += 10;
                        }
                        npc.spriteDirection = 0;
                        Main.LocalPlayer.Dplayer().Bossperspective(npc.Center, 60, false, 0.2f);
                    }
                    else
                    {
                        if (Main.netMode != 1)
                        {
                            npc.ai[2] = NewNPCs(npc.GetSource_FromAI(), npc.Center - new Vector2(0, 400), ModContent.NPCType<BlackHole>(), npc.whoAmI);
                            Main.npc[(int)npc.ai[2]].velocity = Vector2.Zero;
                        }
                        //npc.Kill();
                    }
                    return false;
                }
                if (npc.Dnpc().Bool[4])
                {
                    if (Main.masterMode && false)
                    {
                        Eye(npc);
                    }
                    else
                    {
                        npc.dontTakeDamage = false;
                        return true;
                    }
                }
                else
                {
                    Appearance(npc);
                }
                return false;
            }
            return true;
        }
        //肉前AI
        public void Eye(NPC npc)
        {
            Player player = Main.player[npc.target];
            float Rotation = npc.rotation + MathHelper.PiOver2;
            npc.TargetClosest();
            Vector2 vector = player.Center - npc.Center;
            if (Main.dayTime)
            {
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                npc.ai[3] = 0;
                npc.Dnpc().Bool[3] = true;
                npc.Dnpc().Bool[2] = true;
                return;
            }
            if (npc.target == 255 || npc.target < 0 || player.dead || vector.Length() > 3000)
            {
                npc.TargetClosest();
                npc.velocity.Y -= 0.1f;
                npc.timeLeft--;
                return;
            }
            else
            {
                npc.timeLeft = 30;
            }
            npc.dontTakeDamage = npc.ai[0] == 1 || npc.ai[0] == 2;
            if (npc.life <= npc.lifeMax * 0.77f && npc.ai[0] == 0)
            {
                npc.ai[0] = 1;
                npc.ai[1] = 0;
            }
            if (npc.ai[0] == 0)
            {
                npc.ai[1]++;
                if (npc.ai[1] < 200)
                {
                    npc.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.1F);

                    if (npc.ai[1] % 60 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int A = NewNPCs(npc.GetSource_FromAI(), npc.Center + Rotation.ToRotationVector2() * 60, 5, 0);
                        Main.npc[A].velocity = Rotation.ToRotationVector2() * 5;
                        for (int a = 0; a < 40; a++)
                        {
                            Dust dust = Main.dust[NewDust(npc.Center + Rotation.ToRotationVector2() * npc.height / 2, 16, 16, 5, 0, 0, 0, default, 2)];
                            dust.velocity = Main.rand.NextVector2Unit(Rotation - MathHelper.PiOver4, 2F) * Main.rand.NextFloat(1F, 2);
                        }
                        PlaySound(SoundID.NPCHit1, npc.position);
                    }
                    vector.Y -= 300;
                    if (vector.Length() < 2) npc.velocity *= 0.98F; else npc.velocity = (npc.velocity * 20 + vector.PerfectNormalize() * 12) / 21;
                }
                else if (npc.ai[1] < 260)
                {
                    if (npc.ai[1] % 30 == 0)
                    {
                        PlaySound(SoundID.Roar, npc.position);
                        //vector = Utils.RotatedBy(vector.PerfectNormalize(), MathHelper.PiOver4, default);
                        npc.velocity = vector.PerfectNormalize() * 15;
                        npc.netUpdate = true;
                    }
                    if (npc.ai[1] > 210)
                    {
                        npc.rotation = npc.velocity.ToRotation() - MathHelper.PiOver2;
                    }
                    else
                    {
                        npc.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.1F);
                    }
                }
                else if (npc.ai[1] < 300)
                {
                    npc.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.1F);
                    vector.Y -= 300;
                    if (vector.Length() < 2) npc.velocity *= 0.98F; else npc.velocity = (npc.velocity * 20 + vector.PerfectNormalize() * 12) / 21;
                    npc.alpha += 10;
                    if (npc.alpha > 255)
                    {
                        npc.alpha = 255;
                    }
                    for (int a = 0; a < 20; a++)
                    {
                        NewDust(npc.position, npc.width, npc.height, 5, 0, 0, 0, default, 1);
                    }
                    npc.dontTakeDamage = true;
                }
                else if (npc.ai[1] == 300)
                {
                    npc.Center = player.Center + Utils.RotatedBy(new Vector2(0, 300), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4), default);
                    npc.netUpdate = true;
                }
                else if (npc.ai[1] < 340)
                {
                    npc.alpha -= 10;
                    for (int a = 0; a < 20; a++)
                    {
                        NewDust(npc.position, npc.width, npc.height, 5, 0, 0, 0, default, 1);
                    }
                    npc.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.1F);
                    if (vector.Length() < 2) npc.velocity *= 0.98F; else npc.velocity = (npc.velocity * 20 + vector.PerfectNormalize() * 2) / 21;

                    npc.dontTakeDamage = false;
                }
                else if (npc.ai[1] == 340)
                {
                    PlaySound(SoundID.Roar, npc.position);
                    npc.velocity = vector.PerfectNormalize() * 20;
                    npc.netUpdate = true;
                }
                else if (npc.ai[1] < 380)
                {
                    npc.rotation = npc.velocity.ToRotation() - MathHelper.PiOver2;
                }
                else
                {
                    npc.ai[1] = 0;
                }
            }
            else if (npc.ai[0] == 1)
            {
                npc.alpha -= 10;
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
                npc.velocity *= 0.98f;
                npc.ai[1]++;
                npc.rotation += npc.ai[1] / 100;
                if (npc.ai[1] > 90)
                {
                    npc.ai[0] = 2;
                    PlaySound(SoundID.NPCHit1, npc.position);
                    int num;
                    for (int num35 = 0; num35 < 2; num35 = num + 1)
                    {
                        Gore.NewGore(npc.GetSource_FromAI(), npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 8, 1f);
                        Gore.NewGore(npc.GetSource_FromAI(), npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 7, 1f);
                        Gore.NewGore(npc.GetSource_FromAI(), npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 6, 1f);
                        num = num35;
                    }
                    for (int num36 = 0; num36 < 20; num36 = num + 1)
                    {
                        NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f, 0, default, 1f);
                        num = num36;
                    }
                    PlaySound(SoundID.Roar, npc.position);
                }
            }
            else if (npc.ai[0] == 2)
            {
                npc.ai[1] -= 1F;
                npc.rotation += npc.ai[1] / 100;
                if (npc.ai[1] <= 0)
                {
                    npc.ai[0] = 3;
                    npc.ai[1] = 0;
                }
            }
            else if (npc.ai[0] == 3)
            {
                npc.defense = 0;
                npc.damage = 40;
                //第一段调整位置
                if (npc.ai[1] == 0f)
                {
                    npc.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.1F);

                    vector.Y -= 300;
                    if (vector.Length() < 2) npc.velocity *= 0.9F; else npc.velocity = (npc.velocity * 20 + vector.PerfectNormalize() * 12) / 21;
                    if (npc.life <= npc.lifeMax / 3)
                    {
                        if (npc.ai[2] == 0)
                        {
                            NewProjectile(npc.GetSource_FromAI(), player.Center + new Vector2(600 + Main.rand.NextFloat(0, 400), Main.rand.NextFloat(100, 400)), Vector2.Zero, ModContent.ProjectileType<BlackHoleProj>(), npc.damage / 6, 1, 0);
                            NewProjectile(npc.GetSource_FromAI(), player.Center + new Vector2(600 + Main.rand.NextFloat(0, 40), Main.rand.NextFloat(-400, -100)), Vector2.Zero, ModContent.ProjectileType<BlackHoleProj>(), npc.damage / 6, 1, 0);
                            NewProjectile(npc.GetSource_FromAI(), player.Center - new Vector2(600 + Main.rand.NextFloat(0, 400), Main.rand.NextFloat(100, 400)), Vector2.Zero, ModContent.ProjectileType<BlackHoleProj>(), npc.damage / 6, 1, 0);
                            NewProjectile(npc.GetSource_FromAI(), player.Center - new Vector2(600 + Main.rand.NextFloat(0, 400), Main.rand.NextFloat(-400, -100)), Vector2.Zero, ModContent.ProjectileType<BlackHoleProj>(), npc.damage / 6, 1, 0);

                            for (int A = 0; A < 3; A++)
                            {
                                Vector2 NPCs = Utils.RotatedBy(new Vector2(350), Main.rand.NextFloat(0, MathHelper.TwoPi), default);
                                NewProjectile(npc.GetSource_FromAI(), player.Center + NPCs, Vector2.Zero, ModContent.ProjectileType<BlackHoleProj>(), 0, 1, 0, 1);
                            }
                        }
                    }
                    npc.ai[2] += 1f;
                    if (npc.ai[2] >= 200f)
                    {
                        npc.ai[1] = 1f;
                        npc.ai[2] = 0f;
                        npc.ai[3] = 0f;

                        npc.netUpdate = true;
                    }
                }
                //疯狗一段
                else if (npc.ai[1] == 1f)
                {
                    if (Main.netMode != 1)
                    {
                        float Length = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y) / 4f;
                        vector.X -= player.velocity.X * Length;
                        vector.Y -= player.velocity.Y * Length / 4f;

                        float A = 200 - Math.Abs(player.Center.Y) - Math.Abs(npc.Center.Y);
                        if (player.Center.Y - npc.Center.Y > 0)
                        {
                            if (A < 0) A = 0;
                        }
                        else
                        {
                            A = -200 + Math.Abs(player.Center.Y) - Math.Abs(npc.Center.Y);
                            if (A > 0) A = 0;
                        }
                        vector.Y += A;

                        npc.velocity = vector.PerfectNormalize() * 20;
                        npc.rotation = npc.velocity.ToRotation() - 1.57F;
                        npc.ai[1] = 2f;
                        npc.netUpdate = true;
                        if (npc.netSpam > 10)
                        {
                            npc.netSpam = 10;
                        }
                    }
                }
                else if (npc.ai[1] == 2f)
                {
                    if (npc.ai[2] == 0f)
                    {
                        SoundStyle sound = SoundID.ForceRoar;
                        sound.Pitch = 0.5f;
                        PlaySound(sound, npc.position);
                    }
                    npc.rotation = npc.velocity.ToRotation() - 1.57F;
                    npc.ai[2] += 1f;
                    if ((npc.ai[2] > 20 && Math.Abs(vector.Y) > 300) || npc.ai[2] > 40)
                    {
                        npc.netUpdate = true;
                        if (npc.netSpam > 10)
                        {
                            npc.netSpam = 10;
                        }
                        npc.ai[3] += 1f;
                        npc.ai[2] = 0f;
                        if (npc.ai[3] >= 5f)
                        {
                            npc.ai[1] = 3f;
                            npc.ai[3] = 0f;
                            if (vector.X > 0)
                            {
                                npc.Dnpc().vector[0] = Utils.RotatedBy(new Vector2(400, 0), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4), default);
                            }
                            else
                            {
                                npc.Dnpc().vector[0] = Utils.RotatedBy(new Vector2(-400, 0), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4), default);
                            }
                            npc.netUpdate = true;
                        }
                        else
                        {
                            npc.ai[1] = 1f;
                        }
                    }
                }
                else if (npc.ai[1] == 3f)
                {
                    if (npc.ai[2] == 0f)
                    {
                        int A = NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<EyeofCthulhuBosssProj>(), npc.damage / 6, 1, 0, npc.whoAmI);
                        Main.projectile[A].netUpdate = true;
                    }
                    npc.ai[2]++;
                    npc.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.1F);
                    vector += npc.Dnpc().vector[0];
                    if (vector.Length() < 2) npc.velocity *= 0.9F; else npc.velocity = (npc.velocity * 20 + vector.PerfectNormalize() * 12) / 21;
                    if (npc.ai[2] > 120)
                    {
                        npc.ai[2] = 0;
                        npc.ai[1] = 4f;
                    }
                    npc.netUpdate = true;
                }
                else if (npc.ai[1] == 4f)
                {
                    if (npc.ai[2] == 0f)
                    {
                        npc.velocity = vector.PerfectNormalize() * 16;
                        SoundStyle sound = SoundID.ForceRoar;
                        sound.Pitch = -0.5f;
                        PlaySound(sound, npc.position);
                        npc.netUpdate = true;
                    }
                    npc.rotation = npc.velocity.ToRotation() - 1.57F;
                    npc.ai[2]++;
                    if (npc.ai[2] > 120)
                    {
                        if (npc.life > npc.lifeMax / 3)
                            npc.ai[1] = 0f;
                        else
                            npc.ai[1] = 5f;

                        npc.ai[2] = 0;
                        npc.ai[3] = 0f;
                    }
                }
                else if (npc.ai[1] == 5f)
                {
                    npc.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.1F);
                    vector.Y -= 300;
                    if (vector.Length() < 2) npc.velocity *= 0.98F; else npc.velocity = (npc.velocity * 20 + vector.PerfectNormalize() * 12) / 21;
                    npc.alpha += 10;
                    if (npc.alpha > 255)
                    {
                        npc.alpha = 255;
                        npc.Center = player.Center + Utils.RotatedBy(new Vector2(0, 400), Main.rand.NextFloat(0, MathHelper.TwoPi), default);
                        npc.netUpdate2 = true;
                        npc.ai[2] = 0;
                        npc.ai[1] = 6;
                    }
                    npc.netUpdate = true;
                    for (int a = 0; a < 20; a++)
                    {
                        NewDust(npc.position, npc.width, npc.height, 5, 0, 0, 0, default, 1);
                    }
                    npc.dontTakeDamage = true;
                }
                else if (npc.ai[1] == 6f)
                {
                    npc.ai[2]++;
                    npc.alpha -= 5;
                    npc.netUpdate = true;
                    for (int a = 0; a < 20; a++)
                    {
                        NewDust(npc.position, npc.width, npc.height, 5, 0, 0, 0, default, 1);
                    }
                    npc.RotationSpeed(vector.ToRotation() - MathHelper.PiOver2, 0.1F);
                    if (vector.Length() < 2) npc.velocity *= 0.98F; else npc.velocity = (npc.velocity * 20 + vector.PerfectNormalize() * 2) / 21;
                    npc.position += player.velocity;
                    npc.dontTakeDamage = false;
                    if (npc.ai[2] > 80)
                    {
                        npc.ai[2] = 0;
                        npc.ai[1] = 7;
                    }
                }
                else if (npc.ai[1] == 7f)
                {
                    if (Main.netMode != 1)
                    {
                        float Length = Math.Abs(player.velocity.X) * 1.5F + Math.Abs(player.velocity.Y) / 4f;
                        vector.X -= player.velocity.X * Length * 1.5F;
                        vector.Y -= player.velocity.Y * Length / 4f;

                        float A = 200 - Math.Abs(player.Center.Y) - Math.Abs(npc.Center.Y);
                        if (player.Center.Y - npc.Center.Y > 0)
                        {
                            if (A < 0) A = 0;
                        }
                        else
                        {
                            A = -200 + Math.Abs(player.Center.Y) - Math.Abs(npc.Center.Y);
                            if (A > 0) A = 0;
                        }
                        vector.Y += A;

                        npc.velocity = vector.PerfectNormalize() * 45;
                        npc.rotation = npc.velocity.ToRotation() - 1.57F;
                        npc.ai[1] = 8f;
                        npc.netUpdate = true;
                        if (npc.netSpam > 10)
                        {
                            npc.netSpam = 10;
                        }
                    }
                }
                else if (npc.ai[1] == 8f)
                {
                    if (npc.ai[2] == 0f)
                    {
                        SoundStyle sound = SoundID.ForceRoar;
                        sound.Pitch = 0.5f;
                        PlaySound(sound, npc.position);
                    }
                    npc.rotation = npc.velocity.ToRotation() - 1.57F;
                    npc.ai[2] += 1f;
                    if ((npc.ai[2] > 10 && Math.Abs(vector.Y) > 200) || npc.ai[2] > 20)
                    {
                        npc.netUpdate = true;
                        if (npc.netSpam > 10)
                        {
                            npc.netSpam = 10;
                        }
                        npc.ai[3] += 1f;
                        npc.ai[2] = 0f;
                        if (npc.ai[3] >= 20f)
                        {
                            npc.ai[1] = 0f;
                            npc.ai[3] = 0f;
                            npc.netUpdate = true;
                        }
                        else
                        {
                            npc.ai[1] = 7f;
                        }
                    }
                }
            }
        }
        public void Appearance(NPC npc)
        {
            npc.netUpdate = true;
            if (npc.ai[1] == 0)
            {
                Player player = Main.player[npc.target];
                npc.TargetClosest();
                Vector2 vector = player.Center - npc.Center;
                npc.rotation = vector.ToRotation() - 1.57f;
            }
            npc.ai[1]++;
            npc.ai[2] += 0.1f;
            npc.ai[3] -= 0.1f;
            npc.velocity = Vector2.Zero;
            Main.LocalPlayer.Dplayer().Bossperspective(npc.Center, 60,false, 0.2f);
            if (npc.ai[1] >= 236)
            {
                npc.localAI[0] -= 2f;
            }
            else if (npc.ai[1] > 90)
            {
                if (npc.ai[1] < 100)
                {
                    npc.ai[1] += 3;
                }
                else if (npc.localAI[0] < 40)
                {
                    npc.localAI[0] += 0.25f;
                }
                else
                {
                    npc.ai[1] += 3;
                }
            }
            else if (npc.ai[1] < 60)
            {
                npc.localAI[0] += 0.5f;
            }
            if (npc.ai[1] >= 256)
            {
                npc.ai[0] = 0;
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                npc.ai[3] = 0;
                npc.Dnpc().Bool[4] = true;
                npc.netUpdate = true;
            }
            Vector2 Center = Utils.RotatedBy(new Vector2(Main.rand.NextFloat(-npc.localAI[0], npc.localAI[0]), 0), npc.rotation, default);
            Vector2 Vector = Utils.RotatedBy((npc.rotation - 1.57f).ToRotationVector2().PerfectNormalize(), Main.rand.NextFloat(-1.5F, 1.5F), default);
            DDParticle.RequestParticleSpawn(ParticleType.blackHole, new ParticleOrchestraSettings
            {
                PositionInWorld = npc.Center + Vector + Center + (npc.rotation - 1.57f).ToRotationVector2().PerfectNormalize() * 92,
                MovementVector = -Vector * 2
            });
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                if (!AnyNPCs(4))
                {
                    return true;
                }
                if (!npc.Dnpc().Bool[4])
                {
                    if (npc.ai[1] < 90)
                    {
                        Vector2 Center = npc.Center - screenPos + (npc.rotation + 1.57f).ToRotationVector2().PerfectNormalize() * -94;
                        Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/NPCs/BlackHole").Value;
                        DDHelper.Compression(texture, new Color(255, 0, 0) * 0.9f, 0, 255, new Vector2(5, 1), 0, npc.ai[2], BlendState.Additive);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Value, Center, null, new Color(255, 0, 0) * 0.9f, npc.rotation + 1.57f, ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Size() / 2, npc.localAI[0] / 140, SpriteEffects.None, 0);
                        DDHelper.Compression(texture, new Color(255, 0, 0) * 0.9f, 0, 255, new Vector2(5, 1), 0, npc.ai[3], BlendState.Additive);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Value, Center, null, new Color(255, 0, 0) * 0.9f, npc.rotation + 1.57f, ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Size() / 2, npc.localAI[0] / 140, SpriteEffects.None, 0);
                        DDHelper.Compression(texture, new Color(255, 255, 255) * 0.9f, 0, 255, new Vector2(5, 1), 0, 0, BlendState.AlphaBlend);
                        spriteBatch.Draw(texture, Center, null, Color.White, npc.rotation + 1.57f, texture.Size() / 2, npc.localAI[0] / 35, SpriteEffects.None, 0);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    }
                    else
                    {

                        Vector2 Center = npc.Center - screenPos + (npc.rotation + 1.57f).ToRotationVector2().PerfectNormalize() * -94;
                        Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/NPCs/BlackHole").Value;
                        DDHelper.Compression(texture, new Color(255, 0, 0) * 0.9f, 0, 255, new Vector2(5, 1), 0, npc.ai[2], BlendState.Additive);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Value, Center, null, new Color(255, 0, 0) * 0.9f, npc.rotation + 1.57f, ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Size() / 2, npc.localAI[0] / 140, SpriteEffects.None, 0);

                        DDHelper.Compression(texture, new Color(255, 0, 0) * 0.9f, 0, 255, new Vector2(5, 1), 0, npc.ai[3], BlendState.Additive);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Value, Center, null, new Color(255, 0, 0) * 0.9f, npc.rotation + 1.57f, ModContent.Request<Texture2D>("DDmod/Image/旋转特效").Size() / 2, npc.localAI[0] / 140, SpriteEffects.None, 0);

                        DDHelper.Compression(texture, new Color(255, 255, 255) * 0.9f, 0, 255, new Vector2(5, 1), 0, 0, BlendState.AlphaBlend);
                        spriteBatch.Draw(texture, Center, null, Color.White, npc.rotation + 1.57f, texture.Size() / 2, npc.localAI[0] / 35, SpriteEffects.None, 0);

                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                        Texture2D Eye = (Texture2D)TextureAssets.Npc[npc.type];
                        Vector2 vector = new Vector2(npc.width / 2, npc.height / 2) + (npc.rotation + 1.57f).ToRotationVector2().PerfectNormalize() * -14;
                        Rectangle rectangle = npc.frame;
                        if (npc.ai[1] < 256)
                        {
                            rectangle.Y += Eye.Height / 6 - (int)(npc.ai[1] - 90);
                        }
                        rectangle.Height = (int)(npc.ai[1] - 90);

                        if (rectangle.Height > Eye.Height / 6)
                        {
                            rectangle.Height = Eye.Height / 6;
                        }
                        spriteBatch.Draw(Eye, npc.position + vector - screenPos, new Rectangle?(rectangle), npc.GetAlpha(drawColor), npc.rotation, new Vector2(Eye.Width, Eye.Height / 6) / 2, npc.scale, (SpriteEffects)(-npc.spriteDirection), 0f);
                    }
                    return false;
                }
                if (npc.Dnpc().Bool[3])
                {
                    Texture2D texture = (Texture2D)TextureAssets.Npc[npc.type];
                    Vector2 vector = new Vector2(npc.width / 2, npc.height / 2) + (npc.rotation - 1.57f).ToRotationVector2().PerfectNormalize() * (24 * npc.scale);
                    spriteBatch.Draw(texture, npc.position + vector - screenPos, new Rectangle?(npc.frame), npc.GetAlpha(drawColor), npc.rotation, new Vector2(texture.Width, texture.Height / 6) / 2, npc.scale, (SpriteEffects)(-npc.spriteDirection), 0f);
                    return false;
                }
                if (Main.masterMode && false)
                {
                    Texture2D texture = (Texture2D)TextureAssets.Npc[npc.type];
                    Vector2 vector = new Vector2(npc.width / 2, npc.height / 2) + (npc.rotation - 1.57f).ToRotationVector2().PerfectNormalize() * (24 * npc.scale);

                    for (int i = 0; i < npc.oldRot.Length; i++)
                    {
                        Vector2 vector2 = npc.oldPos[i] + vector - screenPos;
                        Color color = npc.GetAlpha(new Color(200, 0, 0)) * ((npc.oldPos.Length - i) / (float)npc.oldPos.Length / 4f);
                        spriteBatch.Draw(texture, vector2, new Rectangle?(npc.frame), color, npc.rotation, new Vector2(texture.Width, texture.Height / 6) / 2, npc.scale * 1.1F, (SpriteEffects)(-npc.spriteDirection), 0f);
                    }
                    spriteBatch.Draw(texture, npc.position + vector - screenPos, new Rectangle?(npc.frame), npc.GetAlpha(drawColor), npc.rotation, new Vector2(texture.Width, texture.Height / 6) / 2, npc.scale, (SpriteEffects)(-npc.spriteDirection), 0f);
                    return false;
                }
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
            }
        }
    }
}