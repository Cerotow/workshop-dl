using DDmod.Content.Projectiles.Magic.Staff;

namespace DDmod.Content.Items.Magic.Staff.Make
{
    public class CorruptionStaffItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shootSpeed = 4;
            Item.shoot = ModContent.ProjectileType<CorruptionStaff>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.DemoniteBar, 16).AddIngredient(ItemID.ShadowScale, 20).AddTile(TileID.Anvils).Register();
        }
    }
}