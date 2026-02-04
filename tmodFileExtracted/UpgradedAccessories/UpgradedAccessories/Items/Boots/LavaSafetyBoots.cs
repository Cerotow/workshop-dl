using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Boots {
    [AutoloadEquip(EquipType.Shoes)]
    public class LavaSafetyBoots : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Lava Safety Boots");
            Tooltip.SetDefault("Allows flight, super fast running and extra mobility on ice\n" +
                "Provides the ability to walk on water and lava\n" +
                "Grants immunity to fire blocks and 7 seconds of immunity to lava\n" +
                "Increases jump height and negates fall damage\n" +
                "8% increased movement speed");
        }
        public override void SetDefaults() {
            item.width = 36;
            item.height = 30;
            item.value = 150000;
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            Util.SetHermesRocketBoots(player);
            Util.SetLavaWader(player);
            player.iceSkate = true;
            player.jumpBoost = true;
            player.noFallDmg = true;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<FrostSafetyBoots>());
            recipe.AddIngredient(ItemID.LavaWaders);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
