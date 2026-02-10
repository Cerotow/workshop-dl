using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class BreakerBlade : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 120;
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            Projectile.width = 20;
            Projectile.height = 80;
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
            Projectile.MeleeProj().oldVels2 = 72;
            Projectile.scale = 2.5f;
            Projectile.extraUpdates = 6;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        public override bool PreAI()
        {

            Player player = Main.player[Projectile.owner];

            Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            Projectile.width = (int)(20 * Projectile.scale);
            Projectile.height = (int)(80 * Projectile.scale);
            Projectile.HoldProj(player, 60 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);
            Projectile.HoldSword2(player, -player.HeldItem.useAnimation* (Projectile.extraUpdates+1), -1, 2.5f, false);

            int useTime = (int)(player.HeldItem.useTime / player.GetTotalAttackSpeed(DamageClass.Melee));
            if (Projectile.localAI[1] >= 0 && Projectile.ai[1] == 2 && Projectile.DProj().Times[2] < useTime)
            {
                Projectile.ai[1] = 3;

                TryGetActiveSound(PlaySound(SoundID.Item71, Projectile.position), out var Sound);
            }
            if (Projectile.localAI[1] >= 0)
            {
                //Eye2();
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
                {
                    if (Main.rand.NextBool(10))
                    {
                        int Type = 6;
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 2;
                        dust.scale = 1.5f;
                    }
                }
            }
            Projectile.spriteDirection = Projectile.DProj().Times[0] > 0 ? 0 : 1;
            return false;
        }
        public void Eye2()
        {
            Vector2 position = Projectile.Center;
            int radius = 4;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(y + position.Y / 16.0f);

                    for (int A = -Projectile.height / 2 / 16; A < Projectile.height / 2 / 16; A += radius)
                    {
                        Vector2 vector = new Vector2(xPosition, yPosition);
                        vector += Projectile.velocity.PerfectNormalize() * (A + 4);
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
        public override void OnKill(int timeLeft)
        {
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.life / target.lifeMax > 0.9F)
            {
                modifiers.SourceDamage *= 2;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            for (int A = 0; A < 50; A++)
            {
                int Type = 6;
                Dust dust = Main.dust[NewDust(target.Center, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.3f;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 8);
                dust.color = new Color(255, 255, 255, 0);
            }
            Color color = new Color(145, 120, 105,100);
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A2 = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A2].DProj().color = color;
            Main.projectile[A2].localAI[1] = 0.5F;
        }
        Color color = new Color(85, 70,55, 0);
        public float TWidth()
        {
            return 46 * Projectile.scale;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1]++;
                return false;
            }
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["刀光"]);
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

            Texture2D texture = TextureAssets.Item[426].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            if (Projectile.MeleeProj().DelayedKill <= 0)
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
                Projectile.alpha += 10;
            }
            return false;
        }
    }
}