using DDmod.Content.Projectiles.Talisman;

namespace DDmod.Content.Items.Talisman
{

    public class EnchantedVoodooDollItme : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 5000;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 1, 80, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<EnchantedVoodooDoll>();
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.Damage, 900, 1200);
            Item.crit = 15;
            Item.master = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.Titem().TalismanSpawning(Item, player, TalismanPlayer.巫毒娃娃);
        }
    }
}