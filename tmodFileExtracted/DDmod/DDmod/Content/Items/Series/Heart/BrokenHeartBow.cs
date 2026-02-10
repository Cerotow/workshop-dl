using DDmod.Content.Projectiles.Ranged;
using Terraria.ID;

namespace DDmod.Content.Items.Series.Heart
{
    public class BrokenHeartBow : ModItem
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
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<BrokenHeartArrow>();
            Item.shootSpeed = 7f;
            Item.useAmmo = 40;
            Item.GetGlobalItem<RangedGlobalItem>().Bow = true;
            Item.GetGlobalItem<RangedGlobalItem>().Skill = 1;
            Item.GetGlobalItem<RangedGlobalItem>().PrePostConvertArrows = new int[] { 1 };
            Item.GetGlobalItem<RangedGlobalItem>().PostConvertArrows = ModContent.ProjectileType<BrokenHeartArrow>();
            Item.GetGlobalItem<RangedGlobalItem>().ConvertProbability =33;
            Item.GetGlobalItem<RangedGlobalItem>().SetString(5, 5, 3, 0, 4);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<HeartIngot>(), 12).AddTile(TileID.Anvils).Register();
        }
    }
}