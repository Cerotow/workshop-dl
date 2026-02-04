using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UpgradedAccessories.Buffs;
using Microsoft.Xna.Framework;

namespace UpgradedAccessories.Items.Intimidation {
    public class Intimidation : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"...!\"\n" +
                "All slimes become friendly including king slime\n" +
                "You will ignore any enemy or projectile deemed not worthy of your respect\n" +
                "Nearby enemies are terrified of your mere presence\n" +
                "Bosses are affected by this to a lesser degree");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 12));
        }

        public override void SetDefaults() {
            item.width = 42;
            item.height = 38;
            item.accessory = true;
            item.rare = ItemRarityID.Red;
            item.expert = true;
            item.value = 150000;
        }

        public const float RADIUS_SQ = 90000f; // 300 * 300

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetModPlayer<MyPlayer>().intimidation = true;
            var threatened = ModContent.BuffType<Threatened>();
            var intimidated = ModContent.BuffType<Intimidated>();
            foreach(var npc in Main.npc) {
                if(npc.IsHittableHostile() && Vector2.DistanceSquared(player.Center, npc.Center) < RADIUS_SQ) {
                    if(npc.IsBoss()) {
                        if(npc.FindBuffIndex(threatened) < 0) npc.AddBuff(ModContent.BuffType<Threatened>(), 120);
                    } else {
                        if(npc.FindBuffIndex(intimidated) < 0) npc.AddBuff(ModContent.BuffType<Intimidated>(), 120);
                    }
                }
            }

            var a = player.npcTypeNoAggro;

            a[1] = true;
            a[16] = true;
            a[50] = true; // king slime
            a[59] = true;
            a[71] = true;
            a[81] = true;
            a[121] = true;
            a[122] = true;
            a[138] = true;
            a[141] = true;
            a[147] = true;
            a[183] = true;
            a[184] = true;
            a[204] = true;
            a[225] = true;
            a[244] = true;
            a[302] = true;
            a[333] = true;
            a[334] = true;
            a[335] = true;
            a[336] = true;
            a[535] = true; // spiked slime
            a[537] = true;


        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ItemID.RoyalGel);
            r.AddIngredient(ModContent.ItemType<ThreateningPresence>());
            r.AddIngredient(ItemID.LunarBar, 8);
            r.AddTile(TileID.LunarCraftingStation);
            r.AddRecipe();
        }
    }
}
