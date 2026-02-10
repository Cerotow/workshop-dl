using DDmod.Content.Tiles.Relic;

namespace DDmod.Content.Items.Boss.流星破坏者
{
    public class 流星电池 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;
            Item.maxStack = Item.CommonMaxStack;
            Item.rare = 3;
            Item.value = Item.buyPrice(0, 0, 1, 0);
        }
    }
}