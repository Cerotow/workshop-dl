namespace DDmod.Content.Projectiles.Magic
{
    public class ForestBullets : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1F;
            Projectile.timeLeft = 200;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            if (Projectile.localAI[1] >= 2 && Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
            }
            int Type = ModContent.DustType<Dusts.生命粒子>();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            for (float A = 0; A < Projectile.scale; A += 0.5f)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale *= 0.5f;
                dust.velocity *= 0.1f;
                Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                dust.velocity += -vector * Projectile.scale;
            }
            if (Projectile.npcProj)
            {
                int PlayerID = Player.FindClosest(Projectile.Center, 1, 1);
                Player player = Main.player[PlayerID];
                Vector2 vector = player.Center - Projectile.Center;
                if (vector.Length() < 10)
                {
                    if (player.whoAmI == Main.myPlayer)
                        player.Heal(1 + player.statLifeMax2 / 100);
                    Projectile.Kill();
                }
                if (vector.Length() < 800)
                {
                    Projectile.velocity = (Projectile.velocity * 20 + vector.PerfectNormalize() * 5) / 21;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Projectile.ProjScaleChange();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            int Type = ModContent.DustType<Dusts.生命粒子>();
            for (float A = 0; A < Projectile.scale; A += 0.01f)
            {
                Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale *= 1 * (Projectile.scale / 2);
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f * Projectile.scale, 2.5f * Projectile.scale);
            }
        }
        public override bool PreKill(int timeLeft)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            if (Projectile.localAI[0] == 0)
            {
                SpriteEffects spriteEffects = (SpriteEffects)1;
                if (Projectile.direction == 1)
                {
                    spriteEffects = 0;
                }
                Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(125, 213, 18, 20) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(255, 255, 255, 255), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            }
            return false;
        }
    }
}