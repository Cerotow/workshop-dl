using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.SpaceSuit {
    public class AnkhSuit : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Grants the ability to swim and greatly extends underwater breathing\n" +
                "Provides light under water and extra mobility on ice\n" +
                "Prevents getting chilled from cold water\n" +
                "Prevents strong winds from pushing you\n" +
                "Grants immunity to knockback and fire blocks\n" +
                "Grants immunity to most debuffs");
        }

        public override void SetDefaults() {
            item.width = 20;
            item.height = 36;
            item.value = 80000;
            item.accessory = true;
            item.rare = ItemRarityID.LightPurple;
            item.defense = 4;
        }



        public override void UpdateAccessory(Player player, bool hideVisual) {
            Util.SetArcticDivingGear(player);
            player.buffImmune[BuffID.WindPushed] = true;

            player.noKnockback = true;
            player.fireWalk = true;

            var b = player.buffImmune;
            b[BuffID.Bleeding] = true;
            b[BuffID.BrokenArmor] = true;
            b[BuffID.Confused] = true;
            b[BuffID.Cursed] = true;
            b[BuffID.Darkness] = true;
            b[BuffID.Poisoned] = true;
            b[BuffID.Silenced] = true;
            b[BuffID.Slow] = true;
            b[BuffID.Weak] = true;
            b[BuffID.Chilled] = true;
        }

        public override void AddRecipes() {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AnkhShield);
            recipe.AddIngredient(ModContent.ItemType<DesertDivingGear>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
