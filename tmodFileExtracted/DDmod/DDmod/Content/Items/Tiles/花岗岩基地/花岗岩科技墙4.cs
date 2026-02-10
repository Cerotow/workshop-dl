using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.Trophy;
using DDmod.Content.Tiles.花岗岩基地;
using DDmod.Content.Tiles.花岗岩基地.Walls;
using Terraria.ID;

namespace DDmod.Content.Items.Tiles.花岗岩基地
{
    public class 花岗岩科技墙4 : ModItem
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
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.createWall = ModContent.WallType<花岗岩科技墙4Tile>();
            Item.placeStyle = 0;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override void AddRecipes()
        {
            //CreateRecipe(1).AddIngredient(ModContent.ItemType<晶凝石块>(), 8).AddTile(TileID.Anvils).Register();
        }
    }
}