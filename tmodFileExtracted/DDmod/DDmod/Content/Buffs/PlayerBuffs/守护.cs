using DDmod.Content.Items;

namespace DDmod.Content.Buffs.PlayerBuffs
{
    public class 守护 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            //tip = DDSystem.English ? "Increase" + Main.LocalPlayer.ActiveItem().GetGlobalItem<MeleeGlobalItem>().Endurance*100 + "% Endurance" : "增加" + Main.LocalPlayer.ActiveItem().GetGlobalItem<MeleeGlobalItem>().Endurance*100 + "%减伤";
        }
    }
    public class 真守护 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
    }
    public class 泰拉守护 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
    }
}
