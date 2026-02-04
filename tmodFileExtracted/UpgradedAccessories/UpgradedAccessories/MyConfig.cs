using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace UpgradedAccessories {
    public class MyConfig : ModConfig {
        public static MyConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        [Label("Enables space suit item animation")]
        public bool spaceSuitAnimation;
    }
}