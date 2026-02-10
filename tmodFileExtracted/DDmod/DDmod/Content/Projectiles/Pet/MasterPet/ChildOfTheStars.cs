using DDmod.Content.Projectiles.Pet.MasterPet.MasterPetBuff;

namespace DDmod.Content.Projectiles.Pet.MasterPet
{
    public class ChildOfTheStars : ModProjectile
    {
        public ref float AlphaForVisuals => ref Projectile.ai[0];

        public override void SetStaticDefaults()
        {

            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet);

            Projectile.aiStyle = -1;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        float R = 0;
        float Sc = 0;
        float rotation;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Bounds, lightColor, Projectile.rotation, texture.Size() / 2 + new Vector2(0, 3), 1f, SpriteEffects.None, 0);
            R += 0.1f;
            if (R > MathHelper.TwoPi * 6)
            {
                R = 0;
            }
            if (R > MathHelper.TwoPi * 3)
            {
                if (Sc < 36)
                {
                    Sc += 0.1F;
                }
            }
            else
            {
                if (Sc < 24)
                {
                    Sc += 0.2f;
                }
                else
                {
                    Sc -= 0.2f;
                }
            }
            rotation += 0.2f;
            Texture2D eyeTexture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Pet/MasterPet/ServantOfTheSonofTheStarGuard");

            Vector2 offset = new Vector2(0, Projectile.gfxOffY);
            Vector2 orbitingCenter = Projectile.Center + offset;

            int eyeCount = 5;
            for (int i = 0; i < eyeCount; i++)
            {
                Vector2 rotatedPos = (Vector2.UnitY * Sc).RotatedBy(i / (float)eyeCount * MathHelper.TwoPi + R) + new Vector2(2, 2);
                Vector2 drawPos = orbitingCenter - Main.screenPosition + rotatedPos;
                if (Main.dayTime)
                {
                    Main.EntitySpriteDraw(eyeTexture, drawPos, eyeTexture.Bounds, lightColor, rotation, eyeTexture.Size() / 2, 1f, SpriteEffects.None, 0);
                }
                else
                {
                    Vector3 vector = new Vector3(0.25f, 0.5f, 1f);
                    Lighting.AddLight(Projectile.Center + rotatedPos, vector);
                    //Main.EntitySpriteDraw(eyeTexture, drawPos, eyeTexture.Bounds, Color.White, rotatedPos.ToRotation() + MathHelper.PiOver2, TextureAssets.Projectile[Projectile.type].Size() / 2, 1f, SpriteEffects.None, 0);
                    Main.EntitySpriteDraw(eyeTexture, drawPos, eyeTexture.Bounds, new Color(0, 100, 255, 0), rotation, eyeTexture.Size() / 2, 1f, SpriteEffects.None, 0);
                    Main.EntitySpriteDraw(eyeTexture, drawPos, eyeTexture.Bounds, new Color(0, 100, 255, 0), rotation, eyeTexture.Size() / 2, 1f, SpriteEffects.None, 0);
                }

            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            CheckActive(player);

            bool movesFast = Movement(player);

            Animate(movesFast);

            AlphaForVisuals = GetAlphaForVisuals(player);
        }

        private void CheckActive(Player player)
        {
            if (!player.dead && (player.HasBuff(ModContent.BuffType<ChildOfTheStarsBuff>()) || player.HasBuff(ModContent.BuffType<星心之子Buff>())))
            {
                Projectile.timeLeft = 2;
            }
        }

        private bool Movement(Player player)
        {
            float velDistanceChange = 2f;

            int dir = -player.direction;
            Projectile.direction = Projectile.spriteDirection = dir;

            Vector2 desiredCenterRelative = new Vector2(dir * -80, -80f);

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
            Projectile.rotation += 0.1f;

            return movesFast;
        }

        private void Animate(bool movesFast)
        {
            int animationSpeed = 7;

            if (movesFast)
            {
                animationSpeed = 4;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter > animationSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }

        private static float GetAlphaForVisuals(Player player)
        {
            float lifeRatio = player.statLife / (float)player.statLifeMax2;
            return Utils.Clamp(2 * (1f - lifeRatio), 0f, 1f);
        }
    }
}
