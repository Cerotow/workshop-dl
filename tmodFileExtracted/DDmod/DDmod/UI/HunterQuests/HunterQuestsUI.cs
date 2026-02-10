using DDmod.Content;
using DDmod.SubworldLibraryWorld;
using DDmod.SubworldLibraryWorld.草原;
using log4net.Repository.Hierarchy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace DDmod.UI.HunterQuests
{

    //绘制面板
    class HunterQuestPanel : UIElement
    {
        public Vector2[] vectors = new Vector2[8];
        public Vector2[] vectorsText = new Vector2[8];
        public bool[] Bool = new bool[8];
        /// <summary>
        /// 关卡数量
        /// </summary>
        public int MaxLevel = 111;
        /// <summary>
        /// 页
        /// </summary>
        public int page = 1;
        /// <summary>
        /// 切换页按钮
        /// </summary>
        public Vector2[] handoffpage = new Vector2[2];
        public bool[] handoffpageBool = new bool[2];
        /// <summary>
        /// 按键
        /// </summary>
        public bool Key = false;
        public int Key2 = 5;
        /// <summary>
        /// 每列显示多少项目
        /// </summary>
        public int column = 6;

        public static Dictionary<float, string> ChallengeWorld = new Dictionary<float, string>();
        public static Dictionary<float, bool> WorldLock = new Dictionary<float, bool>();
        public static Dictionary<float, string> WorldLockText = new Dictionary<float, string>();
        public override void Update(GameTime gameTime)
        {
            column = 6;
            column = (int)(column *((float)Main.screenHeight/ 1080));
            column += 1;
            vectors = new Vector2[column];
            vectorsText = new Vector2[column];
            Bool = new bool[column];
            //设置按钮距离所属ui部件的最左端的距离
            Left.Set(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板").Width() / 2 + 20, 0f);
            //设置按钮距离所属ui部件的最顶端的距离
            Top.Set((Main.screenHeight / 2 + 20), 0f);


            int Width = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板").Width();
            int Height = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板").Height();
            Height *= column + 2;
            Vector2 position = new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y) - new Vector2(Width,Height) / 2;
            if (new Rectangle((int)position.X, (int)position.Y, Width, Height).Intersects(new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1)))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            float U = (float)Main.screenHeight / 1080;
            //关卡选项
            int Choose = -1;
            for (int a = 0; a < column; a++)
            {
                vectors[a] = new Vector2(Width * 0.9f, Height / (column+2) * (a + 1)) + position;
                vectorsText[a] = new Vector2(Width * 0.04f, Height / (column + 2) * (a + 1)) + position;
                Rectangle rectangle = new Rectangle((int)(vectors[a].X - 18), (int)(vectors[a].Y - 24), 36, 48);
                if (rectangle.Intersects(new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1)))
                {
                    Bool[a] = true;
                    Choose = (a % column + (page - 1) * column);
                }
                else
                {
                    Bool[a] = false;
                }
            }
            if (Main.mouseLeft)
            {
                ChallengeSubworld(World(Choose));
            }
            //切换按钮
            handoffpage[0] = new Vector2(57, Height / (column + 2) * (column+1F)) + position;
            handoffpage[1] = new Vector2(Width - 57, Height / (column + 2) * (column+1F)) + position;
            Rectangle handoffRectangle = new Rectangle((int)(handoffpage[0].X - 49), (int)(handoffpage[0].Y - 40), 98, 80);
            Rectangle handoffRectangle2 = new Rectangle((int)(handoffpage[1].X - 49), (int)(handoffpage[1].Y - 40), 98, 80);
            if (handoffRectangle.Intersects(new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1)))
            {
                handoffpageBool[0] = true;
                if (Main.mouseLeft)
                {
                    Key = true;
                }
            }
            else
            {
                handoffpageBool[0] = false;
            }
            if (handoffRectangle2.Intersects(new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1)))
            {
                handoffpageBool[1] = true;
                if (Main.mouseLeft)
                {
                    Key = true;
                }
            }
            else
            {
                handoffpageBool[1] = false;
            }
            int MaxPage = (MaxLevel - 1) / column + 1;
            if (page > MaxPage) page = MaxPage;
            if (page < 1) page = 1;
            if (!Main.mouseLeft && Key)
            {
                if (handoffpageBool[0] && page > 1)
                {
                    page--;
                }
                if (handoffpageBool[1] && page < MaxPage)
                {
                    page++;
                }
                Key = false;
            }
            base.Update(gameTime);
        }
        //重写绘制方法
        public override void Draw(SpriteBatch spriteBatch)
        {
            //if(ChallengeWorld.LongCount()==0)
            {
                Y();
            }
            float Width = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板").Width();
            float Height = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板").Height();
            Height *= column + 2;
            //创建一个局部变量，变量的值为ui左上角坐标
            Vector2 position = new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y);

            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务标题").Value, position-new Vector2(0,30), null, Color.White, 0, new Vector2(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务标题").Width(), Height) / 2,1, 0, 0);
            //关卡选项
            int Choose = -1;
            for (int a = 0; a < vectors.Length; a++)
            {
                Choose = a % column + (page - 1) * column;
                string Text = World(Choose);
                WorldLockText.TryGetValue(Choose, out string LockText);
                spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板").Value, new Vector2(position.X, vectors[a].Y), null, Color.White, 0, new Vector2(Width, Height / (column + 2)) / 2, 1, 0, 0);

                if (Bool[a])
                {
                    spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮").Value, vectors[a], null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮").Size() / 2, 1F, 0, 0);
                    if (Choose >= MaxLevel || Text == Language.GetTextValue("Mods.DDmod.Lock.未解锁"))
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮_Glow").Value, vectors[a], null, new Color(255, 0, 0, 255), 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮_Glow").Size() / 2, 1F, 0, 0);
                    }
                    else
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮_Glow").Value, vectors[a], null, new Color(0, 255, 0, 255), 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮_Glow").Size() / 2, 1F, 0, 0);
                    }
                }
                else
                {
                    spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮").Value, vectors[a], null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮").Size() / 2, 1, 0, 0);
                }
                if (Bool[a])
                {
                    if(Choose>=MaxLevel)
                    {
                        StrokeText(spriteBatch, Language.GetTextValue("Mods.DDmod.DrawText.敬请期待"), new Vector2(Main.mouseX, Main.mouseY) + new Vector2(20), Vector2.Zero, new Color(255, 0, 0, 255), Color.White, 1f, 1);
                    }
                    else
                    if (Text == Language.GetTextValue("Mods.DDmod.Lock.未解锁"))
                    {
                        StrokeText(spriteBatch, LockText, new Vector2(Main.mouseX, Main.mouseY) + new Vector2(20), Vector2.Zero, new Color(255, 0, 0, 255), Color.White, 1f, 1);
                    }
                    else
                    {
                        StrokeText(spriteBatch, Language.GetTextValue("Mods.DDmod.DrawText.进入") + Text, new Vector2(Main.mouseX, Main.mouseY) + new Vector2(20), Vector2.Zero, new Color(0, 255, 0, 255), Color.White, 1f, 1);
                    }
                }
                



                Vector2 Textorigin = ChatManager.GetStringSize(FontAssets.DeathText.Value, Text, Vector2.One, 0) / 4;
                Textorigin.X = 0;
                StrokeText(spriteBatch, Text, vectorsText[a], Textorigin, new Color(0, 0, 0, 255), Color.White, 1f, 1);

                //StrokeText(spriteBatch, "" + page, (handoffpage[0] + handoffpage[1]) / 2, ChatManager.GetStringSize(FontAssets.MouseText.Value, "" + page, Vector2.One, 0),new Color(0,0,0,255),Color.White, 1.5f,1);

            }
            Vector2 origin = ChatManager.GetStringSize(FontAssets.DeathText.Value, Language.GetTextValue("Mods.DDmod.DrawText.选择地图"), Vector2.One, 0) / 2;
            origin.Y -= origin.Y;
            StrokeText(spriteBatch, Language.GetTextValue("Mods.DDmod.DrawText.选择地图"), position - new Vector2(0, Height / 2+10), origin, new Color(0, 0, 0, 255), Color.White, 2,1);
            //箭头绘制
            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务表切换左").Value, handoffpage[0], null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务表切换左").Size() / 2, 1, 0, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务表切换右").Value, handoffpage[1], null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务表切换右").Size() / 2, 1, 0, 0);
            //if (page != 1)
            {
                //spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/箭头").Value, handoffpage[0], null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/箭头").Size() / 2, Scale, 0, 0);
            }
            //if (page < MaxPage)
            {
                //spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/箭头2").Value, handoffpage[1], null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/箭头2").Size() / 2, Scale2, 0, 0);
            }
            base.Draw(spriteBatch);
        }
        public void StrokeText(SpriteBatch spriteBatch,string Text,Vector2 position,Vector2 origin,Color color1,Color color2,float Scale=1, int Thick=1)
        {
            for (int y = -Thick; y <= Thick; y++)
            {
                for (int x = -Thick; x <= Thick; x++)
                {
                    DynamicSpriteFontExtensionMethods.DrawString(
                        spriteBatch,
                        FontAssets.DeathText.Value,
                        Text,
                        position+new Vector2(x,y),
                        color1, 0f,
                        origin,
                        Scale*0.4F, SpriteEffects.None, 0f);
                }
            }
            DynamicSpriteFontExtensionMethods.DrawString(
                spriteBatch,
                FontAssets.DeathText.Value,
                Text,
                position,
                color2, 0f,
                origin,
                Scale*0.4F, SpriteEffects.None, 0f);
        }
        public string World(int P)
        {
            ChallengeWorld.TryGetValue(P,out string WorldText);
            WorldLock.TryGetValue(P,out bool Lock);

            if (WorldText != null&& Lock)
            {
                return WorldText;
            }
            return Language.GetTextValue("Mods.DDmod.Lock.未解锁");
        }
        public void ChallengeSubworld(string Text)
        {
            if (Text != Language.GetTextValue("Mods.DDmod.Lock.未解锁"))
            {
                Main.LocalPlayer.Dplayer().PreLocation = Main.LocalPlayer.position;
                if (Text == Language.GetTextValue("Mods.DDmod.Level.草原"))
                {
                    if (!SubworldSystem.IsActive<Grassland>())
                    {
                        SubworldSystem.Enter<Grassland>();
                    }
                }
                if (Text == Language.GetTextValue("Mods.DDmod.Level.草原2"))
                {
                    if (!SubworldSystem.IsActive<Grassland2>())
                    {
                        SubworldSystem.Enter<Grassland2>();
                    }
                }
                if (Text == Language.GetTextValue("Mods.DDmod.Level.墓地"))
                {
                    if (!SubworldSystem.IsActive<Graveyard>())
                    {
                        SubworldSystem.Enter<Graveyard>();
                    }
                }
                if (Text == Language.GetTextValue("Mods.DDmod.Level.墓地2"))
                {
                    if (!SubworldSystem.IsActive<Graveyard2>())
                    {
                        SubworldSystem.Enter<Graveyard2>();
                    }
                }
                if (Text == Language.GetTextValue("Mods.DDmod.Level.草原3"))
                {
                    if (!SubworldSystem.IsActive<Grassland3>())
                    {
                        SubworldSystem.Enter<Grassland3>();
                    }
                }
                if (Text == Language.GetTextValue("Mods.DDmod.Level.草原4"))
                {
                    if (!SubworldSystem.IsActive<Grassland4>())
                    {
                        SubworldSystem.Enter<Grassland4>();
                    }
                }
            }
        }
        public void Y()
        {
            int Level = 0;

            AddWorld(Language.GetTextValue("Mods.DDmod.Level.草原"), ref Level, true, "");
            AddWorld(Language.GetTextValue("Mods.DDmod.Level.草原2"), ref Level, GrasslandSystem.GrasslandFinish, Language.GetTextValue("Mods.DDmod.Lock.草原2"));
            AddWorld(Language.GetTextValue("Mods.DDmod.Level.墓地"), ref Level, GraveyardSystem.GraveyardExplore, Language.GetTextValue("Mods.DDmod.Lock.墓地"));
            AddWorld(Language.GetTextValue("Mods.DDmod.Level.草原3"), ref Level, Grassland2System.Grassland2Finish, Language.GetTextValue("Mods.DDmod.Lock.草原3"));
            AddWorld(Language.GetTextValue("Mods.DDmod.Level.草原4"), ref Level, Grassland3System.Grassland3Finish, Language.GetTextValue("Mods.DDmod.Lock.草原4"));

            //AddWorld(Language.GetTextValue("Mods.DDmod.Level.墓地2"), ref Level, GraveyardSystem.GraveyardFinish, Language.GetTextValue("Mods.DDmod.Lock.墓地2"));

            MaxLevel = Level;
        }
        public void AddWorld(string name, ref int Level, bool Lock, string LockText)
        {
            //添加世界
            if (!ChallengeWorld.ContainsKey(Level))
            {
                ChallengeWorld.Add(Level, name);
            }
            //添加世界锁
            if (!WorldLock.ContainsKey(Level))
            {
                WorldLock.Add(Level, Lock);
                WorldLockText.Add(Level, LockText);
            }
            //检测世界锁
            WorldLock.TryGetValue(Level, out bool LockB);
            //如果世界锁出现变动重新生成
            if ((!LockB && Lock) || (LockB && !Lock))
            {
                WorldLock.Remove(Level);
                WorldLock.Add(Level, Lock);
            }
            Level++;
        }
    }
    internal class HunterQuestsUI : UIState
    {
        public static bool Visible = false;
        public static Vector2 Location;
        UIImageButton button;
        HunterQuestPanel UI;
        private UIText text;
        public static int Synthesis;
        public override void OnInitialize()
        {
            text = new UIText("0/0", 0.8f);
            text.Width.Set(50, 0f);
            text.Height.Set(50, 0f);
            text.TextColor = new Color(158, 242, 255);

            //用tr原版图片实例化一个图片按钮
            UI = new HunterQuestPanel();
            //设置按钮距宽度
            UI.Width.Set(400f, 0f);
            //设置按钮高度
            UI.Height.Set(800f, 0f);
            //设置按钮距离所属ui部件的最左端的距离
            UI.Left.Set(Main.screenWidth / 8, 0f);
            //设置按钮距离所属ui部件的最顶端的距离
            UI.Top.Set((Main.screenHeight / 2 + 20), 0f);
            /*
            //用tr原版图片实例化一个图片按钮
            button = new UIImageButton(ModContent.Request<Texture2D>("DDmod/UI/HunterQuestsUI/进入按钮"));
            //设置按钮距宽度
            button.Width.Set(18f, 0f);
            //设置按钮高度
            button.Height.Set(18f, 0f);
            float Width = ModContent.Request<Texture2D>("DDmod/UI/HunterQuestsUI/猎人任务面板").Width();
            float Height = ModContent.Request<Texture2D>("DDmod/UI/HunterQuestsUI/猎人任务面板").Height();
            //设置按钮距离所属ui部件的最左端的距离
            button.Left.Set(Width * 0.9F - Width / 2, 0f);
            //设置按钮距离所属ui部件的最顶端的距离
            button.Top.Set(Height * 0.1F - Height / 2, 0f);
            //注册一个事件，这个事件将会在按钮按下时被激活
            button.OnClick += CloseButton_OnClick;
            //将按钮注册入面板中，这个按钮的坐标将以面板的坐标为基础计算
            */
            Append(UI);
            //UI.Append(button);
            base.OnInitialize();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //调用SetValue方法更新进度条的值
            //bar.DrawAdvBox(spriteBatch,400,400, ModContent.GetTexture("PVZ/NPCs/金剑").Width, ModContent.GetTexture("PVZ/NPCs/金剑").Height, Color.White, ModContent.GetTexture("PVZ/NPCs/金剑"),new Vector2(0));
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.talkNPC == -1 || Main.LocalPlayer.talkNPC != Synthesis)
            {
                Visible = false;
            }
            UI.Update(gameTime);
            /*button.Append(text);
            text.SetText("前往墓地副本");
            text.Height.Set(50, 0f);
            text.Top.Set(0, 0f);
            text.Left.Set(-340, 0f);*/

            Player player = Main.player[Main.myPlayer];
        }
    }
}
