using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Projectiles.Ranged.Ammo;

namespace DDmod.Content.Items.Ammo
{
    public class 珍珠木箭 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 12;
			Item.width = 18;
			Item.height = 44;
			Item.consumable = true;
			Item.knockBack = 3f;
			Item.value = Item.buyPrice(0, 0, 0, 12);
			Item.rare = ItemRarityID.Orange;
			Item.shoot = 1;
			Item.shootSpeed = 7f;
			Item.ammo = AmmoID.Arrow;
		}
        public override void AddRecipes()
		{
            CreateRecipe(100).AddIngredient(ItemID.WoodenArrow, 100).AddIngredient(75).AddIngredient(ModContent.ItemType<绿岩锭>()).AddTile(TileID.Anvils).Register();
        }
    }
}