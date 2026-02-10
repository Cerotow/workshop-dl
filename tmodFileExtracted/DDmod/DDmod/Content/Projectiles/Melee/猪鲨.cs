using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using DDmod.Content.Dusts;
using DDmod.Content.NPCs;

namespace DDmod.Content.Projectiles.Melee
{
	public class 猪鲨 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.timeLeft = 600;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;

		}

		public override void SetStaticDefaults()
		{

		}
		float SP;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(23, 147, 234) * 0.003F);
			if (SP == 0)
			{
				SP = Projectile.velocity.Length();
			}
			if (Projectile.ai[0] == 2)
			{
				Projectile.frameCounter++;
				Projectile.frame = (Projectile.frameCounter % 6) / 3;
				Projectile.aiStyle = -1;
				Projectile.velocity = Vector2.Zero;
				Projectile.tileCollide = false;
				if (Main.npc[(int)Projectile.ai[1]].active)
				{
					//Vector2 PO = Vector;
					//PO = Utils.RotatedBy(PO, projectile.localAI[0]+Main.npc[(int)projectile.ai[1]].rotation, default(Vector2));
					Projectile.Center = Main.npc[(int)Projectile.ai[1]].Center;
					Projectile.rotation = R - (Projectile.localAI[0] - Main.npc[(int)Projectile.ai[1]].rotation);
					Projectile.knockBack = 0;
				}
				else
				{
					Projectile.Kill();
				}
			}
			else
			{
				Projectile.frameCounter++;
				Projectile.spriteDirection = 0;
				Projectile.rotation = Projectile.velocity.ToRotation();
				if (Projectile.velocity.X < 0)
				{
					Projectile.spriteDirection = 1;
					Projectile.rotation += MathHelper.Pi;
				}
				Projectile.frame = (Projectile.frameCounter % 10) / 5 + 2;

				if (Projectile.ai[0] == 0)
				{
					if (Projectile.DProj().track > 10 && !Projectile.wet)
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
				else
				{
					if (!Projectile.DProj().Bool[0])
					{

                        for (int A = 0; A < 60; A++)
                        {
                            int type = 33;
                            if (Main.rand.NextBool(2))
                            {
                                type = ModContent.DustType<光球粒子>();

                            }
                            int dust = Dust.NewDust(Projectile.position + DrawVector.RotatedBy(-(Projectile.localAI[0] - Main.npc[(int)Projectile.ai[1]].rotation)), Projectile.width, Projectile.height, type, 0f, 0f, 10, new Color(23, 147, 234, 0), 2f);
                            if (type == ModContent.DustType<光球粒子>())
                            {
                                Main.dust[dust].scale = 1;

                            }
                            if (Main.rand.NextBool(2))
                            {
                                Main.dust[dust].scale = 0.5f;
                                Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.02f;
                            }
                            Main.dust[dust].noLightEmittence = false;
                            Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(5);
                            if (type == ModContent.DustType<光球粒子>())
                            {
                                Main.dust[dust].velocity /= 3;
                                Main.dust[dust].customData = -1;
                            }
                        }
						Projectile.DProj().Bool[0] = true;
                    }
					Projectile.tileCollide = false;
					Projectile.Track(500, 20, 16, 0, true, (int)(Projectile.ai[1]));
				}

			}
			if (Projectile.damage == 0)
			{
				Projectile.Kill();
			}
		}
		public override void OnKill(int timeLeft)
		{
			for (int A = 0; A < 60; A++)
			{
				int type = 33;
				if (Main.rand.NextBool(2))
				{
					type = ModContent.DustType<光球粒子>();

				}
				int dust = Dust.NewDust(Projectile.position + DrawVector.RotatedBy(-(Projectile.localAI[0] - Main.npc[(int)Projectile.ai[1]].rotation)), Projectile.width, Projectile.height, type, 0f, 0f, 10, new Color(23, 147, 234, 0), 2f);
				if (type == ModContent.DustType<光球粒子>())
				{
					Main.dust[dust].scale = 1;

				}
				if (Main.rand.NextBool(2))
				{
					Main.dust[dust].scale = 0.5f;
					Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.02f;
				}
				Main.dust[dust].noLightEmittence = false;
				Main.dust[dust].velocity = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(5);
				if (type == ModContent.DustType<光球粒子>())
				{
					Main.dust[dust].velocity /= 3;
					Main.dust[dust].customData = -1;
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
			if (Projectile.ai[0] != 2)
			{
				Projectile.ai[1] = target.whoAmI;
				Vector = Projectile.Center - target.Center;
				Projectile.localAI[0] = target.rotation;
				R = Projectile.rotation;
				if (Projectile.ai[0] > -2)
				{
					Vector2 vector = Vector2.One.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(80, 200);
					Projectile.NewProjectileChange(target.Center + vector, -vector / 12, Projectile.type, Projectile.damage, 0, -1, Projectile.ai[0] - 1, target.whoAmI);
				}
				Projectile.damage /= 10;
				Projectile.ai[0] = 2;
				Projectile.netUpdate = true;
			}
		}
		Vector2 DrawVector;
		public override bool PreDraw(ref Color lightColor)
        {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			SpriteEffects sprite = 0;
			if(Projectile.spriteDirection == 1)
            {
				sprite = SpriteEffects.FlipHorizontally;

			}

			Rectangle? rectangle = new Rectangle?(new Rectangle(0, texture.Height / 4 * Projectile.frame, texture.Width, texture.Height / 4));
			Main.spriteBatch.Draw(texture,Projectile.Center + DrawVector.RotatedBy(-(Projectile.localAI[0] - Main.npc[(int)Projectile.ai[1]].rotation)) - Main.screenPosition,rectangle,Color.White,Projectile.rotation,new Vector2(texture.Width, texture.Height / 4)/2,Projectile.scale, sprite,0);
			Color color = new Color(23, 147, 234, 0);
			Vector2 vector = new Vector2(Projectile.width, Projectile.height) / 2;
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				Vector2 vector2 = Projectile.oldPos[i] + vector - Main.screenPosition;
				Color oldcolor = color * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(texture, vector2 + DrawVector.RotatedBy(-(Projectile.localAI[0] - Main.npc[(int)Projectile.ai[1]].rotation)), rectangle, oldcolor, Projectile.rotation, new Vector2(texture.Width, texture.Height / 4) / 2, ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), sprite, 0f);
			}
			if (Vector != Vector2.Zero)
			{
				DrawVector = Vector;
			}

			return false;
        }
    }
}
