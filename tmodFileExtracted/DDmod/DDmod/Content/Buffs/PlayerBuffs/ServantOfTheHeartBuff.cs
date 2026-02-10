using DDmod.Content.Projectiles.OrnamentProjectile;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class ServantOfTheHeartBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ServantOfTheHeart>()] >= 1)
            {
                player.Aplayer().ServantOfTheHeart = true;
            }
            else
            {
                player.Aplayer().ServantOfTheHeart = false;
            }
            if (!player.Aplayer().ServantOfTheHeart)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}