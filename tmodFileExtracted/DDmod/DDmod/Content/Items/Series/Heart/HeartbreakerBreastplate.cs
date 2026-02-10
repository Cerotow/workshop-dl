using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.Heart
{
	[AutoloadEquip(EquipType.Body)]
	public class HeartbreakerBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 0, 30);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}
		public override void UpdateEquip(Player player)
		{
			player.endurance += 0.05f;
			player.lifeRegen += 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<HeartIngot>(), 15).AddTile(TileID.Anvils).Register();
		}
	}
}