using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Magic
{
    public class 鬼牙 : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            Projectile.width = 66;
            Projectile.height = 66;
            Projectile.scale = 1.3f;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 200;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
        }
        int[] Body = new int[5];
        Vector2[] Center = new Vector2[5];
        float[] Rotation = new float[5];
        float Time;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Color color = Projectile.GetAlpha(Color.White);
            for (int B = Body.Length - 1; B >= 0; B--)
            {
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), color, Rotation[B]+MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), color, Rotation[B] + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
            }
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public void Visual()
        {
            Center[0] = Projectile.Center += Projectile.velocity;
            Rotation[0] = Projectile.rotation;
            for (int B = 0; B < Body.Length; B++)
            {
                if (B > 0)
                {
                    Vector2 vector = Center[B - 1] - Center[B];
                    Rotation[B] = vector.ToRotation() + MathHelper.PiOver2;
                    Center[B] = Center[B - 1] - vector.PerfectNormalize() * (42 * Projectile.scale);
                }
            }
            int Length = 3+(int)(10* Projectile.scale);
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
            if (Projectile.timeLeft > 51)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 5;
                }
            }
            else
            {
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += 5;
                }
            }
            Projectile.ProjScaleChange();
            NPC npc = NPCdirection.FindClosest(Projectile.Center, 500, true);
            if(Projectile.GetGlobalProjectile<DDGlobalProjectile>().track <= 30)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            if (npc != null && Projectile.GetGlobalProjectile<DDGlobalProjectile>().track > 30)
            {
                DDHelper.RotateSpeed(ref Projectile.rotation, (npc.Center - Projectile.Center).ToRotation(), 0.01F * Projectile.ai[1]);
            }
            if (Projectile.ai[1] < 12*Projectile.scale)
            {
                Projectile.ai[1] += 0.1F;
            }
            Projectile.velocity = Projectile.rotation.ToRotationVector2().PerfectNormalize() * Projectile.ai[1];
            Visual();
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
                    break;
                }
            }
            return new bool?(B);
        }
    }
}