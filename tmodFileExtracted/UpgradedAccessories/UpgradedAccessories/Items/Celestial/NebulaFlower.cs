using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using UpgradedAccessories.Buffs;

namespace UpgradedAccessories.Items.Celestial {
    public class NebulaFlower : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Nebula Flower");
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"Yer a wizard, [Player Name Here]!\"\n" +
                "Dealing magic damage will summon nebula boosters that grant magic damage buff\n" +
                "It will also drop hearts and stars\n" +
                "Picking up heart/star will grant health/mana regen buff\n" +
                "These buffs will boost each other's effect\n" +
                "15% increased magic damage and critical chance\n" +
                "Increases pickup range for stars\n" +
                "8% reduced mana usage\n" +
                "You automatically use mana potions when needed\n" +
                "Enemies are less likely to target you");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 8));
        }
        public override void SetDefaults() {
            item.width = 24;
            item.height = 56;
            item.value = 300000;
            item.rare = ItemRarityID.Red;
            item.expert = true;
            item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            if(Main.LocalPlayer.active) {
                foreach(var line in tooltips) {
                    if(line.mod.Equals("Terraria") && line.Name.Equals("Tooltip1")) {
                        line.text = $"\"Yer a wizard, {Main.LocalPlayer.name}!\"";
                    }
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<MyPlayer>().nebulaFlower = true;
            player.manaMagnet = true;
            player.manaFlower = true;
            player.magicDamage += 0.15f;
            player.magicCrit += 10;
            player.manaCost -= 0.08f;
            player.aggro -= 400;
        }
        public override void AddRecipes() {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CelestialEmblem);
            recipe.AddIngredient(ItemID.ManaFlower);
            recipe.AddIngredient(ItemID.ShinyStone);
            recipe.AddIngredient(ItemID.LunarBar, 16);
            recipe.AddIngredient(ItemID.FragmentNebula, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class NebulaBooster : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Nebula Booster");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
        }

        public override void SetDefaults() {
            item.width = 14;
            item.height = 22;
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void PostUpdate() {
            var colorMul = Main.rand.Next(90, 111) * 0.01f * Main.essScale;
            Lighting.AddLight(item.Center, 0.1f * colorMul, 0.5f * colorMul, 0.1f * colorMul);
        }

        public override bool ItemSpace(Player player) {
            return true;
        }

        public override bool OnPickup(Player player) {
            if(player.GetModPlayer<MyPlayer>().nebulaFlower) {
                player.AddBuff(ModContent.BuffType<NebulaCatalyst>(), 300);
            }
            Main.PlaySound(SoundID.Grab, player.Center);
            return false;
        }

        public override void GrabRange(Player player, ref int grabRange) {
            if(player.GetModPlayer<MyPlayer>().nebulaFlower) {
                grabRange += 100;
            }
        }
    }
}
