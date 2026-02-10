using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 星空 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(22, 10, 28, 35, 1.5f, 3, 15, ModContent.ProjectileType<星空Proj>());
            Item.DItem().HandheldColor = new Color(255, 255, 255, 100);
            Value(0, 1, 0, 0);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Item.DItem().HandheldColor;
        }
    }
}