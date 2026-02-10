using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Summon
{
    public class 狱火爆炸Proj : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 5;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (Projectile.ai[2] != 0)
            {
                Projectile.scale = Projectile.ai[2];
                Projectile.ai[2] = 0;
            }
            if (Projectile.ai[0] == 0)
            {
                DDust.NewDustChange4(100, Projectile.Center-new Vector2(4),Vector2.Zero, ModContent.DustType<速度粒子>(), 2 * Projectile.scale, 16 * Projectile.scale, true,2 * Projectile.scale, 7 * Projectile.scale, Rot:1000,color:new Color(253,Main.rand.Next(62,122),2,0),Data:4* Projectile.scale);
                SoundStyle sound = SoundID.Item14;
                sound.Pitch = -0.5F;
                PlaySound(sound, Projectile.Center);
                Projectile.ai[0] = 1;
            }
            Projectile.scale += 0.1F;
            Projectile.ProjScaleChange();
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool? CanDamage()
        {
            return base.CanDamage();
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
            target.AddBuff(ModContent.BuffType<地狱之火>(), 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}