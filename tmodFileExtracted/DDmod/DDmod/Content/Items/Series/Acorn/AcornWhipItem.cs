using DDmod.Content.Projectiles.Summon.Whip;
using Terraria;

namespace DDmod.Content.Items.Series.Acorn
{
    public class AcornWhipItem : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            DGlobalItem.DefaultToWhip(Item, ModContent.ProjectileType<AcornWhip>(), 8, 1, 2, 35);
            Item.rare = 2;
            Item.value = Item.buyPrice(0, 0, 20, 0);
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
            CreateRecipe(1).AddIngredient(ModContent.ItemType<SoulOfNature>(), 8).AddIngredient(ItemID.Acorn, 15).AddTile(TileID.WorkBenches).Register();
        }
    }
}
