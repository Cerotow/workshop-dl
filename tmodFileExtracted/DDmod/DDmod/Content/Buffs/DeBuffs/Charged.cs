namespace DDmod.Content.Buffs.DeBuffs
{
    public class Charged : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 150;
            int dust = NewDust(npc.position, npc.width, npc.height, Utils.SelectRandom(Main.rand, 226, 228, 75), 0f, 1f, 0, default, 5f);
            Main.dust[dust].scale = 1f;
            Main.dust[dust].noGravity = true;

            if (npc.Dnpc().Properties.Iron||npc.Dnpc().Properties.Water)
            {
                npc.lifeRegen -= 150;
            }

        }
    }
}