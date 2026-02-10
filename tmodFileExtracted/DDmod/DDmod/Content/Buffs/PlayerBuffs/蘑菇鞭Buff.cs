namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 蘑菇鞭Buff : ModBuff
    {
        public override string Texture => base.Texture;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Dnpc().蘑菇鞭 = true;
        }
    }
}
