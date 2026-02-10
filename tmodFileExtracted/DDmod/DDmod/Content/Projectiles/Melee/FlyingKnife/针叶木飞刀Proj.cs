namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 针叶木飞刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 20; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                NewDust(Projectile.position, 1, 1, 214, projDirection.X, projDirection.Y,0,default,1f);
            }

        }
    }
}