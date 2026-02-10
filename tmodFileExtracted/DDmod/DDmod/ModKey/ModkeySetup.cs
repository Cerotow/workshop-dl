namespace DDmod.Modkey
{
    public class ModkeySetup
    {
        public static ModKeybind SetBonus { get; private set; }
        public static ModKeybind TalismanKey { get; private set; }
        public static ModKeybind ShowKey { get; private set; }
        public static ModKeybind BattlePetsKey { get; private set; }
        public static ModKeybind CommissionKey { get; private set; }
        public static ModKeybind StrengtheningKey { get; private set; }
        public static void LoadKey(Mod mod)
        {
            if (Main.netMode != 2)
            {
                SetBonus = KeybindLoader.RegisterKeybind(mod, "套装奖励", "Z");
                TalismanKey = KeybindLoader.RegisterKeybind(mod, "使用法宝", "Q");
                ShowKey = KeybindLoader.RegisterKeybind(mod, "展示物品", "L");
                BattlePetsKey = KeybindLoader.RegisterKeybind(mod, "战宠待命", "P");
                CommissionKey = KeybindLoader.RegisterKeybind(mod, "任务列表", "K");
                StrengtheningKey = KeybindLoader.RegisterKeybind(mod, "快捷强化", "Z");
            }
        }
        public static void UnloadKey()
        {
            if (Main.netMode != 2)
            {
                SetBonus = null;
                TalismanKey = null;
                ShowKey = null;
                BattlePetsKey = null;
                CommissionKey = null;
                StrengtheningKey = null;
            }
        }
    }
}
