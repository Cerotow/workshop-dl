using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.TrueShieldofCthulhu {
    public class UnoReverseCard : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("\"NO U\"\n" +
                "Very low chance to reflect any hostile projectile");
        }

        public override void SetDefaults() {
            item.width = 24;
            item.height = 32;
            item.rare = ItemRarityID.Yellow;
            item.value = 10000;
        }


        public override void UpdateAccessory(Player player, bool hideVisual) {
            var mp = player.GetModPlayer<MyPlayer>();
            if (mp.reflect < 1) mp.reflect = 1;
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ItemID.SpellTome);
            r.AddIngredient(ItemID.MagicMirror);
            r.AddIngredient(ItemID.CrystalShard, 20);
            r.AddIngredient(ItemID.SoulofFright, 10);
            r.AddIngredient(ItemID.SoulofMight, 10);
            r.AddIngredient(ItemID.SoulofSight, 10);
            r.AddTile(TileID.Bookcases);
            r.AddRecipe();

            r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ItemID.SpellTome);
            r.AddIngredient(ItemID.IceMirror);  
            r.AddIngredient(ItemID.CrystalShard, 20);
            r.AddIngredient(ItemID.SoulofFright, 10);
            r.AddIngredient(ItemID.SoulofMight, 10);
            r.AddIngredient(ItemID.SoulofSight, 10);
            r.AddTile(TileID.Bookcases);
            r.AddRecipe();
        }

    }
}
