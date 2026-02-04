using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace UpgradedAccessories.Items.Celestial {
    public class StardustScarab : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Stardust Scroll");
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"You and what army?\"\n" +
                "Grants a shield that scales with your minion abilities\n" +
                "Summons a guardian that protects you\n" +
                "30% increased minion damage\n" +
                "Increases your max number of minions by 2\n" +
                "Increases your max number of sentries by 1\n" +
                "Increases knockback of your minions\n" +
                "Enemies are less likely to target you");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 7));
        }
        public override void SetDefaults() {
            item.width = 36;
            item.height = 36;
            item.value = 300000;
            item.rare = ItemRarityID.Red;
            item.expert = true;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<MyPlayer>().stardustScroll = true;
            player.minionKB += 2f;
            player.minionDamage += 0.3f;
            player.maxMinions += 2;
            player.maxTurrets++;
            player.aggro -= 400;
        }
        public override void AddRecipes() {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PapyrusScarab);
            recipe.AddIngredient(ItemID.ApprenticeScarf);
            recipe.AddIngredient(ItemID.WormScarf);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddIngredient(ItemID.FragmentStardust, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PapyrusScarab);
            recipe.AddIngredient(ItemID.SquireShield);
            recipe.AddIngredient(ItemID.WormScarf);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddIngredient(ItemID.FragmentStardust, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PapyrusScarab);
            recipe.AddIngredient(ItemID.HuntressBuckler);
            recipe.AddIngredient(ItemID.WormScarf);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddIngredient(ItemID.FragmentStardust, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PapyrusScarab);
            recipe.AddIngredient(ItemID.MonkBelt);
            recipe.AddIngredient(ItemID.WormScarf);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddIngredient(ItemID.FragmentStardust, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
