using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Summon;
using DDmod.Modkey;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Talisman
{
    public class 地狱葫芦Proj : Talismans
    {
        public override void SetStaticDefaults()
        {
        }
        public override void PreUse()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = Projectile.DProj().vector[0] - Projectile.Center;
            Projectile.rotation = vector.ToRotation() + MathHelper.PiOver2;
            Projectile.DProj().Times[0]++;
            if (Projectile.DProj().Times[0] < 60)
            {
                Projectile.velocity += vector.PerfectNormalize() * 0.1F;
                if (Projectile.DProj().Times[0] > 30 && Projectile.DProj().Times[0] % 2 == 0 && Projectile.owner == Main.myPlayer)
                {
                    int A = NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.3F,0.3F)) * Main.rand.NextFloat(8, 12), ModContent.ProjectileType<FireBall>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1,1);
                    Main.projectile[A].tileCollide = false;
                    Main.projectile[A].DamageType = DamageClass.Default;
                    for (int I = 0; I < 20; I++)
                    {
                        Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) - new Vector2(0, 12).RotatedBy(Projectile.rotation), 1, 1, 6, 0, 0, 0, Color.White)];
                        dust.noGravity = true;
                        dust.scale = 1.2F;
                        dust.velocity = vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(2, 6);
                    }
                    Projectile.velocity -= vector.PerfectNormalize() * 1;
                }
            }
            else
            {
                for (int I = 0; I < 2; I++)
                {
                    Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) - new Vector2(0, 12).RotatedBy(Projectile.rotation), 1, 1, 6, 0, 0, 0, Color.White)];
                    dust.noGravity = true;
                    dust.scale = 1.2F;
                    dust.velocity = vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-1, 1)) * Main.rand.NextFloat(1, 2);
                }
            }
            if (!Projectile.DProj().Bool[1])
            {
                NewDustChange(60, Projectile.Center, Vector2.Zero, 6, 0.1F, 5, Scale: Main.rand.NextFloat(1.2F, 1.8F));
                Projectile.Center = player.Dplayer().MouseWorld - vector.PerfectNormalize().RotatedBy(Main.rand.NextFloat(0,MathHelper.TwoPi))*200;
                NewDustChange(60, Projectile.Center, Vector2.Zero, 6, 0.1F, 5, Scale: Main.rand.NextFloat(1.2F, 1.8F));
                Projectile.DProj().Bool[1] = true;
                Projectile.netUpdate = true;
            }
            Projectile.velocity *= 0.98f;
        }
        public override void End()
        {
            Projectile.DProj().Bool[1] = false;
            Projectile.DProj().Times[0] = 0;
            NewDustChange(160, Projectile.Center, Vector2.Zero, 6, 0.1F, 5, Scale: Main.rand.NextFloat(0.8F, 1.5F));
        }
        public override void ExtraUse()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.DProj().vector[0] = player.Dplayer().MouseWorld;
            Projectile.DProj().Bool[1] = false;
            Projectile.DProj().Times[0] = 0;
            Projectile.velocity = Vector2.Zero;
        }
        public override bool MobileAI()
        {
             Projectile.RotationSpeed(Projectile.velocity.X * 0.05F, 0.05F);
            if (Math.Abs(Projectile.velocity.X)<0.2F)
            {
                Projectile.rotation = 0;
            }
            Player player = Main.player[Projectile.owner];
            Projectile.spriteDirection = -player.direction;
            if (player.TPlayer().TalismanCD >= player.TPlayer().MaxTalismanCD)
            {
                Dust dust = Main.dust[NewDust(Projectile.Center - new Vector2(4) - new Vector2(0, 16).RotatedBy(Projectile.rotation), 6, 1, 6, 0, 0, 0, Color.White)];
                dust.noGravity = true;
                dust.scale = 0.8F;
                dust.velocity = new Vector2(0, -1).RotatedBy(Projectile.rotation) * Main.rand.NextFloat(2, 6);
            }
            return base.MobileAI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[2]+=0.1F;
            Player player = Main.player[Projectile.owner];
                SpriteEffects spriteEffects = 0;
            if (Projectile.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            
            Vector2 vector = Projectile.Size / 2;

            Color color = new Color(253, 62, 3, 0);
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Texture2D texture = DDTextures.Circle[9].Value;
            Main.spriteBatch.Draw(VoidStar, Projectile.Center+new Vector2(0,18).RotatedBy(Projectile.rotation) - Main.screenPosition, null, color, Projectile.rotation, VoidStar.Size() / 2, new Vector2(0.5F, 0.1F), 0, 0f);
            Main.spriteBatch.Draw(VoidStar, Projectile.Center + new Vector2(0, 18).RotatedBy(Projectile.rotation) - Main.screenPosition, null, color, Projectile.rotation, VoidStar.Size() / 2, new Vector2(0.5F, 0.1F), 0, 0f);
            DDHelper.Compression(texture, color, Projectile.rotation, Projectile.Opacity, new Vector2(1, 15), 1, Projectile.DProj().Times[2], BlendState.Additive);

            Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(0, 18).RotatedBy(Projectile.rotation) - Main.screenPosition, null, color, 0f, Utils.Size(texture) / 2, 0.1F, 0, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(0, 18).RotatedBy(Projectile.rotation) - Main.screenPosition, null, color, 0f, Utils.Size(texture) / 2, 0.1F, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center- Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            if (Projectile.DProj().Bool[0])
            {
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(255, 124, 57, 0), Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
                Main.spriteBatch.Draw(texture, Projectile.position + vector - Main.screenPosition, null, new Color(255, 124, 57, 0), Projectile.rotation, texture.Size() / 2, 1, spriteEffects, 0f);
            }
            else
            {
            }
            return false;
        }
    }
}