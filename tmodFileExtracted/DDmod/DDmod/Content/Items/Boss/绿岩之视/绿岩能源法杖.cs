using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.Content.Projectiles.Ranged.Gun;
using Terraria.ID;

namespace DDmod.Content.Items.Boss.绿岩之视
{
    public class 绿岩能源法杖 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 33;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.mana = 30;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = 13;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<绿岩能源法杖Proj>();
            Item.staff[Item.type] = true;
            Item.shootSpeed = 14f;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = 2;
        }
        public override void SetStaticDefaults()
        {

        }
        public override void AddRecipes()
        {
        }
    }
}