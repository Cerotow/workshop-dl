using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace UpgradedAccessories.Items.Boots {
    [AutoloadEquip(EquipType.Shoes, EquipType.Wings)]
    public class LunarNinjaBoots : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Lunar Ninja Boots");
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"Equipped with portable quantum tunneling device\"\n" +
                "Allows ultra fast running and extra mobility on ice\n" +
                "Allows flight and slow fall\n" +
                "Provides the ability to move freely in and walk on liquid\n" +
                "Being wet or at low health drastically boosts your wing speed\n" +
                "Grants immunity to fire blocks and lava\n" +
                "Increases jump height and negates fall damage\n" +
                "Grants the ability to dash\n" +
                "15% Increased movement speed");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
        }

        public override void SetDefaults() {
            item.width = 36;
            item.height = 30;
            item.value = 300000;
            item.rare = ItemRarityID.Red;
            item.accessory = true;
            item.expert = true;
        }



        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.wingTimeMax = 180;

            Util.SetSpectreBoots(player, 0.15f, 44.49f);
            player.iceSkate = true;
            player.rocketTimeMax = 21;

            Util.SetLavaWader(player, 3600); // 1 minute of lava immunity
            if(!player.lavaWet) {
                player.lavaTime += 2; // recovers 3 instead of 1
                if(player.lavaTime > player.lavaMax) player.lavaTime = player.lavaMax;
            }
            player.lavaImmune = true;
            player.ignoreWater = true;
            player.dash = 1;

            player.jumpBoost = true;
            player.noFallDmg = true;

            var mp = player.GetModPlayer<MyPlayer>();
            mp.lunarBoots = true;

            if(player.wet) {
                mp.lunaticSpeedCounter = 300;
            } else if(player.statLife <= player.statLifeMax2 / 2) {
                mp.lunaticSpeedCounter = 60;
            }

        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
            var mul = player.GetModPlayer<MyPlayer>().lunaticSpeedCounter > 0 ? 1.5f : 1f;
            ascentWhenFalling = 0.85f * mul;
            ascentWhenRising = 0.15f * mul;
            maxCanAscendMultiplier = 1f * mul;
            maxAscentMultiplier = 3f * mul;
            constantAscend = 0.135f * mul;
        }



        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration) {
            var mul = 1f;
            if(player.GetModPlayer<MyPlayer>().lunaticSpeedCounter > 0) mul *= 1.5f;
            if(player.controlDown && player.controlJump && player.wingTime > 0) mul *= 1.5f;
            speed = 12 * mul;
            acceleration *= 3 * mul;
        }

        public override bool WingUpdate(Player player, bool inUse) {
            if(inUse && Main.rand.Next(2) == 0) {
                int wingOffset = player.direction == 1 ? -40 : 4;
                int id = Dust.NewDust(new Vector2(player.position.X + player.width / 2 + wingOffset, player.position.Y + player.height / 2 - 15f), 30, 30,
#pragma warning disable ChangeMagicNumberToID // Change magic numbers into appropriate ID values
                    20, 0f, 0f, 50, default, 0.6f);
#pragma warning restore ChangeMagicNumberToID // Change magic numbers into appropriate ID values
                Dust dust = Main.dust[id];
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0.3f;
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
            }
            if(player.velocity.Y != 0 && player.GetModPlayer<MyPlayer>().lunaticSpeedCounter > 0) {
                for(int i = 0; i < 4; i++) {
                    var dust = Main.dust[Dust.NewDust(new Vector2(player.position.X - 4, player.position.Y), player.width + 8, player.height,
                        DustID.BubbleBlock, player.velocity.X * -0.5f, player.velocity.Y * 0.5f, 0, Main.DiscoColor, 1)];
                    dust.noGravity = true;
                }
            }
            return false;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<FlameNinjaBoots>());
            recipe.AddIngredient(ItemID.FragmentSolar, 8);
            recipe.AddIngredient(ItemID.FragmentNebula, 8);
            recipe.AddIngredient(ItemID.FragmentVortex, 8);
            recipe.AddIngredient(ItemID.FragmentStardust, 8);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddIngredient(ItemID.ShrimpyTruffle);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
