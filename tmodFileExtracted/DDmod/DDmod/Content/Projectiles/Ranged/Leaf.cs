using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace DDmod.Content.Projectiles.Ranged
{
	public class Leaf : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.timeLeft = 1800;
			Projectile.scale = 1f;
			Projectile.DamageType = DamageClass.Ranged;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 8;

		}

		public override void AI()
		{
			if (Projectile.ai[1] == 0)
			{
				Projectile.frameCounter++;
				if (Projectile.frameCounter >= 10)
				{
					Projectile.frame = Projectile.frame + 1;
					if (Projectile.frame >= 4)
					{
						Projectile.frame = 0;
					}
					Projectile.frameCounter = 0;
				}
			}
			else
            {
				Projectile.frameCounter++;
				if (Projectile.frameCounter >= 10)
				{
					Projectile.frame = Projectile.frame + 1;
					if (Projectile.frame >= 8)
					{
						Projectile.frame = 4;
					}
					Projectile.frameCounter = 0;
				}
			}
			Projectile.rotation = Projectile.velocity.X*0.5f;
			int A = 5;
			if (Main.player[Projectile.owner].ZoneSandstorm)
            {
				A += 100;
			}
			Projectile.velocity.X = Main.windSpeedCurrent* A;
		}
    }
}