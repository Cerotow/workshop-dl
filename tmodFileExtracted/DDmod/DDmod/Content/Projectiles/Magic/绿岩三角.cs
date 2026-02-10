using DDmod.Content.Dusts;
using DDmod.Content.Items.Melee.Sword;
using Terraria;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic
{
    public class 绿岩三角 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] =16;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.scale = 1;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
        }
        public override void AI()
        {

            if (Projectile.ai[0] < 0)
            {
                Projectile.damage = 0;
                if(Projectile.ai[0]==-1)
                {
                    Projectile.alpha = 0;
                    Projectile.ai[0] = -2;
                }
                if(Projectile.alpha<255)
                {
                    Projectile.alpha += 20;
                    Projectile.scale += 0.2F;
                }
                else
                {
                    Projectile.Kill();
                }
                return;
            }
            Projectile.ProjScaleChange();
            if (Projectile.soundDelay <= 0)
            {
                for (int a = 0; a < 1; a++)
                {
                    int A = NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<速度粒子>(), 0, 0, 100, new Color(89, 237, 100, 50), Main.rand.NextFloat(1F, 3F) * Projectile.scale);
                    Main.dust[A].velocity = -Projectile.velocity.PerfectNormalize() * 5;
                    Main.dust[A].customData = 1F;
                    Main.dust[A].rotation = Projectile.velocity.ToRotation();
                    Main.dust[A].noGravity = true;
                }
                Projectile.soundDelay = 12;
            }
            Projectile.scale += 0.005F;
            if (Projectile.originalDamage == 0)
                Projectile.originalDamage=Projectile.damage;

            Projectile.damage = (int)(Projectile.originalDamage * Projectile.scale);
            Projectile.extraUpdates = 1;
            Projectile.ai[1]+=Projectile.velocity.Length()/11;
            if (Projectile.ai[1]>= 3)
            {
                Projectile.ai[1] = 0;
            }
            if (Projectile.ai[0] == 0)
            {
                if(Projectile.alpha>0)
                    Projectile.alpha -= 20;
            }
            else
            {
                if (Projectile.alpha < 0)
                    Projectile.alpha += 40;
            }
            //Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;
        }
        /*
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0]==0)
            {
                Projectile.ai[0] = 1;
                Projectile.velocity = oldVelocity * 0.1F;
            }
            else
            {
                Projectile.velocity = oldVelocity;

            }
            return false;
        }*/
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
            float Length = Projectile.velocity.Length() / (Projectile.height / 4);
            //Projectile.ai[2] += 0.2F;
            if (Projectile.ai[0] >= 0)
            {
                DDHelper.Compression(texture, Color.White, 0, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.ai[2], BlendState.Additive);
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    if (i % 3 == (int)Projectile.ai[1])
                    {
                        Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                        Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                        GameShaders.Misc["压缩"].UseColor(color*0.15f);
                        GameShaders.Misc["压缩"].Apply(default);
                        for (int a = 1; a <= 4; a++)
                        {
                            Main.spriteBatch.Draw(texture, vector2 + Projectile.velocity.PerfectNormalize() * (0.5F * a), null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                        }
                    }
                }
            }
            if (Projectile.alpha < 0)
            {
                DDHelper.Compression(texture, Projectile.GetAlpha(Color.White), 0, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.ai[2], BlendState.AlphaBlend);
            }
            else
            {
                DDHelper.Compression(texture, Projectile.GetAlpha(Color.White)*0.1F, 0, Projectile.Opacity, new Vector2(4, 1), Projectile.direction, Projectile.ai[2], BlendState.Additive);

            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            for (int a = 1; a <= 6; a++)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center + Projectile.velocity.PerfectNormalize() * (0.5F*a) - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] >= 0)
            {
                if(Projectile.velocity.X<0)
                {
                    Projectile.rotation += MathHelper.Pi;
                }
                int Type = ModContent.DustType<光球粒子>();
                for (int i = 0; i < 10; i+=1)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center+new Vector2(i, i-5) - new Vector2(4), 1, 1, Type, 0, 0, 100, new Color(89, 237, 100, 50),2)];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale;
                    dust.velocity = ((dust.position - Projectile.Center) * new Vector2(1, 2F)).RotatedBy(Projectile.rotation) * 0.5F * (1 + Projectile.scale / 3);
                    dust.rotation = dust.velocity.ToRotation();
                    dust.customData = 5.4F+DDHelper.DustAI(dust,1);
                }
                for (int i = 0; i < 10; i += 1)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(-i, i - 5) - new Vector2(4), 1, 1, Type, 0, 0, 100, new Color(89, 237, 100, 50), 2)];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale;
                    dust.velocity = ((dust.position - Projectile.Center) * new Vector2(1, 2F)).RotatedBy(Projectile.rotation) * 0.5F*(1+Projectile.scale/3);
                    dust.rotation = dust.velocity.ToRotation();
                    dust.customData = 5.4F + DDHelper.DustAI(dust, 1);
                }
                for (int i = -10; i < 10; i += 1)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center + new Vector2(i, 5) - new Vector2(4), 1, 1, Type, 0, 0, 100, new Color(89, 237, 100, 50), 2)];
                    dust.noGravity = true;
                    dust.scale = Projectile.scale;
                    dust.velocity = ((dust.position - Projectile.Center) * new Vector2(1, 2F)).RotatedBy(Projectile.rotation) * 0.5F * (1 + Projectile.scale / 3);
                    dust.rotation = dust.velocity.ToRotation();
                    dust.customData = 5.4F + DDHelper.DustAI(dust, 1);
                }
            }
        }
    }
}