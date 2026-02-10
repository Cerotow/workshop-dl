
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class TrueFrostArrows : ModProjectile
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
            Projectile.coldDamage = true;
            DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Ranged/TrueFrostArrows_Glow");
            DDGlobalProjectile.ScaleGlow[Projectile.type] =4;
            DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255,255,255,0);
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 1;
                /*
                Dust dust = Main.dust[NewDust(Projectile.Center+ Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 135)];
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
                */
                Dust dust = Main.dust[NewDust(Projectile.PreviousCenter() + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(0, 155, 255, 0))];
                dust.noGravity = true;
                dust.velocity = -Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0.3F, 0.5F)) * Main.rand.NextFloat(0.25F, 0.75F);
                dust.alpha = 100;
                dust.scale = 2;
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;

                dust = Main.dust[NewDust(Projectile.PreviousCenter() + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(0, 155, 255, 0))];
                dust.noGravity = true;
                dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(0.3F, 0.5F)) * Main.rand.NextFloat(0.25F, 0.75F);
                dust.alpha = 100;
                dust.scale = 2;
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;


                dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(0, 155, 255, 0))];
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.alpha = 100;
                dust.scale = 3;
                dust.customData = 2;
                dust.rotation = Projectile.velocity.ToRotation();
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.velocity.Length() < 1)
            {
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.netUpdate = true;
            if (!target.Dnpc().BossPhysique && target.realLife < 0)
            {
                target.AddBuff(ModContent.BuffType<Freeze>(), (int)Main.rand.NextFloat(120 - Projectile.ai[0], 300 - Projectile.ai[0]));
            }
            else
            {
                target.AddBuff(ModContent.BuffType<Frozen>(), (int)Main.rand.NextFloat(120 - Projectile.ai[0], 300 - Projectile.ai[0]));
            }
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                /*
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.8F, 4.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 135)];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 1.3f;
                */
                Dust dust = Main.dust[NewDust(Projectile.oldPosition + Projectile.Size / 2 + Vector2.Normalize(Projectile.oldVelocity) * 32 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(0, 155, 255, 0))];
                dust.noGravity = true;
                dust.velocity = -Projectile.oldVelocity.RotatedBy(-Main.rand.NextFloat(0.23F, 0.75F)) * Main.rand.NextFloat(0.1F, 1F);
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(2, 4);
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();

                dust = Main.dust[NewDust(Projectile.oldPosition+Projectile.Size/2 + Vector2.Normalize(Projectile.oldVelocity) * 32 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(0, 155, 255, 0))];
                dust.noGravity = true;
                dust.velocity = -Projectile.oldVelocity.RotatedBy(Main.rand.NextFloat(0.23F, 0.75F)) * Main.rand.NextFloat(0.1F, 1F);
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(2,4);
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();
            }
            PlaySound(SoundID.Item27, Projectile.Center);
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(0, 186, 242, 0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(0, 186, 242, 0), (float)Math.Pow((double)completionRatio, 1.0)); ;
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
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2().PerfectNormalize()*(texture.Height/2-2), 104, null);

            Color color = Color.White;
            color.A = 0;
            Main.spriteBatch.Draw(DDGlobalProjectile.Glow[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, DDGlobalProjectile.Glow[Projectile.type].Size() / 2, Projectile.scale / DDGlobalProjectile.ScaleGlow[Projectile.type], 0, 0f);
            color.A = 255;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);

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
