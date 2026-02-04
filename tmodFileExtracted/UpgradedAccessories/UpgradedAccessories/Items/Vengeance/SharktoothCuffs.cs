using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Vengeance {
    public class SharktoothCuffs : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Restores mana when damaged\n" +
                "Increases armor penetration by 7");
        }

        public override void SetDefaults() {
            item.width = 28;
            item.height = 30;
            item.accessory = true;
            item.value = 30000;
            item.rare = ItemRarityID.Pink;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.magicCuffs = true;
            player.armorPenetration += 7;
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ItemID.MagicCuffs);
            r.AddIngredient(ItemID.SharkToothNecklace);
            r.AddTile(TileID.TinkerersWorkbench);
            r.AddRecipe();
        }
    }
}
