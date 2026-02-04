using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Exoskeleton {
    public class ShroomiteVisor : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shroomite Leg");
            Tooltip.SetDefault("Increases arrow, bullet and rocket damage by 10%");
        }

        public override void SetDefaults() {
            item.width = 24;
            item.height = 32;
            item.accessory = true;
            item.rare = ItemRarityID.Yellow;
            item.value = 50000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.arrowDamage += 0.1f;
            player.bulletDamage += 0.1f;
            player.rocketDamage += 0.1f;
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddTile(TileID.TinkerersWorkbench);
            r.AddIngredient(ItemID.ShroomiteBar, 10);
            r.AddRecipe();
        }
    }
}
