using ArtificerMod.Common;
using Terraria;
using Terraria.ModLoader;

namespace ArtificerMod.Content.Buffs.ArmorH
{
	public class NovaSurge : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<ArtificerPlayer>().accFlagAs)
            {
                player.GetDamage(DamageClass.Generic) += 0.1f;
                player.statDefense += 10;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            
        }
    }
}