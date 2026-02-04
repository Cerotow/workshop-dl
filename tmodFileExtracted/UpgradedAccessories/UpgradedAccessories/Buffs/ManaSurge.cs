using Terraria;
using Terraria.ModLoader;

namespace UpgradedAccessories.Buffs {
    public class ManaSurge : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Mana Surge");
            Description.SetDefault("Increased mana regeneration");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.GetModPlayer<MyPlayer>().manaSurge = true;
        }

        public override bool ReApply(Player player, int time, int buffIndex) {
            time += player.buffTime[buffIndex];
            if(time > 600) time = 600;
            player.buffTime[buffIndex] = time;
            return true;
        }
    }
}
