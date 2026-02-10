using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;

namespace DDmod.Content.Projectiles.Pet.MasterPet
{
    public class 公海幽浮 : ModProjectile
    {
        public ref float AlphaForVisuals => ref Projectile.ai[0];

        public override void SetStaticDefaults()
        {

            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
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
            Rectangle rectangle = new Rectangle(0, texture.Height / 6 * Projectile.frame, texture.Width, texture.Height/6);
            SpriteEffects sprite = 0;
            if(Projectile.Player().Center.X-Projectile.Center.X>0)
            {
                sprite = SpriteEffects.FlipHorizontally;
            }
            Main.EntitySpriteDraw(texture, Projectile.Center-Main.screenPosition, rectangle, lightColor, Projectile.rotation + MathHelper.PiOver2, rectangle.Size()/2, 1f, sprite, 0);
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
            if (!player.dead && player.HasBuff(ModContent.BuffType<海幽浮Buff>()))
            {
                Projectile.timeLeft = 2;
            }
        }

        private bool Movement(Player player)
        {
            float velDistanceChange = 2f;

            int dir = -player.direction;
            Projectile.direction = Projectile.spriteDirection = dir;

            DDHelper.BackAndForth(-80, -0, 2, ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0]);
            Vector2 desiredCenterRelative = new Vector2(dir * 100, Projectile.DProj().Times[0]);


            Vector2 desiredCenter = player.MountedCenter + desiredCenterRelative;
            Vector2 betweenDirection = desiredCenter - Projectile.Center;
            float betweenSQ = betweenDirection.LengthSquared();


            if (betweenDirection != Vector2.Zero && Projectile.frame % 6 == 3 && Projectile.frameCounter == 0)
            {
                float r = 4;
                if(betweenDirection.Length() > 300)
                {
                    r = 22;
                }
                Projectile.velocity = Projectile.rotation.ToRotationVector2() * r;
            }
            if (betweenDirection.Length() > 100)
            {
                if (betweenDirection.Length() > 300)
                    Projectile.RotationSpeed(betweenDirection.ToRotation(), 0.2F);
                else
                    Projectile.RotationSpeed(betweenDirection.ToRotation(), 0.05F);
            }
            Projectile.velocity *= 0.92F;
            bool movesFast = Projectile.velocity.LengthSquared() > 6f * 6f;

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
