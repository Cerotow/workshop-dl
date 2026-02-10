using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles
{
    public class UPProj : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide =false;
            Projectile.penetrate = -1;
            
        }
        Vector2[] Vector;
        public override void AI()
        {
            Color color = new Color(255, 188, 35, 0);
            Color color2 = new Color(255, 188, 35, 0);
            if (Projectile.ai[0]==1)
            {
                color = new Color(205, 20, 10, 220);
                color2 = new Color(205, 20, 10, 220);
            }
            NewDustChange2(100, Projectile.Center, Vector2.Zero, ModContent.DustType<光球粒子>(), 0, 8, false, 0.2F, 1, 0, color);
            NewDustChange2(50, Projectile.Center, Vector2.Zero, ModContent.DustType<速度粒子>(), 8, 12, true, 2F, 4, 0, color2);
            SoundStyle sound = new SoundStyle(DDHelper.Sound(1, "升级"));
            sound.Pitch = Main.rand.NextFloat(-0.3F, 0.3F);
            PlaySound(sound, Projectile.Center);
            Projectile.Kill();
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        { }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
