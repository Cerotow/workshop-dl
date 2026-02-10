using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 魔金飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(8, 10, 28, 12, 1, 5, 16, ModContent.ProjectileType<魔金飞刀Proj>());
            Value(0, 0, 80, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.DemoniteBar, 12).AddTile(TileID.WorkBenches).Register();
        }
    }
}