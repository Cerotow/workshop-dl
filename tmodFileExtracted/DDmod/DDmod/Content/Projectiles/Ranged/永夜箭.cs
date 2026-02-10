
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 永夜箭 : ModProjectile
    {
        public Asset<Texture2D> Glow;
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
            Projectile.penetrate = 1;
            DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>(Texture+"_Glow");
            DDGlobalProjectile.ScaleGlow[Projectile.type] =4;
            DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255,255,255,0);
            Projectile.soundDelay = 2;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void AI()
        {
            /*
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 1;
                Dust dust = Main.dust[NewDust(Projectile.Center+ Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity-new Vector2(4), 1, 1, 27)];
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }*/
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if(Projectile.DProj().Times[0]>0)
            {
                Projectile.DProj().Times[0] -= Projectile.velocity.Length();
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.ai[0] == 2)
            {
                Projectile.Track(400, 20, 20, 10);
            }
            else if (Projectile.ai[0] == 0)
            {
                Projectile.extraUpdates = 2;
                if (Projectile.velocity.Length() > 6)
                {
                    Dust dust;
                    if (Projectile.soundDelay == 0)
                    {
                        Projectile.soundDelay = 2;
                        Vector2 vector = -Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0.3F, 0.5F)).PerfectNormalize()*36 * Main.rand.NextFloat(0.25F, 0.75F);
                        dust = Main.dust[NewDust(Projectile.PreviousCenter() + vector.PerfectNormalize() * 8 + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(81, 6, 233, 0))];
                        dust.noGravity = true;
                        dust.velocity = vector;
                        dust.alpha = 100;
                        dust.scale = 2+Projectile.velocity.Length()/18;
                        dust.customData = 6;
                        dust.rotation = dust.velocity.ToRotation();
                        GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;
                        vector = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(0.3F, 0.5F)).PerfectNormalize() * 36 * Main.rand.NextFloat(0.25F, 0.75F);
                        dust = Main.dust[NewDust(Projectile.PreviousCenter() + vector.PerfectNormalize() * 8 + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(81, 6, 233, 0))];
                        dust.noGravity = true;
                        dust.velocity = vector;
                        dust.alpha = 100;
                        dust.scale = 2+Projectile.velocity.Length() / 18;
                        dust.customData = 6;
                        dust.rotation = dust.velocity.ToRotation();
                        GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;
                    }
                    for (int a = 0; a < Projectile.velocity.Length() / 6; a++)
                    {
                        dust = Main.dust[NewDust(Projectile.Center - Projectile.velocity / (Projectile.velocity.Length() / 6) * a - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(81, 6, 233, 0))];
                        dust.noGravity = true;
                        dust.velocity = Vector2.Zero;
                        dust.alpha = 100;
                        dust.scale = 3;
                        dust.customData = 2;
                        dust.rotation = Projectile.velocity.ToRotation();
                    }

                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge, settings, Projectile.owner);
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.8F, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, 27)];
                dust.velocity = projDirection * Projectile.velocity.Length() / 18;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
            }
            for (int i = 0; i < 20; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.8F, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center-new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(81, 6, 233, 0))];
                dust.velocity = projDirection*Projectile.velocity.Length()/18;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 2.3f;
                dust.rotation = dust.velocity.ToRotation();
            }
            if (Projectile.ai[0] == 0)
            {
                Projectile.position = Projectile.oldPosition;
                Projectile.velocity = Projectile.oldVelocity;
                for (int a = 0; a < 12; a++)
                {
                    Vector2 vector = -Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0, 0.5F)) * Main.rand.NextFloat(0.1F, 0.75F);
                    Dust dust = Main.dust[NewDust(Projectile.Center + vector.PerfectNormalize() * 8 + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(81, 6, 233, 0))];
                    dust.noGravity = true;
                    dust.velocity = vector;
                    dust.alpha = 100;
                    dust.scale = Main.rand.NextFloat(2F, 3.8F);
                    dust.customData = 1.8f;
                    dust.rotation = dust.velocity.ToRotation();
                    vector = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, 0.5F)) * Main.rand.NextFloat(0F, 0.75F);
                    dust = Main.dust[NewDust(Projectile.Center + vector.PerfectNormalize() * 8 + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(81, 6, 233, 0))];
                    dust.noGravity = true;
                    dust.velocity = vector;
                    dust.alpha = 100;
                    dust.scale = Main.rand.NextFloat(3F, 5.8F);
                    dust.customData = 1.8f;
                    dust.rotation = dust.velocity.ToRotation();
                }
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(172, 77, 248, 0)*0.5F,
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(172, 77, 248, 0) * 0.5F, (float)Math.Pow((double)completionRatio, 1.0)); ;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 40f * Projectile.scale, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.FireEffect2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D Glow = DDGlobalProjectile.Glow[Projectile.type].Value;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            if (Projectile.ai[0] == 0)
            {
                DDHelper.绘制偏移头部(texture, Projectile, Color.White, MathHelper.Pi, DDGlobalProjectile.Glow[Projectile.type].Value,10, 4);
                TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + Vector2.Normalize(-Projectile.velocity) * 28, 104, null);
            }
            else
            {
                DDHelper.绘制偏移头部(texture, Projectile, new Color(172, 77, 248,100)*0.6F, MathHelper.Pi);
                TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + Vector2.Normalize(-Projectile.velocity) * 28, 104, null,1,0.6F);
            }
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, Projectile.Size/2, 0, 0f);
            return false;
        }
        internal Trailing TrailDrawer;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 Pvelocity = Projectile.velocity.PerfectNormalize();
            projHitbox.X += (int)(Pvelocity.X * 14);
            projHitbox.Y += (int)(Pvelocity.Y * 14);
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            return null;
        }
    }
}
