using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace DDmod.Content.Projectiles.Ranged
{
	public class 蜂巢箭 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.timeLeft = 600;
		}

		public override void SetStaticDefaults()
		{

		}
		float SP;
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if(Projectile.DProj().track==1)
            {
				Projectile.velocity /= 2;
            }
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
		}
        public override void OnKill(int timeLeft)
		{
			if (Main.myPlayer == Projectile.owner)
			{

				for (int A = 0; A < Main.rand.Next(2, 6); A++)
				{
					if (Main.player[Projectile.owner].strongBees)
					{
						NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)), new Vector2(Main.rand.Next(2, 4)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 566, (int)(Projectile.damage * 0.4f), 0);
					}
                    else
					{
						NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)), new Vector2(Main.rand.Next(2, 4)).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)), 181, (int)(Projectile.damage * 0.25f), 0);
					}
				}
			}
			for (int A = 0; A < 20; A++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 147, 0f, 0f, 10);
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
		}
	}
}
