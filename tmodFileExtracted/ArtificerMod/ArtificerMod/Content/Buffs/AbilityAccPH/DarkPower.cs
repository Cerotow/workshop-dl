using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Buffs.AbilityAccPH
{
	public class DarkPower : ModBuff
	{
		public override void Update(Player player, ref int buffIndex)
		{
            player.GetDamage(DamageClass.Generic) += 0.1f;

            if (Main.rand.NextBool(10)) // Adapted from On Fire! debuff
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Shadowflame,
                    player.velocity.X, player.velocity.Y, 255, default, 1f);
                dust.noGravity = true;
                dust.velocity.Y -= Main.rand.NextFloat(2f, 4f);
            }
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            int currentTime = player.buffTime[player.FindBuffIndex(Type)];

            if(currentTime < 4)
            {
                player.buffTime[player.FindBuffIndex(Type)] = time;
            }
            else if(currentTime < 600)
            {
                player.buffTime[player.FindBuffIndex(Type)] += time / 2;
            }

            return true;
        }
    }
}