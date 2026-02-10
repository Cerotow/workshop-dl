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
using DDmod.SubworldLibraryWorld.草原;
using static DDmod.SubworldLibraryWorld.SWSystem;

namespace DDmod.SubworldLibraryWorld
{
	//墓地
	public class Graveyard : Subworld
	{
		public override int Width => 208 + 10;
        public override int Height => 200;
		public override bool ShouldSave => false;
		public override bool NormalUpdates => true;
		int A = 0;
		int Tiles = 0;
		int Frame = 0;
		public override void DrawMenu(GameTime gameTime)
		{
			Tiles++;
			if (Tiles % 30 == 0)
			{
				A++;
			}
			if (Tiles % 5 == 0)
			{
				Frame++;
				if (Frame > 5)
				{
					Frame = 0;
				}
			}

			string Text = "Loading.....".Remove(8 + A % 4);
			Texture2D texture = ModContent.Request<Texture2D>("DDmod/SubworldLibraryWorld/草原/草原").Value;
			Main.spriteBatch.Draw(texture, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, Color.White, 0, texture.Size() / 2, 1, 0, 0);
			texture = ModContent.Request<Texture2D>("DDmod/Content/NPCs/TownNPC/猎人史莱姆模板").Value;
			Main.spriteBatch.Draw(texture, new Vector2(20, Main.screenHeight - 20 - FontAssets.DeathText.Value.MeasureString(Text).Y), new Rectangle?(new Rectangle(0, texture.Height / 6 * Frame, texture.Width, texture.Height / 6)), Color.White, 0, Vector2.Zero, 1, (SpriteEffects)1, 0);
			Main.spriteBatch.DrawString(FontAssets.DeathText.Value, Text + Message, new Vector2(20 + texture.Width, Main.screenHeight - 14 - FontAssets.DeathText.Value.MeasureString(Text).Y), Color.White, 0, Vector2.Zero, 0.7F, 0, 0);
			DDOn.DDmodOn.Music = MusicID.Graveyard;
			DDOn.DDmodOn.MusicTime = 30;
		}
		public static string Message;
		public override List<GenPass> Tasks
		{
			get
			{
				List<GenPass> list = new List<GenPass>();

				list.Add(new SubworldGenPass(1, delegate (GenerationProgress progress)
				 {
					 Message = Language.GetTextValue("Mods.DDmod.world.world1");
					 for (int a = 0; a < Width; a++)
					 {
						 for (int b = Height - 1; b > Height - 100; b--)
						 {
							 Tile tile = Main.tile[a, b];
							 tile.TileType = 0;
							 tile.HasTile = true;
						 }
					 }
				 }));

				list.Add(new SubworldGenPass(2, delegate (GenerationProgress progress)
				 {
					 Message = Language.GetTextValue("Mods.DDmod.world.world2");
					 GenerateStructure("SubworldLibraryWorld/草原/墓地", new Point16(0, 45), Mod);
					 for (int a = 6; a < 9; a++)
					 {
						 for (int b = 0; b < Height; b++)
						 {
							 Tile tile = Main.tile[a, b];
							 tile.TileType = 2;
							 tile.HasTile = true;
						 }
					 }

					 for (int a = Width - 8; a < Width - 8 + 3; a++)
					 {
						 for (int b = 0; b < Height; b++)
						 {
							 Tile tile = Main.tile[a, b];
							 tile.TileType = 2;
							 tile.HasTile = true;
						 }
					 }

				 }));

				list.Add(new SubworldGenPass(3, delegate (GenerationProgress progress)
				 {
					 Message = Language.GetTextValue("Mods.DDmod.world.world3");
					 Main.spawnTileX = 50;
					 Main.spawnTileY = 120;
					 NPC.NewNPC(new EntitySource_WorldEvent(), Main.spawnTileX, Main.spawnTileY, ModContent.NPCType<HunterSlime>());
				 }));
				return list;
			}
		}
		public override void OnLoad()
		{
			Message = "";
			Main.dayTime = true;
			SWSystem.Basicinfo(500);
			SWSystem.ReadSave();
		}
		public override void OnUnload()
		{
			base.OnUnload();
		}
	}
	//墓地(困难)
	public class Graveyard2 : Subworld
	{
		public override int Width => 208;
		public override int Height => 200;
		public override bool ShouldSave => false;
		public override bool NormalUpdates => true;
		public override List<GenPass> Tasks
		{
			get
			{
				List<GenPass> list = new List<GenPass>();

				list.Add(new SubworldGenPass(1, delegate (GenerationProgress progress)
				 {
					 progress.Message = Language.GetTextValue("Mods.DDmod.world.world1");
					 for (int a = 0; a < Width; a++)
					 {
						 for (int b = Height - 1; b > Height - 100; b--)
						 {
							 Tile tile = Main.tile[a, b];
							 tile.TileType = 0;
							 tile.HasTile = true;
						 }
					 }
				 }));

				list.Add(new SubworldGenPass(2, delegate (GenerationProgress progress)
				 {
					 progress.Message = Language.GetTextValue("Mods.DDmod.world.world2");
					 GenerateStructure("SubworldLibraryWorld/墓地", new Point16(0, 45), Mod);
					 for (int a = 6; a < 9; a++)
					 {
						 for (int b = 0; b < Height; b++)
						 {
							 Tile tile = Main.tile[a, b];
							 tile.TileType = 2;
							 tile.HasTile = true;
						 }
					 }

					 for (int a = Width - 8; a < Width - 8 + 3; a++)
					 {
						 for (int b = 0; b < Height; b++)
						 {
							 Tile tile = Main.tile[a, b];
							 tile.TileType = 2;
							 tile.HasTile = true;
						 }
					 }

				 }));

				list.Add(new SubworldGenPass(3, delegate (GenerationProgress progress)
				 {
					 progress.Message = Language.GetTextValue("Mods.DDmod.world.world3");
					 Main.spawnTileX = 50;
					 Main.spawnTileY = 120;
					 NPC.NewNPC(new EntitySource_WorldEvent(), Main.spawnTileX, Main.spawnTileY, ModContent.NPCType<HunterSlime>());
				 }));
				return list;
			}
		}
		public override void OnLoad()
		{
			Main.worldSurface = 500;
			Main.dayTime = true;
			SWSystem.TrueSubworld = true;
			SWSystem.ForbidVandalism = true;
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
		public override void OnUnload()
		{
			base.OnUnload();
		}
	}
	//墓地副本信息
	public class GraveyardSystem : ModSystem
	{
		/// <summary>
		/// 这个世界有玩家探索过墓地
		/// </summary>
		public static bool GraveyardExplore;
		/// <summary>
		/// 生成计数器
		/// </summary>
		public static int spawning;
		/// <summary>
		/// 触发生成
		/// </summary>
		public static bool Trigger;
		/// <summary>
		/// 完成
		/// </summary>
		public static bool Finish;
		/// <summary>
		/// 墓地通关
		/// </summary>
		public static bool GraveyardFinish;
		public override void OnWorldLoad()
		{
			spawning = 0;
			Trigger = false;
			GraveyardFinish = false;
			GraveyardExplore = false;
		}
		public override void OnWorldUnload()
		{
			spawning = 0;
			Trigger = false;
			GraveyardFinish = false;
			GraveyardExplore = false;
		}
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(spawning);
			BitsByte flags = new BitsByte();
			flags[0] = Trigger;
			flags[1] = Finish;
			flags[2] = GraveyardFinish;
			flags[3] = GraveyardExplore;
			writer.Write(flags);

		}

		public override void NetReceive(BinaryReader reader)
		{
			spawning = reader.ReadInt32();
			BitsByte flags = reader.ReadByte();
			Trigger = flags[0];
			Finish = flags[1];
			GraveyardFinish = flags[2];
			GraveyardExplore = flags[3];
		}
		public override void SaveWorldData(TagCompound tag)
		{
			if(GraveyardFinish) tag["GraveyardFinish"] = GraveyardFinish;
			if (GraveyardExplore) tag["GraveyardExplore"] = GraveyardExplore;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			GraveyardFinish = tag.ContainsKey("GraveyardFinish");
			GraveyardExplore = tag.ContainsKey("GraveyardExplore");
		}
		public override void PreUpdateTime()
		{
			if (SubworldSystem.IsActive<Graveyard>() || SubworldSystem.IsActive<Graveyard2>())
			{
				Main.LocalPlayer.Dplayer().Music = MusicID.Graveyard;
				if (!Main.tile[Main.spawnTileX, Main.spawnTileY].HasTile || !WorldGen.SolidTile(Main.spawnTileX, Main.spawnTileY))
				{
					Main.spawnTileY++;
				}
				if (!Trigger)
				{
					for (int a = 0; a < 255; a++)
					{
						if (Main.player[a].active && !Main.player[a].dead && Main.player[a].Center.X / 16 > 100)
						{
							Trigger = true;
						}
					}
				}
				else
				{
					spawning++;
					if (spawning == 300)
					{
						SWNewNPCs(new EntitySource_WorldEvent(), (Main.maxTilesX - 20) * 16, (Main.spawnTileY - 5) * 16, ModContent.NPCType<WitheredAcornSpirit>(), 1, 1);
					}
					if (spawning >= 400)
					{
						bool Bool = true;
						for (int a = 0; a < 200; a++)
						{
                            if (Main.npc[a].active && !Main.npc[a].friendly && Main.npc[a].Dnpc().Copy)
                            {
								Bool = false;
							}
						}
						if (Bool)
						{
							Finish = true;
						}
					}
				}
			}
			else if (Finish)
			{
				if (!Grassland2System.Grassland2Finish)
				{
					DDHelper.newText(Language.GetTextValue("Mods.DDmod.Level.草原3") + Language.GetTextValue("Mods.DDmod.Lock.解锁"), new Color(0, 200, 0));

					Grassland2System.Grassland2Finish = true;
				}
				if (!GraveyardExplore)
				{
					DDHelper.newText(Language.GetTextValue("Mods.DDmod.Level.墓地") + Language.GetTextValue("Mods.DDmod.Lock.解锁"), new Color(0, 200, 0));
					NPCDowned.downedWitheredAcornSpirit = true;
					GraveyardExplore = true;
					Finish = false;
				}
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
				//if (!GraveyardFinish)
				{
					//DDHelper.newText("墓地(困难)已解锁,找到猎人史莱姆前往", new Color(0, 200, 0));
					//GraveyardFinish = true;
				}
			}
		}
	}
}