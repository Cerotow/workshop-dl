using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace UpgradedAccessories.Items.Celestial {
    public class VortexScope : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Vortex Scope");
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"It\'s just applied aerodynamics\"\n" +
                "Increases view range for guns (Hide visual to disable this effect)\n" +
                "Summons 2 vortex arrows that will chase down and damage enemies\n" +
                "20% increased ranged damage and 15% increased ranged critical chance\n" +
                "20% chance to not consume ammo\n" +
                "Increases arrow speed and grants 20% chance to not consume arrow\n" +
                "Enemies are less likely to target you");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 39));
        }
        public override void SetDefaults() {
            item.width = 40;
            item.height = 40;
            item.value = 300000;
            item.rare = ItemRarityID.Red;
            item.expert = true;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<MyPlayer>().vortexScope = true;
            player.aggro -= 400;
            if (!hideVisual && (player.HeldItem.useAmmo == AmmoID.Bullet || player.HeldItem.useAmmo == AmmoID.CandyCorn || player.HeldItem.useAmmo == AmmoID.Stake ||
                player.HeldItem.useAmmo == AmmoID.Gel)) player.scope = true;
            player.rangedDamage += 0.2f;
            player.rangedCrit += 15;
            player.magicQuiver = true;
        }
        public override void AddRecipes() {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SniperScope);
            recipe.AddIngredient(ItemID.MagicQuiver);
            recipe.AddIngredient(ItemID.SporeSac);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddIngredient(ItemID.FragmentVortex, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
