using Terraria;
namespace DDmod.Content.Projectiles.Melee.FlyingKnife
{
    public class 血猩獠牙Proj : 飞刀Proj
    {

        public override void Defaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            AIStyle = 飞刀AI.AI3;
            Rotation = MathHelper.PiOver2;
            Rotation2 = MathHelper.Pi;
            Projectile.penetrate = 3;
        }
        public override bool PreAI()
        {

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 5;
                NewDust(Projectile.position + Vector2.Normalize(-Projectile.velocity) * 38 + Projectile.velocity, 1, 1, 5);
            }
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.HasBuff(BuffID.Bleeding))
            {
                modifiers.SourceDamage += 0.2F;
            }
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(30, Main.rand.Next(100, 300));

            }
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 50; i++)
            {
                Vector2 projDirection = Main.rand.NextVector2Unit(Projectile.velocity.ToRotation(), Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * (Main.rand.NextFloat(2.8f, 3f) * (Projectile.velocity.Length() / 10));
                NewDust(Projectile.position, 1, 1, 5, projDirection.X, projDirection.Y,0,default,1.3f);
            }

        }
        public override bool? CanDamage()
        {
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
                Color color = new Color(136, 0, 0, 0) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length)*1.2f;
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], new Vector2(texture.Width/2, texture.Height / 4), Projectile.scale * 1.2F, 0, 0f);
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], new Vector2(texture.Width / 2, texture.Height / 4), Projectile.scale * 0.8F, 0, 0f);

                if (Projectile.DProj().Times[3] == 0)
                {
                    Projectile.oldPos[i] += player.velocity/2;
                }
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition,null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 4), Projectile.scale, 0, 0f);

            return false;
        }
    }
}