using DDmod.Content.Projectiles.Magic.Gun;

namespace DDmod.Content.Items.Boss.流星破坏者
{
    public class 破坏者激光枪 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.mana = 8;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<破坏者激光枪Proj>();
            Item.shootSpeed = 7f;
            Item.channel = true;
            Item.noUseGraphic = true;
        }
        public override void SetStaticDefaults()
        {

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = NewProjectile(source, position, velocity, ModContent.ProjectileType<破坏者激光枪Proj>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}