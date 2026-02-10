using DDmod.Content.Projectiles.Melee;

namespace DDmod.Content.Items.Melee.Sword
{
    public class C : ModItem
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
            Item.value = Item.buyPrice(0, 1, 40, 0);
            Item.rare = ItemRarityID.Orange;
            Item.scale = 1F;
            //Item.shoot = ModContent.ProjectileType<BProj>();
            //Item.shootSpeed = 8;
        }
        public override void SetStaticDefaults()
        {
           //DisplayName.SetDefault("Goblin Sword");
           //DisplayName.AddTranslation(7, "C");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            for(int A =-1;A<=1;A++)
            {
                //NewProjectile(source, position, velocity.RotatedBy(A*0.5f+Main.rand.NextFloat(-0.2f,0.2f)), type, damage, knockBack,player.whoAmI);
            }
            return true;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}