using DDmod.Content.Projectiles.Ranged.Gun;

namespace DDmod.Content.Items.Boss.MeteorDiggerItems
{
    public class MeteorRifleItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 13;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 7f;
            Item.useAmmo = AmmoID.Bullet;
            Item.channel = true;
            Item.noUseGraphic = true;
        }
        public override void SetStaticDefaults()
        {

        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return player.itemTime>0;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LargeMechanicalScrap>(), 12).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.流星合成"), () => Main.LocalPlayer.Dplayer().MeteorRecipe)).Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            NewProjectile(source, position, velocity, ModContent.ProjectileType<MeteorRifle>(), damage, knockback, player.whoAmI, 0f, 0f,Item.type);
            return false;
        }
    }
}