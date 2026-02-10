namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class MeteorMarker : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
           //DisplayName.SetDefault("Meteor Marker");
            BuffID.Sets.IsATagBuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.Dnpc().MeteorMarker = true;
        }
    }
}
