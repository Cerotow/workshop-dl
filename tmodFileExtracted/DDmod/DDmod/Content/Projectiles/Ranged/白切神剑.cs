using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;
using DDmod.Content.Projectiles.Melee.Sword;
using Microsoft.Xna.Framework.Graphics;
using DDmod.Content.Dusts;

namespace DDmod.Content.Projectiles.Ranged
{
	public class 白切神剑 : ModProjectile
	{
		public override void SetDefaults()
		{
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			Projectile.width = 10;
			Projectile.height = 60;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
			Projectile.tileCollide = false;
            Projectile.hide = true;
        }
		
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
        public override void SetStaticDefaults()
		{

		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			fallThrough = false;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			Projectile.tileCollide = Projectile.position.Y+Projectile.height > Projectile.ai[2];
			if (Projectile.localAI[0] ==0)
			{
                Projectile.localAI[0] = Projectile.damage;
			}
			Projectile.damage = (int)Projectile.localAI[0];
			Projectile.DProj().Magnification = Projectile.localAI[1];
            if (Projectile.ai[0]>0)
			{
				Projectile.velocity *= 0.9F;
				Projectile.damage = (int)(Projectile.localAI[0] / 10);
                Projectile.DProj().Magnification = Projectile.localAI[1]/10;
                Projectile.tileCollide = false;

            }
			if(Projectile.timeLeft<5)
			{
				Projectile.timeLeft = 2;
				Projectile.ai[1] -= 0.01F;

                if (Projectile.ai[1]<0)
				{
					Projectile.Kill();
				}
			}
			else
			{

                if (Projectile.ai[1] < 1F)
                {
                    Projectile.ai[1] += 0.05F;

                }
            }


        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0]++;
				Projectile.velocity = oldVelocity/4;
                Projectile.tileCollide = false;
            }
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
			if (target.knockBackResist != 0)
			{
				if (target.Center.X -Projectile.Player().Center.X > 0)
				{
                    target.velocity.X = 4* target.knockBackResist;
				}
				else
				{

                    target.velocity.X = target.knockBackResist * -4;

                }
                DDmod.SyncData(DDType.NPCCenter, target.whoAmI, -1, Projectile.owner);
            }
            if (Projectile.ai[0] == 0)
			{
				Vector2 vector = Main.rand.NextVector2Unit() * 60;
				int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
				Main.projectile[A].DProj().color = new Color(255, 255, 255, 100) * 0.5F;
				Main.projectile[A].scale = 0.6F;
			}
			else
			{

                Vector2 vector = Main.rand.NextVector2Unit() * 60;
                int A = NewProjectile(Projectile.GetSource_FromThis(), target.Center, -vector / 10, ModContent.ProjectileType<GlobalSlash>(), 0, 0, Projectile.owner, target.whoAmI, 1);
                Main.projectile[A].DProj().color = new Color(255, 255, 255, 100) * 0.1F;
                Main.projectile[A].scale = 0.1F;
            }
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				Vector2 vector2 = Projectile.oldPos[i] + Projectile.Size/2- Main.screenPosition;
				Color color = new Color(255, 255, 255, 40) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * (Projectile.ai[1])*0.5F;
				Main.spriteBatch.Draw(texture, vector2, null, color, Projectile.oldRot[i], new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale , 0, 0f);
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White * (Projectile.ai[1]), Projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, Projectile.scale, 0, 0);
			//Main.spriteBatch.Draw(DDTextures.WhitePng.Value, Projectile.position - Main.screenPosition, null, Color.White * (Projectile.ai[1]), 0, Vector2.Zero, Projectile.Size/2, 0, 0);
            return false;
        }
    }
}
