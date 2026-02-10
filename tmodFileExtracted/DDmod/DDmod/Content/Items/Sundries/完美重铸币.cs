using DDmod.Content.NPCs.Boss.LifeGuardLes;
using Terraria.ID;

namespace DDmod.Content.Items.Sundries
{
    public class 完美重铸币 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = 6;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
        }
        public override void HoldItem(Player player)
        {
        }
        public override void UpdateInventory(Player player)
        {
        }
    }
}
    