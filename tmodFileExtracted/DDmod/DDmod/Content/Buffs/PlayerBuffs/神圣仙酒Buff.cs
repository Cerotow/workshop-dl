using DDmod.Content.Dusts;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 神圣仙酒Buff : ModBuff
    {
        public override string Texture => base.Texture;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 6;
            player.statLifeMax2 += 20;
            player.GetDamage(DamageClass.Generic) += 0.05F;
            if(Main.rand.NextBool(20))
            {
                NewDust(player.position,player.width,player.height,ModContent.DustType<光球粒子>(),0,0,100,new Color(255, 191, 0, 100),Main.rand.NextFloat(0.5F,1.2F));
            }
        }
    }
}
