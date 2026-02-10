using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Melee.Tool.Pick
{
	public class 花岗岩能量镐 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.damage = 300;
			Item.DamageType = DamageClass.Melee;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2.5f;
			Item.value = Item.buyPrice(1, 0, 0, 0);
			Item.rare = 9;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.pick = 350;
		}

		public override void AddRecipes()
		{
			//reateRecipe(1).AddIngredient(ItemID.LeadBar, 18).AddIngredient(9, 8).AddTile(16).Register();
		}
	}
}
