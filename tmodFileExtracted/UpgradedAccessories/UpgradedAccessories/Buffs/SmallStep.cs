using Terraria;
using Terraria.ModLoader;

namespace UpgradedAccessories.Buffs {
    public class SmallStep : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Small Step");
            Description.SetDefault("\"One small step for man...\"\n" +
                "Increased running speed\n" +
                "Bonus damage against airborn enemies");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.GetModPlayer<MyPlayer>().smallStep = true;
        }
    }
}