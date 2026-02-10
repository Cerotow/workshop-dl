using DDmod.Content.Dusts;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 远古能量Buff : ModBuff
    {
        public override string Texture => base.Texture;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statManaMax2 += 40;
            player.manaRegen++;
            if(Main.rand.NextBool(8))
            {
                NewDust(player.position,player.width,player.height,ModContent.DustType<光球粒子>(),0,0,100,new Color(0,120,255,0),Main.rand.NextFloat(0.5F,1.2F));
            }
        }
    }
}
