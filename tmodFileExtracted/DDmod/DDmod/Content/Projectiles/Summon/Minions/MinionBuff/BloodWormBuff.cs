namespace DDmod.Content.Projectiles.Summon.Minions.MinionBuff
{
    public class BloodWormBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Blood worm");
           //DisplayName.AddTranslation(7, "血虫");
          //Description.SetDefault("Don't worry, he won't bite you");
           //Description.AddTranslation(7, "放心,他不会咬你");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BloodWorm>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}