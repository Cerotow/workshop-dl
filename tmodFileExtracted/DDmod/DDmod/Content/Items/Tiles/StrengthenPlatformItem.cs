using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.Trophy;
using Terraria.ID;

namespace DDmod.Content.Items.Tiles
{
    public class StrengthenPlatformItem : ModItem
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
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.createTile = ModContent.TileType<StrengthenPlatform>();
            Item.placeStyle = 0;
        }
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Strengthen Platform");
           //DisplayName.AddTranslation(7, "强化台");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
    }
}