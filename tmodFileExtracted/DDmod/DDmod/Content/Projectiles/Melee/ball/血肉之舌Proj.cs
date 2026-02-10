namespace DDmod.Content.Projectiles.Melee.ball
{
	public class 血肉之舌Proj : ModProjectile
	{
		public override void Load()
		{
		}
		public override void SetDefaults()
		{
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 3;
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
			if(Projectile.velocity.Length()>16F/3)
			{
				Projectile.velocity = Projectile.velocity.PerfectNormalize() * 16F/3;

            }
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
				Projectile.ai[0] += 1.5F * Projectile.scale;
            }
			if (!Projectile.DProj().Bool[0] && Projectile.ai[2] != 0)
			{
				NewDustChange(60, Projectile.Center - Projectile.Size / 2, Projectile.Size, 5, 0, 6, Scale: 1);

				Projectile.DProj().Bool[0] = true;
			}
			for(int A=0;A<Main.item.Length;A++)
			{
				if (Main.item[A].active)
				{
                    if(Main.item[A].getRect().Intersects(Projectile.getRect()))
					{
						Main.item[A].Center = Projectile.Center;
						Main.item[A].velocity = -Projectile.velocity;
                    }

                }
			}
            if (Projectile.DProj().Bool[0])
			{
				if(Projectile.ai[2]>0&& Projectile.ai[0]>10)
				{
					NPC npc = Main.npc[(int)Projectile.ai[2] - 1];
					npc.Center = Projectile.Center;
					npc.velocity = Projectile.velocity;
				}
				Projectile.ai[0] -= 1.5F * Projectile.scale;
				if ((Projectile.Center - player.Center).Length() < 100)
				{
					Projectile.Kill();
				}
			}
			Projectile.Center = player.Center + Projectile.velocity.PerfectNormalize() * 16+ Projectile.velocity * Projectile.ai[0];
			if (Projectile.ai[0] > 60 * Projectile.scale)
            {
                if (!Projectile.DProj().Bool[0])
                {
                    NewDustChange(60, Projectile.Center - Projectile.Size / 2, Projectile.Size, 5, 0, 6, Scale: 1);

                    Projectile.DProj().Bool[0] = true;
                }
            }

            for (int W = 1; W < 20; W++)
			{
				//位置                                             判断长度和方向
				Center[W] = player.Center + Projectile.velocity.PerfectNormalize() * 25 + Projectile.velocity * (Projectile.ai[0] / 20 * W);

				if (Main.rand.NextBool(50))
					NewDustChange(1, Center[W] - Projectile.Size / 4, Projectile.Size/2, 5, 0, 2, Scale: 0.75F);
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

			Projectile.ai[2] = -1;

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
            if (Projectile.ai[2]==0&& target.knockBackResist>0&&target.Dnpc().Properties.Control)
			{
                Projectile.ai[2]=target.whoAmI+1;
				Projectile.netUpdate = true;

            }

        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Color color = new Color(253, 74, 3, 0) * 0.5F;
			Player player = Projectile.Player();
			//方向
			float RO = (Projectile.Player().Center + Projectile.velocity.PerfectNormalize() * 16 - Projectile.Center).ToRotation() - MathHelper.PiOver2;
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 vector = new Vector2(22, 16);

			for (int W = 1; W < 20; W++)
			{
				Rectangle? rectangle = new Rectangle?(new Rectangle(0, 28, (int)vector.X, (int)(vector.Y)));
				Main.spriteBatch.Draw(texture, Center[W] - Main.screenPosition, rectangle, lightColor, RO, vector / 2, Projectile.scale, 0, 0f);
			}
			int F = Main.projFrames[Projectile.type];
			vector = new Vector2(22, 26);
			Rectangle? r = new Rectangle?(new Rectangle(0, 0, (int)vector.X, (int)vector.Y));
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, r, lightColor, RO, vector / 2, Projectile.scale, 0, 0f);
			vector = new Vector2(22, 32);
			Rectangle? re = new Rectangle?(new Rectangle(0, 46, (int)vector.X, (int)vector.Y));
			Main.spriteBatch.Draw(texture, player.Center + Projectile.velocity.PerfectNormalize() * 18 - Main.screenPosition, re, lightColor, RO, vector / 2, Projectile.scale, 0, 0f);

			return false;
		}
	}
}