using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace UpgradedAccessories.Items.Exoskeleton {
    public class Exoskeleton : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Exoskeleton");
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"Deadly lasers\"\n" +
                "Automatically shoots deadly lasers to nearby enemies\n" +
                "Moving fast will drastically increase laser damage and range\n" +
                "It will also moderately increase damage\n" +
                "Reduces damage taken by 10%\n" +
                "Increased melee speed by 10%\n" +
                "Increases damage multiplier by 10%\n" +
                "Increases your max number of minions\n" +
                "Grants 1% exo life steal");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(12, 4));
        }
        public override void SetDefaults() {
            item.width = 48;
            item.height = 52;
            item.value = 100000;
            item.rare = ItemRarityID.Red;
            item.accessory = true;
            item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.endurance += 0.1f;
            player.meleeSpeed += 0.1f;
            player.allDamageMult *= 1.1f;
            player.maxMinions++;
            player.GetModPlayer<MyPlayer>().exoskeleton = true;
        }

        public override void AddRecipes() {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<BeetleHelm>());
            recipe.AddIngredient(ModContent.ItemType<ShroomiteVisor>());
            recipe.AddIngredient(ModContent.ItemType<SpectreCrown>());
            recipe.AddIngredient(ModContent.ItemType<SpookyLeg>());
            recipe.AddIngredient(ItemID.MinecartMech);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
