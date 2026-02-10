using DDmod.Content.Dusts;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 腥臭Buff : ModBuff
    {
        public override string Texture => base.Texture;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statLifeMax2 -= 20;
            if(Main.rand.NextBool(20))
            {
                NewDust(player.position,player.width,player.height,5,0,0,100,default,Main.rand.NextFloat(0.5F,1.2F));
            }
        }
    }
}
