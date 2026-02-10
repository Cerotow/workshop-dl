using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.ID;

namespace DDmod.Content.Items.Ammo
{
    public class 受潮弹 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 4;
			Item.width = 18;
			Item.height = 44;
			Item.consumable = true;
			Item.knockBack = 3f;
			Item.value = Item.buyPrice(0, 0, 0, 3);
			Item.rare = -1;
			Item.shoot = ModContent.ProjectileType<受潮弹Proj>();
			Item.shootSpeed = 0f;
			Item.ammo = AmmoID.Bullet;
		}
        public override void AddRecipes()
		{
		}
    }
}