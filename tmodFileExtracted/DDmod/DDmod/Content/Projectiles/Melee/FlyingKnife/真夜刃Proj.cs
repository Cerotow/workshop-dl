using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 真夜刃Proj : 飞刀Proj
    {

        public static Asset<Texture2D> Glow;
        public override void Load()
        {
            Glow = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override void Defaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            AIStyle = 飞刀AI.AI3;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            Projectile.alpha = 255;
        }
        public override bool PreAI()
        {
            int A = NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 27,0,0,0,default,1.4f);
            Main.dust[A].noGravity = true;
            A = NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 75,0,0,0,default,2.4f);
            Main.dust[A].noGravity = true;
            if (Projectile.ai[0] == 1)
            {
                Projectile.alpha -= 30;
                if(Projectile.alpha<0)
                {
                    Projectile.alpha = 0;
                }
                Projectile.tileCollide = false;
            }
            else
            {
                Projectile.alpha = 0;
            }
            Player player = Main.player[Projectile.owner];
            if (player.Aplayer().TrueNightEnergy)
            {
                Projectile.extraUpdates = 1;
            }
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0)
            {
                if (player.Aplayer().TrueNightEnergy)
                {
                    for (int A = -2; A <= 2; A++)
                    {
                        int[] T = new int[]{Type,ModContent.ProjectileType<夜刃Proj>()};
                        int Ty = Main.rand.Next(T);
                        Vector2 vector = Projectile.Center - Projectile.velocity.RotatedBy(A*0.7f - Main.rand.NextFloat(-0.3f, 0.3f)).PerfectNormalize() * 240;
                        NewProjectile(Projectile.GetSource_FromAI(), vector, (Projectile.Center - vector).PerfectNormalize() * 16, Ty, Projectile.damage / 2, 0, Projectile.owner, 1);

                        if (Ty == Type)
                        {
                            NewDustChange(50, vector - new Vector2(4), Vector2.Zero, 75, 0, 6, true, 2f);
                        }
                        else
                        {
                            NewDustChange(50, vector - new Vector2(4), Vector2.Zero, 27, 0, 6, true, 2f);
                        }
                    }
                }
                else
                {
                    for (int A = -1; A <= 1; A++)
                    {
                        int[] T = new int[] { Type, ModContent.ProjectileType<夜刃Proj>() };
                        int Ty = Main.rand.Next(T);
                        Vector2 vector = Projectile.Center - Projectile.velocity.RotatedBy(A - Main.rand.NextFloat(-0.3f, 0.3f)).PerfectNormalize() * 240;
                        NewProjectile(Projectile.GetSource_FromAI(), vector, (Projectile.Center - vector).PerfectNormalize() * 16, Ty, Projectile.damage / 2, 0, Projectile.owner, 1);

                        if (Ty == Type)
                        {
                            NewDustChange(50, vector - new Vector2(4), Vector2.Zero, 75, 0, 6, true, 2f);
                        }
                        else
                        {
                            NewDustChange(50, vector - new Vector2(4), Vector2.Zero, 27, 0, 6, true, 2f);
                        }
                    }
                }
            }
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueNightsEdge, settings, Projectile.owner);
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 25; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                int A = NewDust(Projectile.position, 1, 1, 27, projDirection.X, projDirection.Y, 0, default, 1.8f);
                Main.dust[A].noGravity = true;
            }
            for (int i = 0; i < 25; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2)) * (Main.rand.NextFloat(12.8f, 13f) * (Projectile.velocity.Length() / 10));
                int A = NewDust(Projectile.position, 1, 1, 75, projDirection.X, projDirection.Y, 0, default, 1.8f);
                Main.dust[A].noGravity = true;
            }

        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D glow = Glow.Value;
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, Projectile.height / 2);
            Vector2 Origia2 = new Vector2(Glow.Width() * 0.5f, 20 * 4);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height - Projectile.height / 2);
                Origia2 = new Vector2(Glow.Width() * 0.5f, Glow.Height() - 20 * 4);
            }

            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float A = Projectile.oldRot[i];
                if (Projectile.velocity.X < 0)
                {
                    A -= Rotation2;
                }
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size/2 - Main.screenPosition;
                Color color = new Color(36, 208, 2, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 1.2f;
                if(i%4<2&& Projectile.ai[0] == 0)
                {
                    color = new Color(81, 6, 233, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 1.2f;
                }
                Main.spriteBatch.Draw(glow, vector2, null, Projectile.GetAlpha(color), A, Origia2, Projectile.scale/4, sprite, 0f);
            }
            if (Projectile.ai[0] == 0)
            {
                Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0), RO, Origia2, Projectile.scale/4, sprite, 0f);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, RO, Origia, Projectile.scale, sprite, 0f);
            }
            return false;
        }
        internal Trailing TrailDrawer;
        internal Color ColorFunction(float completionRatio)
        {
            float colorFade = 1f - Utils.GetLerpValue(0f, 1f, completionRatio, true);
            return Color.Lerp(playerHelper.MulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new Color[]
            {
                new Color(36, 208, 2),
                new Color(36, 208, 2),
            }) * MathHelper.Lerp(0f, 1.4f, colorFade), new Color(81, 6, 233), (float)Math.Pow((double)completionRatio, 1.0));
        }
        internal float WidthFunction(float completionRatio)
        {
            float widthRatio = Utils.GetLerpValue(0f, 1f, completionRatio, false);
            return MathHelper.Lerp(playerHelper.FMulticolorLerp((float)Math.Pow((double)completionRatio, 0.5), new float[]
            {
                10,
                20,
                30,
                40,
                50,
                60,
                50,
                40,
                30,
                20,
                10,
            }) * Projectile.scale, 10 * Projectile.scale, (float)Math.Pow((double)completionRatio, 1.0));
        }
    }
}