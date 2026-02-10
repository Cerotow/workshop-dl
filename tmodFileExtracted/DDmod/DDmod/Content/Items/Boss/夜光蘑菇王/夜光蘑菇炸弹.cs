using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Melee.FlyingKnife;
using DDmod.Content.Projectiles.Ranged;
using Terraria.ID;

namespace DDmod.Content.Items.Boss.夜光蘑菇王
{
    public class 夜光蘑菇炸弹 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 2;
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<夜光蘑菇炸弹Proj>();
            Item.shootSpeed = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            DGlobalItem.FlyingKnife[Type] = true;
            Item.value = Item.buyPrice(0, 0, 0, 50);
        }
    }
}