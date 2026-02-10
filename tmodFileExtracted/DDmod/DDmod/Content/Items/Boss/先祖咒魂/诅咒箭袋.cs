namespace DDmod.Content.Items.Boss.先祖咒魂
{
    public class 诅咒箭袋 : ModItem
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
            Item.value = Item.buyPrice(0, 10, 50, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Aplayer().暗影焰力量 = 1;
            player.arrowDamage += 0.1f;
            if(player.ActiveItem().useAmmo==AmmoID.Bullet)
            {
                int P = Item.prefix;
                Item.SetDefaults(ModContent.ItemType<诅咒弹药箱>());
                Item.Prefix(P);
            }
        }
    }
}
