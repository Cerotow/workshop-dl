using DDmod.Content.Dusts;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 绿岩电流 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 150;
            int dust = NewDust(player.position, player.width, player.height, ModContent.DustType<绿岩电光粒子>(), 0f, 1f, 0, default, 5f);
            Main.dust[dust].scale = 1f;
            Main.dust[dust].noGravity = true;

        }
    }
}