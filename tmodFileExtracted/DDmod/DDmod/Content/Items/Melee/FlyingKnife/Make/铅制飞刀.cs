using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 铅制飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(7, 10, 28, 23, 1, 3, 10, ModContent.ProjectileType<铅制飞刀Proj>());
            Value(0, 0, 6, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.LeadBar, 12).AddTile(TileID.Anvils).Register();
        }
    }
}