using DDmod.Content.Projectiles.Melee.FlyingKnife;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 血猩獠牙 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(33, 16, 40, 18, 3, 4, 20, ModContent.ProjectileType<血猩獠牙Proj>());
            Value(0, 0, 80, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.CrimtaneBar, 12).AddIngredient(ItemID.TissueSample, 20).AddTile(TileID.Anvils).Register();
        }
    }
}