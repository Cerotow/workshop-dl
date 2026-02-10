using DDmod.Content.Items.Series.ShadowFlame;
using DDmod.Content.Projectiles.Melee.Sword;
using Terraria.ID;
using Terraria.UI;

namespace DDmod.Content.Items.Series.王朝木
{
    public class 王朝木锤 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Melee;
            Item.hammer = 38;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = true;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            CreateRecipe(1).AddIngredient(ItemID.DynastyWood,8).AddTile(TileID.WorkBenches).Register();
        }
    }
}