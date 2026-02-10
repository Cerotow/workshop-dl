namespace DDmod.Content.Projectiles.Magic
{
    public class 测试蠕虫弹幕 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("测试蠕虫弹幕");
           //DisplayName.AddTranslation(7, "测试蠕虫弹幕");

        }

        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
        }
        int[] Body = new int[7];
        Vector2[] Center = new Vector2[7];
        float[] Rotation = new float[7];
        public override bool PreDraw(ref Color lightColor)
        {
            int Length = 20;
            if (Body.Length != Length)
            {
                Body = new int[Length];
                Center = new Vector2[Length];
                Rotation = new float[Length];
                for (int B = 0; B < Length; B++)
                {
                    Center[B] = Projectile.Center - new Vector2(0.1f);
                }
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Center[0] = Projectile.Center;
            Rotation[0] = Projectile.rotation;
            for (int B = 0; B < Body.Length; B++)
            {
                if (B > 0)
                {
                    Vector2 vector = Center[B - 1] - Center[B];
                    Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;

                    float Distance = (vector.Length() - (10 * Projectile.scale)) / vector.Length();
                    Center[B] = Center[B] + vector * Distance;
                }
            }
            for (int B = Body.Length - 1; B >= 0; B--)
            {
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[0], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
            }
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.Track(1000,120,12,30);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle[] vectors = new Rectangle[Body.Length];
            bool B = false;
            for (int A = 0; A < Body.Length; A++)
            {
                Vector2 Size = Projectile.Size / 2;
                vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
                if (vectors[A].Intersects(targetHitbox))
                {
                    B = true;
                }
            }
            return new bool?(B);
        }
    }
}