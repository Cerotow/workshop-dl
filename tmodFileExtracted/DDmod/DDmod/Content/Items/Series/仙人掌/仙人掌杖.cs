using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.Content.Projectiles.Summon.Minions;
using DDmod.Content.Projectiles.Summon.Minions.MinionBuff;

namespace DDmod.Content.Items.Series.仙人掌
{
    public class 仙人掌杖 : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 2;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noUseGraphic = true;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.channel = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<仙人掌杖Proj>();
            Item.shootSpeed = 5f;
        }
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<仙人掌之魂>(), 4).AddIngredient(276, 12).AddTile(16).Register();
        }
    }
}