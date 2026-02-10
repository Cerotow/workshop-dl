namespace DDmod.Content.Buffs.DeBuffs
{
    public class Bleed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.IsATagBuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 12;
            npc.Dnpc().Penetrate += 5;
        }
    }
}