namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 拉肚子 : ModBuff
    {
        public override string Texture => base.Texture;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.AddBuff(33,2);
            player.AddBuff(120,2);
        }
    }
}
