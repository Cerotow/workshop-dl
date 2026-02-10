using DDmod.Content.NPCs.Boss.流星破坏者;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Items.Boss.流星破坏者
{
    public class 流星召唤杖 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage =50;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.buffType = ModContent.BuffType<流星破坏球Buff>();
            Item.shoot = ModContent.ProjectileType<流星破坏球>();
            Item.shootSpeed = 2;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            Item.shoot = ModContent.ProjectileType<流星破坏球>();
            Projectile projectile = NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;
            return false;
        }
    }
}
