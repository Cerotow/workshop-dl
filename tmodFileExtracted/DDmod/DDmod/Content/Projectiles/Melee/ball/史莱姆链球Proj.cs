using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Projectiles.Melee.ball
{
	public class 史莱姆链球Proj : ModProjectile
	{
		public static Asset<Texture2D> HeartChain;
		public override void Load()
		{
			HeartChain = ModContent.Request<Texture2D>(Texture + "_Chain");
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
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
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
						Projectile.netUpdate = true;
						Projectile.damage = (int)(Projectile.originalDamage * (Projectile.ai[1] / 100));
						Projectile.ClearInvincibleFrame();
					}
				}
			}
			if (Projectile.ai[0] == 0)
			{
				player.heldProj = Projectile.whoAmI;
				Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
				Projectile.velocity = Projectile.velocity.PerfectNormalize().RotatedBy(Projectile.ai[1] / 500 * player.direction) * (5+15 * Projectile.ai[1]/200);
				if (Projectile.velocity.Y > 0)
				{
					Projectile.Center = player.Center + new Vector2(Projectile.velocity.X * 2, Projectile.velocity.Y / 2);
				}
				else
				{
					Projectile.Center = player.Center + new Vector2(Projectile.velocity.X * 2, Projectile.velocity.Y * 1.3f);
				}
				if (Projectile.ai[1] < 200)
				{
					Projectile.ai[1]++;
				}
				JCHC += 0.02f;
				if(Main.rand.NextBool(300))
				{
					JCHC += 5F;
				}
                Projectile.damage = Projectile.originalDamage/2;

                player.ChangeDir(player.Dplayer().MouseWorld.X-player.Center.X>0?1:-1);
            }
			else if (Projectile.ai[0] == 1)
			{
				Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
				Projectile.velocity = Projectile.DProj().vector[0].PerfectNormalize() * (Projectile.ai[1] / 5);
				Projectile.tileCollide = true;
				player.ChangeDir(Projectile.direction);
				if ((Projectile.Center - player.Center).Length() > Projectile.ai[1] * 3)
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
					Projectile.Kill();
				}
				Projectile.tileCollide = false;
			}
			else
			{
				Projectile.localAI[0]++;
				if (Projectile.localAI[0] > 40)
				{
					Projectile.ai[0] = 2;
				}
				Projectile.velocity.Y += 0.1f;
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
				Projectile.velocity.X = -oldVelocity.X * 0.8f;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.X = oldVelocity.X * 0.8f;
				Projectile.velocity.Y = -oldVelocity.Y * 0.8f;
            }
            Projectile.ai[0] = 3;

            return false;
		}
		float JCHC = 40; 
		int JC = 40; 
		public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
		{
			if (Projectile.ai[0] == 0)
			{
				for (int A = 0; A < Main.rand.Next(1, 4); A++)
                {
                    if (JCHC > 0 && Projectile.damage / 4 > 0)
                    {
                        JCHC--;
                        Projectile.NewProjectileChange(Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0.1F, 0.25F), ModContent.ProjectileType<史莱姆尖刺>(), Projectile.damage / 4, 0, -1, 0, 0, 0, 0.25F);
                    }}
			}
			else
			{
				for (int A = 0; A < Main.rand.Next(3, 7); A++)
				{
					if (JC > 0&& Projectile.damage / 4>0)
					{
						JC--;
						Projectile.NewProjectileChange(Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0.1F, 0.25F), ModContent.ProjectileType<史莱姆尖刺>(), Projectile.damage / 4, 0, -1, 0, 0, 0, 0.25F);
					}
				}
                Projectile.damage = (int)(Projectile.damage * 0.8F);
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

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, 0, 0f);

			if (Projectile.ai[0] == 0)
			{
				DDHelper.DrawExpanded(Main.GameViewMatrix.TransformationMatrix, Main.spriteBatch, Projectile.Player().Center - Main.screenPosition + new Vector2(0, -50), 0.5f, Projectile.ai[1] / 200 * 0.5f, new Color(100, 100, 255, 0), new Color(100, 100, 255, 0), 3F, 0.6f);
				DynamicSpriteFontExtensionMethods.DrawString(
				  Main.spriteBatch,
				  FontAssets.MouseText.Value,
				  (int)(Projectile.ai[1] / 200 * 100) + "%",
				  Projectile.Player().Center - Main.screenPosition - new Vector2(0, 38),
				   new Color(100, 100, 255, 0), 0f,
				  ChatManager.GetStringSize(FontAssets.MouseText.Value, (int)(Projectile.ai[1] / 200 * 100) + "%", Vector2.One) / 2,
				  0.75F, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}