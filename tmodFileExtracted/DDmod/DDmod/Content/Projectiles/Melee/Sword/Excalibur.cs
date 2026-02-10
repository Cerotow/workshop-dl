using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class Excalibur : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            Projectile.width = 20;
            Projectile.height = 58;
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
        }

        int proj = 0;
        float S = 0;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));
            if (Projectile.Player().Aplayer().HolyEnergy)
            {
                Projectile.height = (int)(Projectile.height * 2.5f);
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
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), 80, Projectile.DProj().Times[4], true);
            }
            else if (S >= 0)
            {
                Projectile.extraUpdates = 16;
                Projectile.DProj().Times[4] = 2.4f;
                Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), -1, Projectile.DProj().Times[4], true);
            }

            Projectile.HoldProj(player, 50 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);

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
                }
            }
            if (Projectile.localAI[1] >= 0)
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(14 * Projectile.scale))
                {
                    if (Main.rand.NextBool(15))
                    {
                        int Type = 57;
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2();
                        dust.scale = 0.8f;
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
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Excalibur, settings, Projectile.owner);
            if (S != 0 && S <= 2)
            {
                if (proj == 1)
                {
                    proj++;
                    if (Projectile.Player().Aplayer().HolyEnergy)
                    {
                        for (int A = -3; A <= 3; A++)
                        {
                            Vector2 vector = Main.rand.NextVector2Unit() * 200;
                            NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 50, ModContent.ProjectileType<ExcaliburProj>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                        }
                    }
                    else
                    {
                        for (int A = -1; A <= 1; A++)
                        {
                            Vector2 vector = Main.rand.NextVector2Unit() * 200;
                            NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 50, ModContent.ProjectileType<ExcaliburProj>(), Projectile.damage / 3, Projectile.knockBack / 2, Projectile.owner);
                        }
                    }
                }
            }
        }
        Color color = new Color(236, 200, 89, 120) * 2;
        public float TWidth()
        {
            Projectile.MeleeProj().oldVels2 = 72;
            if (Projectile.Player().Aplayer().HolyEnergy)
            {
                Projectile.MeleeProj().oldVels2 = 118;
                return 44 * Projectile.scale;
            }
            return 28 * Projectile.scale;
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
            DDHelper.BladeTrail(DDTextures.WhitePng, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, TWidth());
            DDHelper.BladeTrail(DDTextures.Wave, color, 1F, Projectile.DProj().Times[0] > 0);
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, TWidth());
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D texture_G = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            Vector2 Scale = new Vector2(Projectile.scale);
            if (S > 2)
            {
                Scale = new Vector2(Projectile.scale);
            }
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.Player().Aplayer().HolyEnergy)
                {
                    if (Projectile.spriteDirection == 0)
                    {
                        Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize() * 38 * Projectile.scale, null, color, Projectile.rotation, texture_G.Size() / 2, Scale * 1.5f, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize() * 38 * Projectile.scale, null, color, Projectile.rotation + MathHelper.PiOver2, texture_G.Size() / 2, Scale * 1.5f, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                Texture2D texture = TextureAssets.Item[ItemID.Excalibur].Value;
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture_G, Center, null, new Color(50, 50, 50, 155), Projectile.rotation, texture_G.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    Main.spriteBatch.Draw(texture_G, Center, null, color, Projectile.rotation, texture_G.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation, texture.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize(), null, new Color(50, 50, 50, 155), Projectile.rotation + MathHelper.PiOver2, texture_G.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize(), null, color, Projectile.rotation + MathHelper.PiOver2, texture_G.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                if (Projectile.Player().Aplayer().HolyEnergy)
                {
                    if (Projectile.spriteDirection == 0)
                    {
                        Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize() * 38 * Projectile.scale, null, color * 0.5f, Projectile.rotation, texture_G.Size() / 2, Scale * 1.5f, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture_G, Center + Projectile.velocity.PerfectNormalize() * 38 * Projectile.scale, null, color * 0.5f, Projectile.rotation + MathHelper.PiOver2, texture_G.Size() / 2, Scale * 1.5f, (SpriteEffects)Projectile.spriteDirection, 0f);
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
