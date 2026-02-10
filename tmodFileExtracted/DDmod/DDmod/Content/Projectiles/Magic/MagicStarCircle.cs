using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Magic
{
    public class MagicStarCircle : ModProjectile
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
            Projectile.minionSlots = 1;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().Times[0] += 0.11f;
            Projectile.ai[1]++;
            if (Projectile.ai[1] < 30)
            {
                Projectile.scale += 0.04F;
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
                Projectile.minionSlots = 0;
                player.AddBuff(ModContent.BuffType<MagicStarBuff>(), 2);
                Projectile projectile = NewProjectileDirect(player.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2().PerfectNormalize() * 8, ModContent.ProjectileType<MagicStarMini>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                projectile.originalDamage = projectile.damage / 2;
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
            Texture2D texture = DDTextures.VoidStar.Value;
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Color color = Projectile.GetAlpha(new Color(0, 100, 255, 0));
            Main.spriteBatch.Draw(texture, vector, null, color, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale - Projectile.ai[0] / 8, Projectile.scale), 0, 0f);

            DDHelper.Compression(texture2, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(1 + Projectile.ai[0], 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);

            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.EntitySpriteDraw(texture2, vector2, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height)), color2, 0f, Utils.Size(texture2) * 0.5f, Projectile.scale, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}