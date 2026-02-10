using DDmod.Content.Projectiles.Talisman;

namespace DDmod.Content.Items.Talisman
{
    public class HunyuanPearlUmbrellaItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 1, 80, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<HunyuanPearlUmbrella>();
            Item.accessory = true;
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.Resist, 3600, 1200);
            Item.DItem().DrawMelee = true;
            Item.crit = 20;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.Titem().TalismanSpawning(Item, player,TalismanPlayer.混元伞);
        }
    }
}