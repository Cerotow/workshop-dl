using DDmod.Content.Items;
using DDmod.Content.Items.农场;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 飞翔药水Buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.Dplayer().wingTimeMax2 += DDHelper.Second(1);
        }
    }
}