using DDmod.Content.Dusts;
using DDmod.Helper;

namespace DDmod.Content.Projectiles.Boss.MiniBoss
{
    public class Boss妖精弹 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.scale = 1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.alpha = 0;
            Projectile.localAI[0] = 1F;
            Projectile.localAI[1] = 6;
        }
        public override void AI()
        {
            if (Projectile.ai[0] ==0)
            {
                Projectile.scale = Projectile.localAI[0];
            }
            if (Projectile.ai[0] ==1)
            {
                Projectile.scale = 1.25f;
                if (Projectile.ai[1]++>60)
                {
                    Projectile.Kill();
                }
            }
            if (Projectile.ai[0] ==2)
            {
                Projectile.localAI[0] = Main.rand.NextFloat(0.5F, 0.75F);
                Projectile.ai[0] = 0;
            }
            if (Projectile.ai[0] ==3)
            {
                Projectile.localAI[0] = Main.rand.NextFloat(0.75F, 1.05F);
                Projectile.ai[0] = 0;
            }
            if (Projectile.ai[0] ==4)
            {
                Projectile.velocity = ((Projectile.rotation - MathHelper.PiOver2).ToRotationVector2()) * Projectile.localAI[1];
                if (Projectile.localAI[2] == 0)
                {
                    if (Main.rand.NextBool(2))
                    {
                        Projectile.localAI[2] = -1;
                    }
                    else
                    {
                        Projectile.localAI[2] = 1;
                    }
                    Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                }
                Projectile.scale = 0.8F;
                if (Projectile.DProj().track > 500)
                {
                    Projectile.Kill();

                }
                if (Projectile.DProj().track > 80)
                {
                    if (Projectile.localAI[1] < 8)
                    {
                        Projectile.localAI[1] += 0.3F;
                        Player player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
                        Vector2 vector = player.Center - Projectile.Center;
                        Projectile.RotationSpeed(vector.ToRotation() + MathHelper.PiOver2, 0.03F * Projectile.localAI[1]);
                    }
                    else
                        if(Projectile.localAI[1]>8)
                    {
                        Projectile.localAI[1] = 8;
                    }
                    
                }
                else
                {
                    Projectile.localAI[1] *= 0.98F;
                    Projectile.rotation += Projectile.localAI[1]*0.005F * Projectile.localAI[2];
                }
            }
            if(Projectile.velocity.Length()>2&&Main.rand.NextBool(4))
            {
                int Type = ModContent.DustType<速度粒子>();
                Dust dust = Main.dust[NewDust(Projectile.position, Projectile.width, Projectile.height, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 191, 0, 0))];
                dust.noGravity = true;
                dust.scale =  1.5F*Projectile.scale;
                dust.rotation = Projectile.velocity.ToRotation();
                dust.customData = Projectile.scale;
                dust.velocity = -Projectile.velocity/4;

            }
            Projectile.ProjScaleChange();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color color = new Color(255, 191, 0, 100);
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            if (Projectile.ai[0] != 4)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            int L = Projectile.oldPos.Length;
            if (Projectile.scale<1)
            {
                L = (int)(L * Projectile.scale);
            }
            for (int i = 0; i < L; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color2 = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4 , SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color.Opposite()*0.5F, Projectile.rotation, texture.Size() / 2, Projectile.scale / 4, SpriteEffects.None, 0);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                int Type = ModContent.DustType<光球粒子>();
                Dust dust = Main.dust[NewDust(Projectile.oldPos[i] + Projectile.Size / 2 - new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 191, 0, 100))];
                dust.noGravity = true;
                dust.scale = 0.07f * (Projectile.oldPos.Length - i);
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
            }
            if(Main.netMode!=1&& Projectile.ai[0] == 1)
            {
                for (int i = 0; i < 5; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center,new Vector2(0,Main.rand.NextFloat(3,7)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)),Projectile.type,(int)(Projectile.damage*0.8F),0,-1,2);
            }
            for (int i = 0; i < 20; i++)
            {
                int Type = ModContent.DustType<光球粒子>();
                Dust dust = Main.dust[NewDust(Projectile.Center-new Vector2(4), 1, 1, Type, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 191, 0, 100))];
                dust.noGravity = true;
                dust.scale = Projectile.scale;
                dust.velocity = new Vector2(0, -1).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * Main.rand.NextFloat(2, 4);
            }

        }
    }
}