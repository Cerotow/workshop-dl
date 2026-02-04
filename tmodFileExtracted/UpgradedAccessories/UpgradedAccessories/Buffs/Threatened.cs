using Terraria;
using Terraria.ModLoader;
using UpgradedAccessories.Items.Intimidation;

namespace UpgradedAccessories.Buffs {
    public class Threatened : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Threatened");
            Description.SetDefault("Reduced damage, increased damage taken, reduced movement speed and slowly losing life");
        }

        public override void Update(NPC npc, ref int buffIndex) {
            var ign = npc.GetGlobalNPC<IntimidationGlobalNPC>();
            ign.seared = true;
            ign.rotten = true;
            ign.webbed = true;
            ign.poisoned = true;
        }
    }
}
