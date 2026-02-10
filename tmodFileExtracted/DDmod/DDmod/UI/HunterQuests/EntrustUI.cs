using DDmod.Content;
using DDmod.Content.Items.Sundries;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Players;
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
using static DDmod.Helper.DDHelper;

namespace DDmod.UI.HunterQuests
{

    //绘制面板
    class EntrustPanel : UIElement
    {
        public Vector2 vectors;
        public Vector2 vectorsText;
        public bool Bool;
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
        public bool MKey = false;
        public int Key2 = 5;
        Player player;
        public override void Update(GameTime gameTime)
        {
            player = Main.LocalPlayer;
            EntrustPlayer EntrustPlayer = Main.LocalPlayer.GetModPlayer<EntrustPlayer>();
            //设置按钮距离所属ui部件的最左端的距离
            Left.Set(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Width() / 2 + 400, 0f);
            //设置按钮距离所属ui部件的最顶端的距离
            Top.Set((Main.screenHeight / 2 + 20), 0f);


            int Width = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Width();
            int Height = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Height();

            Vector2 position = new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y) - new Vector2(Width,Height) / 2;
            if (new Rectangle((int)position.X, (int)position.Y, Width, Height).Intersects(new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1)))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            float U = (float)Main.screenHeight / 1080;
            vectors = new Vector2(Width * 0.92f, Height/1.5f) + position;
            vectorsText = new Vector2(Width * 0.04f, 48) + position;
            Rectangle rectangle = new Rectangle((int)(vectors.X - 18), (int)(vectors.Y - 24), 36, 48);
            if (rectangle.Intersects(new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1)))
            {
                Bool = true;
                Main.LocalPlayer.mouseInterface = true;
            }
            else
            {
                Bool = false;
            }
            if (Main.mouseLeft && Bool)
            {
                MKey = true;
            }
            if (!Main.mouseLeft&& MKey && Bool)
            {
                if (EntrustPlayer.entrust[page - 1].type >EntrustID.任务完成)
                {
                    if (EntrustPlayer.entrust[page - 1].Accept && EntrustPlayer.entrust[page - 1].EntrustFinish)
                    {
                        UITextDraw.NewText(new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), Language.GetTextValue("Mods.DDmod.Entrust.领取成功"), 60, 1);
                        EntrustPlayer.entrust[page - 1].ELoot();
                    }
                    else if (!EntrustPlayer.entrust[page - 1].Accept && !EntrustPlayer.entrust[page - 1].EntrustFinish)
                    {
                        if (player.BuyItem(EntrustPlayer.entrust[page - 1].value))
                        {
                            UITextDraw.NewText(new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), Language.GetTextValue("Mods.DDmod.Entrust.接受委托成功"), 60, 1);
                            EntrustPlayer.entrust[page - 1].AcceptEntrust();
                        }
                        else
                        {

                            UITextDraw.NewText(new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), Language.GetTextValue("Mods.DDmod.Entrust.你没有足够的押金"), 60, 1);
                        }
                    }
                    else
                    {
                        UITextDraw.NewText(new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), Language.GetTextValue("Mods.DDmod.Entrust.未完成"), 60, 1);
                    }
                }
                MKey = false;
            }
            //切换按钮
            handoffpage[0] = new Vector2(57, Height+40) + position;
            handoffpage[1] = new Vector2(Width - 57, Height+40) + position;
            Rectangle handoffRectangle = new Rectangle((int)(handoffpage[0].X - 49), (int)(handoffpage[0].Y - 40), 98, 80);
            Rectangle handoffRectangle2 = new Rectangle((int)(handoffpage[1].X - 49), (int)(handoffpage[1].Y - 40), 98, 80);
            if (handoffRectangle.Intersects(new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1)))
            {
                handoffpageBool[0] = true;
                Main.LocalPlayer.mouseInterface = true;
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
                Main.LocalPlayer.mouseInterface = true;
                if (Main.mouseLeft)
                {
                    Key = true;
                }
            }
            else
            {
                handoffpageBool[1] = false;
            }
            EntrustPlayer.MaxEntrust = 5;
            int MaxPage = EntrustPlayer.MaxEntrust;
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
            player = Main.LocalPlayer;
            EntrustPlayer EntrustPlayer = Main.LocalPlayer.GetModPlayer<EntrustPlayer>();
            float Width = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Width();
            float Height = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Height();
            //创建一个局部变量，变量的值为ui左上角坐标
            Vector2 position = new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y);
            Texture2D 标题 = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务标题").Value;
            spriteBatch.Draw(标题, position - new Vector2(0, Height / 2+标题.Height/2), null, Color.White, 0, 标题.Size()/2, 1, 0, 0);
            //关卡选项
            string Text = EntrustPlayer.entrust[page-1].Text;
            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Value, position, null, Color.White, 0, new Vector2(Width, Height) / 2, 1, 0, 0);


            if (EntrustPlayer.entrust[page - 1].type != EntrustID.任务完成)
            {
                if (Bool)
                {
                    if (!EntrustPlayer.entrust[page - 1].Accept)
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮").Value, vectors, null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮").Size() / 2, 1F, 0, 0);
                        if (player.CanAfford(EntrustPlayer.entrust[page - 1].value))
                        {
                            string T = "";
                            int Value = EntrustPlayer.entrust[page - 1].value;
                            int 铂金 = Value / 1000000;

                            int 金 = Value / 10000;
                            金 %= 100;
                            int 银 = Value / 100;
                            银 %= 100;
                            int 铜 = Value;
                            铜 %= 100;

                            if (EntrustPlayer.entrust[page - 1].value<100)
                            {
                                T = Language.GetTextValue("Mods.DDmod.Entrust.接受委托1", new string[] { EntrustPlayer.entrust[page - 1].value.ToString() });
                            }
                            else if (EntrustPlayer.entrust[page - 1].value < 10000)
                            {
                                T = Language.GetTextValue("Mods.DDmod.Entrust.接受委托2", new string[] { 银.ToString(),铜.ToString() });
                            }
                            else if (EntrustPlayer.entrust[page - 1].value < 1000000)
                            {
                                T = Language.GetTextValue("Mods.DDmod.Entrust.接受委托3", new string[] { 金.ToString(), 银.ToString(), 铜.ToString() });
                            }
                            else
                            {
                                T = Language.GetTextValue("Mods.DDmod.Entrust.接受委托4", new string[] { 铂金.ToString(),金.ToString(),银.ToString(), 铜.ToString() });
                            }
                            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮_Glow").Value, vectors, null, new Color(0, 255, 0, 255), 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮_Glow").Size() / 2, 1F, 0, 0);
                            StrokeText(spriteBatch, T, new Vector2(Main.mouseX, Main.mouseY) + new Vector2(20), Vector2.Zero, new Color(0, 255, 0, 255), Color.White, 1f, 1);
                        }
                        else
                        {
                            string T = "";
                            int Value = EntrustPlayer.entrust[page - 1].value;
                            int 铂金 = Value / 1000000;

                            int 金 = Value / 10000;
                            金 %= 100;
                            int 银 = Value / 100;
                            银 %= 100;
                            int 铜 = Value;
                            铜 %= 100;

                            if (EntrustPlayer.entrust[page - 1].value < 100)
                            {
                                T = Language.GetTextValue("Mods.DDmod.Entrust.接受委托1", new string[] { EntrustPlayer.entrust[page - 1].value.ToString() });
                            }
                            else if (EntrustPlayer.entrust[page - 1].value < 10000)
                            {
                                T = Language.GetTextValue("Mods.DDmod.Entrust.接受委托2", new string[] { 银.ToString(), 铜.ToString() });
                            }
                            else if (EntrustPlayer.entrust[page - 1].value < 1000000)
                            {
                                T = Language.GetTextValue("Mods.DDmod.Entrust.接受委托3", new string[] { 金.ToString(), 银.ToString(), 铜.ToString() });
                            }
                            else
                            {
                                T = Language.GetTextValue("Mods.DDmod.Entrust.接受委托4", new string[] { 铂金.ToString(), 金.ToString(), 银.ToString(), 铜.ToString() });
                            }
                            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮_Glow").Value, vectors, null, new Color(255, 0, 0, 255), 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮_Glow").Size() / 2, 1F, 0, 0);
                            StrokeText(spriteBatch, T, new Vector2(Main.mouseX, Main.mouseY) + new Vector2(20), Vector2.Zero, new Color(255, 0, 0, 255), Color.White, 1f, 1);

                        }
                    }
                    else if (EntrustPlayer.entrust[page - 1].Accept && !EntrustPlayer.entrust[page - 1].EntrustFinish)
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Value, vectors, null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Size() / 2, 1F, 0, 0);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮_Glow").Value, vectors, null, new Color(255, 0, 0, 255), 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮_Glow").Size() / 2, 1F, 0, 0);
                        StrokeText(spriteBatch, Language.GetTextValue("Mods.DDmod.Entrust.未完成"), new Vector2(Main.mouseX, Main.mouseY) + new Vector2(20), Vector2.Zero, new Color(255, 0, 0, 255), Color.White, 1f, 1);
                    }
                    else if (EntrustPlayer.entrust[page - 1].Accept && EntrustPlayer.entrust[page - 1].EntrustFinish)
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Value, vectors, null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Size() / 2, 1F, 0, 0);
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮_Glow").Value, vectors, null, new Color(0, 255, 0, 255), 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮_Glow").Size() / 2, 1F, 0, 0);
                        StrokeText(spriteBatch, Language.GetTextValue("Mods.DDmod.Entrust.领取奖励"), new Vector2(Main.mouseX, Main.mouseY) + new Vector2(20), Vector2.Zero, new Color(0, 255, 0, 255), Color.White, 1f, 1);
                    }
                }
                else
                {
                    if (!EntrustPlayer.entrust[page - 1].Accept)
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮").Value, vectors, null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/进入按钮").Size() / 2, 1, 0, 0);
                    }
                    else if (EntrustPlayer.entrust[page - 1].Accept && !EntrustPlayer.entrust[page - 1].EntrustFinish)
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Value, vectors, null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Size() / 2, 1, 0, 0);
                    }
                    else if (EntrustPlayer.entrust[page - 1].Accept && EntrustPlayer.entrust[page - 1].EntrustFinish)
                    {
                        spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Value, vectors, null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Size() / 2, 1, 0, 0);
                    }
                }
            }


            DynamicSpriteFont font = FontAssets.MouseText.Value;
            TextDisplayCache textDisplay = new TextDisplayCache();
            textDisplay.PrepareCache(Text, font,380);
            string[] textLines = textDisplay.TextLines;
            int amountOfLines = textDisplay.AmountOfLines + 1;
            float C = ChatManager.GetStringSize(font, textLines[0], Vector2.One, 0).X / 2;
            for (int A = 0; A < amountOfLines; A++)
            {
                if (C < ChatManager.GetStringSize(font, textLines[A], Vector2.One, 0).X / 2)
                {
                    C = ChatManager.GetStringSize(font, textLines[A], Vector2.One, 0).X / 2;
                }
            }
            for (int A = 0; A < amountOfLines; A++)
            {
                if (A != amountOfLines - 1)
                {
                    textLines[A] = textLines[A].Remove(textLines[A].Length - 1).Replace("-", "");
                }
                Vector2 vector = new Vector2(0, 23 * (A-1));
                Vector2 o = ChatManager.GetStringSize(font, textLines[A], Vector2.One, 0) / 2;

                ChatManager.DrawColorCodedStringWithShadow(
                    spriteBatch,
                    font, textLines[A], vectorsText + vector, Color.White, 0, Vector2.Zero, new Vector2(1), 380);
            }
          //  spriteBatch.Draw(DDTextures.WhitePng.Value, vectorsText+new Vector2(0,80), null, Color.White, 0, Vector2.Zero, new Vector2(Width/2-Width*0.02F,1), 0, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/需求图标").Value, vectorsText + new Vector2(0, 78), null, Color.White, 0, Vector2.Zero, 1, 0, 0);
            if (EntrustPlayer.entrust[page - 1].type == EntrustID.收集任务)
            {
                ChatManager.DrawColorCodedStringWithShadow(
                    spriteBatch,
                    font, ":", vectorsText + new Vector2(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Width(), 84), Color.White, 0, Vector2.Zero, new Vector2(1), 420);
                DrawItem(spriteBatch, EntrustPlayer.entrust[page - 1].EntrustItem, vectorsText + new Vector2(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Width() + 20, 98), new Vector2(40));
            }
            else if (EntrustPlayer.entrust[page - 1].type == EntrustID.战斗任务)
            {
                if(EntrustPlayer.entrust[page - 1].EntrustNPC != null)
                {
                    string Name = EntrustPlayer.entrust[page - 1].EntrustNPC.FullName;
                    NPC[] npc = new NPC[3] { new NPC(), new NPC(), new NPC(), };
                    if (EntrustPlayer.entrust[page - 1].EntrustNPC.type == 1)
                    {
                        npc[0].SetDefaults(ModContent.NPCType<GrassSlime>());
                        npc[1].SetDefaults(147);
                        Name = Language.GetTextValue("Mods.DDmod.Entrust.史莱姆统计", npc[0].FullName ,npc[1].FullName );
                    }
                    if (EntrustPlayer.entrust[page - 1].EntrustNPC.type == 63)
                    {
                        npc[0].SetDefaults(63);
                        npc[1].SetDefaults(64);
                        npc[2].SetDefaults(103);
                        Name = Language.GetTextValue("Mods.DDmod.Entrust.三统计", npc[0].FullName, npc[1].FullName, npc[2].FullName);
                    }
                    if (EntrustPlayer.entrust[page - 1].EntrustNPC.type == 635)
                    {
                        npc[0].SetDefaults(635);
                        npc[1].SetDefaults(254);
                        Name = Language.GetTextValue("Mods.DDmod.Entrust.或", npc[0].FullName, npc[1].FullName);
                    }
                    textDisplay.PrepareCache(":" + Name + "X" + EntrustPlayer.entrust[page - 1].MaxEntrustNPCStack, font, 360);
                    textLines = textDisplay.TextLines;
                    amountOfLines = textDisplay.AmountOfLines + 1;
                    C = ChatManager.GetStringSize(font, textLines[0], Vector2.One, 0).X / 2;
                    for (int A = 0; A < amountOfLines; A++)
                    {
                        if (C < ChatManager.GetStringSize(font, textLines[A], Vector2.One, 0).X / 2)
                        {
                            C = ChatManager.GetStringSize(font, textLines[A], Vector2.One, 0).X / 2;
                        }
                    }
                    for (int A = 0; A < amountOfLines; A++)
                    {
                        if (A != amountOfLines - 1)
                        {
                            textLines[A] = textLines[A].Remove(textLines[A].Length - 1).Replace("-", "");
                        }
                        Vector2 vector = new Vector2(0, 23 * (A));
                        Vector2 o = ChatManager.GetStringSize(font, textLines[A], Vector2.One, 0) / 2;

                        ChatManager.DrawColorCodedStringWithShadow(
                            spriteBatch,
                            font, textLines[A], vectorsText + new Vector2(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Width(), 88) + vector, Color.White, 0, Vector2.Zero, new Vector2(1), 360);
                    }
                }
            }
           // spriteBatch.Draw(DDTextures.WhitePng.Value, vectorsText+new Vector2(0,160), null, Color.White, 0, Vector2.Zero, new Vector2(Width/2-Width*0.02F,1), 0, 0);
            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Value, vectorsText + new Vector2(0, 160), null, Color.White, 0, Vector2.Zero, 1, 0, 0);
            ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                font, ":", vectorsText + new Vector2(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Width(), 166), Color.White, 0, Vector2.Zero, new Vector2(1), 420);
            if (EntrustPlayer.entrust[page - 1].Loot != null)
            {
                for (int A = 0; A < EntrustPlayer.entrust[page - 1].Loot.Count; A++)
                {
                    DrawItem(spriteBatch, EntrustPlayer.entrust[page - 1].Loot[A], vectorsText + new Vector2(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Width() + 20 + 40 * A, 178), new Vector2(40));
                }
            }
            ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                font, "Lv."+ EntrustPlayer.entrust[page - 1].Level, vectorsText + new Vector2(Width-30, 48), Color.White, 0, new Vector2(ChatManager.GetStringSize(font, "Lv."+ EntrustPlayer.entrust[page - 1].Level, Vector2.One, 0).X,0), new Vector2(1), 420);
            ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                font, page+"/" + EntrustPlayer.MaxEntrust, vectorsText + new Vector2(Width-30, 180), Color.White, 0, new Vector2(ChatManager.GetStringSize(font, page + "/" + EntrustPlayer.MaxEntrust, Vector2.One, 0).X,0), new Vector2(1), 420);
            //StrokeText(spriteBatch, "" + page, (handoffpage[0] + handoffpage[1]) / 2, ChatManager.GetStringSize(FontAssets.MouseText.Value, "" + page, Vector2.One, 0),new Color(0,0,0,255),Color.White, 1.5f,1);


            Vector2 origin = ChatManager.GetStringSize(FontAssets.DeathText.Value, Language.GetTextValue("Mods.DDmod.Entrust.每日委托"), Vector2.One, 0) / 2;
            origin.Y -= origin.Y / 2;
            StrokeText(spriteBatch, Language.GetTextValue("Mods.DDmod.Entrust.每日委托"), position - new Vector2(0, Height / 2 + 标题.Height / 2), origin, new Color(0, 0, 0, 255), Color.White, 2, 1);
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
            DrawItem(spriteBatch, EntrustPlayer.entrust[page - 1].EntrustItem, vectorsText + new Vector2(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Width() + 20, 98), new Vector2(40));
            if (EntrustPlayer.entrust[page - 1].type == EntrustID.任务完成)
            {
                ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                FontAssets.DeathText.Value, "√", vectors + new Vector2(0, 14), new Color(0, 255, 0), 0, ChatManager.GetStringSize(FontAssets.DeathText.Value, "√", Vector2.One, 0) / 2, new Vector2(1), 420);
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
                    texScale *= (float)DrawRectangle.Height * 1F / size.Y;
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
    internal class EntrustUI : UIState
    {
        public static bool Visible = false;
        public static Vector2 Location;
        UIImageButton button;
        EntrustPanel UI;
        private UIText text;
        public static int Synthesis;

        public override void OnInitialize()
        {
            
            text = new UIText("0/0", 0.8f);
            text.Width.Set(50, 0f);
            text.Height.Set(50, 0f);
            text.TextColor = new Color(158, 242, 255);
            
            //用tr原版图片实例化一个图片按钮
            UI = new EntrustPanel();
            //设置按钮距宽度
            UI.Width.Set(400f, 0f);
            //设置按钮高度
            UI.Height.Set(800f, 0f);
            //设置按钮距离所属ui部件的最左端的距离
            UI.Left.Set(Main.screenWidth / 8, 0f);
            //设置按钮距离所属ui部件的最顶端的距离
            UI.Top.Set((Main.screenHeight / 2 + 20), 0f);
            Append(UI);

            base.OnInitialize();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.talkNPC == -1 || Main.LocalPlayer.talkNPC != Synthesis)
            {
                Visible = false;
            }
            UI.Update(gameTime);
        }

        public override void Recalculate()
        {
            base.Recalculate();
        }
    }
}
