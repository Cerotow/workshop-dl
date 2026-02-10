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
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.Items.Series.Venture.Level_1;
using DDmod.Content.Items.Series.Venture;
using static DDmod.SubworldLibraryWorld.SWSystem;
using StructureHelper.API;

namespace DDmod.SubworldLibraryWorld.草原
{
	//草原
	public class Grassland4 : Subworld
	{
		public override int Width => 360+ 10;
		public override int Height => 500;
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
			Main.spriteBatch.DrawString(FontAssets.DeathText.Value, Text+ Message, new Vector2(20 + texture.Width, Main.screenHeight - 14 - FontAssets.DeathText.Value.MeasureString(Text).Y), Color.White, 0, Vector2.Zero, 0.7F, 0, 0);
			DDOn.DDmodOn.Music = 1;
			DDOn.DDmodOn.MusicTime = 30;
		}
		public static List<GenPass> list = null;
		public static string Message;
		public static void World(int Width,int Height,Mod Mod)
        {
			list = new List<GenPass>();
			list.Add(new SubworldGenPass(1, delegate (GenerationProgress progress)
			{
				Message = Language.GetTextValue("Mods.DDmod.world.world1");
				for (int a = 0; a < Width; a++)
				{
					for (int b = Height - 1; b > Height - 250; b--)
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
                MultiStructureGenerator.GenerateMultistructureSpecific("SubworldLibraryWorld/草原/草原4", 1, new Point16(8, 115), Mod);
				for (int a = 6; a < 9; a++)
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
				Main.spawnTileY = 180;
				for (int a = 0; a < 100; a++)
				{
					if (!Main.tile[Main.spawnTileX, Main.spawnTileY].HasTile || !WorldGen.SolidTile(Main.spawnTileX, Main.spawnTileY))
					{
						Main.spawnTileY++;
					}
                    else
                    {
						break;
                    }
				}
				Main.hardMode = false;
				NPC.NewNPC(new EntitySource_WorldEvent(), Main.spawnTileX, Main.spawnTileY, ModContent.NPCType<HunterSlime>());
				胜利史莱姆箱(282, 187);
				NewProjectile(new EntitySource_WorldEvent(), new Vector2(296 *16,189 * 16), Vector2.Zero, ModContent.ProjectileType<Portals>(), 0, 0, 0);
			}));
		}
		public override List<GenPass> Tasks
		{
			get
			{
				return list;
			}
		}
		public static void 胜利史莱姆箱(int x, int y)
		{
			int PlacementSuccess = WorldGen.PlaceChest(x, y, (ushort)21, false, 34);
			if (PlacementSuccess >= 0)
			{
				Random ran = new();
				int[] Items = ALevelRewards.Items(1);
				Chest chest = Main.chest[PlacementSuccess];
				chest.name = Language.GetTextValue("Mods.DDmod.Level.草原4") + Language.GetTextValue("Mods.DDmod.Level.通关宝箱");
				int Citem = 0;
				chest.item[Citem].SetDefaults(Items[ran.Next(Items.Length)], false);
				chest.item[Citem].stack = 1;
				chest.item[Citem].AGItem().Quality = SWSystem.RandNext(10, 15, 25, 25, 15, 10);
				Citem++;
				chest.item[Citem].SetDefaults(9, false);
				chest.item[Citem].stack = ran.Next(100, 200);
				Citem++;
				chest.item[Citem].SetDefaults(23, false);
				chest.item[Citem].stack = ran.Next(40, 100);
				Citem++;
				chest.item[Citem].SetDefaults(29, false);
				chest.item[Citem].stack = ran.Next(1, 3);
				Citem++;
				chest.item[Citem].SetDefaults(28, false);
				chest.item[Citem].stack = ran.Next(3, 5);
				Citem++;
				chest.item[Citem].SetDefaults(73, false);
				chest.item[Citem].stack = ran.Next(1, 3);
				Citem++;
				chest.item[Citem].SetDefaults(72, false);
				chest.item[Citem].stack = ran.Next(10, 100);
				Citem++;
				chest.item[Citem].SetDefaults(71, false);
				chest.item[Citem].stack = ran.Next(30, 100);
				Citem++;
				if(ran.Next(2)==0)
                {
					chest.item[Citem].SetDefaults(ModContent.ItemType<StrengtheningStone>());
					chest.item[Citem].stack = 1;
					Citem++;
				}
                else if (ran.Next(5) == 0)
				{

                }
				if(ran.Next(10)==0)
                {
					chest.item[Citem].SetDefaults(ModContent.ItemType<凝胶背包>());
					chest.item[Citem].stack = 1;
					Citem++;
				}
			}
			NetMessage.SendObjectPlacement(-1, x, y, 21, 0, 0, -1, -1);
		}
		public override void OnLoad()
		{
			Message = "";
			Main.dayTime = true;
			SWSystem.Basicinfo(300);

			SWSystem.ReadSave();
		}
        public override void OnUnload()
        {
            base.OnUnload();
        }
    }
	//草原副本信息
	public class Grassland4System : ModSystem
	{
		/// <summary>
		/// 生成计数器
		/// </summary>
		public static int[] spawning = new int[1];
		/// <summary>
		/// 触发生成
		/// </summary>
		public static bool[] Trigger = new bool[1];
		/// <summary>
		/// 完成
		/// </summary>
		public static bool[] Finish = new bool [1];
		/// <summary>
		/// 草原通关
		/// </summary>
		public static bool Grassland4Finish;
        public override void Load()
		{
			Grassland4.World(360, 500, Mod);
		}
		public override void OnWorldLoad()
		{
			for (int a = 0; a < spawning.Length; a++)
			{
				spawning[a] = 0;
			}
			for (int a = 0; a < Trigger.Length; a++)
			{
				Trigger[a] = false;
			}
			if (SWSystem.path != null && SWSystem.path.Length > 4)
			{
				int L = SWSystem.path.Length - 4;
				if (L > Main.worldPathName.Length || Main.worldPathName.Remove(L) != SWSystem.path.Remove(L))
				{
					Grassland4Finish = false;
				}
			}
		}

		public override void OnWorldUnload()
		{
			for (int a = 0; a < spawning.Length; a++)
			{
				spawning[a] = 0;
			}
			for (int a = 0; a < Trigger.Length; a++)
			{
				Trigger[a] = false;
			}
			if (SWSystem.path != null && SWSystem.path.Length > 4)
			{
				int L = SWSystem.path.Length - 4;
				if (L < Main.worldPathName.Length || Main.worldPathName.Remove(L) != SWSystem.path.Remove(L))
				{
					Grassland4Finish = false;
				}
			}
		}
		public override void NetSend(BinaryWriter writer)
		{
			for (int a = 0; a < spawning.Length; a++)
			{
				writer.Write(spawning[a]);
			}
			BitsByte flags = new BitsByte();
			for (int a = 0; a < Trigger.Length; a++)
			{
				flags[a] = Trigger[a];
				flags[a+Trigger.Length] = Finish[a];
			}
			writer.Write(flags);
			writer.Write(Grassland4Finish);
		}

		public override void NetReceive(BinaryReader reader)
		{
			for (int a = 0; a < spawning.Length; a++)
			{
				spawning[a] = reader.ReadInt32();
			}
			BitsByte flags = reader.ReadByte();
			for (int a = 0; a < Trigger.Length; a++)
			{
				Trigger[a] = flags[a];
				Finish[a] = flags[a + Trigger.Length];
			}
			Grassland4Finish = reader.ReadBoolean();
		}
		public override void SaveWorldData(TagCompound tag)
		{
			if(Grassland4Finish) tag["Grassland4Finish"] = true;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			Grassland4Finish = tag.ContainsKey("Grassland4Finish");
		}
		Vector2 ScreenPosition;

		public override void PostDrawTiles()
		{
			if (SubworldSystem.IsActive<Grassland4>()&& Main.gamePaused)
			{
				//Main.LocalPlayer.Dplayer().Bossperspective(ScreenPosition, 2, false, 0f);
			}
		}
		public int Max = 0;
		public int Min = 0;
		public int Centre = 0;
		public override void PreUpdateTime()
		{
			if (SubworldSystem.IsActive<Grassland4>())
			{
				Main.dayTime = true;
				Main.time = 12000;
				Vector2 vector = Vector2.Zero;
				Main.LocalPlayer.Dplayer().Music = 1;
				float Height = (Main.spawnTileY-16)*16;
				if(Main.LocalPlayer.Center.Y+220>Height)
                {
					Height = Main.LocalPlayer.Center.Y + 220;
				}
				//第几波
				int wave = 0;
				for (int I = 0; I < Finish.Length; I++)
				{
					if (Finish[I])
					{
						wave++;
					}
				}
				if (wave < Trigger.Length && !Trigger[wave])
				{
					for (int a = 0; a < 255; a++)
					{
						if (Main.player[a].active && !Main.player[a].dead)
						{
							if (wave == 0 && Main.player[a].Center.X > 2000)
							{
								Trigger[wave] = true;
							}
						}
					}
					ScreenPosition = new Vector2(Main.LocalPlayer.Center.X, Height - Main.screenHeight / 4 / Main.GameZoomTarget);
					//Main.LocalPlayer.Dplayer().Bossperspective(ScreenPosition, 2, false, 0.3f);
				}
				else if (wave < Trigger.Length)
				{
					int num = 0;
					for (int a = 0; a < 200; a++)
					{
                        if (Main.npc[a].active && !Main.npc[a].friendly && Main.npc[a].Dnpc().Copy)
                        {
							num++;
						}
					}
					bool SP = false;
					if (num < 30)
					{
						SP = true;
						spawning[wave]++;
					}
					switch (wave)
					{
						//第一波
						case 0:
							Max = 4400;
							Min = 1600;
							Centre = (Max + Min) / 2;
							//视野锁定
							SWSystem.ScreenPosition(Main.LocalPlayer, ref vector.X, ref vector.Y, true, Max, Min);
							ScreenPosition = new Vector2(vector.X, Height - Main.screenHeight / 4 / Main.GameZoomTarget);
							//Main.LocalPlayer.Dplayer().Bossperspective(ScreenPosition, 2, false, 0.3f);
							if (spawning[wave] == 100 && SP)
							{
								if (Main.netMode != 1)
								{
									SWNewNPCs(new EntitySource_WorldEvent(), Centre, (Main.spawnTileY - 2) * 16, 50, 0);
								}
							}
							if (spawning[wave] >= 400 && SP)
							{
								Finish[wave] = num==0;
							}
							break;
					}
				}
				else
				{
					ScreenPosition = new Vector2(Main.LocalPlayer.Center.X, Height - Main.screenHeight / 4 / Main.GameZoomTarget);
					//Main.LocalPlayer.Dplayer().Bossperspective(ScreenPosition, 2, false, 0.3f);
				}
			}
			else if (Finish[Finish.Length-1])
			{
				Finish[Finish.Length - 1] = false;
				//草原完成
				if (!Grassland4Finish)
				{
					Grassland4Finish = true;
				}
			}
			else
			{
				for (int a = 0; a < Finish.Length; a++)
				{
					Finish[a] = false;
				}
			}

		}
    }
	public class Grassland4NPC :GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
        }
        public override bool PreAI(NPC npc)
		{
			if (SWSystem.TrueSubworld && (npc.aiStyle == 1 || npc.aiStyle == 3))
			{
				npc.TargetClosest();
			}
			return base.PreAI(npc);
        }
    }
}