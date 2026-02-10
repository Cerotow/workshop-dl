using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Magic.Staff;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic
{
    public class ShadowTentacles : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 100;
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
                new Color(81, 6, 233, 0),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(81, 6, 233, 0), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(1f, 0f, completionRatio, false);
            return MathHelper.Lerp(0, 40f, widthRatio) * MathHelper.Clamp(1f - (float)Math.Pow((double)completionRatio, 0.4), 1f, 0.5f);
        }
        internal static Trailing TrailDrawer;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D texture = DDTextures.VoidStar.Value;
            Color color = new Color(81, 6, 233, 255);
            if (TrailDrawer == null)
            {
                TrailDrawer = new Trailing(new Trailing.VertexWidthFunction(WidthFunction), new Trailing.VertexColorFunction(ColorFunction), null, GameShaders.Misc["贴图拖尾"]);
            }
            GameShaders.Misc["贴图拖尾"].SetShaderTexture(DDTextures.GlowTrail);
            GameShaders.Misc["贴图拖尾"].Shader.Parameters["uSpeed"].SetValue(0.1f);
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Vector2[] v = new Vector2[Center.Length];
            for (int B = 0; B < Projectile.DProj().Times[1]; B++)
            {
                if (Center[B] != Vector2.Zero)
                {
                    v[B] = Center[B] - Projectile.Center;
                    v[B] = Projectile.Center + v[B].RotatedBy(Projectile.localAI[0]);
                }
            }
            TrailDrawer.Draw(v, -Main.screenPosition, 100, null, Projectile.scale, Projectile.scale);
            if (Projectile.ai[2] == 0)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color) * Projectile.scale, Rotation[0], texture.Size() / 2, Projectile.scale * 0.75f, 0, 0);
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(new Color(81, 6, 233, 55)) * Projectile.scale, Rotation[0], texture.Size() / 2, Projectile.scale * 0.75f, 0, 0);
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(new Color(81, 6, 233, 55)) * Projectile.scale, Rotation[0], texture.Size() / 2, Projectile.scale * 0.75f, 0, 0);

            }
            /*
            for (int a = 0; a < 6 / ((player.ownedProjectileCounts[Type] == 0) ? 1 : player.ownedProjectileCounts[Type]); a++)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[0], texture.Size() / 2, Projectile.scale * 1.5F, 0, 0);
            }for (int B = (int)(Projectile.DPoroj().Times[1] - 1); B >= 0; B--)
            {
                if (B == 0)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[0], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length), 0, 0);
                    // Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[0], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length/2), 0, 0);
                }
                else if (B < Body.Length - 1)
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[B], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length), 0, 0);
                    //Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[B], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length/2), 0, 0);
                }
                else
                {
                    Main.EntitySpriteDraw(texture, Center[B] - Main.screenPosition, null, Projectile.GetAlpha(color), Rotation[B], texture.Size() / 2, Projectile.scale * ((Body.Length + B) / (float)Body.Length / 2), 0, 0);
                }
            }*/
            for (int B = 0; B < Projectile.DProj().Times[1]; B++)
            {
                if (!Main.gamePaused && !Projectile.DProj().Bool[4])
                {
                    Center[B] += player.Dplayer().PrePosition;
                }
            }

            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 300);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.alpha = 0;
            if (!player.controlUseItem || Projectile.DProj().Bool[4])
            {
                Projectile.DProj().Bool[4] = true;
                Projectile.position = Projectile.oldPosition;
                //Projectile.velocity *= 0.001f;
                Projectile.scale -= 0.03f;
                Projectile.DProj().Times[1] -= 0.03F;
                if (Projectile.scale <= 0.2f)
                {
                    Projectile.Kill();
                }
                return;
            }
            Projectile.localNPCHitCooldown = (int)player.IteUseAnimation2();
            int projId = -1;
            for (int a = 0; a < 1000; a++)
            {
                Projectile projectile = Main.projectile[a];
                if (projectile.active && projectile.owner == Projectile.owner && projectile.type == Projectile.ai[0])
                {
                    projId = projectile.whoAmI;
                    break;
                }
            }
            if (projId == -1)
            {
                return;
            }
            Projectile.localAI[0] = Main.projectile[(int)projId].velocity.ToRotation();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.Center = Main.projectile[(int)projId].Center + Main.projectile[(int)projId].velocity.PerfectNormalize() * 40;
            DDHelper.BackAndForth(-2f, 2f, (0.075F * (1 - Projectile.ai[1] / 40)) * player.GetTotalAttackSpeed(Projectile.DamageType), ref Projectile.DProj().Times[0], ref Projectile.DProj().Bool[0]); ;

            Projectile.velocity = new Vector2(1, 0).RotatedBy(Projectile.DProj().Times[0]);
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
                int Length = (int)Projectile.ai[1];
                if (Body.Length != Length)
                {
                    Body = new int[Length];
                    Center = new Vector2[Length];
                    Rotation = new float[Length];
                    Velocity = new Vector2[Length];
                    for (int B = 0; B < Length; B++)
                    {
                        //Center[B] = Projectile.Center - new Vector2(0.1f);
                    }
                }
                Center[0] = Projectile.Center;
                Rotation[0] = Projectile.rotation;
                Velocity[0] = Projectile.velocity;
                for (int B = 0; B < Body.Length; B++)
                {
                    if (B > 0 && B < Projectile.DProj().Times[1])
                    {
                        Vector2 vector = Center[B - 1] - Center[B];
                        Velocity[B] = Velocity[B - 1];
                        Rotation[B] = (float)Math.Atan2(vector.Y, vector.X) + 1.57f;
                        float Distance = (vector.Length() - (20 * Projectile.scale)) / vector.Length();
                        Center[B] += vector * Distance + Velocity[B - 1].PerfectNormalize() * Length/4 + Projectile.velocity.PerfectNormalize() * player.velocity.Length() / 5;
                    }
                }
                if (Main.projectile[(int)projId].active)
                {
                    Projectile.timeLeft = 122;
                }
                if (Projectile.DProj().Times[1] < Body.Length)
                {
                    Projectile.DProj().Times[1] += 0.1f;
                }
                DDHelper.MaxandMinF(ref Projectile.DProj().Times[1], Body.Length, 0);
            }
            Projectile.netUpdate = true;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle[] vectors = new Rectangle[Body.Length];
            bool B = false;
            Vector2[] v = new Vector2[Center.Length];
            for (int A = 0; A < Projectile.DProj().Times[1] - 3; A++)
            {
                v[A] = Center[A] - Projectile.Center;
                v[A] = Projectile.Center + v[A].RotatedBy(Projectile.localAI[0]);

                Vector2 vector = Projectile.Size * (1.3f - (A) / (float)Body.Length);
                Vector2 Size = vector / 2;
                vectors[A] = new Rectangle((int)(v[A].X - Size.X), (int)(v[A].Y - Size.Y), (int)vector.X, (int)vector.Y);
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