using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 铜制飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(6, 10, 28, 25, 1, 3, 10, ModContent.ProjectileType<铜制飞刀Proj>());
            Value(0, 0, 1, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.CopperBar, 12).AddTile(TileID.Anvils).Register();
        }
    }
}