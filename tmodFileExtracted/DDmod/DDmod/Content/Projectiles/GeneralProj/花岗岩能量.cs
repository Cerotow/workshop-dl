
using DDmod.Content.Buffs.DeBuffs;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.GeneralProj
{
    public class 花岗岩能量 : ModProjectile
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void Load()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 2;
            Projectile.extraUpdates = 50;
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
            Projectile.rotation = Projectile.velocity.ToRotation();
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if(npc.active)
            {
                Projectile.timeLeft = 2;
            }
            Vector2 vector = npc.Center - Projectile.Center;
            Projectile.velocity = vector.PerfectNormalize() * 4;
            int R =NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<速度粒子>(), Projectile.velocity.X/2, Projectile.velocity.Y/2, 100, new Color(50, 155, 255, 0), 2);
            Main.dust[R].velocity = Projectile.velocity/2;
            Main.dust[R].rotation = Projectile.rotation;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == Projectile.ai[0])
            {
                return base.CanHitNPC(target);
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            NewDustChange2(30,Projectile.Center-new Vector2(4),Vector2.Zero,ModContent.DustType<速度粒子>(),1,2,true,1,3,100,new Color(50,155,255,0));
            Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
        }
    }
}
