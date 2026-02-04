using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UpgradedAccessories.Buffs;

namespace UpgradedAccessories.Items.Intimidation {
    public class CrimsonGem : ModItem{
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Rots nearby enemies, making them take more damage\n" +
                "Bosses are not affected by this");
        }

        public override void SetDefaults() {
            item.width = 28;
            item.height = 22;
            item.accessory = true;
            item.rare = ItemRarityID.Pink;
            item.value = 50000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            var rotten = ModContent.BuffType<Rotten>();
            foreach(var npc in Main.npc) {
                if(npc.IsHittableHostile() && !npc.IsBoss() && Vector2.DistanceSquared(player.Center, npc.Center) < Intimidation.RADIUS_SQ) {
                    if(npc.FindBuffIndex(rotten) < 0) npc.AddBuff(rotten, 120);
                }
            }
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ItemID.CrimtaneBar, 12);
            r.AddIngredient(ItemID.TissueSample, 8);
            r.AddTile(TileID.Anvils);
            r.AddRecipe();
        }
    }
}