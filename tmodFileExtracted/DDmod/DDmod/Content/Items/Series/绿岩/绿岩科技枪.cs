using DDmod.Content.Dusts;
using DDmod.Content.Projectiles.GeneralProj;
using DDmod.Content.Projectiles.Magic;
using DDmod.Content.Projectiles.Ranged;
using DDmod.Content.Projectiles.Ranged.Gun;

namespace DDmod.Content.Items.Series.绿岩
{
    public class 绿岩科技枪 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 29;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.mana = 6;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.rare = 2;
            SoundStyle sound = SoundID.Item157;
            sound.Pitch = -0.5F;
            Item.UseSound = sound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<绿岩三角>();
            Item.shootSpeed = 6f;
            Item.GetGlobalItem<RangedGlobalItem>().ShootOffset = new Vector2(0, -12);
        }
        public override void SetStaticDefaults()
        {
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int q = NewProjectile(source, position-new Vector2(0,6).RotatedBy(player.itemRotation) + velocity.PerfectNormalize() * 46, velocity.PerfectNormalize() * 0.1F, type, damage, knockback, player.whoAmI, -1, 0, 0) ;
            int p = NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI,0,0,0);
            Main.projectile[q].rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            Main.projectile[p].rotation = Main.projectile[q].rotation;
            /*
            for (int a = 0; a < 10; a++)
            {
                int dust = NewDust(position + velocity.PerfectNormalize() * 30 - new Vector2(4), 1, 1, ModContent.DustType<速度粒子>(), 0, 0, 0, new Color(119, 237, 130, 50), 2);
                Main.dust[dust].velocity = velocity.PerfectNormalize().RotatedBy(Main.rand.NextFloat(-0.5F, 0.5F)) * Main.rand.NextFloat(7, 12);
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
                Main.dust[dust].noGravity = true;
            }*/
            if (Item.GetGlobalItem<RangedGlobalItem>().Rota < 0.8F)
            {
                Item.GetGlobalItem<RangedGlobalItem>().Rota += 0.3F;
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, -4);
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 12).AddTile(16).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
    }
}