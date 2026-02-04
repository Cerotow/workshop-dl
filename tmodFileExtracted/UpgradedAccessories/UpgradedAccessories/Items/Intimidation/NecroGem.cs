using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UpgradedAccessories.Buffs;

namespace UpgradedAccessories.Items.Intimidation {
    public class NecroGem : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Webs nearby enemies, making them slower\n" +
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
            var webbed = ModContent.BuffType<Webbed>();
            foreach(var npc in Main.npc) {
                if(npc.IsHittableHostile() && !npc.IsBoss() && Vector2.DistanceSquared(player.Center, npc.Center) < Intimidation.RADIUS_SQ) {
                    if(npc.FindBuffIndex(webbed) < 0) npc.AddBuff(webbed, 120);
                }
            }
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ItemID.Bone, 60);
            r.AddIngredient(ItemID.Cobweb, 50);
            r.AddTile(TileID.Anvils);
            r.AddRecipe();
        }
    }
}