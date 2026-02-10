using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace DDmod.Content.Items.农场.水壶
{
	public class 钨水壶 : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.height = 2;
			Item.width = 2;
			Item.useTurn = false;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.channel = true;
			Item.useTime = 10;
			Item.shoot = ModContent.ProjectileType<水壶Proj>();
			Item.shootSpeed = 1;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Green;
			Item.consumable = false;
			Item.value = Item.buyPrice(0, 0, 12, 0);
			Item.GetGlobalItem<水壶>().MaxWaterVolume = 1300;
			Item.GetGlobalItem<水壶>().kettle = true;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(1, 2));
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.TungstenBar, 12).AddTile(16).Register();
		}
	}
}