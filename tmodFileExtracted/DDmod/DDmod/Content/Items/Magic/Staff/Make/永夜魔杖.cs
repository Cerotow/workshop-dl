using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.NoContent.Config;

namespace DDmod.Content.Items.Magic.Staff.Make
{
    public class 永夜魔杖 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            RecipesSystem.Add(ModContent.ItemType<CorruptionStaffItem>(), 1, ModContent.ItemType<丛林权杖>(), 157, ModContent.ItemType<FlameStaffItem>(), Type);
            RecipesSystem.Add(ModContent.ItemType<BloodStaffItem>(), 1, ModContent.ItemType<丛林权杖>(), 157, ModContent.ItemType<FlameStaffItem>(), Type);
        }
        
        public override void SetDefaults()
        {
            Item.damage =14;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 1, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 4;
            Item.shoot = ModContent.ProjectileType<永夜魔杖Proj>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, 0, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => !ModContent.GetInstance<DDConfigServer>().HunterSlime);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<CorruptionStaffItem>()).AddIngredient(ModContent.ItemType<丛林权杖>()).AddIngredient(157).AddIngredient(ModContent.ItemType<FlameStaffItem>()).AddCondition(text).Register();
            CreateRecipe(1).AddIngredient(ModContent.ItemType<BloodStaffItem>()).AddIngredient(ModContent.ItemType<丛林权杖>()).AddIngredient(157).AddIngredient(ModContent.ItemType<FlameStaffItem>()).AddCondition(text).Register();
        }
    }
}