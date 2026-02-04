using Terraria;
using Terraria.ModLoader;

namespace UpgradedAccessories.Buffs {
    public class NebulaCatalyst : ModBuff{
        public override void SetDefaults() {
            DisplayName.SetDefault("Nebula Catalyst");
            Description.SetDefault("Increased magic damage");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.GetModPlayer<MyPlayer>().nebulaBoost = true;
        }

        public override bool ReApply(Player player, int time, int buffIndex) {
            time += player.buffTime[buffIndex];
            if(time > 600) time = 600;
            player.buffTime[buffIndex] = time;
            return true;
        }
    }
}