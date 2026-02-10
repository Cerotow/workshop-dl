using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.流星破坏者;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.Boss.流星破坏者;
using DDmod.Content.Particles;
using DDmod.Worlds;
using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Magic
{
    public class 魔法破坏者激光束 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orange Laser");
            //DisplayName.AddTranslation(7, "橙激光");
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.scale = 0.5f;
            Projectile.timeLeft = 600;
        }
        int A;
        public override void AI()
        {
            Projectile.DProj().color = new Color(252, 128, 48, 00)*0.3F;
            Projectile.timeLeft = 5;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Vector2 vector = Projectile.velocity.PerfectNormalize() * 10;
            vectors = new Vector2[80];
            vectors[0] = Projectile.Center;
            for (int a = 1; a < vectors.Length; a++)
            {
                vectors[a] = Projectile.Center + vector * a;
            }
            if (Projectile.ai[2]++ > 80)
            {
                Projectile.localAI[0] -= 0.02F;
                if (Projectile.localAI[0] <= 0)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                SoundStyle sound = SoundID.Item12;
                sound.Pitch = 0.5F;
                sound.MaxInstances = 20;
                sound.Volume = 0.1F;
                PlaySound(sound, Projectile.position);
                Projectile.localAI[0] = 1F;

            }
            DDHelper.BackAndForth(1, 2, 0.1F, ref Projectile.localAI[2], ref Projectile.DProj().Bool[0]);
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
        public Vector2[] vectors;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (vectors == null)
            {
                return false;
            }
            if (Projectile.localAI[0] < 1)
            {
                return false;
            }
            for (int a = 0; a < vectors.Length; a++)
            {
                projHitbox.X = (int)(vectors[a].X - projHitbox.Width / 2);
                projHitbox.Y = (int)(vectors[a].Y - projHitbox.Height / 2);
                if (projHitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(vectors==null)
            {
                return false;
            }
            Texture2D texture2 = DDTextures.Starlight3.Value;
            Texture2D texture3 = DDTextures.LightEffect.Value;
            Texture2D texture = TextureAssets.Item[ModContent.ItemType<破坏者激光枪>()].Value;
            if (Projectile.velocity.X > 0)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 26, null, new Color(252, 128, 48, 100) * Projectile.localAI[0], Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale * 2, 0, 0) ;
            }
            else
            {

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - Projectile.velocity.PerfectNormalize() * 26, null, new Color(252, 128, 48, 100) * Projectile.localAI[0], Projectile.velocity.ToRotation() + MathHelper.Pi, texture.Size() / 2, Projectile.scale * 2, SpriteEffects.FlipHorizontally, 0);

            }
            texture = DDTextures.VoidStar.Value;
            for (int a = 0; a < vectors.Length; a++)
            {
                float SC = (1 - 10F / 30) + ((float)a % 11) / 30;
                if (a % 22 < 11)
                {
                    SC = 1 - ((float)a % 11) / 30;
                }
                Main.spriteBatch.Draw(texture, vectors[a] - Main.screenPosition, null, Projectile.DProj().color * Projectile.localAI[0], 0, texture.Size() / 2, Projectile.scale * 0.75f * SC, 0, 0);
                Main.spriteBatch.Draw(texture3, vectors[a] - Main.screenPosition, null, Projectile.DProj().color * Projectile.localAI[0], Projectile.ai[2] / 5, texture3.Size() / 2, Projectile.scale * 2f * SC, 0, 0);
                if (a == 0)
                {
                    Main.spriteBatch.Draw(texture2, vectors[a] - Main.screenPosition, null, Projectile.DProj().color * Projectile.localAI[0], 0, texture2.Size() / 2, Projectile.scale * new Vector2(1, 2) * Projectile.localAI[2], 0, 0);
                    Main.spriteBatch.Draw(texture2, vectors[a] - Main.screenPosition, null, Projectile.DProj().color * Projectile.localAI[0], MathHelper.PiOver2, texture2.Size() / 2, Projectile.scale * new Vector2(1, 3) * Projectile.localAI[2], 0, 0);
                }
                if (a == vectors.Length - 1)
                {

                    Main.spriteBatch.Draw(texture2, vectors[a] - Main.screenPosition, null, Projectile.DProj().color * Projectile.localAI[0], 0, texture2.Size() / 2, Projectile.scale * new Vector2(1, 2) * Projectile.localAI[2], 0, 0);
                    Main.spriteBatch.Draw(texture2, vectors[a] - Main.screenPosition, null, Projectile.DProj().color * Projectile.localAI[0], MathHelper.PiOver2, texture2.Size() / 2, Projectile.scale * new Vector2(1, 3) * Projectile.localAI[2], 0, 0);
                }
            }
            return false;
        }
    }
}