using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Vengeance {
    public class Vengeance : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Vengeance");
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"Moon's haunted\" \"What?\" \"Moon's haunted\"\n" +
                "Causes exploding moons to fall and increases length of invincibility after taking damage\n" +
                "Releases incredibly strong beesiles that chase down enemies when damaged\n" +
                "Increases the strength of friendly bees\n" +
                "Increases movement speed after being struck\n" +
                "Restores mana when damaged\n" +
                "Increases armor penetration by 10\n");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 12));
        }

        public override void SetDefaults() {
            item.width = 28;
            item.height = 28;
            item.value = 150000;
            item.rare = ItemRarityID.Red;
            item.accessory = true;
            item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.longInvince = true;
            player.GetModPlayer<MyPlayer>().vengeance = true;
            player.panic = true;
            player.magicCuffs = true;
            player.strongBees = true;
            player.armorPenetration += 10;
        }

        public override void AddRecipes() {
            var recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddIngredient(ItemID.StarVeil);
            recipe.AddIngredient(ItemID.SweetheartNecklace);
            recipe.AddIngredient(ModContent.ItemType<SharktoothCuffs>());
            recipe.AddIngredient(ItemID.HiveBackpack);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddIngredient(ItemID.BeeCloak);
            recipe.AddIngredient(ModContent.ItemType<HeartcrossNecklace>()); ;
            recipe.AddIngredient(ModContent.ItemType<SharktoothCuffs>());
            recipe.AddIngredient(ItemID.HiveBackpack);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.AddRecipe();
        }
    }
}
