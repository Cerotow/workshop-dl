using DDmod.Content.Projectiles.Melee.FlyingKnife;
using DDmod.NoContent.Config;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 真夜刃 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            RecipesSystem.Add(ModContent.ItemType<夜刃>(),2, new Item(547,20), new Item(548, 20), new Item(549, 20), new Item(Type, 1));
        }
        public override void Defaults()
        {
            属性(64, 10, 28, 20, 1.5f, 5, 15, ModContent.ProjectileType<真夜刃Proj>());
            Value(0, 10, 0, 0);
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => !ModContent.GetInstance<DDConfigServer>().HunterSlime);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<夜刃>()).AddIngredient(547, 20).AddIngredient(548, 20).AddIngredient(549, 20).AddCondition(text).Register();
        }
    }
}