using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Items.Melee.Tool.Hoe
{
	public class 木锄头 : ModItem
	{
		public override void SetStaticDefaults()
		{
		}
		public override void SetDefaults()
		{
			Item.damage = 2;
			Item.DamageType = DamageClass.Melee;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 2.5f;
			Item.value = Item.buyPrice(0, 0, 0, 50);
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.GetGlobalItem<MeleeGlobalItem>().Hoe = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(9, 18).AddTile(18).Register();
		}
	}
}
