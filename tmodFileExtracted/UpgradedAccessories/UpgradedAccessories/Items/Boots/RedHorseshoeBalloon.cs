using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Boots {
    [AutoloadEquip(EquipType.Balloon)]
    public class RedHorseshoeBalloon : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Red Horseshoe Balloon");
            Tooltip.SetDefault("Increases jump height and negates fall damage");
        }

        public override void SetDefaults() {
            item.width = 28;
            item.height = 48;
            item.value = 50000;
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.jumpBoost = true;
            player.noFallDmg = true;
        }

        public override void AddRecipes() {
            var recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddIngredient(ItemID.ShinyRedBalloon);
            recipe.AddIngredient(ItemID.LuckyHorseshoe);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.CloudinaBottle);
            recipe.SetResult(ItemID.BlueHorseshoeBalloon);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.FartinaJar);
            recipe.SetResult(ItemID.BalloonHorseshoeFart);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.HoneyComb);
            recipe.SetResult(ItemID.BalloonHorseshoeHoney);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.BlizzardinaBottle);
            recipe.SetResult(ItemID.WhiteHorseshoeBalloon);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.SandstorminaBottle);
            recipe.SetResult(ItemID.YellowHorseshoeBalloon);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.TsunamiInABottle);
            recipe.SetResult(ItemID.BalloonHorseshoeSharkron);
            recipe.AddRecipe();
        }
    }
}
