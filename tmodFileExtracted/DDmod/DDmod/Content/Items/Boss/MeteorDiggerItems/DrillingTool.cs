
namespace DDmod.Content.Items.Boss.MeteorDiggerItems
{
    public class DrillingTool : ModItem
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
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.expert = true;
        }
        public override LocalizedText Tooltip => base.Tooltip;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.pickSpeed *= 0.7f;
            if (player.whoAmI == Main.myPlayer)
            {
                Player.tileRangeX += 5;
                Player.tileRangeY += 5;
            }
        }
    }
}
