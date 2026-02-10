using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Projectiles.Melee
{
    public class MeleeAcorn : ModProjectile
    {
        public override void SetDefaults()
        {

            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Main.projFrames[Projectile.type] = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void SetStaticDefaults()
        {
        }
        public override void AI()
        {
            Projectile.CritChance = 0;
            Projectile.localAI[0]++;
            if (Projectile.originalDamage==0)
            {
                Projectile.originalDamage = Projectile.damage;
            }
            Projectile.damage = (int)(Projectile.originalDamage * (0.2F+Projectile.velocity.Length()/4));
            if (Projectile.ai[2] != 10000&& Projectile.localAI[0]>20)
            {
                if(Projectile.ai[2]==0)
                    Projectile.rotation += 0.1F;
                for (int A = 0; A < 1000; A++)
                {
                    Projectile proj = Main.projectile[A];
                    if (proj.active && proj.Player().HeldItem.type==ModContent.ItemType<AcornSword>() && proj.type == ModContent.ProjectileType<GlobalSword>()&&proj.localAI[1] > 0)
                    {
                        if (proj.Colliding(proj.getRect(), Projectile.getRect()))
                        {
                            Projectile.velocity = proj.DProj().vector[0] * 16;
                            Projectile.ai[2] = 1;
                            Projectile.netUpdate = true;
                        }
                        break;
                    }

                }
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[2]==1;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            if (Projectile.ai[2] == 0)
            {
                Projectile.ai[2] = 10000;
            }
            Projectile.netUpdate = true;
            Projectile.velocity *= 0.5f;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int A158 = 0; A158 < 20; A158++)
            {
                int A159 = NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 7, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 0.5f);
                if (Main.rand.NextBool(3))
                {
                    Main.dust[A159].fadeIn = 1.1f + Main.rand.Next(-10, 11) * 0.01f;
                    Main.dust[A159].scale = 0.35f + Main.rand.Next(-10, 11) * 0.01f;
                    Main.dust[A159].type++;
                }
                else
                {
                    Main.dust[A159].scale = 1.2f + Main.rand.Next(-10, 11) * 0.01f;
                }
                Main.dust[A159].noGravity = true;
                Main.dust[A159].velocity *= 2.5f;
                Main.dust[A159].velocity -= Projectile.oldVelocity / 10f;
            }
        }
    }
}