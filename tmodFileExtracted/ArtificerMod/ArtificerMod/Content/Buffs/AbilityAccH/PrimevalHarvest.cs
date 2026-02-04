using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Buffs.AbilityAccH
{
	public class PrimevalHarvest : ModBuff
	{
        public override void Update(Player player, ref int buffIndex)
        {
            // Effects applied in ArtificerPlayer

            if (Main.rand.NextBool(10))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.ChlorophyteWeapon, player.velocity.X, player.velocity.Y, Scale: 1.2f);
                dust.noGravity = true;
                dust.velocity.Y -= Main.rand.NextFloat(2f, 4f);
            }
        }
    }
}