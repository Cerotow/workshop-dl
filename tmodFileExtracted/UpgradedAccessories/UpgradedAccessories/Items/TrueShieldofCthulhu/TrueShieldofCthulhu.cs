using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.TrueShieldofCthulhu {
    [AutoloadEquip(EquipType.Shield)]
    public class TrueShieldofCthulhu : ModItem {

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("True Shield of Cthulhu");
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"You have to face danger head-on\"\n" +
                "Puts a shell around the owner when below 50% life that reduces damage\n" +
                "Absorbs 25% of damage done to players on your team\n" +
                "Toggle vanity off to disable this effect\n" +
                "10% increased critical strike chance\n" +
                "Critical strikes reduces enemy defense a lot\n" +
                "Allows the player to dash through the enemies\n" +
                "Not fleeing from a danger drastically reduces damage taken from it\n" +
                "Moderate chance to reflect any hostile projectile");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 86));
        }

        public override void SetDefaults() {
            item.width = 30;
            item.height = 32;
            item.accessory = true;
            item.rare = ItemRarityID.Yellow;
            item.value = 150000;
            item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.noKnockback = true;
            if (player.statLife < player.statLifeMax2 / 2) player.AddBuff(BuffID.IceBarrier, 5);
            if(!hideVisual) Util.SetPaladinShield(player);
            var mp = player.GetModPlayer<MyPlayer>();
            if (mp.reflect < 3) mp.reflect = 3;
            mp.allCirt += 10;
            mp.trueSoC = true;
            player.dash = 0; // override vanilla dash
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddTile(TileID.LunarCraftingStation);
            r.AddIngredient(ItemID.EoCShield);
            r.AddIngredient(ModContent.ItemType<OmniscientShield>());
            r.AddIngredient(ItemID.LunarBar, 8);
            r.AddRecipe();
        }


    }
}
