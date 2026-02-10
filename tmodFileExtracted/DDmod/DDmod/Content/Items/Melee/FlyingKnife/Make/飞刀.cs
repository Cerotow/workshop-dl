using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public abstract class 飞刀 : ModItem
    {
        public virtual void Value(int 铂金, int 金, int 银, int 铜)
        {
            Item.value = Item.buyPrice(铂金, 金, 银, 铜);
        }
        public void 属性(int Damage,int width,int height,int Speed,float knockBack,int rare,float shootSpeed, int Shoot)
        {
            Item.damage = Damage;
            Item.width = width;
            Item.height = height;
            Item.useTime = Speed;
            Item.useAnimation = Speed;
            Item.knockBack = knockBack;
            Item.rare = rare;
            Item.shoot = Shoot;
            Item.shootSpeed = shootSpeed;
        }
        public virtual void Defaults()
        {

        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            //Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useStyle = MeleeGlobalItem.FlyingKnife;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            DGlobalItem.FlyingKnife[Type] = true;
            Defaults();
        }
    }
}