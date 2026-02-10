namespace DDmod.Textures
{
    public class DDItemTextures
    {
        public static void ItemT(int type)
        {
            Item[type] = TextureAssets.Item[type];
            TextureAssets.Item[type] = ModContent.Request<Texture2D>("DDmod/Textures/Items/Item_" + type);
        }
        public static void LoadItemTextures()
        {
            if (Main.dedServ)
            {
                return;
            }
            ItemT(4);
            ItemT(24);
            ItemT(112);
            ItemT(273);
            ItemT(368);
            ItemT(653);
            ItemT(656);
            ItemT(659);
            ItemT(674);
            ItemT(675);
            ItemT(723);
            ItemT(757);
            ItemT(921);
            ItemT(989);
            ItemT(1227);
            ItemT(1264);
            ItemT(2517);
            ItemT(2745);
            ItemT(3484);
            ItemT(3490);
            ItemT(3496);
            ItemT(3502);
            ItemT(3508);
            ItemT(3514);
            ItemT(3520);
            ItemT(5284);
            Mouse[0] = TextureAssets.Cursors[0];
            Mouse[1] = TextureAssets.Cursors[1];
            Mouse[2] = TextureAssets.Cursors[11];
            Mouse[3] = TextureAssets.Cursors[12];
            Mouse[4] = TextureAssets.CursorRadial;
            Mouse[5] = TextureAssets.LockOnCursor;
        }
        /// <summary> 火之花 </summary>
        public static Asset<Texture2D>[] Item = new Asset<Texture2D>[5455];
        /*
        /// <summary> 火之花 </summary>
        public static Asset<Texture2D> Item_112;
        /// <summary> 火山 </summary>
        public static Asset<Texture2D> Item_121;
        /// <summary> 永夜刃 </summary>
        public static Asset<Texture2D> Item_273;
        /// <summary> 神圣剑 </summary>
        public static Asset<Texture2D> Item_368;
        /// <summary> 觉醒神圣剑 </summary>
        public static Asset<Texture2D> Item_674;
        /// <summary> 觉醒永夜刃 </summary>
        public static Asset<Texture2D> Item_675;
        /// <summary> 光束剑 </summary>
        public static Asset<Texture2D> Item_723;
        /// <summary> 泰拉刃 </summary>
        public static Asset<Texture2D> Item_757;
        /// <summary> 寒霜花 </summary>
        public static Asset<Texture2D> Item_1264;
        /// <summary> 木剑 </summary>
        public static Asset<Texture2D> Item_24;
        public static Asset<Texture2D> Item_653;
        public static Asset<Texture2D> Item_656;
        public static Asset<Texture2D> Item_659;
        public static Asset<Texture2D> Item_921;
        public static Asset<Texture2D> Item_2517;
        public static Asset<Texture2D> Item_2745;
        public static Asset<Texture2D> Item_5284;*/
        /// <summary> 鼠标 </summary>
        public static Asset<Texture2D>[] Mouse = new Asset<Texture2D>[6];
        public static int MouseTime;
        public static void UItemT(int type)
        {
            TextureAssets.Item[type] = Item[273];
        }
        public static void UnloadItemTextures()
        {
            if (Main.dedServ)
            {
                return;
            }
            TextureAssets.Cursors[0] = DDItemTextures.Mouse[0];
            TextureAssets.Cursors[1] = DDItemTextures.Mouse[1];
            TextureAssets.Cursors[11] = DDItemTextures.Mouse[2];
            TextureAssets.Cursors[12] = DDItemTextures.Mouse[3];
            TextureAssets.CursorRadial = DDItemTextures.Mouse[4];
            TextureAssets.LockOnCursor = DDItemTextures.Mouse[5];
            UItemT(4);
            UItemT(24);
            UItemT(112);
            UItemT(273);
            UItemT(368);
            UItemT(653);
            UItemT(656);
            UItemT(659);
            UItemT(674);
            UItemT(675);
            UItemT(723);
            UItemT(757);
            UItemT(921);
            UItemT(989);
            UItemT(1227);
            UItemT(1264);
            UItemT(2517);
            UItemT(2745);
            UItemT(3484);
            UItemT(3490);
            UItemT(3496);
            UItemT(3502);
            UItemT(3508);
            UItemT(3514);
            UItemT(3520);
            UItemT(5284);
            Item = null;
    }
    }
}
