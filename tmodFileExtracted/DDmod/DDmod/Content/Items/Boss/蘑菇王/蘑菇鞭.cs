using DDmod.Content.Projectiles.Summon.Whip;
using Terraria;

namespace DDmod.Content.Items.Boss.蘑菇王
{
    public class 蘑菇鞭 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            DGlobalItem.DefaultToWhip(Item, ModContent.ProjectileType<蘑菇鞭Proj>(), 14, 1, 6.5f, 20);
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 0, 21, 0);
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