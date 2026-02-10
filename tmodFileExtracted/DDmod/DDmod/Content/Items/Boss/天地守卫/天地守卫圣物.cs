using DDmod.Content.Tiles.Relic;

namespace DDmod.Content.Items.Boss.天地守卫
{
    public class 天地守卫圣物 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<天地守卫圣物Tile>(), 0);

            Item.width = 30;
            Item.height = 40;
            Item.maxStack = Item.CommonMaxStack;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.buyPrice(0, 3, 0, 0);
        }
    }
}