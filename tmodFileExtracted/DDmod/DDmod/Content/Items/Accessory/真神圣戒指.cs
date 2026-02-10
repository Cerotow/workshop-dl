using DDmod.Content.Items.Series.杂物;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Accessory
{
    public class 真神圣戒指 : ModItem
    {
        public override void SetStaticDefaults()
        {
            RecipesSystem.Add(ModContent.ItemType<神圣戒指>(), 3, new Item(520, 20), new Item(521, 20), new Item(ModContent.ItemType<叶绿龟壳>()), new Item(Type));
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = 6;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Aplayer().HolyEnergy = true;
            player.Aplayer().TrueHolyEnergy = true;
            player.Aplayer().真神圣戒指 = true;
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => false);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<神圣戒指>()).AddIngredient(520, 20).AddIngredient(521, 20).AddIngredient(ModContent.ItemType<叶绿龟壳>()).AddCondition(text).Register();
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<神圣戒指>() || equippedItem.type == ModContent.ItemType<泰拉之庇>())
            {
                return false;
            }
            return true;
        }



    }
}
