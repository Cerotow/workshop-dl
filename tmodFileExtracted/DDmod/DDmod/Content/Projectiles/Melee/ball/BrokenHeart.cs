using Terraria;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Melee.ball
{
	public class BrokenHeart : ModProjectile
	{
		public static Asset<Texture2D> HeartChain;
		public static Asset<Texture2D> Glow;
		public override void Load()
		{
			HeartChain = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Melee/ball/HeartChain");
			Glow = ModContent.Request<Texture2D>(Texture+"_Glow");
		}
		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 4;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

		public override void SetStaticDefaults()
		{
		}
		public override void AI()
		{
			Player player = Projectile.Player();
			bool channeling = player.channel && !player.noItems && !player.CCed && !player.dead && player.Dplayer().ForbiddenToAttack == 0;
			if (Projectile.originalDamage == 0)
			{
				Projectile.originalDamage = Projectile.damage;
			}
			if (!channeling && Projectile.ai[0] == 0)
			{
				if (Projectile.ai[1] > 100)
				{
					Projectile.ai[0] = 1;
					if (Projectile.DProj().MouseWorld == Vector2.Zero && Projectile.owner == Main.myPlayer)
					{
						Projectile.DProj().MouseWorld = Main.MouseWorld;
						Projectile.Center = player.Center;
						Projectile.DProj().vector[0] = Projectile.DProj().MouseWorld - Projectile.Center;
						Projectile.damage = (int)(Projectile.originalDamage * (Projectile.ai[1] / 100));
						Projectile.ClearInvincibleFrame();
                        Projectile.netUpdate = true;
                    }
				}
			}
			if (Projectile.ai[0] == 0)
            {
                player.heldProj = Projectile.whoAmI;
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
				Projectile.velocity = Projectile.velocity.PerfectNormalize().RotatedBy(Projectile.ai[1]/500 * player.direction) * 13;
				if (Projectile.velocity.Y > 0)
				{
					Projectile.Center = player.Center + new Vector2(Projectile.velocity.X * 2, Projectile.velocity.Y / 2);
				}
				else
				{
					Projectile.Center = player.Center + new Vector2(Projectile.velocity.X * 2, Projectile.velocity.Y * 1.3f);
				}
				if(Projectile.ai[1]<200)
                {
					Projectile.ai[1]++;
				}
			}
			else if (Projectile.ai[0] == 1)
			{
				Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
				Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize() * (Projectile.ai[1]/5);
				Projectile.tileCollide = true;
				player.ChangeDir(Projectile.direction);
				if ((Projectile.Center - player.Center).Length() > Projectile.ai[1]*3)
				{
					Projectile.ai[0] = 2;
					Projectile.damage = Projectile.originalDamage;
					Projectile.ClearInvincibleFrame();
				}
			}
			else if (Projectile.ai[0] == 2)
			{ 
				Projectile.rotation = (-Projectile.velocity).ToRotation() - MathHelper.PiOver2;
				player.ChangeDir(-Projectile.direction);
				Projectile.velocity = (player.Center - Projectile.Center).PerfectNormalize() * 14;
				if ((Projectile.Center - player.Center).Length() < 12)
				{
					if (Projectile.frame >= 3)
                    {
                        if (player.whoAmI == Main.myPlayer)
                            player.Heal(5);
					}
					Projectile.Kill();
				}
				Projectile.tileCollide = false;
			}
			else
            {
				Projectile.localAI[0]++;
				if(Projectile.localAI[0]>40)
                {
					Projectile.ai[0] = 2;
                }
				Projectile.velocity.Y+=0.1f;
				Projectile.velocity.X *= 0.95f;
				Projectile.rotation += Projectile.velocity.X * 0.02f;
				if (Projectile.velocity.X > 0)
				{
					Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.02f;
				}
				else
				{
					Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.02f;
				}
            }
            if (Projectile.frame >= 3)
            {
                Projectile.damage = 0;
            }
            player.itemTime = 5;
			player.itemAnimation = 5;
			float v = 0;
			if (player.direction == -1) v = 3.14f;
			player.itemRotation = Projectile.velocity.ToRotation() + v - player.fullRotation;
        }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X * 0.3f;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.X = oldVelocity.X * 0.6f;
				Projectile.velocity.Y = -oldVelocity.Y * 0.3f;
			}
			Projectile.ai[0] = 3;
			return false;
		}
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
		{
			if (Projectile.frame == 2)
			{
				for (int a = 3; a < 5; a++)
				{
					NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi))/3, ModContent.ProjectileType<FragmentOfHeart>(), Projectile.damage / 3, Projectile.knockBack / 3, Projectile.owner, a);
				}
				Projectile.frame++;
			}
			if (Projectile.frame == 1)
			{
				for (int a = 0; a < 3; a++)
				{
					NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) / 3, ModContent.ProjectileType<FragmentOfHeart>(), Projectile.damage / 3, Projectile.knockBack / 3, Projectile.owner, a);
				}
				Projectile.frame++;
			}
			if (Projectile.frame == 0)
			{
				NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(-1, 1)) / 3, ModContent.ProjectileType<FragmentOfHeart>(), Projectile.damage / 3, Projectile.knockBack / 3, Projectile.owner, 5);
				Projectile.frame++;
			}
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 vector = Projectile.Size / 2;
			for (int W = 0; W < (Projectile.Player().Center - Projectile.Center).Length() / HeartChain.Width(); W++)
			{
				//位置                                             判断长度和方向
				Vector2 Center = Projectile.Center + ((Projectile.Player().Center - Projectile.Center).PerfectNormalize() * (HeartChain.Width() * W)) - Main.screenPosition;
				//方向
				float RO = (Projectile.Player().Center - Projectile.Center).ToRotation();
				if (W < (Projectile.Player().Center - Projectile.Center).Length() / HeartChain.Width() - 1)
				{
					//绘制
					Main.spriteBatch.Draw(HeartChain.Value, Center, null, lightColor, RO, HeartChain.Size() / 2, 1, 0, 0f);
				}
				else
				{
					//绘制
					Rectangle? rectangle = new Rectangle?(new Rectangle(0, 0, (int)(HeartChain.Width() - (HeartChain.Width() - ((Projectile.Player().Center - Projectile.Center).Length() % HeartChain.Width()))), HeartChain.Height()));

					Main.spriteBatch.Draw(HeartChain.Value, Center, rectangle, lightColor, RO, HeartChain.Size() / 2, 1, 0, 0f);
				}
			}
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int F = Main.projFrames[Projectile.type];
			Rectangle? r = new Rectangle?(new Rectangle(0, texture.Height / F * Projectile.frame, texture.Width, texture.Height / F));


			if (Projectile.frame >= 3)
			{
				Lighting.AddLight(Projectile.Center, new Color(255, 100, 100).ToVector3());
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, r, lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / F/2) / 2, Projectile.scale, 0, 0f);
				Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 100, 100, 0)*0.7F, Projectile.rotation, new Vector2(Glow.Width(), Glow.Height() / 2) / 2, Projectile.scale*1.1F, 0, 0f);
				Main.spriteBatch.Draw(Glow.Value, Projectile.Center - Main.screenPosition, null, new Color(255, 100, 100, 0)*0.7F, Projectile.rotation, new Vector2(Glow.Width(), Glow.Height() / 2) / 2, Projectile.scale*1.1F, 0, 0f);
			}
			else
            {
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, r, lightColor, Projectile.rotation, new Vector2(texture.Width, texture.Height / F) / 2, Projectile.scale, 0, 0f);
			}
			if (Projectile.ai[0] == 0)
			{
				DDHelper.DrawExpanded(Main.GameViewMatrix.TransformationMatrix, Main.spriteBatch, Projectile.Player().Center - Main.screenPosition + new Vector2(0, -50), 0.5f, Projectile.ai[1] / 200 * 0.5f, new Color(155, 50, 50, 0), new Color(255, 50, 50, 0), 3F, 0.6f);
				DynamicSpriteFontExtensionMethods.DrawString(
				  Main.spriteBatch,
				  FontAssets.MouseText.Value,
				  (int)(Projectile.ai[1] / 200 * 100) + "%",
				  Projectile.Player().Center - Main.screenPosition - new Vector2(0, 38),
				   new Color(255, 0, 0, 0), 0f,
				  ChatManager.GetStringSize(FontAssets.MouseText.Value, (int)(Projectile.ai[1] / 200 * 100) + "%", Vector2.One) / 2,
				  0.75F, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}