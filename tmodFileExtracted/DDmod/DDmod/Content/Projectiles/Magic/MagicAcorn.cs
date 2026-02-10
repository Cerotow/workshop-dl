using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic
{
    public class MagicAcorn : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Main.projFrames[Projectile.type] = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;

        }
        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 10;
            }
            else
            {
                Projectile.alpha = 0;
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.velocity *= 0.5f;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            NewDustChange(40, Projectile.position, Projectile.Size, ModContent.DustType<生命粒子>(), 1F, 3F);
            if (Main.myPlayer == Projectile.owner)
                NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(0, 10), Vector2.Zero, ModContent.ProjectileType<MagicSapling>(), Projectile.damage / 3, 0, Projectile.owner, Main.rand.Next(3));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(texture.Width / 2, texture.Height / 2);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition;
                Color color = new Color(0, 255, 0, 50) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f)* Projectile.Visible();
                Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.rotation, texture.Size() / 2, Projectile.scale * 1.4f * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), 0, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, null, lightColor * Projectile.Visible(), Projectile.rotation, vector, Projectile.scale, 0, 0f);
            return false;
        }
    }
}