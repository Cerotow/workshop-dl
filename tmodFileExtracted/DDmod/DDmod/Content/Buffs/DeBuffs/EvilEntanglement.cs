namespace DDmod.Content.Buffs.DeBuffs
{
    public class EvilEntanglement : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 10;
            if(npc.Dnpc().Properties.Light)
            {
                npc.lifeRegen -= 20;
            }
            npc.Dnpc().Penetrate += 8;
            int Type = 14;
            Dust dust = Main.dust[NewDust(npc.position, npc.width, npc.height, Type)];
            dust.noGravity = true;
            dust.scale = 1.2f;
            dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.2f, 3f);
        }
    }
}