using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 星星 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/星星光效");
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.01F;
            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.01F;
            }
            else
            {
                Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.01F;
            }
            if (Projectile.ai[0] == 0)
            {
                if (Projectile.DProj().vector[0] == Vector2.Zero)
                    Projectile.DProj().vector[0] = Projectile.velocity;
                
                DDHelper.BackAndForth(-15F, 15F, 3F, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[1]);
                Projectile.velocity = Projectile.DProj().vector[0] + new Vector2(0, Projectile.DProj().Times[0] * Projectile.ai[1]).RotatedBy(Projectile.DProj().vector[0].ToRotation()+ Projectile.rotation/20 * Projectile.ai[1]);
                
            }
            Projectile.ProjScaleChange();
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange2(20, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 3 * Projectile.scale, 10 * Projectile.scale, true, Projectile.scale*2, Projectile.scale*3,0,new Color(0,100,255,0));
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                NewDustChange2(2, Projectile.oldPos[i] + Projectile.Size / 2 - new Vector2(4), Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 1, true, Projectile.scale / 2,Projectile.scale,0, new Color(0, 100, 255, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length));

            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 v = Projectile.Center - Main.screenPosition;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Asset<Texture2D> Glow = DDTextures.VoidStar;
            Texture2D textureGlow = Glow.Value;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2;
                Color color = new Color(0, 100, 255,0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
                Main.spriteBatch.Draw(textureGlow, vector2, null, new Color(0, 100, 255, 0) * 0.4f, 1, textureGlow.Size() / 2, Projectile.scale * 0.6f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, v, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);

            Main.spriteBatch.Draw(textureGlow, v, null, new Color(0, 100, 255, 0), 1, textureGlow.Size() / 2, Projectile.scale* 0.6f, SpriteEffects.None, 0);
            return false;
        }
    }
}