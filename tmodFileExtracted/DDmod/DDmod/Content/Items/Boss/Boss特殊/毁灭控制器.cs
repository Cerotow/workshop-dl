using DDmod.Content.Buffs.PlayerBuffs;
using DDmod.Content.Projectiles.Summon;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;
using Microsoft.Xna.Framework.Graphics;

namespace DDmod.Content.Items.Boss.Boss特殊
{
    public class 毁灭控制器 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
        }
        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 12, 20, 0);
            Item.rare = 6;
            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.buffType = ModContent.BuffType<机械蠕虫Buff>();
            Item.shoot = ModContent.ProjectileType<机械蠕虫>();
            Item.shootSpeed = 2;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 3;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);
            if (player.ownedProjectileCounts[Item.shoot] == 0)
            {
                Projectile projectile = NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, Main.myPlayer);
                projectile.originalDamage = Item.damage;
            }
            Projectile projectile2 = NewProjectileDirect(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<机械蠕虫计时器>(), damage, knockback, Main.myPlayer);
            projectile2.originalDamage = Item.damage;
            return false;
        }
    }
}
