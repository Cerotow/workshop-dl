using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace DDmod.Content.Projectiles.Ranged
{
	public class 仙人掌箭 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
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
		float SP;
		public override void AI()
		{
			if(SP == 0)
            {
				SP = Projectile.velocity.Length();
            }
			if (Projectile.ai[0] == 2)
			{
				Projectile.aiStyle = -1;
				Projectile.velocity = Vector2.Zero;
				Projectile.tileCollide = false;
				if (Main.npc[(int)Projectile.ai[1]].active)
				{
					//Vector2 PO = Vector;
					//PO = Utils.RotatedBy(PO, projectile.localAI[0]+Main.npc[(int)projectile.ai[1]].rotation, default(Vector2));
					Projectile.Center = Main.npc[(int)Projectile.ai[1]].Center +Vector.RotatedBy(-(Projectile.localAI[0] - Main.npc[(int)Projectile.ai[1]].rotation));
					Projectile.rotation = R-(Projectile.localAI[0] - Main.npc[(int)Projectile.ai[1]].rotation);
					Projectile.knockBack = 0;
				}
				else
				{
					Projectile.Kill();
				}
			}
            else
            {
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
				if (Projectile.DProj().track>10)
				{
					if (Projectile.ai[2]<0.3F)
                    {
						Projectile.ai[2] += 0.01F;
                    }
					if (Projectile.velocity.Y < 10)
						Projectile.velocity.Y += Projectile.ai[2];
					if (Projectile.velocity.X > 0)
						Projectile.velocity.X -= Projectile.velocity.X/80;
					if (Projectile.velocity.X < 0)
						Projectile.velocity.X -= Projectile.velocity.X / 80;

				}


			}
			if(Projectile.damage == 0)
            {
				Projectile.Kill();
            }
		}
        public override void OnKill(int timeLeft)
		{
			for (int A = 0; A < 20; A++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 40, 0f, 0f, 10, new Color(200, 200, 200, 160), 1f);
				if (Main.rand.NextBool(2))
				{
					Main.dust[dust].scale = 0.1f;
					Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.02f;
				}
			}
		}
        float R;
		Vector2 Vector;
		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.ai[0] == 2)
			{
				if (Main.npc[(int)Projectile.ai[1]].active)
				{
					return target.whoAmI == Projectile.ai[1];

				}
			}
			return base.CanHitNPC(target);
		}
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
		{
			Projectile.damage = (int)(Projectile.damage*0.66F);
			if (Projectile.ai[0] != 2)
			{
				Projectile.ai[0] = 2;
				Projectile.ai[1] = target.whoAmI;
				Vector = Projectile.Center - target.Center+Projectile.velocity.PerfectNormalize()*8;
				Projectile.localAI[0] = target.rotation;
				R = Projectile.rotation;
				Projectile.netUpdate = true;
			}
		}
	}
}
