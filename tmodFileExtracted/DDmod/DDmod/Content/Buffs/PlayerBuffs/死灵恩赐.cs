namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 死灵恩赐 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 1.5f * player.buffTime[buffIndex] / 600;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.5f * player.buffTime[buffIndex] / 600;
        }
    }
}
