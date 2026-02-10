namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 绿岩鞭Buff : ModBuff
    {
        public override string Texture => "DDmod/Content/Buffs/PlayerBuffs/Buff";
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Dnpc().绿岩鞭 = true;
        }
    }
}
