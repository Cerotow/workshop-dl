using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Projectiles.Magic
{
    public class 魔法狱火小蛇 : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.scale = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = 900;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 15;
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
                float AL = Projectile.DProj().Times[0] - B;
                if (AL > 1)
                {
                    AL = 1;
                }
                Color color = Projectile.GetAlpha(new Color(254, 62, 3, 0) * AL);
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), color, Rotation[0], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 4, 0, 0);
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), color, Rotation[0], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 4, 0, 0);
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), color, Rotation[0], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 4, 0, 0);
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), color, Rotation[0], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 4, 0, 0);
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height / 3)), color, Rotation[0], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 4, 0, 0);
                }
                else if (B < Body.Length - 4)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 4, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 4, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 4, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 4, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 4, 0, 0);
                }
                else if (B < Body.Length - 3)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 5, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 5, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 5, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 5, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 5, 0, 0);
                }
                else if (B < Body.Length - 2)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 6, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 6, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 6, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 6, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 6, 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 7, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 7, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 7, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 7, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 7, 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 8, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 8, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 8, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 8, 0, 0);
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / 3 * 2, texture.Width, texture.Height / 3)), color, Rotation[B], new Vector2(texture.Width / 2, texture.Height / 6), Projectile.scale / 8, 0, 0);
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
                    float D = (vector.Length() - 14 * Projectile.scale) / vector.Length();
                    if (B == 1)
                    {
                        D = (vector.Length() - (14-Projectile.velocity.Length()) * Projectile.scale) / vector.Length();
                    }
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
            Projectile.DProj().Times[0] += 0.4f;
            Visual();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.DProj().vector[0] == Vector2.Zero)
            {
                Projectile.DProj().vector[0] = Projectile.velocity;
            }
            DDHelper.BackAndForth(-1.2f, 1.2f, 0.1F, ref Projectile.ai[0], ref Projectile.DProj().Bool[0]);

            NPC npc = Projectile.FindTargetWithinRange(600, false);
            if (npc != null && npc.active && Projectile.GetGlobalProjectile<DDGlobalProjectile>().track > 15)
            {
                if (!Projectile.hostile && Projectile.friendly)
                {
                    Vector2 vector = (npc.Center - Projectile.Center);
                    DDHelper.RotateSpeed(ref Projectile.DProj().Times[2], vector.ToRotation() + MathHelper.PiOver2, 0.2F);
                    if (vector.Length() > 100)
                    {
                        Projectile.velocity = (Projectile.DProj().Times[2] - MathHelper.PiOver2).ToRotationVector2().RotatedBy(Projectile.ai[0]) * 8;
                    }
                    else
                    {
                        Projectile.ai[0] = 0;
                        Projectile.velocity = (Projectile.DProj().Times[2] - MathHelper.PiOver2).ToRotationVector2() * 8;
                    }
                }
                Projectile.DProj().vector[0] = Projectile.velocity;
            }
            else
            {
                Projectile.velocity = Projectile.DProj().vector[0].RotatedBy(Projectile.ai[0]);
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<地狱之火>(), 300);
        }
        public override void OnKill(int timeLeft)
        {
            for (int A = 0; A < Body.Length; A++)
            {
                DDust.NewDustChange(10, Center[A], Projectile.Size, 6, 0, 4, Scale: 2);
                SoundStyle sound = SoundID.Item14;
                sound.Pitch = -1F;
                PlaySound(sound, Center[A]);
            }
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