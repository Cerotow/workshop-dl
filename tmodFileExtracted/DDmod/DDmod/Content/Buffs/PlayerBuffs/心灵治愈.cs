namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 心灵治愈 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 10;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
