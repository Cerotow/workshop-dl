using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Summon.Minions
{
    public class 机械蠕虫计时器 : Summons
    {
        public override string Texture => "DDmod/Image/Nullpng";
        public override void SetDefault()
        {
            Defaults(ModContent.BuffType<机械蠕虫Buff>(), 12, 21, 800, 0, 10, 7, 150);
            Projectile.width = 22;
            Projectile.height = 36;
            Projectile.minionSlots = 1;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            /// <summary> 感觉有点针对召唤师了 </summary> ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }
        public override bool MinionContactDamage()
        {
            return false;
        }
        public override void Visual()
        {
        }
        public override bool MobileAI()
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
        public override bool AttackAI()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
    public class 机械蠕虫 : Summons
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

        }
        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
        }
        public override void SetDefault()
        {
            Projectile.width = Projectile.height= 24;
            Projectile.minionSlots = 0.5f;
            Defaults(ModContent.BuffType<机械蠕虫Buff>(), 12, 21, 1500, 0, 0, 0, 0);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;
            Projectile.minionSlots = 0;
            Projectile.extraUpdates = 0;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
        }
        int[] Body = new int[5];
        Vector2[] Center = new Vector2[5];
        float[] Rotation = new float[5];
        float Time;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D texture2 = Glow.Value;
            Rectangle rectangle = new Rectangle((int)(texture.Width/3 * Projectile.DProj().Times[1]), 0, texture.Width/3, texture.Height / 3);
            for (int B = Body.Length - 1; B >= 0; B--)
            {
                lightColor = Lighting.GetColor((int)Center[B].X/16, (int)Center[B].Y/16, Color.White);
                if (B == 0)
                {
                    rectangle.Y = 0;
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rectangle, Projectile.GetAlpha(lightColor), Rotation[0], rectangle.Size() / 2, Projectile.scale, 0, 0);
                    Main.EntitySpriteDraw(texture2, Projectile.Center - Main.screenPosition, rectangle, Projectile.GetAlpha(Color.White), Rotation[0], rectangle.Size() / 2, Projectile.scale, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    rectangle.Y = texture.Height / 3;
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, rectangle, Projectile.GetAlpha(lightColor), Rotation[B], rectangle.Size()/2, Projectile.scale, 0, 0);
                    Main.EntitySpriteDraw(texture2, Center[B] - Main.screenPosition, rectangle, Projectile.GetAlpha(Color.White), Rotation[B], rectangle.Size()/2, Projectile.scale, 0, 0);
                }
                else
                {
                    rectangle.Y = texture.Height / 3*2;
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, rectangle, Projectile.GetAlpha(lightColor), Rotation[B], rectangle.Size() / 2, Projectile.scale, 0, 0);
                    Main.EntitySpriteDraw(texture2, Center[B] - Main.screenPosition, rectangle, Projectile.GetAlpha(Color.White), Rotation[B], rectangle.Size() / 2, Projectile.scale, 0, 0);
                }
            }

            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public override bool CheckActive()
        {
            if (Minibuff != 0)
            {
                if (player.dead || !player.active)
                {
                    player.ClearBuff(Minibuff);

                    return false;
                }

                if (player.HasBuff(Minibuff))
                {
                    Projectile.timeLeft = 2;
                }
            }
            if (Projectile.DProj().track > 10 && Projectile.ai[2] == 0)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[2] < player.ownedProjectileCounts[ModContent.ProjectileType<机械蠕虫计时器>()])
            {
                Projectile.ai[2] = player.ownedProjectileCounts[ModContent.ProjectileType<机械蠕虫计时器>()];
            }
            else
            {
                Projectile.ai[2] = player.ownedProjectileCounts[ModContent.ProjectileType<机械蠕虫计时器>()];
            }
            if (Projectile.DProj().Times[1] != (int)((Projectile.ai[2]-1) / 3) && Projectile.DProj().Times[1]<2)
            {
                Projectile.DProj().Bool[0] = false;
            }
            Projectile.DProj().Times[1] = (int)((Projectile.ai[2] - 1) / 3);
            if (Projectile.DProj().Times[1] > 2)
            {
                Projectile.DProj().Times[1] = 2;
            }
            Projectile.width = Projectile.height = 24;
            if (Projectile.DProj().Times[1] == 1)
            {
                Projectile.width = Projectile.height = 30;
            }
            if (Projectile.DProj().Times[1] == 2)
            {
                Projectile.width = Projectile.height = 44;
            }
            return true;
        }
        public override void Visual()
        {
            Projectile.position += Projectile.velocity;
            Center[0] = Projectile.Center;
            Rotation[0] = Projectile.rotation;
            Projectile.DProj().Magnification = (1 + (Projectile.ai[2] - 1) *0.8F);
            Projectile.damage = (int)(Projectile.damage * Projectile.DProj().Magnification);
            bool rr = false;
            if (Projectile.DProj().Times[1] == 2)
            {
                if (Projectile.DProj().Times[0] > 0)
                {
                    Projectile.DProj().Times[0]--;
                }
                else
                {

                    if (target)
                    {
                        rr = true;
                        Projectile.DProj().Times[0] = 240;
                    }
                }
            }
            for (int B = 0; B < Body.Length; B++)
            {
                if (rr)
                {
                    Body[B] = Main.rand.Next(10, 150);
                }
                    Body[B]--;
                if (Projectile.DProj().Times[1] == 2 && target)
                {
                    if (Body[B] == 0)
                    {
                        Vector2 vector = npc.Center - Center[B];
                        Projectile.NewProjectileChange(Center[B], (npc.Center + (npc.position - npc.oldPosition) * (vector.Length() / 150) - Center[B]).PerfectNormalize() * Main.rand.NextFloat(18, 22), 389, Projectile.damage, 0, -1, 0, 0, 0, Projectile.DProj().Magnification);
                    }
                }
                if (B > 0)
                {
                    Vector2 vector = Center[B - 1] - Center[B];
                    Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + MathHelper.PiOver2;
                    float D = (vector.Length() - Projectile.height*0.75f * Projectile.scale) / vector.Length();
                    Center[B] += vector * D;
                }
                if (!Projectile.DProj().Bool[0])
                {
                    for (float A = 0; A < 22; A++)
                    {
                        Dust dust = Main.dust[NewDust(Center[B] - new Vector2(4), 1, 1, ModContent.DustType<光球粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y,100,new Color(200,50,50,0))];
                        dust.noGravity = true;
                        dust.scale = Projectile.scale*1.25F;
                        dust.customData = -1;
                        dust.velocity = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(1, 4);
                    }
                }
            }
            Projectile.DProj().Bool[0] = true;
            int Length = (int)Projectile.ai[2]+4;
            if (Body.Length != Length)
            {
                Vector2[] vectors = Center;
                float[] floats = Rotation;
                Body = new int[Length];
                Center = new Vector2[Length];
                Rotation = new float[Length];
                for (int B = 0; B < Length; B++)
                {
                    if (B > 0&& B < vectors.Length)
                    {
                        Center[B] = vectors[B];
                        Rotation[B] = floats[B];

                    }
                    else
                    {

                        Center[B] = Projectile.Center - new Vector2(0.1f);
                    }
                }
            }
        }

        public override bool MobileAI()
        {
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
                float Speed = 15+(5* Projectile.DProj().Times[1]);
                Projectile.netUpdate = true;
                if (Projectile.DProj().Times[2] < Speed)
                {
                    Projectile.DProj().Times[2] += Speed / 20;
                }
                Projectile.DProj().Times[3]--;
                
                if (Projectile.DProj().Times[3] <= 0)
                {
                    if (Projectile.DProj().Times[3] < -60)
                    {
                        Projectile.DProj().Times[3] = -60;
                    }
                    Projectile.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, -Projectile.DProj().Times[3] / 120 + 0.01F);
                }
                if (Projectile.DProj().Times[2] > 0)
                {
                    Projectile.velocity = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * Projectile.DProj().Times[2];
                }
                else
                {
                    Projectile.velocity *= 0.5f;
                }
            }
            else
            {
                Vector2 direction = player.Center - Projectile.Center - Projectile.DProj().vector[0];
                direction.Y -= 120f;
                Projectile.velocity = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * (12 - Projectile.DProj().Times[1] + direction.Length() / 128);
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
                    Projectile.RotationSpeed(direction.ToRotation() + MathHelper.PiOver2, 0.1F);
                }
                if (Projectile.velocity == Vector2.Zero)
                {
                    Projectile.velocity = Projectile.rotation.ToRotationVector2() * 3;
                }
            }
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        float I = 1;
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Projectile.DProj().Times[3] =1;
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