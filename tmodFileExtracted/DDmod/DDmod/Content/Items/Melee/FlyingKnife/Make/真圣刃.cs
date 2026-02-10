using DDmod.Content.Items.Series.杂物;
using DDmod.Content.Projectiles.Melee.FlyingKnife;
using DDmod.NoContent.Config;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 真圣刃 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            RecipesSystem.Add(ModContent.ItemType<圣刃>(),3, new Item(520,20), new Item(521, 20), new Item(ModContent.ItemType<叶绿龟壳>()), new Item(Type, 1));
        }
        public override void Defaults()
        {
            属性(50, 10, 28, 12, 1.5f, 5, 15, ModContent.ProjectileType<真圣刃Proj>());
            Value(0, 10, 0, 0);
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => !ModContent.GetInstance<DDConfigServer>().HunterSlime);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<圣刃>()).AddIngredient(520, 20).AddIngredient(521, 20).AddIngredient(ModContent.ItemType<叶绿龟壳>()).AddCondition(text).Register();
        }
    }
}