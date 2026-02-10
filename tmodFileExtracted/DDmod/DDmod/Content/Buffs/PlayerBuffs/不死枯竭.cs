namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 不死枯竭 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
        }
    }
}
