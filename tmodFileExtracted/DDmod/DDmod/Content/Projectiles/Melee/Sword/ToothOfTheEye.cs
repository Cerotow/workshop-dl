namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class ToothOfTheEye : ModProjectile
    {
        public static Asset<Texture2D> texture;
        public override void Load()
        {
            texture = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Melee/Sword/变身");
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 161;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 360;
            Projectile.hide = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.MeleeProj().DaggerDashDistance = 20;
        }
        int T = 0;
        int T2 = 0;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            return true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
            float num2 = 0.75f;
            float LaserLength = MathHelper.Lerp(0, 36 * Projectile.scale, num2);
            Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), 0, default);
            float num = 0f;
            return new bool?(Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), player.Center + Pvelocity * 18, Projectile.Center + Pvelocity * LaserLength, projHitbox.Width, ref num));
        }
        public override bool? CanCutTiles()
        {
            Player player = Main.player[Projectile.owner];
            float num2 = 0.75f;
            float LaserLength = MathHelper.Lerp(0, 36 * Projectile.scale, num2);
            Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), 0, default);
            DelegateMethods.tilecut_0 = (Terraria.Enums.TileCuttingContext)2;
            Utils.PlotTileLine(player.Center, player.Center + Pvelocity * LaserLength, Projectile.width * Projectile.scale * 2f, new Utils.TileActionAttempt(DelegateMethods.CutTiles));
            return new bool?(true);
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override bool? CanDamage()
        {
            return true;
        }
        int A;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 Center;
            if (Projectile.DProj().Bool[0])
            {
                player.Dplayer().Hide2 = 5;
                Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                    Color AlphaColor = new Color(255, 25, 25, 25) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture.Value, vector2, new Rectangle?(new Rectangle(0, 0, texture.Width(), texture.Height() / 2)), AlphaColor*0.6f, Projectile.oldRot[i] + MathHelper.PiOver2, new Vector2(texture.Width() / 2, texture.Height() / 4), new Vector2(3F), 0, 0f);

                }
                Center = Projectile.Center - Main.screenPosition;
                Main.spriteBatch.Draw(texture.Value, Center, new Rectangle?(new Rectangle(0, 0, texture.Width(), texture.Height() / 2)), lightColor * 0.8f, Projectile.rotation + MathHelper.PiOver2, new Vector2(texture.Width() / 2, texture.Height() / 4), Projectile.scale * 2.5f, 0, 0f);
            }
            else
            {
                Center = Projectile.Center - Main.screenPosition;
                Texture2D Projtexture = TextureAssets.Projectile[Projectile.type].Value;
                Main.spriteBatch.Draw(Projtexture, Center, new Rectangle?(new Rectangle(0, 0, Projtexture.Width, Projtexture.Height)), lightColor, Projectile.rotation - MathHelper.PiOver4, new Vector2(Projtexture.Width / 2, Projtexture.Height / 2), Projectile.scale, 0, 0f);
            }
            return false;
        }
    }
}