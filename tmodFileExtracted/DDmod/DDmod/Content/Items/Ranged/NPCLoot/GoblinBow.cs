namespace DDmod.Content.Items.Ranged.NPCLoot
{
    public class GoblinBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            if (Main.netMode != 2)
                DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
        }
        public override void Load()
        {
            DDTextures.Bow[Type] = ModContent.Request<Texture2D>(Texture + "_NoStrings");
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.crit = 5;
            Item.value = Item.buyPrice(0, 0, 30, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useAmmo = AmmoID.Arrow;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = 10;
            Item.shootSpeed =12f;
            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            Item.GetGlobalItem<RangedGlobalItem>().SetString(7, 7, 3, 0, 6, false, new Color(172, 172, 172));
        }
        public override bool CanShoot(Player player)
        {
            return base.CanShoot(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile projectile =  Main.projectile[NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI)];
            projectile.extraUpdates = 0;
            return false;
        }
    }
}