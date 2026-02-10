namespace DDmod.Content.Projectiles.Magic
{
    public class ShadowBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override string Texture => "DDmod/Image/VoidStar";
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }
        public override void AI()
        {
            for (int A = 0; A < 2; A++)
            {
                int Type = 27;
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1;
                dust.velocity = Vector2.Zero;
                dust.GetAlpha(new Color(0, 0, 0, 0));
                dust.rotation = Projectile.rotation;
            }
            if(Projectile.ai[0]!=0)
            {
                Projectile.tileCollide = Projectile.velocity.Y > 0;
            }
            Projectile.ProjScaleChange();
            if (Projectile.velocity.Y < 12)
            {
                Projectile.velocity.Y += 0.1F;
            }
        }
        public override void OnKill(int timeLeft)
        {
            int Type = 27;
            for (int A = 0; A < 30; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3.5f, 7.5f);
                dust.rotation = Projectile.rotation;
            }
            if(Projectile.scale>=0.4F&&Projectile.owner == Main.myPlayer)
            {
                for(int A = 0;A<4;A++)
                {
                    int proj = NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.One.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 5, Projectile.type, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                    Main.projectile[proj].scale = 0.2f;
                    Main.projectile[proj].timeLeft=120;
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.timeLeft -= 60;
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
                }
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = Projectile.DProj().Bool[0];
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            int Type = 27;
            for (int A = 0; A < 10; A++)
            {
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                dust.noGravity = true;
                dust.scale = 1;
                dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f, 2.5f);
                dust.rotation = Projectile.rotation;
            }
            target.AddBuff(BuffID.ShadowFlame, 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = (SpriteEffects)1;
            if (Projectile.direction == 1)
            {
                spriteEffects = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(81, 6, 233, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + vector - Main.screenPosition, null, new Color(81, 6, 233, 0), Projectile.rotation, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), spriteEffects, 0f);
            }

                return false;
        }
    }
}