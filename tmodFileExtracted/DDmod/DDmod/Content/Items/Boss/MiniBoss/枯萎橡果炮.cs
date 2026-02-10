using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.Ranged.Gun;

namespace DDmod.Content.Items.Boss.MiniBoss
{
    public class 枯萎橡果炮 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 46;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Orange;
            SoundStyle sound = SoundID.Item14;
            sound.Pitch = -0.5F;
            Item.UseSound = sound;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 7f;
            Item.useAmmo = 27;
        }
        public override void SetStaticDefaults()
        {

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Item.GetGlobalItem<RangedGlobalItem>().Rota = 1;
            position += velocity.PerfectNormalize() * 50;
            NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            for (int a = 0; a < 50; a++)
            {
                int dust = NewDust(player.Center-new Vector2(0,4).RotatedBy(player.itemRotation)  + velocity.PerfectNormalize() * 50, 1, 1, ModContent.DustType<枯萎粒子>(), 0, 0, 0, default, 1.4F);
                Main.dust[dust].velocity = velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(0, 8);
                Main.dust[dust].noGravity = true;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4,-2);
        }
    }
}