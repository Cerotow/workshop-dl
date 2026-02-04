using Terraria;
using Terraria.ModLoader;

namespace UpgradedAccessories.Buffs {
    public class GiantLeap : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Giant Leap");
            Description.SetDefault("\"One giant leap for mankind...\"\n" +
                "Bonus damage against ground enemies");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.GetModPlayer<MyPlayer>().giantLeap = true;
        }
    }
}