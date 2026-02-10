namespace DDmod.Content.Projectiles
{
    public class 潮汐之泪proj : ModProjectile
    {
        public override void Load()
        {
        }
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
            Projectile.timeLeft = 30;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 30;
        }
        public override void OnKill(int timeLeft)
        {
            int Type = 33;
            for (float A = 0; A < Projectile.scale; A += 0.01f)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100)];
                dust.noGravity = true;
                dust.scale *= 2;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f * Projectile.scale, 2.5f * Projectile.scale);
            }
            Main.raining = true;
            Main.rainTime = DDHelper.Second(600);
            Main.maxRaining = 0.4F;
            Main.NewText(Language.GetTextValue("Mods.DDmod.ItemTips.开始下起了倾盆大雨"), 25,99,192);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.position = Main.player[Projectile.owner].position;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}