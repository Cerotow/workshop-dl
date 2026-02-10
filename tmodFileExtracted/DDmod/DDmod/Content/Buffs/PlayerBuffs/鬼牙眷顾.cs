namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 鬼牙眷顾 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
    }
}
