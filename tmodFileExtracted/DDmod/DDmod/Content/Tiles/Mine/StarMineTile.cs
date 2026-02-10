using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using DDmod.Worlds;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;
using DDmod.Content.Items.Series.Star;
using DDmod.Content.Dusts;
using Terraria.Chat;

namespace DDmod.Content.Tiles.Mine
{
	public class StarMineTile : ModTile
	{
		public static Asset<Texture2D> 星矿;
		public override void Load()
		{
			星矿 = ModContent.Request<Texture2D>(Texture + "Spawn");
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
            DustType = ModContent.DustType<魔力水晶粒子>();
            HitSound = SoundID.Tink;


			LocalizedText modTranslation = CreateMapEntryName();
			// modTranslation.SetDefault("Mana Ore");
			//modTranslation.AddTranslation(7, "星矿");

			AddMapEntry(new Color(0, 0, 255), modTranslation);
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<StarMine>();
			MinPick = 50;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return NPCDowned.downedStarGuard2;
		}
		public override bool CanExplode(int i, int j)
		{
			return NPCDowned.downedStarGuard2;
		}
		public override bool CanPlace(int i, int j)
		{
			return NPCDowned.downedStarGuard2;
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
	public class SpawnStarMineSystem : ModSystem
	{
		/// <summary> 生成星矿 </summary>
		public static bool SpawnStarMine = false;
		public override void OnWorldLoad()
		{
			SpawnStarMine = false;
		}

		public override void OnWorldUnload()
		{
			SpawnStarMine = false;
		}
		public override void SaveWorldData(TagCompound tag)
		{
			if (SpawnStarMine) tag["SpawnStarMine"] = true;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			SpawnStarMine = tag.ContainsKey("SpawnStarMine");
		}

		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = SpawnStarMine;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			SpawnStarMine = flags[0];
		}
        public int ColorBlock(byte bytes)
        {
            if (bytes == 1)
            {
                return ModContent.TileType<StarMineTile>();
            }
            return -1;
        }
		public void PlaceBlock(Texture2D texture, Point Position)
		{
			byte[] bytes =
			[

				0,0,0,0,0,0,1,0,0,0,0,0,0,
				0,0,0,0,0,1,1,1,0,0,0,0,0,
				0,0,0,0,0,1,1,1,0,0,0,0,0,
				0,0,0,0,1,1,1,1,1,0,0,0,0,
				1,1,1,1,1,1,1,1,1,1,1,1,1,
				1,1,1,1,1,1,1,1,1,1,1,1,1,
				0,1,1,1,1,1,1,1,1,1,1,1,0,
				0,0,1,1,1,1,1,1,1,1,1,0,0,
				0,0,1,1,1,1,1,1,1,1,1,0,0,
				0,1,1,1,1,1,1,1,1,1,1,1,0,
				0,1,1,1,1,1,0,1,1,1,1,1,0,
				1,1,1,1,0,0,0,0,0,1,1,1,1,
				1,1,0,0,0,0,0,0,0,0,0,1,1,
			];

			for (int i = 0; i < bytes.Length; i++)
			{
				int x = Position.X + i % texture.Width;
				int y = Position.Y + i / texture.Width;
				int Tile = ColorBlock(bytes[i]);
				if (Tile >= 0)
				{
					if (!Main.tile[x, y].HasTile || (Main.tile[x, y].TileType == 1 || Main.tile[x, y].TileType == 0 || Main.tile[x, y].TileType == 59))
					{
						Tile tile = Main.tile[x, y];
						tile.TileType = (ushort)Tile;
						tile.HasTile = true;
						WorldGen.SquareTileFrame(x, y, true);
						if (Main.netMode == 1)
							NetMessage.SendTileSquare(-1, x, y);
					}
				}
			}
		}
		public override void PreUpdateTime()
		{
			if (NPCDowned.downedStarGuard2 && !SpawnStarMine)
            {
                SpawnStarMine = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Language.GetTextValue("Mods.DDmod.WorldTips.StarMine")), new Microsoft.Xna.Framework.Color(180, 0, 255));
                    NetMessage.SendData(MessageID.WorldData);
                }
                else if (Main.netMode == 0)
                {
                    Main.NewText(Language.GetTextValue("Mods.DDmod.WorldTips.StarMine"), 0, 0, 255);
                }
				if (Main.netMode != 1)
                {
                    for (int k = 0; k < (int)(Main.maxTilesX * 0.003); k++)
                    {
                        Point origin;
                        origin = new Point(WorldGen.genRand.Next(Main.maxTilesX / 10, Main.maxTilesX - Main.maxTilesX / 10), WorldGen.genRand.Next((int)(Main.maxTilesY * 0.01f), (int)(Main.maxTilesY * 0.08f)));
                        PlaceBlock(StarMineTile.星矿.Value, origin);
                    }
                }
			}
		}
        public override void PreUpdateWorld()
		{
		}
	}
}