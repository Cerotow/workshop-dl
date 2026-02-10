using DDmod.Content.Items.Boss.鬼牙;
using DDmod.Content.NPCs.Boss.鬼牙;
using DDmod.Content.Tiles.EquipTiles;
using DDmod.Content.Tiles.Trophy;
using DDmod.Content.Tiles.晶凝;
using DDmod.Content.Tiles.杂物块;
using DDmod.Content.Tiles.绿岩.家具;
using Terraria.ID;

namespace DDmod.Content.Items.Tiles
{
    public class 蓝钢匣 : ModItem
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
            Item.value = Item.buyPrice(0, 0, 1, 0);
            Item.createTile = ModContent.TileType<蓝钢匣Tile>();
            Item.placeStyle = 0;
            Item.consumable = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            int[] A =
            [
                700,
                11,
            ];
            itemLoot.NormalLoot(1, 1, 12, 34, A);
            itemLoot.Add(ItemDropRule.Common(1922,1,4,11));

            itemLoot.Add(ItemDropRule.Coins(5000,true));
        }
    }
}