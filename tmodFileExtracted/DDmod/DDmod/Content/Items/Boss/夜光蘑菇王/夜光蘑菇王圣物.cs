using DDmod.Content.Tiles.Relic;

namespace DDmod.Content.Items.Boss.夜光蘑菇王
{
    public class 夜光蘑菇王圣物 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<夜光蘑菇王圣物Tile>(), 0);

            Item.width = 30;
            Item.height = 40;
            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.buyPrice(0, 3, 0, 0);
        }
    }
}