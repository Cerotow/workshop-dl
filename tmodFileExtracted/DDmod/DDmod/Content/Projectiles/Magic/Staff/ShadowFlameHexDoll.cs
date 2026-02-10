namespace DDmod.Content.Projectiles.Magic.Staff
{
    public class ShadowFlameHexDoll : AMagicStaff
    {

        public override int ProjShoot => ModContent.ProjectileType<ShadowFireball>();
        public override float CircleValue => 0.01F;
        public override float MaxCircle => 1f;
        public override float Distance => 24;
        public override float ShootDistance => 36;
        public override byte AIStyle => 5;
        public override void Set()
        {
            RotSpeed = 0.05f;
            MaxShoot = 5;
        }
        public override bool PreAI()
        {
            return true;
        }
        public override void Shoot(Player player)
        {
            NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.PerfectNormalize() * Projectile.SolidTileDistanceDetection(Main.rand.NextFloat(10, 15), 50) + new Vector2(0, Main.rand.NextFloat(-30, 30)).RotatedBy(Projectile.rotation - MathHelper.PiOver4), Projectile.velocity * Main.rand.NextFloat(10, 15), ModContent.ProjectileType<ShadowTentacles>(), Projectile.damage, 0, Projectile.owner, Projectile.type, Main.rand.Next(12, 24), player.ownedProjectileCounts[ModContent.ProjectileType<ShadowTentacles>()]);

        }
        public override SoundStyle Sound()
        {
            SoundStyle sound = SoundID.Item43;
            sound.Pitch = 1f;
            sound.Volume = 0.5f;
            return sound;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            ////

            Projectile.DProj().Times[0] += 0.05f;
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
            Main.spriteBatch.Draw(Staff, Projectile.Center - Main.screenPosition, null, lightColor, rot, Staff.Size() / 2, Projectile.scale, sprite, 0f);
            Color color = new Color(81, 6, 233, 0) * 0.5f;
            color.A = 100;


            Vector2 vector = Projectile.Center - Main.screenPosition + Projectile.velocity.PerfectNormalize() * 22;
            //光球渲染
            Main.spriteBatch.Draw(VoidStar, vector, null, color * 0.4f, Projectile.rotation - StaffRot, VoidStar.Size() / 2, new Vector2(0.5f, 1.1f) * Circle, 0, 0f);

            texture = DDTextures.Circle[9].Value;
            color = new Color(81, 6, 233, 0);
            //法阵
            DDHelper.Compression(texture, color, Projectile.rotation - StaffRot, Projectile.Opacity, new Vector2(5, 1), Projectile.direction, Projectile.DProj().Times[0], BlendState.Additive);

            Main.spriteBatch.Draw(texture, vector, null, color, 0f, Utils.Size(texture) / 2, Circle / 4, 0, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}