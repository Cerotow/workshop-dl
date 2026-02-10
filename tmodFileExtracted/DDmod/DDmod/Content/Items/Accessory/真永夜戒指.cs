using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Accessory
{
    public class 真永夜戒指 : ModItem
    {
        public override void SetStaticDefaults()
        {
            RecipesSystem.Add(ModContent.ItemType<永夜戒指>(), 2, new Item(547,20), new Item(548, 20), new Item(549, 20), new Item(Type));
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
            player.Aplayer().TrueNightEnergy = true;
            player.Aplayer().NightEnergy = true;
            player.Aplayer().真永夜戒指 = true;
            if (hideVisual)
            {
                player.Aplayer().YYTime = 240;
            }
            player.statLifeMax2 += 50;
            player.statManaMax2 += 80;
            player.lifeRegen+=2;
            player.manaRegen += 4;
            player.GetAttackSpeed(DamageClass.Generic) += 0.15F;
            player.moveSpeed += 0.4F;
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => false);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<永夜戒指>()).AddIngredient(547,20).AddIngredient(548, 20).AddIngredient(549, 20).AddCondition(text).Register();
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<永夜戒指>() || equippedItem.type == ModContent.ItemType<泰拉之庇>())
            {
                return false;
            }
            return true;
        }


    }
}
