using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 鬼牙仆从 : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

        }

        public override void SetDefault()
        {
            Main.projPet[Projectile.type] = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.minionSlots = 1;
            inertia = 10f;
            SearchRange = 1000;
            IgnoreTile = false;
            Minibuff = ModContent.BuffType<鬼牙仆从Buff>();
            Speed = 12;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.GetGlobalProjectile<SummonProjectile>().ReboundSpeed = 2;
            Projectile.scale = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        int[] Body = new int[5];
        int[] Alpha = new int[5];
        Vector2[] Center = new Vector2[5];
        float[] Rotation = new float[5];
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Summon/Minions/鬼牙仆从_Glow");
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2 = Glow.Value;
            for (int B = Body.Length - 1; B >= 0; B--)
            {
                lightColor = Color.White * (1 - Alpha[B] / 255F);
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture2, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture2.Width, texture2.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture2.Width / 2, texture2.Height / 6), Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture2, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture2.Height / 3, texture2.Width, texture2.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture2.Width / 2, texture2.Height / 6), Projectile.scale, 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture2, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture2.Height / 3 * 2, texture2.Width, texture2.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture2.Width / 2, texture2.Height / 6), Projectile.scale, 0, 0);
                }
            }
            for (int B = Body.Length - 1; B >= 0; B--)
            {
                lightColor = Lighting.GetColor((int)Center[B].X / 16, (int)Center[B].Y / 16) * (1 - Alpha[B] / 255F);
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else
                if (B == 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), Projectile.GetAlpha(lightColor), Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale, 0, 0);
                }
            }
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public override void Visual()
        {
            Center[0] = Projectile.Center+=Projectile.velocity;
            Rotation[0] = Projectile.rotation;
            for (int B = 0; B < Body.Length; B++)
            {
                if (B > 0)
                {
                    Vector2 vector = Center[B - 1] - Center[B];
                    Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;
                    float D = (vector.Length() - 22 * Projectile.scale) / vector.Length();
                    Center[B] += vector * D;
                }
                if (target)
                {
                    Vector2 vector = npc.Center - Center[B];
                    if (Alpha[B] > (int)(vector.Length()))
                    {
                        Alpha[B] -= 15;
                    }
                    else
                    {
                        Alpha[B] += 15;
                    }
                }
                else
                {
                    if (Alpha[B] > 200)
                    {
                        Alpha[B] -=15;
                    }
                    else if(Alpha[B] <= 185)
                    {
                        Alpha[B] += 15;
                    }
                    else
                    {
                        Alpha[B] = 200;
                    }
                }
                if (Alpha[B] > 255)
                {
                    Alpha[B] = 255;
                }
                if (Alpha[B] < 0)
                {
                    Alpha[B] = 0;
                }
            }
            int Length = 8;
            if (Body.Length != Length)
            {
                Body = new int[Length];
                Center = new Vector2[Length];
                Rotation = new float[Length];
                Alpha = new int[Length];
                for (int B = 0; B < Length; B++)
                {
                    Center[B] = Projectile.Center - new Vector2(0.1f);
                }
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool MobileAI()
        {
            if (Projectile.DProj().Times[0] > 0)
            {
                Projectile.DProj().Times[0]--;
            }
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = new Vector2(Main.rand.NextFloat(-200, 200), Main.rand.NextFloat(-200, 200));
            }
            if (target)
            {
                Vector2 vector = npc.Center - Projectile.Center;
                if (Projectile.velocity == Vector2.Zero)
                {
                    Projectile.velocity = Projectile.rotation.ToRotationVector2() * 3;
                }
                Projectile.netUpdate = true;
                if (vector.Length() > 200 || !DDHelper.SpecifyDirection(Projectile.rotation, vector.ToRotation() + MathHelper.PiOver2, 0.2f))
                {
                    if(HitNPC <= 0)
                    Projectile.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, 0.2f);
                }
                Projectile.velocity = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 18;
            }
            else
            {
                Vector2 direction = player.Center - Projectile.Center - Projectile.DProj().vector[0];
                direction.Y -= 120f;
               
                if (direction.Length() > 500)
                {
                    if (Projectile.ai[2]<25)
                    Projectile.ai[2] += 0.2F;
                    Projectile.velocity = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * Projectile.ai[2];
                }
                else
                {
                    if (Projectile.ai[2] > 3)
                        Projectile.ai[2] -= 0.2F;
                    else
                        Projectile.ai[2] += 0.2F;
                    Projectile.velocity = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * Projectile.ai[2];
                }
                if (player.velocity.Length() > 20 || direction.Length() > 200)
                {
                    float speed = Speed + player.velocity.Length();
                    Projectile.netUpdate = true;
                    if (DistancePlayer > 3000f)
                    {
                        Projectile.Center = player.Center;
                    }
                    direction = direction.PerfectNormalize();
                    direction *= speed;
                    float temp = inertia;
                    Projectile.RotationSpeed(direction.ToRotation() + MathHelper.PiOver2, 0.05F);
                    //Projectile.velocity = (Projectile.velocity.RotatedBy(I) * temp + direction) / (temp + 1);
                }
                if (Projectile.velocity == Vector2.Zero)
                {
                    Projectile.velocity = Projectile.rotation.ToRotationVector2() * 3;
                }
            }
            Projectile.alpha = 0;
            return false;
        }
        float I = 1;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            HitNPC = 10;
        }
        public override bool AttackAI()
        {
            return false;
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