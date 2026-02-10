namespace DDmod.Content.Projectiles.Summon
{
    public class ShadowFireballExplosion : ModProjectile
    {
        public float TelegraphDelay
        {
            get
            {
                return Projectile.ai[0];
            }
            set
            {
                Projectile.ai[0] = value;
            }
        }
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 3;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.width = (int)(24 * Projectile.scale);
                Projectile.height = (int)(24 * Projectile.scale);
                Projectile.position -= new Vector2(Projectile.width - 24, Projectile.height - 24) / 2;
                Projectile.ai[0] = 1;
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreKill(int timeLeft)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}