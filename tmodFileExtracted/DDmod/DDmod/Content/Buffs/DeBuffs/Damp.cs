namespace DDmod.Content.Buffs.DeBuffs
{
    public class Damp : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            Dust.NewDust(player.position,player.width,player.height,DustID.Water);
            player.RotationSpeed(player.velocity.ToRotation() + MathHelper.PiOver2, 0.1f);
            if (player.velocity.Y == 0)
            {
                player.AddBuff(103, player.buffTime[buffIndex] + 480);
                player.buffTime[buffIndex] = 0;
            }
            else
            {
                if (player.HasBuff(103))
                {
                    player.buffTime[player.FindBuffIndex(103)] = 0;
                }
            }
        }
    }
}