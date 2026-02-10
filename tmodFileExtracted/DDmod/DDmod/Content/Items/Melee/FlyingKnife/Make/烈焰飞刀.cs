using DDmod.Content.Projectiles.Melee.FlyingKnife;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
	public class 烈焰飞刀 : 飞刀
	{
		public override void SetStaticDefaults()
		{
		}

		public override void Defaults()
		{
			属性(31, 10, 28, 30, 1, 3, 12, ModContent.ProjectileType<烈焰飞刀Proj>());
			Value(0, 3, 0, 0);
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HellstoneBar, 12).AddTile(TileID.Anvils).Register();
		}
	}
}
