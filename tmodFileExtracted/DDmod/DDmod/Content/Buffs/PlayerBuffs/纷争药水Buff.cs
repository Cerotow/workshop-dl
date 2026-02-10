using DDmod.Content.Dusts;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 纷争药水Buff : ModBuff
    {
        public override string Texture => base.Texture;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.enemySpawns = true;
        }
    }
}
