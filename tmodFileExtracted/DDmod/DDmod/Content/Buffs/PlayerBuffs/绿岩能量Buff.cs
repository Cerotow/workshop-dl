namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 绿岩能量Buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Magic) += 0.5f;
        }
    }
}
