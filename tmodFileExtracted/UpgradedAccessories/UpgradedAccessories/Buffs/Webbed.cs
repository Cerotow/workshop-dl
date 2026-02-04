using Terraria;
using Terraria.ModLoader;
using UpgradedAccessories.Items.Intimidation;

namespace UpgradedAccessories.Buffs {
    public class Webbed : ModBuff{
        public override void SetDefaults() {
            DisplayName.SetDefault("Webbed");
            Description.SetDefault("Reduced movement speed");
        }

        public override void Update(NPC npc, ref int buffIndex) {
            npc.GetGlobalNPC<IntimidationGlobalNPC>().webbed = true;
        }
    }
}