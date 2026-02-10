using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Items.Boss.StarGuardItems
{
    public class MagicStarSwordItem : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.damage = 31;
            Item.DamageType = DamageClass.Melee;
            Item.mana = 10;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 34;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.shoot = ModContent.ProjectileType<MagicStarSword>();
            Item.shootSpeed = 20f;
            Item.channel = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix()
        {
            return true; 
        }
    }
}