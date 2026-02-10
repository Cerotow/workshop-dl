using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.IO;
using StructureHelper;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.Default;
using DDmod.Worlds;
using DDmod.Content.NPCs.TownNPC;
using DDmod.Content.NPCs.EliteMonster;
using Terraria.Chat;
using Terraria.Map;
using DDmod.Content.Items;
using DDmod.Content.Items.Series.Venture.Level_1;

namespace DDmod.SubworldLibraryWorld
{
	//草原
	public static class ALevelRewards
	{
		/// <summary>
		/// 根据等级随机指定奖励
		/// </summary>
		/// <param name="Level"></param>
		/// <returns></returns>
		public static int[] Items(int Level)
        {
			//草原随机掉落物
			if(Level==1)
			{
				return new[] { ModContent.ItemType<木盾>(), ModContent.ItemType<生命木剑>() , ModContent.ItemType<青弦>(),ModContent.ItemType<生命木杖物品>(),ModContent.ItemType<生命呼唤>()};
			}
			return null;
        }
	}
}