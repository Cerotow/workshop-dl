namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 永恒诅咒 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.IsATagBuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Dnpc().Penetrate += 15;
            if (npc.Dnpc().BossPhysique)
            {
                npc.Dnpc().MoveSpeed *= 0.75f;
            }
            else
            {
                npc.Dnpc().MoveSpeed *= 0.2f;
            }
            npc.lifeRegen = -30;
        }
    }
}