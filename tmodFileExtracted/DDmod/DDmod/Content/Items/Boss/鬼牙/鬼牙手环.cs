using DDmod.Content.Projectiles.OrnamentProjectile;

namespace DDmod.Content.Items.Boss.鬼牙
{
    public class 鬼牙手环 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Aplayer().GhostfangBracelet = true;
        }
    }
}
