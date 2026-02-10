using DDmod.Content.Projectiles.Talisman;

namespace DDmod.Content.Items.Talisman
{
    public class DeadLeavesSpiritItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 1, 80, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<DeadLeavesSpirit>();
            Item.accessory = true;
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.Restoration, DDHelper.Second(40), DDHelper.Second(10));
            Item.crit = 10;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.Titem().TalismanSpawning(Item, player, TalismanPlayer.枯叶灵);
        }
    }
}