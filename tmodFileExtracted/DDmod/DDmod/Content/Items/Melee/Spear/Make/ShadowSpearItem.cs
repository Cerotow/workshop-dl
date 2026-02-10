using DDmod.Content.Projectiles.Melee.Spear;

namespace DDmod.Content.Items.Melee.Spear.Make
{
    public class ShadowSpearItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 100;
            Item.useAnimation = 100;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 0, 30, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 1;
            Item.shoot = ModContent.ProjectileType<ShadowSpear>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;

        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.Spears[Type] = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI);
            return false;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
        }
        public override void HoldItem(Player player)
        {
            if (!Main.dayTime)
            {
                player.GetAttackSpeed(Item.DamageType) += 1;
            }
        }
        public override float UseSpeedMultiplier(Player player)
        {
                return base.UseSpeedMultiplier(player);
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.DemoniteBar, 12).AddTile(TileID.Anvils).Register();
        }
    }
}