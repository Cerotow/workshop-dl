namespace DDmod.Content.Projectiles.Summon
{
    public class TerraShit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.scale = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0.9f;
            return false;
        }
        public override void AI()
        {
            if (Projectile.GetGlobalProjectile<DDGlobalProjectile>().track >= 80)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + 1.57F;
                if (Projectile.GetGlobalProjectile<DDGlobalProjectile>().track >= 100)
                {
                    if (NPCdirection.FindClosest(Projectile.position, 1000, false) != null)
                    {
                        int num214 = NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 8, 8, 107, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 0.5f);
                        Dust dust2 = Main.dust[num214];
                        dust2.velocity *= -0.25f;
                        num214 = NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 8, 8, 107, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 0.5f);
                        dust2 = Main.dust[num214];
                        dust2.velocity *= -0.25f;
                        dust2 = Main.dust[num214];
                        dust2.position -= Projectile.velocity * 0.5f;
                    }
                }
            }
            else
            {
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.tileCollide = true;
            NPCdirection.Track(Projectile, 1000, 20, 12, 80, true);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if (Projectile.GetGlobalProjectile<DDGlobalProjectile>().track < 80)
            {
                fallThrough = false;
            }
            else
            {
                fallThrough = true;

            }
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            if (Projectile.GetGlobalProjectile<DDGlobalProjectile>().track < 80)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, 0, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(100, 255, 100, 0), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, 0, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(100, 255, 100, 0), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2), Projectile.scale, 0, 0f);
            }
            return false;
        }
    }
}