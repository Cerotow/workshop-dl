using DDmod.Content.Projectiles.RemoteControl;

namespace DDmod.Content.Items.Series.绿岩
{
    public class 绿岩设计图 : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = 5000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.channel = true;
        }
        public override bool? UseItem(Player player)
        {
            if (!player.Dplayer().GreenstoneRecipe)
            {
                player.Dplayer().GreenstoneRecipe = true;
                Main.NewText(Language.GetTextValue("Mods.DDmod.Recipes.学习绿岩"), 50, 155, 255);
            }
            return true;
        }
        public override void AddRecipes()
        {
        }
    }
}