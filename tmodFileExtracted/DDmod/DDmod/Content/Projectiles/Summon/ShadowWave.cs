namespace DDmod.Content.Projectiles.Summon
{
    public class ShadowWave : ModProjectile
    {
        public float TelegraphDelay
        {
            get
            {
                return Projectile.ai[0];
            }
            set
            {
                Projectile.ai[0] = value;
            }
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            int Type = 27;
            Projectile.rotation = Projectile.velocity.ToRotation();

            for (float A = 0; A < 2; A++)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100)];
                dust.noGravity = true;
                dust.scale *= 1.25f;
                dust.velocity = -Projectile.velocity;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[1] == 1)
            {
                int A = NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FireballExplosion>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner, 0, 0);
                Main.projectile[A].scale = Projectile.scale;
            }
        }
        public override bool PreKill(int timeLeft)
        {
            int Type = 27;
            for (float A = 0; A < Projectile.scale; A += 0.01f)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(160, 160, 160))];
                dust.noGravity = true;
                dust.scale *= 2;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f * Projectile.scale, 2.5f * Projectile.scale);
            }
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0] == 0)
            {
                SpriteEffects spriteEffects = (SpriteEffects)1;
                if (Projectile.direction == 1)
                {
                    spriteEffects = (SpriteEffects)(-1);
                }
                Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
                Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(81, 6, 233, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            }

            return false;
        }
    }
}