using DDmod.Content.Projectiles.Magic.Gun;
using DDmod.Content.Projectiles.Magic.Staff;

namespace DDmod.Content.Items.Boss.Boss特殊
{
    public class 星辰炮 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 300;
            Item.useAnimation = 300;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 5, 20, 0);
            Item.rare = 4;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 4;
            Item.MagicItem().charging = 3;
            Item.MagicItem().ExtraMana = 5;
            Item.shoot = ModContent.ProjectileType<星辰炮Proj>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 3;
            Item.DItem().DrawRanged = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}