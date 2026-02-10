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

	public class 绿岩史莱姆旗 : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<DDMonsterBanner>(), (int)DDMonsterBanner.StyleID.绿岩史莱姆);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
		}
	}
}
