using DDmod.Content.Projectiles.Talisman;

namespace DDmod.Content.Items.Talisman
{
    public class 魔力菇 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 3;
            Item.shoot = ModContent.ProjectileType<魔力菇Proj>();
            Item.accessory = true;
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.RestorationMana, DDHelper.Second(5), DDHelper.Second(10));
            Item.crit = 10;
            Item.master = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.Titem().TalismanSpawning(Item, player, TalismanPlayer.魔力菇);
        }
    }
}