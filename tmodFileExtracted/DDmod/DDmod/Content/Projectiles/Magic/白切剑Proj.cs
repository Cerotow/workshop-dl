using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using DDmod.Content.Projectiles.Melee.Sword;
using Microsoft.Xna.Framework.Graphics;
using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Magic
{
	public class 白切剑Proj : ModProjectile
	{
		public override void SetDefaults()
		{
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.frame = Main.rand.Next(3);
			Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
			Projectile.tileCollide = false;
        }

		public override void SetStaticDefaults()
		{

		}
		public override void AI()
		{
			
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			if (Projectile.damage == 0)
			{
				Projectile.ai[0] = 1;

            }
			Projectile.scale = Projectile.ai[2];

            if (Projectile.ai[0] >= 1)
			{
				Projectile.ai[1] -= 0.05F;
				if(Projectile.ai[1]<=0)
				{
					Projectile.Kill();
                }
                Projectile.velocity *= 0.92F;

            }
			else
			{
				if(Projectile.ai[1]==0)
				{

                    for (int a = 0; a < 20; a++)
                    {
                        int dust = NewDust(Projectile.Center-new Vector2(4)-Projectile.velocity.PerfectNormalize()*10,0,0,ModContent.DustType<光球粒子>(), Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(255, 255, 255, 100));
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].scale = 0.8f;
                        Main.dust[dust].velocity = Projectile.velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.3F,0.3F)) * Main.rand.NextFloat(0, 4);
                        Main.dust[dust].noLightEmittence = false;
						Main.dust[dust].customData = 2;
                        GlobalDust.DustPlayerOwner[dust] = Projectile.Player().whoAmI;
                    }
                }
                if (Projectile.ai[1] <= 1)
                {
                    Projectile.ai[1] += 0.05F;

                }
				else
				{
					Projectile.tileCollide = true;

                }
                Projectile.Track(800, 20, 10, 30);
            }
			Projectile.ProjScaleChange();

		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0]++;
            Projectile.damage = 0;
			Projectile.velocity = oldVelocity/4;
            return false;
        }
        public override void OnKill(int timeLeft)
		{
		}
		public override bool? CanHitNPC(NPC target)
		{
			return Projectile.damage>0;
		}
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
		{
			Projectile.ai[0]++;
			Projectile.damage = 0;
            Vector2 vector = Main.rand.NextVector2Unit() * 60;
            int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
            Main.projectile[A].DProj().color = new Color(255,255,255,100)*0.1F;
			Main.projectile[A].scale = 0.3F ;

        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size/2- Main.screenPosition;
				Color color = new Color(255, 255, 255, 40) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * (Projectile.ai[1])*0.5F;
				Main.spriteBatch.Draw(texture, vector2, new Rectangle?(new Rectangle(texture.Width / 3 * Projectile.frame, 0, texture.Width / 3, texture.Height)), color, Projectile.oldRot[i], new Vector2(texture.Width / 6, texture.Height / 2), Projectile.scale , 0, 0f);
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(texture.Width / 3 * Projectile.frame, 0, texture.Width / 3, texture.Height)), Color.White * (Projectile.ai[1]), Projectile.rotation, new Vector2(texture.Width / 3, texture.Height) / 2, Projectile.scale, 0, 0);
            return false;
        }
    }
}
