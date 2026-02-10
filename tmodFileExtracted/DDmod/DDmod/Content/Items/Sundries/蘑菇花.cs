using DDmod.Content.NPCs.Boss.LifeGuardLes;
using Terraria.ID;

namespace DDmod.Content.Items.Sundries
{
    public class 蘑菇花 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<奇幻花>();

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
            return player.Dplayer().statManaMax >= 100 && player.Dplayer().statManaMax < 200;
        }
        public override bool? UseItem(Player player)
        {
            if(player.Dplayer().statManaMax >= 100 && player.Dplayer().statManaMax < 200)
            {
                player.Dplayer().statManaMax += 5;
                player.ManaEffect(5);
                if(player.Dplayer().statManaMax > 200)
                {
                    player.Dplayer().statManaMax = 200;
                }
            }
            return true;
        }
    }
}
    