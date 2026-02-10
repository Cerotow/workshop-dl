namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 炼狱之火 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(1))
            {
                player.lifeRegen -= 10;
                for (int i = 0; i < 1; i++)
                {
                    Dust dust = Main.dust[NewDust(player.position, player.width, player.height, 6)];
                    dust.noGravity = false;
                    dust.alpha = 100;
                    dust.scale = 0.8f;
                }
            }
            else
            {

                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Main.dust[NewDust(player.position, player.width, player.height, 6)];
                    dust.noGravity = false;
                    dust.alpha = 100;
                    dust.scale = 1.3f;
                }
                player.lifeRegen -= 100;
            }
        }
    }
}
