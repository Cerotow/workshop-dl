using Terraria;
using Terraria.ModLoader;
using UpgradedAccessories.Items.Intimidation;

namespace UpgradedAccessories.Buffs {
    public class Intimidated : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Intimidated");
            Description.SetDefault("\"...!\"\n" +
                "Drastically reduced damage, increased damage taken, reduced movement speed and losing life fast");
        }

        public override void Update(NPC npc, ref int buffIndex) {
            var ign = npc.GetGlobalNPC<IntimidationGlobalNPC>();
            ign.seared = true;
            ign.rotten = true;
            ign.webbed = true;
            ign.poisoned = true;
            ign.intimidated = true;
        }
    }
}
