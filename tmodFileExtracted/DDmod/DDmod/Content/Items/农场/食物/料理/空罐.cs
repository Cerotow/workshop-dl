using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.NPCs.Boss.天地守卫;
using DDmod.Content.NPCs.Boss.蘑菇王;
using DDmod.Content.Items.Series.Heart;
using DDmod.Content.Items.Series.钢;

namespace DDmod.Content.Items.农场.食物.料理
{
	public class 空罐 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 30;
			Item.rare = 0;
			Item.consumable = true;
			Item.useTime = 20;
			Item.useAnimation = 20;
            Item.noMelee = true;
			Item.value = Item.buyPrice(0, 0, 1, 50);
		}

		public override void SetStaticDefaults()
        {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<钢锭>()).AddTile(TileID.Anvils).Register();
        }
    }
}