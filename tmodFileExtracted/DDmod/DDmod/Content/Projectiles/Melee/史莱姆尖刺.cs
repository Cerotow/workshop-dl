using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace DDmod.Content.Projectiles.Melee
{
	public class 史莱姆尖刺 : ModProjectile
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
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;

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

				Projectile.velocity.X *= 0.98F;


            }
			if (Projectile.damage == 0)
			{
				Projectile.Kill();
			}
		}
        public override void OnKill(int timeLeft)
		{
			for (int A = 0; A < 12; A++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,4, 0f, 0f, 100, new Color(78, 136, 255, 105), 0.8f);
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
			Projectile.damage = (int)(Projectile.damage*0.8F);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color2 = lightColor * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length / 2f);
                Main.spriteBatch.Draw(texture, vector2, null, color2, Projectile.rotation, texture.Size() / 2, Projectile.scale* ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
