using Terraria;

namespace DDmod.Sync
{
    public static class SNPC 
    {
        //同步位置(客户端)
        public static void SyncNPC(Mod mod, BinaryReader reader)
        {
            byte npc = reader.ReadByte();
            int Control = reader.ReadInt32();
            Vector2 Center = reader.ReadVector2();
            Vector2 Velocityr = reader.ReadVector2();
            int player = reader.ReadInt32();
            Main.npc[npc].Center = Center;
            Main.npc[npc].velocity = Velocityr;
            Main.npc[npc].Dnpc().Control = Control;
            if (Main.netMode == 2)
            {
                DDmod.SyncData(DDType.NPCCenter, npc, -1, player);
            }
        }
        //同步血量(服务器端)
        public static void SyncNPCLife(Mod mod, BinaryReader reader)
        {
            byte npc = reader.ReadByte();
            int life = reader.ReadInt32();
            int lifeMax = reader.ReadInt32();
            Main.npc[npc].life = life;
            Main.npc[npc].lifeMax = lifeMax;
            if (Main.netMode == 2)
            {
                DDmod.SyncData(DDType.NPCLife, npc, -1, -1);
            }
        }
    }
}