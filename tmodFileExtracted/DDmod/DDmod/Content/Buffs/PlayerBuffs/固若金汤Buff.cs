using DDmod.Players;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 固若金汤Buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if(player.Dplayer().FightPets<0)
            {
                if (player.buffTime[buffIndex] > 2)
                    player.buffTime[buffIndex] = 2;
                return;
            }
            BattlePets pets = player.Dplayer().Bpets[player.Dplayer().FightPets];
            if (pets != null&& pets.Type==Players.BattlePets.牢中灵骷&&pets.Variant == 2)
            {
                player.statDefense += pets.Defense;
            }
            else
            {
                if (player.buffTime[buffIndex] > 2)
                    player.buffTime[buffIndex] = 2;
            }
        }
    }
}
