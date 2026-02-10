using DDmod.Content.Projectiles.Magic.Gun;
using DDmod.Content.Projectiles.Ranged.Gun;

namespace DDmod.Content.Items.Boss.流星破坏者
{
    public class 流星炮 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 680;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 75;
            Item.useAnimation = 75;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<流星炮Proj>();
            Item.shootSpeed = 7f;
            Item.channel = true;
            Item.noUseGraphic = true;
        }
        public override void SetStaticDefaults()
        {

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = NewProjectile(source, position, velocity, ModContent.ProjectileType<流星炮Proj>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}