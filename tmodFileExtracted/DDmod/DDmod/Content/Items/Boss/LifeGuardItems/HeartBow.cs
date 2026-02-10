using DDmod.Content.Projectiles.Ranged;

namespace DDmod.Content.Items.Boss.LifeGuardItems
{
    public class HeartBow : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            if (Main.netMode != 2)
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.crit = 5;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<HeartArrow>();
            Item.shootSpeed = 10f;
            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            Item.GetGlobalItem<RangedGlobalItem>().Skill = 1;
            Item.GetGlobalItem<RangedGlobalItem>().StringDown += 2;
            Item.GetGlobalItem<RangedGlobalItem>().StringUP += 2;
            Item.GetGlobalItem<RangedGlobalItem>().Offset += 6;
            Item.GetGlobalItem<RangedGlobalItem>().PrePostConvertArrows = new int[] { 1 };
            Item.GetGlobalItem<RangedGlobalItem>().PostConvertArrows = ModContent.ProjectileType<HeartArrow>();
        }
        public override bool CanShoot(Player player)
        {
            return base.CanShoot(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == 1)
            {
                type = ModContent.ProjectileType<HeartArrow>();
            }
            NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            return false;
        }
    }
}