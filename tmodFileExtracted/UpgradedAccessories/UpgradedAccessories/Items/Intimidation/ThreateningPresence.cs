using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UpgradedAccessories.Buffs;

namespace UpgradedAccessories.Items.Intimidation {
    public class ThreateningPresence : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Nearby enemies are weakened\n" +
                "Bosses are not affected by this");
        }

        public override void SetDefaults() {
            item.width = 38;
            item.height = 34;
            item.accessory = true;
            item.rare = ItemRarityID.Yellow;
            item.value = 150000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            var threatened = ModContent.BuffType<Threatened>();
            foreach(var npc in Main.npc) {
                if(npc.IsHittableHostile() && !npc.IsBoss() && Vector2.DistanceSquared(player.Center, npc.Center) < Intimidation.RADIUS_SQ) {
                    if(npc.FindBuffIndex(threatened) < 0) npc.AddBuff(threatened, 120);
                }
            }
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ModContent.ItemType<MoltenGem>());
            r.AddIngredient(ModContent.ItemType<CrimsonGem>());
            r.AddIngredient(ModContent.ItemType<NecroGem>());
            r.AddIngredient(ModContent.ItemType<JungleGem>());
            r.AddIngredient(ItemID.SoulofMight, 5);
            r.AddIngredient(ItemID.SoulofSight, 5);
            r.AddIngredient(ItemID.SoulofFright, 5);
            r.AddTile(TileID.TinkerersWorkbench);
            r.AddRecipe();

            r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ModContent.ItemType<MoltenGem>());
            r.AddIngredient(ModContent.ItemType<ShadowGem>());
            r.AddIngredient(ModContent.ItemType<NecroGem>());
            r.AddIngredient(ModContent.ItemType<JungleGem>());
            r.AddIngredient(ItemID.SoulofMight, 5);
            r.AddIngredient(ItemID.SoulofSight, 5);
            r.AddIngredient(ItemID.SoulofFright, 5);
            r.AddTile(TileID.TinkerersWorkbench);
            r.AddRecipe();
        }
    }
}
