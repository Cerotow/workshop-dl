namespace DDmod.Content.Projectiles.Magic
{
    public class CrystalThorn : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 34;
            Projectile.height = 34;
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
            if (Projectile.localAI[1] >= 1 && Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
            }
            int Type = Main.rand.Next(new int[] { 68, 70 });
            if (Projectile.localAI[0] == 1)
            {
                for (int a = 1; a < Projectile.scale * 4; a++)
                {
                    if (Projectile.ai[1] == 0)
                    {
                        Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), Main.rand.NextFloat(-0.2F, 0.2F) * Projectile.scale, default);
                        Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, Pvelocity * 30, ModContent.ProjectileType<水晶刺>(), (int)(Projectile.ai[0] / 2), Projectile.knockBack, Projectile.owner, 0, Main.rand.Next((int)(6 + Projectile.velocity.Length() / 4), (int)(12 + Projectile.velocity.Length() / 2)), 0.5F)];
                        projectile.CritChance = Projectile.CritChance;
                    }
                }
                for (int A = 0; A < (int)(Projectile.scale * 300); A++)
                {
                    Type = Main.rand.Next(new int[] { 68, 70 });
                    Dust dust = Main.dust[NewDust(Projectile.position, (int)Projectile.Size.X, (int)Projectile.Size.Y, Type, 0, 0, 100, default)];
                    dust.noGravity = true;
                    dust.scale = 1.5F * (Projectile.scale / 2);
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2 * Projectile.scale, 5 * Projectile.scale);
                }
                Projectile.scale *= 2f;
                Projectile.timeLeft = 3;
                Projectile.velocity = Vector2.Zero;
                Projectile.localAI[0]++;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0)
            {
                for (float A = 0; A < Projectile.scale; A += 0.5f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale *= 1.25f;
                    dust.velocity *= 0.1f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                }
            }
            Projectile.ProjScaleChange();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.localAI[0] == 0)
                Projectile.localAI[0]++;
            Projectile.tileCollide = false;
            Projectile.velocity = oldVelocity;
            return Projectile.localAI[0] == 2;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool PreKill(int timeLeft)
        {
            if (Projectile.localAI[0] == 0)
                Projectile.localAI[0]++;
            return Projectile.localAI[0] == 2;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.localAI[1]++;
            if (Projectile.localAI[0] == 0)
            {
                int Type = Main.rand.Next(68, 71);
                for (float A = 0; A < Projectile.scale; A += 0.01f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale *= 1 * (Projectile.scale / 2);
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f * Projectile.scale, 2.5f * Projectile.scale);
                    dust.color = new Color(155, 200, 100, 100);
                }
            }
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
                    Color color = new Color(55, 55, 55, 0);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)/4, spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(155, 155, 155, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/4, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(155, 155, 155, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/4, spriteEffects, 0f);
            }
            return false;
        }
    }
}