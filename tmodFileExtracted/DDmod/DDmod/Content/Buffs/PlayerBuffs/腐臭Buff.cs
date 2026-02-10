using DDmod.Content.Dusts;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 腐臭Buff : ModBuff
    {
        public override string Texture => base.Texture;
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 0.95f;
            if(Main.rand.NextBool(20))
            {
                NewDust(player.position,player.width,player.height, 284, 0,0,100, new Color(111, 12, 183, 150), Main.rand.NextFloat(0.5F,1.2F));
            }
        }
    }
}
