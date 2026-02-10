using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.绿岩之视;
using DDmod.Content.Particles;
using DDmod.Worlds;
using System.Transactions;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 灵剑 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            Projectile.MeleeProj().oldVels = new Vector2[Projectile.oldPos.Length];
            Projectile.MeleeProj().oldVels2 = 42;

            Projectile.width = 14;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 20;
            Projectile.scale = 1F;
            Projectile.timeLeft = 3000;
            Projectile.hide = false;
            Projectile.DProj().Times[0] = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation += 0.05F;
            if(Projectile.rotation>=MathHelper.TwoPi)
            {
                Projectile.rotation -= MathHelper.TwoPi;
                Projectile.ClearInvincibleFrame();
            }
            Projectile.velocity = (Projectile.rotation-MathHelper.PiOver4).ToRotationVector2().PerfectNormalize();
            Projectile.position = Projectile.oldPosition;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
        }
        int MCD;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            MCD++;
            if (MCD < 4)
            {
                return false;
            }
            float num2 = 1.25F;
            float LaserLength = MathHelper.Lerp(0, Projectile.height, num2);
            Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), 0, default);
            float num = 0f;
            return new bool?(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center + Pvelocity * LaserLength, projHitbox.Width, ref num));
        }
        Color color = new Color(140, 96, 4, 0);
        public Color TrailColor(float completionRatio)
        {
            float trailOpacity = Utils.GetLerpValue(0f, 0.1f, completionRatio, true) * Utils.GetLerpValue(0.7f, 0.58f, completionRatio, true);
            Color startingColor = Color.Lerp(color, color, 0.07f);
            return playerHelper.MulticolorLerp(completionRatio, new Color[]
            {
                startingColor,
            }) * trailOpacity;
        }
        public float TrailWidth(float completionRatio)
        {
            return 18;
        }
        internal static Trailing TrailDrawer;
        public override bool PreDraw(ref Color lightColor)
        {
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(TrailWidth), new Trailing.VertexColorFunction(TrailColor), null, GameShaders.Misc["刀光"]);
            }
            Vector2 vector = Projectile.Center;
            GameShaders.Misc["刀光"].SetShaderTexture(DDTextures.WhitePng);
            GameShaders.Misc["刀光"].UseColor(Projectile.GetAlpha(color * 0.6f));
            GameShaders.Misc["刀光"].UseSecondaryColor(Projectile.GetAlpha(color) * 0.6f);
            GameShaders.Misc["刀光"].Shader.Parameters["FlameColor"].SetValue(Projectile.GetAlpha(color).ToVector3());
            GameShaders.Misc["刀光"].Shader.Parameters["flipped"].SetValue(Projectile.DProj().Times[0] > 0);
            GameShaders.Misc["刀光"].Shader.Parameters["uDarkshade"].SetValue(0);
            GameShaders.Misc["刀光"].Apply(default(DrawData?));
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null);

            GameShaders.Misc["刀光"].SetShaderTexture(DDTextures.Wave);
            GameShaders.Misc["刀光"].UseColor(Projectile.GetAlpha(color));
            GameShaders.Misc["刀光"].UseSecondaryColor(Projectile.GetAlpha(color));
            GameShaders.Misc["刀光"].Shader.Parameters["FlameColor"].SetValue(Projectile.GetAlpha(color).ToVector3());
            GameShaders.Misc["刀光"].Shader.Parameters["flipped"].SetValue(Projectile.DProj().Times[0] > 0);
            GameShaders.Misc["刀光"].Shader.Parameters["uDarkshade"].SetValue(0);
            GameShaders.Misc["刀光"].Apply(default(DrawData?));
            TrailDrawer.Draw(Projectile.MeleeProj().oldVels, vector - Main.screenPosition, 88, null);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 Center = Projectile.Center - Main.screenPosition;

            Rectangle? rectangle = new Rectangle?(new Rectangle(0, 0, texture.Width/4, texture.Height));
            if (Projectile.spriteDirection == 0)
            {
                Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, Projectile.rotation, new Vector2(0,rectangle.Value.Height), Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, Projectile.rotation + MathHelper.PiOver2, new Vector2(rectangle.Value.Width, rectangle.Value.Height), Projectile.scale, (SpriteEffects)Projectile.spriteDirection, 0f);
            }

            Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null,Color.White*0.5F,0,Vector2.Zero, Projectile.Size/2, (SpriteEffects)Projectile.spriteDirection, 0f);

            return false;
        }
    }
}