using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Projectiles.AbilityAccH
{
	public class GamblecoreCopper : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Generic;
			
			Projectile.friendly = true;
			Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;

                if (++Projectile.frame >= 8)
                {
                    Projectile.frame = 0;
                }
            }

            if(Main.rand.NextBool(10))
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if(Projectile.velocity.Y < 16f)
            {
                Projectile.velocity.Y += 0.4f;
            }
            Projectile.velocity.X *= 0.995f;
        }

		public override void OnKill(int timeLeft)
		{
            int item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), ItemID.CopperCoin);
            if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
            {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight - 2);

            Vector2 origin = sourceRectangle.Size() / 2f;

            Color drawColor = Projectile.GetAlpha(lightColor);

            SpriteEffects spriteFX = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteFX, 0);

            return false;
        }
    }

    public class GamblecoreSilver : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<GamblecoreCopper>());
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;

                if (++Projectile.frame >= 8)
                {
                    Projectile.frame = 0;
                }
            }

            if (Main.rand.NextBool(10))
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SilverCoin);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.velocity.Y < 16f)
            {
                Projectile.velocity.Y += 0.4f;
            }
            Projectile.velocity.X *= 0.995f;
        }

        public override void OnKill(int timeLeft)
        {
            if(!Main.rand.NextBool(3))
            {
                int item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), ItemID.SilverCoin);
                if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight - 2);

            Vector2 origin = sourceRectangle.Size() / 2f;

            Color drawColor = Projectile.GetAlpha(lightColor);

            SpriteEffects spriteFX = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteFX, 0);

            return false;
        }
    }

    public class GamblecoreGold : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<GamblecoreCopper>());
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;

                if (++Projectile.frame >= 8)
                {
                    Projectile.frame = 0;
                }
            }

            if (Main.rand.NextBool(10))
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.velocity.Y < 16f)
            {
                Projectile.velocity.Y += 0.4f;
            }
            Projectile.velocity.X *= 0.995f;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(3))
            {
                int item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), ItemID.GoldCoin);
                if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight - 2);

            Vector2 origin = sourceRectangle.Size() / 2f;

            Color drawColor = Projectile.GetAlpha(lightColor);

            SpriteEffects spriteFX = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteFX, 0);

            return false;
        }
    }

    public class GamblecorePlatinum : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<GamblecoreCopper>());
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;

                if (++Projectile.frame >= 8)
                {
                    Projectile.frame = 0;
                }
            }

            if (Main.rand.NextBool(10))
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PlatinumCoin);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.velocity.Y < 16f)
            {
                Projectile.velocity.Y += 0.4f;
            }
            Projectile.velocity.X *= 0.995f;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.rand.NextBool(10))
            {
                int item = Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), ItemID.PlatinumCoin);
                if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight - 2);

            Vector2 origin = sourceRectangle.Size() / 2f;

            Color drawColor = Projectile.GetAlpha(lightColor);

            SpriteEffects spriteFX = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteFX, 0);

            return false;
        }
    }
}