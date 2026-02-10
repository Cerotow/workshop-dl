namespace DDmod.Content.Projectiles.Melee.ball
{
    public class CorruptionBall : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 50;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void SetStaticDefaults()
		{
		}
		public override void AI()
		{
			Projectile.rotation += Projectile.velocity.X*0.03f;
			if (Projectile.velocity.Y < 10)
			{
				Projectile.velocity.Y += 0.1F;
			}
			NPC npc = NPCdirection.FindClosest(Projectile.Center, 300, false);
			if (npc != null)
			{
				Vector2 vector = (npc.Center - Projectile.Center).PerfectNormalize();
				Projectile.velocity.X = (Projectile.velocity.X * 150 + vector.X * 5) / 151;
			}
			Lighting.AddLight(Projectile.Center, 0.88f * 0.5f, 0.68f * 0.5f, 1.97f * 0.5f);
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if(Projectile.velocity.X==0)
            {
				Projectile.velocity.X = -oldVelocity.X/3;
			}
			if (Projectile.velocity.Y == 0)
			{
				if (oldVelocity.Y > 6)
				{
					Projectile.velocity.Y = -oldVelocity.Y / 3;
				}
				else
                {
					Projectile.velocity.Y = -3;
				}
			}
            return false;
        }
        public override void OnHitNPC(NPC target, HitInfo hit, int damageDone)
        {
			Projectile.velocity.X = -Projectile.velocity.X / 2;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			NPC npc = NPCdirection.FindClosest(Projectile.Center, 300, false);
			fallThrough = true;
			if (npc != null)
			{
				fallThrough = (npc.position.Y) - (Projectile.position.Y + Projectile.height) >= 0;
			}
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height())), new Color(255, 255, 255, 255), Projectile.rotation, TextureAssets.Projectile[Projectile.type].Size() / 2, Projectile.scale, 0, 0f);
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 0, TextureAssets.Projectile[Projectile.type].Width(), TextureAssets.Projectile[Projectile.type].Height())), new Color(88, 68, 255, 0), Projectile.rotation, TextureAssets.Projectile[Projectile.type].Size() / 2, Projectile.scale, 0, 0f);
			return false;
		}
	}
}
