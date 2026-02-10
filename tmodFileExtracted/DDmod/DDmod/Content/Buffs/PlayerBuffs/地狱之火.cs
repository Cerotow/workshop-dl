namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 地狱之火 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Main.dust[NewDust(player.position, player.width, player.height, 6)];
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = 0.5f;
            }
            int D = NewDust(player.position, player.width, player.height, ModContent.DustType<Dusts.速度粒子>(), Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5), 0, new Color(253, Main.rand.Next(62, 152), 3,0),Main.rand.NextFloat(1,3));
            Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
            player.lifeRegen -= 30;
            
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Main.dust[NewDust(npc.position, npc.width, npc.height, 6)];
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale =0.5f;
            }
            int D = NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.速度粒子>(), Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5), 0, new Color(253, Main.rand.Next(62,152), 3, 0), Main.rand.NextFloat(1, 3));
            Main.dust[D].rotation = Main.dust[D].velocity.ToRotation();
            npc.lifeRegen -= 60;
            if(npc.Combustion())
            {
                npc.lifeRegen -= 60;
            }

        }
    }
}
