

namespace DDmod.Content.Mounts
{
	public class 怒云之子 : ModMount
	{
		public override void SetStaticDefaults()
        {
            MountData.spawnDust = 185;
            MountData.spawnDustNoGravity = true;
			MountData.buff = ModContent.BuffType<怒云之子Buff>();
			MountData.flightTimeMax = 999999999;
			MountData.fatigueMax = 999999999;
			MountData.fallDamage = 0f;
			MountData.usesHover = true;
			MountData.runSpeed = 3;
			MountData.dashSpeed = 2f;
			MountData.acceleration = 0.5f;
			MountData.jumpHeight = 0;
			MountData.jumpSpeed = 0;
			MountData.blockExtraJumps = true;
            MountData.totalFrames = 6;
            int[] array = new int[MountData.totalFrames];
            for (int num12 = 0; num12 < array.Length; num12++)
            {
                array[num12] = 8;
            }

            MountData.playerYOffsets = array;
            MountData.heightBoost = 4;
            MountData.yOffset = 12;
            MountData.xOffset = -2;
            MountData.bodyFrame = 0;
			MountData.playerHeadOffset = 20;
			MountData.standingFrameCount = 6;
			MountData.standingFrameDelay = 0;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 6;
			MountData.runningFrameDelay = 0;
			MountData.runningFrameStart = 0;
			MountData.flyingFrameCount = 6;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = 6;
			MountData.inAirFrameDelay = 0;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = 6;
			MountData.idleFrameDelay = 0;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;
			if (Main.netMode != NetmodeID.Server)
            {
                MountData.frontTexture = ModContent.Request<Texture2D>("DDmod/Content/Mounts/怒云之子");
                MountData.textureWidth = MountData.frontTexture.Width();
				MountData.textureHeight = MountData.frontTexture.Height();
				MountData.frontTextureExtraGlow = ModContent.Request<Texture2D>("DDmod/Content/Mounts/怒云之子_Glow");

                MountData.backTextureExtraGlow = ModContent.Request<Texture2D>("DDmod/Content/Mounts/怒云之子");
            }
		}
        public override void Dismount(Player player, ref bool skipDust)
        {
			player.velocity /= 10;
        }
        public override void UpdateEffects(Player player)
        {
            player.velocity *= 0.96F;
			MountData.dashSpeed = 36;
			MountData.runSpeed = 18;
			MountData.acceleration = 6f;
            MountData.jumpHeight = 0;
            MountData.jumpSpeed = 0;
            //player.position.Y = player.oldPosition.Y;
		}
        public override void UseAbility(Player player, Vector2 mousePosition, bool toggleOn)
        {
            base.UseAbility(player, mousePosition, toggleOn);
        }
        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {
            Time++;
			if (Math.Abs(velocity.X) > 0.1F)
			{
				if (Time / 4 >= 6)
				{
					Time = 0;
				}
			}
			else
			{

				if (Time / 4 >= 4)
				{
					Time = 0;
				}
			}
            return base.UpdateFrame(mountedPlayer, state, velocity);
		}
		int Time;
		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
		{
            Player player = drawPlayer;
			frame = new Rectangle(Math.Abs(drawPlayer.velocity.X) > 0.1F ? texture.Width / 2 : 0, texture.Height / MountData.standingFrameCount * (Time / 4 % 6), texture.Width / 2, texture.Height / 6);
			DrawData item = new DrawData(texture, drawPosition, frame, drawColor, rotation, frame.Size()/2, drawScale, Math.Abs(drawPlayer.velocity.X) > 0.1F? (drawPlayer.velocity.X>0 ? 0 : SpriteEffects.FlipHorizontally) : drawPlayer.direction == 1 ? 0 : SpriteEffects.FlipHorizontally, 0);
			item.shader = player.cMount;
			playerDrawData.Add(item);
            Texture2D Glow = MountData.frontTextureExtraGlow.Value;
			item.texture = Glow;
			item.color = Color.White;
			playerDrawData.Add(item);


			return false;
		}
	}
}