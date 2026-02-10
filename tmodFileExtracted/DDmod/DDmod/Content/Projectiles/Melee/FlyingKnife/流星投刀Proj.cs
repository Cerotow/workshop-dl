namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 流星投刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            Projectile.width = 20;
            Projectile.height = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        }
        public override void PostAI()
        {
            if (Main.rand.NextBool(5))
            {
                NewDust(Projectile.position, Projectile.width, Projectile.height, 6,-Projectile.velocity.X/10, -Projectile.velocity.Y / 10, 0, default, 1f);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(BuffID.OnFire, 180);
            }
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            /*
            for (int i = 0; i < 20; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                NewDust(Projectile.position, Projectile.width, Projectile.height, 214, projDirection.X, projDirection.Y,0,default,1.3f);
            }
            for (int i = 0; i < 5; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                NewDust(Projectile.position, 1, 1, 6, projDirection.X, projDirection.Y,0,default,1f);
            }*/
            if (Projectile.owner == Main.myPlayer)
            {
                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FlameExplosion>(), Projectile.damage, 0, player.whoAmI, 0);
            }

        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, texture.Height * 0.25F);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height * 0.75F);
            }
            
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float RO2 = Projectile.oldRot[i];
                if (Projectile.velocity.X < 0)
                    RO2 -= Rotation2;


                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Color color = new Color(233, 86, 3, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, vector2, null, color, RO2, Origia, Projectile.scale, sprite, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, RO, Origia, Projectile.scale, sprite, 0f);
        }
    }
}