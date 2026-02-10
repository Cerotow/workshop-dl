using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Gun;

namespace DDmod.Content.Items.Boss.狱火蛇物品
{
    public class 狱火炮 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 8;
            SoundStyle sound = SoundID.Item14;
            sound.Pitch = -0.8F;
            Item.UseSound = sound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<远程狱火球>();
            Item.shootSpeed = 20f;
            Item.useAmmo = 929;
        }
        public override void SetStaticDefaults()
        {

        }
        public bool A;
        public int useTime;
        public int useAnimation;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = Item.shoot;
               velocity = velocity.RotatedBy(Item.GetGlobalItem<RangedGlobalItem>().Rota * (-player.direction));
            position += velocity.PerfectNormalize() * 50 - new Vector2(0, 10).RotatedBy(player.itemRotation);
            NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (A)
            {
                Item.useTime = useTime;
                Item.useAnimation = useAnimation;
            }
            if (Main.rand.NextBool(4))
            {
                useTime = Item.useTime;
                useAnimation = Item.useAnimation;
                Item.useTime = 10;
                Item.useAnimation = 10;
                A = true;
            }
            player.velocity += -velocity.RotatedBy(Item.GetGlobalItem<RangedGlobalItem>().Rota * -player.direction) / 10;
            for (int a = 0; a < 40; a++)
            {
                int dust = NewDust(position - new Vector2(4), 1, 1, 6, 0, 0, 0, default, 2.4F);
                Main.dust[dust].velocity = velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0, 8);
                Main.dust[dust].noGravity = true;
            }
            for (int a = 0; a < 10; a++)
            {
                int dust = NewDust(position-new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(253, 162, 3,140), 1);
                Main.dust[dust].velocity = velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(5, 11);
                Main.dust[dust].noGravity = true;
            }
            if (Item.GetGlobalItem<RangedGlobalItem>().Rota < 1.6F)
            {
                Item.GetGlobalItem<RangedGlobalItem>().Rota += 0.8F;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -6);
        }
    }
}