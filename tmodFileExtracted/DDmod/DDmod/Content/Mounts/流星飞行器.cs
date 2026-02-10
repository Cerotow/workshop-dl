

namespace DDmod.Content.Mounts
{
	public class 流星飞行器 : ModMount
	{
		public override void SetStaticDefaults()
		{
			MountData.spawnDust = 6;
			MountData.spawnDustNoGravity = true;
			MountData.buff = ModContent.BuffType<流星飞行器Buff>();
			MountData.heightBoost = 2;
			MountData.flightTimeMax = 999999999;
			MountData.fatigueMax = 999999999;
			MountData.fallDamage = 0f;
			MountData.usesHover = true;
			MountData.runSpeed = 3;
			MountData.dashSpeed = 2f;
			MountData.acceleration = 0.5f;
			MountData.jumpHeight = 5;
			MountData.jumpSpeed = 5;
			MountData.blockExtraJumps = true;
            MountData.totalFrames = 1;
            int[] array = new int[MountData.totalFrames];
            for (int num12 = 0; num12 < array.Length; num12++)
            {
                array[num12] = 20;
            }

            MountData.playerYOffsets = array;
            MountData.xOffset = 0;
            MountData.yOffset = 22;
            MountData.heightBoost = 20;
            MountData.bodyFrame = 0;
            MountData.playerHeadOffset = 20;
            MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 0;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 1;
			MountData.runningFrameDelay = 0;
			MountData.runningFrameStart = 0;
			MountData.flyingFrameCount = 1;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 0;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 0;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;
			if (Main.netMode != NetmodeID.Server)
			{
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
                MountData.backTextureExtraGlow = ModContent.Request<Texture2D>("DDmod/Content/Mounts/流星飞行器_Glow");
                MountData.frontTextureExtraGlow = ModContent.Request<Texture2D>("DDmod/Content/Mounts/流星飞行器2_Glow");
            }
		}
		public override void UpdateEffects(Player player)
        {
            player.velocity *= 0.96F;
            if (player.Aplayer().破坏者核心装置)
            {
                MountData.dashSpeed = 12;
                MountData.runSpeed = 6;
                MountData.acceleration = 2f;
                MountData.jumpSpeed = 6;
				
                if (player.statLife <= player.statLifeMax2 / 2)
                {
                    MountData.dashSpeed = 24;
                    MountData.runSpeed = 12;
                    MountData.acceleration = 12f;
                    MountData.jumpSpeed = 12;
                }
            }
			else
            {
				if(Math.Abs(player.velocity.Y)>1)
				{
					player.velocity.Y *= 0.92F;
				}
                MountData.jumpSpeed = 1;
                MountData.dashSpeed = 4;
                MountData.runSpeed = 2;
                MountData.acceleration = 0.5f;
                if (player.statLife <= player.statLifeMax2 / 2)
                {
                    MountData.jumpSpeed = 2;
                    MountData.dashSpeed = 8;
                    MountData.runSpeed = 4;
                    MountData.acceleration = 1f;
                }
            }
            if (player.statLife <= player.statLifeMax2 / 2)
            {
                Lighting.AddLight(player.Center, new Vector3(1, 210, 7) / 200);
            }
            else
            {

                Lighting.AddLight(player.Center, new Vector3(248, 66, 5) / 200);
            }

        }
        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {
			mountedPlayer.legFrame.Y = 0;
            return base.UpdateFrame(mountedPlayer, state, velocity);
        }
        public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
		{
			Player player = drawPlayer;
            DrawData item = new DrawData(texture, drawPosition, null, drawColor, rotation, drawOrigin, drawScale, 0, 0);
            item.shader = player.cMount;
            playerDrawData.Add(item);
            if (drawPlayer.statLife <= drawPlayer.statLifeMax2 / 2)
			{
				Texture2D Glow = MountData.frontTextureExtraGlow.Value;
				item.texture = Glow;
				item.color = Color.White;

                playerDrawData.Add(item);
			}
			else
			{
				Texture2D Glow = MountData.backTextureExtraGlow.Value;
                item.texture = Glow;
                item.color = Color.White;
                playerDrawData.Add(item);
				
			}

			return false;
		}
	}
}