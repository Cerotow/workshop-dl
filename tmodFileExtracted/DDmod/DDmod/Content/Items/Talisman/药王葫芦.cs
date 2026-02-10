using DDmod.Content.Projectiles.Talisman;
using Terraria;

namespace DDmod.Content.Items.Talisman
{
    public class 药王葫芦 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage =0;
            Item.crit = 20;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(0, 10, 80, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<药王葫芦Proj>();
            Item.Titem().SetTalismanDefaults(Item, TalismanTypes.Restoration, DDHelper.Second(30), DDHelper.Second(18));
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.statLife<= player.Aplayer().LifeMax / 2)
            {
                Item.damage = player.Aplayer().LifeMax/100;
            }
            else
            {
                Item.damage = 0;
            }
            Item.Titem().TalismanSpawning(Item, player, TalismanPlayer.药王葫芦);
        }
    }
}