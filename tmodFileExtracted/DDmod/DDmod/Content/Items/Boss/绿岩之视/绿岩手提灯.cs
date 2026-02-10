using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Projectiles.Magic.Gun;

namespace DDmod.Content.Items.Boss.绿岩之视
{
    public class 绿岩手提灯 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 28;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.mana = 3;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<绿岩手提灯Proj>();
            Item.shootSpeed = 7f;
            Item.channel = true;
            Item.noUseGraphic = true;
        }
        public override void SetStaticDefaults()
        {

        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 24).AddIngredient(ModContent.ItemType<绿岩电池>(), 5).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = NewProjectile(source, position, velocity, ModContent.ProjectileType<绿岩手提灯Proj>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}