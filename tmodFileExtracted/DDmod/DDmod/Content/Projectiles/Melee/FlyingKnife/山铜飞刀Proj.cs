using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 山铜飞刀Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            AIStyle = 飞刀AI.AI1;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
        }
        public override void PostAI()
        {
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 20; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                NewDust(Projectile.Center, 1, 1, 145, projDirection.X, projDirection.Y,0,default,0.75f);
            }

        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            NewProjectile(Projectile.GetSource_FromThis(), target.Center + vector, -vector / 10, ModContent.ProjectileType<花斩>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner, target.whoAmI, 1,1);
        }
    }
    public class 花斩 : ModProjectile
    {
        public override string Texture => "DDmod/Image/VoidStar";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 90;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 20;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 1.4f;
        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[1] != 0)
            {
                return target.whoAmI == (int)Projectile.ai[0];
            }
            return null;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.scale = Projectile.ai[2];
            Projectile.alpha++;
            Projectile.ProjScaleChange2();

            for (int A = -Projectile.height / 2; A < Projectile.height / 2; A +=10)
            {
                if (Main.rand.NextBool(100))
                {
                    NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), ModContent.ProjectileType<山铜花瓣>(), Projectile.damage, 0, Projectile.owner, 0, 0, Projectile.ai[2]/1.4F);
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, Projectile.alpha));
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            if(Projectile.alpha<=0)
            {
                return false;
            }
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = new Color(246, 111, 227, 0)*0.1F;
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int a = 0; a < 3; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 0.4f, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 16) * 1.5f, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, color * 4.4f, Projectile.rotation, texture.Size() / 2, new Vector2(Projectile.scale, Projectile.scale / 16), spriteEffects, 0f);
            }

            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float num2 = 0.75f;
            float LaserLength = MathHelper.Lerp(0, Projectile.height, num2);
            Vector2 Pvelocity = Utils.RotatedBy(Projectile.velocity.PerfectNormalize(), 0, default);
            float num = 0f;
            bool T = false;
            if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center + Pvelocity * LaserLength, projHitbox.Width, ref num))
            {
                T = true;
            }
            if (Collision.CheckAABBvLineCollision(Utils.TopLeft(targetHitbox), Utils.Size(targetHitbox), Projectile.Center, Projectile.Center - Pvelocity * LaserLength, projHitbox.Width, ref num))
            {
                T = true;
            }
            return new bool?(T);

        }
    }
    public class 山铜花瓣 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Main.projFrames[Type] = 3;
            Projectile.timeLeft = 1800;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.idStaticNPCHitCooldown = 30;
            Projectile.scale = 1f;
            Projectile.localAI[0] = Main.rand.Next(3);
        }
        public override void AI()
        {
            Projectile.scale = Projectile.ai[2];
            Projectile.ProjScaleChange2();
            Projectile.rotation += Projectile.velocity.X * 0.03f;
            int A = 5;
            if (Main.player[Projectile.owner].ZoneSandstorm)
            {
                A += 100;
            }
            if (Main.windSpeedCurrent * A > 0)
            {
                if (Projectile.velocity.X < Main.windSpeedCurrent * A)
                {
                    Projectile.velocity.X += 0.02F;
                }
            }
            if (Main.windSpeedCurrent * A < 0)
            {
                if (Projectile.velocity.X > Main.windSpeedCurrent * A)
                {
                    Projectile.velocity.X -= 0.02F;
                }
            }
            if (Projectile.velocity.Y < 3)
            {
                Projectile.velocity.Y += 0.02F;
            }
            if(Projectile.alpha>100)
            {
                Projectile.damage = 0;
            }
            Projectile.ai[0]++;
            if(Projectile.ai[0]>120)
            {
                Projectile.DProj().Bool[0] = true;
            }
            if (Projectile.DProj().Bool[0])
            {
                Projectile.alpha++;
                if(Projectile.alpha>255)
                {
                    Projectile.Kill();
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.DProj().Bool[0] = true;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = 0;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = 0;
                Projectile.velocity.X *= 0.92f;
            }
            return false;
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, (int)(20 * Projectile.localAI[0]), 20,20)), lightColor* (1-(float)Projectile.alpha/255), Projectile.rotation, new Vector2(texture.Width,texture.Height/3) / 2, Projectile.scale, spriteEffects, 0f);

            return false;
        }
    }
}