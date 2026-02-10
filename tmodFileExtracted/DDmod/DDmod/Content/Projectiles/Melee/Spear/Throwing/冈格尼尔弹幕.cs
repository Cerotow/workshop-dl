
using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria;

namespace DDmod.Content.Projectiles.Melee.Spear.Throwing
{
    public class 冈格尼尔弹幕 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height=  40;
            Projectile.friendly = true;
            Projectile.penetrate =  1;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Main.projFrames[Type] = 5;
            Projectile.extraUpdates = 2;
            Projectile.DProj().Magnification = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[0]++;
            //NewDustChange2(2, Projectile.Center - new Vector2(4), Vector2.Zero, ModContent.DustType<速度粒子>(), 1, 8, true,1, 4f,0, new Color(255, 40, 48, 0));
            if (Projectile.ai[0] %5==0&&Projectile.ai[0]>=10&& Projectile.ai[0]<25)
            {
                int a = 25;
                int type = ModContent.DustType<速度粒子>();

                for (int i = 0; i < a; i++)
                {
                    Vector2 vector = new Vector2(0, 15+30*Projectile.ai[0]/6).RotatedBy(MathHelper.TwoPi / a * i);
                    vector *= new Vector2(0.4F, 1);
                    vector = vector.RotatedBy(Projectile.velocity.ToRotation());
                    Dust dust2 = Main.dust[Dust.NewDust(Projectile.Center + vector, 1, 1,type , 0, 0, 0, new Color(255, 40, 48, 0))];
                    dust2.noGravity = true;
                    dust2.customData=3+ dust2.DustAI(1);
                    dust2.velocity = -Projectile.velocity/2;
                    dust2.rotation = Projectile.velocity.ToRotation();
                    dust2.scale = 2F * Projectile.ai[0] / 6*0.4f;
                    if(type == ModContent.DustType<速度粒子>())
                    {
                        dust2.velocity = -Projectile.velocity;
                        dust2.customData = 3;
                        dust2.scale = 2F * Projectile.ai[0] / 6;

                    }
                }
            }
            /*
            //if (Main.rand.NextBool(4))
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (Projectile.height/2) + Projectile.velocity.PerfectNormalize().RotatedBy(0.4f) * (Projectile.height / 2) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 40, 48, 0) * 0.6F, 4)];
                dust.velocity = -Projectile.velocity.PerfectNormalize().RotatedBy(-0.4f) * 15;
                dust.customData = 2;
                dust.noGravity = false;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;
            }
            //if (Main.rand.NextBool(4))
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (Projectile.height/2)+ Projectile.velocity.PerfectNormalize().RotatedBy(-0.4f) * (Projectile.height / 2) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 40, 48, 0) * 0.6F, 4)];
                dust.velocity = -Projectile.velocity.PerfectNormalize().RotatedBy(0.4f) * 15;
                dust.rotation = dust.velocity.ToRotation();
                GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;
                dust.customData = 2;
                dust.noGravity = false;
            }*/
            Dust dust;
            /*
            if (Main.rand.NextBool(4))
            {
                dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (Projectile.height / 2) + Projectile.velocity.PerfectNormalize().RotatedBy(0.4f) * (Projectile.height / 2) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 40, 48, 0))];
                dust.noGravity = true;
                dust.velocity = -Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0.75F, 1.25F)) * Main.rand.NextFloat(0.5F, 2F);
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(3F, 5F);
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();

            }
            if (Main.rand.NextBool(4))
            {
                dust = Main.dust[NewDust(Projectile.Center + Projectile.velocity.PerfectNormalize() * (Projectile.height / 2) + Projectile.velocity.PerfectNormalize().RotatedBy(-0.4f) * (Projectile.height / 2) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 40, 48, 0))];
                dust.noGravity = true;
                dust.velocity = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(0.75F, 1.25F)) * Main.rand.NextFloat(0.5F, 2F);
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(3F, 5F);
                dust.customData = 2;
                dust.rotation = dust.velocity.ToRotation();
            }*/
            //GlobalDust.DustProjectileOwner[dust.dustIndex] = Projectile.whoAmI;

            dust = Main.dust[NewDust(Projectile.Center-Projectile.velocity.PerfectNormalize()*24 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 40, 48, 0))];
            dust.noGravity = true;
            dust.velocity = -Projectile.velocity/3;
            dust.alpha = 100;
            dust.scale = 8;
            dust.customData = 6;
            dust.rotation = Projectile.velocity.ToRotation();

            Projectile.frameCounter++;
            if (Projectile.frameCounter%6==0)
            {
                Projectile.frame++;
            }
            Projectile.frame %= Main.projFrames[Type];
        }
        public override void OnKill(int timeLeft)
        {
            PlaySound(SoundID.Dig, Projectile.position);
            Player player = Main.player[Projectile.owner];
            for (int A = 0; A <30; A++)
            {
                /*
                Dust dust = Main.dust[NewDust(Projectile.Center, 0, 0, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 40, 48, 0))];
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(3,7);
                dust.customData = 3F;
                dust.velocity = -Projectile.oldVelocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi)) * Main.rand.NextFloat(0, 33);
                dust.rotation = dust.velocity.ToRotation();
                */
                Projectile.velocity = Projectile.oldVelocity;
                Projectile.position = Projectile.oldPosition;
                Vector2 vector = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(0F, 0.8F)) * Main.rand.NextFloat(0.3F, 2.5F);
              Dust dust = Main.dust[NewDust(Projectile.Center + vector.PerfectNormalize() * 32 + Projectile.velocity.PerfectNormalize() * (Projectile.height / 2) + Projectile.velocity.PerfectNormalize().RotatedBy(0.4f) * (Projectile.height / 2) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 40, 48, 0) )];
                dust.noGravity = true;
                dust.velocity = vector;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(4F, 8F);
                dust.customData = 5;
                dust.rotation = dust.velocity.ToRotation();
                vector = -Projectile.velocity.RotatedBy(-Main.rand.NextFloat(0F, 0.8F)) * Main.rand.NextFloat(0.3F, 2.5F);
                dust = Main.dust[NewDust(Projectile.Center+ vector.PerfectNormalize()*32 + Projectile.velocity.PerfectNormalize() * (Projectile.height / 2) + Projectile.velocity.PerfectNormalize().RotatedBy(-0.4f) * (Projectile.height / 2) - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(255, 40, 48, 0))];
                dust.noGravity = true;
                dust.velocity = vector;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(4F, 8F);
                dust.customData = 5;
                dust.rotation = dust.velocity.ToRotation();
            }
            /*Texture2D texture = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Logo").Value;
            Color[] colors = DDHelper.GetColors(texture);
            for (int i = 0; i < colors.Length; i++)
            {
                float x = i % texture.Width;
                float y = i / texture.Width;
                if (colors[i] != new Color(0, 0, 0, 0))
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center + (new Vector2(x, y) - texture.Size() / 2).RotatedBy(Projectile.rotation), 1, 1, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(255, 40, 48, 0), 1f)];
                    //dust.customData = true;
                    dust.noGravity = true;
                    dust.velocity = dust.position - Projectile.Center;
                }
            }*/
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            float RO = Projectile.rotation;
            SpriteEffects sprite = 0;
            Vector2 Origia = new Vector2(texture.Width-Projectile.width/2, texture.Height / Main.projFrames[Type] * 0.5f);
            if (Projectile.velocity.X < 0)
            {
                sprite = SpriteEffects.FlipVertically;
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float RO2 = Projectile.oldRot[i];
                Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                Color color = Color.White * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);

                Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Type])), color, RO2, Origia, Projectile.scale, sprite, 0f);
            }

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, texture.Height / Main.projFrames[Type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Type])), Color.White, RO, Origia, Projectile.scale, sprite, 0f);
            //Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White*0.5F, 0, Vector2.Zero, Projectile.Size/2, sprite, 0f);
            return false;
        }
    }
}