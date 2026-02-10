using DDmod.Content.Tiles.Trophy;

namespace DDmod.Content.Items.Boss.MeteorDiggerItems
{
    public class MeteorDiggerTrophyItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.createTile = ModContent.TileType<MeteorDiggerTrophy>();
            Item.placeStyle = 0;
        }

        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Meteor Excavator Trophy");
           //DisplayName.AddTranslation(7, "流星掘地者纪念章");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
    }
}