
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Potion
{
	public class 弱效飞翔药水 : ModItem
	{
        public override void SetDefaults()
        {
            Item.maxStack = Item.CommonMaxStack;
            Item.width = 26;
            Item.height = 30;
            Item.rare = 3;
            Item.consumable = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item3;
            Item.value = Item.buyPrice(0, 0, 5, 0);
            Item.useTurn = true;
            Item.buffType = ModContent.BuffType<弱效飞翔药水Buff>();
            Item.buffTime = DDHelper.Second(600);
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.DrinkParticleColors[Item.type] = new Color[] {
                new Color(23, 127, 186,100),
            };
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override bool? UseItem(Player player)
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(2304, 1).AddIngredient(320, 3).AddTile(13).Register();
        }
    }
}