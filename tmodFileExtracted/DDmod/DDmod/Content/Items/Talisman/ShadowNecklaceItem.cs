using DDmod.Content.Projectiles.Talisman;

namespace DDmod.Content.Items.Talisman
{
    public class ShadowNecklaceItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 1, 80, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<ShadowNecklace>();
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.Damage, DDHelper.Second(30), DDHelper.Second(20));
            Item.crit = 15;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.Titem().TalismanSpawning(Item, player, TalismanPlayer.暗影项链);
        }
    }
}