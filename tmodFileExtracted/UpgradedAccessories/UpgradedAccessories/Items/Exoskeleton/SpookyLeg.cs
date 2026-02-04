using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Exoskeleton {
    public class SpookyLeg : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("10% increased minion damage\n" +
                "Increases your max number of minions");
        }

        public override void SetDefaults() {
            item.width = 22;
            item.height = 32;
            item.accessory = true;
            item.rare = ItemRarityID.Yellow;
            item.value = 50000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.minionDamage += 0.1f;
            player.maxMinions++;
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddTile(TileID.TinkerersWorkbench);
            r.AddIngredient(ItemID.SpookyWood, 100);
            r.AddRecipe();
        }
    }
}