using DDmod.Content;
using DDmod.Content.Items.Melee.TwinSwords;
using DDmod.Content.Items.Ranged;
using DDmod.Content.Items.Series.Venture;
using DDmod.Content.Items.Series.Venture.Level_1;
using DDmod.Content.Items.Series.Venture.奖励袋.特别奖励;
using DDmod.Content.Items.Talisman;
using DDmod.Content.Items.农场.种子;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Players;
using DDmod.SubworldLibraryWorld;
using DDmod.SubworldLibraryWorld.草原;
using DDmod.UI.ItemUI.背包;
using log4net.Core;
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
    class EntrustLvelePanel : UIElement
    {
        public Vector2 vectors;
        public Vector2 vectorsText;
        public bool Bool;
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
            //设置按钮距离所属ui部件的最左端的距离
            Left.Set(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Width() / 2 + 400, 0f);
            //设置按钮距离所属ui部件的最顶端的距离
            Top.Set((Main.screenHeight / 2 + 20), 0f);


            int Width = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Width();
            int Height = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Height();

            Vector2 position = new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y) - new Vector2(Width, Height) / 2;
            if (new Rectangle((int)position.X, (int)position.Y, Width, Height).Intersects(new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1)))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            float U = (float)Main.screenHeight / 1080;
            vectors = new Vector2(Width * 0.92f, Height / 1.5f) + position;
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
            if (!Main.mouseLeft && MKey && Bool)
            {
                LevelDemand(player.GetModPlayer<EntrustPlayer>().AdventurerLevel, out bool Demand, out NPC npc,out int NeedLevel);
                if (Demand)
                {
                    UITextDraw.NewText(new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), Language.GetTextValue("Mods.DDmod.Entrust.领取成功"), 60, 1);
                    LevelELoot(player.GetModPlayer<EntrustPlayer>().AdventurerLevel);
                }
                MKey = false;
            }
            //切换按钮
            handoffpage[0] = new Vector2(57, Height + 40) + position;
            handoffpage[1] = new Vector2(Width - 57, Height + 40) + position;
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
            //int MaxPage = EntrustPlayer.entrusts.Length;
            base.Update(gameTime);
        }
        //重写绘制方法
        public override void Draw(SpriteBatch spriteBatch)
        {
            player = Main.LocalPlayer;
            float Width = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Width();
            float Height = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Height();
            //创建一个局部变量，变量的值为ui左上角坐标
            Vector2 position = new Vector2(GetInnerDimensions().X, GetInnerDimensions().Y);
            Texture2D 标题 = ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务标题").Value;
            spriteBatch.Draw(标题, position - new Vector2(0, Height / 2 + 标题.Height / 2), null, Color.White, 0, 标题.Size() / 2, 1, 0, 0);
            //关卡选项
            string Text = LevelDemand(player.GetModPlayer<EntrustPlayer>().AdventurerLevel, out bool Demand, out NPC npc,out int NeedLevel);

            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/猎人任务面板2").Value, position, null, Color.White, 0, new Vector2(Width, Height) / 2, 1, 0, 0);


            if (Bool)
            {
                if (Demand)
                {
                    StrokeText(spriteBatch, Language.GetTextValue("Mods.DDmod.Entrust.领取奖励"), new Vector2(Main.mouseX, Main.mouseY) + new Vector2(20), Vector2.Zero, new Color(0, 255, 0, 255), Color.White, 1f, 1);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Value, vectors, null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Size() / 2, 1F, 0, 0);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮_Glow").Value, vectors, null, new Color(0, 255, 0, 255), 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮_Glow").Size() / 2, 1F, 0, 0);
                }
            }
            else
            {
                if (Demand)
                {
                    spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Value, vectors, null, Color.White, 0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Size() / 2, 1, 0, 0);
                }
            }


            DynamicSpriteFont font = FontAssets.MouseText.Value;
            TextDisplayCache textDisplay = new TextDisplayCache();
            textDisplay.PrepareCache(Text, font, 390);
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
                Vector2 vector = new Vector2(0, 23 * (A - 1));
                Vector2 o = ChatManager.GetStringSize(font, textLines[A], Vector2.One, 0) / 2;

                ChatManager.DrawColorCodedStringWithShadow(
                    spriteBatch,
                    font, textLines[A], vectorsText + vector, Color.White, 0, Vector2.Zero, new Vector2(1), 390);
            }
            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/需求图标").Value, vectorsText + new Vector2(0, 78), null, Color.White, 0, Vector2.Zero, 1, 0, 0);

            string Name = npc.FullName;
            if (npc.type == NPCID.None)
            {
                Name = Language.GetTextValue("Mods.DDmod.Entrust.无");
            }
            if (npc.type == 134)
            {
                Name = Language.GetTextValue("Mods.DDmod.Entrust.机械三王");
            }
            textDisplay.PrepareCache(Language.GetTextValue("Mods.DDmod.Entrust.击败目标") + Name , font, 360);
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
            ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                font, Language.GetTextValue("Mods.DDmod.Entrust.需要等级")+": " + NeedLevel, vectorsText + new Vector2(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Width(), 72 + 25 * (amountOfLines + 1)), Color.White, 0, Vector2.Zero, new Vector2(1), 360);

            spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Value, vectorsText + new Vector2(0, 160), null, Color.White, 0, Vector2.Zero, 1, 0, 0);
            ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                font, ":", vectorsText + new Vector2(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Width(), 166), Color.White, 0, Vector2.Zero, new Vector2(1), 420);
            if (LevelLoot(player.GetModPlayer<EntrustPlayer>().AdventurerLevel) != null)
            {
                for (int A = 0; A < LevelLoot(player.GetModPlayer<EntrustPlayer>().AdventurerLevel).Count; A++)
                {
                    DrawItem(spriteBatch, LevelLoot(player.GetModPlayer<EntrustPlayer>().AdventurerLevel)[A], vectorsText + new Vector2(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/领取按钮").Width() + 20 + 40 * A, 178), new Vector2(40));
                }
            }


            Vector2 origin = ChatManager.GetStringSize(FontAssets.DeathText.Value, Language.GetTextValue("Mods.DDmod.Entrust.冒险等级"), Vector2.One, 0) / 2;
            origin.Y -= origin.Y / 2;
            StrokeText(spriteBatch, Language.GetTextValue("Mods.DDmod.Entrust.冒险等级"), position - new Vector2(0, Height / 2 + 标题.Height / 2), origin, new Color(0, 0, 0, 255), Color.White, 2, 1);

            base.Draw(spriteBatch);
        }
        public void StrokeText(SpriteBatch spriteBatch, string Text, Vector2 position, Vector2 origin, Color color1, Color color2, float Scale = 1, int Thick = 1)
        {
            for (int y = -Thick; y <= Thick; y++)
            {
                for (int x = -Thick; x <= Thick; x++)
                {
                    DynamicSpriteFontExtensionMethods.DrawString(
                        spriteBatch,
                        FontAssets.DeathText.Value,
                        Text,
                        position + new Vector2(x, y),
                        color1, 0f,
                        origin,
                        Scale * 0.4F, SpriteEffects.None, 0f);
                }
            }
            DynamicSpriteFontExtensionMethods.DrawString(
                spriteBatch,
                FontAssets.DeathText.Value,
                Text,
                position,
                color2, 0f,
                origin,
                Scale * 0.4F, SpriteEffects.None, 0f);
        }
        public void DrawItem(SpriteBatch spriteBatch, Item item, Vector2 ItemPo, Vector2 Size)
        {
            Rectangle DrawRectangle = new Rectangle((int)ItemPo.X, (int)ItemPo.Y, (int)Size.X, (int)Size.Y);
            if (item != null && item.type != 0)
            {
                var frame = Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(TextureAssets.Item[item.type].Value) : TextureAssets.Item[item.type].Frame(1, 1, 0, 0);
                var size = frame.Size();
                var texScale = 1f;
                if (DrawRectangle.Width > DrawRectangle.Height)
                {
                    if (size.X > DrawRectangle.Width * 1F)
                    {
                        texScale *= (float)DrawRectangle.Width * 1F / size.X;
                    }
                }
                else
                {
                    if (size.Y > DrawRectangle.Height * 1F)
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
        //战利品
        public void LevelELoot(int Level)
        {
            Main.LocalPlayer.GetModPlayer<EntrustPlayer>().AdventurerLevel++;
            if (LevelLoot(Level) != null)
            {
                for (int A = 0; A < LevelLoot(Level).Count; A++)
                {
                    if (LevelLoot(Level)[A].type != ModContent.ItemType<升级>())
                    {
                        Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Loot(), LevelLoot(Level)[A], LevelLoot(Level)[A].stack);
                    }
                }
            }
        }
        //战利品
        public List<Item> LevelLoot(int Level)
        {
            List<Item> items = new List<Item>();
            items.Add(new Item(ModContent.ItemType<升级>()));
            if (Level == 0)
            {
                Item item = new Item(ModContent.ItemType<布袋>(), 1);
                item.SetDefaults(ModContent.ItemType<布袋>());
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[0] = new Item(ModContent.ItemType<叶木双剑>(), 1);
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[1] = new Item(27, 20);
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[2] = new Item(28, 10);
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[3] = new Item(ModContent.ItemType<StrengtheningStone>(), 3);
                items.Add(item);
            }
            if (Level == 1)
            {
                items.Add(new Item(ModContent.ItemType<樱之弓>(), 1));
                items.Add(new Item(ModContent.ItemType<荧光果种子>(), 3));
                items.Add(new Item(1325, 1));
                items.Add(new Item(188, 10));
                items.Add(new Item(ModContent.ItemType<StrengtheningStone>(), 9));
            }
            if (Level ==2)
            {
                Item item = new Item(ModContent.ItemType<凝胶背包>(), 1);
                item.SetDefaults(ModContent.ItemType<凝胶背包>());
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[0] = new Item(ModContent.ItemType<白切精华>(), 1);
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[1] = new Item(188, 10);
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[2] = new Item(289, 5);
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[3] = new Item(290, 5);
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[4] = new Item(292, 5);
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[4] = new Item(ModContent.ItemType<玉米种子>(), 3);
                item.GetGlobalItem<BackpackUIItem>().ContainedItem[5] = new Item(ModContent.ItemType<StrengtheningStone2>(), 3);
                items.Add(item);
            }
            if (Level ==3)
            {
                if (WorldGen.crimson)
                {
                    items.Add(new Item(1257, 30));
                    items.Add(new Item(1329, 60));
                }
                else
                {
                    items.Add(new Item(57, 30));
                    items.Add(new Item(86, 60));
                }
                items.Add(new Item(ModContent.ItemType<草莓种子>(), 5));
                items.Add(new Item(ModContent.ItemType<玉净瓶>()));
                items.Add(new Item(227, 10));
                items.Add(new Item(ModContent.ItemType<StrengtheningStone2>(), 9));
            }
            if (Level == 4)
            {
                items.Add(new Item(154, 100));
                items.Add(new Item(ModContent.ItemType<土豆种子>(), 3));
                items.Add(new Item(2326, 10));
                items.Add(new Item(2328, 10));
                items.Add(new Item(4477, 10));
                items.Add(new Item(ModContent.ItemType<StrengtheningStone2>(), 9));
            }
            if (Level == 5)
            {
                items.Add(new Item(499, 10));
                items.Add(new Item(ModContent.ItemType<玉米种子>(), 3));
                items.Add(new Item(2345, 10));
                items.Add(new Item(2347, 10));
                items.Add(new Item(2349, 10));
                items.Add(new Item(ModContent.ItemType<HunyuanPearlUmbrellaItem>()));
                items.Add(new Item(ModContent.ItemType<StrengtheningStone3>(), 3));
            }
            if (Level == 6)
            {
                items.Add(new Item(499, 30));
                items.Add(new Item(ModContent.ItemType<草莓种子>(), 3));
                items.Add(new Item(2346, 10));
                items.Add(new Item(4479, 10));
                items.Add(new Item(ModContent.ItemType<Content.Items.Talisman.药王葫芦>()));
                items.Add(new Item(ModContent.ItemType<StrengtheningStone3>(), 6));
            }
            if (Level == 7)
            {
                items.Add(new Item(3544, 24));
                items.Add(new Item(ModContent.ItemType<土豆种子>(), 3));
                items.Add(new Item(2345, 30));
                items.Add(new Item(2346, 30));
                items.Add(new Item(4479, 30));
                items.Add(new Item(ModContent.ItemType<StrengtheningStone3>(), 9));
            }
            return items;
        }
        //升级需求
        public string LevelDemand(int Level, out bool Demand, out NPC npc,out int NeedLevel)
        {
            //Player player = Main.LocalPlayer;
            //player.GetModPlayer<EntrustPlayer>().Level = 0;
            npc = new NPC();
            npc.SetDefaults(0);
            Demand = true;
            string Max = "";
            if (Level == 0)
            {
                NeedLevel = 5;
                Demand = player.GetModPlayer<EntrustPlayer>().Level >= NeedLevel;
            }
            else
            if (Level == 1)
            {
                NeedLevel = 10;
                Demand = player.GetModPlayer<EntrustPlayer>().Level >= NeedLevel;
            }
            else
            if (Level == 2)
            {
                NeedLevel = 15;
                npc.SetDefaults(50);
                Demand = NPC.downedSlimeKing&& player.GetModPlayer<EntrustPlayer>().Level >= NeedLevel;
            }
            else
            if (Level == 3)
            {
                NeedLevel = 20;
                if (WorldGen.crimson)
                {
                    npc.SetDefaults(266);
                }
                else
                {
                    npc.SetDefaults(13);
                }
                Demand = NPC.downedBoss2 && player.GetModPlayer<EntrustPlayer>().Level >= NeedLevel;
            }
            else
            if (Level == 4)
            {
                NeedLevel = 30;
                npc.SetDefaults(35);
                Demand = NPC.downedBoss3 && player.GetModPlayer<EntrustPlayer>().Level >= NeedLevel;
            }
            else
            if (Level == 5)
            {
                NeedLevel = 30;
                npc.SetDefaults(113);
                Demand = Main.hardMode && player.GetModPlayer<EntrustPlayer>().Level >= NeedLevel;
            }
            else
            if (Level == 6)
            {
                NeedLevel = 40;
                npc.SetDefaults(134);
                Demand = NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && player.GetModPlayer<EntrustPlayer>().Level >= NeedLevel;
            }
            else
            if (Level == 7)
            {
                NeedLevel = 50;
                npc.SetDefaults(262);
                Demand = NPC.downedPlantBoss && player.GetModPlayer<EntrustPlayer>().Level >= NeedLevel;
            }
            else
            {
                Max = " (Max)";
                NeedLevel = -1;
                Demand = false;
            }
            if (player.GetModPlayer<EntrustPlayer>().AdventurerLevel == 0)
            {
                return Language.GetTextValue("Mods.DDmod.Entrust.升级条件2", npc.FullName) + "\n" +
                Language.GetTextValue("Mods.DDmod.Entrust.当前等级") + ": " +
                player.GetModPlayer<EntrustPlayer>().AdventurerLevel + Max + "\n" +
               Language.GetTextValue("Mods.DDmod.Entrust.当前拥有") + ": " +
                 player.GetModPlayer<EntrustPlayer>().Level;
            }
            else
            {
                return Language.GetTextValue("Mods.DDmod.Entrust.升级条件") + "\n" +
                Language.GetTextValue("Mods.DDmod.Entrust.当前等级") + ": " +
                player.GetModPlayer<EntrustPlayer>().AdventurerLevel + Max + "\n" +
               Language.GetTextValue("Mods.DDmod.Entrust.当前拥有") + ": " +
                 player.GetModPlayer<EntrustPlayer>().Level;
            }
        }
    }
    internal class EntrustLevelUI : UIState
    {
        public static bool Visible = false;
        public static Vector2 Location;
        UIImageButton button;
        EntrustLvelePanel UI;
        private UIText text;
        public static int Synthesis;

        public override void OnInitialize()
        {
            
            text = new UIText("0/0", 0.8f);
            text.Width.Set(50, 0f);
            text.Height.Set(50, 0f);
            text.TextColor = new Color(158, 242, 255);
            
            //用tr原版图片实例化一个图片按钮
            UI = new EntrustLvelePanel();
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
