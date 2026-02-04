using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Buffs.AbilityAccPH
{
	public class OverdriveBoost : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.accRunSpeed < 10f)
            {
                player.accRunSpeed = 10f;
            }
            if (player.maxRunSpeed < 10f)
            {
                player.maxRunSpeed = 10f;
            }
            player.runAcceleration *= 2f;
			player.runSlowdown *= 2f;

			player.jumpSpeedBoost += 9f;

			// Dust FX:
			if (player.velocity.Length() > 6.5f)
			{
				for (int i = 0; i < 3; i++)
				{
					int num21 = Dust.NewDust(new Vector2(player.position.X - player.velocity.X * 2f, player.position.Y - 2f - player.velocity.Y * 2f), player.width, player.height, DustID.Electric, 0f, 0f, 100, default(Color), 0.5f);
					Main.dust[num21].noGravity = true;
					Main.dust[num21].noLight = true;
					Main.dust[num21].velocity.X -= player.velocity.X * 0.8f;
					Main.dust[num21].velocity.Y -= player.velocity.Y * 0.8f;
				}
			}
		}
    }
}