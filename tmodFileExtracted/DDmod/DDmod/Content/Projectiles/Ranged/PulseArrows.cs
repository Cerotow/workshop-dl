
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.Spear.Proj;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class PulseArrows : ModProjectile
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
            Projectile.penetrate = 8;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Ranged/PulseArrows_Glow");
            DDGlobalProjectile.ScaleGlow[Projectile.type] = 4;
            DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255, 255, 255, 0);
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void AI()
        {
            if (Projectile.ai[0] > 0)
            {
                Projectile.penetrate = 1;

                Dust dust = Main.dust[NewDust(Projectile.PreviousCenter() + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(100, 255, 255, 0))];
                dust.noGravity = true;
                dust.velocity = -Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0.3F, 0.5F)) * Main.rand.NextFloat(0.25F, 0.75F);
                dust.alpha = 100;
                dust.scale = 2;
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;

                dust = Main.dust[NewDust(Projectile.PreviousCenter() + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(100, 255, 255, 0))];
                dust.noGravity = true;
                dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(0.3F, 0.5F)) * Main.rand.NextFloat(0.25F, 0.75F);
                dust.alpha = 100;
                dust.scale = 2;
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;


                dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(100, 255, 255, 0))];
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.alpha = 100;
                dust.scale = 3;
                dust.customData = 2;
                dust.rotation = Projectile.velocity.ToRotation();

                dust = Main.dust[NewDust(Projectile.Center + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(100, 255, 255, 0))];
                dust.noGravity = true;
                dust.velocity = -Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0F, 0.5F)) * Main.rand.NextFloat(0.05F, 0.5F);
                dust.alpha = 100;
                dust.scale = 2;
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();

                dust = Main.dust[NewDust(Projectile.Center + Vector2.Normalize(Projectile.velocity) * 16 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(100, 255, 255, 0))];
                dust.noGravity = true;
                dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(0F, 0.5F)) * Main.rand.NextFloat(0.05F, 0.5F);
                dust.alpha = 100;
                dust.scale = 3;
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();
            }
            if (Main.rand.NextBool(5))
            {
                Dust dust = Main.dust[NewDust(Projectile.position + Projectile.velocity.PerfectNormalize() * 14, Projectile.width, Projectile.height, 226)];
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 0.8f;
            }
            DDHelper.BackAndForth(-1.5F, 1.5F, 0.5F, ref Projectile.ai[1], ref Projectile.DProj().Bool[0]);
            // if (Projectile.oldPos[1]!=Vector2.Zero)
            // Projectile.oldPos[1] += Projectile.velocity.RotatedBy(-Projectile.ai[1]);

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 2F);
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 226)];
                dust.velocity = projDirection;
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 1f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            Projectile.penetrate--;
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Charged2>(), 180);
            if (Projectile.ai[0] <= 0)
            {
                for (int A = 0; A < Main.rand.Next(2, 5); A++)
                {
                    int a = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 8, ModContent.ProjectileType<弓闪电>(), Projectile.damage / 4, Projectile.knockBack, -1, 0, 0.75F, Main.rand.Next(50, 90));
                    Main.projectile[a].DamageType = DamageClass.Ranged;
                }
            }
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 100; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 6F);
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 226)];
                dust.velocity = projDirection;
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 1.6f;
            }
            if (Projectile.ai[0] > 0)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int a = 0; a < 6; a++)
                    {
                        NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 8, ModContent.ProjectileType<Electric>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
            SoundStyle sound = SoundID.Item14;
            sound.Pitch = -1F;
            PlaySound(sound, Projectile.Center);
            NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.001F, ModContent.ProjectileType<ElectricExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
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
            return 10f;
        }
        internal Color ColorFunction2(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(0, 186, 242, 0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(0, 186, 242, 0), (float)Math.Pow((double)completionRatio, 1.0)) * 0.3f;
        }
        internal float WidthFunction2(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 30f, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.LightningTrailing);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * (texture.Height / 2 - 2), 88, null, Projectile.scale);
            if (TrailDrawer2 == null)
            {
                TrailDrawer2 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction2), new Trailing.VertexColorFunction(ColorFunction2), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            TrailDrawer2.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2().PerfectNormalize() * (texture.Height / 2 - 2), 88, null, Projectile.scale);

            Color color = Color.White;
            color.A = 0;
            Main.spriteBatch.Draw(DDGlobalProjectile.Glow[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, DDGlobalProjectile.Glow[Projectile.type].Size() / 2, Projectile.scale / DDGlobalProjectile.ScaleGlow[Projectile.type], 0, 0f);
            color.A = 255;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);

            return false;
        }
        internal static Trailing TrailDrawer;
        internal static Trailing TrailDrawer2;
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
    public class ElectricExplosion : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3;
            Projectile.tileCollide = false;
            Projectile.arrow = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;

        }

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Electric Explosion");
            //DisplayName.AddTranslation(7, "电磁爆");
        }
        public override void AI()
        {
            Projectile.ProjScaleChange();
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Charged2>(), 180);
            Projectile.netUpdate = true;
        }
    }
    public class Electric : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.timeLeft = 90;
            Projectile.extraUpdates = Projectile.timeLeft / 30;
            Projectile.DProj().Times[1] = 90;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 90;
        }

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Electric");
            //DisplayName.AddTranslation(7, "电");
        }
        public override void AI()
        {
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity / 3;
                Projectile.velocity = Projectile.DProj().vector[0].RotatedBy(Main.rand.NextFloat(-1F, 1F));
            }
            if (Main.rand.NextBool(10))
            {
                Projectile.velocity = Projectile.DProj().vector[0].RotatedBy(Main.rand.NextFloat(-1F, 1F));
            }
            if (Projectile.timeLeft == 1)
            {
                Projectile.timeLeft = 100000;

                for (int i = 0; i < 40; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0, 4F);
                    Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 226)];
                    dust.velocity = projDirection;
                    dust.noGravity = false;
                    dust.alpha = 100;
                    dust.scale = 1f;
                }
                SoundStyle sound = SoundID.Item14;
                sound.Pitch = -1F;
                PlaySound(sound, Projectile.Center);
                int A = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.001F, ModContent.ProjectileType<ElectricExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[A].scale = 0.75F;
                Main.projectile[A].ProjScaleChange();
            }
            if (Projectile.timeLeft > 1000)
            {
                Projectile.timeLeft = 10000;
                Projectile.velocity = Vector2.Zero;
                if (Projectile.oldPos[Projectile.oldPos.Length - 1] == Projectile.position)
                {
                    Projectile.Kill();
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Charged2>(), 180);
            Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.timeLeft = 2;
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = DDTextures.MiniVoidStar.Value;
            Color color = new Color(0, 186, 242, 0);
            Vector2 vector = Projectile.Size / 2;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Main.spriteBatch.Draw(texture, vector2, null, new Color(0, 186, 242, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) + 0.2f, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, new Color(255, 70, 15, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) / 2 + 0.1f, spriteEffects, 0f);
            }
            return false;
        }
    }
    public class 弓闪电 : 闪电
    {
        public override void AI()
        {
            if (V2 == null)
            {
                V2 = new List<Vector2>();
            }
            if (Projectile.ai[2] != 0)
            {
                Projectile.timeLeft = (int)Projectile.ai[2];
                Projectile.localAI[2] = Projectile.ai[2];
                Projectile.ai[2] = 0;
            }
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity.PerfectNormalize() * 3;
                Projectile.velocity = Projectile.DProj().vector[0];
            }
            if (Projectile.timeLeft < 2)
            {
                if (Vector == null)
                {
                    Vector = new Vector2[Projectile.oldPos.Length];
                    for (int i = 0; i < Projectile.oldPos.Length; i++)
                    {
                        Vector[i] = Projectile.oldPos[i];
                    }
                }
                Projectile.timeLeft = 10000;
            }
            else
            if (Projectile.timeLeft > 1000)
            {
                Projectile.extraUpdates = 15;
                Projectile.damage = 0;
                Projectile.timeLeft = 10000;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale -= 0.004F;
                if (Projectile.scale <= 0)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                Projectile.scale = Projectile.ai[1];
                V2.Add(Projectile.position);
                Projectile.DProj().Times[0]++;
                if (Projectile.DProj().Times[0] > 20 && Main.rand.NextBool(10) && Projectile.DProj().track > 3)
                {
                    Projectile.DProj().Times[0] = 0;
                    Vector2 vector1 = Utils.RotatedBy(Projectile.DProj().vector[0].PerfectNormalize() * Projectile.velocity.Length(), Main.rand.NextFloat(-0.6F, 0.6F), default);
                    Projectile.velocity = vector1;

                    Projectile.netUpdate = true;
                }
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Charged2>(), 30);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4, -14), 1, 1, 226)];
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(1, 4);
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = Projectile.ai[1];
            }
            SoundStyle sound = new SoundStyle("DDmod/NoContent/Sounds/Items/闪电");
            sound.Pitch = -0.1f;
            sound.Volume = .1f;
            PlaySound(sound, Projectile.position);
            Projectile.netUpdate = true;
        }
    }
}