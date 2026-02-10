using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Content.NPCs.Boss.狱火蛇;

namespace DDmod.Content.Items.Boss.狱火蛇物品
{

    public class 火蛇卵 : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = (Item.height = 16);
            Item.rare = 9;
            Item.maxStack = 1;
            Item.useStyle = 4;
            Item.useTime = (Item.useAnimation = 20);
            Item.noMelee = true;
            Item.consumable = false;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item43;
        }
        public override bool CanUseItem(Player player)
        {
            return !AnyNPCs(ModContent.NPCType<狱火蛇头>())&&player.ZoneUnderworldHeight;
        }
        public override bool? UseItem(Player player)
        {
            SpawnOnPlayer(player.whoAmI, ModContent.NPCType<狱火蛇头>());
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(1006,12).AddIngredient(175,30).AddIngredient(521,10).AddCondition(Condition.NearLava).Register();
        }
    }
}
