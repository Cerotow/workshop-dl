using DDmod.Content.Projectiles.Summon.Whip;
using Terraria;

namespace DDmod.Content.Items.Boss.狱火蛇物品
{
    public class 狱火鞭 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            DGlobalItem.DefaultToWhip(Item, ModContent.ProjectileType<狱火鞭Proj>(), 125, 1, 9.5f, 20);
            Item.value = Item.buyPrice(0, 31, 20, 0);
            Item.rare = 8;
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
    }
}