using DDmod.Content.Projectiles.Melee;

namespace DDmod.Content.Items.Melee.Sword
{
    public class 超级胡萝卜 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.scale = 1F;
            Item.shoot = ModContent.ProjectileType<胡萝卜>();
            Item.shootSpeed = 13;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(213, 128, 36)*0.3F;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            velocity = velocity.RotatedBy(Main.rand.NextFloat(-0.3F, 0.3F));
            NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI);
            return false;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}