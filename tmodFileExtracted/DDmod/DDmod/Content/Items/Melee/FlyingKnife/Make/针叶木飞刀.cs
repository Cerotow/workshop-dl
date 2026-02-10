using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 针叶木飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(4, 10, 28, 12, 1, 3, 16, ModContent.ProjectileType<针叶木飞刀Proj>());
            Value(0, 0, 0, 20);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.BorealWood, 12).AddTile(TileID.WorkBenches).Register();
        }
    }
}