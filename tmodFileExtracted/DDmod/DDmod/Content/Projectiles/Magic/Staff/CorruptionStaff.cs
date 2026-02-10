using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class CorruptionStaff : AMagicStaff
    {
        public override int ProjShoot => ModContent.ProjectileType<CorrosiveBullet>();
        public override float CircleValue => 0.05F;
        public override float MaxCircle => 1f;
        public override float Distance => 24;
        public override float ShootDistance => 36;
        public override void Set()
        {
        }
        public override bool PreAI()
        {
            return true;
        }
        public override SoundStyle Sound()
        {
            SoundStyle sound = SoundID.Item20;
            sound.MaxInstances = 10;
            sound.Pitch = 1f;
            sound.Volume = 0.2f;
            return sound;
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
                int Dust = NewDust(vector + new Vector2(0, Main.rand.NextFloat(-40, 40)).RotatedBy(Projectile.rotation - StaffRot), 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(46, 33, 91, 255), Main.rand.NextFloat(0.4F, 0.8F));
                Main.dust[Dust].velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(1, ProjShootSpeed * 4);
                Main.dust[Dust].customData = 2F;
            }
        }
        public override void Shoot(Player player)
        {
            Vector2 vector = Projectile.velocity.PerfectNormalize();
            Projectile projectile = Main.projectile[NewProjectileChange(Projectile.GetSource_FromThis(), player.Center + SolidTileDistanceDetection(player, vector * ShootDistance) + new Vector2(0, Main.rand.NextFloat(-40, 40)).RotatedBy(Projectile.rotation - StaffRot), vector * ProjShootSpeed, ProjShoot, Projectile.damage, Projectile.knockBack, Projectile.owner)];
            projectile.scale = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DProj().Times[0] += 0.05f;
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Texture2D VoidStar = DDTextures.VoidStar.Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, 0, 0f);

            Color color = new Color(46, 33, 91, 0);
            color.A = 100;
            Vector2 vector = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 22;
            //光球渲染
            Main.spriteBatch.Draw(VoidStar, vector, null, color * 0.4f, Projectile.rotation - StaffRot, VoidStar.Size() / 2, new Vector2(0.5f, 1.1f) * Circle, 0, 0f);

            texture = DDTextures.Circle[12].Value;
            color = new Color(46, 33, 91, 255);
            //法阵
            DDHelper.Compression(texture, color, Projectile.rotation - StaffRot, Projectile.Opacity, new Vector2(5, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.AlphaBlend);

            Main.spriteBatch.Draw(texture, vector, null, color, 0f, Utils.Size(texture) / 2, Circle, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}