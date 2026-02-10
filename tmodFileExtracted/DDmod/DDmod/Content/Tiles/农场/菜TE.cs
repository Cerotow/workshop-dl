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

namespace DDmod.Content.Tiles.农场
{
	public class 菜TE : ModTileEntity
	{
		//菜成长时间
		public int Time;
		//自动同步时间
		public int netTime;
		//菜在水中的时间
		public int DampTime;
		//方向
		public int direction;
		//判定作物变异
		public bool variation;
		public bool variationB;
		public 菜TE()
		{
		}
		public override bool IsTileValidForEntity(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile.HasTile && tile.TileFrameX == 0 && tile.TileFrameY == 0;
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
			if (tileData == null)
			{
				return;
			}
			if (direction == 0)
			{
				direction = Main.rand.NextBool(2) ? 1 : -1;
			}
			int B = 0;
			for (int a = 0; a <= tileData.Height; a++)
			{
				for (int b = 0; b < tileData.Width; b++)
				{
					if (锄土.FindFirstTile(new Point16(Position.X + b, Position.Y + a), out int type) >= 0 && DDWorld.土[type].DampTime > 0 && Main.tile[Position.X + b, Position.Y + a].TileType == ModContent.TileType<锄过的土块>())
					{
						B++;
					}
					if (TileID.Sets.IsATreeTrunk[tile.TileType] && Main.tile[Position.X + b, Position.Y + a].TileType != ModContent.TileType<锄过的土块>())
					{
						B++;
						direction = 1;
						break;
					}
				}
			}

			if (Main.maxRaining > 0)
			{
				DampTime = DDHelper.Second(300);
			}
			Time += B;
			DampTime--;
			if (!variationB)
			{
				if (Main.rand.NextBool(100))
				{
					variation = true;
				}
				variationB = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
				}
			}
			netTime++;
			bool Net = false;
			for (int a = 0; a < 255; a++)
			{
				if (Main.player[a].active && (Main.player[a].Center - new Vector2(Position.X, Position.Y) * 16).Length() < 2000)
				{
					Net = true;
					break;
				}
			}
			if (Main.netMode == NetmodeID.Server && netTime >= 600 && Net)
			{
				netTime = 0;
				NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
			}
		}
		public override void OnKill()
		{
		}
		public override void SaveData(TagCompound tag)
		{
			tag.Add("菜Time", Time);
			tag.Add("菜DampTime", DampTime);
			tag.Add("菜variation", variation);
			tag.Add("菜variationB", variationB);
			tag.Add("菜direction", direction);

		}
		public override void LoadData(TagCompound tag)
		{
			Time = tag.Get<int>("菜Time");
			DampTime = tag.Get<int>("菜DampTime");
			variation = tag.Get<bool>("菜variation");
			variationB = tag.Get<bool>("菜variationB");
			direction = tag.Get<int>("菜direction");
		}
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(Time);
			writer.Write(DampTime);
			writer.Write(variation);
			writer.Write(variationB);
			writer.Write(direction);
		}
		public override void NetReceive(BinaryReader reader)
		{
			Time = reader.ReadInt32();
			DampTime = reader.ReadInt32();
			variation = reader.ReadBoolean();
			variationB = reader.ReadBoolean();
			direction = reader.ReadInt32();
		}
		public static 菜TE Entity(int i, int j)
		{
			TileObjectData tileData = TileObjectData.GetTileData(Main.tile[i, j].TileType, 0, 0);

			return playerHelper.FindTileEntity2<菜TE>(i, j, tileData.Width, tileData.Height, 18);
		}
		public static void Read(BinaryReader reader)
		{
			Vector2 vector = reader.ReadVector2();
			int r = reader.ReadInt32();

			Entity((int)vector.X, (int)vector.Y).Time = r;
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, Entity((int)vector.X, (int)vector.Y).ID, number2: vector.X, number3: vector.Y);
            }
        }

	}
}
