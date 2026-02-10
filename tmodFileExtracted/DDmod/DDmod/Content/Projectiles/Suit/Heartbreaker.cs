namespace DDmod.Content.Projectiles.Suit
{
    public class Heartbreaker : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 0;
			Projectile.alpha = 0;
		}

		public override void SetStaticDefaults()
		{
		}
		bool LD;
		public override void AI()
		{
			Projectile.rotation += Projectile.velocity.X * 0.08f;
			if (Projectile.velocity.X > 0)
			{
				Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.03f;
			}
			else
			{
				Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.03f;
			}
			if (Projectile.velocity.Y < 5)
			{
				Projectile.velocity.Y += 0.1F;
			}
			if(LD)
            {
				Projectile.velocity.X *= 0.92f;
				Projectile.velocity.X += Main.windSpeedCurrent/20;
			}
			//遍历玩家
			for (int A = 0; A < 255; A++)
			{
				Player player = Main.player[A];
				//能踩的碰撞箱
				Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
				//玩家脚碰撞箱
				Rectangle playerRectangle = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
				if(rectangle.Intersects(playerRectangle))
                {
                    if (player.whoAmI == Main.myPlayer)
                        player.Heal(15);
					Projectile.active = false;
					return;
                }
			}
			Lighting.AddLight(Projectile.Center, 0.88f * 0.5f, 0.68f * 0.5f, 1.97f * 0.5f);
		}
		public override void OnKill(int timeLeft)
		{
			if (Main.netMode != 2)
			{
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-1, 1)), Mod.Find<ModGore>("Heartbreaker").Type, 1f);
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-1, 1)), Mod.Find<ModGore>("Heartbreaker2").Type, 1f);
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-1, 1)), Mod.Find<ModGore>("Heartbreaker3").Type, 1f);
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-1, 1)), Mod.Find<ModGore>("Heartbreaker4").Type, 1f);
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!LD)
			{
				Projectile.timeLeft = 180;
			}
			Projectile.velocity.Y = 0;
			LD = true;
			return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			fallThrough = false;

			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height())), Color.White, Projectile.rotation, TextureAssets.Projectile[Projectile.type].Size() / 2, Projectile.scale, 0, 0f);
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height())), new Color(255, 0, 0, 0), Projectile.rotation, TextureAssets.Projectile[Projectile.type].Size() / 2, Projectile.scale, 0, 0f);
			return false;
		}
	}
}
