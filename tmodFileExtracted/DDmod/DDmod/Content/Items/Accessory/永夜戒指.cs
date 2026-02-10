using DDmod.AccessorySlot;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Projectiles.GeneralProj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Accessory
{
    public class 永夜戒指 : ModItem
    {
        public override void SetStaticDefaults()
        {
            RecipesSystem.Add(ModContent.ItemType<魔光戒指>(), 1, ModContent.ItemType<荆棘戒指>(), ModContent.ItemType<远古魔戒>(), ModContent.ItemType<火山戒指>(), Type);
            RecipesSystem.Add(ModContent.ItemType<血腥戒指>(), 1, ModContent.ItemType<荆棘戒指>(), ModContent.ItemType<远古魔戒>(), ModContent.ItemType<火山戒指>(), Type);
        }

        public override void SetDefaults()
        {
            Item.width = 28; 
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare =4;
            Item.accessory = true;
        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Aplayer().永夜戒指 = true;
            player.Aplayer().NightEnergy = true;
            if (hideVisual)
            {
                player.Aplayer().YYTime = 240;
            }
            player.statLifeMax2 += 25;
            player.statManaMax2 += 40;
            player.lifeRegen++;
            player.manaRegen+=2;
            player.GetAttackSpeed(DamageClass.Generic) += 0.1F;
            player.moveSpeed += 0.2F;
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<真永夜戒指>() || equippedItem.type == ModContent.ItemType<泰拉之庇>())
            {
                return false;
            }
            return true;
        }


        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => false);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<魔光戒指>()).AddIngredient(ModContent.ItemType<荆棘戒指>()).AddIngredient(ModContent.ItemType<远古魔戒>()).AddIngredient(ModContent.ItemType<火山戒指 > ()).AddCondition(text).Register();
            CreateRecipe(1).AddIngredient(ModContent.ItemType<血腥戒指>()).AddIngredient(ModContent.ItemType<荆棘戒指>()).AddIngredient(ModContent.ItemType<远古魔戒>()).AddIngredient(ModContent.ItemType<火山戒指 > ()).AddCondition(text).Register();
        }
    }
}
