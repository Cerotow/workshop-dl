namespace DDmod.Content.Projectiles.Summon
{
    public class SummonBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            CooldownSlot = 1;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = Projectile.velocity.Length();
            }
            Projectile.ai[0]+=0.2F;
            NPCdirection.Track(Projectile, 1000, 10, Projectile.ai[0]);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = lightColor;
            color.A = 255;
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, color * 0.7f, Projectile.rotation, new Vector2(TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height()) / 2, Projectile.scale, 0, 0f);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Item54, Projectile.position);
            for (int num208 = 0; num208 < 60; num208++)
            {
                int num209 = 25;
                int num210 = NewDust(Projectile.Center - Vector2.One * num209, num209 * 2, num209 * 2, DustID.BubbleBurst_White, 0f, 0f, 0, default, 1f);
                Dust dust101 = Main.dust[num210];
                Vector2 vector10 = Vector2.Normalize(dust101.position - Projectile.Center);
                dust101.position = Projectile.Center + vector10 * 25f * Projectile.scale;
                if (num208 < 30)
                {
                    dust101.velocity = vector10 * dust101.velocity.Length();
                }
                else
                {
                    dust101.velocity = vector10 * Main.rand.Next(45, 91) / 10f;
                }
                dust101.color = Main.hslToRgb((float)(0.4000000059604645 + Main.rand.NextDouble() * 0.20000000298023224), 0.9f, 0.5f);
                dust101.color = Color.Lerp(dust101.color, Color.White, 0.3f);
                dust101.noGravity = true;
                dust101.scale = 0.7f;
            }
        }
    }
}