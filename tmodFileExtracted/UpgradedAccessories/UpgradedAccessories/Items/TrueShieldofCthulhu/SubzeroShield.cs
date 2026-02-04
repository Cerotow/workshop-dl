using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.TrueShieldofCthulhu {
    [AutoloadEquip(EquipType.Shield)]
    public class SubzeroShield : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Puts a shell around the owner when below 50% life that reduces damage\n" +
                "Absorbs 25% of damage done to players on your team\n" +
                "Toggle vanity off to disable this effect");
        }

        public override void SetDefaults() {
            item.width = 28;
            item.height = 38;
            item.accessory = true;
            item.rare = ItemRarityID.Yellow;
            item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.noKnockback = true;
            if (player.statLife < player.statLifeMax2 / 2) player.AddBuff(BuffID.IceBarrier, 5);
            if(!hideVisual) Util.SetPaladinShield(player);
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddTile(TileID.TinkerersWorkbench);
            r.AddIngredient(ItemID.PaladinsShield);
            r.AddIngredient(ItemID.FrozenTurtleShell);
            r.AddRecipe();
        }
    }
}
