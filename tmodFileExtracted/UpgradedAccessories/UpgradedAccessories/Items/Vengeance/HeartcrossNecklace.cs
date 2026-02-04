using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace UpgradedAccessories.Items.Vengeance {
    public class HeartcrossNecklace : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Increases movement speed after being struck\n" +
                "Increases length of invincibility after taking damage");
        }

        public override void SetDefaults() {
            item.width = 30;
            item.height = 30;
            item.accessory = true;
            item.rare = ItemRarityID.LightPurple;
            item.value = 30000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.longInvince = true;
            player.panic = true;
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ItemID.PanicNecklace);
            r.AddIngredient(ItemID.CrossNecklace);
            r.AddTile(TileID.TinkerersWorkbench);
            r.AddRecipe();
        }
    }
}
