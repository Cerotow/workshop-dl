using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 远古短刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
        }
        public override void PostAI()
        {
            NewDustChange(3, Projectile.Center+Projectile.velocity - new Vector2(4), Vector2.Zero, 172, 0, 4, true, 0.75f);
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 20; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                NewDust(Projectile.Center, 1, 1, 172, projDirection.X, projDirection.Y,0,default,0.75f);
            }

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 10, ModContent.ProjectileType<MuramasaSlash>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, target.whoAmI, 1);
        }
    }
}