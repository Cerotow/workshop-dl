using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 刺草 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(22, 10, 28, 20, 1.5f, 3, 15, ModContent.ProjectileType<刺草Proj>());
            Value(0, 0, 40, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(331, 12).AddIngredient(209, 24).AddTile(TileID.Anvils).Register();
        }
    }
}