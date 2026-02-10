namespace DDmod.Content.Items.Series.ShadowFlame
{
    public class ShadowFlame : ModItem
    {
        public override void SetDefaults()
        {

            Item.width = 30;
            Item.height = 24;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(0, 0, 2, 0);
            Item.rare = ItemRarityID.Orange;
        }

        public override void SetStaticDefaults()
        {

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(3, 5));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }
        int F;
        int F2;
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            F++;
            if (F % 3 == 0)
            {
                F2++;
            }
            if (F2 >= 5)
            {
                F2 = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Item[Item.type];
            spriteBatch.Draw(texture, Item.position + Item.Size / 2 - Main.screenPosition, new Rectangle?(new Rectangle(0, TextureAssets.Item[Item.type].Height() / 5 * F2, TextureAssets.Item[Item.type].Width(), TextureAssets.Item[Item.type].Height() / 5)), alphaColor,0,new Vector2(TextureAssets.Item[Item.type].Width(), TextureAssets.Item[Item.type].Height() / 5)/2,1,0,0);
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
             Dust dust = Main.dust[NewDust(Item.position, Item.width, Item.height, 27, 0f, -1f, 0, default, 0.7f)];
            dust.velocity = new Vector2(0, -0.3F);
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            maxFallSpeed = 0f;
        }
    }
}