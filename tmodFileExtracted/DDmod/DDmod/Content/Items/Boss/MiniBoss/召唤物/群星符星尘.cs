using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.NPCs.Boss.MeteorDigger;
using DDmod.Content.NPCs.EliteMonster.四柱护卫;

namespace DDmod.Content.Items.Boss.MiniBoss.召唤物
{

    public class 群星符星尘 : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = (Item.height = 16);
            Item.rare = 10;
            Item.maxStack = 1;
            Item.useStyle = 4;
            Item.useTime = (Item.useAnimation = 10);
            Item.noMelee = true;
            Item.consumable = false;
            Item.autoReuse = false;
            Item.UseSound = null;
        }
        public override bool CanUseItem(Player player)
        {
            return !AnyNPCs(ModContent.NPCType<星尘护卫头>()) && !AnyNPCs(493);
        }

        public override bool? UseItem(Player player)
        {
           
            SpawnOnPlayer(player.whoAmI, ModContent.NPCType<星尘护卫头>());
            return true;
        }
        public override void AddRecipes()
        {
        }
    }
}
