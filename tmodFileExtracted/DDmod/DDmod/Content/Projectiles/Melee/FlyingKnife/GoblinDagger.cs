namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class GoblinDagger : 飞刀Proj
    {


        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI2;
            Rotation = MathHelper.PiOver4;
            Rotation2 = MathHelper.PiOver2;
            Projectile.penetrate = 3;
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            for (int A = 0; A < 10; A++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 1, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default(Color), 0.75f);
            }

        }
    }
}