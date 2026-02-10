using System;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;

namespace DDmod.Content.Items.Boss.MeteorAnnihilatorItems
{
	
    public class 奇怪的控制器 : ModItem
    {
		public override void SetStaticDefaults()
		{
		}

        public override void SetDefaults()
        {
            Item.width = (Item.height = 16);
            Item.rare = 9;
            Item.maxStack = Item.CommonMaxStack;
            Item.useStyle = 4;
            Item.useTime = (Item.useAnimation = 20);
            Item.noMelee = true;
            Item.consumable = false;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item43;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<MeteorAnnihilator>());
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<MeteorAnnihilator>());
			return true;
		}
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.MeteoriteBar, 12).AddIngredient(ModContent.ItemType<LargeMechanicalScrap>(), 12).AddTile(TileID.Anvils).AddCondition(new Condition(Language.GetTextValue("Mods.DDmod.Recipes.流星合成"), () => Main.LocalPlayer.Dplayer().MeteorRecipe)).Register();
        }
    }
}
