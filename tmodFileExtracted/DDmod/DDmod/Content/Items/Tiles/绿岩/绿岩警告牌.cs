using DDmod.Content.Items.Boss.绿岩之视;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.Trophy;
using DDmod.Content.Tiles.晶凝;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Tiles.绿岩.家具;
using Terraria.ID;

namespace DDmod.Content.Items.Tiles.绿岩
{
    public class 绿岩警告牌 : ModItem
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
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.createTile = ModContent.TileType<绿岩警告牌Tile>();
            Item.placeStyle = 0;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
    }
}