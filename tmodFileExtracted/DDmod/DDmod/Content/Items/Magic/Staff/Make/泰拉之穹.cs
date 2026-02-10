using DDmod.Content.Items.Accessory;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Projectiles.Magic.Staff;
using DDmod.NoContent.Config;
using Terraria;
using static AssGen.Assets;

namespace DDmod.Content.Items.Magic.Staff.Make
{
    public class 泰拉之穹 : ModItem
    {
        public override void SetStaticDefaults()
        {

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            RecipesSystem.Add(ModContent.ItemType<英雄断杖>(), 4, new Item(ModContent.ItemType<真神圣法杖>(), 1), new Item(ModContent.ItemType<真永夜魔杖>(), 1), new Item(ModContent.ItemType<SoulOfNature>(), 50), new Item(Type, 1));

        }
        public override void SetDefaults()
        {
            Item.damage = 36;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 40;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 200;
            Item.useAnimation = 200;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.staff[Item.type] = true;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 10, 20, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shootSpeed = 4;
            Item.shoot = ModContent.ProjectileType<泰拉之穹Proj>();
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.泰拉级;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true; 
            Item.DItem().DrawVec -= new Vector2(12);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            NewProjectile(source, position, velocity, type, damage, 0, player.whoAmI, 0f, 0f);
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, frame, drawColor, 0, origin, scale * 1.5F, 0, 0);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw(texture, Item.position + new Vector2(Item.width / 2, Item.height) - Main.screenPosition, null, alphaColor, rotation, new Vector2(texture.Width / 2, texture.Height - 10), scale, 0, 0);

            return false;
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => !ModContent.GetInstance<DDConfigServer>().HunterSlime);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<英雄断杖>()).AddIngredient(ModContent.ItemType<真神圣法杖>()).AddIngredient(ModContent.ItemType<真永夜魔杖>()).AddIngredient(ModContent.ItemType<SoulOfNature>(), 50).AddCondition(text).Register();

        }
    }
}