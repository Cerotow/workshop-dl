using Terraria;
using Terraria.ModLoader;

namespace UpgradedAccessories.Buffs {
    public class Balance : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Balance");
            Description.SetDefault("12% increased damage, 8 increased defense, 8% increased endurance\n" +
                "Additional healing from celestial healing bolts");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.allDamage += 0.12f;
            player.statDefense += 8;
            player.endurance += 0.08f;
            player.GetModPlayer<MyPlayer>().balance = true;
        }
    }
}