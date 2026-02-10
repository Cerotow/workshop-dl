using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Boss
{
    public class 狱火小蛇 : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.timeLeft = 1000;
        }
        int[] Body = new int[5];
        Vector2[] Center = new Vector2[5];
        float[] Rotation = new float[5];
        float Time;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.GetAlpha(new Color(254, 62, 3, 0));
            Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height / 3);
            for (int A = 0; A < 3; A++)
            {
                for (int B = Body.Length - 1; B >= 0; B--)
                {
                    if (B == 0)
                    {
                        rectangle.Y = 0;
                        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rectangle, color, Rotation[0], rectangle.Size() / 2, Projectile.scale / 4, 0, 0);
                    }
                    else if (B < Body.Length - 4)
                    {
                        rectangle.Y = texture.Height / 3;
                        Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, rectangle, color, Rotation[B], rectangle.Size() / 2, Projectile.scale / 4, 0, 0);
                    }
                    else if (B < Body.Length - 3)
                    {
                        rectangle.Y = texture.Height / 3;
                        Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, rectangle, color, Rotation[B], rectangle.Size() / 2, Projectile.scale / 5, 0, 0);
                    }
                    else if (B < Body.Length - 2)
                    {
                        rectangle.Y = texture.Height / 3;
                        Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, rectangle, color, Rotation[B], rectangle.Size() / 2, Projectile.scale / 6, 0, 0);
                    }
                    else if (B < Body.Length - 1)
                    {
                        rectangle.Y = texture.Height / 3;
                        Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, rectangle, color, Rotation[B], rectangle.Size() / 2, Projectile.scale / 7, 0, 0);
                    }
                    else
                    {
                        rectangle.Y = texture.Height / 3 * 2;
                        Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, rectangle, color, Rotation[B], rectangle.Size() / 2, Projectile.scale / 8, 0, 0);
                    }
                }
            }
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public void Visual()
        {
            Center[0] = Projectile.Center;
            Rotation[0] = Projectile.rotation;
            for (int B = 0; B < Body.Length; B++)
            {
                if (B > 0)
                {
                    Vector2 vector = Center[B - 1] - Center[B];
                    Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;
                    float D = (vector.Length() - 10 * Projectile.scale) / vector.Length();
                    if (B > Body.Length - 4)
                    {
                        D = (vector.Length() - 6 * Projectile.scale) / vector.Length();
                    }
                    Center[B] += vector * D;
                }
            }
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.DProj().Times[1] == 0)
            {
                Projectile.DProj().Times[1] = Main.rand.Next(18, 28);
                Projectile.netUpdate = true;
            }
            int Length = (int)Projectile.DProj().Times[1];
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
        }

        public override void AI()
        {
            Visual();
            Projectile.rotation = Projectile.velocity.ToRotation() +MathHelper.PiOver2;
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity;
            }
            DDHelper.BackAndForth(-1f, 1f, 0.05F, ref Projectile.ai[0], ref Projectile.DProj().Bool[0]);
            Projectile.velocity = Projectile.DProj().vector[0].RotatedBy(Projectile.ai[0]);
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