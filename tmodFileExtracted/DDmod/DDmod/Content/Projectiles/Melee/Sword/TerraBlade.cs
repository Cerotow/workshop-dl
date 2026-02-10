using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Players;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class TerraBlade : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            Projectile.width = 20;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 66;
            Projectile.extraUpdates = 12;
            Projectile.DProj().Times[4] = 2.2F;
            Projectile.noEnchantments = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

        private int proj = 0;

        //特殊攻击
        private bool Special;
        //特殊攻击
        private int S;

        //是否给Buff
        private bool Buff;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(Special);
            writer.Write(Buff);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
            Special = reader.ReadBoolean();
            Buff = reader.ReadBoolean();
        }

        private Player player;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (!Special && Projectile.scale == 1 && !player.HasBuff(ModContent.BuffType<SpecialAttackCD>()))
            {
                if (player.controlUseTile)
                {
                    Projectile.DProj().MouseWorld = player.Dplayer().MouseWorld;
                    Projectile.DProj().vector[2] = Projectile.DProj().MouseWorld - player.Center;
                    //player.AddBuff(ModContent.BuffType<SpecialAttackCD>(), 600);
                    Projectile.damage = (int)(Projectile.damage * 2.25f);
                    Special = true;
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            if (S > 4)
            {
                Projectile.Kill();
                player.itemTime = 0;
                player.itemAnimation = 0;
                return false;
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

            if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
            {
                Projectile.height = (int)(Projectile.height * 2.75f);
            }
            if (Special)
            {
                if (Projectile.DProj().vector[2].X > 0)
                {
                    Projectile.HoldProj(player, 48 * Projectile.scale, Projectile.ai[0], Projectile.DProj().MouseWorld.RotatedBy(1.2F - MathHelper.PiOver2), MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                }
                else
                {
                    Projectile.HoldProj(player, 48 * Projectile.scale, Projectile.ai[0], Projectile.DProj().MouseWorld.RotatedBy(1.8F + MathHelper.PiOver2), MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
                }
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), 120, Projectile.DProj().Times[4] * 2, true);
            }
            else
            {
                Projectile.HoldProj(player, 48 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);

                if (S <= 2)
                {
                    Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), -1, Projectile.DProj().Times[4], true);
                }
                else if (S <= 4)
                {
                    Projectile.extraUpdates = 18;
                    Projectile.DProj().Times[4] = MathHelper.TwoPi * 2.3F;
                    if (S == 3)
                    {
                        Projectile.netUpdate = true;
                        Projectile.ClearInvincibleFrame();
                        S++;
                    }
                    if (proj != 0)
                        proj++;
                    if (Projectile.owner == Main.myPlayer && proj > 10)
                    {
                        if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
                        {
                            if (Main.rand.NextBool(20))
                            {
                                int P = NewProjectile(Projectile.GetSource_FromThis(), player.Center + Projectile.velocity.PerfectNormalize() * 122, Projectile.DProj().vector[0].PerfectNormalize() * 30, ModContent.ProjectileType<TerraEdge>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack / 2, Projectile.owner);
                                Main.projectile[P].scale = 1f;
                                Main.projectile[P].DProj().track = 100;
                            }
                        }
                        else
                        {
                            if (Main.rand.NextBool(40))
                            {
                                int P = NewProjectile(Projectile.GetSource_FromThis(), player.Center + Projectile.velocity.PerfectNormalize() * 82, Projectile.DProj().vector[0].PerfectNormalize() * 30, ModContent.ProjectileType<TerraEdge>(), Projectile.damage/2, Projectile.knockBack / 2, Projectile.owner);
                                Main.projectile[P].scale = 0.8f;
                                Main.projectile[P].DProj().track = 100;
                            }
                        }
                    }
                    Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), 80, Projectile.DProj().Times[4], true);
                }
            }

            if ((Projectile.DProj().Times[0] > 0 && player.direction > 0) || (Projectile.DProj().Times[0] < 0 && player.direction < 0))
            {
                Projectile.rotation -= 0.02F * player.direction;
            }
            else
            {
                Projectile.rotation += 0.02F * player.direction;
            }
            if (Projectile.localAI[1] < 0)
                Projectile.MeleeProj().oldVels3 = 0.08F;

            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead;
            if (channeling)
            {
                Projectile.DProj().Times[2] = 0;
            }
            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                if (Special && proj != 0)
                {
                    Projectile.Kill();
                }
                proj = 0;
            }
            else
            {
                if (Special)
                {
                    if (player.fullRotation != 0 && (!player.mount.Active && !player.mount.Cart))
                    {
                        player.fullRotationOrigin = player.Size / 2;
                    }
                    float PI = 0;
                    if (player.direction < 0)
                    {
                        PI = MathHelper.Pi;
                    }

                    player.fullRotation = Projectile.rotation - MathHelper.PiOver4 + PI;
                    if (proj == 1)
                    {
                        if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
                        {
                            Projectile.extraUpdates = 20;
                           // player.position -= player.velocity;
                            player.velocity.Y = -0.001f;
                            player.velocity.X = 0;
                            player.position += Collision.TileCollision(player.position, Projectile.DProj().vector[2].PerfectNormalize() * 2, player.width, player.height, true, false);

                            if (Collision.TileCollision(player.position, Projectile.DProj().vector[2].PerfectNormalize() * 2, player.width, player.height, true, true).Length() < 0.1F)
                            {
                                Projectile.Kill();
                            }
                            player.Aplayer().NoGravity = 2;
                        }
                        else
                        {
                            Projectile.extraUpdates = 20;
                            //player.position -= player.velocity;
                            player.velocity.Y = -0.001f;
                            player.velocity.X = 0;
                            player.position += Collision.TileCollision(player.position, Projectile.DProj().vector[2].PerfectNormalize(), player.width, player.height, true, false);

                            if (Collision.TileCollision(player.position, Projectile.DProj().vector[2].PerfectNormalize(), player.width, player.height, true, true).Length() < 0.1F)
                            {
                                Projectile.Kill();
                            }
                            player.Aplayer().NoGravity = 2;
                        }
                    }
                    DDPlayer.移动玩家(player, 30, true);
                    if (Main.rand.NextBool(80) && Projectile.owner == Main.myPlayer)
                    {
                        int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center, new Vector2(0, 20).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), ModContent.ProjectileType<TerraEdge>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                    }
                    player.dashDelay = 5;
                }
                if (proj == 0)
                {
                    proj++;
                    S++;
                    SoundStyle sound = SoundID.Item71;
                    sound.Pitch = -0.7F;
                    if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
                    {
                        sound.Pitch = -1F;
                    }
                    PlaySound(sound, Projectile.position);
                    if (!Special)
                    {
                        if (Projectile.owner == Main.myPlayer && S <= 2)
                        {
                            Projectile.DProj().MouseWorld = Main.MouseWorld;
                            Projectile.netUpdate = true;

                            Vector2 vector = Main.rand.NextVector2Unit() * 200;
                            if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
                            {
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld, Vector2.Zero, ModContent.ProjectileType<TerraExplode>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner, 0, 1.4F);

                                float R = Main.rand.NextFloat(0, MathHelper.TwoPi);
                                vector = new Vector2(0, -1).RotatedBy(MathHelper.PiOver4 + R) * 500;
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 20, ModContent.ProjectileType<TerraSlash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                                vector = new Vector2(0, -1).RotatedBy(-MathHelper.PiOver4 + R) * 500;
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 20, ModContent.ProjectileType<TerraSlash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                                vector = new Vector2(0, 1).RotatedBy(-MathHelper.PiOver4 + R) * 500;
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 20, ModContent.ProjectileType<TerraSlash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                                vector = new Vector2(0, 1).RotatedBy(MathHelper.PiOver4 + R) * 500;
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 20, ModContent.ProjectileType<TerraSlash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                                /*
                                for (int a = 1; a <= 3; a++)
                                {
                                    int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0] * ((float)a + 2) * Projectile.scale, ModContent.ProjectileType<TrueNightBlade>(), (int)(Projectile.damage * ((float)a / 3)), Projectile.knockBack / 2, Projectile.owner, 2);
                                    Main.projectile[A].scale *= a * 0.33f * Projectile.scale;
                                }*/
                                int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0] * 3 * Projectile.scale, ModContent.ProjectileType<TrueNightBlade>(), (int)(Projectile.damage * 1.5f), Projectile.knockBack / 2, Projectile.owner, 2);
                                Main.projectile[A].scale *= Projectile.scale;
                                Main.projectile[A].DProj().Magnification = 2;
                            }
                            else
                            {
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld, Vector2.Zero, ModContent.ProjectileType<TerraExplode>(), (int)(Projectile.damage * 1.5f), Projectile.knockBack, Projectile.owner, 0, 1F);

                                float R = Main.rand.NextFloat(0, MathHelper.TwoPi);
                                vector = new Vector2(0, -1).RotatedBy(MathHelper.PiOver4 + R) * 500;
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 20, ModContent.ProjectileType<TerraSlash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                                vector = new Vector2(0, 1).RotatedBy(MathHelper.PiOver4 + R) * 500;
                                NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 20, ModContent.ProjectileType<TerraSlash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);

                                int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0] * 3 * Projectile.scale, ModContent.ProjectileType<TrueNightBlade>(), Projectile.damage, Projectile.knockBack / 2, Projectile.owner, 2);
                                Main.projectile[A].scale *= 0.6f * Projectile.scale;
                            }
                        }
                    }
                    else
                    {
                        Projectile.DProj().Times[4] = 26;
                    }
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.localAI[1] >= 0)
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
                {
                    if (Main.rand.NextBool(100))
                    {
                        int Type = ModContent.DustType<星光粒子>();
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 0, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0))];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 3;
                        dust.scale = Projectile.scale * Main.rand.NextFloat(0.7F, 1F);
                    }
                    if (Main.rand.NextBool(10) && Projectile.localAI[1] >= 1 && Projectile.Player().magmaStone)
                    {
                        int Type = 6;
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 3;
                        dust.scale = Projectile.scale * Main.rand.NextFloat(0.6F, 1.3F);
                    }
                }
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Special)
            {
                Projectile.Player().velocity = new Vector2().MicroStop();
                if (Projectile.owner == Main.myPlayer)
                    DDmod.SyncData(DDType.PlayerCenter, Projectile.owner, -1, Projectile.owner);
            }
            if (Buff)
            {
                Projectile.Player().AddBuff(ModContent.BuffType<TerraPower>(), 900);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            /*
            for (int A = 0; A < 30; A++)
            {
                int Type = 107;
                Dust dust = Main.dust[NewDust(target.Center, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.5F;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 5);
            }
            */
            for (int a = 0; a < 2; a++)
            {
                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                //NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 10, ModContent.ProjectileType<TerraSlash>(), Projectile.damage / 4, Projectile.knockBack / 2, Projectile.owner,target.whoAmI,1);
            }
            if (Special)
            {
                Buff = true;
                for (int a = 0; a < 3; a++)
                {
                    Vector2 vector = Main.rand.NextVector2Unit() * 60;
                    //NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 10, ModContent.ProjectileType<TerraSlash>(), Projectile.damage / 6, Projectile.knockBack / 2, Projectile.owner, target.whoAmI, 1);
                }
            }
            else
            {
                if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
                {
                    for (int a = 0; a < 2; a++)
                    {
                        Vector2 vector = Main.rand.NextVector2Unit() * 60;
                        //NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 10, ModContent.ProjectileType<TerraSlash>(), Projectile.damage / 4, Projectile.knockBack / 2, Projectile.owner, target.whoAmI, 1);
                    }
                }
            }
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, settings, Projectile.owner);
        }

        private Color color = new Color(71, 233, 60, 120);
        private Color color2 = new Color(0, 162, 232, 120);
        public float TWidth()
        {
            if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
            {
                Projectile.MeleeProj().oldVels2 = 72 * 1.75f;
                return 30 * 1.75f;
            }
            else
            {
                Projectile.MeleeProj().oldVels2 = 68;
                return 30;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1]++;
                return false;
            }
            Player player = Projectile.Player();
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }
            if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
            {
                DDHelper.BladeTrail(DDTextures.WhitePng, [color2, color, color], 0.2F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
                DDHelper.BladeTrail(DDTextures.远古背景2, [color2, color, color], 0.2F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
            }
            else
            {
                DDHelper.BladeTrail(DDTextures.WhitePng, [color2, color, color], 0.2F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
                DDHelper.BladeTrail(DDTextures.Wave, [color2, color, color], 0.2F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D texture2 = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
                {
                    if (Projectile.spriteDirection == 0)
                    {
                        Main.spriteBatch.Draw(texture2, Center + Projectile.velocity.PerfectNormalize() * 40 * Projectile.scale, null, new Color(255, 255, 255, 155), Projectile.rotation, texture2.Size() / 2, Projectile.scale / 4 * 1.75F, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(texture2, Center, null, Color.White, Projectile.rotation, texture2.Size() / 2, Projectile.scale / 4, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(texture2, Center + Projectile.velocity.PerfectNormalize() * 40 * Projectile.scale, null, new Color(100,255,100,155) * 0.5f, Projectile.rotation, texture2.Size() / 2, Projectile.scale / 4 * 1.75F, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture2, Center + Projectile.velocity.PerfectNormalize() * 40 * Projectile.scale, null, new Color(255, 255, 255, 155), Projectile.rotation + MathHelper.PiOver2, texture2.Size() / 2, Projectile.scale / 4 * 1.75F, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(texture2, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture2.Size() / 2, Projectile.scale / 4, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(texture2, Center + Projectile.velocity.PerfectNormalize() * 40 * Projectile.scale, null, new Color(100, 255, 100, 155)*0.5f, Projectile.rotation + MathHelper.PiOver2, texture2.Size() / 2, Projectile.scale / 4 * 1.75F, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                else
                {
                    if (Projectile.spriteDirection == 0)
                    {
                        Main.spriteBatch.Draw(texture2, Center, null, Color.White, Projectile.rotation, texture2.Size() / 2, Projectile.scale / 4, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture2, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture2.Size() / 2, Projectile.scale / 4, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }

            }
            else
            {
                Projectile.alpha += 10;
            }
            {
                if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
                    DDHelper.BackAndForth(0.4F, 1F, 0.05F, ref T, ref Bool, true);
                else
                    DDHelper.BackAndForth(0.3F, 0.75F, 0.03F, ref T, ref Bool, true);
                for (int A = 0; A < Projectile.MeleeProj().oldVels.Length; A += 3)
                {
                    vector = Projectile.Player().ArmCenter();
                    if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
                    {
                        vector = Projectile.MeleeProj().oldPlayer;
                    }
                    if (Projectile.MeleeProj().oldVels[A] != Vector2.Zero && A < 15)
                    {
                        Texture2D texture = DDTextures.Starlight3.Value;
                        Color color = new Color(173, 255, 170, 0) * T;
                        vector += Projectile.MeleeProj().oldVels[A].PerfectNormalize() * 90 * Projectile.scale;
                        if (Projectile.Player().HasBuff(ModContent.BuffType<TerraPower>()))
                        {
                            vector += Projectile.MeleeProj().oldVels[A].PerfectNormalize() * 80 * Projectile.scale;
                        }
                        if (A == 3)
                        {
                            vector -= Projectile.MeleeProj().oldVels[A].PerfectNormalize() * 25 * Projectile.scale;
                        }
                        if (A == 6)
                        {
                            vector -= Projectile.MeleeProj().oldVels[A].PerfectNormalize() * 40 * Projectile.scale;
                        }
                        if (A == 9)
                        {
                            vector -= Projectile.MeleeProj().oldVels[A].PerfectNormalize() * 15 * Projectile.scale;
                        }
                        if (A == 12)
                        {
                            vector -= Projectile.MeleeProj().oldVels[A].PerfectNormalize() * 20 * Projectile.scale;
                        }
                        Main.spriteBatch.Draw(texture, vector - Main.screenPosition, null, color, 0, new Vector2(texture.Width, texture.Height) / 2, new Vector2(0.5F, 1.5F) * T * (1 - (float)A / Projectile.MeleeProj().oldVels.Length), 0, 0f);
                        Main.spriteBatch.Draw(texture, vector - Main.screenPosition, null, color, MathHelper.PiOver2, new Vector2(texture.Width, texture.Height) / 2, new Vector2(0.75F, 3.5F) * T * (1 - (float)A / Projectile.MeleeProj().oldVels.Length), 0, 0f);
                    }
                }
            }
            return false;
        }

        private float T;
        private bool Bool;
    }
    public class TerraSlash : ModProjectile
    {
        public override string Texture => "DDmod/Image/VoidStar";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terra Slash");
            //DisplayName.AddTranslation(7, "泰拉斩");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Projectile.width = 8;
            Projectile.height = 90;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 20;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 1.4f;
            Projectile.ArmorPenetration = 100000;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[1] != 0)
            {
                return target.whoAmI == (int)Projectile.ai[0];
            }
            return null;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;

            for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
            {
                if (Main.rand.NextBool(10))
                {
                    int Type = ModContent.DustType<速度粒子>();
                    Dust dust = Main.dust[NewDust(Projectile.position + Projectile.Size / 4 + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(96, 248, 96, 0))];
                    dust.noGravity = true;
                    dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2();
                    dust.scale = 1.3f;
                    dust.rotation = Projectile.velocity.ToRotation();
                }
            }
        }
        public override void OnKill(int timeLeft)
        {

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }

        private int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = DDTextures.Starlight3.Value;
            Color color = new Color(96, 248, 96, 0);
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int a = 0; a < 3; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.5f, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale / 3, Projectile.scale * 4), spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.5f, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale / 3, Projectile.scale * 4) / 2, spriteEffects, 0f);
            }
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num2 = 0.75f;
            float LaserLength = MathHelper.Lerp(0, Projectile.height, num2);
            Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), 0, default);
            float num = 0f;
            bool T = false;
            if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center + Pvelocity * LaserLength, projHitbox.Width, ref num))
            {
                T = true;
            }
            if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center - Pvelocity * LaserLength, projHitbox.Width, ref num))
            {
                T = true;
            }
            return new bool?(T);

        }
    }
}
