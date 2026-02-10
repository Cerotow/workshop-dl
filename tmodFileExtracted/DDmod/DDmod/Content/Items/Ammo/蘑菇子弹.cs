using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.ID;

namespace DDmod.Content.Items.Ammo
{
    public class 蘑菇子弹 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 18;
			Item.width = 18;
			Item.height = 44;
			Item.consumable = true;
			Item.knockBack = 3f;
			Item.value = Item.buyPrice(0, 0, 2, 0);
			Item.rare = 7;
			Item.shoot = ModContent.ProjectileType<蘑菇子弹Proj>();
            Item.shootSpeed = 4f;
            Item.ammo = AmmoID.Bullet;
		}
        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(1432, 50).AddIngredient(1552).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}