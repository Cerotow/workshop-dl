using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace UpgradedAccessories.Buffs {
    public class Drunk : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Drunk");
            Description.SetDefault("Reduces damage taken by 8% for 0.5 seconds after melee hit\n" +
                "3% life steal on ranged hit\n" +
                "Increased mana, inspiration and exhaustion regeneration\n" +
                "Minions have 10% chance to deal critical hit\n");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.manaRegenBonus += 2;
            player.GetModPlayer<MyPlayer>().drunk = true;
            var tp = player.GetModPlayer<ThoriumPlayer>();
            tp.inspirationRegenBonus += 0.05f;
            tp.throwerExhaustionRegenBonus += 0.05f;
        }
    }
}