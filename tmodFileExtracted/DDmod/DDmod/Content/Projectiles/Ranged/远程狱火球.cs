namespace DDmod.Content.Projectiles.Ranged
{
    public class 远程狱火球 : ModProjectile
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
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.scale = 1.75F;
            Projectile.width = (int)(Projectile.width/ Projectile.scale);
            Projectile.height = (int)(Projectile.height/ Projectile.scale);
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            if (Projectile.localAI[1] >= 1 && Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
            }
            int Type = 6;
            if (Projectile.localAI[0] == 1)
            {
                NewDustChange((int)(Projectile.scale * 10), Projectile.position, Projectile.Size, ModContent.DustType<Dusts.拉长粒子>(), 3 * Projectile.scale, 7 * Projectile.scale, true, Main.rand.NextFloat(1,2), 0, new Color(253, 162, 3,40));
                NewDustChange((int)(Projectile.scale * 60), Projectile.position, Projectile.Size, Type, 2 * Projectile.scale, 5 * Projectile.scale, true, 1.5F * (Projectile.scale / 2), 100);
                Projectile.scale *= 3f;
                Projectile.timeLeft = 3;
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
                if (Projectile.timeLeft <3)
                {
                    Projectile.localAI[0]++;
                }
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 120)
            {
                Projectile.Track(500, 30,20, 0);
            }
            else
            {
                if (Projectile.velocity.Length() < 20)
                {
                    Projectile.velocity += Projectile.velocity.PerfectNormalize();
                }
            }
            if (Projectile.localAI[0] >= 1) Projectile.velocity = Vector2.Zero;
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
            return true;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.localAI[0] == 0)
                Projectile.localAI[0]++;
            Projectile.tileCollide = false;
            target.AddBuff(ModContent.BuffType<地狱之火>(),300);
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
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(192, 74, 90, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/5, spriteEffects, 0f);

                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color color = new Color(192, 74, 90, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    //Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vector2, null, color * 0.6f, Projectile.rotation, ModContent.Request<Texture2D>("DDmod/Image/VoidStar").Size() / 2, Projectile.scale / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/5 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, vector2-Projectile.velocity/2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/5 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                for (int i = 0; i < 4; i++)
                {
                    Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition + new Vector2(2* Projectile.scale).RotatedBy(MathHelper.TwoPi / 4*i + (float)Projectile.timeLeft / 10), null, new Color(253, 62, 3, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale/5, spriteEffects, 0f);
                }
            }
            return false;
        }
    }
}