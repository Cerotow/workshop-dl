using DDmod.Content.Items.Series.苦难;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Boss.恐惧缝合体
{
	[AutoloadEquip(EquipType.Body)]
	public class 血滴子盔甲 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
            Item.value = Item.buyPrice(0, 0, 50);
            Item.rare = 4;
            Item.defense = 11;
        }
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 25;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<恐惧肉块>(), 12).AddIngredient(ModContent.ItemType<苦难之魂>(), 1).AddTile(TileID.Anvils).Register();
        }
    }
}