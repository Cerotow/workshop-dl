using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 锡制飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(6, 10, 28, 25, 1, 3, 10, ModContent.ProjectileType<锡制飞刀Proj>());
            Value(0, 0, 1, 50);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.TinBar, 12).AddTile(TileID.Anvils).Register();
        }
    }
}