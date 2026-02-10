using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Summon
{
    public class MagicStarCircleMini : ModProjectile
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
            Projectile.DProj().Times[0] += 0.11f;
            Projectile.ai[1]++;
            if (Projectile.ai[1] < 30)
            {
                Projectile.scale += 0.03F;
                Projectile.rotation += 0.1f;
                if (Projectile.rotation > MathHelper.Pi)
                {
                    Projectile.rotation -= MathHelper.TwoPi;
                }
                Projectile.rotation = (player.Center - Projectile.Center).ToRotation();
            }
            else if (Projectile.ai[1] < 200)
            {
                if (Projectile.ai[1] < 50)
                    Projectile.ai[0] += 0.1f;
                if (Projectile.rotation <= (player.Center - Projectile.Center).ToRotation())
                {
                    Projectile.rotation += 0.11f;
                    Projectile.DProj().Times[0] -= 0.11f;
                }
                if (Projectile.rotation >= (player.Center - Projectile.Center).ToRotation())
                {
                    Projectile.rotation -= 0.11f;
                    Projectile.DProj().Times[0] += 0.11f;
                }
                float A = Projectile.rotation - (player.Center - Projectile.Center).ToRotation();

                if (A * A < 0.04f && Projectile.ai[1] < 199 && Projectile.ai[1] > 50)
                {
                    Projectile.ai[1] = 199;
                }
            }
            else if (Projectile.ai[1] == 200)
            {
                player.AddBuff(ModContent.BuffType<MagicStarBuff>(), 2);
                Projectile projectile = NewProjectileDirect(player.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2().PerfectNormalize() * 8, ModContent.ProjectileType<MagicStarMini>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                projectile.originalDamage = Projectile.originalDamage;
            }
            else
            {
                Projectile.scale -= 0.02f;
                if (Projectile.scale < 0.02F)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Vector2 vector2 = Projectile.Center - Main.screenPosition;
            Color color2 = Projectile.GetAlpha(new Color(0, 100, 255, 0)) * 0.66F;
            A(texture2, Projectile.Opacity * 0.7f, 0f, BlendState.Additive);

            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D texture = DDTextures.VoidStar.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Color color = Projectile.GetAlpha(new Color(0, 100, 255, 0));
            Main.spriteBatch.Draw(texture, vector, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale - Projectile.ai[0] / 16, Projectile.scale * 1.1f), 0, 0f);
            Main.spriteBatch.Draw(texture, vector, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale - Projectile.ai[0] / 16, Projectile.scale * 1.1f), 0, 0f);
            return false;
        }
        private void A(Texture2D texture, float opacity, float circularRotation, BlendState blendMode)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, blendMode, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Matrix viewMatrix;
            Matrix projectionMatrix;
            playerHelper.CalculatePerspectiveMatricies(out viewMatrix, out projectionMatrix, 0);
            GameShaders.Misc["压缩"].UseColor(Projectile.GetAlpha(new Color(0, 100, 255, 0)));
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