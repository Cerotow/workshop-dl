using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.ID;

namespace DDmod.Content.Items.Ammo
{
    public class 绿岩弹 : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 8;
			Item.width = 18;
			Item.height = 44;
			Item.consumable = true;
			Item.knockBack = 3f;
			Item.value = Item.buyPrice(0, 0, 0, 12);
			Item.rare = 2;
			Item.shoot = ModContent.ProjectileType<绿岩弹Proj>();
			Item.shootSpeed = 4f;
			Item.ammo = AmmoID.Bullet;
        }
        public override void AddRecipes()
        {
            //CreateRecipe(50).AddIngredient(ItemID.MusketBall, 50).AddIngredient(ItemID.DemoniteBar).AddTile(TileID.Anvils).Register();
            CreateRecipe(100).AddIngredient(97, 100).AddIngredient(75).AddIngredient(ModContent.ItemType<绿岩锭>()).AddTile(TileID.Anvils).Register();
        }
    }
}