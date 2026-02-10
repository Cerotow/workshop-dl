using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Projectiles.Melee.FlyingKnife;
using Terraria.ID;

namespace DDmod.Content.Items.Series.Venture.奖励袋.特别奖励
{
    public class 樱花剑 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 0, 60, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<花斩>();
            Item.shootSpeed = 4;
        }
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            Vector2 vector = velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
            int A = NewProjectile(source, position + velocity.PerfectNormalize() * 75 - vector * 8, vector, type, damage / 2, 0.2F, -1,0,0,0.75F);
            vector = velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
            A = NewProjectile(source, position + velocity.PerfectNormalize() * 150 - vector * 8, vector, type, damage / 4, 0.2F, -1, 0, 0, 0.75F);
            return false;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
        }
    }
}