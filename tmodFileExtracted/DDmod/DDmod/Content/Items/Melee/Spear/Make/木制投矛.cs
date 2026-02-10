using DDmod.Content.Projectiles.Melee.Spear;
using DDmod.Content.Projectiles.Melee.Spear.Throwing;

namespace DDmod.Content.Items.Melee.Spear.Make
{
    public class 木制投矛 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 1;
            Item.shoot = ModContent.ProjectileType<木制投矛Proj>();
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
            return true;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
        }
        public override void HoldItem(Player player)
        {
        }
        public override float UseSpeedMultiplier(Player player)
        {
                return base.UseSpeedMultiplier(player);
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}