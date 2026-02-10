using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DDmod.Content.Items.Series.Heart
{
	[AutoloadEquip(EquipType.Legs)]
	public class HeartbreakerLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.05f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<HeartIngot>(), 8).AddTile(TileID.Anvils).Register();
		}
	}
}