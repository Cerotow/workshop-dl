using DDmod.Content.Dusts;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 异星毒液Buff : ModBuff
    {
        public override string Texture => base.Texture;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 30;
            player.statLifeMax2 -= player.Aplayer().LifeMax / 5;
            if (Main.rand.NextBool(12))
            {
                NewDust(player.position, player.width, player.height, 229, 0, 0, 100, default, Main.rand.NextFloat(0.3F, 0.6F));
            }
        }
    }
}
