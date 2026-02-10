using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class Boss异星毒刺 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.scale = 1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Projectile.scale = Projectile.ai[0]*1.25f;
            Projectile.ProjScaleChange();
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 30;
            }
            else
            {
                for (int a = 0; a < 2; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, 229, 0, 0, 100, Color.White, Main.rand.NextFloat(0.3F, 1F));
                    Main.dust[A].velocity = -Projectile.velocity.PerfectNormalize() * 5;
                    Main.dust[A].customData = 2.4F;
                    Main.dust[A].noGravity = true;
                }

                for (int a = 0; a < 2; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 100,new Color(34, 221, 151, 50), Main.rand.NextFloat(1.5F, 2F));
                    Main.dust[A].velocity = -Projectile.velocity.PerfectNormalize() * 2;
                    Main.dust[A].rotation = Projectile.velocity.ToRotation();
                    Main.dust[A].customData = 1F;
                    Main.dust[A].noGravity = true;
                }
            }
            if (Projectile.ai[1]==0)
            {
                if(Projectile.DProj().track>5&& Projectile.DProj().track < 25)
                {
                    Projectile.RotationSpeed((Main.player[Player.FindClosest(Projectile.Center,0,0)].Center-Projectile.Center).ToRotation(),0.03F);
                    Projectile.velocity = Projectile.rotation.ToRotationVector2() * Projectile.velocity.Length();
                }
                else
                {

                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<异星毒液Buff>(),300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(34, 221, 151, 50);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = Projectile.Size / 2;
            float RO = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            int L = Projectile.oldPos.Length;
            for (int i = 0; i < L; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color2 = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                color2.A = (byte)(color2.A * 0.5F);
                Main.spriteBatch.Draw(texture, vector2, null, color2, RO, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
                Main.spriteBatch.Draw(texture, vector2, null, color2, RO, texture.Size() / 2, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, RO, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                int Type = 229;
                Dust dust = Main.dust[NewDust(Projectile.oldPos[i] + Projectile.Size / 2, 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100)];
                dust.noGravity = true;
                dust.scale = 0.07f * (Projectile.oldPos.Length - i);
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
            }

        }
    }
}