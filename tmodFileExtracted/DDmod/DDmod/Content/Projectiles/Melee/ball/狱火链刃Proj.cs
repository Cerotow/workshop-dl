namespace DDmod.Content.Projectiles.Melee.ball
{
	public class 狱火链刃Proj : ModProjectile
	{
		public override void Load()
		{
		}
		public override void SetDefaults()
		{
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 10;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}

		public override void SetStaticDefaults()
		{

		}
		Vector2[] Center = new Vector2[20];
		public override void AI()
        {
            Player player = Projectile.Player();
			if (Projectile.originalDamage == 0)
			{
				Projectile.originalDamage = Projectile.damage;
			}
			if (!Projectile.DProj().Bool[0])
			{
				if (Projectile.ai[0] == 0)
				{
					Projectile.ai[0] = 0;
				}
				Projectile.ai[0] += 0.5F * Projectile.scale;
			}
			if (Projectile.DProj().Bool[0])
			{
				Projectile.ai[0] -= 0.5F * Projectile.scale;
				if ((Projectile.Center - player.Center).Length() < 100)
				{
					Projectile.Kill();
				}
			}
			Projectile.Center = player.Center + Projectile.velocity.PerfectNormalize() * 20 + Projectile.velocity * Projectile.ai[0];
			if (Projectile.ai[0] > 60 * Projectile.scale)
			{
				Projectile.DProj().Bool[0] = true;
				NewDustChange(60, Projectile.Center - Projectile.Size / 2, Projectile.Size, 6, 0, 6, Scale: 2);
			}
			for (int W = 1; W < 20; W++)
			{
				//位置                                             判断长度和方向
				Center[W] = player.Center + Projectile.velocity.PerfectNormalize() * 20 + Projectile.velocity * (Projectile.ai[0] / 20 * W);
				if (Main.rand.NextBool(10))
					NewDustChange(1, Center[W] - Projectile.Size / 2, Projectile.Size, 6, 0, 2, Scale: 0.75F);
			}
			Projectile.ProjScale();

			player.heldProj = Projectile.whoAmI;
			player.itemTime = 5;
			player.itemAnimation = 5;
			float v = 0;
			if (player.direction == -1) v = 3.14f;

			player.itemRotation = Projectile.velocity.ToRotation() + v - player.fullRotation;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = oldVelocity;
			if (!Projectile.DProj().Bool[0])
			{
				NewDustChange(60, Projectile.Center - Projectile.Size / 2, Projectile.Size, 6, 0, 6, Scale: 2);
				Projectile.DProj().Bool[0] = true;
			}
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Rectangle[] vectors = new Rectangle[Center.Length];
			bool B = false;
			for (int A = 0; A < Center.Length; A++)
			{
				Vector2 Size = Projectile.Size / 2;
				vectors[A] = new Rectangle((int)(Center[A].X - Size.X), (int)(Center[A].Y - Size.Y), Projectile.width, Projectile.height);
				if (vectors[A].Intersects(targetHitbox))
				{
					B = true;
				}
			}
			return new bool?(B);
		}
		public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<地狱之火>(), 300);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.White;
			Color color = new Color(253, 74, 3, 0) * 0.5F;
			Player player = Projectile.Player();
			//方向
			float RO = (Projectile.Player().Center + Projectile.velocity.PerfectNormalize() * 20 - Projectile.Center).ToRotation() - MathHelper.PiOver2;
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 vector = new Vector2(22, 16);

			for (int W = 0; W < (Projectile.Player().Center + Projectile.velocity.PerfectNormalize() * 20 - Projectile.Center).Length() / vector.Y; W++)
			{
				//位置                                             判断长度和方向
				Vector2 Center = Projectile.Center + ((Projectile.Player().Center + Projectile.velocity.PerfectNormalize() * 20 - Projectile.Center).PerfectNormalize() * (vector.Y * W)) - Main.screenPosition;
				if (W < (Projectile.Player().Center + Projectile.velocity.PerfectNormalize() * 20 - Projectile.Center).Length() / vector.Y - 1)
				{
					//绘制
					Rectangle? rectangle = new Rectangle?(new Rectangle(0, 30, (int)vector.X, (int)(vector.Y)));
					Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, RO, vector / 2, Projectile.scale, 0, 0f);
					Main.spriteBatch.Draw(texture, Center, rectangle, color, RO, vector / 2, Projectile.scale, 0, 0f);
				}
				else
				{
					//绘制
					Rectangle? rectangle = new Rectangle?(new Rectangle(0, 30, (int)vector.X, (int)(vector.Y - (vector.Y - ((Projectile.Player().Center - Projectile.Center).Length() % vector.Y)))));
					Main.spriteBatch.Draw(texture, Center, rectangle, lightColor, RO, vector / 2, Projectile.scale, 0, 0f);
					Main.spriteBatch.Draw(texture, Center, rectangle, color, RO, vector / 2, Projectile.scale, 0, 0f);
				}
			}
			for (int W = 1; W < 20; W++)
			{
				Rectangle? rectangle = new Rectangle?(new Rectangle(0, 48, (int)vector.X, (int)(vector.Y)));
				Main.spriteBatch.Draw(texture, Center[W] - Main.screenPosition, rectangle, lightColor, RO, vector / 2, Projectile.scale, 0, 0f);
				Main.spriteBatch.Draw(texture, Center[W] - Main.screenPosition, rectangle, color, RO, vector / 2, Projectile.scale, 0, 0f);
			}
			int F = Main.projFrames[Projectile.type];
			vector = new Vector2(22, 28);
			Rectangle? r = new Rectangle?(new Rectangle(0, 0, (int)vector.X, (int)vector.Y));
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, r, lightColor, RO, vector / 2, Projectile.scale, 0, 0f);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, r, color, RO, vector / 2, Projectile.scale, 0, 0f);
			vector = new Vector2(22, 38);
			Rectangle? re = new Rectangle?(new Rectangle(0, 66, (int)vector.X, (int)vector.Y));
			Main.spriteBatch.Draw(texture, player.Center + Projectile.velocity.PerfectNormalize() * 18 - Main.screenPosition, re, lightColor, RO, vector / 2, Projectile.scale, 0, 0f);

			return false;
		}
	}
}