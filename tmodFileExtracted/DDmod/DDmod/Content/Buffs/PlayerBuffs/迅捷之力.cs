namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 迅捷之力 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Generic)+=0.25f;
            player.moveSpeed += 0.4f;
        }
    }
}
