using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Gun;

namespace DDmod.Content.Items.Boss.星心守卫
{
    public class 星心炮 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 8;
            SoundStyle sound = SoundID.Item14;
            sound.Pitch = -0.8F;
            Item.UseSound = sound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<心心>();
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
            velocity = velocity.RotatedBy(Item.GetGlobalItem<RangedGlobalItem>().Rota * (-player.direction));
            position += velocity.PerfectNormalize() * 50 - new Vector2(0, 10).RotatedBy(player.itemRotation);
            for (int a = -2; a <= 2; a++)
            {
                int Proj = 0;
                if (a!=0)
                {
                    type = ModContent.ProjectileType<星星>();
                    Proj = NewProjectileChange(source, position, velocity, type, damage, knockback, player.whoAmI, 0, a, 1);
                }
                else
                {
                    type = Item.shoot;
                    damage *= 3;
                    Proj = NewProjectileChange(source, position, velocity, type, damage, knockback, player.whoAmI, 0, a, 3);
                }
                Main.projectile[Proj].DamageType = DamageClass.Ranged;
            }
            for (int a = 0; a < 30; a++)
            {
                int dust = NewDust(position - new Vector2(4), 1, 1, ModContent.DustType<Dusts.速度粒子>(), 0, 0, 0, Main.rand.Next(new Color[] { new Color(255,100,100,0), new Color(0, 100, 255,0) }), Main.rand.NextFloat(2,4));
                Main.dust[dust].velocity = velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(5, 11);
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
            }
            if (Item.GetGlobalItem<RangedGlobalItem>().Rota < 0.9F)
            {
                Item.GetGlobalItem<RangedGlobalItem>().Rota += 0.9F;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, -6);
        }
    }
}