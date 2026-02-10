using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using DDmod.Worlds;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;
using DDmod.Content.Items.Series.Heart;
using DDmod.Content.Dusts;
using Terraria.Chat;

namespace DDmod.Content.Tiles.Mine
{
	public class HeartMineTile : ModTile
	{
		public static Asset<Texture2D> 心矿;
		public override void Load()
		{
			心矿 = ModContent.Request<Texture2D>(Texture + "Spawn");
		}
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileOreFinderPriority[Type] = 255;
			Main.tileShine[Type] = 800;
			Main.tileLighted[Type] = true;
			DustType = ModContent.DustType<生命水晶粒子>();
			HitSound = SoundID.Tink;


			LocalizedText modTranslation = CreateMapEntryName();
			// modTranslation.SetDefault("Life Ore");
			//modTranslation.AddTranslation(7, "心矿");

			AddMapEntry(new Color(255, 0, 0), modTranslation);
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<HeartMine>();
			MinPick = 50;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return NPCDowned.downedLifeGuard2;
		}
		public override bool CanExplode(int i, int j)
		{
			return NPCDowned.downedLifeGuard2;
		}
		public override bool CanPlace(int i, int j)
		{
			return NPCDowned.downedLifeGuard2;
		}
		public override bool KillSound(int i, int j, bool fail)
		{
			if (!fail)
			{
				PlaySound(SoundID.Shatter, new Vector2(i, j) * 16);
			}
			return base.KillSound(i, j, fail);
		}
	}
	public class SpawnHeartMineSystem : ModSystem
	{
		/// <summary> 生成心矿 </summary>
		public static bool SpawnHeartMine = false;
		public override void OnWorldLoad()
		{
			SpawnHeartMine = false;
		}

		public override void OnWorldUnload()
		{
			SpawnHeartMine = false;
		}
		public override void SaveWorldData(TagCompound tag)
		{
			if (SpawnHeartMine) tag["SpawnHeartMine"] = true;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			SpawnHeartMine = tag.ContainsKey("SpawnHeartMine");
		}

		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = SpawnHeartMine;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			SpawnHeartMine = flags[0];
		}
		public int ColorBlock(byte bytes)
		{
			if (bytes==1)
			{
				return ModContent.TileType<HeartMineTile>();
			}
			return -1;
		}
		public void PlaceBlock(Point Position)
		{
            byte[] bytes =
            [

                0,0,1,1,1,0,0,0,1,1,1,0,0,
				0,1,1,1,1,1,0,1,1,1,1,1,0,
				1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,
				0,1,1,1,1,1,1,1,1,1,1,1,0,
				0,0,1,1,1,1,1,1,1,1,1,0,0,
				0,0,0,1,1,1,1,1,1,1,0,0,0,
				0,0,0,0,1,1,1,1,1,0,0,0,0,
				0,0,0,0,0,1,1,1,0,0,0,0,0,
				0,0,0,0,0,0,1,0,0,0,0,0,0,

        ]
                ;

            for (int i = 0; i < bytes.Length; i++)
			{
				int x = Position.X + i % 13;
				int y = Position.Y + i / 13;
				int Tile = ColorBlock(bytes[i]);
				if (Tile >= 0)
				{
					if (Main.tile[x, y].HasTile && (Main.tile[x, y].TileType == 1 || Main.tile[x, y].TileType == 0 || Main.tile[x, y].TileType == 59))
					{
						Main.tile[x, y].TileType = (ushort)Tile;
					}
				}

				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendTileSquare(-1, x, y);
				}
			}
		}
		public override void PreUpdateTime()
		{
			if (NPCDowned.downedLifeGuard2 && !SpawnHeartMine)
            {
                SpawnHeartMine = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Language.GetTextValue("Mods.DDmod.WorldTips.HeartMine")), new Microsoft.Xna.Framework.Color(180, 0, 255));
                    NetMessage.SendData(MessageID.WorldData);
                }
                else if (Main.netMode == 0)
                {
                    Main.NewText(Language.GetTextValue("Mods.DDmod.WorldTips.HeartMine"), 180, 0, 255);
                }
				if (Main.netMode != 1)
				{
					for (int k = 0; k < (int)(Main.maxTilesX * 0.05); k++)
					{
						Point origin;
						origin = new Point(WorldGen.genRand.Next(100, Main.maxTilesX - 100), WorldGen.genRand.Next((int)(Main.maxTilesY * 0.4f), (int)(Main.maxTilesY * 0.7f)));
						PlaceBlock(origin);
					}
				}
			}
		}
		public override void PreUpdateWorld()
		{
		}
	}
}