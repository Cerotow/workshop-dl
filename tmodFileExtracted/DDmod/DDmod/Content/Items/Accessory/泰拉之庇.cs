using DDmod.Content.Items.Series.Acorn;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Accessory
{
    public class 泰拉之庇 : ModItem
    {
        public override void SetStaticDefaults()
        {
            RecipesSystem.Add(ModContent.ItemType<英雄断戒>(), 4, new Item(ModContent.ItemType<真永夜戒指>()), new Item(ModContent.ItemType<真神圣戒指>()), new Item(ModContent.ItemType<SoulOfNature>(), 50), new Item(Type));
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.rare = 8;
            Item.accessory = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.泰拉级;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Aplayer().TrueNightEnergy = true;
            player.Aplayer().NightEnergy = true;
            player.Aplayer().TrueHolyEnergy = true;
            player.Aplayer().HolyEnergy = true;
            player.Aplayer().泰拉戒指 = true;
            if(hideVisual)
            {
                player.Aplayer().YYTime = 240;
            }
            player.statLifeMax2 += 100;
            player.statManaMax2 += 120;
            player.lifeRegen += 3;
            player.manaRegen += 6;
            player.GetAttackSpeed(DamageClass.Generic) += 0.2F;
            player.moveSpeed += 0.5F;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, frame, drawColor, 0, origin, scale * 1.3F, 0, 0);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw(texture, Item.position + new Vector2(Item.width / 2, Item.height) - Main.screenPosition, null, alphaColor, rotation, new Vector2(texture.Width / 2, texture.Height - 10), scale, 0, 0);

            return false;
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => false);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<英雄断戒>()).AddIngredient(ModContent.ItemType<真永夜戒指>()).AddIngredient(ModContent.ItemType<真神圣戒指>()).AddIngredient(ModContent.ItemType<SoulOfNature>(), 50).AddCondition(text).Register();
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<神圣戒指>() || equippedItem.type == ModContent.ItemType<真神圣戒指>()|| equippedItem.type == ModContent.ItemType<真永夜戒指>()|| equippedItem.type == ModContent.ItemType<永夜戒指>())
            {
                return false;
            }
            return true;
        }


    }
}
