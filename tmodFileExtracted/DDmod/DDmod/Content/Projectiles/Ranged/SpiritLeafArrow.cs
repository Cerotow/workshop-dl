using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Projectiles.Ranged
{
	public class SpiritLeafArrow : ModProjectile
	{
		public override void SetStaticDefaults()
        {
		}
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 1800;
			Projectile.aiStyle = 1;
		}
		public override void AI()
		{
			Projectile.ai[1]++;
			if (Projectile.ai[1] > 7&& Main.rand.NextBool(5))
			{
				Projectile.ai[1] = 0;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center, new Vector2(0, 2), ModContent.ProjectileType<Leaf>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner,0, Main.rand.Next(2));
			}
		}
        public override void OnKill(int timeLeft)
		{
			for (int A = 0; A < 10; A++)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 7, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			}
		}
    }
}