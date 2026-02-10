using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee
{
    public class FragileHeartOfLife : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetStaticDefaults()
        {
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.03f;
            Projectile.velocity.X *= 0.98f;
            Projectile.velocity.Y += 0.5f;
        }

        public override void OnKill(int timeLeft)
        {

            for (int A = 0; A < 200; A++)
            {
                if (Main.npc[A].active && !Main.npc[A].dontTakeDamage && Main.npc[A].CanBeChasedBy(Projectile, false) && (Projectile.Center - Main.npc[A].Center).Length() < 50)
                {
                    Main.player[Projectile.owner].ApplyDamageToNPC(Main.npc[A], Projectile.damage, Projectile.knockBack, 2, false);
                }
            }
            float scale = 5;
            float velocity = 2;
            Vector2 projDirection = Utils.RotatedBy(new Vector2(0, -2 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection - Vector2.Zero) * velocity;

            Vector2 projDirection2 = Utils.RotatedBy(new Vector2(1 * scale, -3 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection2, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection2 - Vector2.Zero) * velocity;

            Vector2 projDirection3 = Utils.RotatedBy(new Vector2(-1 * scale, -3 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection3, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection3 - Vector2.Zero) * velocity;

            Vector2 projDirection4 = Utils.RotatedBy(new Vector2(+2 * scale, -4 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection4, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection4 - Vector2.Zero) * velocity;

            Vector2 projDirection5 = Utils.RotatedBy(new Vector2(-2 * scale, -4 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection5, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection5 - Vector2.Zero) * velocity;

            Vector2 projDirection6 = Utils.RotatedBy(new Vector2(+3 * scale, -4 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection6, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection6 - Vector2.Zero) * velocity;

            Vector2 projDirection7 = Utils.RotatedBy(new Vector2(-3 * scale, -4 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection7, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection7 - Vector2.Zero) * velocity;

            Vector2 projDirection8 = Utils.RotatedBy(new Vector2(+4 * scale, -3 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection8, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection8 - Vector2.Zero) * velocity;

            Vector2 projDirection9 = Utils.RotatedBy(new Vector2(-4 * scale, -3 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection9, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection9 - Vector2.Zero) * velocity;

            Vector2 projDirection10 = Utils.RotatedBy(new Vector2(+4 * scale, -2 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection10, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection10 - Vector2.Zero) * velocity;

            Vector2 projDirection11 = Utils.RotatedBy(new Vector2(-4 * scale, -2 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection11, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection11 - Vector2.Zero) * velocity;

            Vector2 projDirection12 = Utils.RotatedBy(new Vector2(+4 * scale, -1 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection12, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection12 - Vector2.Zero) * velocity;

            Vector2 projDirection13 = Utils.RotatedBy(new Vector2(-4 * scale, -1 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection13, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection13 - Vector2.Zero) * velocity;

            Vector2 projDirection14 = Utils.RotatedBy(new Vector2(+3 * scale, 0), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection14, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection14 - Vector2.Zero) * velocity;

            Vector2 projDirection15 = Utils.RotatedBy(new Vector2(-3 * scale, 0), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection15, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection15 - Vector2.Zero) * velocity;

            Vector2 projDirection16 = Utils.RotatedBy(new Vector2(+2 * scale, +1 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection16, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection16 - Vector2.Zero) * velocity;

            Vector2 projDirection17 = Utils.RotatedBy(new Vector2(-2 * scale, +1 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection17, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection17 - Vector2.Zero) * velocity;

            Vector2 projDirection18 = Utils.RotatedBy(new Vector2(+1 * scale, +2 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection18, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection18 - Vector2.Zero) * velocity;

            Vector2 projDirection19 = Utils.RotatedBy(new Vector2(-1 * scale, +2 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection19, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection19 - Vector2.Zero) * velocity;

            Vector2 projDirection20 = Utils.RotatedBy(new Vector2(0, +3 * scale), Projectile.rotation, default);
            Main.dust[NewDust(Projectile.Center + projDirection20, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, new Color(255, 0, 0, 0), 0.5f)].velocity = Vector2.Normalize(projDirection20 - Vector2.Zero) * velocity;
            TryGetActiveSound(PlaySound(SoundID.Item4, Projectile.position), out var Sound);
            Sound.Sound.Pitch = 1.5f;
        }
    }
}