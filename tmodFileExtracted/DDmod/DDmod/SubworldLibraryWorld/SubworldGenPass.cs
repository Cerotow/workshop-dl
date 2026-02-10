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

namespace DDmod.SubworldLibraryWorld
{
	public class SubworldGenPass : GenPass
	{
		public SubworldGenPass(Action<GenerationProgress> method) : base("", 1f)
		{
			this.method = method;
		}
		public SubworldGenPass(float weight, Action<GenerationProgress> method) : base("", weight)
		{
			this.method = method;
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			this.method.Invoke(progress);
		}

		private Action<GenerationProgress> method;
	}
}