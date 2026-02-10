namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class CrimsonFury : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic)+=0.1f;
        }
    }
}
