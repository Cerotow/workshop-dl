using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic
{
    public class CursetheWallFire : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 100;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1F;
            Projectile.timeLeft = 1200*10;
            Projectile.extraUpdates = 10;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.scale = 1.5F;
        }
        //长度
        int[] Body = new int[30];
        Vector2[] Center = new Vector2[30];
        float[] Rotation = new float[30];
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(39, 300);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Center[0] == Vector2.Zero)
            {
                Center[0] = Projectile.Center;
            }
            if(!player.controlUseTile)
            {
                Projectile.DProj().Bool[0] = true;
            }
            if (player.ownedProjectileCounts[Projectile.type]>1&&Projectile.DProj().Bool[0])
            {
                Projectile.Kill();
            }
            for (int B = 0; B < Body.Length; B++)
            {
                if (B > 0 && Center[B] == Vector2.Zero && Center[B - 1] != Vector2.Zero&&!Projectile.DProj().Bool[0])
                {
                    Vector2 vector = Center[B - 1] - Projectile.Center;
                    if (vector.Length() > 10)
                    {
                        Center[B] = Projectile.Center;
                        Rotation[B] = Projectile.rotation;
                    }
                }
            }
            Projectile.alpha = 0;
            if (Center[Center.Length - 1] == Vector2.Zero)
            {
                float SP = ((player.Dplayer().MouseWorld - Projectile.Center).Length() / 10 + 1);
                if (SP > 5) SP = 5;
                Projectile.velocity = (player.Dplayer().MouseWorld - Projectile.Center).PerfectNormalize() * SP;
                Projectile.rotation =  Projectile.velocity.ToRotation();
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
            }
            for (int A = 0; A < Body.Length; A++)
            {
                if (Center[A] != Vector2.Zero)
                {
                    if (Main.netMode != 2 &&Main.rand.NextBool(100))
                    {
                        int Type = 75;
                        int dust = NewDust(Center[A] - new Vector2(5) + Rotation[A].ToRotationVector2()*Main.rand.NextFloat(0,20), 10, 10, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale = Main.rand.NextFloat(1F, 1.2F);
                        Main.dust[dust].velocity.X = 0;
                        Main.dust[dust].velocity.Y = -Main.rand.NextFloat(0.4F, 0.7F);
                        int dust2 = NewDust(Center[A] - new Vector2(5) + Rotation[A].ToRotationVector2() * Main.rand.NextFloat(0, 20), 10, 10, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default);
                        Main.dust[dust2].noGravity = false;
                        Main.dust[dust2].scale = Main.rand.NextFloat(1F, 1.2F);
                        Main.dust[dust2].velocity.X = 0;
                        Main.dust[dust2].velocity.Y = 0;

                    }
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle[] vectors = new Rectangle[Body.Length];
            bool B = false;
            for (int A = 0; A < vectors.Length; A++)
            {
                Vector2 vector = Projectile.Size * ((Body.Length + A) / (float)Body.Length);
                Vector2 Size = vector / 2;
                vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), (int)vector.X, (int)vector.Y);
                if (vectors[A].Intersects(targetHitbox)&& Center[A]!=Vector2.Zero)
                {
                    B = true;
                }
            }
            return new bool?(B);
        }
    }
}