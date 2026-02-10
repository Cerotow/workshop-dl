namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 狱火爆炸 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Dnpc().狱火爆炸 = true;
        }
    }
}
