using DDmod.Content.Items;
using DDmod.Content.Items.农场;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 恶魔力量Buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }
        public static int A;
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.8F;
        }
    }
}