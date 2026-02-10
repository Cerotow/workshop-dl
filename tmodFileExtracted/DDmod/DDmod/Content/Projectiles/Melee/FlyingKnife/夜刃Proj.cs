using Terraria;
using Terraria.GameContent.Drawing;

namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 夜刃Proj : 飞刀Proj
    {

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
            if (player.Aplayer().NightEnergy)
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
                if (player.Aplayer().NightEnergy)
                {
                    for (int A = -1; A <= 1; A++)
                    {
                        Vector2 vector = Projectile.Center - Projectile.velocity.RotatedBy(A - Main.rand.NextFloat(-0.3f, 0.3f)).PerfectNormalize() * 240;
                        NewProjectile(Projectile.GetSource_FromAI(), vector, (Projectile.Center - vector).PerfectNormalize() * 16, Type, Projectile.damage / 2, 0, Projectile.owner, 1);

                        NewDustChange(10, vector - new Vector2(4), Vector2.Zero, 27, 0, 6, true, 2f);
                    }
                }
                else
                {
                    Vector2 vector = Projectile.Center - Projectile.velocity.RotatedBy(Main.rand.NextFloat(-1.3f,1.3f)).PerfectNormalize() * 240;
                    NewProjectile(Projectile.GetSource_FromAI(), vector, (Projectile.Center - vector).PerfectNormalize() * 16, Type, Projectile.damage / 2, 0, Projectile.owner, 1);

                    NewDustChange(10, vector - new Vector2(4), Vector2.Zero, 27, 0, 6, true, 2f);
                }
            }
            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            settings.MovementVector = Projectile.velocity;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge, settings, Projectile.owner);
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 20; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                int A = NewDust(Projectile.position, 1, 1, 27, projDirection.X, projDirection.Y, 0, default, 1.8f);
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
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width * 0.5f, Projectile.height / 2);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
                RO -= Rotation2;
                Origia = new Vector2(texture.Width * 0.5f, texture.Height - Projectile.height / 2);
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
                Color color = new Color(81, 6, 233, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 1.2f;
                Main.spriteBatch.Draw(texture, vector2, null, Projectile.GetAlpha(color), A, Origia, Projectile.scale * 1.2F, sprite, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, Projectile.GetAlpha(color), A, Origia, Projectile.scale * 0.8F, sprite, 0f);
            }
            if (Projectile.ai[0] == 0)
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, RO, Origia, Projectile.scale, sprite, 0f);

            return false;
        }
    }
}