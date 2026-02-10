using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class ShadowFlameSword : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 540;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            //projectile.light = 0.50f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.ownerHitCheck = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            Projectile.scale = 0.1f;
            Projectile.MeleeProj().SwordHitbox = true;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 88;
            Projectile.MeleeProj().oldVels3 = -0.02f;
            Projectile.extraUpdates = 6;
        }
        int proj = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(proj);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            proj = reader.ReadInt32();
        }
        public override bool PreAI()
        {
            //大小加成
            Player player = Main.player[Projectile.owner];
            Projectile.scale = player.GetAdjustedItemScale(player.ActiveItem());
            Projectile.Resize((int)(Projectile.OriginalWidth() * Projectile.scale), (int)(Projectile.OriginalHeight() * Projectile.scale));

            Projectile.HoldProj(player, 62 * Projectile.scale, Projectile.ai[0], Projectile.DProj().vector[0], MathHelper.PiOver4, 0, true, 0.04f * Projectile.DProj().Times[0], false, false);
            Projectile.HoldSword2(player, -player.HeldItem.useAnimation * (Projectile.extraUpdates+1), 360, 2.2f, false);
            int useTime = (int)(player.HeldItem.useTime / player.GetTotalAttackSpeed(DamageClass.Melee));
            if (Projectile.localAI[1] >= 0 && Projectile.ai[1] == 2 && Projectile.DProj().Times[2] < useTime)
            {
                Projectile.ai[1] = 3;

                PlaySound(SoundID.Item1, Projectile.position);
            }
            if (Projectile.localAI[1] <= 0 || Projectile.MeleeProj().DelayedKill > 0)
            {
                proj = 0;
            }
            else
            {
                if (proj == 0)
                {
                    proj++;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        NewProjectile(Projectile.GetSource_FromThis(), player.Center, Projectile.DProj().vector[0].PerfectNormalize() * 30, ModContent.ProjectileType<ShadowBlaze>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
                        Projectile.netUpdate = true;
                    }
                }
            }

            if (Projectile.scale>0.5F)
            {
                for (int A = -Projectile.height / 2; A < Projectile.height / 2; A += (int)(4 * Projectile.scale))
                {
                    Lighting.AddLight(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), color.ToVector3());
                    Lighting.AddLight(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), color.ToVector3());
                    if (Main.rand.NextBool(30)&& Projectile.localAI[1] >= 1)
                    {
                        int Type = 27;
                        Dust dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (A + 10 * Projectile.scale), Projectile.height / 4, Projectile.height / 4, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 2;
                        dust.scale = 1.5f;
                    }
                }
            }
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 300);
        }

        Color color = new Color(131 ,56, 233, 120);
        Color color2 = new Color(81, 6, 233, 120);
        public float TWidth()
        {
            return 44 * Projectile.scale;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] < 2)
            {
                return false;
            }
            Vector2 vector = Projectile.Player().Center;
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
            if (Projectile.MeleeProj().DelayedKill <= 0)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Vector2 Center = Projectile.Center - Main.screenPosition;
                Main.spriteBatch.Draw(texture, Center, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);
            }
            else
            {
                Projectile.alpha += 10;
            }
            return false;
        }
    }
}