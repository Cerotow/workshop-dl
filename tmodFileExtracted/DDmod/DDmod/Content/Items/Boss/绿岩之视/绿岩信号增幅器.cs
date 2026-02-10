using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.NPCs.Boss.MeteorDigger;

namespace DDmod.Content.Items.Boss.绿岩之视
{

    public class 绿岩信号增幅器 : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = (Item.height = 16);
            Item.rare = 4;
            Item.maxStack = 1;
            Item.useStyle = 4;
            Item.useTime = (Item.useAnimation = 150);
            Item.noMelee = true;
            Item.consumable = false;
            Item.autoReuse = false;
            Item.UseSound = new SoundStyle(DDHelper.Sound(1,"绿岩科技"));
        }
        public override bool CanUseItem(Player player)
        {
            return !AnyNPCs(ModContent.NPCType<Content.NPCs.Boss.绿岩之视.绿岩之视>());
        }

        public override bool? UseItem(Player player)
        {
           
            SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Content.NPCs.Boss.绿岩之视.绿岩之视>());
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<绿岩锭>(), 16).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.绿岩合成"), () => Main.LocalPlayer.Dplayer().GreenstoneRecipe)).Register();
        }
    }
}
