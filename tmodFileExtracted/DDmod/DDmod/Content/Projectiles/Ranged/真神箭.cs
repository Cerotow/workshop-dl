
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 真神箭 : ModProjectile
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
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;

            //DDGlobalProjectile.ScaleGlow[Projectile.type] = 4;
            //DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255, 255, 255, 0);
            //DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Ranged/神箭_Glow");
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ProjScale();
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity;
            }
            float Speed = Projectile.DProj().vector[0].Length();
            DDHelper.BackAndForth(-0.5f * Speed, 0.5f * Speed, 0.1F* Speed, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0]);

            if (Projectile.ai[0] == 1)
            {
                if (Projectile.ai[1] == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(230, 77, 255))];
                        dust.velocity = projDirection;
                        dust.noGravity = true;
                        dust.customData = -2;
                        dust.alpha = 100;
                        dust.scale = Main.rand.NextFloat(0.8F, 1.3F);
                    }
                    Projectile.ai[1] = 1;
                }
                NPC result = null;
                float num = 1000;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy() && npc.Center.Y > Projectile.Center.Y)
                    {
                        if (Collision.CanHitLine(Projectile.position, 1, 1, npc.position, npc.width, npc.height))
                        {
                            float num2 = (Projectile.position - npc.Center).Length();
                            if (!(num <= num2))
                            {
                                num = num2;
                                result = npc;
                            }
                        }
                    }
                }
                if (result != null)
                    Projectile.DProj().vector[0] = (result.Center - Projectile.Center).PerfectNormalize() * Speed;
            }
            if (Projectile.DProj().vector[0].Length() < 16)
            {
                Projectile.DProj().vector[0] *= 1.05F;
            }
            else
            {
                Projectile.DProj().vector[0] = Projectile.DProj().vector[0].PerfectNormalize() * 16;
            }
            Projectile.velocity = Projectile.DProj().vector[0];
            Projectile.velocity += new Vector2(0, Projectile.DProj().Times[0]).RotatedBy(Projectile.DProj().vector[0].ToRotation());
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            //if (Projectile.ai[0] != 1)
            {
                Projectile.DProj().vector[0]= Projectile.DProj().vector[0].PerfectNormalize()*2;
                Projectile.damage = (int)(Projectile.damage * 0.6F);
            }
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueExcalibur, settings, Projectile.owner);
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
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center-new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(230, 77, 255,0))];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.customData = 2;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(0.8F,1.3F);
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(230, 77, 255,0)*0.5F,
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(230, 77, 255, 0) * 0.5F, (float)Math.Pow((double)completionRatio, 1.0)); ;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 10f * Projectile.scale, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["普通拖尾"]);
            }
            GameShaders.Misc["普通拖尾"].SetShaderTexture(DDTextures.FireEffect);
            GameShaders.Misc["普通拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + Vector2.Normalize(-Projectile.velocity) * 20, 104, null);
            DDHelper.绘制偏移头部(texture, Projectile,new Color(255,255,255,0), MathHelper.Pi);
            //if(Projectile.ai[0]==1)
            //DDHelper.绘制偏移头部(texture, Projectile, new Color(255, 255, 255, 0), MathHelper.Pi, DDGlobalProjectile.Glow[Projectile.type].Value,20,4);
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
    public class 真神箭2 : ModProjectile
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
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;

            //DDGlobalProjectile.ScaleGlow[Projectile.type] = 4;
            //DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255, 255, 255, 0);
            //DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Ranged/神箭_Glow");
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.ProjScale();
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity;
            }
            float Speed = Projectile.DProj().vector[0].Length();
            DDHelper.BackAndForth(-0.5f * Speed, 0.5f * Speed, 0.1F* Speed, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0]);

            if (Projectile.ai[0] == 1)
            {
                if (Projectile.ai[1] == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(36, 127, 255))];
                        dust.velocity = projDirection;
                        dust.noGravity = true;
                        dust.customData = -2;
                        dust.alpha = 100;
                        dust.scale = Main.rand.NextFloat(0.8F, 1.3F);
                    }
                    Projectile.ai[1] = 1;
                }
                NPC result = null;
                float num = 1000;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy() && npc.Center.Y > Projectile.Center.Y)
                    {
                        if (Collision.CanHitLine(Projectile.position, 1, 1, npc.position, npc.width, npc.height))
                        {
                            float num2 = (Projectile.position - npc.Center).Length();
                            if (!(num <= num2))
                            {
                                num = num2;
                                result = npc;
                            }
                        }
                    }
                }
                if (result != null)
                    Projectile.DProj().vector[0] = (result.Center - Projectile.Center).PerfectNormalize() * Speed;
            }
            if (Projectile.DProj().vector[0].Length() < 16)
            {
                Projectile.DProj().vector[0] *= 1.05F;
            }
            else
            {
                Projectile.DProj().vector[0] = Projectile.DProj().vector[0].PerfectNormalize() * 16;
            }
            Projectile.velocity = Projectile.DProj().vector[0];
            Projectile.velocity -= new Vector2(0, Projectile.DProj().Times[0]).RotatedBy(Projectile.DProj().vector[0].ToRotation());
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            //if (Projectile.ai[0] != 1)
            {
                Projectile.DProj().vector[0] = Projectile.DProj().vector[0].PerfectNormalize()*2;
                Projectile.damage = (int)(Projectile.damage * 0.6F);
            }
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueExcalibur, settings, Projectile.owner);
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
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(36, 127, 255,0))];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.customData = 2;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(0.8F, 1.3F);
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(36, 127, 255,0)*0.5F,
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(36, 127, 255, 0) * 0.5F, (float)Math.Pow((double)completionRatio, 1.0)); ;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 10f * Projectile.scale, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["普通拖尾"]);
            }
            GameShaders.Misc["普通拖尾"].SetShaderTexture(DDTextures.FireEffect);
            GameShaders.Misc["普通拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + Vector2.Normalize(-Projectile.velocity) * 20, 104, null);
            DDHelper.绘制偏移头部(texture, Projectile, new Color(255, 255, 255, 0), MathHelper.Pi);
           // if (Projectile.ai[0] == 1)
                //DDHelper.绘制偏移头部(texture, Projectile, new Color(255, 255, 255, 0), MathHelper.Pi, DDGlobalProjectile.Glow[Projectile.type].Value, 20, 4);
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
