using DDmod.Content.NPCs.Boss.MeteorDigger;

namespace DDmod.Content.Items.Boss.MeteorDiggerItems
{

    public class AlienRigController : ModItem
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
            return !AnyNPCs(ModContent.NPCType<MeteorDigger_Head>());
        }

        public override bool? UseItem(Player player)
        {
           
            SpawnOnPlayer(player.whoAmI, ModContent.NPCType<MeteorDigger_Head>());
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.MeteoriteBar, 30).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.流星合成"), () => Main.LocalPlayer.Dplayer().MeteorRecipe)).Register();
        }
    }
}
