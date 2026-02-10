
using DDmod.Content.Items.Melee.FlyingKnife.Make;
using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Boss.蘑菇王
{
    public class 蘑菇飞刀 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(12, 10, 28, 20, 1, 4, 16, ModContent.ProjectileType<蘑菇飞刀Proj>());
            Value(0, 0, 10, 0);
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 2;
        }
    }
}