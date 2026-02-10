using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Magic
{
    public class 水晶刺 : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 1f;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 80;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.ArmorPenetration = 5;
        }
        int[] Body = new int[5];
        Vector2[] Center = new Vector2[5];
        float[] Rotation = new float[5];
        float Time;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            for (int B = Body.Length - 1; B >= 0; B--)
            {
                float AL = Projectile.ai[0] / 4;
                if (AL >= B)
                {
                    AL = 1;
                }
                else
                {
                    if (B- AL <= 1)
                    {
                        AL %= 1;
                    }
                    else
                    {
                        AL = 0;
                    }
                }
                Color color = Color.White*AL* Projectile.ai[2];
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
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
            if (Projectile.localAI[0] > 1)
            {
                return false;
            }

                return null;
        }
        public void Visual()
        {
            Center[0] = Projectile.Center;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            for (int B = 0; B < Body.Length; B++)
            {
                if (B > 0)
                {
                    Center[B] = Center[B - 1] + Projectile.velocity.PerfectNormalize() * 28;
                }
            }
            if(Projectile.ai[2]<1)
            {
                Projectile.ai[2] += 0.5F;
            }
            if (Projectile.ai[0] < Projectile.ai[1] * 4)
            {
                Projectile.ai[0]+=2;
            }
            int Length = (int)Projectile.ai[1];
            if (Body.Length != Length)
            {
                Body = new int[Length];
                Center = new Vector2[Length];
                Rotation = new float[Length];
            }
        }
        public override void AI()
        {
            Projectile.ProjScaleChange();
            if (Projectile.timeLeft==1)
            {

                for (int B = 0; B < Body.Length; B++)
                {
                    for (int A = 0; A < 30; A++)
                    {
                        int Type = Main.rand.Next(68, 71);
                        Dust dust = Main.dust[NewDust(Center[B]-Projectile.Size/2, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default)];
                        dust.noGravity = true;
                        dust.scale *= 1.25f;
                        dust.velocity *= 0.1f;
                        Vector2 vector = (dust.position - Projectile.Center).PerfectNormalize();
                        dust.velocity += -vector * Projectile.scale;
                    }
                }
            }
            if (Projectile.localAI[0]<=0)
            {
                Projectile.localAI[0] = Main.rand.NextFloat(0, 30);
                Projectile.netUpdate = true;
            }
            if(Projectile.localAI[0]>1)
            {
                Projectile.localAI[0]--;
            }
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
                float AL = Projectile.ai[0] / 4;

                if (AL < A && A - AL > 1)
                {
                    B = false;
                    break;
                }
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