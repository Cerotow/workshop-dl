using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Exoskeleton {
    public class BeetleHelm : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Beetle Arm");
            Tooltip.SetDefault("Reduces damage taken by 10%\n" +
                "Increased melee damage and speed by 5%");
        }

        public override void SetDefaults() {
            item.width = 22;
            item.height = 32;
            item.accessory = true;
            item.rare = ItemRarityID.Yellow;
            item.value = 50000;
            item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.endurance += 0.1f;
            player.meleeDamage += 0.05f;
            player.meleeSpeed += 0.05f;
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddTile(TileID.TinkerersWorkbench);
            r.AddIngredient(ItemID.ChlorophyteBar, 10);
            r.AddIngredient(ItemID.BeetleHusk, 10);
            r.AddRecipe();
        }
    }
}
