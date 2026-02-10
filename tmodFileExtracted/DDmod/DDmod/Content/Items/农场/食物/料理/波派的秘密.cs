using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.NPCs.Boss.蘑菇王;
using DDmod.Content.NPCs.Boss.夜光蘑菇王;

namespace DDmod.Content.Items.农场.食物.料理
{
	public class 波派的秘密 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.consumable = true;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item3;
			Item.GetGlobalItem<FoodGlobalItem>().伤害buff(0.07f, DDHelper.Second(240),DamageClass.Melee);
			Item.GetGlobalItem<FoodGlobalItem>().防御buff(8, DDHelper.Second(420));
            Item.GetGlobalItem<FoodGlobalItem>().移速buff(0.25F, DDHelper.Second(180));
            Item.GetGlobalItem<FoodGlobalItem>().挖掘buff(0.2F,DDHelper.Second(600));
            Item.GetGlobalItem<FoodGlobalItem>().夜视buff(DDHelper.Second(600));
			Item.GetGlobalItem<FoodGlobalItem>().Food = true;
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.useTurn = true;
        }

		public override void SetStaticDefaults()
        {
            ItemID.Sets.DrinkParticleColors[Item.type] = new Color[] {
                new Color(100, 255, 100),
            };
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override bool? UseItem(Player player)
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<空罐>(), 1).AddIngredient(ModContent.ItemType<生菜>(), 5).AddIngredient(ModContent.ItemType<玉米>(), 1).AddIngredient(ModContent.ItemType<土豆>(), 3).AddIngredient(ModContent.ItemType<胡萝卜>(), 3).AddTile(TileID.CookingPots).AddTile(16).Register();
        }
    }
}