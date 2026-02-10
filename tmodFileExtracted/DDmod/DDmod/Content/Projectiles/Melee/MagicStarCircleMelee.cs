using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee
{
    public class MagicStarCircleMelee : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.02F;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            NPC npc = NPCdirection.FindClosest(Projectile.Center, 1000, false);
            Projectile.DProj().Times[0] += 0.11f;
            Projectile.ai[1]++;
            if (Projectile.ai[1] < 30)
            {
                Projectile.scale += 0.02F;
                Projectile.rotation += 0.1f;
                if (Projectile.rotation > MathHelper.Pi)
                {
                    Projectile.rotation -= MathHelper.TwoPi;
                }
                if (npc != null)
                {
                    Projectile.rotation = (npc.Center - Projectile.Center).ToRotation();
                }
                else
                {
                    Projectile.rotation = Projectile.DProj().vector[0].ToRotation();
                }
            }
            else if (Projectile.ai[1] < 120)
            {
                if (Projectile.ai[1] < 50)
                    Projectile.ai[0] += 0.1f;
                if (npc != null)
                {
                    if (Projectile.rotation <= (npc.Center - Projectile.Center).ToRotation())
                    {
                        Projectile.rotation += 0.11f;
                        Projectile.DProj().Times[0] -= 0.11f;
                    }
                    if (Projectile.rotation >= (npc.Center - Projectile.Center).ToRotation())
                    {
                        Projectile.rotation -= 0.11f;
                        Projectile.DProj().Times[0] += 0.11f;
                    }
                    float A = Projectile.rotation - (npc.Center - Projectile.Center).ToRotation();

                    if (A * A < 0.04f && Projectile.ai[1] < 119 && Projectile.ai[1] > 50)
                    {
                        Projectile.ai[1] = 119;
                    }
                }
                else
                {
                    if (Projectile.rotation <= Projectile.DProj().vector[0].ToRotation())
                    {
                        Projectile.rotation += 0.11f;
                        Projectile.DProj().Times[0] -= 0.11f;
                    }
                    if (Projectile.rotation >= Projectile.DProj().vector[0].ToRotation())
                    {
                        Projectile.rotation -= 0.11f;
                        Projectile.DProj().Times[0] += 0.11f;
                    }
                    float A = Projectile.rotation - Projectile.DProj().vector[0].ToRotation();

                    if (A * A < 0.04f && Projectile.ai[1] < 119 && Projectile.ai[1] > 50)
                    {
                        Projectile.ai[1] = 119;
                    }
                }
            }
            else if (Projectile.ai[1] == 120)
            {
                if (Main.myPlayer == Projectile.owner)
                    NewProjectileDirect(player.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2().PerfectNormalize() * 18, ModContent.ProjectileType<MagicStarMelee>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            else
            {
                Projectile.scale -= 0.02f;
                if (Projectile.scale < 0.02F)
                {
                    Projectile.Kill();
                }
            }
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Texture2D texture = DDTextures.VoidStar.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;

            Color color = new Color(0, 100, 155, 0);
            //Main.spriteBatch.Draw(texture, vector, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale - Projectile.ai[0] / 16, Projectile.scale * 1.1f)*1.25f, 0, 0f);

            Color color2 = Projectile.GetAlpha(new Color(0, 100, 255, 100));

            DDHelper.Compression(texture2, color2, Projectile.rotation, 1, new Vector2(1 + Projectile.ai[0], 1), 1, Projectile.DProj().Times[0], BlendState.AlphaBlend);

            Main.EntitySpriteDraw(texture2, vector2, null, color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture2, vector2, null, color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture2, vector2, null, color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        private void A(Texture2D texture, float opacity, float circularRotation, BlendState blendMode)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, blendMode, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Matrix viewMatrix;
            Matrix projectionMatrix;
            playerHelper.CalculatePerspectiveMatricies(out viewMatrix, out projectionMatrix, 0);
            GameShaders.Misc["压缩"].UseColor(Projectile.GetAlpha(new Color(0, 100, 255, 255)));
            GameShaders.Misc["压缩"].UseSaturation(Projectile.rotation);
            GameShaders.Misc["压缩"].UseOpacity(opacity);
            GameShaders.Misc["压缩"].Shader.Parameters["usc"].SetValue(new Vector2(1 + Projectile.ai[0], 1));
            GameShaders.Misc["压缩"].Shader.Parameters["uDirection"].SetValue((float)Projectile.direction);
            GameShaders.Misc["压缩"].Shader.Parameters["uCircularRotation"].SetValue(Projectile.DProj().Times[0]);
            GameShaders.Misc["压缩"].Shader.Parameters["uImageSize0"].SetValue(Utils.Size(texture));
            GameShaders.Misc["压缩"].Shader.Parameters["overallImageSize"].SetValue(Utils.Size(ModContent.Request<Texture2D>(Texture, (AssetRequestMode)2).Value));
            GameShaders.Misc["压缩"].Shader.Parameters["uWorldViewProjection"].SetValue(viewMatrix * projectionMatrix);
            GameShaders.Misc["压缩"].Apply(default);
        }
    }
}