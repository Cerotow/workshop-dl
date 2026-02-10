namespace DDmod.Content.Items.Boss.恐惧缝合体
{
    public class 恐惧肉块 : ModItem
    {
        public override void SetDefaults()
        {

            Item.width = 30;
            Item.height = 24;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.Orange;
            Item.GetGlobalItem<StrengthenGlobalItem>().Enchantments(StrengthenGlobalItem.生命, 10, 25, 10, StrengthenGlobalItem.生命, 20, 40, 5);
        }

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }
    }
}