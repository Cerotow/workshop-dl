namespace DDmod.Content.Projectiles.Melee
{
    public class EmeraldBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 22;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            DDHelper.BackAndForth(2, 3, 0.1f, ref Projectile.ai[0], ref Projectile.DProj().Bool[0]);
            int Type = 89;
            for (int A = 0; A < 1; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.5f;
                dust.velocity = Vector2.Zero;
            }
        }
        public override void OnKill(int timeLeft)
        {
            int Type = 89;
            for (int A = 0; A < 30; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1.8f;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2f, 3f);
            }
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 0.4f;
            PlaySound(sound, Projectile.position);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(33, 184, 115, 0);
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int a = 0; a < Projectile.ai[0]; a++)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / Projectile.ai[0]);
                    Main.spriteBatch.Draw(texture, vector2, null, oldcolor, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.ai[0] / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color*0.4f, Projectile.rotation, new Vector2(texture.Width, 0), Projectile.ai[0] / 2, spriteEffects, 0f);
            }
            return false;
        }
    }
}