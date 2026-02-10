using DDmod.Content.Projectiles.Talisman;

namespace DDmod.Content.Items.Talisman
{
    public class 鬼牙 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 41, 80, 0);
            Item.rare = 11;
            Item.shoot = ModContent.ProjectileType<鬼牙Proj>();
            Item.accessory = true;
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.Restoration, DDHelper.Second(60), DDHelper.Second(20));
            Item.crit = 20;
            Item.master = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.Titem().TalismanSpawning(Item, player, TalismanPlayer.鬼牙);
        }
    }
}