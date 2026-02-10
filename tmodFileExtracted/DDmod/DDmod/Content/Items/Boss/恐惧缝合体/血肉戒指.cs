namespace DDmod.Content.Items.Boss.恐惧缝合体
{
    public class 血肉戒指 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 1, 50, 0);
            Item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Aplayer().血肉戒指 = true;
        }
    }
}
