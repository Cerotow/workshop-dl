namespace DDmod.Content.Projectiles.Summon
{
    public class FireBall : ModProjectile
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = 1;
        }
        public override void AI()
        {
            int Type = 6;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0)
            {
                if (Projectile.ai[0] == 0)
                {
                    Projectile.width = (int)(24 * Projectile.scale);
                    Projectile.height = (int)(24 * Projectile.scale);
                    Projectile.position -= new Vector2(Projectile.width - 24, Projectile.height - 24) / 2;
                    Projectile.ai[0] = 1;
                }
                for (float A = 0; A < Projectile.scale; A += 0.5f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100)];
                    dust.noGravity = true;
                    dust.scale *= 1.25f;
                    dust.velocity *= 0.1f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                }
            }

            if (Projectile.ai[2] ==1)
            {
                Projectile.Track(500,21,12,10);
            }
            if (Projectile.ai[1] == 1)
            {
                for (int A = 0; A < 200; A++)
                {
                    if (Main.npc[A].CanBeChasedBy() && Projectile.Colliding(Projectile.getRect(), Main.npc[A].getRect()))
                    {
                        Projectile.Kill();
                    }
                }
            }
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[1] != 1;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[1] == 1)
            {
                int A = NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<FireballExplosion>(), (int)(Projectile.damage * 1f), Projectile.knockBack, Projectile.owner, 0, 0);
                Main.projectile[A].scale = Projectile.scale;
            }
        }
        public override bool PreKill(int timeLeft)
        {
            int Type = 6;
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
                    spriteEffects = 0;
                }
                Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
                Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(253, 62, 3, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 5, spriteEffects, 0f);

                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(253, 62, 3, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    //Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vector2, null, color * 0.6f, Projectile.rotation, ModContent.Request<Texture2D>("DDmod/Image/VoidStar").Size() / 2, Projectile.scale / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 5 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                for (int i = 0; i < 3; i++)
                {
                    Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition + new Vector2(2 * Projectile.scale).RotatedBy(MathHelper.TwoPi / 3 * i + (float)Projectile.timeLeft / 10), null, new Color(253, 62, 3, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 5, spriteEffects, 0f);
                }
            }
            return false;
        }
    }
}