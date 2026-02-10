namespace DDmod.Content.Projectiles.Melee.ball
{
	public class FragmentOfHeart : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 50;
			Projectile.DamageType = DamageClass.Melee;
			Main.projFrames[Projectile.type] = 6;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		public override void SetStaticDefaults()
		{
		}
		public override void AI()
		{
			Projectile.rotation += Projectile.velocity.X * 0.03f;
			if (Projectile.velocity.X > 0)
			{
				Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.03f;
			}
			else
			{
				Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.03f;
			}

			if (Projectile.velocity.Y < 10)
			{
				Projectile.velocity.Y += 0.1F;
			}
		}
		public override void OnKill(int timeLeft)
		{
			NewDustChange(20, Projectile.Center, new Vector2(1), 12, 1, 2, false, 1.1F);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, (int)(TextureAssets.Projectile[Projectile.type].Height() / Main.projFrames[Projectile.type]*Projectile.ai[0]), TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height() / Main.projFrames[Projectile.type])), lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale, 0, 0f);
			return false;
		}
	}
}