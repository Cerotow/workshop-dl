using DDmod.Content.Items.Boss.流星破坏者;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items.Series.Heart;
using DDmod.Content.Items.Series.杂物;
using DDmod.Content.Items.Series.钢;
using DDmod.NoContent.Config;
using System.Linq;
using Terraria;

namespace DDmod.Content.Items
{
    public class RecipesItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void AddRecipes()
        {
            ModifyRecipe();
            AddRecipe();
        }
        private void ModifyRecipe()
        {
            IEnumerable<Recipe> recipes = Enumerable.ToList(Main.recipe);

            List<Recipe> ListRecipes(int a) => Enumerable.ToList(recipes.Where((Recipe r) => r.createItem.type == a));

            //邪箭
            ListRecipes(47).ForEach(delegate (Recipe s)
            {
                s.requiredItem = new List<Item>();
                for (int A = 0; A < 2; A++)
                {
                    s.requiredItem.Add(DDmod.NewItem.Clone());
                }
                s.requiredItem[0].SetDefaults(40);
                s.requiredItem[0].stack = 50;
                s.requiredItem[1].SetDefaults(57);
                s.requiredItem[1].stack = 1;
                s.createItem.stack = 50;
            });
            Condition text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.强化台合成"), () => !ModContent.GetInstance<DDConfigServer>().HunterSlime);
            //永夜刃
            ListRecipes(273).ForEach(delegate (Recipe s)
            {
                s.DisableRecipe();
            });
            {
                Recipe recipe = Recipe.Create(273);
                recipe.AddIngredient(46);
                recipe.AddIngredient(190);
                recipe.AddIngredient(121);
                recipe.AddIngredient(155);
                recipe.AddCondition(text);
                recipe.Register();
            }
            {
                Recipe recipe = Recipe.Create(273);
                recipe.AddIngredient(795);
                recipe.AddIngredient(190);
                recipe.AddIngredient(121);
                recipe.AddIngredient(155);
                recipe.AddCondition(text);
                recipe.Register();
            }
            //真神圣
            ListRecipes(674).ForEach(delegate (Recipe s)
            {
                s.DisableRecipe();
            });
            {
                Recipe recipe = Recipe.Create(674);
                recipe.AddIngredient(368);
                recipe.AddIngredient(ModContent.ItemType<叶绿龟壳>());
                recipe.AddIngredient(520,20);
                recipe.AddIngredient(521,20);
                recipe.AddCondition(text);
                recipe.Register();
            }
            //真永夜
            ListRecipes(675).ForEach(delegate (Recipe s)
            {
                s.DisableRecipe();
            });
            {
                Recipe recipe = Recipe.Create(675);
                recipe.AddIngredient(273);
                recipe.AddIngredient(547,20);
                recipe.AddIngredient(548,20);
                recipe.AddIngredient(549,20);
                recipe.AddCondition(text);
                recipe.Register();
            }
            //泰拉刃
            ListRecipes(757).ForEach(delegate (Recipe s)
            {
                s.DisableRecipe();
            });
            {
                Recipe recipe = Recipe.Create(757);
                recipe.AddIngredient(1570);
                recipe.AddIngredient(674);
                recipe.AddIngredient(675);
                recipe.AddIngredient(ModContent.ItemType<SoulOfNature>(), 50);
                recipe.AddCondition(text);
                recipe.Register();
            }
            //迷你鲨
            {
                Recipe recipe = Recipe.Create(98);
                recipe.AddIngredient(ModContent.ItemType<迷你鲨设计图>(), 1);
                recipe.AddIngredient(319, 2);
                recipe.AddIngredient(ModContent.ItemType<钢锭>(), 12);
                recipe.AddIngredient(324, 1);
                recipe.AddTile(16);
                recipe.Register();
            }
            text = new Condition(Language.GetTextValue("Mods.DDmod.Recipes.流星合成"), () => Main.LocalPlayer.Dplayer().MeteorRecipe);
            //激光剑
            ListRecipes(198).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
            ListRecipes(199).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
            ListRecipes(200).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
            ListRecipes(201).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
            ListRecipes(202).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
            ListRecipes(203).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
            ListRecipes(4258).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
            //其他
            ListRecipes(204).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
            ListRecipes(127).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
            ListRecipes(197).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
            ListRecipes(123).ForEach(delegate (Recipe s)
            {
                s.AddIngredient(ModContent.ItemType<流星电池>(),2);
                s.AddCondition(text);
            });
            ListRecipes(124).ForEach(delegate (Recipe s)
            {
                s.AddIngredient(ModContent.ItemType<流星电池>(), 3);
                s.AddCondition(text);
            });
            ListRecipes(125).ForEach(delegate (Recipe s)
            {
                s.AddIngredient(ModContent.ItemType<流星电池>(), 1);
                s.AddCondition(text);
            });
            ListRecipes(2750).ForEach(delegate (Recipe s)
            {
                s.AddCondition(text);
            });
        }
        //添加合成配方
        private void AddRecipe()
        {
            Recipe recipe = Recipe.Create(29);
            recipe.AddIngredient(ModContent.ItemType<HeartMine>(), 20).Register();
        }
        public override void SetStaticDefaults()
        {
            //永夜
            RecipesSystem.Add(46,1, new Item(190), new Item(121), new Item(155), new Item(273));
            RecipesSystem.Add(795,1, new Item(190), new Item(121), new Item(155), new Item(273));
            //觉醒永夜
            RecipesSystem.Add(273,2, new Item(547,20), new Item(548,20), new Item(549,20), new Item(675));
            //觉醒神圣剑
            RecipesSystem.Add(368,3, new Item(520,20), new Item(521,20), new Item(ModContent.ItemType<叶绿龟壳>()), new Item(674));
            //泰拉刃
            RecipesSystem.Add(1570,4, new Item(674), new Item(675), new Item(ModContent.ItemType<SoulOfNature>(), 50), new Item(757));
        }
        public override void SetDefaults(Item item)
        {
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (RecipesSystem.SpecialMainMaterial[item.type])
            {
                tooltips.Add(new TooltipLine(Mod, "特殊主材料", Language.GetTextValue("Mods.DDmod.Recipes.特殊主材料")));

            }
        }
    }
    public class RecipesSystem : ModSystem
    {
        public static bool[] SpecialMainMaterial;
        public static int[] SyntheticEffects;
        public static Dictionary<int,Item[]> Material = new Dictionary<int, Item[]>();
        /// <summary>
        /// 主要合成物,1材料,2材料,3材料,4合成产物
        /// 合成特效 0,强化成功,1,永夜之刃,2.原版永夜3,原版神圣 4,泰拉,5强化失败
        /// </summary>
        public static void Add(int Type,int Effects, Item item, Item item2, Item item3, Item SyntheticsItem)
        {
            if(SpecialMainMaterial == null || SpecialMainMaterial.Length< ItemLoader.ItemCount)
            {
                Array.Resize(ref SpecialMainMaterial, ItemLoader.ItemCount);
            }
            if(SyntheticEffects ==null|| SyntheticEffects.Length < ItemLoader.ItemCount)
            {
                Array.Resize(ref SyntheticEffects, ItemLoader.ItemCount);
            }
            if (!SpecialMainMaterial[Type])
            {
                SpecialMainMaterial[Type] = true;
                SyntheticEffects[Type] = Effects;
                Material.Add(Type, new Item[] { item, item2, item3, SyntheticsItem });
            }
        }
        /// <summary>
        /// 主要合成物,1材料,2材料,3材料,4合成产物
        /// 合成特效 0,强化成功,1,永夜之刃,2.原版永夜3,原版神圣 4,泰拉,5强化失败
        /// </summary>
        public static void Add(int Type,int Effects, int item, int item2, int item3, int SyntheticsItem)
        {
            if (SpecialMainMaterial ==null || SpecialMainMaterial.Length < ItemLoader.ItemCount)
            {
                Array.Resize(ref SpecialMainMaterial, ItemLoader.ItemCount);
            }
            if (SyntheticEffects == null||SyntheticEffects.Length < ItemLoader.ItemCount)
            {
                Array.Resize(ref SyntheticEffects, ItemLoader.ItemCount);
            }
            if (!SpecialMainMaterial[Type])
            {
                SpecialMainMaterial[Type] = true;
                SyntheticEffects[Type] = Effects;
                Material.Add(Type, new Item[] { new Item(item), new Item(item2), new Item(item3), new Item(SyntheticsItem) });
            }
        }
        public override void Load()
        {
            base.Load();
        }
    }

}