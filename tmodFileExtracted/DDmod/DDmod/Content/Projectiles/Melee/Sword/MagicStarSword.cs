

using DDmod.Content.Dusts;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class MagicStarSword : ModProjectile
    { 
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            Projectile.width = 10;
            Projectile.height = 40;
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
            Projectile.MeleeProj().oldVels2 = 38;
            Projectile.extraUpdates = 12;
            Projectile.DProj().Times[4] = 2F;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
        }
        int proj;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.scale < player.GetAdjustedItemScale(player.ActiveItem()))
            {
                Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            }
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

            Projectile.HoldProj(player, 28 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);

            Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates + 1), -1, Projectile.DProj().Times[4], true);

            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0)
                {
                    PlaySound(SoundID.Item1, Projectile.position);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        for (int a = 0; a < 2; a++)
                        {
                            Vector2 vector = new Vector2(Main.rand.NextFloat(-150, 150), 300 + Main.rand.NextFloat(-52, 52));
                            int A = NewProjectile(Projectile.GetSource_FromThis(), player.Center - vector, Vector2.Zero, ModContent.ProjectileType<MagicStarCircleMelee>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0f, 0f);
                            if (Main.myPlayer == Projectile.owner)
                            {
                                Main.projectile[A].DProj().vector[0] = Main.MouseWorld - (player.Center - vector);
                            }
                            Main.projectile[A].netUpdate = true;
                        }
                    }
                    proj++;
                }
            }
            if (Projectile.localAI[1] >= 0)
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
                {
                    if (Main.rand.NextBool(50))
                    {
                        int Type = ModContent.DustType<星星粒子>();
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 4;
                        dust.scale = 0.75f;
                        dust.customData = 2;
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
            for (int A = 0; A < 20; A++)
            {
                int Type = ModContent.DustType<星星粒子>();
                Dust dust = Main.dust[NewDust(target.Center, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = false;
                dust.scale = 0.75f;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 3);
            }
        }
        Color color = new Color(0, 105, 255, 0)*0.5f;
        public float TWidth()
        {
            return 18 *Projectile.scale;
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

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

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