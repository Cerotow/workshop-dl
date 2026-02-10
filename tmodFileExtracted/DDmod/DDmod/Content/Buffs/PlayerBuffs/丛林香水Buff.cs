using DDmod.Content.Dusts;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 丛林香水Buff : ModBuff
    {
        public override string Texture => base.Texture;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if(Main.rand.NextBool(20))
            {
                NewDust(player.position,player.width,player.height,ModContent.DustType<光球粒子>(),0,0,100,new Color(150,255,0,0),Main.rand.NextFloat(0.5F,1.2F));
            }
        }
    }
}
