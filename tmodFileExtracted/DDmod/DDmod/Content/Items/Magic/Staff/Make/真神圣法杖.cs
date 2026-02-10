using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Series.杂物;
using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.NoContent.Config;

namespace DDmod.Content.Items.Magic.Staff.Make
{
    public class 真神圣法杖 : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            RecipesSystem.Add(ModContent.ItemType<神圣法杖>(), 3, new Item(520, 20), new Item(521, 20), new Item(ModContent.ItemType<叶绿龟壳>()), new Item(Type));
        }
        public override void SetDefaults()
        {
            Item.damage =16;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 14, 20, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 1;
            Item.shoot = ModContent.ProjectileType<真神圣法杖Proj>();
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => !ModContent.GetInstance<DDConfigServer>().HunterSlime);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<神圣法杖>()).AddIngredient(520, 20).AddIngredient(521, 20).AddIngredient(ModContent.ItemType<叶绿龟壳>()).AddCondition(text).Register();
        }
    }
}