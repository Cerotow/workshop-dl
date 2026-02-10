namespace DDmod.Textures
{
    public class DDTextures
    {
        /// <summary> 爆炸 </summary>
        public static Asset<Texture2D> 爆炸;
        /// <summary> 护盾 </summary>
        public static Asset<Texture2D> 护盾;
        public static Asset<Texture2D> 护盾2;
        public static Asset<Texture2D> 护盾3;
        /// <summary> 魔法球 </summary>
        public static Asset<Texture2D> MagicBall;
        /// <summary> 光特效 </summary>
        public static Asset<Texture2D> LightEffect;
        /// <summary> 光柱 </summary>
        public static Asset<Texture2D> GlowTrail;
        public static Asset<Texture2D> GlowTrail2;
        /// <summary> 能量盾光柱 </summary>
        public static Asset<Texture2D> EnergyShieldLight;
        /// <summary> 小圆形光效 </summary>
        public static Asset<Texture2D> MiniVoidStar;
        /// <summary> 特效光 </summary>
        public static Asset<Texture2D> VoidLight;
        /// <summary> 大圆形光效 </summary>
        public static Asset<Texture2D> VoidStar;
        /// <summary> 大圆形光效 </summary>
        public static Asset<Texture2D> VoidStarPure;
        /// <summary> 限制框 </summary>
        public static Asset<Texture2D> 限制框;
        /// <summary> 四角星光效 </summary>
        public static Asset<Texture2D> Starlight;
        public static Asset<Texture2D> Starlight2;
        public static Asset<Texture2D> Starlight3;
        /// <summary> 扫描 </summary>
        public static Asset<Texture2D> Scanning;
        public static Asset<Texture2D> Scanning2;
        public static Asset<Texture2D> Scanning3;
        /// <summary> 线 </summary>
        public static Asset<Texture2D> Wire;
        /// <summary> 空白贴图 </summary>
        public static Asset<Texture2D> Nullpng;
        /// <summary> 白贴图 </summary>
        public static Asset<Texture2D> WhitePng;
        public static Asset<Texture2D> WhitePng2;
        /// <summary> 法阵 </summary>
        public static Asset<Texture2D>[] Circle = new Asset<Texture2D>[13];
        /// <summary> 弓 </summary>
        public static Asset<Texture2D>[] Bow = new Asset<Texture2D>[5452];
        /// <summary> 弓光效 </summary>
        public static Asset<Texture2D>[] BowGlow = new Asset<Texture2D>[5452];
        public static Asset<Texture2D> BowE2624;
        public static Asset<Texture2D> BowE3859;
        /// <summary> 圆 </summary>
        public static Asset<Texture2D> Round;
        public static Asset<Texture2D> Round2;
        public static Asset<Texture2D> Round3;
        /// <summary> 圆形进度条 </summary>
        public static Asset<Texture2D> CircularProgressBar;
        /// <summary> 子弹 </summary>
        public static Asset<Texture2D> Bullet;
        /// <summary> 柏林 </summary>
        public static Asset<Texture2D> Perlin;
        public static Asset<Texture2D> Perlin2;
        /// <summary> 远古背景 </summary>
        public static Asset<Texture2D> 远古背景;
        public static Asset<Texture2D> 远古背景2;
        /// <summary> 上一页箭头 </summary>
        public static Asset<Texture2D> Nav_Prev;
        /// <summary> 护盾条 </summary>
        public static Asset<Texture2D> Shield;
        /// <summary> 护盾值 </summary>
        public static Asset<Texture2D> ShieldValue;
        /// <summary> 波浪特效 </summary>
        public static Asset<Texture2D> Wave;
        /// <summary> 弓蓄力 </summary>
        public static Asset<Texture2D> Bow蓄力;
        /// <summary> 化石 </summary>
        public static Asset<Texture2D> 化石;
        /// <summary> 冻结 </summary>
        public static Asset<Texture2D> 冻结;
        public static Asset<Texture2D> 冻结_Glow;
        /// <summary> 火焰效果 </summary>
        public static Asset<Texture2D> FireEffect;
        public static Asset<Texture2D> FireEffect2;
        /// <summary> 闪电拖尾 </summary>
        public static Asset<Texture2D> LightningTrailing;
        /// <summary> 光芒效果 </summary>
        public static Asset<Texture2D> GlowEffect;
        /// <summary> 彩虹效果 </summary>
        public static Asset<Texture2D> 彩虹;
        /// <summary> Wifi </summary>
        public static Asset<Texture2D> Wifi;
        /// <summary> 聊天泡泡口 </summary>
        public static Asset<Texture2D> TextBubblesMouth;
        /// <summary> 聊天泡泡 </summary>
        public static Asset<Texture2D> TextBubbles;
        /// <summary> 光晕 </summary>
        public static Asset<Texture2D> 光晕;
        public static Asset<Texture2D> 光晕2;
        /// <summary> 枪口特效 </summary>
        public static Asset<Texture2D> GunFlames;
        /// <summary> 背景 </summary>
        public static Asset<Texture2D> 背景;
        /// <summary> 收获 </summary>
        public static Asset<Texture2D> 收获;
        public static Asset<Texture2D> 收获边框;
        /// <summary> 光剑 </summary>
        public static Asset<Texture2D> Lightsaber;
        public static void LoadTextures()
        {
            if (Main.dedServ)
            {
                return;
            }
            Bow = new Asset<Texture2D>[ItemLoader.ItemCount];
            BowGlow = new Asset<Texture2D>[ItemLoader.ItemCount];
            LightEffect = ModContent.Request<Texture2D>("DDmod/Image/LightEffect");
            MagicBall = ModContent.Request<Texture2D>("DDmod/Image/MagicBall");
            GlowTrail = ModContent.Request<Texture2D>("DDmod/Image/GlowTrail");
            GlowTrail2 = ModContent.Request<Texture2D>("DDmod/Image/GlowTrail2");
            EnergyShieldLight = ModContent.Request<Texture2D>("DDmod/Image/EnergyShieldLight");
            MiniVoidStar = ModContent.Request<Texture2D>("DDmod/Image/MiniVoidStar");
            远古背景 = ModContent.Request<Texture2D>("DDmod/Image/远古背景");
            远古背景2 = ModContent.Request<Texture2D>("DDmod/Image/远古背景2");
            VoidLight = ModContent.Request<Texture2D>("DDmod/Image/VoidLight");
            VoidStar = ModContent.Request<Texture2D>("DDmod/Image/VoidStar");
            VoidStarPure = ModContent.Request<Texture2D>("DDmod/Image/VoidStarPure");
            限制框 = ModContent.Request<Texture2D>("DDmod/Image/限制框");
            Starlight = ModContent.Request<Texture2D>("DDmod/Image/Starlight");
            Starlight2 = ModContent.Request<Texture2D>("DDmod/Image/Starlight2");
            Starlight3 = ModContent.Request<Texture2D>("DDmod/Image/Starlight3");
            Scanning = ModContent.Request<Texture2D>("DDmod/Image/Scanning");
            Scanning2 = ModContent.Request<Texture2D>("DDmod/Image/Scanning2");
            Scanning3 = ModContent.Request<Texture2D>("DDmod/Image/Scanning3");
            Wire = ModContent.Request<Texture2D>("DDmod/Image/Wire");
            Nullpng = ModContent.Request<Texture2D>("DDmod/Image/Nullpng");
            WhitePng = ModContent.Request<Texture2D>("DDmod/Image/WhitePng");
            WhitePng2 = ModContent.Request<Texture2D>("DDmod/Image/WhitePng2");
            Shield = ModContent.Request<Texture2D>("DDmod/Image/Shield");
            ShieldValue = ModContent.Request<Texture2D>("DDmod/Image/ShieldValue");
            FireEffect = ModContent.Request<Texture2D>("DDmod/Image/FireEffect");
            FireEffect2 = ModContent.Request<Texture2D>("DDmod/Image/FireEffect2");
            LightningTrailing = ModContent.Request<Texture2D>("DDmod/Image/LightningTrailing");
            GlowEffect = ModContent.Request<Texture2D>("DDmod/Image/GlowEffect");
            for (int a = 0; a < Circle.Length; a++)
            {
                if (a == 0 || a == 1)
                {

                    Circle[a] = ModContent.Request<Texture2D>("DDmod/Image/Circle");
                }
                else
                {
                    Circle[a] = ModContent.Request<Texture2D>("DDmod/Image/Circle" + a);
                }
            }
            Round = ModContent.Request<Texture2D>("DDmod/Image/Round");
            Round2 = ModContent.Request<Texture2D>("DDmod/Image/Round2");
            Round3 = ModContent.Request<Texture2D>("DDmod/Image/Round3");
            CircularProgressBar = ModContent.Request<Texture2D>("DDmod/Image/CircularProgressBar");
            Bullet = ModContent.Request<Texture2D>("DDmod/Image/Bullet");
            Perlin = ModContent.Request<Texture2D>("DDmod/Image/Perlin");
            Perlin2 = ModContent.Request<Texture2D>("DDmod/Image/Perlin2");
            Nav_Prev =  ModContent.Request<Texture2D>("DDmod/ModLinkage/BossChecklist/Nav_Prev");
            Wave = ModContent.Request<Texture2D>("DDmod/Image/Wave");
            化石 = ModContent.Request<Texture2D>("DDmod/Image/化石");
            冻结 = ModContent.Request<Texture2D>("DDmod/Image/冻结");
            冻结_Glow = ModContent.Request<Texture2D>("DDmod/Image/冻结_Glow");
            Bow蓄力 = ModContent.Request<Texture2D>("DDmod/Image/Bow蓄力");
            彩虹 = ModContent.Request<Texture2D>("DDmod/Image/彩虹");
            Wifi = ModContent.Request<Texture2D>("DDmod/Image/Wifi");
            Bow[39] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_39");
            Bow[44] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_44");
            BowGlow[44] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_44_Glow");
            Bow[99] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_99");
            Bow[120] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_120");
            BowGlow[120] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_120_Glow");

            Bow[655] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_655");
            Bow[658] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_658");
            Bow[661] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_661");
            Bow[682] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_682");
            Bow[725] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_725");
            Bow[796] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_796");
            BowGlow[796] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_796_Glow");
            Bow[923] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_923");
            Bow[2223] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_2223");
            Bow[2515] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_2515");
            Bow[2624] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_2624");
            BowE2624 = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_2624_E");
            Bow[2747] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_2747");
            Bow[2888] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_2888");
            Bow[3019] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3019");
            Bow[3029] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3029");
            Bow[3052] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3052");
            Bow[3480] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3480");
            Bow[3486] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3486");
            Bow[3492] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3492");
            Bow[3498] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3498");
            Bow[3504] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3504");
            Bow[3510] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3510");
            Bow[3516] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3516");
            Bow[3540] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3540");
            BowGlow[3540] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3540_Glow");
            Bow[3854] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3854");
            Bow[3859] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3859");
            BowE3859 = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_3859_E");
            Bow[4381] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_4381");
            Bow[4953] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_4953");
            Bow[5282] = ModContent.Request<Texture2D>("DDmod/Textures/Bow/Item_5282");
            TextBubblesMouth = ModContent.Request<Texture2D>("DDmod/Textures/TextBubbles/默认气泡口");
            TextBubbles = ModContent.Request<Texture2D>("DDmod/Textures/TextBubbles/默认气泡");
            光晕 = ModContent.Request<Texture2D>("DDmod/Image/光晕");
            光晕2 = ModContent.Request<Texture2D>("DDmod/Image/光晕2");
            GunFlames = ModContent.Request<Texture2D>("DDmod/Image/枪口");
            收获 = ModContent.Request<Texture2D>("DDmod/Image/收获");
            收获边框 = ModContent.Request<Texture2D>("DDmod/Image/收获边框");
            爆炸 = ModContent.Request<Texture2D>("DDmod/Image/爆炸");
            护盾 = ModContent.Request<Texture2D>("DDmod/Image/护盾");
            护盾2 = ModContent.Request<Texture2D>("DDmod/Image/护盾2");
            护盾3 = ModContent.Request<Texture2D>("DDmod/Image/护盾3");
            背景 = ModContent.Request<Texture2D>("DDmod/Image/背景");
            Lightsaber = ModContent.Request<Texture2D>("DDmod/Image/Lightsaber");

        }
        public static Asset<Texture2D> Items_65;
        public static void UnloadTextures()
        {
            if (Main.dedServ)
            {
                return;
            }
            MagicBall = null;
            LightEffect = null;
            GlowTrail = null;
            GlowTrail2 = null;
            EnergyShieldLight = null;
            MiniVoidStar = null;
            VoidStar = null;
            VoidLight = null;
            VoidStarPure = null;
            限制框 = null;
            Starlight = null;
            Starlight2 = null;
            Starlight3 = null;
            Scanning = null;
            Scanning2 = null;
            Scanning3 = null;
            Wire = null;
            Nullpng = null;
            WhitePng = null;
            WhitePng2 = null;
            Shield = null;
            ShieldValue = null;
            FireEffect = null;
            FireEffect2 = null;
            LightningTrailing = null;
            GlowEffect = null;
            for (int a = 0; a < Circle.Length; a++)
            {
                Circle[a] = null;
            }
            Round = null;
            Round2 = null;
            Round3 = null;
            CircularProgressBar = null;
            Bullet = null;
            Perlin = null;
            Perlin2 = null;
            远古背景 = null;
            远古背景2 = null;
            Nav_Prev = null;
            Wave = null;
            化石 = null;
            冻结 = null;
            冻结_Glow = null;
            Bow蓄力 = null;
            Bow = null;
            BowGlow = null;
            彩虹 = null;
            Wifi = null;
            TextBubblesMouth = null;
            TextBubbles = null;
            光晕 = null;
            光晕2 = null;
            GunFlames = null;
            收获 = null;
            收获边框 = null;
            爆炸 = null;
            护盾 = null;
            护盾2 = null;
            护盾3 = null;
            背景 = null;
            BowE2624 = null;
            BowE3859 = null;
            Lightsaber = null;
        }
    }
}
