using Terraria.ID;

namespace DDmod.Content.Items.Series.Venture
{
    public class StrengtheningStone : ModItem
    {
        public override void SetDefaults()
        {

            Item.width = 30;
            Item.height = 24;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Green;
            Item.GetGlobalItem<StrengthenGlobalItem>().Probabilitys(40, 20, 2);
        }

        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Strengthening Stone(Tier 1)");
           //DisplayName.AddTranslation(7, "一级强化石");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }
    }
}