using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.Trophy;
using DDmod.Content.Tiles.晶凝;
using Terraria.ID;

namespace DDmod.Content.Items.Tiles.晶凝
{
    public class 晶凝书柜 : ModItem
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
            Item.value = Item.buyPrice(0, 0, 1, 0);
            Item.createTile = ModContent.TileType<晶凝书柜Tile>();
            Item.placeStyle = 0;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<晶凝石块>(), 20).AddIngredient(149, 10).AddTile(TileID.Sawmill).Register();
        }
    }
}