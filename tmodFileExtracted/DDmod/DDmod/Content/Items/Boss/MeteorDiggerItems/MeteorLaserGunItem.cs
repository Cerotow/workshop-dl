using DDmod.Content.Projectiles.Magic.Gun;

namespace DDmod.Content.Items.Boss.MeteorDiggerItems
{
    public class MeteorLaserGunItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.mana = 10;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MeteorLaserGun>();
            Item.shootSpeed = 7f;
            Item.channel = true;
            Item.noUseGraphic = true;
        }
        public override void SetStaticDefaults()
        {

        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LargeMechanicalScrap>(), 12).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.流星合成"), () => Main.LocalPlayer.Dplayer().MeteorRecipe)).Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = NewProjectile(source, position, velocity, ModContent.ProjectileType<MeteorLaserGun>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}