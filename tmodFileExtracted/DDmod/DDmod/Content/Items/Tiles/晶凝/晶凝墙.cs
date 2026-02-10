using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.Trophy;
using DDmod.Content.Tiles.晶凝.Walls;
using DDmod.Content.Tiles.花岗岩基地;
using DDmod.Content.Tiles.花岗岩基地.Walls;
using Terraria.ID;

namespace DDmod.Content.Items.Tiles.晶凝
{
    public class 晶凝墙 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 2;
            Item.useTime = 2;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 0, 4);
            Item.createWall = ModContent.WallType<晶凝墙Tile>();
            Item.placeStyle = 0;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient(ModContent.ItemType<晶凝石块>(), 1).AddTile(TileID.WorkBenches).Register();
        }
    }
}