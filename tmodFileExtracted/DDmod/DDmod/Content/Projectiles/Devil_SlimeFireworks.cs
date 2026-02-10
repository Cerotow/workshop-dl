namespace DDmod.Content.Projectiles
{
    public class Devil_SlimeFireworks : ModProjectile
    {
        public override void Load()
        {
            asset = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Devil_Slime");
        }
        public static Asset<Texture2D> asset;
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Devil_Slime Fireworks");
           //DisplayName.AddTranslation(7, "Devil_Slime的烟花");
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.scale = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Main.projFrames[Projectile.type] = 3;
        }
        public void NDust(Texture2D texture, Vector2 Position)
        {
            Color[] colors = DDHelper.GetColors(texture);
            for (int i = 0; i < colors.Length; i++)
            {
                float x = i % texture.Width;
                float y = i / texture.Width;
                if (colors[i] != new Color(0, 0, 0, 0))
                {
                    Dust dust = Main.dust[NewDust(Position + (new Vector2(x, y) - texture.Size() / 2), 1, 1, 267, 0, 0, 0, colors[i], 2.5f)];
                    //dust.customData = true;
                    dust.noGravity = true;
                    dust.velocity = ((new Vector2(x, y) - texture.Size() / 2)) * 0.6f;
                }
            }
        }
        public void Data()
        {
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 1.5f;
            PlaySound(sound, Projectile.position);

            NDust(asset.Value, Projectile.Center);
        }
        public override void OnKill(int timeLeft)
        {
            Data();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 5 == 0)
            {
                Projectile.frame ++;
            }
            Projectile.frame %= 3;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(texture.Width / 3 * Projectile.frame, 0, texture.Width / 3, texture.Height)), Color.AliceBlue, Projectile.rotation, new Vector2(texture.Width / Main.projFrames[Projectile.type], texture.Height)/2, Projectile.scale, 0, 0f);
            return false;
        }
    }
}