using DDmod.Content.Projectiles.GeneralProj;
using Terraria.Graphics.Shaders;


namespace DDmod.Content.Projectiles.Melee.Sword
{
    public class 蘑菇短剑Proj : ModProjectile
    {
        public static Asset<Texture2D> texture;
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            Projectile.MeleeProj().DaggerDashDistance = 0;
        }
        public override void PostAI()
        {
            Player player = Main.player[Projectile.owner];
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
            return base.CanDamage();
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            if (Projectile.DProj().Bool[0])
            {
                for (int A = 0; A < 4; A++)
                {
                    Vector2 vector = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(5, 10);
                    Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center, vector, ModContent.ProjectileType<蘑菇>(), Projectile.damage, Projectile.knockBack, Projectile.owner)];
                    projectile.DamageType = DamageClass.Melee;
                }

            }
        }
        int A;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 Center;
            Center = Projectile.Center - Main.screenPosition;
            Texture2D Projtexture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(Projtexture, Center, new Rectangle?(new Rectangle(0, 0, Projtexture.Width, Projtexture.Height)), lightColor, Projectile.rotation - MathHelper.PiOver4, new Vector2(Projtexture.Width / 2, Projtexture.Height / 2), Projectile.scale, 0, 0f);

            return false;
        }
    }
}