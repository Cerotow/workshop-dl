using DDmod.Content.Projectiles.Magic.Staff;

namespace DDmod.Content.Items.Boss.StarGuardItems
{
    public class StarwandItem : ModItem
    {
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 5;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Starwand>();
            Item.shootSpeed = 7f;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.MagicItem().Handheld = true;
        }
    }
}