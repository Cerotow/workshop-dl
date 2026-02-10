using DDmod.Content.NPCs.Boss.LifeGuardLes;
using Terraria.ID;

namespace DDmod.Content.Items.Sundries
{
    public class 白色委托币 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 1;
            Item.maxStack = Item.CommonMaxStack;
        }
        public override void HoldItem(Player player)
        {
            player.Dplayer().AdventureCoins += Item.stack;
            Main.mouseItem.SetDefaults(0);
        }
        public override void UpdateInventory(Player player)
        {
            player.Dplayer().AdventureCoins += Item.stack;
            Item.SetDefaults(0);
        }
    }
}
    