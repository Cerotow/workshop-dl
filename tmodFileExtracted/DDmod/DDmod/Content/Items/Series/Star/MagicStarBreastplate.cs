using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.Star
{
	[AutoloadEquip(EquipType.Body)]
	public class MagicStarBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.buyPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.manaRegen += 2;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<StarIngot>(), 15).AddTile(TileID.Anvils).Register();
		}
	}
}