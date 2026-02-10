using DDmod.Content.NPCs.Boss.LifeGuardLes;
using Terraria.ID;

namespace DDmod.Content.Items.Sundries
{
    public class 奇幻花 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare =ItemRarityID.Lime;
            Item.maxStack = Item.CommonMaxStack;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.Dplayer().FantasyFlowers;
        }
        public override bool? UseItem(Player player)
        {
            player.Dplayer().FantasyFlowers = true;
            return true;
        }
    }
}
    