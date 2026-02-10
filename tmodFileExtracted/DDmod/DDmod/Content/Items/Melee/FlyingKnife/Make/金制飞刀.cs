using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 金制飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(10, 10, 28, 15, 1.5f, 3, 15, ModContent.ProjectileType<金制飞刀Proj>());
            Value(0, 0, 50, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.GoldBar, 12).AddTile(TileID.Anvils).Register();
        }
    }
}