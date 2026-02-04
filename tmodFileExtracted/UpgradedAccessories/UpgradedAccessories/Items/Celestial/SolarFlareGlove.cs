using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace UpgradedAccessories.Items.Celestial {
    public class SolarFlareGlove : ModItem{
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Solar Flare Glove");
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"WARNING INCOMING SOLAR FLARE ATTACK\"\n" +
                "Taking damage releases a super nova flare that pulls enemies in then explodes dealing massive damage\n" +
                "Reduces damage taken by 10% from projectiles\n" +
                "Increases melee knockback and inflicts daybroken on melee attacks\n" +
                "Gives the user master yoyo skills\n" +
                "15% increased melee damage and speed\n" +
                "10% increased melee critical chance\n" +
                "Enemies are more likely to target you");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));
        }
        public override void SetDefaults() {
            item.width = 26;
            item.height = 30;
            item.value = 300000;
            item.rare = ItemRarityID.Red;
            item.defense = 8;
            item.expert = true;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<MyPlayer>().solarFlareGlove = true;
            player.kbGlove = true;
            player.meleeDamage += 0.15f;
            player.meleeCrit += 10;
            player.meleeSpeed += 0.15f;
            player.aggro += 400;
            player.yoyoGlove = true;
            player.yoyoString = true;
            player.counterWeight = ProjectileID.BlackCounterweight + Main.rand.Next(6);
        }
        public override void AddRecipes() {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FireGauntlet);
            recipe.AddIngredient(ItemID.YoyoBag);
            recipe.AddIngredient(ItemID.BrainOfConfusion);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddIngredient(ItemID.FragmentSolar, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}