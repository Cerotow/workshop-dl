using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 钛金飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(30, 10, 28,25, 2f, 4,12, ModContent.ProjectileType<钛金飞刀Proj>());
            Value(0, 0, 80, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.TitaniumBar,12).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}