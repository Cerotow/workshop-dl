using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace UpgradedAccessories.Items.Celestial {
    public class CelestialIdol : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Celestial Child");
            if(UpgradedAccessories.thoriumLoaded) {
                Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                    "\"Emergency protocol activated\"\n" +
                    "An aura that buffs your allies follows your cursor(hide visual to disable this effect)\n" +
                    "Fast healing bolts that heals life and mana for 4 will periodically emerge from you, 1 for each ally\n" +
                    "These bolts won't trigger any on heal effects, won't extend your heal streak and will only get half of the bonus healing\n" +
                    "20% increased radiant damage\n" +
                    "15% increased radiant critical chance\n" +
                    "10% increased radiant casting and healing speed\n" +
                    "Healing spells will heal an additional 3 life\n" +
                    "Corrupts your radiant powers\n" +
                    "Enemies are less likely to target you");
            } else {
                Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                    "\"Emergency protocol activated\"\n" +
                    "Install thorium mod for this item to work");
            }
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 22));
        }

        public override void SetDefaults() {
            item.width = 35;
            item.height = 24;
            item.value = 300000;
            item.rare = ItemRarityID.Red;
            item.expert = true;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            if(UpgradedAccessories.thoriumLoaded) {
                ApplyEffects(player);
                var mp = player.GetModPlayer<MyPlayer>();
                mp.celestialChild = true;
                if(!hideVisual) mp.yinyangCursor = true;
                player.aggro -= 800;
            }
        }

        private static void ApplyEffects(Player player) {
            var tp = player.GetModPlayer<ThoriumPlayer>();
            tp.radiantBoost += 0.2f;
            tp.radiantCrit += 15;
            tp.radiantSpeed += 0.1f;
            tp.healingSpeed += 0.1f;
            tp.healBonus += 3;
            tp.darkAura = true;
        }

        public override void AddRecipes() {
            if(UpgradedAccessories.thoriumLoaded) {
                var r = new ModRecipe(mod);
                r.SetResult(this);
                r.AddIngredient(UpgradedAccessories.thorium.ItemType("ArchDemonCurse"));
                r.AddIngredient(UpgradedAccessories.thorium.ItemType("ArchangelHeart"));
                r.AddIngredient(UpgradedAccessories.thorium.ItemType("DarkMageStaff"));
                r.AddIngredient(ItemID.LunarBar, 16);
                r.AddIngredient(UpgradedAccessories.thorium.ItemType("CelestialFragment"), 20);
                r.AddTile(TileID.LunarCraftingStation);
                r.AddRecipe();
            }
        }
    }
}
