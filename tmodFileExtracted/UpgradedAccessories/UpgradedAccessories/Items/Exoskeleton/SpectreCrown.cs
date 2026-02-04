using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Exoskeleton {
    public class SpectreCrown : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spectre Arm");
            Tooltip.SetDefault("Grants 1% magic damage life steal");
        }

        public override void SetDefaults() {
            item.width = 24;
            item.height = 32;
            item.accessory = true;
            item.rare = ItemRarityID.Yellow;
            item.value = 50000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<MyPlayer>().spectreLeggings = true;
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddTile(TileID.TinkerersWorkbench);
            r.AddIngredient(ItemID.SpectreBar, 10);
            r.AddRecipe();
        }
    }
}
