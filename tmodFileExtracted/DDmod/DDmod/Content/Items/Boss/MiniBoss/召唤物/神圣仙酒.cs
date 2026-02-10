using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DDmod.Content.Items.农场;
using DDmod.Content.Items.Melee.Sword;
using Microsoft.Xna.Framework.Graphics;

namespace DDmod.Content.Items.Boss.MiniBoss.召唤物
{
	public class 神圣仙酒 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 30;
			Item.rare = ItemRarityID.Green;
			Item.consumable = true;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item3;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.useTurn = true;
			Item.buffType = ModContent.BuffType<神圣仙酒Buff>();
			Item.buffTime = DDHelper.Second(300);
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsFood[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}
        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<FoodPlayer>().color = new Color(255, 191, 0, 100);
			
            return base.UseItem(player);
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Items/Boss/MiniBoss/召唤物/神圣仙酒_Item");
            Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Items/Boss/MiniBoss/召唤物/神圣仙酒_Glow");
            scale *= 0.8f;
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), scale, (SpriteEffects)1, 0f);
            spriteBatch.Draw(texture2, position, null, Color.White, 0, new Vector2(texture2.Width / 2, texture2.Height / 2), scale, (SpriteEffects)1, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Items/Boss/MiniBoss/召唤物/神圣仙酒_Item");
            Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("DDmod/Content/Items/Boss/MiniBoss/召唤物/神圣仙酒_Glow");
            spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, lightColor, 0, new Vector2(texture.Width / 2, texture.Height / 2), scale, (SpriteEffects)1, 0f);
            spriteBatch.Draw(texture2, Item.Center - Main.screenPosition, null, lightColor, 0, new Vector2(texture2.Width / 2, texture2.Height / 2), scale, (SpriteEffects)1, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(351,1).AddIngredient(501, 20).AddIngredient(526, 1).AddTile(TileID.Kegs).Register();
        }
    }
}