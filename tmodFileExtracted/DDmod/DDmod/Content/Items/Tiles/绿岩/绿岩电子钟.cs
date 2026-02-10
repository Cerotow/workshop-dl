using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.Trophy;
using DDmod.Content.Tiles.晶凝;
using DDmod.Content.Tiles.绿岩.家具;
using Terraria.ID;

namespace DDmod.Content.Items.Tiles.绿岩
{
    public class 绿岩电子钟 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 0, 50);
            Item.createTile = ModContent.TileType<绿岩电子钟Tile>();
            Item.placeStyle = 0;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩砖>(), 10).AddRecipeGroup(RecipeGroupID.IronBar,3).AddIngredient(170, 6).AddTile(TileID.Sawmill).Register();
        }
    }
}