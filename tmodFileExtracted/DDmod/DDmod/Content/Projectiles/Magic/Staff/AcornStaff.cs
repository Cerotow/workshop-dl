using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class AcornStaff : AMagicStaff
    {
        public override int ProjShoot => ModContent.ProjectileType<MagicAcorn>();
        public override float CircleValue => 0.03F;
        public override float MaxCircle => 0.75f;
        public override float Distance => 24;
        public override float ShootDistance => 26;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            CircleE *= 1.09F;
            CircleE2 *= 1.06F;
            CircleE3 *= 1.03F;
            CircleA -= 0.1F;
            return true;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void ShootEffect(Player player)
        {
            /*
            Vector2 vector = Projectile.Center + Projectile.velocity.PerfectNormalize() * 26 - new Vector2(4);
            for (int a = 0; a < 30; a++)
            {
                Vector2 R = new Vector2(0, 1).RotatedBy(MathHelper.TwoPi / 30 * a);
                R.X /= 5;
               int Dust =  NewDust(vector + R.RotatedBy(Projectile.rotation - MathHelper.PiOver4)*10, 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(100, 255, 100, 0), 0.6F);
                Main.dust[Dust].velocity = R.RotatedBy(Projectile.rotation - MathHelper.PiOver4) * 5;
                Main.dust[Dust].customData = 1F;
            }*/
            Vector2 vector = Projectile.Center + Projectile.velocity.PerfectNormalize() * 26 - new Vector2(4);
            for (int a = 0; a < 30; a++)
            {
                int Dust = NewDust(vector+new Vector2(0, Main.rand.NextFloat(-50, 50)).RotatedBy(Projectile.rotation - MathHelper.PiOver4), 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(100, 255, 100, 0), Main.rand.NextFloat(0.4F,1));
                Main.dust[Dust].velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4F,0.4F))*Main.rand.NextFloat(1, ProjShootSpeed);
                Main.dust[Dust].customData = 2F;
            }
            Circle = MaxCircle;
            Circle *= 1.75F;
            CircleA = 1;
            CircleE = 1;
            CircleE2 = 1;
            CircleE3 = 1;
        }
        public override void Shoot(Player player)
        {
            Vector2 vector = Projectile.velocity.PerfectNormalize();
            Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), player.Center + SolidTileDistanceDetection(player, vector * ShootDistance) + new Vector2(0, Main.rand.NextFloat(-30, 30)).RotatedBy(Projectile.rotation - MathHelper.PiOver4), vector * ProjShootSpeed, ProjShoot, Projectile.damage, Projectile.knockBack, Projectile.owner)];
            projectile.scale = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[0] += 0.05f;
            Color color = Projectile.GetAlpha(new Color(100, 255, 100, 0));
            SpriteEffects sprite = (SpriteEffects)((Main.player[Projectile.owner].direction == 1) ? 0 : 1);
            Texture2D texture = DDTextures.Circle[4].Value;
            Texture2D Staff = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            float rot = Projectile.rotation;

            if (Main.player[Projectile.owner].direction == -1)
            {
                rot += MathHelper.PiOver2;
            }
            //法杖
            Main.spriteBatch.Draw(Staff, Projectile.Center - Main.screenPosition, null, lightColor,rot , Staff.Size()/2, Projectile.scale, sprite, 0f);


            Vector2 vector = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 26;
            //光球渲染
            Main.spriteBatch.Draw(VoidStar, vector, null, color * 0.4f, Projectile.rotation - MathHelper.PiOver4, VoidStar.Size() / 2, new Vector2(0.5f,1.1f)* Circle, 0, 0f);

            //法阵
            DDHelper.Compression(texture, color, Projectile.rotation - MathHelper.PiOver4, Projectile.Opacity, new Vector2(5, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);

            Main.spriteBatch.Draw(texture, vector, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color, 0f, Utils.Size(texture) / 2, Circle, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (CircleA > 0)
            {
                Main.spriteBatch.Draw(texture, vector, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color * CircleA, Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) / 2, new Vector2(0.2F, 1) * Circle * CircleE, 0, 0);
                Main.spriteBatch.Draw(texture, vector , new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color * CircleA, Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) / 2, new Vector2(0.2F, 1) * Circle * CircleE2, 0, 0);
                Main.spriteBatch.Draw(texture, vector, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), color * CircleA, Projectile.rotation - MathHelper.PiOver4, Utils.Size(texture) / 2, new Vector2(0.2F, 1) * Circle * CircleE3, 0, 0);
            }
            return false;
        }
    }
}