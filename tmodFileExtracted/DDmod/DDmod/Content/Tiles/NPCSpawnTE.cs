using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ObjectData;
using Terraria.ModLoader.Default;
using DDmod.UI.ItemUI;
using DDmod.Content.Projectiles.Melee.Sword;
using DDmod.Content.Projectiles.Melee;
using DDmod.Content.Items.Series.Acorn;
using DDmod.Content.Items;
using DDmod.Worlds;
using DDmod.Content.NPCs.EliteMonster;
using DDmod.Content.Tiles.绿岩;
using DDmod.UI;

namespace DDmod.Content.Tiles
{
	public class NPCSpawnTE : ModTileEntity
	{
		//生成怪物时间
		public int Time;
		//产卵频率
		public int SpawnTime;
		public bool Canspawn;
		//产卵运行中
		public bool SpawnRunning;
		/// <summary>
		/// 启动
		/// </summary>
		public bool Initiate = true;
		public bool Wire = false;
		public bool Dust = false;
		public byte WireTime = 0;
		public byte player;

        public NPCSpawnTE()
		{
		}
		public override bool IsTileValidForEntity(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile.HasTile&&tile.TileFrameY == 0;
		}
		public Vector2 Center
		{
			get
			{
				return Utils.ToWorldCoordinates(Position, 32f, 32f);
			}
		}
		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
		{
			TileObjectData tileData = TileObjectData.GetTileData(type, style, 0);
			Tile t = Main.tile[i, j];
			i -= (int)t.TileFrameX % (tileData.Width * 16) / 16;
			j -= (int)t.TileFrameY % (tileData.Height * 16) / 16;
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i, j, tileData.Width, tileData.Height);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, (float)j, Type, 0f, 0, 0, 0);
                return -1;
			}
			return Place(i, j);
		}
		public override void OnNetPlace()
		{
			//NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
		}
		public override void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
		{
		}
		public override void Update()
        {
            Tile tile = Main.tile[Position.X, Position.Y];
			TileObjectData tileData = TileObjectData.GetTileData(tile.TileType, 0, 0);
            if (tileData==null)
            {
                return;
            }
			if (Main.netMode == 2)
			{
				Dust = false;
			}
			if (Main.netMode != 1)
            {
				if (SpawnTime == 0)
				{
					if (DDUISystem.TR != 0)
					{
						SpawnTime = DDUISystem.TR;

					}
					else
					{
						SpawnTime = 120;

					}
				}

                player = Player.FindClosest(new Vector2(Position.X, Position.Y) * 16, 1, 1);

				if (Time == 0 && tile.TileType != ModContent.TileType<绿岩炮台Tile>())
				{
					Time = Main.rand.Next(600);

                }
				if ((new Point16((int)Main.player[player].Center.X / 16, (int)Main.player[player].Center.Y / 16) - Position).ToVector2().Length() < 100)
				{
                    //Main.NewText(Initiate);
                    //Main.NewText(Time);
                }
                Wire = false;
                TileLoader.HitWire(Position.X, Position.Y,tile.TileType);

                if (Canspawn)
                {
                    Time++;
                    Canspawn = false;
                }
				if(WireTime>0)
				{
					WireTime--;

                }
            }

            /*
			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
			}*/
        }
        public override void PreGlobalUpdate()
        {
        }
        public override void OnPlayerUpdate(Player player)
        {
        }

        public override void OnKill()
		{
		}
		public override void SaveData(TagCompound tag)
		{
			tag.Add("SpawnTime", Time);
			tag.Add("SpawnSpawnTime", SpawnTime);
			tag.Add("SpawnInitiate", Initiate);

		}
		public override void LoadData(TagCompound tag)
		{
			Time = tag.Get<int>("SpawnTime");
            SpawnTime = tag.Get<int>("SpawnSpawnTime");
            //if (Initiate) tag["SpawnInitiate"] = true;
            Initiate = tag.Get<bool>("SpawnInitiate"); 
        }
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(Initiate);
			writer.Write(Time);
			writer.Write(Dust);
			writer.Write(SpawnRunning);

        }
		public override void NetReceive(BinaryReader reader)
		{
			Initiate = reader.ReadBoolean();
            Time = reader.ReadInt32();
            Dust = reader.ReadBoolean();
            SpawnRunning = reader.ReadBoolean();
        }
        public static NPCSpawnTE Entity(int i, int j)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Main.tile[i, j].TileType, 0, 0);

            return playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
        }
        public static void Read(BinaryReader reader)
        {
            Vector2 vector = reader.ReadVector2();
            bool Canspawn = reader.ReadBoolean();

            Entity((int)vector.X, (int)vector.Y).Initiate = Canspawn;
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, number: Entity((int)vector.X, (int)vector.Y).ID, number2: vector.X, number3: vector.Y);
            }
        }

    }
}
