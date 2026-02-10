using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria.ID;
using Terraria.UI;

namespace DDmod.Content.Items.Series.ShadowFlame
{
    public class ShadowFlameSwordItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 43;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 5, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 1;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<ShadowFlameSword>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            return true;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.FieryGreatsword).AddIngredient(ModContent.ItemType<ShadowFlame>()).AddIngredient(ItemID.Bone,24).AddTile(TileID.Anvils).Register();
        }
    }
}