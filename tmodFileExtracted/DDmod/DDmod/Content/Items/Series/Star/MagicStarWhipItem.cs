using DDmod.Content.Projectiles.Summon.Whip;
using Terraria;

namespace DDmod.Content.Items.Series.Star
{
    public class MagicStarWhipItem : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            DGlobalItem.DefaultToWhip(Item, ModContent.ProjectileType<MagicStarWhip>(), 17, 1, 3.5f, 40);
            Item.value = Item.buyPrice(0, 0, 80, 0);
            Item.rare = 3;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<StarIngot>(), 16).AddTile(TileID.Anvils).Register();
        }
    }
}