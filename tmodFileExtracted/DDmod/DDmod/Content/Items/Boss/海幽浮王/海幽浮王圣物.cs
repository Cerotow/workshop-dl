using DDmod.Content.Tiles.Relic;

namespace DDmod.Content.Items.Boss.海幽浮王
{
    public class 海幽浮王圣物 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<海幽浮王圣物Tile>(), 0);

            Item.width = 30;
            Item.height = 40;
            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.buyPrice(0, 3, 0, 0);
        }
    }
}
