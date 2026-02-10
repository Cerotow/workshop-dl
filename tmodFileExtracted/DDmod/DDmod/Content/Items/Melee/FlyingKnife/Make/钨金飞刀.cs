using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 钨金飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(10, 10, 28, 20, 1.3f, 3, 12, ModContent.ProjectileType<钨金飞刀Proj>());
            Value(0, 0, 12, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.TungstenBar, 12).AddTile(TileID.Anvils).Register();
        }
    }
}