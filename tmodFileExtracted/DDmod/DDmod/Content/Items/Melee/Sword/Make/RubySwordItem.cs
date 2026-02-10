using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Items.Melee.Sword.Make
{
    public class RubySwordItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 4, 80, 0);
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
            Item.shootSpeed = 1;
            Item.shoot = ModContent.ProjectileType<RubySword>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.scale = 1.1f;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public byte Time = 0;
        public byte R = 0;
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (Time++ > 60)
            {
                Time = 0;
                R = 0;
            }
        }
        public override void UpdateInventory(Player player)
        {
            if (Time++ > 60)
            {
                Time = 0;
                R = 0;
            }
        }
        public override void HoldItem(Player player)
        {
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI, 0f, 0f, R);
            R = 0;
            return false;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Ruby, 12).AddTile(TileID.Anvils).Register();
        }
    }
}