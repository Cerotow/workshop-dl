using DDmod.Content.Projectiles.Talisman;

namespace DDmod.Content.Items.Talisman
{
    public class 地狱葫芦 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 1, 80, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<地狱葫芦Proj>();
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.Damage, DDHelper.Second(10), 100);
            Item.crit = 15;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.Titem().TalismanSpawning(Item, player, TalismanPlayer.地狱葫芦);
        }
    }
}