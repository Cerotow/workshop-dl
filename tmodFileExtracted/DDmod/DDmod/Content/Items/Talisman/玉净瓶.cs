using DDmod.Content.Projectiles.Talisman;

namespace DDmod.Content.Items.Talisman
{
    public class 玉净瓶 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.crit = 20;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 1, 80, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<玉净瓶Proj>();
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.Restoration, DDHelper.Second(30), DDHelper.Second(5));
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.Titem().TalismanSpawning(Item, player, TalismanPlayer.玉净瓶);
        }
    }
}