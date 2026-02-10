using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace DDmod.Content.Items.农场.食物
{
	public class 棒棒土豆 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 26;
			Item.height = 30;
			Item.holdStyle = 1;
			Item.value = Item.buyPrice(0, 1, 0, 0);
		}
        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
			player.itemLocation.X = player.position.X + (float)player.width * 0.5f + (float)(8 * player.direction);
			if (player.whoAmI == Main.myPlayer)
			{
				int num5 = (int)(player.itemLocation.X + (float)heldItemFrame.Width * 0.8f * (float)player.direction) / 16;
				int num6 = (int)(player.itemLocation.Y + player.HeightOffsetHitboxCenter + (float)(heldItemFrame.Height / 2)) / 16;

				Tile tile = Main.tile[num5, num6];
				if (tile == null)
					tile = new Tile();

				if (tile.HasTile && TileID.Sets.Campfire[tile.TileType] && tile.TileFrameY < 54)
				{
					player.miscTimer++;
					if (Main.rand.NextBool(5))
						player.miscTimer++;

					if (player.miscTimer > 1800)
					{
						player.miscTimer = 0;
						Item.SetDefaults(ModContent.ItemType<烤土豆>());
						if (player.selectedItem == 58)
							Main.mouseItem.SetDefaults(ModContent.ItemType<烤土豆>());

						for (int k = 0; k < 58; k++)
						{
							if (player.inventory[k].type == Item.type && k != player.selectedItem && player.inventory[k].stack < player.inventory[k].maxStack)
							{
								SoundEngine.PlaySound(SoundID.Item7);
								player.inventory[k].stack++;
					    		Item.SetDefaults();
								if (player.selectedItem == 58)
									Main.mouseItem.SetDefaults();
							}
						}
					}
				}
				else
				{
					player.miscTimer = 0;
				}
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<土豆>()).AddIngredient(ItemID.Wood).Register();
		}
	}
}