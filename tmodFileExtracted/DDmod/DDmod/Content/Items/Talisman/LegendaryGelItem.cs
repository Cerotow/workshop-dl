using DDmod.Content.Projectiles.Talisman;

namespace DDmod.Content.Items.Talisman
{
    
    public class LegendaryGelItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 1, 80, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<LegendaryGel>();
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.Damage, 300, 600);
            Item.crit = 10;
        }
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Legendary Gel");
           //DisplayName.AddTranslation(7, "传奇凝胶");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.Titem().TalismanSpawning(Item, player,TalismanPlayer.传奇凝胶);
        }
    }
}