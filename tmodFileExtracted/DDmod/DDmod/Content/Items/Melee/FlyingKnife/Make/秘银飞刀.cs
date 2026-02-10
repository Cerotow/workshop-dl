using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 秘银飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(36, 10, 28, 14, 3, 4, 20, ModContent.ProjectileType<秘银飞刀Proj>());
            Value(0, 0, 35, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.MythrilBar, 12).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}