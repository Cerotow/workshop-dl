using Terraria.ID;

namespace DDmod.Content.Items.Series.Venture
{
    public class StrengtheningStone3 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(0, 2, 5, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.GetGlobalItem<StrengthenGlobalItem>().Probabilitys(100,80,40,20,2);
        }

        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Strengthening Stone (Tier 2)");
           //DisplayName.AddTranslation(7, "二级强化石");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }
    }
}