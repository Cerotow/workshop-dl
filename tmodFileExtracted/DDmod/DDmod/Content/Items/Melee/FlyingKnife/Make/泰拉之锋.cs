using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Projectiles.Melee.FlyingKnife;
using DDmod.NoContent.Config;
using Terraria;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 泰拉之锋 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            RecipesSystem.Add(ModContent.ItemType<破损英雄飞刀>(),4, new Item(ModContent.ItemType<真圣刃>(), 1), new Item(ModContent.ItemType<真夜刃>(), 1), new Item(ModContent.ItemType<SoulOfNature>(), 50), new Item(Type, 1));
        }
        public override void Defaults()
        {
            属性(38, 10, 28, 18, 1.5f, 8, 15, ModContent.ProjectileType<泰拉之锋Proj>());
            Value(0, 10, 0, 0);
            Item.channel = true;
            Item.GetGlobalItem<MeleeGlobalItem>().SpecialAttack = true;
            Item.GetGlobalItem<AdventureGearGlobalItem>().稀有度 = AdventureGearGlobalItem.泰拉级;
            Item.DItem().DrawVec = new Vector2(-8);
            Item.DItem().DrawDistance = 8;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, frame, drawColor, 0, origin, scale * 1.3F, 0, 0);
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
            CreateRecipe(1).AddIngredient(ModContent.ItemType<破损英雄飞刀>()).AddIngredient(ModContent.ItemType<真圣刃>()).AddIngredient(ModContent.ItemType<真夜刃>()).AddIngredient(ModContent.ItemType<SoulOfNature>(), 50).AddCondition(text).Register();
        }
    }
}