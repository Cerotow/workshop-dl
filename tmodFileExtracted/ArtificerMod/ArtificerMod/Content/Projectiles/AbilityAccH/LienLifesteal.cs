using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Projectiles.AbilityAccH
{
	public class LienLifesteal : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 6;
		}

		// Adapted from vampire knives lifesteal
		public override void AI()
		{
			Player target = Main.player[(int)Projectile.ai[0]];

			float targetDistY = target.Center.X - Projectile.Center.X;
			float targetDistX = target.Center.Y - Projectile.Center.Y;
			float distToTarget = Vector2.Distance(Projectile.Center, target.Center);

			if (Projectile.Hitbox.Intersects(target.Hitbox))
			{
				if (Projectile.owner == Main.myPlayer && !Main.player[Main.myPlayer].moonLeech)
				{
					int healAmount = (int)Projectile.ai[1];
					target.HealEffect(healAmount, broadcast: false);
					target.statLife += healAmount;
					if (target.statLife > target.statLifeMax2)
					{
						target.statLife = target.statLifeMax2;
					}
					NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, target.whoAmI, healAmount);

					if (Projectile.ai[2] == 0)
					{
                        for (int i = 0; i < 255; i++)
                        {
							if (i != Projectile.ai[0] && Main.player[i].active && !Main.player[i].dead &&
								((!Main.player[Projectile.owner].hostile && !Main.player[i].hostile)
								|| Main.player[Projectile.owner].team == Main.player[i].team)
								&&
								Vector2.Distance(Main.player[i].Center, Projectile.Center) < 1000f
								&& Projectile.owner == Main.myPlayer
								&& Main.rand.NextBool(5))
                            {
                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Projectile.velocity * 0f, ModContent.ProjectileType<LienLifesteal>(), 0, 0f, Projectile.owner, i, Main.rand.NextBool(5) ? 2 : 1, 1);
                            }
                        }
                    }
                }
				Projectile.Kill();
			}

			distToTarget = 4f / distToTarget;
			targetDistY *= distToTarget;
			targetDistX *= distToTarget;
			Projectile.velocity.X = (Projectile.velocity.X * 15f + targetDistY) / 16f;
			Projectile.velocity.Y = (Projectile.velocity.Y * 15f + targetDistX) / 16f;

			for (int i = 0; i < 2; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height,
											 DustID.Phantasmal, 0f, 0f, 0, default, 0.9f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity *= 0f;
				Main.dust[dustIndex].position.X -= Projectile.velocity.X * 0.334f * (float)i;
				Main.dust[dustIndex].position.Y -= (0f - Projectile.velocity.Y * 0.334f) * (float)i;
			}
		}
	}
}