using DDmod.Content.NPCs.IittleMonster;
using DDmod.NoContent.Config;
using Microsoft.Build.Tasks;
using Terraria;
using Terraria.ModLoader.IO;

namespace DDmod.Content.NPCs.BossAI
{
    //史莱姆王
    public class KingSlime : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool Appearance;
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.KingSlime)
            {
                if (Main.masterMode && false)
                {
                    NPCID.Sets.TrailingMode[npc.type] = 5;
                    NPCID.Sets.TrailCacheLength[npc.type] = 10;
                }
                if (ModContent.GetInstance<DDConfigServer>().BossAnimation)
                {
                    npc.Dnpc().Deathrattle = true;
                    npc.dontTakeDamage = true;
                    npc.scale = 0.01F;
                }
            }
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (npc.type == NPCID.KingSlime)
            {
                if (npc.Dnpc().Deathrattle)
                {
                    if (npc.life <= 0)
                    {
                        npc.life = 100;
                        npc.Dnpc().Bool[4] = true;
                        npc.dontTakeDamage = true;
                    }
                }
            }
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            BinaryWriter packet = binaryWriter;
            for (int a = 0; a < 255; a++)
            {
                packet.Write(Players[a]);
            }
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            for (int a = 0; a < 255; a++)
            {
                Players[a] = binaryReader.ReadInt32();
            }
        }

        public override bool PreAI(NPC npc)
        {
            //NPC.downedPlantBoss = false;
            if (npc.type == NPCID.KingSlime)
            {
                if (npc.Dnpc().Bool[4])
                {
                    npc.dontTakeDamage = true;
                    if (npc.Dnpc().Times[4] % 0.1 < 0.05F)
                    {
                        npc.scale += 0.03f;
                    }
                    else
                    {
                        npc.scale -= 0.03f;
                    }
                    if (npc.Dnpc().Times[4] > 0.9F)
                    {
                        npc.scale += 0.1f;
                        npc.alpha += 10;
                    }
                    npc.velocity = new Vector2(0, 10);
                    npc.Dnpc().Times[4] += 0.005f;
                    if (CountNPCS(50) == 1)
                    {
                        Main.LocalPlayer.Dplayer().Bossperspective(npc.Center, 120, false, 0.2f);
                        DDOn.DDmodOn.Start = 5;
                    }
                    if (Main.player[npc.target].dead && Main.hardMode)
                    {
                        npc.active = false;
                    }
                    if (npc.Dnpc().Times[4] > 1)
                    {
                        Main.LocalPlayer.Dplayer().PlayerShake(60, 20);
                        SoundStyle sound = SoundID.Roar;
                        sound.Pitch = -0.5f;
                        Reload();
                        PlaySound(sound, npc.position);

                        npc.Dnpc().Deathrattle = false;
                        npc.SimpleStrikeNPC(9999, 0,false,0);
                        for (int i = 0; i < 1000; i++)
                        {
                            int D = NewDust(npc.position, npc.width, npc.height, 4, 0f, 0f, 100, new Color(90, 150, 255, 55), 1f);
                            Main.dust[D].noGravity = true;
                            Main.dust[D].scale *= 1f + Main.rand.Next(5);
                            Vector2 vector = Utils.RotatedBy(new Vector2(0, Main.rand.NextFloat(-8, -3)), Main.rand.NextFloat(-1.57f, 1.57f), default);
                            Main.dust[D].velocity = vector * 6;
                        }
                    }
                    npc.velocity *= 0.98f;
                    return false;
                }
                if (!npc.Dnpc().Bool[0] && ModContent.GetInstance<DDConfigServer>().BossAnimation)
                {
                    double num256 = npc.life / (double)npc.lifeMax;
                    if (npc.scale >= (float)num256 + 0.5f)
                    {
                        SoundStyle sound = SoundID.Roar;
                        sound.Pitch = -0.5f;

                        Reload();
                        PlaySound(sound, npc.position);

                        Main.LocalPlayer.Dplayer().PlayerShake(60,20);
                        npc.Dnpc().Bool[0] = true;
                    }
                    else
                    {
                        Main.LocalPlayer.Dplayer().Bossperspective(npc.Center, 120, false, 0.2f);
                        npc.scale += 0.01f;
                        if (num256 != npc.scale)
                        {
                            ref float ptr = ref npc.position.X;
                            ptr += (npc.width / 2);
                            ptr = ref npc.position.Y;
                            ptr += npc.height;
                            npc.width = (int)(152f * npc.scale);
                            npc.height = (int)(92f * npc.scale);
                            ptr = ref npc.position.X;
                            ptr -= (npc.width / 2);
                            ptr = ref npc.position.Y;
                            ptr -= npc.height;
                        }
                        for (int num251 = 0; num251 < 10; num251++)
                        {
                            Dust dust = Main.dust[NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f)];
                            dust.noGravity = true;
                            dust.velocity *= 0.5f;
                        }
                    }
                    return false;
                }
                if (Main.masterMode && false)
                {
                    if (npc.Dnpc().Bool[4] && !Main.hardMode)
                    {
                        slime(npc);
                    }
                    else
                    {
                        slime(npc);
                    }
                    for (int a = 0; a < 255; a++)
                    {
                        Player player = Main.player[a];
                        if (player.active && !player.dead)
                        {
                            if (Players[a] != 0)
                            {
                                player.velocity = Vector2.Zero;
                                player.position = npc.Center - new Vector2(player.width / 2, player.height / 2);
                                player.immune = true;
                                player.immuneAlpha = (int)(128 * (1 - (float)player.statLife / player.statLifeMax2)) + 127;
                                player.wingTimeMax = 0;
                                //player.immuneAlphaDirection = 25;
                                player.immuneTime = 2;
                                player.Dplayer().ForbiddenToAttack = 5;
                                player.fullRotation = player.velocity.X * 0.05f;
                                //StartSync(player, npc);
                            }
                        }
                    }
                    if (!Appearance)
                    {
                        Appearance = true;
                        Main.slimeRainTime = 60;
                    }
                    return false;
                }
            }
            return true;
        }
        public int[] Players = new int[255];
        public int[] PlayersTime = new int[255];
        //肉前史莱姆王AI
        public void slime(NPC npc)
        {
            Player player = Main.player[npc.target];
            float num239 = 1f;
            bool flag8 = false;
            bool flag9 = false;
            npc.aiAction = 0;
            if (npc.ai[3] == 0f && npc.life > 0)
            {
                npc.ai[3] = npc.lifeMax;
            }
            if (npc.localAI[3] == 0f && Main.netMode != 1)
            {
                npc.ai[0] = -100f;
                npc.localAI[3] = 1f;
                npc.TargetClosest(true);
                npc.netUpdate = true;
            }
            int num240 = 500;
            if (Main.player[npc.target].dead || Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) / 16f > num240)
            {
                npc.TargetClosest(true);
                if (Main.player[npc.target].dead || Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) / 16f > num240)
                {
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                    {
                        npc.direction = 1;
                    }
                    else
                    {
                        npc.direction = -1;
                    }
                    if (npc.ai[1] < 5f)
                    {
                        npc.ai[0] = 0;
                    }
                    npc.ai[1] = 5;
                    if (npc.ai[0] >= 55)
                    {
                        npc.active = false;
                    }
                }
            }
            if (!Main.player[npc.target].dead && npc.timeLeft > 10 && npc.ai[2] >= 300f && npc.ai[1] < 5f && npc.velocity.Y == 0f)
            {
                npc.ai[2] = 0f;
                npc.ai[0] = 0f;
                npc.ai[1] = 5f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.TargetClosest(false);
                    Point point3 = npc.Center.ToTileCoordinates();
                    Point point4 = Main.player[npc.target].Center.ToTileCoordinates();
                    Vector2 vector30 = Main.player[npc.target].Center - npc.Center;
                    int num241 = 10;
                    int num242 = 0;
                    int num243 = 7;
                    int num244 = 0;
                    bool flag10 = false;
                    if (npc.localAI[0] >= 360f || vector30.Length() > 2000f)
                    {
                        if (npc.localAI[0] >= 360f)
                        {
                            npc.localAI[0] = 360f;
                        }
                        flag10 = true;
                        num244 = 100;
                    }
                    while (!flag10 && num244 < 100)
                    {
                        num244++;
                        int num245 = Main.rand.Next(point4.X - num241, point4.X + num241 + 1);
                        int num246 = Main.rand.Next(point4.Y - num241, point4.Y + 1);
                        if ((num246 < point4.Y - num243 || num246 > point4.Y + num243 || num245 < point4.X - num243 || num245 > point4.X + num243) && (num246 < point3.Y - num242 || num246 > point3.Y + num242 || num245 < point3.X - num242 || num245 > point3.X + num242) && !Main.tile[num245, num246].HasUnactuatedTile)
                        {
                            int num247 = num246;
                            int num248 = 0;
                            bool flag11 = Main.tile[num245, num247].HasUnactuatedTile && Main.tileSolid[Main.tile[num245, num247].TileType] && !Main.tileSolidTop[Main.tile[num245, num247].TileType];
                            if (flag11)
                            {
                                num248 = 1;
                            }
                            else
                            {
                                while (num248 < 150 && num247 + num248 < Main.maxTilesY)
                                {
                                    int num249 = num247 + num248;
                                    bool flag12 = Main.tile[num245, num249].HasUnactuatedTile && Main.tileSolid[Main.tile[num245, num249].TileType] && !Main.tileSolidTop[Main.tile[num245, num249].TileType];
                                    if (flag12)
                                    {
                                        num248--;
                                        break;
                                    }
                                    int num = num248;
                                    num248 = num + 1;
                                }
                            }
                            num246 += num248;
                            bool flag13 = true;
                            if (flag13 && Main.tile[num245, num246].LiquidType == 1)
                            {
                                flag13 = false;
                            }
                            if (flag13 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                            {
                                flag13 = false;
                            }
                            if (flag13)
                            {
                                npc.localAI[1] = (num245 * 16 + 8);
                                npc.localAI[2] = (num246 * 16 + 16);
                                break;
                            }
                        }
                    }
                    if (num244 >= 100)
                    {
                        Vector2 bottom = Main.player[npc.target].Center;//Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].Bottom;
                        npc.localAI[1] = bottom.X;
                        npc.localAI[2] = bottom.Y;
                    }
                }
            }
            if (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) || Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 160f)
            {
                ref float ptr = ref npc.ai[2];
                ref float ptr2 = ref ptr;
                float num250 = ptr;
                ptr2 = num250 + 1f;
                if (Main.netMode != 1)
                {
                    ptr = ref npc.localAI[0];
                    ref float ptr3 = ref ptr;
                    num250 = ptr;
                    ptr3 = num250 + 1f;
                }
            }
            else if (Main.netMode != 1)
            {
                ref float ptr = ref npc.localAI[0];
                ref float ptr4 = ref ptr;
                float num250 = ptr;
                ptr4 = num250 - 1f;
                if (npc.localAI[0] < 0f)
                {
                    npc.localAI[0] = 0f;
                }
            }
            if (npc.timeLeft < 10 && (npc.ai[0] != 0f || npc.ai[1] != 0f))
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
                flag8 = false;
            }
            Dust dust;
            if (npc.ai[1] == 5f)
            {
                flag8 = true;
                npc.aiAction = 1;
                ref float ptr = ref npc.ai[0];
                ref float ptr5 = ref ptr;
                float num250 = ptr;
                ptr5 = num250 + 1f;
                num239 = MathHelper.Clamp((60f - npc.ai[0]) / 60f, 0f, 1f);
                num239 = 0.5f + num239 * 0.5f;
                if (npc.ai[0] >= 60f)
                {
                    flag9 = true;
                }
                if (npc.ai[0] == 60f)
                {
                    Gore.NewGore(npc.GetSource_FromAI(), npc.Center + new Vector2(-40f, (-npc.height / 2)), npc.velocity, 734, 1f);
                }
                if (npc.ai[0] >= 60f && Main.netMode != 1)
                {
                    npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
                    npc.ai[1] = 6f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                }
                if (Main.netMode == 1 && npc.ai[0] >= 120f)
                {
                    npc.ai[1] = 6f;
                    npc.ai[0] = 0f;
                }
                if (!flag9)
                {
                    int num;
                    for (int num251 = 0; num251 < 10; num251 = num + 1)
                    {
                        int num252 = NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                        Main.dust[num252].noGravity = true;
                        dust = Main.dust[num252];
                        dust.velocity *= 0.5f;
                        num = num251;
                    }
                }
            }
            else if (npc.ai[1] == 6f)
            {
                flag8 = true;
                npc.aiAction = 0;
                ref float ptr = ref npc.ai[0];
                ref float ptr6 = ref ptr;
                float num250 = ptr;
                ptr6 = num250 + 1f;
                num239 = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
                num239 = 0.5f + num239 * 0.5f;
                if (npc.ai[0] >= 30f && Main.netMode != 1)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                    npc.TargetClosest(true);
                }
                if (Main.netMode == 1 && npc.ai[0] >= 60f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.TargetClosest(true);
                }
                int num;
                for (int num253 = 0; num253 < 10; num253 = num + 1)
                {
                    int num254 = NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                    Main.dust[num254].noGravity = true;
                    dust = Main.dust[num254];
                    dust.velocity *= 2f;
                    num = num253;
                }
            }
            npc.dontTakeDamage = (npc.hide = flag9);

            if (npc.velocity.Y == 0f)
            {
                if (npc.Dnpc().Times[1] == 0 && npc.ai[1] == 0f)
                {
                    int damage = 15;
                    for (int i = -6; i <= 6; i++)
                    {
                        float Rand = 0;
                        if (i < 0)
                        {
                            Rand = Main.rand.NextFloat(-1F, 0f);
                        }
                        else
                        {
                            Rand = Main.rand.NextFloat(0F, 1f);
                        }
                        Vector2 vector = Utils.RotatedBy(new Vector2(0, -1), Rand, default);
                        int H = NewProjectile(npc.GetSource_FromAI(), npc.Center + vector * npc.height / 2, vector * 4, ProjectileID.SpikedSlimeSpike, damage, 1f, 0);
                        Main.projectile[H].velocity *= 3;
                        float A = npc.scale * 2;
                        if (A > 4)
                        {
                            A = 4;
                        }
                        Main.projectile[H].scale = Main.projectile[H].scale * A;
                        Main.projectile[H].height = (int)(Main.projectile[H].height * A);
                        Main.projectile[H].width = (int)(Main.projectile[H].width * A);
                    }
                    npc.Dnpc().Times[1] = 1;
                }
                ref float ptr = ref npc.velocity.X;
                ptr *= 0.8f;
                if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                {
                    npc.velocity.X = 0f;
                }
                if (!flag8)
                {
                    ptr = ref npc.ai[0];
                    ptr += 2f;
                    if (npc.life < npc.lifeMax * 0.8)
                    {
                        ptr = ref npc.ai[0];
                        ptr += 1f;
                    }
                    if (npc.life < npc.lifeMax * 0.6)
                    {
                        ptr = ref npc.ai[0];
                        ptr += 1f;
                    }
                    if (npc.life < npc.lifeMax * 0.4)
                    {
                        ptr = ref npc.ai[0];
                        ptr += 2f;
                    }
                    if (npc.life < npc.lifeMax * 0.2)
                    {
                        ptr = ref npc.ai[0];
                        ptr += 3f;
                    }
                    if (npc.life < npc.lifeMax * 0.1)
                    {
                        ptr = ref npc.ai[0];
                        ptr += 4f;
                    }
                    if (npc.ai[0] >= 0f)
                    {
                        npc.netUpdate = true;
                        npc.TargetClosest(true);
                        if (npc.ai[1] == 3f)
                        {
                            npc.velocity.Y = -30f;
                            ptr = ref npc.velocity.X;
                            ptr += 25f * npc.direction;
                            npc.ai[0] = -200f;
                            npc.ai[1] = 0f;
                        }
                        else if (npc.ai[1] == 2f)
                        {
                            npc.velocity.Y = -7.5f;
                            ptr = ref npc.velocity.X;
                            ptr += 6f * npc.direction;
                            npc.ai[0] = -120f;
                            ptr = ref npc.ai[1];
                            ptr += 1f;
                        }
                        else
                        {
                            npc.velocity.Y = -10f;
                            ptr = ref npc.velocity.X;
                            ptr += 7f * npc.direction;
                            npc.ai[0] = -120f;
                            ptr = ref npc.ai[1];
                            ptr += 1f;
                        }
                    }
                    else if (npc.ai[0] >= -30f)
                    {
                        npc.aiAction = 1;
                    }
                }
            }
            else if (npc.target < 255 && ((npc.direction == 1 && npc.velocity.X < 3f) || (npc.direction == -1 && npc.velocity.X > -3f)))
            {
                npc.Dnpc().Times[1] = 0;
                if ((npc.direction == -1 && npc.velocity.X < 0.1) || (npc.direction == 1 && npc.velocity.X > -0.1))
                {
                    ref float ptr = ref npc.velocity.X;
                    ptr += 0.2f * npc.direction;
                }
                else
                {
                    ref float ptr = ref npc.velocity.X;
                    ptr *= 0.93f;
                }
            }
            else npc.Dnpc().Times[1] = 0;

            int num255 = NewDust(npc.position, npc.width, npc.height, 4, npc.velocity.X, npc.velocity.Y, 255, new Color(0, 80, 255, 80), npc.scale * 1.2f);
            Main.dust[num255].noGravity = true;
            dust = Main.dust[num255];
            dust.velocity *= 0.5f;
            Scale(npc, num239);
            npc.Dnpc().Times[0]++;
            if (npc.life >= npc.lifeMax / 2)
            {
                if (npc.Dnpc().Times[0] % 1200 == 1)
                {
                    int A = NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<FlyingBlueSlime>());
                    Main.npc[A].SetDefaults(ModContent.NPCType<FlyingBlueSlime>());
                    if (Main.netMode > 0 && A < 200)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, A, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            float center = npc.Center.X - Main.player[npc.target].Center.X;
            if (center < 0) center = -center;
            if (center < 16F && npc.ai[1] == 0f)
            {
                a = true;
            }
            else if (npc.ai[1] != 0)
            {
                npc.noGravity = false;
                a = false;
            }
            if (a)
            {
                npc.velocity.X *= 0.9f;
                npc.velocity.Y = 27f;
                npc.noGravity = true;
            }
            else
            {
                npc.noGravity = false;
            }
            for (int a = 0; a < 255; a++)
            {
                Player player1 = Main.player[a];
                if (player1.controlJump)
                {
                    if (Players[a] == 1)
                    {
                        player.immune = true;
                        player.immuneTime = 120;
                        Vector2 vector = Utils.RotatedBy(new Vector2(0, -1), Main.rand.NextFloat(-1f, 1f), default);
                        player.velocity = vector * 30f;
                    }
                    if (b[a])
                    {
                        Players[a]--;
                        b[a] = false;
                    }
                }
                else
                {
                    if (!b[a])
                    {
                        b[a] = true;
                    }
                }
                if (Players[a] > 0 && player1.whoAmI == a)
                {
                    player1.lifeRegenCount = 0;
                    PlayersTime[a]++;
                    if (npc.Dnpc().Times[0] % 5 == 0)
                    {
                        if (player1.statLife > 0)
                        {
                            player1.statLife -= 1 + PlayersTime[a] / 180;
                        }
                        else
                        {
                            player1.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.DDmod.PlayerKill.Kill4", player.name)), 1, 0, false);
                        }
                    }
                }
                else
                {
                    Players[a] = 0;
                    PlayersTime[a] = 0;
                }

            }
        }

        //肉后史莱姆王AI
        public void slime2(NPC npc)
        {
            Player player = Main.player[npc.target];
            float num239 = 1f;
            bool flag8 = false;
            bool flag9 = false;
            npc.aiAction = 0;
            if (npc.ai[3] == 0f && npc.life > 0)
            {
                npc.ai[3] = npc.lifeMax;
            }
            if (npc.localAI[3] == 0f && Main.netMode != 1)
            {
                npc.ai[0] = -100f;
                npc.localAI[3] = 1f;
                npc.TargetClosest(true);
                npc.netUpdate = true;
            }
            int num240 = 500;
            if (Main.player[npc.target].dead || Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) / 16f > num240)
            {
                npc.TargetClosest(true);
                if (Main.player[npc.target].dead || Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) / 16f > num240)
                {
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 10;
                    }
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                    {
                        npc.direction = 1;
                    }
                    else
                    {
                        npc.direction = -1;
                    }
                    if (npc.ai[1] < 5f)
                    {
                        npc.ai[0] = 0;
                    }
                    npc.ai[1] = 5;
                    if (npc.ai[0] >= 55)
                    {
                        npc.active = false;
                    }
                }
            }
            if (!Main.player[npc.target].dead && npc.timeLeft > 10 && npc.ai[2] >= 300f && npc.ai[1] < 5f && npc.velocity.Y == 0f)
            {
                npc.ai[2] = 0f;
                npc.ai[0] = 0f;
                npc.ai[1] = 5f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.TargetClosest(false);
                    Point point3 = npc.Center.ToTileCoordinates();
                    Point point4 = Main.player[npc.target].Center.ToTileCoordinates();
                    Vector2 vector30 = Main.player[npc.target].Center - npc.Center;
                    int num241 = 10;
                    int num242 = 0;
                    int num243 = 7;
                    int num244 = 0;
                    bool flag10 = false;
                    if (npc.localAI[0] >= 360f || vector30.Length() > 2000f)
                    {
                        if (npc.localAI[0] >= 360f)
                        {
                            npc.localAI[0] = 360f;
                        }
                        flag10 = true;
                        num244 = 100;
                    }
                    while (!flag10 && num244 < 100)
                    {
                        num244++;
                        int num245 = Main.rand.Next(point4.X - num241, point4.X + num241 + 1);
                        int num246 = Main.rand.Next(point4.Y - num241, point4.Y + 1);
                        if ((num246 < point4.Y - num243 || num246 > point4.Y + num243 || num245 < point4.X - num243 || num245 > point4.X + num243) && (num246 < point3.Y - num242 || num246 > point3.Y + num242 || num245 < point3.X - num242 || num245 > point3.X + num242) && !Main.tile[num245, num246].HasUnactuatedTile)
                        {
                            int num247 = num246;
                            int num248 = 0;
                            bool flag11 = Main.tile[num245, num247].HasUnactuatedTile && Main.tileSolid[Main.tile[num245, num247].TileType] && !Main.tileSolidTop[Main.tile[num245, num247].TileType];
                            if (flag11)
                            {
                                num248 = 1;
                            }
                            else
                            {
                                while (num248 < 150 && num247 + num248 < Main.maxTilesY)
                                {
                                    int num249 = num247 + num248;
                                    bool flag12 = Main.tile[num245, num249].HasUnactuatedTile && Main.tileSolid[Main.tile[num245, num249].TileType] && !Main.tileSolidTop[Main.tile[num245, num249].TileType];
                                    if (flag12)
                                    {
                                        num248--;
                                        break;
                                    }
                                    int num = num248;
                                    num248 = num + 1;
                                }
                            }
                            num246 += num248;
                            bool flag13 = true;
                            if (flag13 && Main.tile[num245, num246].LiquidType == 1)
                            {
                                flag13 = false;
                            }
                            if (flag13 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                            {
                                flag13 = false;
                            }
                            if (flag13)
                            {
                                npc.localAI[1] = (num245 * 16 + 8);
                                npc.localAI[2] = (num246 * 16 + 16);
                                break;
                            }
                        }
                    }
                    if (num244 >= 100)
                    {
                        Vector2 bottom = Main.player[npc.target].Center;//Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].Bottom;
                        npc.localAI[1] = bottom.X;
                        npc.localAI[2] = bottom.Y;
                    }
                }
            }
            if (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) || Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 160f)
            {
                ref float ptr = ref npc.ai[2];
                ref float ptr2 = ref ptr;
                float num250 = ptr;
                ptr2 = num250 + 1f;
                if (Main.netMode != 1)
                {
                    ptr = ref npc.localAI[0];
                    ref float ptr3 = ref ptr;
                    num250 = ptr;
                    ptr3 = num250 + 1f;
                }
            }
            else if (Main.netMode != 1)
            {
                ref float ptr = ref npc.localAI[0];
                ref float ptr4 = ref ptr;
                float num250 = ptr;
                ptr4 = num250 - 1f;
                if (npc.localAI[0] < 0f)
                {
                    npc.localAI[0] = 0f;
                }
            }
            if (npc.timeLeft < 10 && (npc.ai[0] != 0f || npc.ai[1] != 0f))
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
                flag8 = false;
            }
            Dust dust;
            if (npc.ai[1] == 5f)
            {
                flag8 = true;
                npc.aiAction = 1;
                ref float ptr = ref npc.ai[0];
                ref float ptr5 = ref ptr;
                float num250 = ptr;
                ptr5 = num250 + 1f;
                num239 = MathHelper.Clamp((60f - npc.ai[0]) / 60f, 0f, 1f);
                num239 = 0.5f + num239 * 0.5f;
                if (npc.ai[0] >= 60f)
                {
                    flag9 = true;
                }
                if (npc.ai[0] == 60f)
                {
                    Gore.NewGore(npc.GetSource_FromAI(), npc.Center + new Vector2(-40f, (-npc.height / 2)), npc.velocity, 734, 1f);
                }
                if (npc.ai[0] >= 60f && Main.netMode != 1)
                {
                    npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
                    npc.ai[1] = 6f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                }
                if (Main.netMode == 1 && npc.ai[0] >= 120f)
                {
                    npc.ai[1] = 6f;
                    npc.ai[0] = 0f;
                }
                if (!flag9)
                {
                    int num;
                    for (int num251 = 0; num251 < 10; num251 = num + 1)
                    {
                        int num252 = NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                        Main.dust[num252].noGravity = true;
                        dust = Main.dust[num252];
                        dust.velocity *= 0.5f;
                        num = num251;
                    }
                }
            }
            else if (npc.ai[1] == 6f)
            {
                flag8 = true;
                npc.aiAction = 0;
                ref float ptr = ref npc.ai[0];
                ref float ptr6 = ref ptr;
                float num250 = ptr;
                ptr6 = num250 + 1f;
                num239 = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
                num239 = 0.5f + num239 * 0.5f;
                if (npc.ai[0] >= 30f && Main.netMode != 1)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                    npc.TargetClosest(true);
                }
                if (Main.netMode == 1 && npc.ai[0] >= 60f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.TargetClosest(true);
                }
                int num;
                for (int num253 = 0; num253 < 10; num253 = num + 1)
                {
                    int num254 = NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                    Main.dust[num254].noGravity = true;
                    dust = Main.dust[num254];
                    dust.velocity *= 2f;
                    num = num253;
                }
            }
            npc.dontTakeDamage = (npc.hide = flag9);

            if (npc.velocity.Y == 0f)
            {
                ref float ptr = ref npc.velocity.X;
                ptr *= 0.8f;
                if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                {
                    npc.velocity.X = 0f;
                }
                if (!flag8)
                {
                    ptr = ref npc.ai[0];
                    ptr += 2f;
                    if (npc.life < npc.lifeMax * 0.8)
                    {
                        ptr = ref npc.ai[0];
                        ptr += 1f;
                    }
                    if (npc.life < npc.lifeMax * 0.6)
                    {
                        ptr = ref npc.ai[0];
                        ptr += 1f;
                    }
                    if (npc.life < npc.lifeMax * 0.4)
                    {
                        ptr = ref npc.ai[0];
                        ptr += 2f;
                    }
                    if (npc.life < npc.lifeMax * 0.2)
                    {
                        ptr = ref npc.ai[0];
                        ptr += 3f;
                    }
                    if (npc.life < npc.lifeMax * 0.1)
                    {
                        ptr = ref npc.ai[0];
                        ptr += 4f;
                    }
                    if (npc.ai[0] >= 0f)
                    {
                        npc.netUpdate = true;
                        npc.TargetClosest(true);
                        if (npc.ai[1] == 3f)
                        {
                            npc.velocity.Y = -30f;
                            ptr = ref npc.velocity.X;
                            ptr += 25f * npc.direction;
                            npc.ai[0] = -200f;
                            npc.ai[1] = 0f;
                        }
                        else if (npc.ai[1] == 2f)
                        {
                            npc.velocity.Y = -7.5f;
                            ptr = ref npc.velocity.X;
                            ptr += 6f * npc.direction;
                            npc.ai[0] = -120f;
                            ptr = ref npc.ai[1];
                            ptr += 1f;
                        }
                        else
                        {
                            npc.velocity.Y = -10f;
                            ptr = ref npc.velocity.X;
                            ptr += 7f * npc.direction;
                            npc.ai[0] = -120f;
                            ptr = ref npc.ai[1];
                            ptr += 1f;
                        }
                    }
                    else if (npc.ai[0] >= -30f)
                    {
                        npc.aiAction = 1;
                    }
                }
            }
            else if (npc.target < 255 && ((npc.direction == 1 && npc.velocity.X < 3f) || (npc.direction == -1 && npc.velocity.X > -3f)))
            {
                npc.Dnpc().Times[1] = 0;
                if ((npc.direction == -1 && npc.velocity.X < 0.1) || (npc.direction == 1 && npc.velocity.X > -0.1))
                {
                    ref float ptr = ref npc.velocity.X;
                    ptr += 0.2f * npc.direction;
                }
                else
                {
                    ref float ptr = ref npc.velocity.X;
                    ptr *= 0.93f;
                }
            }
            else npc.Dnpc().Times[1] = 0;

            int num255 = NewDust(npc.position, npc.width, npc.height, 4, npc.velocity.X, npc.velocity.Y, 255, new Color(0, 80, 255, 80), npc.scale * 1.2f);
            Main.dust[num255].noGravity = true;
            dust = Main.dust[num255];
            dust.velocity *= 0.5f;
            Scale(npc, num239);
            Scale(npc, num239);
            npc.Dnpc().Times[0]++;
            if (npc.life >= npc.lifeMax / 2)
            {
                if (npc.Dnpc().Times[0] % 1200 == 1)
                {
                    int A = NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<FlyingBlueSlime>());
                    Main.npc[A].SetDefaults(ModContent.NPCType<FlyingBlueSlime>());
                    if (Main.netMode > 0 && A < 200)
                    {
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, A, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            float center = npc.Center.X - Main.player[npc.target].Center.X;
            if (npc.ai[1] == 3)
            {
                if (center > 0)
                {
                    npc.Dnpc().Times[3] = 1;
                }
                else
                {
                    npc.Dnpc().Times[3] = -1;
                }
            }
            center = npc.Center.X - Main.player[npc.target].Center.X + 300 * npc.Dnpc().Times[3];
            if (center < 0) center = -center;
            if (center < 16F && npc.ai[1] == 0f)
            {
                a = true;
            }
            else if (npc.ai[1] != 0)
            {
                npc.noGravity = false;
                a = false;
            }
            if (a)
            {
                npc.velocity.X = 0;
                npc.velocity.Y = 27f;
                npc.noGravity = true;
            }
            else
            {
                npc.noGravity = false;
            }
            for (int a = 0; a < 255; a++)
            {
                Player player1 = Main.player[a];
                if (player1.controlUseItem)
                {
                    if (Players[a] == 1)
                    {
                        player.immune = true;
                        player.immuneTime = 120;
                        Vector2 vector = Utils.RotatedBy(new Vector2(0, -1), Main.rand.NextFloat(-1f, 1f), default);
                        player.velocity = vector * 30f;
                    }
                    if (b[a])
                    {
                        Players[a]--;
                        b[a] = false;
                    }
                }
                else
                {
                    if (!b[a])
                    {
                        b[a] = true;
                    }
                }
                if (Players[a] > 0 && player1.whoAmI == a)
                {
                    player1.lifeRegenCount = 0;
                    PlayersTime[a]++;
                    if (npc.Dnpc().Times[0] % 2 == 0)
                    {
                        if (player1.statLife > 0 && PlayersTime[a] < 1800)
                        {
                            player1.statLife--;
                        }
                        else
                        {
                            player1.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.DDmod.PlayerKill.Kill4", player.name)), 1, 0, false);
                        }
                    }
                }
                else
                {
                    Players[a] = 0;
                    PlayersTime[a] = 0;
                }
            }
            if (npc.life <= npc.lifeMax / 2 && Split < 2)
            {
                if (Main.netMode != 1)
                {
                    for (int A = -1; A < 2; A++)
                    {
                        Vector2 vector = npc.Center + new Vector2(100 * A, 0);
                        int num261 = NewNPC(npc.GetSource_FromAI(), (int)vector.X, (int)vector.Y, 50, 0, 0f, 0f, 0f, 0f, 255);
                        //Main.npc[num261].lifeMax /= 2;
                        //Main.npc[num261].life /= 2;
                        Main.npc[num261].netUpdate = true;
                        Main.npc[num261].GetGlobalNPC<KingSlime>().Split = Split + 1;
                        if (Main.netMode == 2 && num261 < 200)
                        {
                            NetMessage.SendData(23, -1, -1, null, num261, 0f, 0f, 0f, 0, 0, 0);
                        }
                        npc.active = false;
                    }
                }
            }
        }
        bool a;
        public bool[] b = new bool[255];

        public int Split;
        public void Scale(NPC npc, float num239)
        {
            if (npc.life > 0)
            {
                double num256 = npc.life / (double)npc.lifeMax;
                if (Split == 0)
                {
                    //if (Main.hardMode) num256 = npc.life / (double)npc.lifeMax * 8;
                }
                if (Split == 1)
                {
                    //if (Main.hardMode) num256 = npc.life / (double)npc.lifeMax * 2;
                }
                if (Split == 2)
                {
                    //if (Main.hardMode) num256 = npc.life / (double)npc.lifeMax * 0.5;
                }
                num256 *= num239;
                if (num256 != npc.scale)
                {
                    ref float ptr = ref npc.position.X;
                    ptr += (npc.width / 2);
                    ptr = ref npc.position.Y;
                    ptr += npc.height;
                    npc.scale = (float)num256 + 0.5f;
                    npc.width = (int)(152f * npc.scale);
                    npc.height = (int)(92f * npc.scale);
                    ptr = ref npc.position.X;
                    ptr -= (npc.width / 2);
                    ptr = ref npc.position.Y;
                    ptr -= npc.height;
                }
                if (Main.netMode != 1)
                {
                    int num257 = (int)(npc.lifeMax * 0.05);
                    if ((npc.life + num257) < npc.ai[3])
                    {
                        npc.ai[3] = npc.life;
                        int num258 = Main.rand.Next(1, 4);
                        int num;
                        for (int num259 = 0; num259 < num258; num259 = num + 1)
                        {
                            int x = (int)(npc.position.X + Main.rand.Next(npc.width - 32));
                            int y = (int)(npc.position.Y + Main.rand.Next(npc.height - 32));
                            int num260 = 1;
                            if (Main.expertMode && Main.rand.NextBool(4))
                            {
                                num260 = 535;
                            }
                            int num261 = NewNPC(npc.GetSource_FromAI(), x, y, num260, 0, 0f, 0f, 0f, 0f, 255);
                            Main.npc[num261].SetDefaults(num260);
                            Main.npc[num261].velocity.X = Main.rand.Next(-15, 16) * 0.1f;
                            Main.npc[num261].velocity.Y = Main.rand.Next(-30, 1) * 0.1f;
                            Main.npc[num261].ai[0] = (-1000 * Main.rand.Next(3));
                            Main.npc[num261].ai[1] = 0f;
                            if (Main.netMode == 2 && num261 < 200)
                            {
                                NetMessage.SendData(23, -1, -1, null, num261, 0f, 0f, 0f, 0, 0, 0);
                            }
                            num = num259;
                        }
                    }
                }
            }
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (npc.type == NPCID.KingSlime)
            {
                if (a)
                {
                    Players[target.whoAmI] = 30;
                }
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (npc.type == NPCID.KingSlime)
            {
                if (Main.hardMode)
                {
                    /*if (proj.active && proj.friendly && !proj.hostile && Main.rand.Next((int)((float)npc.life / (float)npc.lifeMax * 100)) == 0 && !proj.minion)
					{
						Player player = Main.player[(int)Player.FindClosest(proj.Center, 1, 1)];
						proj.netUpdate = true;
						proj.penetrate = 2;
						proj.hostile = true;
						proj.friendly = false;
						if (proj.velocity != Vector2.Zero || player.heldProj != proj.whoAmI)
						{
							proj.velocity *= -1;
						}
					}*/
                }
            }
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.KingSlime)
            {
                Vector2 vector = new Vector2(npc.width / 2, npc.height / 2 - (npc.scale * 12));

                if (npc.velocity != Vector2.Zero)
                {
                    for (int i = 0; i < npc.oldPos.Length; i++)
                    {
                        Vector2 vector2 = npc.oldPos[i] + vector - Main.screenPosition;
                        Color color = npc.GetAlpha(drawColor) * ((npc.oldPos.Length - i) / (float)npc.oldPos.Length / 2f);
                        spriteBatch.Draw((Texture2D)TextureAssets.Npc[npc.type], vector2, new Rectangle?(npc.frame), color, npc.rotation, new Vector2(TextureAssets.Npc[npc.type].Width() * 0.5f, TextureAssets.Npc[npc.type].Height() / 6 * 0.5f), npc.scale, 0, 0f);
                    }
                }
                //spriteBatch.Draw(Main.npcTexture[npc.type], npc.position+ vector - Main.screenPosition, new Rectangle?(npc.frame), drawColor, npc.rotation, new Vector2(Main.npcTexture[npc.type].Width * 0.5f, Main.npcTexture[npc.type].Height / 6 * 0.5f), npc.scale, 0, 0f);
            }
            return true;
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.KingSlime)
            {
                for (int a = 0; a < 255; a++)
                {
                    if (Players[a] > 0 && Main.player[a] == Main.LocalPlayer)
                    {
                        Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Image/逃脱UI槽");
                        Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("DDmod/Image/逃脱UI条");
                        double quotient = ((double)30 - Players[a]) / 30;
                        quotient = Utils.Clamp(quotient, 0f, 1f);

                        int C = (int)(texture2.Width * quotient);

                        spriteBatch.Draw(texture, npc.Center - Main.screenPosition - new Vector2(0, 140), null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1f, 0, 0);
                        spriteBatch.Draw(texture2, npc.Center - Main.screenPosition - new Vector2(0, 140), new Rectangle?(new Rectangle(0, 0, C, texture2.Height)), Color.White, 0, new Vector2(texture2.Width / 2, texture2.Height / 2), 1f, 0, 0);
                    }
                }
            }
        }
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (npc.type == NPCID.KingSlime && Main.hardMode)
            {
                modifiers.ModifyHitInfo += Modifiers_ModifyHitInfo;
            }
        }

        private void Modifiers_ModifyHitInfo(ref HitInfo info)
        {
            if (Split == 0)
            {
                //info.Damage /= 2;
            }
            if (Split == 0)
            {
                //info.Damage = (int)(info.Damage * 0.66F);
            }
        }
    }
}