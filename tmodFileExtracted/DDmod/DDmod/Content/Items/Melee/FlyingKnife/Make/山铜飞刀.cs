using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 山铜飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(20, 10, 28, 22, 3, 4, 20, ModContent.ProjectileType<山铜飞刀Proj>());
            Value(0, 0, 40, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.OrichalcumBar, 12).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}