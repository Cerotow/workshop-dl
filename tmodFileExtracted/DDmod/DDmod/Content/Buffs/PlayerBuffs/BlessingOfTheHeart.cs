namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class BlessingOfTheHeart : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 3;
            player.Aplayer().Heartstrengthening = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Dnpc().HeartMarker = true;
        }
    }
}
