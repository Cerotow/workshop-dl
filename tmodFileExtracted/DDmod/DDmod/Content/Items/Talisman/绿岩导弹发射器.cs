using DDmod.Content.Projectiles.Talisman;

namespace DDmod.Content.Items.Talisman
{
    public class 绿岩导弹发射器 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 1, 80, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<绿岩导弹发射器Proj>();
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.Damage, DDHelper.Second(20), 30);
            Item.crit = 15;
            Item.expert = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.Titem().TalismanSpawning(Item, player, TalismanPlayer.绿岩导弹发射器);
        }
    }
}