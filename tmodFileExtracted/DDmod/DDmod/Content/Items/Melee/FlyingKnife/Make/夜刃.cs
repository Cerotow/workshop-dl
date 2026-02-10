using DDmod.Content.Projectiles.Melee.FlyingKnife;
using DDmod.NoContent.Config;
using Terraria.ID;

namespace DDmod.Content.Items.Melee.FlyingKnife.Make
{
    public class 夜刃 : 飞刀
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            RecipesSystem.Add(ModContent.ItemType<魔金飞刀>(),1, ModContent.ItemType<刺草>(), ModContent.ItemType<远古短刀>(), ModContent.ItemType<烈焰飞刀>(),Type);
            RecipesSystem.Add(ModContent.ItemType<血猩獠牙>(),1, ModContent.ItemType<刺草>(), ModContent.ItemType<远古短刀>(), ModContent.ItemType<烈焰飞刀>(),Type);
        }
        public override void Defaults()
        {
            属性(16, 10, 28, 15, 1.5f, 5, 15, ModContent.ProjectileType<夜刃Proj>());
            Value(0, 10, 0, 0);
        }
        public override void AddRecipes()
        {
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => !ModContent.GetInstance<DDConfigServer>().HunterSlime);
            CreateRecipe(1).AddIngredient(ModContent.ItemType<血猩獠牙>()).AddIngredient(ModContent.ItemType<刺草>()).AddIngredient(ModContent.ItemType<远古短刀>()).AddIngredient(ModContent.ItemType<烈焰飞刀>()).AddCondition(text).Register();
            CreateRecipe(1).AddIngredient(ModContent.ItemType<魔金飞刀>()).AddIngredient(ModContent.ItemType<刺草>()).AddIngredient(ModContent.ItemType<远古短刀>()).AddIngredient(ModContent.ItemType<烈焰飞刀>()).AddCondition(text).Register();
        }
    }
}