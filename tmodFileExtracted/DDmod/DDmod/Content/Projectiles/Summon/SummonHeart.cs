using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Summon
{
    public class SummonHeart : ModProjectile
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
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 0.75F;
            Projectile.timeLeft = 600;
            CooldownSlot = 1;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[0] != 0;
        }
        public void Data(Projectile Projectile)
        {
            float scale = 5;
            float velocity = 2;
            Vector2 projDirection = Utils.RotatedBy(new Vector2(0, -2 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection - Vector2.Zero) * velocity;

            Vector2 projDirection2 = Utils.RotatedBy(new Vector2(1 * scale, -3 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection2, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection2 - Vector2.Zero) * velocity;

            Vector2 projDirection3 = Utils.RotatedBy(new Vector2(-1 * scale, -3 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection3, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection3 - Vector2.Zero) * velocity;

            Vector2 projDirection4 = Utils.RotatedBy(new Vector2(+2 * scale, -4 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection4, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection4 - Vector2.Zero) * velocity;

            Vector2 projDirection5 = Utils.RotatedBy(new Vector2(-2 * scale, -4 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection5, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection5 - Vector2.Zero) * velocity;

            Vector2 projDirection6 = Utils.RotatedBy(new Vector2(+3 * scale, -4 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection6, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection6 - Vector2.Zero) * velocity;

            Vector2 projDirection7 = Utils.RotatedBy(new Vector2(-3 * scale, -4 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection7, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection7 - Vector2.Zero) * velocity;

            Vector2 projDirection8 = Utils.RotatedBy(new Vector2(+4 * scale, -3 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection8, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection8 - Vector2.Zero) * velocity;

            Vector2 projDirection9 = Utils.RotatedBy(new Vector2(-4 * scale, -3 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection9, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection9 - Vector2.Zero) * velocity;

            Vector2 projDirection10 = Utils.RotatedBy(new Vector2(+4 * scale, -2 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection10, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection10 - Vector2.Zero) * velocity;

            Vector2 projDirection11 = Utils.RotatedBy(new Vector2(-4 * scale, -2 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection11, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection11 - Vector2.Zero) * velocity;

            Vector2 projDirection12 = Utils.RotatedBy(new Vector2(+4 * scale, -1 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection12, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection12 - Vector2.Zero) * velocity;

            Vector2 projDirection13 = Utils.RotatedBy(new Vector2(-4 * scale, -1 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection13, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection13 - Vector2.Zero) * velocity;

            Vector2 projDirection14 = Utils.RotatedBy(new Vector2(+3 * scale, 0), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection14, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection14 - Vector2.Zero) * velocity;

            Vector2 projDirection15 = Utils.RotatedBy(new Vector2(-3 * scale, 0), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection15, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection15 - Vector2.Zero) * velocity;

            Vector2 projDirection16 = Utils.RotatedBy(new Vector2(+2 * scale, +1 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection16, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection16 - Vector2.Zero) * velocity;

            Vector2 projDirection17 = Utils.RotatedBy(new Vector2(-2 * scale, +1 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection17, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection17 - Vector2.Zero) * velocity;

            Vector2 projDirection18 = Utils.RotatedBy(new Vector2(+1 * scale, +2 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection18, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection18 - Vector2.Zero) * velocity;

            Vector2 projDirection19 = Utils.RotatedBy(new Vector2(-1 * scale, +2 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection19, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection19 - Vector2.Zero) * velocity;

            Vector2 projDirection20 = Utils.RotatedBy(new Vector2(0, +3 * scale), Projectile.rotation + MathHelper.Pi, default);
            Main.dust[NewDust(Projectile.Center + projDirection20, 1, 1, ModContent.DustType<爱心粒子>(), 0, 0, 0, default, 0.5f)].velocity = Vector2.Normalize(projDirection20 - Vector2.Zero) * velocity;
            TryGetActiveSound(PlaySound(SoundID.Item4, Projectile.position), out var Sound);
            Sound.Sound.Pitch = 1.5f;
        }
        public override void AI()
        {
            int PlayerID = Player.FindClosest(Projectile.Center, 1, 1);
            Player player = Main.player[PlayerID];
            if (Projectile.ai[0] != 0)
            {
                if (Projectile.ai[0] == 1)
                {
                    Vector2 vector = player.Center - Projectile.Center;
                    if (vector != Vector2.Zero) vector.Normalize();
                    Projectile.ai[1]++;
                    if (Projectile.ai[1] < 15)
                    {
                        Projectile.velocity = -vector * 5;
                    }
                    else
                    {
                        Projectile.velocity = vector * 8;
                        if ((player.Center - Projectile.Center).Length() < 18)
                        {
                            if (Main.netMode != NetmodeID.SinglePlayer)
                            {
                                NetMessage.SendData(66, -1, -1, null, player.whoAmI, 2, 0f, 0f, 0, 0, 0);
                            }
                            Data(Projectile);
                            player.statLife += 2;
                            player.HealEffect(2);
                            Projectile.Kill();
                        }
                    }
                    Projectile.rotation = vector.ToRotation() + MathHelper.Pi / 2;
                }
                return;
            }
            if (Projectile.velocity == Vector2.Zero)
            {
                if ((player.Center - Projectile.Center).Length() < 500)
                {
                    TryGetActiveSound(PlaySound(SoundID.Item4, Projectile.position), out var Sound);
                    Sound.Sound.Pitch = -0.5f;
                    Projectile.ai[0] = 1;
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 projDirection = Utils.RotatedBy(new Vector2(2), (double)(MathHelper.TwoPi / 5 * i), default);
                        int T = NewDust(Projectile.Center, 1, 1, ModContent.DustType<爱心粒子>(), projDirection.X, projDirection.Y, 0, default, 0.5f);
                        Main.dust[T].velocity = projDirection;
                    }
                }
            }
            else
            {
                if (Projectile.velocity.Length() <= 0.2f)
                {
                    Projectile.velocity = Vector2.Zero;
                }
                else
                {
                    Projectile.velocity *= 0.98f;
                }
            }
            Projectile.rotation = MathHelper.Pi;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5) && Projectile.ai[0] == 0)
            {
                Main.player[Projectile.owner].AddBuff(ModContent.BuffType<BlessingOfTheHeart>(), 300);
            }
            Data(Projectile);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition;
                Color color = new Color(214, 70, 100, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/爱心光效"), vector2, null, color, Projectile.rotation, ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/爱心光效").Size() / 2, Projectile.scale * 1.2f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }

            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 2);
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/爱心光效"), Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, new Color(214, 70, 100, 0), Projectile.rotation, ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/爱心光效").Size() / 2, Projectile.scale * 1.2f, spriteEffects, 0f);
            Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, new Color(214, 70, 100, 150), Projectile.rotation, vector, Projectile.scale, spriteEffects, 0f);
            return false;
        }
    }
}