using DDmod.Content.Projectiles.Melee.Sword;

namespace DDmod.Content.Items.Boss.MeteorAnnihilatorItems
{
    public class 哈迪斯之刃物品 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 16;
            Item.useStyle = 13;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 1, 40, 0);
            Item.rare = ItemRarityID.Orange;
            Item.GetGlobalItem<MeleeGlobalItem>().Color = new Color(0,50,205,150);
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<哈迪斯之刃>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.GetGlobalItem<MeleeGlobalItem>().SpecialAttack = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 2;
        }
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Haste of Hades");
            //DisplayName.AddTranslation(7, "哈迪斯之刃");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 9, false));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return false;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}