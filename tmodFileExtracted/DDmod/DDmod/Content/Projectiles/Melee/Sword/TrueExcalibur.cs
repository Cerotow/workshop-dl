using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class TrueExcalibur : ModProjectile
    {
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Melee/Sword/TrueExcalibur_Glow");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            Projectile.width = 20;
            Projectile.height = 54;
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
            Projectile.MeleeProj().oldVels2 = 44;
            Projectile.extraUpdates = 6;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }

        int proj = 0;
        float S = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(S);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
            S = reader.ReadFloat();
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));
            Projectile.MeleeProj().oldVels2 = 68;
            if (Projectile.Player().Aplayer().TrueHolyEnergy)
            {
                Projectile.MeleeProj().oldVels2 = 110;
                Projectile.height = (int)(Projectile.height * 3f);
            }
            if (S > 4)
            {
                player.itemTime = 0;
                player.itemAnimation = 0;
                Projectile.Kill();
                return false;
            }
            else if (S > 2)
            {
                Projectile.extraUpdates = 16;
                Projectile.DProj().Times[4] = MathHelper.TwoPi * 4.5F;
                if (S == 3)
                {
                    Projectile.ClearInvincibleFrame();
                    S++;
                }
                if (proj != 0)
                    proj++;
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Projectile.Player().Aplayer().TrueHolyEnergy)
                    {
                        if (proj % 54 == 5)
                        {
                            int P = NewProjectile(Projectile.GetSource_FromThis(), player.Center + Projectile.velocity.PerfectNormalize() * 122, Projectile.velocity.PerfectNormalize() * 12, ModContent.ProjectileType<TrueExcaliburProj>(), (int)(Projectile.damage*0.75f), Projectile.knockBack / 2, Projectile.owner);
                            Main.projectile[P].scale = 1.3f;
                        }
                    }
                    else
                    {
                        if (proj % 104 == 5)
                            NewProjectile(Projectile.GetSource_FromThis(), player.Center + Projectile.velocity.PerfectNormalize() * 82, Projectile.velocity.PerfectNormalize() * 12, ModContent.ProjectileType<TrueExcaliburProj>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack / 2, Projectile.owner);
                    }
                }
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), 80, Projectile.DProj().Times[4], true);
            }
            else
            {
                Projectile.extraUpdates = 16;
                Projectile.DProj().Times[4] = 2.4f;
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), -1, Projectile.DProj().Times[4], true);

            }

            Projectile.HoldProj(player, 48 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);

            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead;
            if (channeling)
            {
                Projectile.DProj().Times[2] = 0;
            }
            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0)
                {
                    PlaySound(SoundID.Item1, Projectile.position);
                    proj++;
                    S++;
                    /*
                    if (Projectile.owner == Main.myPlayer)
                    {
                        if (S == 1)
                        {
                            if (Main.dayTime)
                            {
                                for (int A = -2; A <= 2; A++)
                                {
                                    int P = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(0.2f * A) * 14, 156, (int)(Projectile.damage * 1.2f), Projectile.knockBack / 2, Projectile.owner);
                                    Main.projectile[P].scale = 1.5f;
                                }
                            }
                            else
                            {
                                for (int A = -1; A <= 1; A++)
                                {
                                    NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(0.2f * A) * 17, 156, Projectile.damage, Projectile.knockBack / 2, Projectile.owner);
                                }
                            }
                        }
                        else if (S == 2)
                        {
                            if (Main.dayTime)
                            {
                                for (int A = -2; A <= 2; A++)
                                {
                                    if (A != 0)
                                    {
                                        int P = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(0.2f * A) * 17, ModContent.ProjectileType<TrueExcaliburProj>(), (int)(Projectile.damage * 1.2f), Projectile.knockBack / 2, Projectile.owner);
                                        Main.projectile[P].scale = 1.5f;
                                    }
                                }
                            }
                            else
                            {
                                for (int A = -1; A <= 1; A++)
                                {
                                    if (A != 0)
                                        NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].PerfectNormalize().RotatedBy(0.2f * A) * 17, ModContent.ProjectileType<TrueExcaliburProj>(), Projectile.damage, Projectile.knockBack / 2, Projectile.owner);
                                }
                            }
                        }
                    }*/
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.localAI[1] >= 0)
            {
                for (int A = -Projectile.height / 2; A <= Projectile.height / 2; A += (int)(14 * Projectile.scale))
                {
                    if (Main.rand.NextBool(12))
                    {
                        Color color = new Color(217, 68, 200);
                        if (S > 2)
                        {
                            color = new Color(255, 255, 104);
                        }
                        else if (S > 1)
                        {
                            color = new Color(90, 182, 255);
                        }
                        else if (S == 0)
                        {
                            color = new Color(255, 255, 104);
                        }
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, ModContent.DustType<光球粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 255, color)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2();
                        dust.scale = 0.7f;
                    }
                }
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] == 1 ? 0 : 1;
            return false;
        }
        public override void OnKill(int timeLeft)
        { }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (S != 0 && S <= 2)
            {
                modifiers.SourceDamage *= 2;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueExcalibur, settings, Projectile.owner);
            if (S != 0 && S <= 2)
            {
                if (proj == 1)
                {
                    proj++;
                    if (Projectile.Player().Aplayer().TrueHolyEnergy)
                    {
                        for (int A = -3; A <= 3; A++)
                        {
                            Vector2 vector = Main.rand.NextVector2Unit() * 200;
                            NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector/50, ModContent.ProjectileType<ExcaliburProj>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner,1,0, target.whoAmI);
                        }
                    }
                    else
                    {
                        for (int A = -1; A <= 1; A++)
                        {
                            Vector2 vector = Main.rand.NextVector2Unit() * 200;
                            NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector/50, ModContent.ProjectileType<ExcaliburProj>(), Projectile.damage / 3, Projectile.knockBack / 2, Projectile.owner,1, 0, target.whoAmI);
                        }
                    }
                }
            }
        }
        Color color = new Color(236, 200, 19, 0);
        Color color2 = new Color(57, 115, 210, 0);
        Color color3 = new Color(215, 53, 203, 0);
        public float TWidth()
        {
            if (Projectile.Player().Aplayer().TrueHolyEnergy)
            {
                return 64;
            }
            return 30;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1]++;
                return false;
            }
            Vector2 vector = Projectile.Player().ArmCenter();
            if (Projectile.MeleeProj().oldPlayer != Vector2.Zero)
            {
                vector = Projectile.MeleeProj().oldPlayer;
            }

            DDHelper.BladeTrail(DDTextures.WhitePng, new Color(100,100,100,255), 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale* TWidth());
            DDHelper.BladeTrail(DDTextures.WhitePng, [color3, color2, color], 0F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale* TWidth());

            DDHelper.BladeTrail(DDTextures.Wave, [color3, color2, color], 0F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale* TWidth());
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D texture_G = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            Vector2 Scale = new Vector2(Projectile.scale) ;
            if (S > 2)
            {
                Scale = new Vector2(Projectile.scale);
            }
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.Player().Aplayer().TrueHolyEnergy)
                {
                    if (Projectile.spriteDirection == 0)
                    {
                        Main.spriteBatch.Draw(Glow.Value, Center + Projectile.velocity.PerfectNormalize() * 34 * Projectile.scale, null, new Color(100, 100, 100, 255), Projectile.rotation, Glow.Value.Size() / 2, Projectile.scale * 1.75F, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(Glow.Value, Center + Projectile.velocity.PerfectNormalize() *34* Projectile.scale, null, new Color(100, 100, 100, 255), Projectile.rotation + MathHelper.PiOver2, Glow.Value.Size() / 2, Projectile.scale * 1.75F, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                Texture2D texture = TextureAssets.Item[ItemID.TrueExcalibur].Value;
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture_G, Center, null, new Color(200,200,200,0), Projectile.rotation, texture_G.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation, texture.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize(), null, new Color(200, 200, 200, 0), Projectile.rotation + MathHelper.PiOver2, texture_G.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                if (Projectile.Player().Aplayer().TrueHolyEnergy)
                {
                    if (Projectile.spriteDirection == 0)
                    {
                        Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize() * 34* Projectile.scale, null, color, Projectile.rotation, texture_G.Size() / 2, Scale * 1.75f, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize() * 34* Projectile.scale, null, new Color(19, 55, 236, 0) * 0.25f, Projectile.rotation, texture_G.Size() / 2, Scale * 1.75f, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize() * 34 * Projectile.scale, null, color, Projectile.rotation + MathHelper.PiOver2, texture_G.Size() / 2, Scale * 1.75f, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize() *34 * Projectile.scale, null, new Color(19, 55, 236, 0) * 0.25f, Projectile.rotation + MathHelper.PiOver2, texture_G.Size() / 2, Scale * 1.75f, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
    }
}
