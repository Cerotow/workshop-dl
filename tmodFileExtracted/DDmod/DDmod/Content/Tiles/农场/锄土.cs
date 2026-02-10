using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using DDmod.Worlds;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;
using DDmod.Content.Items.Series.Heart;
using DDmod.Content.Items.Tiles.晶凝;

namespace DDmod.Content.Tiles.农场
{
    public partial class 锄土
    {
        public Point16 tiles;
        public int Who = -1;
        public int DampTime = 0;
        public 锄土(int i,int j)
        {
            tiles = new Point16(i,j);
        }
        public void SaveWorldData(int Who,TagCompound tag)
        {
            tag.Add("锄土X"+Who, (int)tiles.X);
            tag.Add("锄土Y"+Who, (int)tiles.Y);
            tag.Add("锄土T"+Who, DampTime);
        }
        public void LoadWorldData(int Who, TagCompound tag)
        {
            int pointX=0, pointY=0;
            if (tag.ContainsKey("锄土X" + Who))
            {
                object rawX = tag["锄土X" + Who];
                if (rawX is short shortX)
                {
                    pointX = shortX;
                }
                else if (rawX is int intX)
                {
                    pointX = intX;
                }
            }
            if (tag.ContainsKey("锄土Y" + Who))
            {
                object rawX = tag["锄土Y" + Who];
                if (rawX is short shortX)
                {
                    pointX = shortX;
                }
                else if (rawX is int intX)
                {
                    pointX = intX;
                }
            }
            tiles = new Point16(pointX, pointY);
            DampTime = tag.Get<int>("锄土T" + Who);
        }

        public void NetSend(BinaryWriter writer)
        {
            writer.Write(tiles.X);
            writer.Write(tiles.Y);
            writer.Write(DampTime);
            writer.Write(Who);
        }

        public void NetReceive(BinaryReader reader)
        {
            tiles = new Point16(reader.ReadInt16(), reader.ReadInt16());
            DampTime = reader.ReadInt32();
            Who = reader.ReadInt32();
        }
        public static int New(Point16 point)
        {
            for (int a = 0; a < DDWorld.土.Length; a++)
            {
                if (DDWorld.土[a].tiles == Point16.Zero)
                {
                    DDWorld.土[a].tiles = point;
                    DDWorld.土[a].Who = a;
                    DDWorld.土[a].DampTime = 0;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket packet = DDmod.Instance.GetPacket(256);
                        //写入要发的包
                        packet.Write((byte)DDType.锄地);
                        packet.Write(point.X);
                        packet.Write(point.Y);
                        packet.Write((short)a);
                        packet.Write(0);
                        //发出去
                        packet.Send(-1, -1);
                    }
                    return a;
                }
            }
            return -1;
        }

        public static int FindFirstTile(Point16 point,out int type)
        {
            for (int a = 0; a < DDWorld.土.Length; a++)
            {
                if (DDWorld.土[a].tiles == point)
                {
                    type = a;
                    return a;
                }
            }
            type = -1;
            return -1;
        }
        public static bool FindFirstTileActive(Point16 point)
        {
            for (int a = 0; a < DDWorld.土.Length; a++)
            {
                if (DDWorld.土[a].tiles == point)
                {
                    return true;
                }
            }
            return false;
        }
        public void Update(int ID)
        {
            DampTime--;
            if(Main.tile[tiles.X,tiles.Y].TileType!=ModContent.TileType<锄过的土块>())
            {
               tiles = new Point16(0, 0);
            }
            Who = ID;
        }
    }
}