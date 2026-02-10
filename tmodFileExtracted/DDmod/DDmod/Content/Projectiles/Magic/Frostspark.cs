namespace DDmod.Content.Projectiles.Magic
{
    public class Frostspark : ModProjectile
    {
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
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 100;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (Projectile.localAI[1] >= 1 && Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
            }
            int Type = 135;
            if (Projectile.localAI[0] == 1)
            {
                Projectile.scale *= 4f;
                NewDustChange((int)(Projectile.scale * 40), Projectile.position, Projectile.Size, Type, 0.5f * Projectile.scale, 1.5f * Projectile.scale, true, 1.5F * (Projectile.scale / 10), 100);

                Projectile.timeLeft = 3;
                Projectile.localAI[0]++;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0)
            {
                for (int A = 0; A < 6; A++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale / 2;
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
                int Type = 135;
                for (float A = 0; A < Projectile.scale; A += 0.01f)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(2), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                    dust.noGravity = true;
                    dust.scale *= 1 * (Projectile.scale / 2);
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f * Projectile.scale, 2.5f * Projectile.scale);
                }
            }
            target.AddBuff(BuffID.Frostburn, 300);
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
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(253, 62, 3, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/5, spriteEffects, 0f);

                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(0, 186, 242, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    //Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vector2, null, color * 0.6f, Projectile.rotation, ModContent.Request<Texture2D>("DDmod/Image/VoidStar").Size() / 2, Projectile.scale / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/5 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                for (int i = 0; i < 3; i++)
                {
                    Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition + new Vector2(2* Projectile.scale).RotatedBy(MathHelper.TwoPi / 3*i + Projectile.timeLeft / 10), null, new Color(0, 186, 242, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/5, spriteEffects, 0f);
                }
            }
            return false;
        }
    }
}