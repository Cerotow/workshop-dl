using DDmod.Content.Items.Boss.LifeGuardItems;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Boss.恐惧缝合体;
using DDmod.Content.NPCs.Boss.天地守卫;
using DDmod.Content.NPCs.Boss.星心守卫;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Boss.天地守卫
{
	public class 星心石 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 3200;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = false;
		}

		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(ModContent.NPCType<苍穹守卫替>())&& !NPC.AnyNPCs(ModContent.NPCType<大地守卫替>())&&!NPC.AnyNPCs(ModContent.NPCType<苍穹守卫>())&& !NPC.AnyNPCs(ModContent.NPCType<大地守卫>());
		}

		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<苍穹守卫替>());
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<大地守卫替>());
			return true;
		}

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<SuspiciousHeartStone>(), 1).AddIngredient(ModContent.ItemType<SuspiciousStarStone>(), 1).AddIngredient(520, 25).AddIngredient(521, 25).Register();
        }
    }
}