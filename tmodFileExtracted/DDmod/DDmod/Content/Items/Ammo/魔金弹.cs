using DDmod.Content.Projectiles.Ranged.Ammo;
using Terraria.ID;

namespace DDmod.Content.Items.Ammo
{
    public class 魔金弹 : ModItem
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
			Item.value = Item.buyPrice(0, 0, 0, 20);
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<魔金弹Proj>();
			Item.shootSpeed = 7f;
			Item.ammo = AmmoID.Bullet;
		}
        public override void AddRecipes()
		{
            CreateRecipe(50).AddIngredient(ItemID.MusketBall, 50).AddIngredient(ItemID.DemoniteBar).AddTile(TileID.Anvils).Register();
           // CreateRecipe(50).AddIngredient(ItemID.MusketBall, 50).AddIngredient(ItemID.CrimtaneBar).AddTile(TileID.Anvils).Register();
		}
    }
}