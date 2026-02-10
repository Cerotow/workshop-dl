using DDmod.Content.Items.Accessory;

namespace DDmod.Content.Buffs.DeBuffs
{
    public class 泰拉侵袭 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.rand.NextBool(6))
            {
                for (int i = 0; i < 1; i++)
                {
                    Dust dust = Main.dust[NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.星光粒子>(), 0, 0, 100, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 12))];
                    dust.noGravity = false;
                    dust.customData = 3;
                    dust.alpha = 100;
                    dust.scale = 1.2f;
                }
            }
            if (npc.Dnpc().BossPhysique)
            {
                npc.Dnpc().MoveSpeed *= 0.8f;
            }
            else
            {
                npc.Dnpc().MoveSpeed *= 0.2f;
            }
            npc.lifeRegen = -100;
            int D = NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.速度粒子>(), Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5), 0, new Color(Main.rand.Next(10, 83), Main.rand.Next(204, 255), Main.rand.Next(40, 164), 12), Main.rand.NextFloat(1.2f, 1.8f));
            Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
            Main.dust[D].customData = 2;
        }

    }
}