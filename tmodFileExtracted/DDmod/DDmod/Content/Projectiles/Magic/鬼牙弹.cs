using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic
{
    public class 鬼牙弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            if (Projectile.localAI[1] >= 1 && Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0]++;
            }
            int Type = ModContent.DustType<光球粒子>();
            if (Projectile.localAI[0] == 1)
            {
                Projectile.scale *= 4f;
                for (int a = 1; a < Projectile.scale / 2; a++)
                {
                    float R = Projectile.scale / 10 * Main.rand.NextFloat(0.5F, 1.5F);
                    Vector2 Position = new Vector2(Main.rand.NextFloat(Projectile.width / 2), 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
                    Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), Projectile.Center + Position, Position * Projectile.scale / 300, ModContent.ProjectileType<鬼牙>(), (int)(Projectile.damage * R / 4), Projectile.knockBack, Projectile.owner, 0, 1, Projectile.DProj().Magnification * R)];
                    projectile.scale = R/2;
                }
                for (int A = 0; A < (int)(Projectile.scale * 40); A++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.position, (int)Projectile.Size.X, (int)Projectile.Size.Y, Type, 0, 0, 0, new Color(222, 0, 25, 255))];
                    dust.noGravity = true;
                    dust.scale = 1.5F * (Projectile.scale / 10);
                    dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0, 1.5f * Projectile.scale);
                    dust.customData = 2 + dust.DustAI(1);
                }
                Projectile.timeLeft = 3;
                Projectile.localAI[0]++;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0)
            {
                for (int A = 0; A < 6; A++)
                {
                    Dust dust = Main.dust[NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(222, 0, 25, 255))];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale / 2;
                    dust.velocity *= 0.1f;
                    Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                    dust.velocity += -vector * Projectile.scale;
                    dust.customData = 1 + dust.DustAI(1);
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
                    Color color = Projectile.GetAlpha(new Color(180, 0, 27, 255)) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                    //Main.spriteBatch.Draw(DDTextures.VoidStar.Value, vector2, null, color * 0.6f, Projectile.rotation, ModContent.Request<Texture2D>("DDmod/Image/VoidStar").Size() / 2, Projectile.scale / 2 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                    Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale/4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
                }
                    Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(180, 0, 27, 255), Projectile.rotation, texture.Size() / 2, Projectile.scale/4, spriteEffects, 0f);
                
            }
            return false;
        }
    }
}