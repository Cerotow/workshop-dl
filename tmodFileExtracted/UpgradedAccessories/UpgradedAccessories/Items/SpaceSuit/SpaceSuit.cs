using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradedAccessories.Items.SpaceSuit {
    public class SpaceSuit : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("[c/55f055:-Uber Item-]\n" +
                "\"F = GMm/r^2\"\n" +
                "Grants different buffs on landing and taking off(these buffs have to alternate with each other)\n" +
                "Grants the ability to swim and greatly extends underwater breathing\n" +
                "Provides light under water and extra mobility on ice\n" +
                "Prevents getting chilled from cold water\n" +
                "Prevents strong winds from pushing you\n" +
                "Normalizes gravity\n" +
                "Grants immunity to knockback and fire blocks\n" +
                "Grants immunity to most debuffs\n" +
                "Grants immunity to some thorium and calamity debuffs\n" +
                "Allows the owner to flip gravity(toggle vanity off to disable this)");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 15));
        }

        public override void SetDefaults() {
            item.width = 20;
            item.height = 36;
            item.accessory = true;
            item.rare = ItemRarityID.Red;
            item.expert = true;
            item.value = 150000;
            item.defense = 6;
            item.expert = true;
        }

        private static Texture2D noAnimationTexture;

        internal static void LoadTexture() {
            if(Main.dedServ) return;
            noAnimationTexture = ModContent.GetTexture((typeof(SpaceSuit).Namespace + ".SpaceSuitNoAnimation").Replace('.', '/'));
        }

        internal static void UnloadTexture() {
            if(Main.dedServ) return;
            noAnimationTexture = null;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            if(MyConfig.Instance.spaceSuitAnimation) return true;
            Util.DrawTextureInUI(spriteBatch, noAnimationTexture, position, frame, drawColor, scale);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            if(MyConfig.Instance.spaceSuitAnimation) return true;
            Util.DrawTextureInWorld(spriteBatch, noAnimationTexture, item.position, lightColor);
            return false;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            Util.SetArcticDivingGear(player);
            player.ignoreWater = true;
            if (!hideVisual) player.gravControl = true;
            player.gravity = 0.4f;

            player.noKnockback = true;
            player.fireWalk = true;
            var b = player.buffImmune;
            player.GetModPlayer<MyPlayer>().spaceSuit = true;

            b[BuffID.Bleeding] = true;
            b[BuffID.BrokenArmor] = true;
            b[BuffID.Confused] = true;
            b[BuffID.Cursed] = true;
            b[BuffID.Darkness] = true;
            b[BuffID.Poisoned] = true;
            b[BuffID.Silenced] = true;
            b[BuffID.Slow] = true;
            b[BuffID.Weak] = true;
            b[BuffID.Chilled] = true;

            b[BuffID.OnFire] = true;
            b[BuffID.Venom] = true;
            b[BuffID.Blackout] = true;
            b[BuffID.OgreSpit] = true;
            b[BuffID.WitheredArmor] = true;
            b[BuffID.WitheredWeapon] = true;
            b[BuffID.CursedInferno] = true;
            b[BuffID.Ichor] = true;
            b[BuffID.Frozen] = true;
            b[BuffID.Webbed] = true;
            b[BuffID.Stoned] = true;
            b[BuffID.VortexDebuff] = true;
            b[BuffID.Obstructed] = true;
            b[BuffID.Electrified] = true;
            b[BuffID.WindPushed] = true;
            b[BuffID.Frostburn] = true;
            b[BuffID.ShadowFlame] = true;
            b[BuffID.Dazed] = true; 

            if (UpgradedAccessories.calamityLoaded) {
                b[UpgradedAccessories.calamity.BuffType("BrimstoneFlames")] = true;
                b[UpgradedAccessories.calamity.BuffType("HolyFlames")] = true;
                b[UpgradedAccessories.calamity.BuffType("GlacialState")] = true;
            }
            if (UpgradedAccessories.thoriumLoaded) {
                b[UpgradedAccessories.thorium.BuffType("GraniteSurge")] = true;
                b[UpgradedAccessories.thorium.BuffType("Staggered")] = true;
                b[UpgradedAccessories.thorium.BuffType("Liquefied")] = true;
            }
        }

        public override void AddRecipes() {
            var r = new ModRecipe(mod);
            r.SetResult(this);
            r.AddTile(TileID.LunarCraftingStation);
            r.AddIngredient(ModContent.ItemType<AnkhSuit>());
            r.AddIngredient(ItemID.GravityGlobe);
            r.AddIngredient(ItemID.LunarBar, 8);
            r.AddRecipe();
        }
    }
}
