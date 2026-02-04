using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.Celestial {
    public class ShootingStarSoundStudio : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shooting Star Overdrive System");
            if(UpgradedAccessories.thoriumLoaded) {
                Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                    "\"...and his music was electric.\"\n" +
                    "Hitting enemies with symphonic damage will periodically summon a grog barrel\n" +
                    "It will explode and buff any ally including you after dealing enough symphonic damage\n" +
                    "20% increased symphonic damage\n" +
                    "15% increased symphonic critical chance and playing speed\n" +
                    "Increases the duration of your symphonic empowerments by 2 seconds\n" +
                    "10% increased inspiration regeneration\n" +
                    "You and nearby allies have a chance to elementally backlash enemies when attacking\n" +
                    "Doubles the range of your empowerments effect radius\n" +
                    "Enemies are less likely to target you");
            } else {
                Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                    "\"...and his music was electric.\"\n" +
                    "Install thorium mod for this item to work");
            }
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 6));
        }

        public override void SetDefaults() {
            item.width = 44;
            item.height = 44;
            item.value = 300000;
            item.rare = ItemRarityID.Red;
            item.expert = true;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            if(UpgradedAccessories.thoriumLoaded) {
                ApplyEffects(player);
                player.GetModPlayer<MyPlayer>().shootingStarSoundStudio = true;
                player.aggro -= 400;
            }
        }

        private static void ApplyEffects(Player player) {
            var tp = player.GetModPlayer<ThoriumMod.ThoriumPlayer>();
            tp.symphonicDamage += 0.2f;
            tp.symphonicCrit += 15;
            tp.symphonicSpeed += 0.15f;
            tp.accSubwooferTerrarium = true;
            tp.bardRangeBoost += 500;
            tp.bardBuffDuration += 2;
            tp.inspirationRegenBonus += 0.1f;
        }

        public override void AddRecipes() {
            if(UpgradedAccessories.thoriumLoaded) {
                var r = new ModRecipe(mod);
                r.SetResult(this);
                r.AddIngredient(UpgradedAccessories.thorium.ItemType("TerrariumSubwoofer"));
                r.AddIngredient(UpgradedAccessories.thorium.ItemType("BandKit"));
                r.AddIngredient(UpgradedAccessories.thorium.ItemType("GrogBlueprint"));
                r.AddIngredient(ItemID.LunarBar, 16);
                r.AddIngredient(UpgradedAccessories.thorium.ItemType("CometFragment"), 20);
                r.AddRecipe();
            }
        }
    }
}
