using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 珍珠木飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(28, 10, 28, 8, 1, 3, 21, ModContent.ProjectileType<珍珠木飞刀Proj>());
            Value(0, 0, 0, 50);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Pearlwood, 12).AddTile(TileID.WorkBenches).Register();
        }
    }
}