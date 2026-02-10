using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace DDmod.Content.Projectiles.Magic
{
	public class 魔法仙人掌刺 : ModProjectile
	{
		public override void SetDefaults()
		{
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.frame = Main.rand.Next(3);
			Projectile.extraUpdates = 1;
		}

		public override void SetStaticDefaults()
		{

		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if(Projectile.DProj().track==1)
            {
				for (int A = 0; A < 8; A++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 40, 0f, 0f, 10, new Color(200, 200, 200, 160), 0.6f);
					if (Main.rand.NextBool(2))
					{
						Main.dust[dust].scale = 0.3f;
					}
				}
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
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size/2- Main.screenPosition;
				Color color = new Color(80, 132, 38, 40) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 1.2f;
				Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(texture.Width / 3 * Projectile.frame, 0, texture.Width / 3, texture.Height)), color, Projectile.oldRot[i], new Vector2(texture.Width / 6, texture.Height / 2), Projectile.scale * 1.2F, 0, 0f);
				Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(texture.Width / 3 * Projectile.frame, 0, texture.Width / 3, texture.Height)), color, Projectile.oldRot[i], new Vector2(texture.Width / 6, texture.Height / 2), Projectile.scale * 0.8F, 0, 0f);
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(texture.Width / 3 * Projectile.frame, 0, texture.Width / 3, texture.Height)), lightColor, Projectile.rotation, new Vector2(texture.Width / 3, texture.Height) / 2, Projectile.scale, 0, 0);
            return false;
        }
    }
}
