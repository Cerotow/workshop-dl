using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace UpgradedAccessories.Items.Boots {
    [AutoloadEquip(EquipType.Shoes)]
    public class FlameNinjaBoots : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Flame Ninja Boots");
            Tooltip.SetDefault("Allows enhanced flight, ultra fast running and extra mobility on ice\n" +
                "Provides the ability to walk on water and lava\n" +
                "Grants immunity to fire blocks and 14 seconds of immunity to lava\n" +
                "Increases jump height and negates fall damage\n" +
                "Grants the ability to dash\n" +
                "12% Increased movement speed");
        }
        public override void SetDefaults() {
            item.width = 36;
            item.height = 30;
            item.value = 300000;
            item.rare = ItemRarityID.Cyan;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            Util.SetHermesRocketBoots(player, 0.12f, 39.49f);
            player.rocketTimeMax = 14;
            player.iceSkate = true;
            Util.SetLavaWader(player, 840);
            player.dash = 1;
            player.jumpBoost = true;
            player.noFallDmg = true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<LavaSafetyBoots>());
            recipe.AddIngredient(ItemID.Tabi);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
