using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Projectiles.Ranged.Gun;
using Terraria.ID;

namespace DDmod.Content.Items.Boss.天地守卫
{
    public class 晶凝蓄能炮 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 180;
            Item.useAnimation = 180;
            Item.useStyle = 13;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 7f;
            Item.useAmmo = 929;
            Item.channel = true;
            Item.noUseGraphic = true;
        }
        public override void SetStaticDefaults()
        {

        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            NewProjectile(source, position, velocity, ModContent.ProjectileType<晶凝蓄能炮Proj>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            //CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 24).AddIngredient(ModContent.ItemType<绿岩电池>(), 5).AddTile(TileID.Anvils).Register();
        }
    }
}