using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Ranged
{
	public class 白切神箭 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.timeLeft = 600;
		}

		public override void SetStaticDefaults()
		{
		}
		public override void AI()
		{
			//if(Main.rand.NextBool(2))
			{

                int dust = Dust.NewDust(Projectile.Center-new Vector2(4), 0, 0, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(255, 255, 255, 0));

                Main.dust[dust].scale = .8f;
                Main.dust[dust].velocity = Projectile.velocity/2;

            }
			if(Projectile.ai[2]==1)
            {
                Projectile.hide = true;
                Projectile.scale +=0.1F;
				if(Projectile.timeLeft>3)
				{
					Projectile.timeLeft = 3;
                }
                Projectile.scale *= 1.5F;
				Projectile.ProjScaleChange();
                for (int A = 0; A < 15; A++)
                {
                    int dust = Dust.NewDust(Projectile.Center -new Vector2(4),0, 0, ModContent.DustType<光球粒子>(), 0, 0, 0, new Color(255, 255, 255, 0));

                    Main.dust[dust].scale = 1.2f;
                    Main.dust[dust].customData = 4;
                    Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(4)).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));

                }
            }
        }
		public override void OnKill(int timeLeft)
		{
			if(Main.myPlayer == Projectile.owner)
			{
			}
		}
		public override bool? CanHitNPC(NPC target)
		{
			return base.CanHitNPC(target);
		}
		public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.velocity *= 0.02f;
				Projectile.ai[2] = 1;
				int r = NewProjectile(Projectile.GetSource_FromAI(), target.Center - new Vector2(0, 400), new Vector2(0, 10), ModContent.ProjectileType<白切神剑>(), Projectile.damage * 3, 0, -1, ai2: Projectile.Center.Y);
				Main.projectile[r].localAI[1] = 3;
			}
		}
	}
}
