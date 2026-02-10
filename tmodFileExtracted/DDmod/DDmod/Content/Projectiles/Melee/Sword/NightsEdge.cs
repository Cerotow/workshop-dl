using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class NightsEdge : ModProjectile
    {
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            Projectile.width = 20;
            Projectile.height = 48;
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
        int GJ;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
            writer.Write(GJ);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
            GJ = reader.ReadInt32();
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

            if (GJ >= 3)
            {
                Projectile.height = (int)(Projectile.height * 1.5f);
            }

            Projectile.HoldProj(player, 42 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
            Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, 2.8f, false);

            /*
            bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead;
            if (channeling)
            {
                Projectile.DProj().Times[2] = 0;
            }*/
            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0)
                {
                    proj++;
                    if (player.Aplayer().NightEnergy)
                    {
                        GJ =4;
                    }
                    else
                    {
                        GJ = 1;
                    }
                    if (GJ >= 3)
                    {
                        SoundStyle sound = SoundID.Item71;
                        sound.Pitch = -0.5F;
                        PlaySound(sound, Projectile.position);
                    }
                    else
                    {
                        PlaySound(SoundID.Item1, Projectile.position);
                    }
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Projectile.DProj().MouseWorld = Main.MouseWorld;
                        Projectile.netUpdate = true;
                    }

                    Vector2 vector = Main.rand.NextVector2Unit() * 200;

                    if (Projectile.owner == Main.myPlayer)
                    {
                        if (!player.Aplayer().NightEnergy)
                        {
                            NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 10, ModContent.ProjectileType<NightsSlash>(), Projectile.damage / 2, 0, Projectile.owner);
                        }
                        else
                        {
                            if (GJ >= 3&&Main.hardMode)
                            {
                                int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0] * 3 * Projectile.scale, ModContent.ProjectileType<TrueNightBlade>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                                Main.projectile[A].scale *= 0.6f * Projectile.scale;
                            }
                            NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld,Projectile.DProj().vector[0].PerfectNormalize()*0.01F, ModContent.ProjectileType<NightExplode>(), Projectile.damage *2, Projectile.knockBack, Projectile.owner);
                            
                            float R = Main.rand.NextFloat(0, MathHelper.TwoPi);
                            vector = new Vector2(0, -1).RotatedBy(MathHelper.PiOver4 + R) * 500;
                            NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 20, ModContent.ProjectileType<NightsSlash>(), Projectile.damage / 2, 0, Projectile.owner);
                            vector = new Vector2(0, -1).RotatedBy(-MathHelper.PiOver4 + R) * 500;
                            NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 20, ModContent.ProjectileType<NightsSlash>(), Projectile.damage / 2, 0, Projectile.owner);
                            vector = new Vector2(0, 1).RotatedBy(-MathHelper.PiOver4 + R) * 500;
                            NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 20, ModContent.ProjectileType<NightsSlash>(), Projectile.damage / 2, 0, Projectile.owner);
                            vector = new Vector2(0, 1).RotatedBy(MathHelper.PiOver4 + R) * 500;
                            NewProjectile(Projectile.GetSource_FromThis(), Projectile.DProj().MouseWorld + vector, -vector / 20, ModContent.ProjectileType<NightsSlash>(), Projectile.damage / 2, 0, Projectile.owner);
                        }
                    }
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.localAI[1] >= 0)
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
                {
                    if (Main.rand.NextBool(5))
                    {
                        int Type = 27;
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2();
                        dust.scale = 1.3f;
                    }
                }
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge, settings, Projectile.owner);

                Vector2 vector = Main.rand.NextVector2Unit() * 60;
            NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 10, ModContent.ProjectileType<NightsSlash>(), Projectile.damage / 2, 0, Projectile.owner, target.whoAmI, 1);
            if (GJ >= 3)
            {
                vector = Main.rand.NextVector2Unit() * 60;
                NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 10, ModContent.ProjectileType<NightsSlash>(), Projectile.damage / 2, 0, Projectile.owner, target.whoAmI, 1);
            }
        }
        Color color = new Color(81, 6, 233, 50);
        Color color2 = new Color(81, 6, 233, 0);
        public float TWidth()
        {
            if (GJ <= 2)
            {
                Projectile.MeleeProj().oldVels2 = 50;
                return 30;
            }
            else
            {
                Projectile.MeleeProj().oldVels2 = 62;
                return 55;
            }
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
            if (GJ <= 2)
            {
                DDHelper.BladeTrail(DDTextures.WhitePng, color, 2F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale*TWidth());
                DDHelper.BladeTrail(DDTextures.Wave, color2, 1F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale*TWidth());
            }
            else
            {
                DDHelper.BladeTrail(DDTextures.WhitePng, color*2, 2F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale* TWidth());
                DDHelper.BladeTrail(DDTextures.远古背景2, color2, 1F, Projectile.DProj().Times[0] > 0);
                TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null, Projectile.scale * TWidth());
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D texture = TextureAssets.Item[ItemID.NightsEdge].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (GJ <= 2)
                {
                    if (Projectile.spriteDirection == 0)
                    {
                        Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                }
                else
                {
                   Color color = new Color(81, 6, 233,0)*2;
                    if (Projectile.spriteDirection == 0)
                    {
                        Main.spriteBatch.Draw(texture, Center + Projectile.velocity.PerfectNormalize() * 17 * Projectile.scale, null, new Color(100, 100, 100, 255), Projectile.rotation, texture.Size() / 2, Projectile.scale * 1.5F, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(texture, Center + Projectile.velocity.PerfectNormalize() * 17 * Projectile.scale, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1.5F, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, Center + Projectile.velocity.PerfectNormalize() * 17 * Projectile.scale, null, new Color(100, 100, 100, 255), Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale * 1.5F, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(texture, Center + Projectile.velocity.PerfectNormalize() * 17 * Projectile.scale, null, color, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale * 1.5F, (SpriteEffects)Projectile.spriteDirection, 0f);
                        Main.spriteBatch.Draw(texture, Center, null, Color.White, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
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
    public class NightsSlash : ModProjectile
    {
        public override string Texture => "DDmod/Image/VoidStar";
        public override void SetStaticDefaults()
        {
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
            Projectile.rotation = Projectile.velocity.ToRotation()-MathHelper.PiOver2;

            for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
            {
                if (Main.rand.NextBool(10))
                {
                    int Type = 27;
                    Dust dust = Main.dust[NewDust(Projectile.Center-new Vector2(Projectile.height/8) + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height/4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2();
                    dust.scale = 1.3f;
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
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = DDTextures.Starlight3.Value;
            Color color = new Color(81, 6, 233,12);
            Color color2 = new Color(81, 6, 233, 12);
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int a = 0; a < 3; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.5f, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale/3, Projectile.scale*4), spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color2 * 0.5f, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale/3, Projectile.scale*4)/2, spriteEffects, 0f);
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
            if(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center + Pvelocity * LaserLength, projHitbox.Width, ref num))
            {
                T = true;
            }
            if(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center - Pvelocity * LaserLength, projHitbox.Width, ref num))
            {
                T = true;
            }
            return new bool?(T);

        }
    }
}
