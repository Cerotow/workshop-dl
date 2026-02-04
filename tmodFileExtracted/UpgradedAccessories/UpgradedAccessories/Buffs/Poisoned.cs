using Terraria;
using Terraria.ModLoader;
using UpgradedAccessories.Items.Intimidation;

namespace UpgradedAccessories.Buffs {
    public class Poisoned : ModBuff{
        public override void SetDefaults() {
            DisplayName.SetDefault("Poisoned");
            Description.SetDefault("Slowly losing life");
        }

        public override void Update(NPC npc, ref int buffIndex) {
            npc.GetGlobalNPC<IntimidationGlobalNPC>().poisoned = true;
        }
    }
}