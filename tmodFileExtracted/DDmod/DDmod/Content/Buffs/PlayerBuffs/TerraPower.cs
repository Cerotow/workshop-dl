using DDmod.Content.Dusts;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class TerraPower : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Main.dust[NewDust(player.position, player.width, player.height, ModContent.DustType<星光粒子>(), newColor: new Color(141, 233, 130, 0))];
                dust.noGravity = false;
                dust.alpha = 100;
                dust.scale = Main.rand.NextFloat(0.6F, 1);
            }
        }
    }
}
