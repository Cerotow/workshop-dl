using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 圣刃 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(40, 10, 28, 12, 1.5f, 5, 15, ModContent.ProjectileType<圣刃Proj>());
            Value(0, 2, 0, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.HallowedBar,12).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}