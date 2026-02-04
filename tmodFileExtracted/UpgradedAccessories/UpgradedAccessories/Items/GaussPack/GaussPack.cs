using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.GaussPack {
    public class GaussPack : ModItem{
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"lrr r lr rrlr rrlr\"\n" +
                "Drastically increases tile and wall placement speed and reach\n" +
                "Automatically paints placed objects\n" +
                "Drastically reduces spawn rates\n" +
                "Set and use the hotkey to hover infinitely with very fast movement at the cost of dealing no damage\n" +
                "Grants night vision");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 9));
        }

        public override void SetDefaults() {
            item.width = 30;
            item.height = 34;
            item.value = 300000;
            item.rare = ItemRarityID.Red;
            item.expert = true;
            item.accessory = true;
            item.backSlot = 8;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.autoPaint = true;
            player.tileSpeed += 0.5f;
            player.wallSpeed += 0.5f;
            player.nightVision = true;
            if (Main.myPlayer == player.whoAmI) {
                Player.tileRangeX += 5;
                Player.tileRangeY += 5;
            }
            player.GetModPlayer<MyPlayer>().gaussPack = true;
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddIngredient(ItemID.ArchitectGizmoPack);
            r.AddIngredient(ItemID.CalmingPotion, 5);
            r.AddIngredient(ItemID.PeaceCandle);
            r.AddIngredient(ItemID.SuspiciousLookingTentacle);
            r.AddTile(TileID.LunarCraftingStation);
            r.AddRecipe();
        }
    }
}
