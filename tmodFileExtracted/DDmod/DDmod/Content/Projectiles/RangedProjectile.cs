using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged.Ammo;
using DDmod.Players;
using Terraria;
using Terraria.ModLoader.IO;

namespace DDmod.Content.Projectiles
{
    public class RangedProjectile : GlobalProjectile
    {

        public override bool InstancePerEntity => true;
        public int Bullet;
        /// <summary>
        /// 子弹
        /// </summary>
        public bool BulletProj;
        public bool Shop;
        /// <summary>
        /// 可以复制
        /// </summary>
        public bool copy;
        ///<summary>弓的大小</summary>
        public Vector2 Scale;

        ///<summary>弓箭射速</summary>
        public float[] BowSpeed = new float[3];
        ///<summary>弓箭伤害</summary>
        public int[] BowDamage = new int[3];
        ///<summary>弓箭击退</summary>
        public float[] BowKnockBack = new float[3];
        ///<summary>弓箭ID</summary>
        public int[] BowUsedAmmoItemId = new int[3];
        /// <summary>弹药</summary>
        public int[] Ammo = new int[3];
        /// <summary>拉弓时间</summary>
        public float BowTime;
        /// <summary>射出停顿</summary>
        public bool ShootShop;
        /// <summary>弓特殊效果</summary>
        public bool SpecialEffect;
        /// <summary>启用右键蓄力</summary>
        public bool Charge;
        /// <summary>右键</summary>
        public bool RightClick;
        /// <summary>弓动画时间</summary>
        public int BowAnimationTime = 20;
        /// <summary>
        /// 旧位置
        /// </summary>
        public Vector2[] oldPos;
        public float[] oldRoe;
        public static Asset<Texture2D> 珍珠木箭;
        public override void Load()
        {
            珍珠木箭 = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/珍珠木箭");
        }
        public override void SetDefaults(Projectile projectile)
        {
            //邪箭
            if (projectile.type == 4)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
                projectile.penetrate = 1;
            }
            //幻影矢
            if (projectile.type == 631)
            {
                ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            }
            // 骸骨箭
            if (projectile.type == 117)
            {
                NPCHit(projectile, -1);
            }
            // 蜜蜂
            if (projectile.type == 566)
            {
                NPCHit(projectile, 60);
                projectile.penetrate = 2;
            }
            // 蜜蜂
            if (projectile.type == 181)
            {
                NPCHit(projectile, 60);
                projectile.penetrate = 2;
            }
            // 大蜜蜂
            if (projectile.type == 189)
            {
                NPCHit(projectile, 60);
                projectile.penetrate = 2;
            }
            //暗影箭
            if (projectile.type == 495)
            {
                NPCHit(projectile, -1);
            }
            //狱炎箭
            if (projectile.type == 41)
            {
                NPCHit(projectile, -1);
            }
            //狱炎箭
            if (projectile.type == 485)
            {
                projectile.ArmorPenetration = 15;
                NPCHit(projectile, -1);
            }
            //高速子弹
            if (projectile.type == 242)
            {
                NPCHit(projectile, -1);
            }
            if (projectile.type is 14 or 89 or 36 or 90 or 104 or 279 or 242 or 283 or 284 or 285 or 286 or 287 or 638 or 981 or 207)
            {
                BulletProj = true;
                projectile.extraUpdates = 2;
            }

            //大炮
            if (projectile.type == 929)
            {
                projectile.DamageType = DamageClass.Ranged;
            }
        }
        public void NPCHit(Projectile projectile, int Hit)
        {
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = Hit;
            projectile.usesIDStaticNPCImmunity = false;
        }

        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            Shop = binaryReader.ReadBoolean();
            Charge = binaryReader.ReadBoolean();
            ///<summary>弓箭射速</summary>
            BowSpeed[0] = binaryReader.ReadFloat();
            BowSpeed[1] = binaryReader.ReadFloat();
            BowSpeed[2] = binaryReader.ReadFloat();
            /*
            ///<summary>弓箭伤害</summary>
            BowDamage[0] = binaryReader.ReadInt32();
            BowDamage[1] = binaryReader.ReadInt32();
            BowDamage[2] = binaryReader.ReadInt32();
            ///<summary>弓箭击退</summary>
            BowKnockBack[0] = binaryReader.ReadFloat();
            BowKnockBack[1] = binaryReader.ReadFloat();
            BowKnockBack[2] = binaryReader.ReadFloat();*/
            /// <summary>弹药</summary>
            Ammo[0] = binaryReader.ReadInt32();
            Ammo[1] = binaryReader.ReadInt32();
            Ammo[2] = binaryReader.ReadInt32();
            ///<summary>弓箭ID</summary>
            BowUsedAmmoItemId[0] = binaryReader.ReadInt32();
            BowUsedAmmoItemId[1] = binaryReader.ReadInt32();
            BowUsedAmmoItemId[2] = binaryReader.ReadInt32();
            /// <summary>拉弓时间</summary>
            BowTime = binaryReader.ReadFloat();
            /// <summary>射出停顿</summary>
            ShootShop = binaryReader.ReadBoolean();
            /// <summary>弓特殊效果</summary>
            SpecialEffect = binaryReader.ReadBoolean();
            /// <summary>启用右键蓄力</summary>
            Charge = binaryReader.ReadBoolean();
            /// <summary>右键</summary>
            RightClick = binaryReader.ReadBoolean();
            /// <summary>弓动画时间</summary>
            BowAnimationTime = binaryReader.ReadInt32();
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Shop);
            binaryWriter.Write(Charge);
            ///<summary>弓箭射速</summary>
            binaryWriter.Write(BowSpeed[0]);
            binaryWriter.Write(BowSpeed[1]);
            binaryWriter.Write(BowSpeed[2]);
            /*
            ///<summary>弓箭伤害</summary>
            binaryWriter.Write(BowDamage[0]);
            binaryWriter.Write(BowDamage[1]);
            binaryWriter.Write(BowDamage[2]);
            ///<summary>弓箭击退</summary>
            binaryWriter.Write(BowKnockBack[0]);
            binaryWriter.Write(BowKnockBack[1]);
            binaryWriter.Write(BowKnockBack[2]);*/
            /// <summary>弹药</summary>
            binaryWriter.Write(Ammo[0]);
            binaryWriter.Write(Ammo[1]);
            binaryWriter.Write(Ammo[2]);
            ///<summary>弓箭ID</summary>
            binaryWriter.Write(BowUsedAmmoItemId[0]);
            binaryWriter.Write(BowUsedAmmoItemId[1]);
            binaryWriter.Write(BowUsedAmmoItemId[2]);
            /// <summary>拉弓时间</summary>
            binaryWriter.Write(BowTime);
            /// <summary>射出停顿</summary>
            binaryWriter.Write(ShootShop);
            /// <summary>弓特殊效果</summary>
            binaryWriter.Write(SpecialEffect);
            /// <summary>启用右键蓄力</summary>
            binaryWriter.Write(Charge);
            /// <summary>右键</summary>
            binaryWriter.Write(RightClick);
            /// <summary>弓动画时间</summary>
            binaryWriter.Write(BowAnimationTime);
        }
        Vector2 Vector;
        Vector2 Vector2;
        public override bool PreAI(Projectile projectile)
        {
            /*
            if (projectile.type == 1)
            {

                Dust dust = Main.dust[NewDust(projectile.Center + Vector2.Normalize(projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(55, 55, 55, 0))];
                dust.noGravity = true;
                dust.velocity = -projectile.velocity.PerfectNormalize().RotatedBy(-Main.rand.NextFloat(0.3F, 0.5F)) * Main.rand.NextFloat(5F, 8F);
                dust.alpha = 100;
                dust.scale = 1;
                dust.customData = 1;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = projectile.whoAmI;

                dust = Main.dust[NewDust(projectile.Center + Vector2.Normalize(projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(55, 55, 55, 0))];
                dust.noGravity = true;
                dust.velocity = -projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(0.3F, 0.5F)) * Main.rand.NextFloat(5F, 8F);
                dust.alpha = 100;
                dust.scale = 1;
                dust.customData = 1;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = projectile.whoAmI;

                dust = Main.dust[NewDust(projectile.Center- Vector2.Normalize(projectile.velocity)*20 + projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2)*8 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(55, 55, 55, 0))];
                dust.noGravity = true;
                dust.velocity =Vector2.Zero;
                dust.alpha = 100;
                dust.scale = 1;
                dust.customData = 0.5F;
                dust.rotation = projectile.velocity.ToRotation();

                dust = Main.dust[NewDust(projectile.Center - Vector2.Normalize(projectile.velocity) * 20 + projectile.velocity.PerfectNormalize().RotatedBy(-MathHelper.PiOver2)*8 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(55, 55, 55, 0))];
                dust.noGravity = true;
                dust.velocity =Vector2.Zero;
                dust.alpha = 100;
                dust.scale = 1;
                dust.customData = 0.5F;
                dust.rotation = projectile.velocity.ToRotation();
            }*/
            if (projectile.type == 932)
            {
                //projectile.ai[1] += 0.01F;
            }
            //子弹
            if (projectile.type is 14 or 89 or 36 or 90 or 104 or 279 or 242 or 283 or 284 or 285 or 286 or 287 or 638 or 981)
            {
                if (projectile.ai[2] < projectile.velocity.Length() * (projectile.extraUpdates + 1))
                {
                    projectile.ai[2] += projectile.velocity.Length() / 5;
                }
                else
                {
                    projectile.ai[2] = projectile.velocity.Length() * (projectile.extraUpdates + 1);
                }
            }
            if (BulletProj)
            {
                if (projectile.DProj().track < 5 && projectile.Player().ActiveItem().type > 0 && (projectile.Player().ActiveItem().type == 1254 || projectile.Player().ActiveItem().DItem().Sniper))
                {
                    if (projectile.extraUpdates < 5)
                    {
                        projectile.extraUpdates = 5;
                    }
                }
            }
            //子弹
            if (projectile.type == 981)
            {
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 20;
                }
            }
            if (projectile.type == 631)
            {
                if (projectile.ai[1] <= 0)
                {
                    projectile.ai[1]--;
                }
            }

            if (oldPos != null)
            {
                oldPos[0] = projectile.position;
                for (int i = oldPos.Length - 1; i > 0; i--)
                {
                    oldPos[i] = oldPos[i - 1];
                }
            }
            if (oldRoe != null)
            {
                oldRoe[0] = projectile.rotation;
                for (int i = oldRoe.Length - 1; i > 0; i--)
                {
                    oldRoe[i] = oldRoe[i - 1];
                }
            }
            if (projectile.type == 117 && projectile.DProj().Bool[0])
            {
                projectile.penetrate = -1;
                if (projectile.DProj().Bool[1])
                {
                    projectile.DProj().Times[2] += 0.3F;
                    NPC npc = Main.npc[(int)projectile.DProj().Times[1]];
                    npc.Center = projectile.Center + Vector;
                    if (npc.Dnpc().Control2 > 0)
                    {
                        if (!Main.npc[(int)projectile.DProj().Times[1]].active)
                        {
                            projectile.DProj().Times[1] = -1;
                            projectile.DProj().Bool[1] = false;
                            projectile.DProj().Bool[2] = true;
                        }
                    }
                    npc.Dnpc().Control2 = 5;
                }
            }
            if (projectile.type == 706)
            {
                if (projectile.DProj().Bool[0])
                {
                    if (projectile.DProj().vector[0] == Vector2.Zero)
                        projectile.DProj().vector[0] = projectile.velocity;
                    DDHelper.BackAndForth(-10F, 10F, 1F, ref projectile.DProj().Times[0], ref projectile.DProj().Bool[1]);
                    projectile.velocity = projectile.DProj().vector[0] + new Vector2(0, projectile.DProj().Times[0] * projectile.DProj().Times[2]).RotatedBy(projectile.DProj().vector[0].ToRotation());

                }
            }
            if (projectile.damage == 0 && projectile.type == 598)
            {
                for (int a = 0; a < Main.player.Length; a++)
                {
                    Player player = Main.player[a];

                    //检测玩家有没有按下
                    if (!player.controlDown)
                    {
                        if ((bool)ProjectileLoader.Colliding(projectile, projectile.getRect(), new Rectangle((int)player.position.X, (int)player.position.Y + player.height - 4, projectile.width, 2)))
                        {
                            Tile tile = Main.tile[(int)(player.position.X / 16), (int)(player.position.Y) / 16 - 1];
                            Tile tile2 = Main.tile[(int)(player.position.X / 16) + 1, (int)(player.position.Y) / 16 - 1];
                            if (!WorldGen.SolidTile(tile) && !WorldGen.SolidTile(tile2))
                            {
                                player.position.Y -= 2f;
                            }
                        }
                        if ((bool)ProjectileLoader.Colliding(projectile, projectile.getRect(), new Rectangle((int)player.position.X, (int)player.position.Y + player.height - 4, projectile.width, 4)))
                        {
                            //调整Y位置
                            //player.position.Y = projectile.position.Y - player.height + 4;
                            //踩
                            Tile tile = Main.tile[(int)(player.position.X / 16), (int)(player.position.Y + player.height) / 16 + 1];
                            if (tile.HasTile && !player.controlJump)
                            {
                                player.velocity.Y = 0f;
                                AttributesPlayer.Walk(player);
                                player.velocity.Y = -0.001f;
                                player.gfxOffY = 0;
                            }
                            if (player.velocity.Y >= 0)
                            {
                                player.Aplayer().Stand = 2;
                            }
                        }

                    }
                }
                for (int a = 0; a < Main.npc.Length; a++)
                {
                    NPC npc = Main.npc[a];
                    //检测玩家有没有按下
                    if ((bool)ProjectileLoader.Colliding(projectile, projectile.getRect(), new Rectangle((int)npc.position.X, (int)npc.position.Y + npc.height - 4, projectile.width, 1)))
                    {
                        npc.position.Y -= 2f;
                    }
                    if ((bool)ProjectileLoader.Colliding(projectile, projectile.getRect(), new Rectangle((int)npc.position.X, (int)npc.position.Y + npc.height - 4, projectile.width, 4)))
                    {
                        //调整Y位置
                        //player.position.Y = projectile.position.Y - player.height + 4;
                        //踩
                        if (npc.velocity.Y >= 0)
                        {
                            npc.Dnpc().Stand = 3;
                        }
                    }
                }
            }
            if (projectile.type == 90)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
                projectile.velocity *= 0.92F;
                if (projectile.velocity.Length() < 0.1F)
                {
                    projectile.Kill();
                }
                return false;
            }
            if (projectile.type == 36)
            {
                projectile.alpha += 5;
            }
            if (projectile.type == 242 || projectile.type == 279 || projectile.type == 104)
            {
                projectile.alpha += 5;
            }
            if (projectile.type == 287)
            {
                int num183 = Dust.NewDust(projectile.Center - new Vector2(4) + new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20)), 1, 1, 246);
                Main.dust[num183].alpha = projectile.alpha;
                Main.dust[num183].scale = 1;
                Main.dust[num183].velocity *= 0f;
                Main.dust[num183].noGravity = true;

            }
            if (projectile.type == 207)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 20;
                    if (projectile.localAI[0] == 0)
                    {
                        projectile.localAI[0] = projectile.velocity.Length();
                    }
                }


                if (projectile.alpha < 170)
                {
                    int A = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<激光粒子>(), projectile.velocity.X / 2, projectile.velocity.Y / 2, 0, new Color(100, 255, 0, 0), 0.2f);
                    Main.dust[A].velocity = Vector2.Zero;
                    Main.dust[A].customData = -1;
                    Main.dust[A].rotation = projectile.rotation - MathHelper.PiOver2;
                }
                if (projectile.DProj().track % 30 == 0)
                {
                    NPC npc = NPCdirection.FindClosest(projectile.Center, 300, false);
                    if (npc != null)
                    {
                        projectile.ai[2] = npc.whoAmI;
                    }
                    else
                    {
                        projectile.ai[2] = -2;
                    }
                }
                projectile.Track(300, 10, projectile.localAI[0], 0, NPCID: (int)projectile.ai[2]);
                return false;
            }
            if (projectile.type == 819)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
                if (projectile.DProj().track > 30)
                {
                    projectile.velocity.X *= 0.99F;
                    if (projectile.velocity.Y < 12)
                    {
                        projectile.velocity.Y += 0.05F;
                    }
                }
                int A = NewDust(projectile.Center - new Vector2(4), 1, 1, 5, projectile.velocity.X / 2, projectile.velocity.Y / 2, 0, default, 1.6F);
                Main.dust[A].velocity = projectile.velocity / 4;
                Main.dust[A].noGravity = true;
                Main.dust[A].rotation = projectile.rotation - MathHelper.PiOver2;
                return false;
            }
            return base.PreAI(projectile);
        }
        public override void EmitEnchantmentVisualsAt(Projectile projectile, Vector2 boxPosition, int boxWidth, int boxHeight)
        {
            Player player = Main.player[projectile.owner];

            if (projectile.numUpdates==0&&((player.Aplayer().暗影焰力量 == 1 && projectile.arrow) || (player.Aplayer().暗影焰力量 == 2 && BulletProj)) && projectile.friendly && !projectile.hostile && !projectile.noEnchantments)
            {
                int num2 = Dust.NewDust(new Vector2(boxPosition.X - 4f, boxPosition.Y - 4f), boxWidth + 8, boxHeight + 8, 27, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 1.25f);
                if (Main.rand.Next(2) == 0)
                    Main.dust[num2].scale = 0.75f;

                Main.dust[num2].noGravity = true;
                Main.dust[num2].velocity.X *= 2f;
                Main.dust[num2].velocity.Y *= 2f;
            }
        }
        public override void PostAI(Projectile projectile)
        {
            if (projectile.Player().Aplayer().暗影焰力量 == 1)
            {
                if (projectile.DProj().track == 5)
                {
                    if (projectile.arrow)
                        projectile.MaxUpdates *= 2;
                }
            }
            if (projectile.Player().Aplayer().暗影焰力量 == 2)
            {
                if (projectile.DProj().track == 5)
                {
                    if (BulletProj)
                        projectile.MaxUpdates *= 2;
                }
            }
            if (projectile.type == 615)
            {
                Item item = projectile.Player().ActiveItem();
                projectile.ai[1]++;
                projectile.ai[1] -= projectile.Player().GetTotalAttackSpeed(DamageClass.Ranged)+(1 - (float)item.useAnimation / item.DItem().OriginaluseAnimation(item));
            }
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.type == 36)
            {
                for (int a = 0; a < 2; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(248, 68, 68, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * projectile.oldVelocity.Length() * (projectile.extraUpdates + 1);
                    Main.dust[dust].noGravity = true;
                }
                projectile.alpha = 255;
            }
            if (projectile.type == 285)
            {
                for (int a = 0; a < 2; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(0, 167, 240, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * projectile.oldVelocity.Length() * (projectile.extraUpdates + 1);
                    Main.dust[dust].noGravity = true;
                }
                projectile.alpha = 255;
            }
            //骸骨箭
            if (projectile.type == 117 && projectile.DProj().Bool[0])
            {
                if (projectile.DProj().Times[0] == 0)
                {
                    if (projectile.DProj().Bool[1])
                    {
                        Main.npc[(int)projectile.DProj().Times[1]].SimpleStrikeNPC((int)(projectile.damage * (projectile.DProj().Times[2] / 10)), 0);
                        Vector2 vector = Vector.PerfectNormalize();
                        Vector.X -= (Main.npc[(int)projectile.DProj().Times[1]].width * vector.X * 0.9f);
                        Vector.Y -= (Main.npc[(int)projectile.DProj().Times[1]].height * vector.Y * 0.9f);
                    }
                    projectile.position += projectile.velocity.PerfectNormalize() * 22;
                }
                if (projectile.DProj().Times[1] >= 0)
                {
                    if (projectile.DProj().Bool[1])
                    {
                        NPC npc = Main.npc[(int)projectile.DProj().Times[1]];
                        npc.position -= projectile.velocity.PerfectNormalize() * 16;
                    }
                }
                projectile.DProj().Times[0] = 1;
                projectile.DProj().Bool[2] = true;
                projectile.aiStyle = -1;
                projectile.velocity = Vector2.Zero;

                return true;
            }
            //骨头镖枪
            if (projectile.type == 598)
            {
                projectile.aiStyle = -1;
                projectile.position += projectile.velocity.PerfectNormalize() * 22;
                projectile.velocity = Vector2.Zero;
                projectile.damage = 0;
                projectile.alpha = 0;
                projectile.tileCollide = false;
                int r = 0;
                for (int a = 0; a < Main.projectile.Length; a++)
                {
                    Projectile proj = Main.projectile[a];
                    if (proj.type == 598 && proj.damage == 0 && proj.active && proj.DProj().Times[0] != 0)
                    {
                        r++;
                    }
                }
                projectile.DProj().Times[0] = r + 1;
                if (r > 3)
                {
                    for (int a = 0; a < Main.projectile.Length; a++)
                    {
                        Projectile proj = Main.projectile[a];
                        if (proj.type == 598 && proj.damage == 0 && proj.active && projectile.DProj().Times[0] != 0)
                        {
                            if (proj.DProj().Times[0] == 1)
                            {
                                proj.Kill();
                            }
                            proj.DProj().Times[0]--;
                        }
                    }
                }
                return false;
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }
        public override bool? Colliding(Projectile projectile, Rectangle projHitbox, Rectangle targetHitbox)
        {
            //骨头镖枪
            if (projectile.type == 598 && projectile.damage == 0)
            {
                float num = 0f;
                return Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), projectile.Center, projectile.Center - (projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 66, 2, ref num);
            }
            return base.Colliding(projectile, projHitbox, targetHitbox);
        }
        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if ((projectile.type == 117 && projectile.DProj().Bool[0] && projectile.DProj().Bool[1]))
            {
                return false;
            }
            return base.CanHitNPC(projectile, target);
        }
        public override bool? CanDamage(Projectile projectile)
        {

            if (projectile.type == 615)
            {
                return false;
            }
                return base.CanDamage(projectile);
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.type == 117 && projectile.DProj().Bool[0] && !projectile.DProj().Bool[1] && !target.Dnpc().BossPhysique)
            {
                projectile.DProj().Times[1] = target.whoAmI;
                Vector = target.Center - projectile.Center;
                projectile.DProj().Bool[1] = true;
                projectile.netUpdate = true;
            }
        }

        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            //星旋火箭
            if (projectile.type == 616)
            {
                NewDustChange4(50, projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 3, 8, true, 3.4F, 5.5F, 100, 1000, new Color(63, 172, 204, 0), 3);
                SoundStyle sound = SoundID.NPCDeath14;
                sound.Pitch = -0.5F;
                PlaySound(sound, projectile.Center);
                return false;
            }
            return base.PreKill(projectile, timeLeft);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            float A = projectile.oldVelocity.Length() * 0.5F * (projectile.extraUpdates + 1);
            if (A > 30)
            {
                A = 30;
            }
            //夜明弹
            if (projectile.type == 638)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(94, 255, 197, 0), 1);
                    Main.dust[dust].velocity = projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            //邪箭
            if (projectile.type == 4)
            {
                if (target.HasBuff(ModContent.BuffType<EvilEntanglement>()))
                {
                    NewProjectile(projectile.GetSource_FromAI(), projectile.Center, new Vector2(Main.rand.NextFloat(-25, 25), -50), ModContent.ProjectileType<UnholyArrowRemnant>(), projectile.damage / 2, 0, projectile.owner, 0, target.whoAmI);
                }
                else if (Main.rand.NextBool(6))
                {
                    target.AddBuff(ModContent.BuffType<EvilEntanglement>(), Main.rand.Next(60, 600));
                    NewProjectile(projectile.GetSource_FromAI(), projectile.Center, new Vector2(Main.rand.NextFloat(-25, 25), -50), ModContent.ProjectileType<UnholyArrowRemnant>(), projectile.damage / 2, 0, projectile.owner, 0, target.whoAmI);
                }
            }
            //冰霜箭
            if (projectile.type == 120 && Main.rand.NextBool(6))
            {
                target.AddBuff(ModContent.BuffType<Frozen>(), 180);
            }
            if (((player.Aplayer().暗影焰力量 == 1 && projectile.arrow) || (player.Aplayer().暗影焰力量 == 2 && BulletProj)) && projectile.friendly && !projectile.hostile && !projectile.noEnchantments)
            {
                target.AddBuff(153, 300);
            }
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            float A = projectile.oldVelocity.Length() * 0.75F * (projectile.extraUpdates + 1);
            if (A > 45)
            {
                A = 45;
            }
            if (projectile.type == 14)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(253, 122, 3, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 36)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(248, 68, 68, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 104)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(100, 255, 0, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 207)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, new Color(100, 255, 0, 0), Main.rand.NextFloat(3, 4));
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                    Main.dust[dust].customData = 2;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 279)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(255, 155, 0, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 242)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(255, 255, 100, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 283)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(151, 79, 163, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 284)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(240, 0, 158, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 285)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(0, 167, 240, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 286)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(240, 62 + Main.rand.Next(100), 0, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 287)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(255, 228, 66, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 638)
            {
                for (int a = 0; a < 5; a++)
                {
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(94, 255, 197, 0), 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            if (projectile.type == 981)
            {
                for (int a = 0; a < 5; a++)
                {
                    Color color = new Color(253, 122, 3, 0);
                    if (Main.rand.NextBool(5))
                    {
                        color = new Color(200, 200, 200, 0);
                    }
                    int dust = NewDust(projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, color, 1);
                    Main.dust[dust].velocity = -projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0.1F, 0.5f) * A;
                    Main.dust[dust].noGravity = true;
                }
            }
            projectile.netUpdate = true;
        }
        public override bool PreDraw(Projectile Projectile, ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            //子弹
            float A = Projectile.ai[2] * 1;
            if (A > 90)
            {
                A = 90;
            }
            //幻影矢
            if (Projectile.type == 1)
            {
                if (Projectile.ai[2] == 1)
                {
                    Main.spriteBatch.Draw(珍珠木箭.Value, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width, 0) / 2, Projectile.scale, 0, 0f);
                    return false;
                }
                return true;
            }
            //幻影矢
            if (Projectile.type == 631)
            {
                if (Projectile.ai[1] <= -5)
                {
                    Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
                    for (int i = 0; i < Projectile.oldPos.Length; i++)
                    {
                        Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                        Color color = new Color(255, 255, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 0.75f;
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, new Vector2(texture.Width, 0) / 2, Projectile.scale, 0, 0f);

                    }
                }
                return false;
            }
            //子弹偏移
            Vector2 BV = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * Projectile.height / 2;
            if (Projectile.type == 14)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, Projectile.GetAlpha(new Color(253, 122, 3, 0)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, Projectile.GetAlpha(new Color(100, 100, 100, 0)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            //子弹
            if (Projectile.type == 89)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(43, 139, 248, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            //子弹
            if (Projectile.type == 36)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(248, 68, 68, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            //子弹
            if (Projectile.type == 90)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(43, 139, 248, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            //子弹
            if (Projectile.type == 104)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 255, 0, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            if (Projectile.type == 279)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(255, 155, 0, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            if (Projectile.type == 242)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(255, 255, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            if (Projectile.type == 283)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(151, 79, 163, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            if (Projectile.type == 284)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(240, 0, 158, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            if (Projectile.type == 285)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(0, 167, 240, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            if (Projectile.type == 286)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(240, 62, 0, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            if (Projectile.type == 287)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(255, 228, 66, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            if (Projectile.type == 638)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(94, 255, 197, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(100, 100, 100, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            //子弹
            if (Projectile.type == 981)
            {
                texture = DDTextures.WhitePng.Value;
                Main.spriteBatch.Draw(texture, BV, null, new Color(253, 122, 3, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A), 0, 0f);
                Main.spriteBatch.Draw(texture, BV, null, new Color(200, 200, 200, 0) * (1 - (Projectile.alpha / 255F)), Projectile.rotation, new Vector2(texture.Width / 2, 0), new Vector2(Projectile.scale, A / 8), 0, 0f);

                return false;
            }
            //邪箭
            if (Projectile.type == 4)
            {
                Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    for (int a = 0; a < 3; a++)
                    {
                        Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                        Color color = new Color(42, 39, 82, 20) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 0.5f;
                        Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], new Vector2(texture.Width, 0) / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                    }
                }
            }
            return base.PreDraw(Projectile, ref lightColor);
        }
    }
}