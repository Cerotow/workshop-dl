namespace DDmod.Content.Buffs.DeBuffs
{
    public class Charged2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 100;
            int dust = NewDust(npc.position, npc.width, npc.height, 226, Main.rand.NextFloat(-3,3), Main.rand.NextFloat(-3, 3), 0, default, 5f);
            Main.dust[dust].scale = 0.6f;
            Main.dust[dust].noGravity = true;

            if(npc.Dnpc().Properties.Iron || npc.Dnpc().Properties.Water)
            {
                npc.lifeRegen -= 100;
            }
        }
    }
}