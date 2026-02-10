namespace DDmod.Content.Projectiles.Ranged.Ammo
{
    public class VampiricArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.arrow = true;
        }

        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Vampiric arrow");
           //DisplayName.AddTranslation(7, "吸血鬼箭");
        }
        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 5;
                NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 5);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.NextBool(5))
            {
                float lifeStoled = damageDone * 0.1f;
                if (lifeStoled < 1)
                {
                    lifeStoled = 1;
                }
                if (lifeStoled > player.statLifeMax2 / 100)
                {
                    lifeStoled = player.statLifeMax2 / 100;
                }
                if (target.HasBuff(30)) lifeStoled *= 2;
                if ((int)lifeStoled > 0 && !player.moonLeech && target.type != NPCID.TargetDummy && target.life > 2 && player.Dplayer().VampireCD <= 0)
                {
                    player.Dplayer().VampireCD = 60;
                    NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileID.VampireHeal, 0, 0f, player.whoAmI, player.whoAmI, lifeStoled);
                }
                if (Main.rand.NextBool(5))
                {
                    target.AddBuff(30, Main.rand.Next(100, 300));
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 20; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 3));
                NewDust(Projectile.position, 1, 1, 5, projDirection.X, projDirection.Y);
            }

        }
    }
}
