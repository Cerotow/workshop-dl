using DDmod.Content.NPCs.Boss.LifeGuardLes;

namespace DDmod.Content.Items.Sundries
{
    public class 潮汐之泪 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = Item.CommonMaxStack;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.consumable = true;
            Item.shoot = ModContent.ProjectileType<潮汐之泪proj>();
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }
    }
}
    