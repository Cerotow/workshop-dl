using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.SpaceSuit {
    [AutoloadEquip(EquipType.Face)]
    public class DesertDivingGear : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Grants the ability to swim and greatly extends underwater breathing\n" +
            "Provides light under water and extra mobility on ice\n" +
            "Prevents getting chilled from cold water\n" +
            "Prevents strong winds from pushing you");
        }

        public override void SetDefaults() {
            item.width = 30;
            item.height = 34;
            item.value = 70000;
            item.accessory = true;
            item.rare = ItemRarityID.Pink;
        }



        public override void UpdateAccessory(Player player, bool hideVisual) {
            Util.SetArcticDivingGear(player);
            player.buffImmune[BuffID.WindPushed] = true;
        }

        public override void AddRecipes() {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ArcticDivingGear);
            recipe.AddIngredient(ItemID.SandBlock, 100);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}