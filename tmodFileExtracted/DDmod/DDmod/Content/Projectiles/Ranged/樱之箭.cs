using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Ranged
{
	public class 樱之箭 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.timeLeft = 600;
		}

		public override void SetStaticDefaults()
		{
		}
		public override void AI()
		{
			if (Main.rand.NextBool(5))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<樱花粒子>());

				Main.dust[dust].scale = 0.6f;
				Main.dust[dust].velocity /= 10f;
			}
		}
		public override void OnKill(int timeLeft)
		{
			for (int A = 0; A < 7; A++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<樱花粒子>());

				Main.dust[dust].scale = 0.6f;

			}
		}
		public override bool? CanHitNPC(NPC target)
		{
			return base.CanHitNPC(target);
		}
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
		{
		}
	}
}
