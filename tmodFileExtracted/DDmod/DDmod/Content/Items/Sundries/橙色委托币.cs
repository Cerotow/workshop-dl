using DDmod.Content.NPCs.Boss.LifeGuardLes;
using Terraria.ID;

namespace DDmod.Content.Items.Sundries
{
    public class 橙色委托币 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 2;
            Item.maxStack = Item.CommonMaxStack;
        }
        public override void HoldItem(Player player)
        {
            player.Dplayer().AdventureCoins5 += Item.stack;
            Main.mouseItem.SetDefaults(0);
        }
        public override void UpdateInventory(Player player)
        {
            player.Dplayer().AdventureCoins5 += Item.stack;
            Item.SetDefaults(0);
        }
    }
}
    