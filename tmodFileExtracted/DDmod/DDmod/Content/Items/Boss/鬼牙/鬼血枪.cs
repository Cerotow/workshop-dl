using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Gun;

namespace DDmod.Content.Items.Boss.鬼牙
{
    public class 鬼血枪 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 220;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 33, 0, 0);
            Item.rare = 8;
            SoundStyle sound = SoundID.NPCDeath13;
            sound.Pitch = 1;
            sound.Volume = 0.3f;
            Item.UseSound = sound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<血弹>();
            Item.shootSpeed = 20f;
            Item.GetGlobalItem<RangedGlobalItem>().ShootOffset = new Vector2(-4, -6);
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.001f;
        }
        public bool A;
        public int LS;
        public int useTime;
        public int useAnimation;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = Item.shoot;
            velocity = velocity.RotatedBy(Item.GetGlobalItem<RangedGlobalItem>().Rota * (-player.direction));
            //position += new Vector2(-4, -6);
            NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (A)
            {
                if (LS<=0)
                {
                    Item.useTime = useTime;
                    Item.useAnimation = useAnimation;
                    A = false;
                }
                else
                {
                    LS--;
                }
            }
            if (!A&& Main.rand.NextBool(8))
            {
                useTime = Item.useTime;
                useAnimation = Item.useAnimation;
                Item.useTime = 2;
                Item.useAnimation = 2;
                A = true;
                LS = 5;
            }
            for (int a = 0; a < 20; a++)
            {
                int dust = NewDust(position + velocity.PerfectNormalize() * 30-new Vector2(4), 1, 1,5, 0, 0, 0, default, 1F);
                Main.dust[dust].velocity = velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(3, 8);
            }
            for (int a = 0; a < 4; a++)
            {
                int dust = NewDust(position + velocity.PerfectNormalize() * 30 - new Vector2(4), 1, 1, ModContent.DustType<Dusts.拉长粒子>(), 0, 0, 0, new Color(153, 0, 0), 1);
                Main.dust[dust].velocity = velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(5, 11);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLightEmittence= true;
            }
            if (Item.GetGlobalItem<RangedGlobalItem>().Rota < 0.8F)
            {
                Item.GetGlobalItem<RangedGlobalItem>().Rota += 0.2F;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -6);
        }
    }
}