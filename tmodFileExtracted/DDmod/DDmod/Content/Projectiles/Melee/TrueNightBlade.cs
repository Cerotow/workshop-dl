using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class TrueNightBlade : ModProjectile
    {
        public override string Texture => "DDmod/Content/Projectiles/Melee/SwordWave4";
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.DProj().Times[0]++;
            if(Projectile.DProj().Times[0]<80)
            {
                Projectile.Center = player.Center + Projectile.velocity.PerfectNormalize() * 100;
                Projectile.localAI[2] = 400;
                Projectile.DProj().Times[1] = 0;
                Projectile.DProj().Times[2] = 1;
            }
            else if(Projectile.timeLeft < 20)
            {
                Projectile.extraUpdates = 1;
                Projectile.timeLeft = 2;
                if (Projectile.localAI[2] > 0)
                {
                    Projectile.localAI[2] -= 10;
                }
                else
                {
                    Projectile.localAI[2] = 0;
                }
                Projectile.DProj().Times[1] -= 0.02F;
                if(Projectile.DProj().Times[1]<=0)
                {
                    Projectile.Kill();
                }
                Projectile.DProj().Times[2] += 0.02f;
            }
            else
            {
                
                Projectile.DProj().Times[1] = 1F;
                Projectile.localAI[2] = 400;

            }
            DDHelper.MaxandMinF(ref Projectile.DProj().Times[1],1,0);
            if (Projectile.DProj().Times[1]>=0.5f)
            {
                for (int a = -2; a <= 2; a++)
                {
                    Vector2 vector = new(Projectile.position.X, Projectile.position.Y);
                    vector += Projectile.velocity.RotatedBy(MathHelper.PiOver2).PerfectNormalize() * (Projectile.width * a);
                    vector -= Projectile.velocity.PerfectNormalize() * Math.Abs(a * 20 * Projectile.scale);
                    int Type = 75;
                    Color color = new Color(96, 248, 2, 12);
                    if (Projectile.ai[0] == 0)
                    {
                        Type = 27;
                        color = new Color(81, 6, 233, 12);
                    }
                    if (Projectile.ai[0] == 1&&Main.rand.NextBool(3))
                    {
                        Type = 27;
                        color = new Color(81, 6, 233, 12);
                    }
                    if (Projectile.ai[0] == 2)
                    {
                        Type = ModContent.DustType<星光粒子>();
                        color = new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164),0);
                    }
                    if (Main.rand.NextBool(50))
                    {
                        if (Projectile.ai[0] != 2 || Main.rand.NextBool(5))
                        {
                            Dust dust = Main.dust[NewDust(vector, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, color)];
                            dust.noGravity = true;
                            dust.velocity = Projectile.velocity;
                            dust.scale = Projectile.scale;
                            if (Projectile.ai[0] == 2)
                            {
                                dust.scale *= 2;
                            }
                        }
                    }
                    if (Main.rand.NextBool(10))
                    {
                        if (Projectile.ai[0] == 2)
                        {
                            color = new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 0)*0.4f;
                        }
                        Type = ModContent.DustType<速度粒子>();
                        Dust dust = Main.dust[NewDust(vector, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, color)];
                        dust.noGravity = true;
                        dust.velocity = Projectile.velocity*2;
                        dust.rotation = Projectile.velocity.ToRotation();
                        dust.scale = Main.rand.NextFloat(2,4);
                        dust.customData = dust.scale*0.8F;
                    }
                }
            }
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] = Projectile.scale;
            }
            if (Projectile.localAI[1] == 0)
            {
                Projectile.localAI[1] = Projectile.damage;
            }
            Projectile.scale = Projectile.localAI[0] * Projectile.DProj().Times[2];
            Projectile.damage = (int)(Projectile.localAI[1] * (Projectile.DProj().Times[1]));
            Projectile.ProjScaleChange();
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            //target.AddBuff(24, 300);
            Projectile.damage = (int)(Projectile.damage * 0.8F);
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            if (Projectile.ai[0] == 0)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge, settings, Projectile.owner);
            }
            if (Projectile.ai[0] == 1)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueNightsEdge, settings, Projectile.owner);
            }
            if (Projectile.ai[0] == 2)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, settings, Projectile.owner);
            }
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            if (Projectile.DProj().Times[0] < 80)
            {
                return false;
            }
            return null;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int a = -2; a <= 2; a++)
            {
                Vector2 vector = new(projHitbox.X, projHitbox.Y);
                vector += Projectile.velocity.RotatedBy(MathHelper.PiOver2).PerfectNormalize() * (projHitbox.Width * a) - Projectile.velocity.PerfectNormalize() * (20 * Math.Abs(a)); ;
                if (new Rectangle((int)vector.X, (int)vector.Y, projHitbox.Width, projHitbox.Height).Intersects(targetHitbox))
                {
                    return new bool?(true);
                }
            }
            return new bool?(false);
        }
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            float A = Projectile.DProj().Times[1];
            Color color = new Color(96, 248, 2, 12);
            if (Projectile.ai[0] != 2)
            {
                if (Projectile.ai[0] == 0)
                {
                    color = new Color(181, 36, 255, 120);
                    return color * A;
                }
                if (Projectile.ai[0] == 1)
                {
                    return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
                    {
                new Color(26, 248, 2, 12)*0.8F,
                new Color(26, 248, 2, 12)*0.8F,
                new Color(26, 248, 2, 12)*0.8F,
                new Color(26, 248, 2, 12)*0.8F,
                    }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(181, 36, 255, 120), (float)Math.Pow((double)completionRatio, 1.0)) * A;
                }
            }
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(83, 255,40 ,0),
                new Color(0, 144, 217 ,0),
                new Color(19,201,122 ,0),
                new Color(54,249,152 ,0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(10, 204, 164, 0), (float)Math.Pow((double)completionRatio, 1.0)) * A;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                50,
                70,
                90,
                110,
                90,
                70,
                50,
            }), 20, (float)Math.Pow((double)completionRatio, 1.0))*Projectile.scale;
        }

        internal static Trailing TrailDrawer;
        internal Trailing TrailDrawer2;
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer2 == null)
            {
                TrailDrawer2 = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            Texture2D texture = TextureAssets.Projectile[Type].Value;


            Color color = new Color(96, 248, 2, 12);

            if (Projectile.ai[0] == 0)
            {
                color = new Color(81, 6, 233, 12);
            }
            float A = Projectile.DProj().Times[1];
            color *= A;
            if (Projectile.ai[0] == 2)
            {
                color = new Color(81, 233, 70, 12) * 0.3F * A;
            }
            //Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, new Color(50,50,50,255) * A, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);

            if (Projectile.ai[0] == 0)
            {
                Color color1 = color;
                color1.A = 255;
                color1 *= A;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color1, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color1, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color1, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            }
            if (Projectile.ai[0] == 1)
            {
                Color color1 = new Color(55, 55, 55, 255) * A;
                color1.A = (byte)(255 * A);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color1, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
                color1 = new Color(46, 248, 2, 0) * A;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color1, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color1, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            }
            if (Projectile.ai[0] == 2)
            {
                Color color1 = new Color(10, 10, 10, 255) * A;
                color1.A = (byte)(255 * A);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color1, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
                color1 = new Color(150, 248, 150, 0) * A;
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, color1, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            }
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.ai[0] == 0)
                {
                    color = new Color(81, 6, 233, (int)(55F * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length))) * A;
                }
                if (Projectile.ai[0] == 1)
                {
                    color = new Color(96, 248, 2, 12);
                    color.R -= (byte)((96 - 81) * 0.75F);
                    color.G -= (byte)((248 - 6) * 0.75F);
                    color.B += (byte)((233 - 2) * 0.75F);

                    color.R -= (byte)((96 - 81) * (0.25F - (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)/4);
                    color.G -= (byte)((248 - 6) * (0.25F - (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)/4);
                    color.B += (byte)((233 - 2) * (0.25F - (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)/4);
                    color *= 0.4F * A;
                }
                if (Projectile.ai[0] == 2)
                {
                    if (i % 15 < 6)
                    {
                        color = new Color(81, 233, 70, 12) * 0.3F;

                    }
                    else
                    {
                        color = new Color(61, 190, 255, 12) * 0.3F;
                    }
                    color *= A;
                }
                Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2) * 0.6F;
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 12, null, oldcolor, Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1.4F, 1), 0, 0);
            }
            if (Projectile.ai[0] == 1)
            {
                color = new Color(96, 248, 2, 12) * A;
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail2);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(1.2f);
            TrailDrawer2.Draw(new Vector2[] { Projectile.Center, Projectile.Center - Projectile.velocity.PerfectNormalize() * Projectile.localAI[2] * Projectile.scale*0.25F }, -Main.screenPosition - Projectile.velocity.PerfectNormalize() * 16 + Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * (80* Projectile.scale), 204, null,0.5F, A);
            TrailDrawer2.Draw(new Vector2[] { Projectile.Center, Projectile.Center - Projectile.velocity.PerfectNormalize() * Projectile.localAI[2] * Projectile.scale*0.5F }, -Main.screenPosition - Projectile.velocity.PerfectNormalize() * 8 + Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * (40* Projectile.scale), 204, null, 0.75F, A);
            TrailDrawer2.Draw(new Vector2[] { Projectile.Center, Projectile.Center - Projectile.velocity.PerfectNormalize() * Projectile.localAI[2] * Projectile.scale }, -Main.screenPosition - Projectile.velocity.PerfectNormalize(),  204, null,1, A);
            TrailDrawer2.Draw(new Vector2[] { Projectile.Center, Projectile.Center - Projectile.velocity.PerfectNormalize() * Projectile.localAI[2] * Projectile.scale*0.5F }, -Main.screenPosition - Projectile.velocity.PerfectNormalize() * 8 - Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2)*(40* Projectile.scale), 204, null, 0.75F, A);
            TrailDrawer2.Draw(new Vector2[] { Projectile.Center, Projectile.Center - Projectile.velocity.PerfectNormalize() * Projectile.localAI[2] * Projectile.scale*0.25F }, -Main.screenPosition - Projectile.velocity.PerfectNormalize() * 16 - Projectile.velocity.PerfectNormalize().RotatedBy(MathHelper.PiOver2) * (80* Projectile.scale), 204, null, 0.5F, A);

       
            /* for (int a = -2; a <= 2; a++)
            {
                Vector2 vector = Projectile.position;
                vector += Projectile.velocity.RotatedBy(MathHelper.PiOver2).PerfectNormalize() * (Projectile.width * a)- Projectile.velocity.PerfectNormalize()*(20*Math.Abs(a));
                Main.spriteBatch.Draw(DDTextures.WhitePng.Value, vector - Main.screenPosition, null, Color.White * 0.5F, 0, Vector2.Zero, Projectile.Size / 2, 0, 0);

            }*/
            if (Projectile.ai[0] != 2) return false;
            color.A = 0;
            color *= (5 - A);
            for (int a = -6; a <= 6; a += 3)
            {
                color = new Color(123, 255, 120, 0) * A * 0.6F;
                Vector2 vector = new(Projectile.Center.X, Projectile.Center.Y);
                vector += Projectile.velocity.RotatedBy(MathHelper.PiOver2).PerfectNormalize() * (Projectile.width * a / 3);
                texture = DDTextures.Starlight3.Value;
                Vector2 Sc = new Vector2(0.5f, 1f)*2 * (1 - Math.Abs((float)a * 0.1f))*Projectile.scale;
                if (Math.Abs(a) == 6)
                {
                    Sc = new Vector2(0.5f, 1f)*2 * (1.2f - Math.Abs((float)a * 0.1f)) * Projectile.scale;
                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition + Projectile.velocity.PerfectNormalize() * ((10 + 14 * -Math.Abs(a / 3)) * Projectile.scale), null, color, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Sc, 0, 0);
                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition + Projectile.velocity.PerfectNormalize() * ((10 + 14 * -Math.Abs(a / 3)) * Projectile.scale), null, color, Projectile.rotation + MathHelper.PiOver2 + MathHelper.TwoPi / 3, texture.Size() / 2, Sc, 0, 0);
                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition + Projectile.velocity.PerfectNormalize() * ((10 + 14 * -Math.Abs(a / 3)) * Projectile.scale), null, color, Projectile.rotation + MathHelper.PiOver2 + MathHelper.TwoPi / 3 * 2, texture.Size() / 2, Sc, 0, 0);
                }
                else
                {
                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition + Projectile.velocity.PerfectNormalize() * ((32 + 14 * -Math.Abs(a / 3)) * Projectile.scale), null, color, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, Sc, 0, 0);
                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition + Projectile.velocity.PerfectNormalize() * ((32 + 14 * -Math.Abs(a / 3)) * Projectile.scale), null, color, Projectile.rotation + MathHelper.PiOver2 + MathHelper.TwoPi / 3, texture.Size() / 2, Sc, 0, 0);
                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition + Projectile.velocity.PerfectNormalize() * ((32 + 14 * -Math.Abs(a / 3)) * Projectile.scale), null, color, Projectile.rotation + MathHelper.PiOver2 + MathHelper.TwoPi / 3 * 2, texture.Size() / 2, Sc, 0, 0);
                }
            }
            return false;
        }
    }
}