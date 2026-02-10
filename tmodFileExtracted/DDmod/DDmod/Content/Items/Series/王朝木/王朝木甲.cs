using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Series.王朝木
{
	[AutoloadEquip(EquipType.Body)]
	public class 王朝木甲 : ModItem
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
			Item.defense = 5;
		}
        public override void UpdateEquip(Player player)
		{
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.DynastyWood, 30).AddTile(TileID.WorkBenches).Register();
		}
	}
}