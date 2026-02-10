using Microsoft.Xna.Framework;
using System;

using Terraria.ID;
using Terraria.ModLoader;

namespace slime.Items.回旋镖
{
	public class 血之渴望 : ModItem
	{
		public override void SetStaticDefaults()
		{
		//DisplayName.SetDefault("血之渴望");
		//Tooltip.SetDefault("扔出一把偷取敌人鲜血的回旋镖,回来会治愈你\n穿透两个敌人才会返回");
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.melee = true;
			Item.width = 30;
			Item.noUseGraphic = true;
			Item.maxStack = 1;
			Item.consumable = false;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.noMelee = true;
			Item.shoot = Mod.Find<ModProjectile>("血之渴望").Type;
			Item.shootSpeed = 5f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 1;
			Item.value = 12000;
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}
        public override bool CanUseItem(Player player)
        {
			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("血之渴望").Type] != 0)
			{
				return false;
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(Mod);
			modRecipe.AddIngredient(ItemID.CrimtaneBar, 12);
			modRecipe.AddIngredient(ItemID.TissueSample, 20);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
