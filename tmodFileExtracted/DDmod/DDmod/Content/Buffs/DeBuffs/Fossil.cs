namespace DDmod.Content.Buffs.DeBuffs
{
    public class Fossil : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Dnpc().NoMove = true;
            if(npc.Dnpc().BossPhysique)
            {
                buffIndex = 0;
            }
        }
    }
}