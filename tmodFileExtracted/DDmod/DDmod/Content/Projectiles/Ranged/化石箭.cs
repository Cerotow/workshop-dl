using DDmod.Content.Buffs.DeBuffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Projectiles.Ranged
{
	public class 化石箭 : ModProjectile
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
		}
        public override void OnKill(int timeLeft)
		{
			for (int A = 0; A < 20; A++)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 0, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f,0,new Color(255,207,130),1.3F);
			}
		}
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
		{
			if ( !target.Dnpc().BossPhysique)
			{
				target.AddBuff(ModContent.BuffType<Fossil>(), 30);
				if (target.realLife > 0)
				{
					Main.npc[target.realLife].AddBuff(ModContent.BuffType<Fossil>(), 30);
				}
			}
		}
        public override bool PreDraw(ref Color lightColor)
        {
			DDHelper.绘制偏移头部(TextureAssets.Projectile[Type].Value, Projectile, lightColor);
            return false;
        }
    }
}