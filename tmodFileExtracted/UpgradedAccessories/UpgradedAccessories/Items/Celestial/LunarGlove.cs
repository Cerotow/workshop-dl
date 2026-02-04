using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Celestial {
    public class LunarGlove : ModItem{
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Final Delivery");
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"It fitz, but the spirits like to throw tantrums\"\n" +
                "Throwing/rogue hits periodically summon a helping hand that claps the enemy\n" +
                "15% increased thrown/rogue damage and critical chance\n" +
                "20% increased thrown/rogue velocity\n" +
                "Enemies are less likely to target you");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 28));
        }

        public override void SetDefaults() {
            item.width = 26;
            item.height = 28;
            item.value = 300000;
            item.rare = ItemRarityID.Red;
            item.expert = true;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<MyPlayer>().lunarGlove = true;
            player.thrownDamage += 0.15f;
            player.thrownCrit += 15;
            player.thrownVelocity += 0.2f;
            player.aggro -= 400;
        }

        public override void AddRecipes() {
            
            if (UpgradedAccessories.thoriumLoaded) {
                var recipe = new ModRecipe(mod);
                recipe.AddIngredient(UpgradedAccessories.thorium.ItemType("ThrowingGuideVolume3"));
                recipe.AddIngredient(ItemID.BoneGlove);
                recipe.AddIngredient(ItemID.LunarBar, 16);
                recipe.AddIngredient(UpgradedAccessories.thorium.ItemType("WhiteDwarfFragment"), 20);
                recipe.AddTile(TileID.LunarCraftingStation);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }else if (UpgradedAccessories.calamityLoaded) {
                var recipe = new ModRecipe(mod);
                recipe.AddIngredient(UpgradedAccessories.calamity.ItemType("VampiricTalisman"));
                recipe.AddIngredient(ItemID.BoneGlove);
                recipe.AddIngredient(ItemID.LunarBar, 16);
                recipe.AddIngredient(UpgradedAccessories.calamity.ItemType("GalacticaSingularity"), 5);
                recipe.AddTile(TileID.LunarCraftingStation);
                recipe.SetResult(this);
                recipe.AddRecipe();
            } else {
                var recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.DestroyerEmblem);
                recipe.AddIngredient(ItemID.BoneGlove);
                recipe.AddIngredient(ItemID.LunarBar, 16);
                recipe.AddIngredient(ItemID.FragmentSolar, 5);
                recipe.AddIngredient(ItemID.FragmentNebula, 5);
                recipe.AddIngredient(ItemID.FragmentStardust, 5);
                recipe.AddIngredient(ItemID.FragmentVortex, 5);
                recipe.AddTile(TileID.LunarCraftingStation);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
            
        }
    }
}