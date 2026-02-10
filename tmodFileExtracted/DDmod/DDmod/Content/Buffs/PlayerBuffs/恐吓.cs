namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 恐吓 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.Aplayer().NoGravity = 2;
            player.velocity.Y = 0.01F;
            player.velocity.X = 0F;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
