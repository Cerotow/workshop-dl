using DDmod.Content.Dusts;
using Terraria.Graphics.Shaders;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class AquaScepter : AMagicStaff
    {
        public override int ProjShoot => 22;
        public override float CircleValue => 0.05F;
        public override float MaxCircle => 0.8f;
        public override float Distance => 24;
        public override float ShootDistance => -16;
        public override void Set()
        {
            StaffRot = 0;
        }
        public override bool PreAI()
        {
            CircleE *= 1.1F;
            CircleA -= 0.1F;

            return true;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override void ShootEffect(Player player)
        {
            Vector2 vector = Projectile.Center + Projectile.velocity.PerfectNormalize() * 22 - new Vector2(4);
            for (int a = 0; a < 30; a++)
            {
                int Dust = NewDust(vector + new Vector2(0, Main.rand.NextFloat(-50, 50)).RotatedBy(Projectile.rotation), 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 100, new Color(11, 46, 255, 255), Main.rand.NextFloat(0.4F, 2));
                Main.dust[Dust].velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4F, 0.4F)) * Main.rand.NextFloat(1, ProjShootSpeed*2);
                Main.dust[Dust].customData = 3F;
            }
            Circle = MaxCircle;
            Circle *= 1.5F;
                CircleA = 0.8F;
                CircleE = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[0] += 0.05f;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);

            Color color = new Color(11, 46, 255, 0)*0.5f;
            color.A = 100;


            Vector2 vector = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 22;
            //光球渲染
            Main.spriteBatch.Draw(VoidStar, vector, null, color * 0.4f, Projectile.rotation, VoidStar.Size() / 2, new Vector2(0.5f, 1.1f) * Circle, 0, 0f);

            texture = DDTextures.Circle[9].Value;
            color = new Color(11, 46, 255, 255);
            if (CircleA > 0)
            {
                Main.spriteBatch.Draw(texture, vector, null, color * CircleA, Projectile.rotation, Utils.Size(texture) / 2, new Vector2(0.2F, 1) * Circle / 4 * CircleE, 0, 0);
                Main.spriteBatch.Draw(texture, vector, null, color * CircleA, Projectile.rotation, Utils.Size(texture) / 2, new Vector2(0.2F, 1) * Circle / 4 * CircleE, 0, 0);
                Main.spriteBatch.Draw(VoidStar, vector, null, color * 0.4f, Projectile.rotation, VoidStar.Size() / 2, new Vector2(0.5f, 1.1f) * Circle, 0, 0f);
            }
            //法阵
            DDHelper.Compression(texture, color, Projectile.rotation, Projectile.Opacity, new Vector2(5, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.AlphaBlend);

            Main.spriteBatch.Draw(texture, vector, null, color, 0f, Utils.Size(texture) / 2, Circle/4, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}