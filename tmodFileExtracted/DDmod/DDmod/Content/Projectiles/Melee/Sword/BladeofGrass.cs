using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class BladeofGrass : ModProjectile
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
            Projectile.MeleeProj().oldVels2 = 54;
            Projectile.extraUpdates = 8;
            Projectile.DProj().Times[4] = 2.5F;
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

            Projectile.HoldProj(player, 48 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.05f * Projectile.DProj().Times[0], false, false);

            Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), -1, Projectile.DProj().Times[4], true);

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
                    if (Main.myPlayer == Projectile.owner)
                    {
                        NewProjectile(Projectile.GetSource_FromAI(), player.Center, Projectile.DProj().vector[0], 976, Projectile.damage / 2, 0, -1, Main.rand.NextFloat(-0.08F, 0.08F));
                    }
                    proj++;
                }
            }
            if (Projectile.localAI[1] >= 0)
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(10 * Projectile.scale))
                {
                    if (Main.rand.NextBool(20))
                    {
                        int Type = 40;
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 4;
                        dust.scale = 2f;
                    }
                }
                if (Main.myPlayer == Projectile.owner)
                    if (Main.rand.NextBool(30))
                {
                    NewProjectile(Projectile.GetSource_FromAI(), player.Center, Projectile.velocity.PerfectNormalize(), 976, Projectile.damage / 2, 0, -1, Main.rand.NextFloat(-0.08F, 0.08F));
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
            for (int A = 0; A < 30; A++)
            {
                int Type = 40;
                Dust dust = Main.dust[NewDust(target.Center, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.5F;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 5);
            }
        }
        Color color = new Color(40, 121, 13, 255);
        public float TWidth()
        {
            return 38*Projectile.scale;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                Projectile.ai[1]++;
                return false;
            }
            Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, this.color);
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

            Texture2D texture = TextureAssets.Item[ItemID.BladeofGrass].Value;

            Vector2 Center = Projectile.Center - Main.screenPosition;

            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                if (Projectile.spriteDirection == 0)
                {
                    Main.spriteBatch.Draw(texture, Center, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, Center, null, lightColor, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
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