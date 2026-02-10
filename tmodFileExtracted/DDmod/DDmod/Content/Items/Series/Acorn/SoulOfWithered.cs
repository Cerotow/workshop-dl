using DDmod.Content.Dusts;

namespace DDmod.Content.Items.Series.Acorn
{
    public class SoulOfWithered : ModItem
    {
        public override void SetDefaults()
        {

            Item.width = 22;
            Item.height = 22;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(0, 0, 2, 0);
            Item.rare = ItemRarityID.Orange;
        }

        public override void SetStaticDefaults()
        {

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }
        int F;
        int F2;
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            F++;
            if (F % 8 == 0)
            {
                F2++;
            }
            if (F2 >= 4)
            {
                F2 = 0;
            }
            Texture2D texture = (Texture2D)TextureAssets.Item[Item.type];
            spriteBatch.Draw(texture, Item.position - Main.screenPosition, new Rectangle?(new Rectangle(0, TextureAssets.Item[Item.type].Height() / 4 * F2, TextureAssets.Item[Item.type].Width(), TextureAssets.Item[Item.type].Height() / 4)), alphaColor);
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            if (Main.rand.NextBool(10))
                NewDust(Item.position, Item.width, Item.height, ModContent.DustType<枯萎粒子>(), 0f, -5f, 0, default, 1f);
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            maxFallSpeed = 0f;
            for(int a = 0;a<1000;a++)
            {
                if (Main.projectile[a].active&&Main.projectile[a].type==10)
                {
                    bool A = new Rectangle((int)Main.projectile[a].position.X, (int)Main.projectile[a].position.Y, Main.projectile[a].width, Main.projectile[a].height).Intersects(new Rectangle((int)Item.position.X, (int)Item.position.Y, Item.width, Item.height));
                    if (A)
                    {
                        int Y = Item.stack;
                        Item.SetDefaults(ModContent.ItemType<SoulOfNature>());
                        Item.stack = Y;
                    }
                }
            }
        }
    }
}