using DDmod.Content.Dusts;

namespace DDmod.Content.Items.Series.仙人掌
{
    public class 仙人掌之魂 : ModItem
    {
        public override void SetDefaults()
        {

            Item.width = 30;
            Item.height = 44;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(0, 0, 2, 0);
            Item.rare = ItemRarityID.Orange;
        }

        public override void SetStaticDefaults()
        {

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 6));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }
        int F;
        int F2;
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            F++;
            if (F % 4 == 0)
            {
                F2++;
            }
            if (F2 >= 6)
            {
                F2 = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Item[Item.type];
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, new Rectangle?(new Rectangle(0, TextureAssets.Item[Item.type].Height() / 6 * F2, TextureAssets.Item[Item.type].Width(), TextureAssets.Item[Item.type].Height() / 6)), alphaColor);
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            maxFallSpeed = 0f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}