using DDmod.Content.Items.Magic.Staff.Make;
using DDmod.Content.Projectiles.Magic.Staff;

namespace DDmod.Content.Items.Series.Venture.Level_1
{
    public class 生命木杖物品 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 7;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 18;
            Item.useAnimation =18;
            Item.scale = 1f;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 1;
            Item.shoot = ModContent.ProjectileType<ForestWoodenMagicwand>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            AdventureGearGlobalItem.WeaponsQualityAttribute(Item, new Random().Next(2, 6), new Random().Next(1, 5));
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}