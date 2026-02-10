using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Gun;

namespace DDmod.Content.Items.Boss.海幽浮王
{
    public class 水球炮 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 64;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 4;
            SoundStyle sound = new SoundStyle(DDHelper.Sound(1,"溅水"));
            sound.MaxInstances = 10;
            Item.UseSound = sound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<水球>();
            Item.shootSpeed = 20f;
            Item.GetGlobalItem<RangedGlobalItem>().ShootOffset = new Vector2(-0, -14);
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.001f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = Item.shoot;
            velocity = velocity.RotatedBy(Item.GetGlobalItem<RangedGlobalItem>().Rota * (-player.direction));
            int A = NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[A].DamageType = DamageClass.Ranged;
            for (int a = 0; a < 20; a++)
            {
                int dust = NewDust(position + velocity.PerfectNormalize() * 30-new Vector2(4), 1, 1,33, 0, 0, 0, default, 1F);
                Main.dust[dust].velocity = velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(3, 8);
            }
            if (Item.GetGlobalItem<RangedGlobalItem>().Rota < 0.4F)
            {
                Item.GetGlobalItem<RangedGlobalItem>().Rota += 0.4F;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, -8);
        }
    }
}