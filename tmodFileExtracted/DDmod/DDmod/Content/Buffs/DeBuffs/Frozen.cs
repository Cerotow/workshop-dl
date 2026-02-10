namespace DDmod.Content.Buffs.DeBuffs
{
    public class Frozen : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.Dnpc().BossPhysique)
            {
                npc.Dnpc().MoveSpeed *= 0.9f;
            }
            else
            {
                npc.Dnpc().MoveSpeed *= 0.5f;
            }
            if (npc.Dnpc().Properties.Fire)
            {
                npc.buffTime[buffIndex] -= 3;
            }
        }
    }
}