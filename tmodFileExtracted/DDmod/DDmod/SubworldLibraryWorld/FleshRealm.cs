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

namespace DDmod.SubworldLibraryWorld
{
	public class FleshRealm : Subworld
	{
		public override int Width => 208;
		public override int Height => 220;
		public override bool ShouldSave => false;
		public override bool NormalUpdates => true;
		public override List<GenPass> Tasks
		{
			get
			{
				List<GenPass> list = new List<GenPass>();
				return list;
			}
		}
        public override void OnUnload()
        {
		}
        public override void OnLoad()
		{
			Main.worldSurface = 300;
			Main.dayTime = true;
			SWSystem.TrueSubworld = true;
			string path = SWSystem.path;
			path = Path.ChangeExtension(path, ".twld");

			if (!FileUtilities.Exists(path, false))
				return;

			byte[] buf = FileUtilities.ReadAllBytes(path, false);

			if (buf[0] != 0x1F || buf[1] != 0x8B)
			{
				return;
			}
			var From = TagIO.FromStream(new MemoryStream(buf));
			var list = From.GetList<TagCompound>("modData");
			foreach (var tag in list)
			{
				if (ModContent.TryFind(tag.GetString("mod"), tag.GetString("name"), out ModSystem system))
				{
					try
					{
						system.LoadWorldData(tag.GetCompound("data"));
					}
					catch (Exception e)
					{
						throw new CustomModDataException(system.Mod,
							"Error in reading custom world data for " + system.Mod.Name, e);
					}
				}
				else
				{
					//ModContent.GetInstance<UnloadedSystem>().SaveWorldData(tag);
				}
			}
		}
	}
}