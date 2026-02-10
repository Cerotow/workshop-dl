
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 血 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 0;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
            if (!Projectile.DProj().Bool[0])
            {
                Projectile.scale = Main.rand.NextFloat(1.2F, 2F);
                Projectile.DProj().Bool[0] = true;
            }
            Projectile.ai[2]++;
            if (Projectile.ai[2] % 1 == 0)
            {
                int R = NewDust(Projectile.Center - new Vector2(4), 0, 0, 5, 0, 0, 100, new Color(50, 155, 255, 0), Projectile.scale);
                Main.dust[R].velocity = Projectile.velocity*0.2F;
                Main.dust[R].noGravity = true; ;
            }
            Projectile.ProjScaleChange();
        }
        public override bool? CanHitNPC(NPC target)
        {
                return base.CanHitNPC(target);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override void OnKill(int timeLeft)
        {
            NewDustChange(20,Projectile.Center-new Vector2(4),Vector2.Zero,5,0,3,false, Main.rand.NextFloat(0.4F, 1F)* Projectile.scale, 100);
        }
    }
}
