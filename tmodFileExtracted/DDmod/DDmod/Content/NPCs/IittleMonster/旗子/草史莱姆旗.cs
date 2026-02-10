using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace DDmod.Content.NPCs.IittleMonster.旗子
{

	public class 草史莱姆旗 : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			// DisplayName.SetDefault("Acorn Spirit Banner");
		//DisplayName.AddTranslation(7, "橡果之灵旗");
			// Tooltip.SetDefault("");
		//Tooltip.AddTranslation(7, "");
		}
		public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<DDMonsterBanner>(), (int)DDMonsterBanner.StyleID.草史莱姆);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
			/*
            Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.rare = 1;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.createTile = ModContent.TileType<DDMonsterBanner>();
			Item.placeStyle = 0;*/
		}
	}
}
