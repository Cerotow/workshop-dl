using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 钴蓝小刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(24, 10, 28, 12, 1, 4, 14, ModContent.ProjectileType<钴蓝小刀Proj>());
            Value(0, 0, 30, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.CobaltBar, 12).AddTile(TileID.Anvils).Register();
        }
    }
}