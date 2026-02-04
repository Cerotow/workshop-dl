using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Boots {
    public class FrostSafetyBoots : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Frost Safety Boots");
            Tooltip.SetDefault("Allows flight, super fast running and extra mobility on ice\n" +
                "Increases jump height and negates fall damage\n" +
                "8% increased movement speed");
        }
        public override void SetDefaults() {
            item.width = 36;
            item.height = 30;
            item.value = 120000;
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            Util.SetSpectreBoots(player);
            player.iceSkate = true;
            player.jumpBoost = true;
            player.noFallDmg = true;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FrostsparkBoots);
            recipe.AddIngredient(ModContent.ItemType<RedHorseshoeBalloon>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
