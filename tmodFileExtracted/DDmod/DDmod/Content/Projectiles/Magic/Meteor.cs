using DDmod.Content.Dusts;
using DDmod.Content.Particles;
using DDmod.Content.Projectiles.Magic.Staff;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Magic
{
    public class Meteor : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.scale = 2F;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
            Main.projFrames[Type] = 4;
        }
        int A;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 4 == 0)
            {
                Projectile.frame++;
                Projectile.frame %= 4;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        public override void OnKill(int timeLeft)
        {
            for (float A = 0; A < 250; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center, 1, 1, 6)];
                dust.noGravity = true;
                dust.scale = 2.2F;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2, 20);
            }
            SoundStyle sound = SoundID.Item14;
            sound.Pitch = -1;
            PlaySound(sound, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 16);
            Color color = new Color(0, 100, 255, 0);
            Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 4 * Projectile.frame, texture.Width, texture.Height / 4)), Color.White, Projectile.rotation, vector, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}