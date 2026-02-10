using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 钛金飞刀Proj : 飞刀Proj
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
                NewDust(Projectile.Center, 1, 1, 146, projDirection.X, projDirection.Y,0,default,0.75f);
            }
            for (int i = 0; i <3; i++)
            {
                Vector2 vector = Main.rand.NextVector2Unit() * Main.rand.NextFloat(4, 6);
                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vector, ModContent.ProjectileType<钛金飞刀碎片>(), Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
    }
    public class 钛金飞刀碎片 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Main.projFrames[Type] = 4;
            Projectile.timeLeft = 1000;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.scale = 1f;
            Projectile.localAI[0] = Main.rand.Next(4);
        }
        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.03f;
            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.03f;
            }
            else
            {
                Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.03f;
            }
            if (Projectile.velocity.Y < 16)
            {
                Projectile.velocity.Y += 0.2F;
            }
            if (Projectile.timeLeft < 255)
            {
                Projectile.alpha++;
                if (Projectile.alpha > 255)
                {
                    Projectile.Kill();
                }
                Projectile.damage = 0;
            }
            else if (Projectile.velocity.Length() < 0.1F)
            {
                Projectile.timeLeft = 255;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.3F;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                if(oldVelocity.Y>0)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.3F;
                }
                else
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                Projectile.velocity.X *= 0.9F;
            }
            return false;
        }
        int Time;
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = 0;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Main.spriteBatch.Draw(texture, Projectile.oldPos[i]+Projectile.Size/2 - Main.screenPosition, new Rectangle?(new Rectangle(0, (int)(20 * Projectile.localAI[0]), 20, 20)), lightColor * (1 - (float)Projectile.alpha / 255) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), Projectile.rotation, new Vector2(texture.Width, texture.Height / 4) / 2, Projectile.scale * 1.1F, spriteEffects, 0f);
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, (int)(20 * Projectile.localAI[0]), 20, 20)), lightColor * (1 - (float)Projectile.alpha / 255), Projectile.rotation, new Vector2(texture.Width, texture.Height / 4) / 2, Projectile.scale, spriteEffects, 0f);


            return false;
        }
    }
}