using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 钯金飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(34, 10, 28, 18, 3, 4, 10, ModContent.ProjectileType<钯金飞刀Proj>());
            Value(0, 0, 35, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.PalladiumBar, 12).AddTile(TileID.Anvils).Register();
        }
    }
}