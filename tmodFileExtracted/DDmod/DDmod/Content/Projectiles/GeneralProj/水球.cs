using DDmod.Content.Dusts;
using DDmod.Content.NPCs.Boss.海幽浮王;
using DDmod.Content.Particles;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 水球 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;

        }
        int A;
        public override void AI()
        {
            Player player = Projectile.Player();
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water);
            DDHelper.BackAndForth(-0.1F, 0.1F, 0.03F, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0], false);
            if (Projectile.velocity.Y < 8)
            {
                Projectile.velocity.Y += 0.1F;
            }
            if(Projectile.alpha>100)
            {
                Projectile.alpha -= 20;
            }
            Projectile.velocity.X *= 0.98F;
            Projectile.ProjScaleChange();
            if(Projectile.wet)
            {
                Projectile.Kill();
            }
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
            if (Projectile.ai[1] <= 2)
            {
                NewDustChange(60, Projectile.position, Projectile.Size, 33, 1, Projectile.velocity.Length() / 2, false, Main.rand.NextFloat(1F, 2F) * Projectile.scale);
            }
            else
            {
                NewDustChange(30, Projectile.position, Projectile.Size, ModContent.DustType<光球粒子>(), 1, Projectile.velocity.Length() / 2, false, Main.rand.NextFloat(1F, 2F) * Projectile.scale, 100, new Color(30, 150, 255));
                NewDustChange(30, Projectile.position, Projectile.Size, ModContent.DustType<光球粒子>(), 1, Projectile.velocity.Length() / 2, false, Main.rand.NextFloat(1F, 2F) * Projectile.scale, 100, new Color(110, 250, 255));

            }
            SoundStyle sound = SoundID.SplashWeak;
            sound.Pitch = -1F;
            SoundEngine.PlaySound(sound  , Projectile.position);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2 = DDTextures.VoidStar.Value;
            Vector2 v = Projectile.Center - Main.screenPosition;
            
            if (Projectile.ai[1] > 2)
                lightColor = Color.White;
            Main.spriteBatch.Draw(texture, v, null, lightColor*(1F- Projectile.alpha/255F), Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1 - Projectile.DProj().Times[0], 1 + Projectile.DProj().Times[0]), 0, 0f);

            if (Projectile.ai[1] > 2)
            {
                Main.spriteBatch.Draw(texture2, v, null, new Color(30, 150, 255) * (1F - Projectile.alpha / 255F), Projectile.rotation, texture2.Size() / 2, Projectile.scale / 2 * new Vector2(1 - Projectile.DProj().Times[0], 1 + Projectile.DProj().Times[0]), 0, 0f);
                Main.spriteBatch.Draw(texture, v, null, new Color(30, 150, 255, 0) * (1F - Projectile.alpha / 255F), Projectile.rotation, texture.Size() / 2, Projectile.scale * new Vector2(1 - Projectile.DProj().Times[0], 1 + Projectile.DProj().Times[0]), 0, 0f);
                for (int a = 0; a < Projectile.oldPos.Length; a++)
                {
                    Vector2 vector = Projectile.oldPos[a] + Projectile.Size / 2;
                    Main.spriteBatch.Draw(texture2, vector - Main.screenPosition, null, new Color(30, 150, 255) * (1F - Projectile.alpha / 255F) * 0.4f * (1 - (float)a / Projectile.oldPos.Length), Projectile.rotation, new Vector2(texture2.Width, texture2.Height) / 2, Projectile.scale / 2 * new Vector2(1 - Projectile.DProj().Times[0], 1 + Projectile.DProj().Times[0]) * (1 - (float)a / Projectile.oldPos.Length / 2), 0, 0f);

                    Main.spriteBatch.Draw(texture, vector - Main.screenPosition, null, new Color(30, 150, 255, 0) * (1F - Projectile.alpha / 255F) * 0.4f * (1 - (float)a / Projectile.oldPos.Length), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale * new Vector2(1 - Projectile.DProj().Times[0], 1 + Projectile.DProj().Times[0]) * (1 - (float)a / Projectile.oldPos.Length / 2), 0, 0f);

                }
            }
            
            return false;
        }
    }
}