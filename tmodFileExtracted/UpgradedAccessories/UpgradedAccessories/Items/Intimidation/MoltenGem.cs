using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UpgradedAccessories.Buffs;

namespace UpgradedAccessories.Items.Intimidation {
    public class MoltenGem : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Sears nearby enemies, making them deal reduced damage\n" +
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
            var seared = ModContent.BuffType<Seared>();
            foreach(var npc in Main.npc) {
                if(npc.IsHittableHostile() && !npc.IsBoss() && Vector2.DistanceSquared(player.Center, npc.Center) < Intimidation.RADIUS_SQ) {
                    if(npc.FindBuffIndex(seared) < 0) npc.AddBuff(seared, 120);
                }
            }
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ItemID.HellstoneBar, 16);
            r.AddTile(TileID.Anvils);
            r.AddRecipe();
        }
    }
}