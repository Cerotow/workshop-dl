using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;

namespace DDmod.Content.Projectiles.Pet.MasterPet
{
    public class 微型绿岩之视 : ModProjectile
    {
        public ref float AlphaForVisuals => ref Projectile.ai[0];

        public override void SetStaticDefaults()
        {

            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet);

            Projectile.aiStyle = -1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        float R = 0;
        float Sc = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = new Rectangle(0, texture.Height / 3*Projectile.frame, texture.Width/3, texture.Height / 3);
            SpriteEffects sprite = 0;
            if(Projectile.Player().Center.X-Projectile.Center.X>0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Main.EntitySpriteDraw(texture, Projectile.Center-Main.screenPosition, rectangle, lightColor, Projectile.rotation, rectangle.Size()/2, 1f, sprite, 0);
            rectangle.X += texture.Width / 3;
            Main.EntitySpriteDraw(texture, Projectile.Center-Main.screenPosition, rectangle,Color.White, Projectile.rotation, rectangle.Size()/2, 1f, sprite, 0);
            rectangle.X += texture.Width / 3;
            Main.EntitySpriteDraw(texture, Projectile.Center-Main.screenPosition, rectangle, Color.White, Projectile.rotation, rectangle.Size()/2, 1f, sprite, 0);
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            CheckActive(player);

            bool movesFast = Movement(player);

            Animate(movesFast);
        }

        private void CheckActive(Player player)
        {
            if (!player.dead && player.HasBuff(ModContent.BuffType<微型绿岩之视Buff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        private bool Movement(Player player)
        {
            float velDistanceChange = 2f;

            int dir = -player.direction;
            Projectile.direction = Projectile.spriteDirection = dir;

            Vector2 desiredCenterRelative = new Vector2(dir * 60, -60f);

            desiredCenterRelative.Y += (float)Math.Sin(Main.GameUpdateCount / 120f * MathHelper.TwoPi) * 5;

            Vector2 desiredCenter = player.ArmCenter() + desiredCenterRelative;
            Vector2 betweenDirection = desiredCenter - Projectile.Center;
            float betweenSQ = betweenDirection.LengthSquared();

            if (betweenSQ > 1000f * 1000f || betweenSQ < velDistanceChange * velDistanceChange)
            {
                Projectile.Center = desiredCenter;
                Projectile.velocity = Vector2.Zero;
            }

            if (betweenDirection != Vector2.Zero)
            {
                Projectile.velocity = betweenDirection * 0.1f * 2;
            }

            bool movesFast = Projectile.velocity.LengthSquared() > 6f * 6f;
            Projectile.rotation = Projectile.velocity.X * 0.03F;

            return movesFast;
        }

        private void Animate(bool movesFast)
        {

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}
