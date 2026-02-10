
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Potion
{
	public class 飞翔药水 : ModItem
	{
        public override void SetDefaults()
        {
            Item.maxStack = Item.CommonMaxStack;
            Item.width = 26;
            Item.height = 30;
            Item.rare =5;
            Item.consumable = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item3;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.useTurn = true;
            Item.buffType = ModContent.BuffType<飞翔药水Buff>();
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
            CreateRecipe(1).AddIngredient(ModContent.ItemType<弱效飞翔药水>(), 1).AddIngredient(575, 2).AddTile(13).Register();
        }
    }
}