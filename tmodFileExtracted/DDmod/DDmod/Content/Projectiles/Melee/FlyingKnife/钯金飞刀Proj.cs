using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 钯金飞刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            Projectile.penetrate = 1;
        }
        public override bool PreAI()
        {
            return base.PreAI();
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(10))
            {
                NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(Main.rand.NextFloat(-3,3), Main.rand.NextFloat(-4, -8)), ModContent.ProjectileType<钯金心>(), Projectile.damage / 2, 0, Projectile.owner, target.whoAmI);
            }
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 20; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                NewDust(Projectile.position, 1, 1, 144, projDirection.X, projDirection.Y, 0, default, 1f);
            }
        }
    }
    public class 钯金心 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Palladium heart");
           //DisplayName.AddTranslation(7, "钯金心");
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 0;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void AI()
        {
            for (int a = 0; a < 255; a++)
            {
                if(Main.player[a].活着()&& Main.player[a].getRect().Intersects(Projectile.getRect()))
                {
                    if (Projectile.Player().whoAmI == Main.myPlayer)
                        Main.player[a].Heal(Main.player[a].statLifeMax2 / 20);

                    Projectile.Kill();
                }
            }
            Player player = Main.player[Player.FindClosest(Projectile.Center,1,1)];
            Projectile.rotation = Projectile.velocity.X * 0.03F;
            if((player.Center - Projectile.Center).Length()<300&& player.活着())
            {
                Projectile.velocity = (player.Center - Projectile.Center).PerfectNormalize() * 5;
            }
            DDHelper.BackAndForth(0.9f, 1.1f, 0.01F, ref Projectile.scale,ref Projectile.DProj().Bool[0]);
            Projectile.velocity *= 0.95F;
        }
        Color[] colors;
        public override void OnKill(int timeLeft)
        {
            SoundStyle sound = SoundID.Item4;
            sound.Pitch = 1.5F;
            PlaySound(sound, Projectile.position);
            if (Main.netMode != 2)
            {
                Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
                if (colors == null|| colors.Length==0)
                {
                    colors = DDHelper.GetColors(texture);
                }
                for (int i = 0; i < colors.Length; i++)
                {
                    float x = i % texture.Width;
                    float y = i / texture.Width;
                    Color color = colors[i];
                    color.A = 0;
                    if (colors[i] != new Color(0, 0, 0, 0))
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center + (new Vector2(x, y) - new Vector2(texture.Width/1.5F, texture.Height/1.5F)), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, color, 0.5f)];
                        dust.customData = 0.75F;
                        dust.noGravity = true;
                        dust.velocity = dust.position - Projectile.Center;
                        dust.velocity *= 0.1F;
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = new Color(255,255,255,120);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, lightColor, 0, TextureAssets.Projectile[Type].Size() / 2, Projectile.scale, 0, 0);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, lightColor, 0, TextureAssets.Projectile[Type].Size() / 2, Projectile.scale, 0, 0);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, lightColor, 0, TextureAssets.Projectile[Type].Size() / 2, Projectile.scale, 0, 0);
            return true;
        }
    }
}