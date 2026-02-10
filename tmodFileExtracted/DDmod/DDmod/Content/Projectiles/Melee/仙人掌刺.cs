using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace DDmod.Content.Projectiles.Melee
{
	public class 仙人掌刺 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.timeLeft = 600;
		}

		public override void SetStaticDefaults()
		{

		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.DProj().track > 10)
			{
				if (Projectile.ai[2] < 0.3F)
				{
					Projectile.ai[2] += 0.01F;
				}
				if (Projectile.velocity.Y < 10)
					Projectile.velocity.Y += Projectile.ai[2];
				if (Projectile.velocity.X > 0)
					Projectile.velocity.X -= Projectile.velocity.X / 80;
				if (Projectile.velocity.X < 0)
					Projectile.velocity.X -= Projectile.velocity.X / 80;

			}
			if (Projectile.damage == 0)
			{
				Projectile.Kill();
			}
		}
        public override void OnKill(int timeLeft)
		{
			for (int A = 0; A < 8; A++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 40, 0f, 0f, 10, new Color(200, 200, 200, 160), 0.6f);
				if (Main.rand.NextBool(2))
				{
					Main.dust[dust].scale = 0.1f;
					Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.02f;
				}
			}
		}
		public override bool? CanHitNPC(NPC target)
		{
			return base.CanHitNPC(target);
		}
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
		{
			Projectile.damage = (int)(Projectile.damage*0.66F);
		}
	}
}
