using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic
{
    public class ErosionLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.scale = 1F;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 3;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.scale = 1.5F;
        }
        int[] Body = new int[7];
        Vector2[] Center = new Vector2[7];
        float[] Rotation = new float[7];
        Vector2[] Velocity = new Vector2[7];
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(184, 44, 225, 0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(184, 44, 225, 0), (float)Math.Pow((double)completionRatio, 1.0)); ;
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 40f * Projectile.scale, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
        internal Trailing TrailDrawer;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = DDTextures.GlowEffect.Value;
            Color color = new Color(184, 44, 225, 0);
            /*if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(0.1f);
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            TrailDrawer.Draw(Center, -Main.screenPosition, 100, null);*/
            for (int B = (int)(Projectile.DProj().Times[1] - 1); B >= 0; B--)
            {
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[0], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length)/8 + 0.1f, 0, 0);
                    // Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[0], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length/2), 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[B], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length) / 8 + 0.1f, 0, 0);
                    //Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[B], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length/2), 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[B], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length / 2) / 8+0.1f, 0, 0);
                }
            }
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(70, 300);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft < 120)
            {
                Projectile.scale -= 0.01f;
                if (Projectile.scale <= 0.2f)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                //长度
                int Length = 20;
                if (Body.Length != Length)
                {
                    Body = new int[Length];
                    Center = new Vector2[Length];
                    Rotation = new float[Length];
                    Velocity = new Vector2[Length];
                    for (int B = 0; B < Length; B++)
                    {
                        Center[B] = Projectile.Center - new Vector2(0.1f);
                    }
                }
                Center[0] = Projectile.Center;
                Rotation[0] = Projectile.rotation;
                Velocity[0] = Projectile.velocity;
                for (int B = 0; B < Body.Length; B++)
                {
                    if (B > 0)
                    {
                        Vector2 vector = Center[B - 1] - Center[B];
                        Velocity[B] = Velocity[B - 1];
                        Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;

                        float Distance = (vector.Length() - (15 * Projectile.scale)) / vector.Length();
                        Center[B] += vector * Distance + Velocity[B - 1]*2;
                    }
                    Center[B] += player.Dplayer().PrePosition / (Projectile.extraUpdates+1);
                }
                Projectile.alpha = 0;

                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                Projectile.Center = Main.projectile[(int)Projectile.ai[0]].Center + Main.projectile[(int)Projectile.ai[0]].velocity.PerfectNormalize() * 20;
                DDHelper.BackAndForth(-1.6f, 1.6f, 0.02f * player.GetTotalAttackSpeed(Projectile.DamageType), ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0]);

                Projectile.velocity = Main.projectile[(int)Projectile.ai[0]].velocity.RotatedBy(Projectile.DProj().Times[0]);
                if (Main.projectile[(int)Projectile.ai[0]].active)
                {
                    Projectile.timeLeft = 122;
                }
                if (Projectile.DProj().Times[1] < Body.Length)
                {
                    Projectile.DProj().Times[1] += 0.1f;
                }
                DDHelper.MaxandMinF(ref Projectile.DProj().Times[1], Body.Length, 0);
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle[] vectors = new Rectangle[Body.Length];
            bool B = false;
            for (int A = 0; A < Projectile.DProj().Times[1]; A++)
            {
                Vector2 vector = Projectile.Size * ((Body.Length + A) / (float)Body.Length);
                Vector2 Size = vector / 2;
                vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), (int)vector.X, (int)vector.Y);
                if (vectors[A].Intersects(targetHitbox))
                {
                    B = true;
                }
            }
            return new bool?(B);
        }
    }
}