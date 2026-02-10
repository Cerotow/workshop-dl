namespace DDmod.Content.Buffs.DeBuffs
{
    public class 永夜侵袭 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Main.dust[NewDust(npc.position, npc.width, npc.height, 27)];
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 0.4f;
            }
            if (npc.Dnpc().BossPhysique)
            {
                npc.Dnpc().MoveSpeed *= 0.92f;
            }
            else
            {
                npc.Dnpc().MoveSpeed *= 0.6f;
            }
            npc.lifeRegen = -30;
            int D = NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.速度粒子>(), Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5), 0, new Color(81, 6, 233, 12), Main.rand.NextFloat(1.2f, 1.8f));
            Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
            Main.dust[D].customData = 2;
        }
    }
}