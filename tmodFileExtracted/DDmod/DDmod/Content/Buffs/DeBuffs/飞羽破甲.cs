namespace DDmod.Content.Buffs.DeBuffs
{
    public class 飞羽破甲 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Dnpc().Penetrate += 10;
        }
    }
}