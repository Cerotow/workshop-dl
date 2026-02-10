
using DDmod.Content.Items.Melee.FlyingKnife.Make;
using DDmod.Content.Projectiles.Ranged;
using Terraria.ID;

namespace DDmod.Content.Items.Ranged.NPCLoot
{
    public class 飞鱼镖 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void Defaults()
        {
            属性(20, 10, 28, 32, 1, 3, 4, ModContent.ProjectileType<飞鱼镖Proj>());
            Value(0, 1, 0, 0);

            Item.DamageType = DamageClass.Ranged;
        }
    }
}