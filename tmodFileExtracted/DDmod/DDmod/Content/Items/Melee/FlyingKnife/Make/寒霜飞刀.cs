using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 寒霜飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(13, 10, 28, 20, 1.5f, 3, 15, ModContent.ProjectileType<寒霜飞刀Proj>());
            Value(0, 1, 0, 0);
        }
    }
}