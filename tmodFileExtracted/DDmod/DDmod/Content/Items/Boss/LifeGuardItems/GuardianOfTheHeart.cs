using DDmod.Content.Projectiles.OrnamentProjectile;

namespace DDmod.Content.Items.Boss.LifeGuardItems
{
    public class GuardianOfTheHeart : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.expert = true;
            //Item.canBePlacedInVanityRegardlessOfConditions = true;
        }

        private int T;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 25;
            player.AddBuff(ModContent.BuffType<ServantOfTheHeartBuff>(), 2);
            player.Aplayer().ServantOfTheHeart = true;
            if (NPCdirection.FindClosest(player.Center, 250) != null)
            {
                Vector2 vector = NPCdirection.FindClosest(player.Center, 250).Center - player.Center;

                if (vector.Length() < 250)
                {
                    int Proj = 0;
                    for (int a = 0; a < 5; a++)
                    {
                        if (player.ownedProjectileCounts[ModContent.ProjectileType<ServantOfTheHeart>()] < 5)
                        {
                            T = NewProjectile(player.GetSource_FromAI(), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<ServantOfTheHeart>(), 15, 3f, player.whoAmI, Proj, T);
                            Main.projectile[T].originalDamage = 15;
                            Proj++;
                        }
                    }
                }
            }
        }
        public float T1 = 1;
        public float T2 = 1.33f;
        public float T3 = 1.66f;
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            T1 += 0.01f;
            T2 += 0.01f;
            T3 += 0.01f;
            if (T1 >= 2f) T1 -= 1;
            if (T2 >= 2f) T2 -= 1;
            if (T3 >= 2f) T3 -= 1;
            Texture2D texture = (Texture2D)TextureAssets.Item[Item.type];

            for (int A = 0; A < 1; A++)
            {
                Color color = new Color(255, 100, 0, 0) * (1 - T1 + 1);
                spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, color, 0, new Vector2(texture.Width / 2, texture.Height / 2),  scale * T1, 0, 0f);
                color = new Color(255, 100, 0, 0) * (1 - T2 + 1);
                spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, color, 0, new Vector2(texture.Width / 2, texture.Height / 2),  scale * T2, 0, 0f);
                color = new Color(255, 100, 0, 0) * (1 - T3+ 1);
                spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, color, 0, new Vector2(texture.Width / 2, texture.Height / 2),  scale * T3, 0, 0f);
            }
            spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), scale, 0, 0f);
            return false;
        }
        float a;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D Backtexture = (Texture2D)TextureAssets.InventoryBack;
            a += 0.1f;
            Texture2D texture = (Texture2D)TextureAssets.Item[Item.type];

            T1 += 0.01f;
            T2 += 0.01f;
            T3 += 0.01f;
            if (T1 >= 2f) T1 -= 1;
            if (T2 >= 2f) T2 -= 1;
            if (T3 >= 2f) T3 -= 1;

            for (int A = 0; A < 1; A++)
            {
                Color color = new Color(255, 100, 0, 0) * (1 - T1 + 1);
                spriteBatch.Draw(texture, position, null,color, 0, new Vector2(texture.Width / 2, texture.Height / 2),  scale * T1, 0, 0f);
                color = new Color(255, 100, 0, 0) * (1 - T2 + 1);
                spriteBatch.Draw(texture, position, null, color, 0, new Vector2(texture.Width / 2, texture.Height / 2),  scale * T2, 0, 0f);
                color = new Color(255, 100, 0, 0) * (1 - T3 + 1);
                spriteBatch.Draw(texture, position, null, color, 0, new Vector2(texture.Width / 2, texture.Height / 2),  scale * T3, 0, 0f);
            }
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), scale, 0, 0f);

            return false;
        }
    }
}
