using Terraria;
using Terraria.ModLoader;
using UpgradedAccessories.Items.Intimidation;

namespace UpgradedAccessories.Buffs {
    public class Rotten : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Rotten");
            Description.SetDefault("Increased damage taken");
        }

        public override void Update(NPC npc, ref int buffIndex) {
            npc.GetGlobalNPC<IntimidationGlobalNPC>().rotten = true;
        }
    }
}