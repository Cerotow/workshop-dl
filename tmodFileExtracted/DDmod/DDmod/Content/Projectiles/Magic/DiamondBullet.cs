namespace DDmod.Content.Projectiles.Magic
{
    public class DiamondBullet : ModProjectile
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
            Projectile.localNPCHitCooldown = -1;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if(Projectile.damage<=0)
            {
                Projectile.Kill();
            }
            if (Projectile.localAI[1] >= 1 && Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
            }
            int Type = 91;
            if (Projectile.localAI[0] >= 2)
            {
                Projectile.scale *= 1.02F;
            }
            if (Projectile.localAI[0] == 1)
            {
                Projectile.scale *= 4f;
                for (int a = 1; a < Projectile.scale / 2; a++)
                {
                    if (Projectile.ai[1] == 0)
                    {
                        float R = Projectile.scale / 10 * Main.rand.NextFloat(0.5F, 1.5F);
                        Vector2 Position = new Vector2(Main.rand.NextFloat(Projectile.width / 2), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Position, Position * Projectile.scale / 300, Projectile.type, (int)(Projectile.damage * R / 10), Projectile.knockBack, Projectile.owner, 0, 1, R)];
                        projectile.scale = R;
                    }
                }
                NewDustChange((int)(Projectile.scale * 30), Projectile.position, Projectile.Size, Type, 0.5f * Projectile.scale, Projectile.scale, true, 1.5F * (Projectile.scale / 10), 100);

                Projectile.scale /= 4f;
                Projectile.timeLeft = 30;
                Projectile.localAI[0]++;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0)
            {
                for (int A = 0; A < 6; A++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale / 3;
                    dust.velocity *= 0.1f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                }
            }
            Projectile.ProjScaleChange();
            if (Projectile.ai[1] != 0)
            {
                Projectile.ai[1]++;
                if (Projectile.ai[1] > 60)

                    NPCdirection.Track(Projectile, 800, 20, 15, 60);
            }
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[1] == 0 || Projectile.ai[1] > 60;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.localAI[0] == 0)
                Projectile.localAI[0]++;
            Projectile.tileCollide = false;
            return Projectile.localAI[0] == 2;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreKill(int timeLeft)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.localAI[1]++;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if (width > 24)
            {
                width = 24;
            }
            if (height > 24)
            {
                height = 24;
            }
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
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
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(200, 200, 200, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(200, 200, 200, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(55, 55, 55, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 1.5F, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(200, 200, 200, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(55, 55, 55, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 1.5F, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(200, 200, 200, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(55, 55, 55, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale / 1.5F, spriteEffects, 0f);
            }
            return false;
        }
    }
}