using Terraria;
using Terraria.ModLoader;
using UpgradedAccessories.Items.Intimidation;

namespace UpgradedAccessories.Buffs {
    public class Seared : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Seared");
            Description.SetDefault("Reduced damage");
        }

        public override void Update(NPC npc, ref int buffIndex) {
            npc.GetGlobalNPC<IntimidationGlobalNPC>().seared = true;
        }
    }
}