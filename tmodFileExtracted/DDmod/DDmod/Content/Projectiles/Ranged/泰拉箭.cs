
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Ranged
{
    public class 泰拉箭 : ModProjectile
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
            Projectile.timeLeft = 40;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;

            DDGlobalProjectile.ScaleGlow[Projectile.type] = 4;
            DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255, 255, 255, 0);
            DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Ranged/泰拉箭_Glow");
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
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.ai[1]++;
                Vector2 vector = new Vector2(Main.rand.NextFloat(-200, 200), -500);
                Vector2 vector1 = -vector.PerfectNormalize() * 12;
                int S = 4;
                if (Projectile.ai[1] % S == 0)
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + vector, vector1, ModContent.ProjectileType<泰拉箭幻影>(), Projectile.damage / 2, 0, Projectile.owner, 1);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] != 1)
            {
                Projectile.damage = (int)(Projectile.damage * 0.6F);
            }
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, settings, Projectile.owner);
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return null;
        }
        public override void OnKill(int timeLeft)
        {
            if ( Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 10; i++)
                {
                    Vector2 vector = new Vector2(Main.rand.Next(-600, 600), Main.rand.Next(-800, -200));
                    Vector2 vector1 = -vector.PerfectNormalize() * 12;
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + vector, vector1, ModContent.ProjectileType<泰拉箭幻影>(), Projectile.damage / 2, 0, Projectile.owner,1);
                }
            }
            for (int i = 0; i < 30; i++)
            {
                Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                Dust dust = Main.dust[NewDust(Projectile.Center-new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(71, 233, 60, 0))];
                dust.velocity = projDirection;
                dust.noGravity = true;
                dust.customData = -2;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(0.8F,1.3F);
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                 new Color(71, 233, 60, 0),
                 new Color(71, 233, 60, 0),
                 new Color(71, 233, 60, 0),
                 new Color(71, 233, 60, 0),
                 new Color(50, 207, 255,0)
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(50, 207, 255, 0), (float)Math.Pow((double)completionRatio, 1.0)); ;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 10f, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
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
            TrailDrawer.Draw(Projectile.oldPos, Projectile.Size * 0.5f - Main.screenPosition + Vector2.Normalize(-Projectile.velocity) * 20, 64, null,Projectile.scale);
            DDHelper.绘制偏移头部(texture, Projectile, new Color(255, 255, 255, 0), MathHelper.Pi, DDGlobalProjectile.Glow[Projectile.type].Value, 20, 4);
            DDHelper.绘制偏移头部(texture, Projectile,new Color(255,255,255,255), MathHelper.Pi);
            {
                texture = DDTextures.Starlight3.Value;
                Color color = new Color(173, 255, 170, 0) * Projectile.DProj().Times[1];
                vector += Projectile.velocity.PerfectNormalize() * 6;
                DDHelper.BackAndForth(0.4F, 1F, 0.05F, ref Projectile.DProj().Times[1], ref Projectile.DProj().Bool[0], true);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, 0, texture.Size() / 2, new Vector2(0.5F, 1.5F) * Projectile.DProj().Times[1] * 1F, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, MathHelper.PiOver2, texture.Size() / 2, new Vector2(0.75F, 3.5F) * Projectile.DProj().Times[1] * 1F, 0, 0f);
            }
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, Projectile.Size/2, 0, 0f);
            return false;
        }
        internal static Trailing TrailDrawer;
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
    public class 泰拉箭幻影 : ModProjectile
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
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.extraUpdates = 1;
           // DDGlobalProjectile.ScaleGlow[Projectile.type] = 4;
           // DDGlobalProjectile.GlowColor[Projectile.type] = new Color(255, 255, 255, 0);
            //DDGlobalProjectile.Glow[Projectile.type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Ranged/泰拉箭_Glow");
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
        }
        public override void AI()
        {
            Player player = Projectile.Player();
            if (Projectile.ai[0] == 2)
            {
                if(Projectile.localAI[0]==0)
                {
                    Projectile.localAI[0] = Projectile.velocity.Length();
                    Projectile.velocity *= 1;
                }
                Projectile.tileCollide = false;
                if(Projectile.velocity.Length()<0.1F)
                Projectile.ai[1]++;
                Projectile.rotation += 0.3F;
                if(Projectile.velocity.X<0)
                {
                    Projectile.rotation -= 0.6F;
                }
                Projectile.velocity *= 0.96F;
                if (Projectile.ai[1]>=8)
                {
                    Projectile.ai[1] = 0;
                    Projectile.ai[0] = 0;
                    Vector2 Center = Projectile.Center;
                    Vector2 vector = (player.Dplayer().MouseWorld - Center).PerfectNormalize() * Projectile.localAI[0];
                    Projectile.velocity = vector;
                    Projectile.DProj().Times[0] = (player.Dplayer().MouseWorld - Center).Length();
                    Projectile.netUpdate = true;
                }
                return;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if(Projectile.ai[0] == 0)
            {
                Projectile.penetrate = 1;
            }
            if (Projectile.DProj().Times[0] > 0)
            {
                Projectile.DProj().Times[0] -= Projectile.velocity.Length();
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.tileCollide = true;
            }
            Projectile.ProjScaleChange();
            float Speed = Projectile.velocity.Length();
            if (Projectile.ai[1] == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(71, 233, 60,0))];
                    dust.velocity = projDirection;
                    dust.noGravity = true;
                    dust.customData = -2;
                    dust.alpha = 100;
                    dust.scale = Main.rand.NextFloat(0.8F, 1.3F);
                }
                for (int i = 0; i < 4; i++)
                {
                    Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<星光粒子>(), newColor: new Color(141, 233, 130,0))];
                    dust.velocity = projDirection;
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(0.8F, 1.3F);
                }
                Projectile.ai[1] = 1;
            }
            NPC result = null;
            float num = 1000;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy())
                {
                    bool r = true;
                    for (int Y = 0; Y < vs.Count; Y++)
                    {
                        if (i == vs[Y])
                        {
                            r = false;
                            break;
                        }
                    }
                    float num2 = (Projectile.position - npc.Center).Length();
                    if (r && !(num <= num2))
                    {
                        if (Collision.CanHitLine(Projectile.position, 1, 1, npc.position, npc.width, npc.height))
                        {
                            num = num2;
                            result = npc;
                        }
                    }
                }
            }
            if (result != null)
            {
                if (Projectile.ai[0] == 0)
                {
                    Projectile.velocity = (Projectile.velocity * 10 + (result.Center - Projectile.Center).PerfectNormalize() * Speed) / 11;
                }
                else
                {
                    if (vs.Count > 0)
                    {
                        Projectile.velocity = (Projectile.velocity * 20 + (result.Center - Projectile.Center).PerfectNormalize() *8) / 21;
                    }
                    else
                    {
                        Projectile.velocity = (result.Center - Projectile.Center).PerfectNormalize() * Speed;
                    }
                }
            }
            else
            {
                if (vs.Count > 0)
                    Projectile.Kill();
            }

            if (Target >= 0)
            {
                result = Main.npc[Target];
                Projectile.velocity = (result.Center - Projectile.Center).PerfectNormalize() * Speed;
                if (!result.active)
                {
                    Target = -1;
                }
            }
            if (vs.Count == 0)
            {
                if (Projectile.velocity.Length() < 16)
                {
                    Projectile.velocity *= 1.05F;
                }
                else
                {
                    Projectile.velocity = Projectile.velocity.PerfectNormalize() * 16;
                }
            }
            else
            {
                if (Projectile.velocity.Length() > 8)
                    Projectile.velocity = Projectile.velocity.PerfectNormalize() * 8;
                Projectile.extraUpdates = 5;
            }
        }
        List<int> vs = new List<int>();
        int Target = -1;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 1)
            {
                if (vs.Count == 0)
                {
                    for (int a = 0; a < Projectile.oldPos.Length; a++)
                    {
                        Projectile.oldPos[a] = Vector2.Zero;
                    }
                }
                vs.Add(target.whoAmI);
            }
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, settings, Projectile.owner);
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            if (Projectile.ai[0] == 2)
            {
                return false;
            }
                return null;
        }
        public override bool? CanHitNPC(NPC target)
        {
            bool? r = null;
            for (int Y = 0; Y < vs.Count; Y++)
            {
                if (target.whoAmI == vs[Y]||target.friendly)
                {
                    r = false;
                }
            }
            return r;
        }
        public override void OnKill(int timeLeft)
        {
            for (int a = 0; a < Projectile.oldPos.Length; a++)
            {
                if (a == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                        Dust dust = Main.dust[NewDust(Projectile.oldPos[a] + Projectile.Size - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), newColor: new Color(71, 233, 60, 0))];
                        dust.velocity = projDirection;
                        dust.noGravity = true;
                        dust.customData = -1;
                        dust.alpha = 100;
                        dust.scale = Main.rand.NextFloat(0.8F, 1F) * (1 - (float)a / Projectile.oldPos.Length);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(Vector2.One, Main.rand.NextFloat(0, MathHelper.TwoPi), default) * Main.rand.NextFloat(0.3F, 2.2F);
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4), 1, 1, ModContent.DustType<星光粒子>(), newColor: new Color(141, 233, 130, 0))];
                        dust.velocity = projDirection;
                        dust.noGravity = true;
                        dust.scale = Main.rand.NextFloat(0.8F, 1.3F);
                    }
                }
                if ((a > 0 && vs.Count > 0))
                {
                    int type = ModContent.DustType<光球粒子>();
                    Vector4 vector4 = new Vector4(71, 233, 60, 0) / 2 * (1 - (float)a / Projectile.oldPos.Length) + new Vector4(50, 207, 255, 0) / 2 * ((float)a / Projectile.oldPos.Length);
                    Color color = new Color((byte)vector4.X, (byte)vector4.Y, (byte)vector4.Z, (byte)vector4.W);
                    if (Main.rand.NextBool(4))
                    {
                        type = ModContent.DustType<星光粒子>();
                    }
                    Vector2 projDirection = (Projectile.oldPos[a] - Projectile.oldPos[a - 1]).PerfectNormalize() * -2;
                    Vector2 vector = new Vector2(20, 20) * (1 - (float)a / Projectile.oldPos.Length + 0.2F);
                    Dust dust = Main.dust[NewDust(Projectile.oldPos[a] + Projectile.Size - vector - new Vector2(4), (int)vector.X * 2, (int)vector.Y * 2, type, newColor: color)];
                    dust.velocity = projDirection;
                    dust.noGravity = true;
                    dust.customData = -1;
                    dust.alpha = 100;
                    dust.scale = Main.rand.NextFloat(0.8F, 1F) * (1 - (float)a / Projectile.oldPos.Length + 0.4F);
                }
            }
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                 new Color(71, 233, 60, 0),
                 new Color(71, 233, 60, 0),
                 new Color(71, 233, 60, 0),
                 new Color(71, 233, 60, 0),
                 new Color(50, 207, 255,0)
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(50, 207, 255, 0), (float)Math.Pow((double)completionRatio, 1.0)); ;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 10f, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
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

            if (Projectile.ai[0] == 2)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size()/2, Projectile.scale, 0, 0f);
                return false;
            }
            Vector2[] PO = new Vector2[10];
            for (int a = 0; a <10; a++)
            {
                PO[a]= Projectile.oldPos[a];

            }
            if (vs.Count == 0)
            {
                TrailDrawer.Draw(PO, Projectile.Size * 0.5f - Main.screenPosition + Vector2.Normalize(-Projectile.velocity) * 16, 32, null, Projectile.scale);
                DDHelper.绘制偏移头部(texture, Projectile, new Color(255, 255, 255, 0), MathHelper.Pi);
                texture = DDTextures.Starlight3.Value;
                Color color = new Color(173, 255, 170, 0) * Projectile.DProj().Times[1];
                vector += Projectile.velocity.PerfectNormalize() * 6;
                DDHelper.BackAndForth(0.4F, 1F, 0.05F, ref Projectile.DProj().Times[1], ref Projectile.DProj().Bool[0], true);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, 0, texture.Size() / 2, new Vector2(0.5F, 1.5F) * Projectile.DProj().Times[1] * 0.6F, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color, MathHelper.PiOver2, texture.Size() / 2, new Vector2(0.75F, 3.5F) * Projectile.DProj().Times[1] * 0.6F, 0, 0f);
            }
            else
            {
                for (int a = 0; a < Projectile.oldPos.Length; a++)
                {
                    Vector4 vector4 = new Vector4(71, 233, 60, 0) / 2 * (1 - (float)a / Projectile.oldPos.Length) + new Vector4(50, 207, 255, 0) / 2 * ((float)a / Projectile.oldPos.Length);
                    Color color = new Color((byte)vector4.X, (byte)vector4.Y, (byte)vector4.Z, (byte)vector4.W);
                    Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, Projectile.oldPos[a] + Projectile.Size - Main.screenPosition, null, color, 0, DDTextures.MiniVoidStar.Size() / 2, (1 - (float)a / Projectile.oldPos.Length) * 0.8F + 0.1f, 0, 0);
                }
            }
            // if (Projectile.ai[0] == 1)
            //DDHelper.绘制偏移头部(texture, Projectile, new Color(255, 255, 255, 0), MathHelper.Pi, DDGlobalProjectile.Glow[Projectile.type].Value, 20, 4);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, Projectile.Size/2, 0, 0f);
            return false;
        }
        internal static Trailing TrailDrawer;
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