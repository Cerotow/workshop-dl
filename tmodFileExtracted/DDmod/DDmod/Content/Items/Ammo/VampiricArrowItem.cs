using DDmod.Content.Projectiles.Ranged.Ammo;

namespace DDmod.Content.Items.Ammo
{
    public class VampiricArrowItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 5;
			Item.width = 18;
			Item.height = 44;
			Item.consumable = true;
			Item.knockBack = 3f;
			Item.value = Item.buyPrice(0, 0, 0, 40);
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<VampiricArrow>();
			Item.shootSpeed = 7f;
			Item.ammo = AmmoID.Arrow;
		}
        public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(ItemID.WoodenArrow, 50).AddIngredient(ItemID.CrimtaneBar).AddTile(TileID.Anvils).Register();
		}
    }
}