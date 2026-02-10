using DDmod.Content.Items;
using DDmod.Content.Items.农场;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 弱效飞翔药水Buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(ModContent.BuffType<飞翔药水Buff>()))
            {
                player.buffTime[buffIndex] = 0;
            }
            player.Dplayer().wingTimeMax2 += DDHelper.Second(0.5F);
        }
    }
}