using DDmod.Content.Projectiles.Boss;
using DDmod.Content.Projectiles.Boss.MiniBoss;

namespace DDmod.Content.Items.Melee.Sword
{
    public class A : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 32;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(0, 1, 40, 0);
            Item.rare = ItemRarityID.Orange;
        }
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Goblin Sword");
            //DisplayName.AddTranslation(7, "A");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(player.GetSource_FromAI(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<暗影触手>(), 30,0,-1,0, 30,1.5F);
            NewProjectile(player.GetSource_FromAI(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<暗影触手>(), 30,0,-1,0, 30,1.5F);
            NewProjectile(player.GetSource_FromAI(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<暗影触手>(), 30,0,-1,0, 30,1.5F);
            NewProjectile(player.GetSource_FromAI(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<暗影触手>(), 30,0,-1,0, 30,1.5F);
            return true;
        }
        public override bool MeleePrefix()
        {
            return false;
        }
    }
}