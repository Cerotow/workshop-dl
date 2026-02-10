namespace DDmod.Textures
{
    public class DDProjTextures
    {
        public static void LoadProjTextures()
        {
            if (Main.dedServ)
            {
                return;
            }
          //  泰拉Logo = TextureAssets.Logo; 
         //   TextureAssets.Logo = ModContent.Request<Texture2D>("DDmod/Textures/泰拉Logo");
          //  泰拉Logo2 = TextureAssets.Logo2; 
            //TextureAssets.Logo2 = ModContent.Request<Texture2D>("DDmod/Textures/泰拉Logo");

          //  TextureAssets.Heart = ModContent.Request<Texture2D>("DDmod/Textures/红心");
          //  TextureAssets.Heart2 = ModContent.Request<Texture2D>("DDmod/Textures/金心");
           //TextureAssets.Mana = ModContent.Request<Texture2D>("DDmod/Textures/魔力星");
            //TextureAssets.ArmorHead[0] = ModContent.Request<Texture2D>("DDmod/Textures/红心");
           // TextureAssets.ArmorHead[1] = ModContent.Request<Texture2D>("DDmod/Textures/金心");
            //TextureAssets.Star[0] = ModContent.Request<Texture2D>("DDmod/Textures/魔力星");

            Proj_20 = TextureAssets.Projectile[20];
            TextureAssets.Projectile[20] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_20");

            Proj_44 = TextureAssets.Projectile[44];
            TextureAssets.Projectile[44] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_44");

            Proj_45 = TextureAssets.Projectile[45];
            TextureAssets.Projectile[45] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_45");

            Proj_83 = TextureAssets.Projectile[83];
            TextureAssets.Projectile[83] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_83");

            Proj_84 = TextureAssets.Projectile[84];
            TextureAssets.Projectile[84] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_84");

            Proj_88 = TextureAssets.Projectile[88];
            TextureAssets.Projectile[88] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_88");

            Proj_100 = TextureAssets.Projectile[100];
            TextureAssets.Projectile[100] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_100");

            Proj_257 = TextureAssets.Projectile[257];
            TextureAssets.Projectile[257] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_257");

            Proj_389 = TextureAssets.Projectile[389];
            TextureAssets.Projectile[389] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_389");

            Proj_389 = TextureAssets.Projectile[389];
            TextureAssets.Projectile[389] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_389");
            Proj_173 = TextureAssets.Projectile[173];
            TextureAssets.Projectile[173] = ModContent.Request<Texture2D>("DDmod/Textures/Proj/Proj_173");


            NPC_134 = TextureAssets.Npc[134];
            //TextureAssets.Npc[134] = ModContent.Request<Texture2D>("DDmod/Textures/NPC/NPC_134");
            NPC_135 = TextureAssets.Npc[135];
            //TextureAssets.Npc[135] = ModContent.Request<Texture2D>("DDmod/Textures/NPC/NPC_135");
            NPC_136 = TextureAssets.Npc[136];
            //TextureAssets.Npc[136] = ModContent.Request<Texture2D>("DDmod/Textures/NPC/NPC_136");

            Dest_1 = TextureAssets.Dest[0];
            //TextureAssets.Dest[0] = ModContent.Request<Texture2D>("DDmod/Textures/NPC/Dest1");
            Dest_2 = TextureAssets.Dest[1];
            //TextureAssets.Dest[1] = ModContent.Request<Texture2D>("DDmod/Textures/NPC/Dest2");
            Dest_3 = TextureAssets.Dest[2];
            //TextureAssets.Dest[2] = ModContent.Request<Texture2D>("DDmod/Textures/NPC/Dest3");
            Gore_156 = TextureAssets.Gore[156];
            //TextureAssets.Gore[156] = ModContent.Request<Texture2D>("DDmod/Textures/NPC/Gore_156");
        }
        /// <summary> 毁灭者 </summary>
        public static Asset<Texture2D> NPC_134;
        public static Asset<Texture2D> NPC_135;
        public static Asset<Texture2D> NPC_136;
        public static Asset<Texture2D> Dest_1;
        public static Asset<Texture2D> Dest_2;
        public static Asset<Texture2D> Dest_3;
        public static Asset<Texture2D> Gore_156;
        /// <summary> 泰拉Logo </summary>
        public static Asset<Texture2D> 泰拉Logo;
        public static Asset<Texture2D> 泰拉Logo2;
        /// <summary> 绿激光 </summary>
        public static Asset<Texture2D> Proj_20;
        /// <summary> 敌对恶魔锄刀 </summary>
        public static Asset<Texture2D> Proj_44;
        /// <summary> 友好恶魔锄刀 </summary>
        public static Asset<Texture2D> Proj_45;
        /// <summary> 肉山激光 </summary>
        public static Asset<Texture2D> Proj_83;
        /// <summary> 探针激光 </summary>
        public static Asset<Texture2D> Proj_84;
        /// <summary> 激光步枪 </summary>
        public static Asset<Texture2D> Proj_88;
        /// <summary> 机械激光 </summary>
        public static Asset<Texture2D> Proj_100;
        /// <summary> 冰巨人激光 </summary>
        public static Asset<Texture2D> Proj_257;
        /// <summary> 小激光眼激光 </summary>
        public static Asset<Texture2D> Proj_389;
        /// <summary> 附魔剑 </summary>
        public static Asset<Texture2D> Proj_173;
        public static void UnloadProjTextures()
        {
            if (Main.dedServ)
            {
                return;
            }
           // TextureAssets.Logo = 泰拉Logo;
            //TextureAssets.Logo2 = 泰拉Logo2;
           // 泰拉Logo = null;
            TextureAssets.Projectile[20] = Proj_20;
            Proj_20 = null;
            TextureAssets.Projectile[83] = Proj_83;
            Proj_83 = null;
            TextureAssets.Projectile[84] = Proj_84;
            Proj_84 = null;
            TextureAssets.Projectile[88] = Proj_88;
            Proj_88 = null;
            TextureAssets.Projectile[100] = Proj_100;
            Proj_100 = null;
            TextureAssets.Projectile[257] = Proj_257;
            Proj_257 = null;
            TextureAssets.Projectile[389] = Proj_389;
            Proj_389 = null;
            TextureAssets.Projectile[173] = Proj_173;
            Proj_173 = null;

            TextureAssets.Npc[134] = NPC_134;
            NPC_134 = null;
            TextureAssets.Npc[135] = NPC_135;
            NPC_135 = null;
            TextureAssets.Npc[136] = NPC_136;
            NPC_136 = null;

            TextureAssets.Dest[0] = Dest_1;
            Dest_1 = null;
            TextureAssets.Dest[1] = Dest_2;
            Dest_2 = null;
            TextureAssets.Dest[2] = Dest_3;
            Dest_3 = null;
            TextureAssets.Gore[156] = Gore_156;
            Gore_156 = null;
        }
    }
}
