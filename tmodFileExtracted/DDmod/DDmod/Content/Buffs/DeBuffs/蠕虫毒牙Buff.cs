namespace DDmod.Content.Buffs.DeBuffs
{
    public class 蠕虫毒牙Buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= npc.buffTime[buffIndex] / 30;
            float I = npc.buffTime[buffIndex] / 2400F;
            if (I > 0.9F)
            {
                I = 0.9F;
            }
            if (npc.Dnpc().BossPhysique)
            {
                npc.Dnpc().MoveSpeed *= 1 - I / 3;
            }
            else
            {

                npc.Dnpc().MoveSpeed *= 1 - I;
            }
            I = npc.buffTime[buffIndex] / 3000F;
            if (I>5)
            {
                I = 5;
            }
                for (int i = 0; i < I + 1; i++)
            {
                Dust dust = Main.dust[NewDust(npc.position, npc.width, npc.height, 18, 0, 0)];
                dust.noGravity = true;
                dust.alpha = 100;
                dust.scale = 0.8f + I/8;
            }

        }
    }
}