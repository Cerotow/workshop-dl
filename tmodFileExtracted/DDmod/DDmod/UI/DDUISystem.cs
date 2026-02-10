using DDmod.Content.Items.Boss.蘑菇王;
using DDmod.Content.Items.Boss.夜光蘑菇王;
using DDmod.Content.Items.Ranged.Make;
using DDmod.Content.Items.Sundries;
using DDmod.Modkey;
using DDmod.Players;
using DDmod.UI.HunterQuests;
using DDmod.UI.ItemUI;
using DDmod.UI.ItemUI.背包;
using Terraria;
using Terraria.Chat;
using Terraria.UI;
using static System.Net.Mime.MediaTypeNames;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using DDmod.Content.NPCs.TownNPC;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using DDmod.Content.Items.Melee.Sword;
using System.Text;
using DDmod.UI.BattlePetUI.技能;
using Terraria.ModLoader.UI;
using DDmod.Content.Items.Boss.MeteorDiggerItems;
using DDmod.Content.Items.Series.绿岩;
using DDmod.NoContent.Config;
using DDmod.Content.Tiles;
using Microsoft.Xna.Framework;
using DDmod.UI.LevelUI;
using DDmod.Worlds;
using DDmod.Content.Items.Boss.先祖咒魂;
using DDmod.Content.Achievements;

namespace DDmod.UI
{

	public class DDUISystem : ModSystem
	{
        public static DDUISystem Instance;

		public static bool BossSurvival;
		internal 绿岩测验机UI GreenstoneUI;
		internal 等级系统UI LevelUI;
		//抽奖UI
		internal 抽奖UI.抽奖UI 抽奖UI;
		//建立一个类型为UserInterface的变量
		internal UserInterface 抽奖Interface;
		//背包UI
		internal BackpackStrengtheningUI BackpackUI;
		//建立一个类型为UserInterface的变量
		internal UserInterface BackpackInterface;

		//法宝强化UI
		internal StrengtheningUI TalismanUI;
		//建立一个类型为UserInterface的变量
		internal UserInterface TalismanInterface;

		//猎手任务UI
		internal HunterQuestsUI HunterUI;
		//建立一个类型为UserInterface的变量
		internal UserInterface HunterInterface;
		//猎手任务UI
		internal EntrustUI entrustUI;
		//建立一个类型为UserInterface的变量
		internal UserInterface EntrustInterface;
		//猎手任务UI
		internal EntrustLevelUI entrustLevelUI;
		//建立一个类型为UserInterface的变量
		internal UserInterface EntrustLevelInterface;

		//猎手任务UI
		internal EntrustTextUI EntrustText;
		//建立一个类型为UserInterface的变量
		internal UserInterface EntrustTextInterface;
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            if (绿岩测验机UI.Visible)
            {
                GreenstoneUI.Draw(Main.spriteBatch);
            }
            Main.spriteBatch.End();

            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1)
			{
				if (BackpackStrengtheningUI.Visible)
				{
					layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				   //这里是绘制层的名字
				   "DDmod:背包",
				   //这里是匿名方法
				   delegate
				   {
					   //绘制UI（运行饱食度UI的Draw方法）
					   BackpackInterface.Draw(Main.spriteBatch, new GameTime());
					   return true;
				   },
				   //这里是绘制层的类型
				   InterfaceScaleType.UI));
				}
				if (UI.抽奖UI.抽奖UI.Visible)
				{
					layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				   //这里是绘制层的名字
				   "DDmod:抽奖",
				   //这里是匿名方法
				   delegate
				   {
					   //绘制UI（运行饱食度UI的Draw方法）
					   抽奖Interface.Draw(Main.spriteBatch, new GameTime());
					   return true;
				   },
				   //这里是绘制层的类型
				   InterfaceScaleType.UI));
				}
				if (StrengtheningUI.Visible)
				{
					layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				   //这里是绘制层的名字
				   "DDmod:法宝强化",
				   //这里是匿名方法
				   delegate
				   {
					   //绘制UI（运行饱食度UI的Draw方法）
					   TalismanInterface.Draw(Main.spriteBatch, new GameTime());
					   return true;
				   },
				   //这里是绘制层的类型
				   InterfaceScaleType.UI));
				}
				if (HunterQuestsUI.Visible)
				{
					layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				   //这里是绘制层的名字
				   "DDmod:猎手任务",
				   //这里是匿名方法
				   delegate
				   {
					   //绘制UI（运行饱食度UI的Draw方法）
					   HunterInterface.Draw(Main.spriteBatch, new GameTime());
					   return true;
				   },
				   //这里是绘制层的类型
				   InterfaceScaleType.UI));
				}
				if (EntrustUI.Visible)
				{
					layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				   //这里是绘制层的名字
				   "DDmod:猎手任务奖励",
				   //这里是匿名方法
				   delegate
				   {
					   //绘制UI（运行饱食度UI的Draw方法）
					   EntrustInterface.Draw(Main.spriteBatch, new GameTime());
					   return true;
				   },
				   //这里是绘制层的类型
				   InterfaceScaleType.UI));
				}
				if (EntrustLevelUI.Visible)
				{
					layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				   //这里是绘制层的名字
				   "DDmod:猎手等级奖励",
				   //这里是匿名方法
				   delegate
				   {
					   //绘制UI（运行饱食度UI的Draw方法）
					   EntrustLevelInterface.Draw(Main.spriteBatch, new GameTime());
					   return true;
				   },
				   //这里是绘制层的类型
				   InterfaceScaleType.UI));
				}
				if (EntrustTextUI.Visible)
				{
					layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
				   //这里是绘制层的名字
				   "DDmod:日常任务",
				   //这里是匿名方法
				   delegate
				   {
					   //绘制UI（运行饱食度UI的Draw方法）
					   EntrustTextInterface.Draw(Main.spriteBatch, new GameTime());
					   return true;
				   },
				   //这里是绘制层的类型
				   InterfaceScaleType.UI));
				}
			}
		}

		public override void UpdateUI(GameTime gameTime)
        {
            if (绿岩测验机UI.Visible)
            {
                GreenstoneUI.Update(gameTime);
            }
            if (BackpackStrengtheningUI.Visible)
			{
				BackpackInterface?.Update(gameTime);
			}
			if (UI.抽奖UI.抽奖UI.Visible)
			{
				抽奖Interface?.Update(gameTime);
			}
			if (StrengtheningUI.Visible)
			{
				TalismanInterface?.Update(gameTime);
			}
			if (HunterQuestsUI.Visible)
			{
				HunterInterface?.Update(gameTime);
			}
			if (EntrustUI.Visible)
			{
				EntrustInterface?.Update(gameTime);
			}
			if (EntrustLevelUI.Visible)
			{
				EntrustLevelInterface?.Update(gameTime);
			}
			if (EntrustTextUI.Visible)
			{
				EntrustTextInterface?.Update(gameTime);
            }
            if (!Main.dedServ)
                LevelUI.Update(gameTime);
        }
		public override void Load()
		{
			Instance = this;
			if (!Main.dedServ)
            {
                GreenstoneUI = new 绿岩测验机UI();
                LevelUI = new 等级系统UI();

                //将法宝强化UI实例化
                BackpackUI = new BackpackStrengtheningUI();
				//将法宝强化UI初始化
				BackpackUI.Activate();
				//将UserInterface实例化
				BackpackInterface = new UserInterface();
				//让UserInterface代理法宝强化UI的事件触发
				BackpackInterface.SetState(BackpackUI);

				//将法宝强化UI实例化
				抽奖UI = new UI.抽奖UI.抽奖UI();
				//将法宝强化UI初始化
				抽奖UI.Activate();
				//将UserInterface实例化
				抽奖Interface = new UserInterface();
				//让UserInterface代理法宝强化UI的事件触发
				抽奖Interface.SetState(抽奖UI);

				//将法宝强化UI实例化
				TalismanUI = new StrengtheningUI();
				//将法宝强化UI初始化
				TalismanUI.Activate();
				//将UserInterface实例化
				TalismanInterface = new UserInterface();
				//让UserInterface代理法宝强化UI的事件触发
				TalismanInterface.SetState(TalismanUI);


				//猎人任务UI
				HunterUI = new HunterQuestsUI();
				//初始化
				HunterUI.Activate();
				//实例化
				HunterInterface = new UserInterface();
				//让UserInterface代理UI的事件触发
				HunterInterface.SetState(HunterUI);

				//猎人任务UI
				entrustUI = new EntrustUI();
				//初始化
				entrustUI.Activate();
				//实例化
				EntrustInterface = new UserInterface();
				//让UserInterface代理UI的事件触发
				EntrustInterface.SetState(entrustUI);

				//猎人任务UI
				entrustLevelUI = new EntrustLevelUI();
				//初始化
				entrustLevelUI.Activate();
				//实例化
				EntrustLevelInterface = new UserInterface();
				//让UserInterface代理UI的事件触发
				EntrustLevelInterface.SetState(entrustLevelUI);

				//猎人任务UI
				EntrustText = new EntrustTextUI();
				//初始化
				EntrustText.Activate();
				//实例化
				EntrustTextInterface = new UserInterface();
				//让UserInterface代理UI的事件触发
				EntrustTextInterface.SetState(EntrustText);


            }
		}
		public override void Unload()
		{
        }
        bool R = false;
        public static int TR = 0;

        string displayText = "";
        private KeyboardState _previousKeyboardState;
        private MouseState _previousMouseState;
        public void Key(ref string T)
        {
            KeyboardState currentState = Keyboard.GetState();

            foreach (Keys key in currentState.GetPressedKeys())
            {
                if (_previousKeyboardState.IsKeyUp(key))
                {
                    if (key >= Keys.D0 && key <= Keys.D9)
                    {
                        T += (char)('0' + (key - Keys.D0));
                    }
                    else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                    {
                        T += (char)('0' + (key - Keys.NumPad0));
                    }
                    else if (key == Keys.Back && T.Length > 0)
                    {
                        T = T[..^1];
                    }
                }
            }
            _previousKeyboardState = currentState;
        }
        /// <summary>
        /// 不想写单独UI的UI
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
            //战斗宠物UI
            BattlePetsUI(spriteBatch);
            //消耗品UI
            DrawusingtheconsumablesUI(spriteBatch);
            //选择商店UI
            SelectStoreUI(spriteBatch);

            PlaystationUI.PlaystationUI.DrawUI(spriteBatch);

            LevelUI.Draw(spriteBatch);
            // Main.spriteBatch.End();
            //Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            for (int a = 0; a < TextSystem.UITextDraw.Length; a++)
            {
                if (TextSystem.UITextDraw[a].active)
                {
                    TextSystem.UITextDraw[a].Update();
                    Vector2 origin = ChatManager.GetStringSize(FontAssets.DeathText.Value, TextSystem.UITextDraw[a].Text, Vector2.One, 0) / 2;
                    ChatManager.DrawColorCodedStringWithShadow(
                    spriteBatch,
                    FontAssets.DeathText.Value, TextSystem.UITextDraw[a].Text, TextSystem.UITextDraw[a].position, TextSystem.UITextDraw[a].color * TextSystem.UITextDraw[a].Al, TextSystem.UITextDraw[a].rotating, origin, new Vector2(TextSystem.UITextDraw[a].scale), 114514);
                }
            }
            for (int a = 0; a < DustSystem.UIDustDraws.Length; a++)
            {
                if (DustSystem.UIDustDraws[a].active && !DustSystem.UIDustDraws[a].Special)
                {
                    DustSystem.UIDustDraws[a].Draw(spriteBatch);
                }
            }

            //神秘数字入口
            /*
            if (R)
            {
                Key(ref displayText);
            }
            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState.MiddleButton == ButtonState.Pressed && _previousMouseState.MiddleButton == ButtonState.Released)
            {
                foreach (TileEntity value in TileEntity.ByPosition.Values)
                {
                    if (value is NPCSpawnTE v)
                    {
                        v.Time= 0;
                    }
                }
                if (!R)
                {
                    Main.NewText("请输入数字");
                    R = true;
                }
                else if (R)
                {
                    if (displayText != "")
                    {
                        TR = int.Parse(displayText);
                        Main.NewText("数字已更新:" + TR);
                        displayText = "";
                    }
                    R = false;
                }
            }
            _previousMouseState = currentMouseState;
            if (displayText != "")
            {
                ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                FontAssets.DeathText.Value, displayText, new Vector2(10, 500), Color.White, 0, Vector2.Zero, new Vector2(1), 114514);
            }
            else
            if (TR > 0)
            {
                ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                FontAssets.DeathText.Value, TR + "", new Vector2(10, 500), Color.White, 0, Vector2.Zero, new Vector2(1), 114514);
            }*/
        }
        //战斗宠物UI
        public static bool BattlePetsUION = false;
        public int BattlePetsChoose = 0;
        public bool Abandon = false;
        public static int BattlePetsPages = -1;
		public void BattlePetsUI(SpriteBatch spriteBatch)
		{
			if (BattlePetsUION)
			{
				if (Main.gameMenu || Main.LocalPlayer.controlInv || Main.LocalPlayer.TalkNPC == null || Main.LocalPlayer.TalkNPC.type!=ModContent.NPCType<宠物管家史莱姆>())
				{
                    BattlePetsUION = false;
					BattlePetsPages = -1;

                    return;
                }
                if (BattlePetsPages==-1)
				{
					BattlePetsPages = 0;
                    for (int a = 0; a < Main.LocalPlayer.Dplayer().Bpets.Length; a++)
					{
						if(Main.LocalPlayer.Dplayer().Bpets[a].Fight)
						{
							BattlePetsPages = a/4;
							BattlePetsChoose = a;

                            break;

                        }

                    }

                }
				Texture2D texture = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/战斗宠物UI").Value;
				Texture2D Fighttexture = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/出战").Value;

				Texture2D FightGlow = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/出战_Glow").Value;
				Texture2D AbandonGlow = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/放生_Glow").Value;
				Texture2D SelectGlow = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/选择UI").Value;
                Texture2D Leftarrowhead = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/左箭头UI").Value;
				Texture2D Leftarrowhead2 = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/左箭头UI_Glow").Value;
				Texture2D Rightarrowhead = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/右箭头UI").Value;
				Texture2D Rightarrowhead2 = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/右箭头UI_Glow").Value;
				Texture2D Projtexture;

                Texture2D Off = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/删除UI").Value;
				Texture2D OffGlow = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/删除UI_Glow").Value;
				Texture2D SkillTexture;
				spriteBatch.Draw(texture, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);

                if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(Main.screenWidth/2-texture.Width/2), (int)(Main.screenHeight/2-texture.Height/2), 800, 500)))
                    Main.LocalPlayer.mouseInterface = true;

                Vector2 ArrowUI = new Vector2(Main.screenWidth, Main.screenHeight) / 2-texture.Size()/2+ new Vector2(96,438);

				if (BattlePetsPages > 0)
				{
					spriteBatch.Draw(Leftarrowhead, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);
					if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(ArrowUI.X), (int)(ArrowUI.Y), 20, 22)))
					{
						spriteBatch.Draw(Leftarrowhead2, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, new Color(255, 255, 0), 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);
						if (Main.mouseLeft && Main.mouseLeftRelease)
						{
							BattlePetsPages--;

                        }
					}
                }
                ArrowUI.X += 74;
                if (BattlePetsPages < Main.LocalPlayer.Dplayer().Bpets.Length / 4-1)
				{
					spriteBatch.Draw(Rightarrowhead, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);
					if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(ArrowUI.X), (int)(ArrowUI.Y), 20, 22)))
					{
						spriteBatch.Draw(Rightarrowhead2, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, new Color(255, 255, 0), 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);
						if (Main.mouseLeft && Main.mouseLeftRelease)
						{
							BattlePetsPages++;
						}
					}
				}
                ArrowUI.X -= 27;
                ArrowUI.Y += 11;

                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, (BattlePetsPages+1) + "/"+ Main.LocalPlayer.Dplayer().Bpets.Length / 4, ArrowUI.X, ArrowUI.Y, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, (BattlePetsPages + 1) + "/" + Main.LocalPlayer.Dplayer().Bpets.Length / 4, new Vector2(1)) / 2, 1);

                //for (int a = 0; a < Main.LocalPlayer.Dplayer().Bpets.Length; a++)
                if (Main.LocalPlayer.Dplayer().Bpets == null)
                    Main.LocalPlayer.Dplayer().Bpets = new BattlePets[20];
                for (int a = 0; a < Main.LocalPlayer.Dplayer().Bpets.Length; a++)
                {
                    if (Main.LocalPlayer.Dplayer().Bpets[a] == null)
                    {
                        Main.LocalPlayer.Dplayer().Bpets[a] = new BattlePets(0);
                    }
                }


				Vector2 vector = new Vector2(Main.screenWidth, Main.screenHeight) / 2-texture.Size()/2+new Vector2(88,88);
				BattlePets PetsChoose = Main.LocalPlayer.Dplayer().Bpets[BattlePetsChoose];
                if (BattlePetsChoose / 4 == BattlePetsPages)
                {
                    spriteBatch.Draw(SelectGlow, new Vector2(Main.screenWidth, Main.screenHeight) / 2 + new Vector2(0, 100 * (BattlePetsChoose % 4)), null, new Color(0, 255, 0, 255), 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);
                }
                for (int a = 0; a < 4; a++)
				{
					BattlePets pets = Main.LocalPlayer.Dplayer().Bpets[a+ BattlePetsPages*4];
					if (pets != null && pets.Type > 0)
                    {
						bool R = false;
                        Vector2 vector2 = new Vector2(Main.screenWidth, Main.screenHeight) / 2 - texture.Size() / 2 + new Vector2(48, 48);
                        if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(vector2.X), (int)(vector2.Y+ 100 * a), 190, 80)))
						{
                            if(BattlePetsChoose != a + BattlePetsPages * 4)
							spriteBatch.Draw(SelectGlow, new Vector2(Main.screenWidth, Main.screenHeight) / 2 + new Vector2(0, 100 * a), null, new Color(255, 255, 0, 255), 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
								BattlePetsChoose = a + BattlePetsPages * 4;
                            }
							if(BattlePetsChoose != a + BattlePetsPages * 4)
                            R = true;
                        }
						string Name = pets.Name(out int Variation);
                        float Nsc = 1;
                        if (ChatManager.GetStringSize(FontAssets.MouseText.Value, Name, new Vector2(1)).X > 100)
                        {
                            Nsc = 100 / ChatManager.GetStringSize(FontAssets.MouseText.Value, Name, new Vector2(1)).X;
                        }
                        pets.Draw(spriteBatch, vector, Vector2.Zero, new Vector3(0, 0, Variation), Color.White, Vector2.One, 0, 0, R);
                        if (pets.Fight)
                        spriteBatch.Draw(Fighttexture, vector+new Vector2(124,26), null, Color.White, 0, Fighttexture.Size()/2, 0.75f, 0, 0);
                        Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Name, vector.X+40, vector.Y, Color.White, Color.Black, new Vector2(0, ChatManager.GetStringSize(FontAssets.MouseText.Value, Name, new Vector2(1)).Y/2), Nsc);
                        Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, "Lv."+ pets.Level, vector.X+40, vector.Y+20, Color.White, Color.Black, new Vector2(0, ChatManager.GetStringSize(FontAssets.MouseText.Value, "Lv." + pets.Level, new Vector2(1)).Y/2), 1);
                    }
					vector.Y += 100;
                    pets.Update(a + BattlePetsPages * 4);
                }
				//选中绘制
                vector = new Vector2(Main.screenWidth, Main.screenHeight) / 2 - texture.Size() / 2 + new Vector2(318, 48) + new Vector2(414, 200)/2;
				if (PetsChoose != null && PetsChoose.Type > 0)
				{
                    spriteBatch.Draw(BattlePets.texture[PetsChoose.Background].Value, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, Color.White, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);

                    string Name = PetsChoose.Name(out int Variation);
                    PetsChoose.Draw(spriteBatch, vector, Vector2.Zero, new Vector3(0, 0, Variation), new Color(150, 150, 150, 255), Vector2.One, 0, 0);
                    Vector2 Datavector = vector - new Vector2(20, 108);
                    /*
                    Texture2D DataTexture = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/加成数据").Value;
                    spriteBatch.Draw(DataTexture, Datavector + DataTexture.Size() / 2, null, Color.White, 0, DataTexture.Size() / 2, 1, 0, 0);
                    if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(Datavector.X), (int)(Datavector.Y), 40, 40)))
                    {
                        string Text = Language.GetTextValue("Mods.DDmod.BattlePetUI.原始属性",PetsChoose.OriginalLifeMax,PetsChoose.OriginalDamage,PetsChoose.OriginalDefense);

                        UICommon.TooltipMouseText(Text);
                    }*/
                        Vector2 Skillvector = new Vector2(Main.screenWidth, Main.screenHeight) / 2 - texture.Size() / 2 + new Vector2(680, 176);

                    for (int a = 0; a < 4; a++)
                    {
                        if (PetsChoose.Skill[a] >= -2)
                        {
                            SkillTexture = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/技能/Skill_" + PetsChoose.Skill[a]).Value;
                            if (a == 1)
                            {
                                Skillvector.Y -= 46;

                            }
                            if (a == 2)
                            {
                                Skillvector.X -= 26;
                                Skillvector.Y -= 45;
                            }
                            if (a == 3)
                            {
                                Skillvector.X += 52;

                            }
                            Color color = new Color(50, 50, 50, 255);

                            if (Variation == 3 || Variation >= a + 1)
                            {
                                color = Color.White;
                            }
                            if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(Skillvector.X), (int)(Skillvector.Y), 40, 40)))
                            {
                                if (color == Color.White)
                                {
                                    string Text = Skill.Text(PetsChoose.Skill[a], PetsChoose);
                                    string Time(int time)
                                    {
                                        string t = "";
                                        time /= 60;
                                        if (time / 60 > 0)
                                        {
                                            t += time / 60 + Language.GetTextValue("Mods.DDmod.Tooltips.Minute");
                                        }
                                        if (time % 60 != 0)
                                        {
                                            t += (time % 60) + Language.GetTextValue("Mods.DDmod.Tooltips.Second");
                                        }
                                        return t;
                                    }
                                    if (a == 1)
                                    {
                                        Text += "\nCD: " + Time(PetsChoose.MaxSkillCD);

                                    }
                                    if (a == 3)
                                    {
                                        Text += "\nCD: " + Time(PetsChoose.MaxSkill2CD);

                                    }

                                    UICommon.TooltipMouseText(Text);

                                }
                                else
                                {
                                    if (a == 0)
                                    {
                                        UICommon.TooltipMouseText(Language.GetTextValue("Mods.DDmod.BattlePetUI.技能解锁", 16));
                                    }
                                    if (a == 1)
                                    {
                                        UICommon.TooltipMouseText(Language.GetTextValue("Mods.DDmod.BattlePetUI.技能解锁", 30));
                                    }
                                    if (a == 2)
                                    {
                                        UICommon.TooltipMouseText(Language.GetTextValue("Mods.DDmod.BattlePetUI.技能解锁", 46));
                                    }
                                    if (a == 3)
                                    {
                                        UICommon.TooltipMouseText(Language.GetTextValue("Mods.DDmod.BattlePetUI.技能解锁", 46));
                                    }
                                }
                            }
                            spriteBatch.Draw(SkillTexture, Skillvector + SkillTexture.Size() / 2, null, color, 0, SkillTexture.Size() / 2, 1, 0, 0);
                        }
                    }
					if (PetsChoose.Fight)
						spriteBatch.Draw(Fighttexture, vector + new Vector2(98, 42), null, Color.White, 0, Fighttexture.Size()/2, 1, 0, 0);

                    float Nsc = 1;
                    Name += " Lv." + PetsChoose.Level;
                    if (ChatManager.GetStringSize(FontAssets.MouseText.Value, Name , new Vector2(1)).X>240)
                    {
                        Nsc = 240 / ChatManager.GetStringSize(FontAssets.MouseText.Value, Name, new Vector2(1)).X;
                    }
					Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Name, vector.X, vector.Y + 102, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Name, new Vector2(1)) / 2, Nsc);

					if (!Abandon)
					{
						Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.放生"), vector.X - 178, vector.Y + 102, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.放生"), new Vector2(1)) / 2, 1);
					}
					else
                    {
                        Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.确定"), vector.X - 178, vector.Y + 102, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.确定"), new Vector2(1)) / 2, 1);
                    }
                    if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(vector.X - 208), (int)(vector.Y + 86), 60, 32)))
					{
						spriteBatch.Draw(AbandonGlow, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, new Color(255, 255, 0, 255), 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);
						if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
							if (!Abandon)
							{
								UITextDraw.NewText(new Vector2(Main.mouseX, Main.mouseY - 30), Language.GetTextValue("Mods.DDmod.BattlePetUI.确认放生"), 60, 0.5F);
								Abandon = true;

                            }
							else
                            {
                                UITextDraw.NewText(new Vector2(Main.mouseX, Main.mouseY - 30), Language.GetTextValue("Mods.DDmod.BattlePetUI.离开"), 60, 0.5F);
                                for (int a = BattlePetsChoose; a < Main.LocalPlayer.Dplayer().Bpets.Length-1; a++)
								{
									Main.LocalPlayer.Dplayer().Bpets[a] = Main.LocalPlayer.Dplayer().Bpets[a+1];
                                }
                                Main.LocalPlayer.Dplayer().Bpets[Main.LocalPlayer.Dplayer().Bpets.Length - 1] = new BattlePets(0);
                                Abandon = false;


                            }
							//PetsChoose.Fight = !PetsChoose.Fight;

                        }
					}
					else
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        Abandon = false;
                    }
                    if (!PetsChoose.Fight)
					{
						Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.出战"), vector.X + 184, vector.Y + 102, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.出战"), new Vector2(1)) / 2, 1);
					}
					else
					{
						Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.休息"), vector.X + 184, vector.Y + 102, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.休息"), new Vector2(1)) / 2, 1);
					}
					if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(vector.X + 154), (int)(vector.Y + 86), 60, 32)))
					{
						spriteBatch.Draw(FightGlow, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, new Color(255, 255, 0, 255), 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);
                        if (Main.mouseLeft && Main.mouseLeftRelease)
						{
							if (!PetsChoose.Fight)
							{
								for (int a = 0; a < Main.LocalPlayer.Dplayer().Bpets.Length; a++)
								{
									BattlePets pets = Main.LocalPlayer.Dplayer().Bpets[a];
                                    pets.Fight = false;
								}
							}
							PetsChoose.Fight = !PetsChoose.Fight;
                            if(PetsChoose.Fight)
                                ModContent.GetInstance<战宠出战>().Condition.Complete();
                            DDmod.SyncData(DDType.Battlepets, Main.myPlayer, -1, Main.myPlayer);
                            if (PetsChoose.Wounded)
							{
								PetsChoose.Fight = false;
                                UITextDraw.NewText(new Vector2(Main.mouseX, Main.mouseY - 30), Language.GetTextValue("Mods.DDmod.BattlePetUI.重伤"), 60, 0.5F);
                            }
						}

                    }
                    vector = new Vector2(Main.screenWidth, Main.screenHeight) / 2 - texture.Size() / 2 + new Vector2(306, 92)+new Vector2(45,22);

                    string PZText = Language.GetTextValue("Mods.DDmod.BattlePetUI.常见");
                    Color PZcolor = new Color(200, 200, 200);
                    if (PetsChoose.Rarity ==1)
                    {
                        PZText = Language.GetTextValue("Mods.DDmod.BattlePetUI.优秀");
                        PZcolor = new Color(100, 255, 100);
                    }
                    if(PetsChoose.Rarity ==2)
                    {
                        PZText = Language.GetTextValue("Mods.DDmod.BattlePetUI.稀有");
                        PZcolor = new Color(100, 255, 255);
                    }
                    if(PetsChoose.Rarity ==3)
                    {
                        PZText = Language.GetTextValue("Mods.DDmod.BattlePetUI.罕见");
                        PZcolor = new Color(120, 0, 255);
                    }
                    if(PetsChoose.Rarity ==4)
                    {
                        PZText = Language.GetTextValue("Mods.DDmod.BattlePetUI.珍稀");
                        PZcolor = new Color(255, 228, 45);
                    }
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, PZText, vector.X, vector.Y, PZcolor, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, PZText, new Vector2(1)) / 2, 1);
                    vector.Y += 42;
                    PZText = PetsChoose.setNature(out float Damage, out float Life, out float Defense, out float Endurance, out float Exp, out PZcolor);
                    
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, PZText, vector.X, vector.Y, PZcolor, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, PZText, new Vector2(1)) / 2, 1);
                    if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(vector.X - 45), (int)(vector.Y - 22), 90, 32)))
                    {
                        string Text = "";
                        if (Damage < 1)
                        {
                            Text += Language.GetTextValue("Mods.DDmod.BattlePetUI.降低", Language.GetTextValue("Mods.DDmod.BattlePetUI.最终攻击"), (Math.Abs(Damage - 1f) * 100).ToString("F0")+"%");
                            Text += "\n";
                        }
                        else if (Damage > 1)
                        {
                            Text += Language.GetTextValue("Mods.DDmod.BattlePetUI.增加", Language.GetTextValue("Mods.DDmod.BattlePetUI.最终攻击"), (Math.Abs(Damage - 1f) * 100).ToString("F0") + "%");
                            Text += "\n";
                        }
                        if (Life < 1)
                        {
                            Text += Language.GetTextValue("Mods.DDmod.BattlePetUI.降低", Language.GetTextValue("Mods.DDmod.BattlePetUI.最终生命"), (Math.Abs(Life - 1f) * 100).ToString("F0") + "%");
                            Text += "\n";
                        }
                        else if (Life > 1)
                        {
                            Text += Language.GetTextValue("Mods.DDmod.BattlePetUI.增加", Language.GetTextValue("Mods.DDmod.BattlePetUI.最终生命"), (Math.Abs(Life - 1f) * 100).ToString("F0") + "%");
                            Text += "\n";
                        }
                        if (Defense < 1)
                        {
                            Text += Language.GetTextValue("Mods.DDmod.BattlePetUI.降低", Language.GetTextValue("Mods.DDmod.BattlePetUI.最终防御"), (Math.Abs(Defense - 1f) * 100).ToString("F0") + "%");
                            Text += "\n";
                        }
                        else if (Defense > 1)
                        {
                            Text += Language.GetTextValue("Mods.DDmod.BattlePetUI.增加", Language.GetTextValue("Mods.DDmod.BattlePetUI.最终防御"), (Math.Abs(Defense - 1f) * 100).ToString("F0") + "%");
                            Text += "\n";
                        }
                        if (Endurance < 0)
                        {
                            Text += Language.GetTextValue("Mods.DDmod.BattlePetUI.降低", Language.GetTextValue("Mods.DDmod.BattlePetUI.减伤"), (Math.Abs(Endurance) * 100).ToString("F0") + "%");
                            Text += "\n";
                        }
                        else if (Endurance > 0)
                        {
                            Text += Language.GetTextValue("Mods.DDmod.BattlePetUI.增加", Language.GetTextValue("Mods.DDmod.BattlePetUI.减伤"), (Math.Abs(Endurance) * 100).ToString("F0") + "%");
                            Text += "\n";
                        }
                        if (Exp < 1)
                        {
                            Text += Language.GetTextValue("Mods.DDmod.BattlePetUI.降低2", Language.GetTextValue("Mods.DDmod.BattlePetUI.经验需求"), (Math.Abs(Exp - 1f) * 100).ToString("F0") + "%");
                            Text += "\n";
                        }
                        else if (Exp > 1)
                        {
                            Text += Language.GetTextValue("Mods.DDmod.BattlePetUI.增加2", Language.GetTextValue("Mods.DDmod.BattlePetUI.经验需求"), (Math.Abs(Exp - 1f) * 100).ToString("F0") + "%");
                            Text += "\n";
                        }
                        UICommon.TooltipMouseText(Text);
                    }
                    vector.Y += 42;
                    PZText = Language.GetTextValue("Mods.DDmod.BattlePetUI.成长");
                    PZcolor = new Color(255,255, 255);
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, PZText, vector.X, vector.Y, PZcolor, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, PZText, new Vector2(1)) / 2, 1);
                    if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)(vector.X-45), (int)(vector.Y-22), 90, 32)))
                    {
                        string Text = Language.GetTextValue("Mods.DDmod.BattlePetUI.原始属性",(PetsChoose.OriginalLifeMax*0.2f).ToString("F1"), (PetsChoose.OriginalDamage*0.1f).ToString("F1"), (PetsChoose.OriginalDefense*0.1f).ToString("F1"));

                        UICommon.TooltipMouseText(Text);
                    }
                    vector = new Vector2(Main.screenWidth, Main.screenHeight) / 2 - texture.Size() / 2 + new Vector2(318, 292);
					Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.生命") +":" + PetsChoose.Life+"/"+ PetsChoose.LifeMax, vector.X+96, vector.Y + 22, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.生命")+":" + PetsChoose.Life + "/" + PetsChoose.LifeMax, Vector2.One) / 2, 1);
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.攻击") + ":" + PetsChoose.Damage, vector.X+318, vector.Y + 22, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.攻击") + ":" + PetsChoose.Damage, Vector2.One) / 2, 1);

                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.防御") + ":" + PetsChoose.Defense, vector.X+ 96, vector.Y + 76, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.防御") + ":" + PetsChoose.Defense, Vector2.One) / 2, 1);
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.减伤") + ":" + ((PetsChoose.Endurance)*100).ToString("F1")+ "%", vector.X+318, vector.Y + 76, Color.White, Color.Black,ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.减伤") + ":" + ((PetsChoose.Endurance) * 100).ToString("F1") + "%", Vector2.One) / 2, 1);

                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.经验") + ":" + PetsChoose.Exp + "/" + PetsChoose.MaxExp, vector.X+ 96, vector.Y + 130, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.经验") + ":" + PetsChoose.Exp + "/" + PetsChoose.MaxExp, Vector2.One) / 2, 1);
                    if (PetsChoose.Melee)
                    {


                        Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.攻击模式"), vector.X + 318, vector.Y + 130, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.BattlePetUI.攻击模式"), Vector2.One) / 2, 1);
                    }
                    else
                    {
                        Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("远程"), vector.X + 318, vector.Y + 130, Color.White, Color.Black, ChatManager.GetStringSize(FontAssets.MouseText.Value, Language.GetTextValue("远程"), Vector2.One) / 2, 1);

                    }
                }
				Vector2 XUI = new Vector2(Main.screenWidth + 800, Main.screenHeight - 500) / 2 + new Vector2(-60, 60);
				spriteBatch.Draw(Off, XUI, null, Color.White, 0, Off.Size() / 2, SC, 0, 0);
				if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)XUI.X - 20, (int)XUI.Y - 20, 40, 40)))
				{
					if (SC < 1.3F)
					{
						SC += 0.03F;

					}
					else
					{
						SC = 1.3f;

					}
					if (Main.mouseLeft&& Main.mouseLeftRelease)
					{
                        BattlePetsUION = false;
						Main.LocalPlayer.SetTalkNPC(-1);

                    }
					spriteBatch.Draw(OffGlow, XUI, null, new Color(255, 255, 0), 0, OffGlow.Size() / 2, SC, 0, 0);
				}
				else
				{
					if (SC > 1)
					{
						SC -= 0.03F;
					}
					else
					{
						SC = 1;
					}
				}
			}

            if (Main.LocalPlayer.Dplayer().FightPets>=0)
			{
				BattlePets pets = Main.LocalPlayer.Dplayer().Bpets[Main.LocalPlayer.Dplayer().FightPets];
                Texture2D texture = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/战宠宠物数据UI").Value;
                Texture2D Lifetexture = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/战宠宠物数据血条UI").Value;
                Texture2D Exptexture = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/战宠宠物数据经验条UI").Value;
                Texture2D SkillTexture = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/技能/Skill_" + pets.Skill[1]).Value;
                Texture2D SkillTexture2 = ModContent.Request<Texture2D>("DDmod/UI/BattlePetUI/技能/Skill_" + pets.Skill[3]).Value;
                float Life = (float)pets.Life / pets.LifeMax;
                if (!float.IsNormal(Life))
                {
                    Life = 0;
                }
                if (Life > 1)
                {
                    Life = 1;
                }
                float Exp = (float)pets.Exp / pets.MaxExp;
                if (!float.IsNormal(Exp))
                {
                    Exp = 0;
                }
                if (Exp > 1)
                {
                    Exp = 1;
                }
                string Text =pets.Name(out int V) + 
					"\n" + Language.GetTextValue("Mods.DDmod.properties.生命") + ": " + pets.Life + "/" + pets.LifeMax +
                    "\n" + Language.GetTextValue("Mods.DDmod.Entrust.升级进度") + ": " + pets.Exp + "/" + pets.MaxExp +
                "\n" + Language.GetTextValue("Mods.DDmod.UI.右键拖动");

                DDPlayer Dplayer = Main.LocalPlayer.Dplayer();

                if (Dplayer.FightPetsUIPo == Vector2.Zero)
                {
                    Dplayer.FightPetsUIPo .X = 800;
                    Dplayer.FightPetsUIPo .Y = 100;
                }
                if (Dplayer.FightPetsUIPo .X < 53 || Dplayer.FightPetsUIPo .X > Main.screenWidth)
                {
                    Dplayer.FightPetsUIPo .X = Main.screenWidth / 2;
                }
                if (Dplayer.FightPetsUIPo .Y < 16 || Dplayer.FightPetsUIPo .Y > Main.screenHeight)
                {
                    Dplayer.FightPetsUIPo .Y = Main.screenHeight / 2;
                }
                spriteBatch.Draw(texture, Dplayer.FightPetsUIPo, null, Color.White, 0, texture.Size()/2, 1, 0, 0);
               
                spriteBatch.Draw(Lifetexture, Dplayer.FightPetsUIPo- texture.Size() / 2+new Vector2(38,22), new Rectangle(0, 0, (int)(Lifetexture.Width * Life), Lifetexture.Height), Color.White, 0, Vector2.Zero, 1, 0, 0);
                spriteBatch.Draw(Exptexture, Dplayer.FightPetsUIPo- texture.Size() / 2 + new Vector2(36, 34), new Rectangle(0, 0, (int)(Exptexture.Width*Exp), Exptexture.Height), Color.White, 0, Vector2.Zero, 1, 0, 0);
                if (pets.Level >= 30)
                {
                    float A = 1 - (float)pets.SkillCD / pets.MaxSkillCD;
                    spriteBatch.Draw(SkillTexture, Dplayer.FightPetsUIPo + new Vector2(-14, 40), new Rectangle(0, 0, (int)(SkillTexture.Width), (int)(SkillTexture.Height)), new Color(50, 50, 50, 50), 0, SkillTexture.Size() / 2, 0.6f, 0, 0);
                    spriteBatch.Draw(SkillTexture, Dplayer.FightPetsUIPo + new Vector2(-14, 40), new Rectangle(0, 0, (int)(SkillTexture.Width), (int)(SkillTexture.Height * A)), new Color(255, 255, 255, 50), 0, SkillTexture.Size() / 2, 0.6f, 0, 0);
                    if (pets.SkillCD > 0)
                    {
                        ChatManager.DrawColorCodedStringWithShadow(
                        spriteBatch,
                        FontAssets.DeathText.Value, "" + (int)(pets.SkillCD / 60 + 1), Dplayer.FightPetsUIPo + new Vector2(-14, 44), Color.White, 0, ChatManager.GetStringSize(FontAssets.DeathText.Value, "" + (int)(pets.SkillCD / 60 + 1), Vector2.One, 0) / 2, new Vector2(0.3f), 114);
                    }
                }
                if (pets.Level >= 46)
                {
                    float A = 1-(float)pets.Skill2CD / pets.MaxSkill2CD;
                    if (!float.IsNaN(A))
                    {
                        spriteBatch.Draw(SkillTexture2, Dplayer.FightPetsUIPo + new Vector2(14, 40), new Rectangle(0, 0, (int)(SkillTexture2.Width), (int)(SkillTexture2.Height)), new Color(50, 50, 50, 50), 0, SkillTexture2.Size() / 2, 0.6f, 0, 0);
                        spriteBatch.Draw(SkillTexture2, Dplayer.FightPetsUIPo + new Vector2(14, 40), new Rectangle(0, 0, (int)(SkillTexture2.Width), (int)(SkillTexture2.Height * A)), new Color(255, 255, 255, 50), 0, SkillTexture2.Size() / 2, 0.6f, 0, 0);
                        if (pets.Skill2CD > 0)
                        {
                            ChatManager.DrawColorCodedStringWithShadow(
                            spriteBatch,
                            FontAssets.DeathText.Value, "" + (int)(pets.Skill2CD / 60 + 1), Dplayer.FightPetsUIPo + new Vector2(14, 44), Color.White, 0, ChatManager.GetStringSize(FontAssets.DeathText.Value, "" + (int)(pets.Skill2CD / 60 + 1), Vector2.One, 0) / 2, new Vector2(0.3f), 114);
                        }
                    }
                }
                Color color = new Color(155, 155, 155, 255);
                if (pets.Level >= 5)
                {
                    color = new Color(255, 255, 255, 255);
                }
                if (pets.Level >= 10)
                {
                    color = new Color(100, 255, 100, 255);
                }
                if (pets.Level >= 20)
                {
                    color = new Color(100, 100, 255, 255);
                }
                if (pets.Level >= 30)
                {
                    color = new Color(255, 0, 255, 255);
                }
                if (pets.Level >= 40)
                {
                    color = new Color(255, 180, 0, 255);
                }
                if (pets.Level >= 50)
                {
                    color = new Color(255, 50, 50, 255);
                }

                Vector2 origin = ChatManager.GetStringSize(FontAssets.DeathText.Value, "" + pets.Level, Vector2.One, 0) / 2;
                ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                FontAssets.DeathText.Value, "" + pets.Level, Dplayer.FightPetsUIPo - new Vector2(38, -4), color, 0, origin, new Vector2(0.3f), 114);

                if (!BpetsmouseX && new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)Dplayer.FightPetsUIPo.X - 63, (int)Dplayer.FightPetsUIPo.Y - 25, 126, 50)))
                {
                    Main.LocalPlayer.mouseInterface = true;
					//Main.hoverItemName = Text;
					UICommon.TooltipMouseText(Text);
                    Bpetsmouse = true;
                }
                if (Main.mouseRight)
                {
                    BpetsmouseX = true;
                }

                if (Bpetsmouse && BpetsmouseX)
                {
                    Dplayer.FightPetsUIPo = new Vector2(Main.mouseX, Main.mouseY);
                }
                if (Main.mouseRightRelease)
                {
                    Bpetsmouse = false;
                    BpetsmouseX = false;
                }

            }
        }
        bool Bpetsmouse;
        bool BpetsmouseX;
        //可选商店
        public class Store
        {
            public float ShopSC = 1;
			public int ItemType;
			public int Type;
			public DDShop Shop;
			public Store(int T, int ItemType, DDShop Shop)
			{
				Type = T;
                this.ItemType = ItemType;
                this.Shop = Shop;

            }
            public void Draw(SpriteBatch spriteBatch, Vector2 Po,bool mouse = false)
            {
                Texture2D texture = TextureAssets.Item[ItemType].Value;
                spriteBatch.Draw(texture, Po, null, Color.White * 0.75f, 0, texture.Size() / 2, ShopSC, 0, 0);
                if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)Po.X - texture.Width / 2, (int)Po.Y - texture.Height / 2, texture.Width, texture.Height)))
                {
                    if (!mouse)
                    {
						if(Type==1)
                        {
                            Main.hoverItemName = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ItemType).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop1") + ")" });

                        }
						if(Type==2)
                        {
                            Main.hoverItemName = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ItemType).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop2") + ")"});

                        }
						if(Type==3)
                        {
                            Main.hoverItemName = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ItemType).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop3") + ")"});

                        }
						if(Type==4)
                        {
                            Main.hoverItemName = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ItemType).Name, "(" + Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop4") + ")"});

                        }
						if(Type==5)
                        {
                            Main.hoverItemName = Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ItemType).Name, "("+Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Shop5") + ")" });
                        }
                        if (Main.mouseLeft)
                        {
                            if (!mouse)
                            {

                                委托兑换商史莱姆.ShopType = Shop;
                            }
                        }

                        if (ShopSC <= 1.3F)
                            ShopSC += 0.05f;
                        else ShopSC = 1.3f;

                    }
                    else
                    {
                        ShopSC = 1f;
                    }
                    Main.LocalPlayer.mouseInterface = true;
                }
                else
                {
                    if (ShopSC > 1F)
                        ShopSC -= 0.05f;
                    else ShopSC = 1;
                }
            }
            public void DrawText(SpriteBatch spriteBatch, Vector2 Po,bool mouse = false)
            {
                Texture2D texture = TextureAssets.Item[ItemType].Value;
                if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)Po.X - texture.Width / 2, (int)Po.Y - texture.Height / 2, texture.Width, texture.Height)))
                {
                    if (!mouse)
                    {
                        Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Language.GetTextValue("Mods.DDmod.NPCs.委托兑换商史莱姆.Options1", new object[] { new Item(ItemType).Name }) + Type, Main.mouseX + 10, Main.mouseY + 10, Color.White, Color.Black, Vector2.Zero, 1);


                    }
                }
            }
		}

		/// <summary>
		/// 选择商店UI开关
		/// </summary>
		public static bool SelectStore = false;
		public float MY;
		public static List<Store> stores = new List<Store>();
        public void SelectStoreUI(SpriteBatch spriteBatch)
		{
            if (SelectStore)
			{
				if(stores==null || stores.Count==0)
				{
                    stores =
                    [
                        new Store(1, ModContent.ItemType<白色委托币>(),DDShop.white1),
                        new Store(2, ModContent.ItemType<白色委托币>(),DDShop.white2),
                        new Store(3, ModContent.ItemType<白色委托币>(),DDShop.white3),
                        new Store(4, ModContent.ItemType<白色委托币>(),DDShop.white4),
                        new Store(5, ModContent.ItemType<白色委托币>(),DDShop.white5),
                        new Store(1, ModContent.ItemType<绿色委托币>(),DDShop.green1),
                        new Store(2, ModContent.ItemType<绿色委托币>(),DDShop.green2),
                        new Store(3, ModContent.ItemType<绿色委托币>(),DDShop.green3),
                        new Store(4, ModContent.ItemType<绿色委托币>(),DDShop.green4),
                        new Store(5, ModContent.ItemType<绿色委托币>(),DDShop.green5),
                        new Store(1, ModContent.ItemType<蓝色委托币>(),DDShop.blue1),
                        new Store(2, ModContent.ItemType<蓝色委托币>(),DDShop.blue2),
                        new Store(3, ModContent.ItemType<蓝色委托币>(),DDShop.blue3),
                        new Store(4, ModContent.ItemType<蓝色委托币>(),DDShop.blue4),
                        new Store(5, ModContent.ItemType<蓝色委托币>(),DDShop.blue5),
                    ];
                }
                Texture2D texture;

                Vector2 Po = new Vector2(Main.screenWidth / 2 + 40, MY + 20);
                texture = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/选择商店UI").Value;
                spriteBatch.Draw(texture, Po, null, Color.White * 0.75f, 0, Vector2.Zero, 1, 0, 0);

				Po += new Vector2(20);
				for (int a = 0; a < stores.Count; a++)
				{
					stores[a].Draw(spriteBatch, Po, mouse);
					Po.X += 30;
					if (a>0&&a % 5 == 4)
					{
						Po.X -= 150;
						Po.Y += 30;
					}
                }
                mouse = false;
                if (Main.mouseLeft)
                {
                    mouse = true;
                }




                if (Main.npcShop>0 || Main.LocalPlayer.talkNPC==-1 || Main.LocalPlayer.TalkNPC.type!=ModContent.NPCType<委托兑换商史莱姆>())
				{
                    SelectStore = false;
                }


            }
			else
			{
				MY = Main.mouseY;

            }
		}
        /// <summary>
        ///添加消耗品内容
        /// </summary>

        public static List<int> type = new List<int>();
        public static List<bool> typeBool = new List<bool>();
        public static List<int> typeint = new List<int>();
        public static List<int> typestack = new List<int>();
		/// <summary>
		/// 消耗品UI开关
		/// </summary>
		public static bool XHPUION = false;
		//添加
		public void AddXHP()
		{
			type = new List<int>();
			typeBool = new List<bool>();
            typeint = new List<int>();
            typestack = new List<int>();
            //生命水晶
            type.Add(29);
			typeBool.Add(Main.LocalPlayer.statLifeMax>=400);
			typeint.Add(15);
			int L = (Main.LocalPlayer.statLifeMax - 100) / 20;
            if (L < 0) L = 0;
            if (L > 15) L = 15;
			typestack.Add(L);
            //魔力水晶
            type.Add(109);
			typeBool.Add(Main.LocalPlayer.statManaMax+Main.LocalPlayer.Dplayer().statManaMax >= 300);
            typeint.Add(14);
            L = (Main.LocalPlayer.statManaMax + Main.LocalPlayer.Dplayer().statManaMax-20) / 20;
            if (L < 0) L = 0;
            if (L > 14) L = 14;
            typestack.Add(L);
            //生命果
            type.Add(1291);
			typeBool.Add(Main.LocalPlayer.statLifeMax >= 500);
			typeint.Add(20); 
			L = (Main.LocalPlayer.statLifeMax - 400) / 5;
            if (L < 0) L = 0;
            if (L > 20) L = 20;
            typestack.Add(L);
            //蘑菇花
            type.Add(ModContent.ItemType<蘑菇花>());
            typeBool.Add(Main.LocalPlayer.Dplayer().statManaMax >= 200);
            typeint.Add(20);
            L = (Main.LocalPlayer.Dplayer().statManaMax - 100) / 5;
            if (L < 0) L = 0;
            if (L > 20) L = 20;
            typestack.Add(L);
            //活力水晶
            type.Add(5337);
			typeBool.Add(Main.LocalPlayer.usedAegisCrystal);
			typeint.Add(1);
            typestack.Add(Main.LocalPlayer.usedAegisCrystal ? 1:0);
            //奥术水晶
            type.Add(5339);
			typeBool.Add(Main.LocalPlayer.usedArcaneCrystal);
			typeint.Add(1);
            typestack.Add(Main.LocalPlayer.usedArcaneCrystal ? 1:0);
            //神盾果
            type.Add(5338);
            typeBool.Add(Main.LocalPlayer.usedAegisFruit);
            typeint.Add(1);
            typestack.Add(Main.LocalPlayer.usedAegisFruit ? 1 : 0);
            //奇幻花
            type.Add(ModContent.ItemType<奇幻花>());
            typeBool.Add(Main.LocalPlayer.Dplayer().FantasyFlowers);
            typeint.Add(1);
            typestack.Add(Main.LocalPlayer.Dplayer().FantasyFlowers ? 1 : 0);
            //恶魔之心
            type.Add(3335);
            typeBool.Add(Main.LocalPlayer.extraAccessory);
            typeint.Add(1);
            typestack.Add(Main.LocalPlayer.extraAccessory ? 1 : 0);
            if (Main.expertMode)
            {
                //生命蘑菇
                type.Add(ModContent.ItemType<生命蘑菇>());
                typeBool.Add(Main.LocalPlayer.Dplayer().MushroomsLife);
                typeint.Add(1);
                typestack.Add(Main.LocalPlayer.Dplayer().MushroomsLife ? 1 : 0);
                //生命蘑菇
                type.Add(ModContent.ItemType<魔法菇>());
                typeBool.Add(Main.LocalPlayer.Dplayer().MushroomsMana);
                typeint.Add(1);
                typestack.Add(Main.LocalPlayer.Dplayer().MushroomsMana ? 1 : 0);
            }
            //珍珠
            type.Add(5340);
			typeBool.Add(Main.LocalPlayer.usedGalaxyPearl);
			typeint.Add(1);
            typestack.Add(Main.LocalPlayer.usedGalaxyPearl ? 1:0);
            //蠕虫
            type.Add(5341);
			typeBool.Add(Main.LocalPlayer.usedGummyWorm);
			typeint.Add(1);
            typestack.Add(Main.LocalPlayer.usedGummyWorm ? 1:0);
            //酒
            type.Add(5342);
			typeBool.Add(Main.LocalPlayer.usedAmbrosia);
			typeint.Add(1);
            typestack.Add(Main.LocalPlayer.usedAmbrosia ? 1:0);
            //工匠面包
            type.Add(5326);
            typeBool.Add(Main.LocalPlayer.ateArtisanBread);
            typeint.Add(1);
            typestack.Add(Main.LocalPlayer.ateArtisanBread ? 1 : 0);
            //火把神的恩赐
            type.Add(5043);
            typeBool.Add(Main.LocalPlayer.unlockedBiomeTorches);
            typeint.Add(1);
            typestack.Add(Main.LocalPlayer.unlockedBiomeTorches ? 1 : 0);
            //矿车升级
            type.Add(5289);
			typeBool.Add(Main.LocalPlayer.unlockedSuperCart);
			typeint.Add(1);
            typestack.Add(Main.LocalPlayer.unlockedSuperCart ? 1:0);
            //旅商背包
            type.Add(5343);
            typeBool.Add(NPC.peddlersSatchelWasUsed);
            typeint.Add(1);
            typestack.Add(NPC.peddlersSatchelWasUsed ? 1 : 0);
            if (Main.expertMode)
            {

                //先进战斗书2
                type.Add(ModContent.ItemType<诅咒之火>());
                typeBool.Add(DDWorld.诅咒之火);
                typeint.Add(1);
                typestack.Add(DDWorld.诅咒之火 ? 1 : 0);
            }
            //先进战斗书
            type.Add(4382);
            typeBool.Add(NPC.combatBookWasUsed);
            typeint.Add(1);
            typestack.Add(NPC.combatBookWasUsed ? 1 : 0);
            //先进战斗书2
            type.Add(5336);
            typeBool.Add(NPC.combatBookVolumeTwoWasUsed);
            typeint.Add(1);
            typestack.Add(NPC.combatBookVolumeTwoWasUsed ? 1 : 0);
            //先进战斗书2
            type.Add(ModContent.ItemType<可疑外星蓝图>());
            typeBool.Add(Main.LocalPlayer.Dplayer().MeteorRecipe);
            typeint.Add(1);
            typestack.Add(Main.LocalPlayer.Dplayer().MeteorRecipe ? 1 : 0);
            //先进战斗书2
            type.Add(ModContent.ItemType<绿岩设计图>());
            typeBool.Add(Main.LocalPlayer.Dplayer().GreenstoneRecipe);
            typeint.Add(1);
            typestack.Add(Main.LocalPlayer.Dplayer().GreenstoneRecipe ? 1 : 0);
        }
		/// <summary>
		/// 绘制使用消耗品UI
		/// </summary>
		float SC = 1;
		float UISC = 0;
		bool mouse = false;


        public void DrawusingtheconsumablesUI(SpriteBatch spriteBatch)
        {

            Texture2D texture;

            if (Main.playerInventory&& !BackpackStrengtheningUI.Visible&& ModContent.GetInstance<DDConfigServer>().AdventureCoinDealerSlime)
			{
				Vector2 Po = new Vector2(570, 110);
				texture = TextureAssets.Item[ModContent.ItemType<白色委托币>()].Value;
				spriteBatch.Draw(texture, Po, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, "X" + Main.LocalPlayer.Dplayer().AdventureCoins, Po.X+24, Po.Y, Color.White, Color.Black,Vector2.Zero, 1);

                Po.Y += 30;

                texture = TextureAssets.Item[ModContent.ItemType<绿色委托币>()].Value;
				spriteBatch.Draw(texture, Po, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, "X" + Main.LocalPlayer.Dplayer().AdventureCoins2, Po.X + 24, Po.Y, Color.White, Color.Black, Vector2.Zero, 1);

                Po.Y += 30;

                texture = TextureAssets.Item[ModContent.ItemType<蓝色委托币>()].Value;
				spriteBatch.Draw(texture, Po, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, "X" + Main.LocalPlayer.Dplayer().AdventureCoins3, Po.X + 24, Po.Y, Color.White, Color.Black, Vector2.Zero, 1);
            }
            if (Main.playerInventory&&Main.LocalPlayer.chest==-1&& Main.npcShop==0&& !BackpackStrengtheningUI.Visible)
            {
                Texture2D JCUI = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/加成UI").Value;
                Texture2D JCUIGlow = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/加成UI_Glow").Value;
                Vector2 Po = new Vector2(568, 280);
				if(ModLoader.TryGetMod("Fargowiltas", out Mod Fargowiltas))
				{
                    Po.X += 50;
                }
                spriteBatch.Draw(JCUI, Po, null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)Po.X, (int)Po.Y, 40, 40)))
                {
                    if (!mouse)
                    {
                        spriteBatch.Draw(JCUIGlow, Po, null, new Color(255, 255, 0), 0, Vector2.Zero, 1, 0, 0);
						if (!XHPUION)
						{
							Main.hoverItemName = Language.GetTextValue("Mods.DDmod.UI.增益清单");
                        }
						if (Main.mouseLeft)
                        {
                            if (!mouse)
                            {
                                XHPUION = !XHPUION;
                            }
                        }
                    }
                    Main.LocalPlayer.mouseInterface = true;
                }
                mouse = false;
                if (Main.mouseLeft)
                {
                    mouse = true;
                }
            }
            texture = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/消耗品UI").Value;
            if (!XHPUION)
            {
				if (UISC > 0)
				{
					UISC -= 0.1f;
					spriteBatch.Draw(texture, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, Color.White * 0.75F, 0, new Vector2(texture.Width / 2, texture.Height / 2), UISC, 0, 0);
				}

                return;
            }
            if (UISC<1)
            {
				UISC += 0.1f;
                spriteBatch.Draw(texture, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, Color.White * 0.75F, 0, new Vector2(texture.Width / 2, texture.Height / 2), UISC, 0, 0);

                return;
            }
            if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle(Main.screenWidth / 2 - 400, Main.screenHeight / 2 - 250, 800, 500)))
                Main.LocalPlayer.mouseInterface = true;
			Texture2D texture2 = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/永久增益框UI").Value;
			Texture2D X = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/叉UI").Value;
			Texture2D Y = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/勾UI").Value;
			Texture2D Off = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/删除UI").Value;
			Texture2D OffGlow = ModContent.Request<Texture2D>("DDmod/UI/InterfaceUI/删除UI_Glow").Value;


			spriteBatch.Draw(texture, new Vector2(Main.screenWidth, Main.screenHeight) / 2, null, Color.White*0.75F, 0, new Vector2(texture.Width / 2, texture.Height / 2), 1, 0, 0);
			Vector2 vector = new Vector2(Main.screenWidth, Main.screenHeight) / 2 - new Vector2(362, 180);

			/*
			for (int a = -1; a <= 1; a += 2)
			{
				for (int b = -1; b <= 1; b += 2)
				{
					DynamicSpriteFontExtensionMethods.DrawString(
				spriteBatch,
				FontAssets.DeathText.Value,
				"永久增益",
				new Vector2(Main.screenWidth, Main.screenHeight - 380) / 2,
				new Color(0, 0, 0, 255), 0f,
			   ChatManager.GetStringSize(FontAssets.DeathText.Value, "永久增益", Vector2.One) / 2,
				1F, SpriteEffects.None, 0f);
				}
			}
			DynamicSpriteFontExtensionMethods.DrawString(
		spriteBatch,
		FontAssets.DeathText.Value,
		"永久增益",
		new Vector2(Main.screenWidth, Main.screenHeight - 380) / 2,
		new Color(0, 255, 0, 255), 0f,
	   ChatManager.GetStringSize(FontAssets.DeathText.Value, "永久增益", Vector2.One) / 2,
		1F, SpriteEffects.None, 0f);*/
			AddXHP();

            int R = type.Count;

            for (int a = 1; a < R+1; a++)
            {
                spriteBatch.Draw(texture2, vector, null, Color.White*0.75F, 0, Vector2.Zero, 1, 0, 0);
				DrawItem(spriteBatch, new Item(type[a - 1]), vector + new Vector2(60, 58 + 12) / 2, new Vector2(58, 36));

                Color color = new Color(255, 0, 0, 255);

                if (typeBool[a - 1])
                {
                    spriteBatch.Draw(Y, vector+new Vector2(46,46), null, Color.White, 0, X.Size()/2, 1, 0, 0);

                    color = new Color(0, 255, 0, 255);
                }
                else
                {
                    spriteBatch.Draw(X, vector + new Vector2(46, 46), null, Color.White, 0, Y.Size()/2, 1, 0, 0);

                }
                string text = typestack[a-1] + "/"+ typeint[a - 1];
                for (int c = -1; c <= 1; c += 2)
                {
                    for (int b = -1; b <= 1; b += 2)
                    {
                        DynamicSpriteFontExtensionMethods.DrawString(
                    spriteBatch,
        FontAssets.DeathText.Value,
                    text,
                    vector + new Vector2(30, 70),
                    new Color(0, 0, 0, 255), 0f,
                   ChatManager.GetStringSize(FontAssets.DeathText.Value, text, Vector2.One) / 2,
                    new Vector2(0.4F), SpriteEffects.None, 0f);
                    }
                }
                DynamicSpriteFontExtensionMethods.DrawString(
            spriteBatch,
        FontAssets.DeathText.Value,
              text,
           vector + new Vector2(30, 70),
            color, 0f,
           ChatManager.GetStringSize(FontAssets.DeathText.Value, text, Vector2.One) / 2,
            new Vector2(0.4F), SpriteEffects.None, 0f);

                vector.X += 74;
				if (a > 0 && a % 10 == 0)
				{
					vector.Y += 78;
                    vector.X -= 74*10;
                }
			}
			Vector2 XUI = new Vector2(Main.screenWidth+800, Main.screenHeight-500) / 2+new Vector2(-50,40);
            spriteBatch.Draw(Off, XUI, null, Color.White, 0, Off.Size() / 2, SC, 0, 0);
            if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)XUI.X - 20, (int)XUI.Y - 20, 40, 40)))
            {
                if (SC<1.3F)
				{
					SC += 0.03F;

                }
				else
				{
					SC = 1.3f;

                }
                if (Main.mouseLeft)
                {
                        XHPUION = false;
                }
                spriteBatch.Draw(OffGlow, XUI, null, new Color(255,255,0), 0, OffGlow.Size() / 2, SC, 0, 0);
            }
			else
			{
				if(SC>1)
				{
					SC -= 0.03F;
                }
				else{
					SC = 1;
                }
            }
			if (Main.LocalPlayer.controlInv)
            {
                XHPUION = false;
				Main.playerInventory = true;
            }

        }
        public void DrawItem(SpriteBatch spriteBatch, Item item, Vector2 ItemPo, Vector2 Size)
        {
            Rectangle DrawRectangle = new Rectangle((int)ItemPo.X, (int)ItemPo.Y, (int)Size.X, (int)Size.Y);
            if (item != null && item.type != 0)
            {
                var frame = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(TextureAssets.Item[item.type].Value) : TextureAssets.Item[item.type].Frame(1, 1, 0, 0);
                var size = frame.Size();
                var texScale = 1f;
                if (size.X > DrawRectangle.Width * 1F)
                {
                    texScale *= (float)DrawRectangle.Width * 1F / size.X;
                }
				if (size.Y > DrawRectangle.Height * 1F)
				{
					if (DrawRectangle.Width * 1F / size.X > DrawRectangle.Height * 1F / size.Y)
					{
						texScale *= (float)DrawRectangle.Height * 1F / size.Y;
					}
				}
                //绘制物品贴图
                Vector2 vector = new Vector2(DrawRectangle.X, DrawRectangle.Y);
                Main.instance.LoadItem(item.type);
                spriteBatch.Draw(TextureAssets.Item[item.type].Value, vector, new Rectangle?(frame), Color.White, 0, size / 2, texScale, 0, 0);
                //绘制物品左下角那个代表数量的数字
                if (item.stack > 1)
                {
                    spriteBatch.DrawString(FontAssets.MouseText.Value, item.stack.ToString(), new Vector2(DrawRectangle.X, DrawRectangle.Y), Color.White, 0f, Vector2.Zero, 0.75F, SpriteEffects.None, 0f);
                }
                if (DrawRectangle.Intersects(new Rectangle(Main.mouseX + DrawRectangle.Width / 2, Main.mouseY + DrawRectangle.Height / 2, 1, 1)))
                {
                    Main.hoverItemName = item.Name;
                    Main.HoverItem = item.Clone();
                }
            }
        }
    }
}