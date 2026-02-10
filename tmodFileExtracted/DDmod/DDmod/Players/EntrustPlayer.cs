using Terraria;
using Terraria.ModLoader.IO;
using static DDmod.UI.HunterQuests.EntrustPanel;
using DDmod.UI.HunterQuests;
using static DDmod.Helper.DDHelper;
using Terraria.UI;
using System.Collections;
using Terraria.GameInput;
using Terraria.GameContent.UI.Elements;
using DDmod.UI;
using DDmod.Content.Items.Series.Venture;
using DDmod.Content.Projectiles.Talisman;
using log4net.Core;
using System.Linq.Expressions;
using DDmod.NoContent.Config;
using DDmod.Worlds;
using System.Linq;
using DDmod.Modkey;

namespace DDmod.Players
{

    public class EntrustTextUI : UIState
    {
        public static bool Visible
        {
            get
            {

                return ModContent.GetInstance<DDConfigServer>().AdventureSlime;
            }
        }

        public static bool mouse;
        private UIElement _basePanel; // 背景板
        public UIScrollbar Scrollbar; // 拖动条
        public DUIList UIList; // 明细列表
        public UIText text; // 明细列表

        public override void OnInitialize()
        {
            _basePanel = new EntrustTextPanel();
            _basePanel.Left.Set(-0, 0f);
            _basePanel.Top.Set(400, 0f);
            _basePanel.Width.Set(300, 0f);
            _basePanel.Height.Set(400, 0f);
            Append(_basePanel);


            UIList = new DUIList
            {
                Width = new(-28f, 1f),
                Height = new(0, 1f),
                ListPadding = 4f,
            };
            _basePanel.Append(UIList);

            Scrollbar = new UIScrollbar
            {
                HAlign = 1f,
                Height = new(0, 1f)
            };
            Scrollbar.SetView(100f, 1000f);
            SetupScrollBar();
            _basePanel.Append(Scrollbar);
        }

        private void SetupScrollBar(bool resetViewPosition = true)
        {
            float height = UIList.GetInnerDimensions().Height;
            Scrollbar.SetView(height, UIList.GetTotalHeight());
        }

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
            if (_basePanel.GetOuterDimensions().ToRectangle().Contains(evt.MousePosition.ToPoint()))
                Scrollbar.ViewPosition -= evt.ScrollWheelValue;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            if (!Visible)
            {
                return;
            }

            if (Scrollbar.IsMouseHovering) // 不知道为啥默认没有
                Main.LocalPlayer.mouseInterface = true;

            SetupList();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            _basePanel.Top.Set(Main.screenHeight / 2 - 200, 0f);
            _basePanel.Append(UIList);
            if (Scrollbar is not null)
            {
                //UIList.ViewPosition=-Scrollbar.ViewPosition;
                UIList._innerList.Top.Set(-Scrollbar.ViewPosition, 0f);
                UIList.Height.Pixels = 100;
            }
            UIList.Recalculate();

            base.DrawSelf(spriteBatch);
        }

        public void SetupList()
        {
            UIList.Clear();
            int T = 0;
            //UIList.Add(new EntrustText("主线任务",1.25F, new Color(255, 255, 0)) { Width = StyleDimension.FromPixels(100), Height = StyleDimension.FromPixels(27f) });
            //UIList.Add(new EntrustText("击败史莱姆王") { Width = StyleDimension.FromPixels(100), Height = StyleDimension.FromPixels(27f) });

            UIList.Add(new EntrustText(Language.GetTextValue("Mods.DDmod.Entrust.每日委托"), 1.25F, new Color(0, 255, 0)) { Width = StyleDimension.FromPixels(100), Height = StyleDimension.FromPixels(27f) });

            EntrustPlayer EntrustPlayer = Main.LocalPlayer.GetModPlayer<EntrustPlayer>();
            for (int a = 0; a < EntrustPlayer.entrust.Length; a++)
            {
                if (EntrustPlayer.entrust[a] == null)
                    EntrustPlayer.entrust[a] = new Entrust();
                if (EntrustPlayer.entrust[a].Accept)
                {
                    if (EntrustPlayer.entrust[a].type == EntrustID.收集任务)
                    {
                        T++;
                        int s = 0;
                        for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
                        {
                            if (Main.LocalPlayer.inventory[i].type == EntrustPlayer.entrust[a].EntrustItem.type)
                            {
                                s += Main.LocalPlayer.inventory[i].stack;
                            }
                        }
                        string Dtext = T + "." + Language.GetTextValue("Mods.DDmod.Entrust.收集") + ": " + EntrustPlayer.entrust[a].EntrustItem.Name + "X" + EntrustPlayer.entrust[a].EntrustItem.stack +
                            "\n" + Language.GetTextValue("Mods.DDmod.Entrust.已拥有") + ":" + s;
                        DynamicSpriteFont font = FontAssets.MouseText.Value;
                        TextDisplayCache textDisplay = new TextDisplayCache();
                        textDisplay.PrepareCache(Dtext, font, 300);
                        string[] textLines = textDisplay.TextLines;
                        int amountOfLines = textDisplay.AmountOfLines + 1;
                        UIList.Add(new EntrustText(Dtext) { Width = StyleDimension.FromPixels(100), Height = StyleDimension.FromPixels(27f * amountOfLines) });
                    }
                    if (EntrustPlayer.entrust[a].type == EntrustID.战斗任务)
                    {
                        T++;
                        if (EntrustPlayer.entrust[a].EntrustNPC != null)
                        {
                            string Name = EntrustPlayer.entrust[a].EntrustNPC.FullName;
                            NPC[] npc = new NPC[3] { new NPC(), new NPC(), new NPC(), };
                            if (EntrustPlayer.entrust[a].EntrustNPC.type == 1)
                            {
                                Name = Language.GetTextValue("Mods.DDmod.Entrust.史莱姆");
                            }
                            if (EntrustPlayer.entrust[a].EntrustNPC.type == 63)
                            {
                                Name = Language.GetTextValue("Mods.DDmod.Entrust.水母");
                            }
                            if (EntrustPlayer.entrust[a].EntrustNPC.type == 635)
                            {
                                npc[0].SetDefaults(635);
                                npc[1].SetDefaults(254);
                                Name = Language.GetTextValue("Mods.DDmod.Entrust.或", npc[0].FullName, npc[1].FullName);
                            }
                            string Dtext = T + "." + Language.GetTextValue("Mods.DDmod.Entrust.击杀") + ": " + Name + "X" + EntrustPlayer.entrust[a].MaxEntrustNPCStack +
                                "\n" + Language.GetTextValue("Mods.DDmod.Entrust.已经击杀") + ":" + EntrustPlayer.entrust[a].EntrustNPCStack;
                            DynamicSpriteFont font = FontAssets.MouseText.Value;
                            TextDisplayCache textDisplay = new TextDisplayCache();
                            textDisplay.PrepareCache(Dtext, font, 300);
                            string[] textLines = textDisplay.TextLines;
                            int amountOfLines = textDisplay.AmountOfLines + 1;
                            UIList.Add(new EntrustText(Dtext) { Width = StyleDimension.FromPixels(100), Height = StyleDimension.FromPixels(27f * amountOfLines) });
                        }
                    }
                    if (EntrustPlayer.entrust[a].type == EntrustID.任务失败)
                    {
                        T++;
                        string Dtext = T + "." + Language.GetTextValue("Mods.DDmod.Entrust.失败");
                        DynamicSpriteFont font = FontAssets.MouseText.Value;
                        TextDisplayCache textDisplay = new TextDisplayCache();
                        textDisplay.PrepareCache(Dtext, font, 300);
                        string[] textLines = textDisplay.TextLines;
                        int amountOfLines = textDisplay.AmountOfLines + 1;
                        UIList.Add(new EntrustText(Dtext) { Width = StyleDimension.FromPixels(100), Height = StyleDimension.FromPixels(27f * amountOfLines) });
                    }
                }
            }
            if (T == 0)
            {

                UIList.Add(new EntrustText(Language.GetTextValue("Mods.DDmod.Entrust.无")) { Width = StyleDimension.FromPixels(100), Height = StyleDimension.FromPixels(27f) });
            }
            Recalculate();
            SetupScrollBar();
        }
    }

    public class EntrustTextPanel : UIElement
    {
        public static bool R = true;
        int X = 0;
        public EntrustTextPanel()
        {
        }
        bool M;
        private void DrawPanel(SpriteBatch spriteBatch)
        {
            Left.Pixels = X;
            Point position = new Point((int)GetInnerDimensions().X, (int)GetInnerDimensions().Y);
            Point point = new Point((int)GetInnerDimensions().Width, (int)GetInnerDimensions().Height);
            Point 切换按钮位置 = new Point((int)position.X + point.X - 2, (int)position.Y + point.Y / 2) - new Point(0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/切换").Height() / 2);
            Color color = Color.White * 0.6F;
            if (X == -300)
            {
                color = Color.White;
            }
                EntrustTextUI.mouse = false;
            if (new Rectangle(切换按钮位置.X, 切换按钮位置.Y - 22, 24, 44).Intersects(new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1)) || ModkeySetup.CommissionKey.JustPressed)
            {
                if(!ModkeySetup.CommissionKey.JustPressed)
                EntrustTextUI.mouse = true;
                if (Main.mouseLeft)
                {
                    M = true;
                }
                if ((M && !Main.mouseLeft)|| ModkeySetup.CommissionKey.JustPressed)
                {
                    if (Main.LocalPlayer.Dplayer().EntrustTextPanelUI)
                    {
                        if (X >= 0)
                        {
                            Main.LocalPlayer.Dplayer().EntrustTextPanelUI = !Main.LocalPlayer.Dplayer().EntrustTextPanelUI;
                        }
                    }
                    else
                    {
                        if (X <= -300)
                        {
                            Main.LocalPlayer.Dplayer().EntrustTextPanelUI = !Main.LocalPlayer.Dplayer().EntrustTextPanelUI;
                        }
                    }
                }
            }
            else
            {
                M = false;
            }
            R = Main.LocalPlayer.Dplayer().EntrustTextPanelUI;
            if (new Rectangle(position.X, position.Y, point.X, point.Y).Intersects(new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1)))
            {
                EntrustTextUI.mouse = true;
            }

            if(EntrustTextUI.mouse)
            {
                Main.LocalPlayer.mouseInterface = true;
                color = Color.White;
            }
            if (X > -300)
            {
                DrawAdvBox(spriteBatch, position.X, position.Y-32, point.X, point.Y+32, color, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/任务框").Value, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/任务框").Size() / 2, 1);
            }
            spriteBatch.Draw(DDTextures.WhitePng.Value, new Vector2(position.X, position.Y - 8), null, color, 0, Vector2.Zero, new Vector2(point.X / 2, 2), 0, 0);
            if (R)
            {
                if (X <0)
                {
                    X+=30;
                }
                else
                {
                    X = 0;
                }
                spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/切换2").Value, new Vector2(切换按钮位置.X, 切换按钮位置.Y), null, color, 0, new Vector2(0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/切换").Height() / 2), 1, 0, 0);

            }
            else
            {
                if (ModkeySetup.CommissionKey.GetAssignedKeys(InputMode.Keyboard).Count == 0)
                {
                    if (X > -300)
                    {
                        X -= 30;
                    }
                    else
                    {
                        X = -300;
                    }
                }
                else
                {
                    if (X > -400)
                    {
                        X -= 30;
                    }
                }
                spriteBatch.Draw(ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/切换").Value, new Vector2(切换按钮位置.X, 切换按钮位置.Y), null, color, 0, new Vector2(0, ModContent.Request<Texture2D>("DDmod/UI/HunterQuests/切换").Height() / 2), 1, 0, 0);

            }
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            TextDisplayCache textDisplay = new TextDisplayCache();
            ChatManager.DrawColorCodedStringWithShadow(
                spriteBatch,
                font, Language.GetTextValue("Mods.DDmod.Entrust.任务追踪"), new Vector2(position.X, position.Y - 30), color, 0, Vector2.Zero, new Vector2(1), 200);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
                DrawPanel(spriteBatch);
        }
        public void DrawAdvBox(SpriteBatch sp, int x, int y, int w, int h, Color c, Texture2D img, Vector2 size4, float scale = 1f)
        {
            var box = img;
            var nw = (int)(w * scale);
            var nh = (int)(h * scale);
            x += (w - nw) / 2;
            y += (h - nh) / 2;
            w = nw;
            h = nh;
            var width = (int)size4.X;
            var height = (int)size4.Y;
            if (w < size4.X)
            {
                w = width;
            }
            if (h < size4.Y)
            {
                h = width;
            }
            sp.Draw(box, new Rectangle(x, y, width, height), new Rectangle(0, 0, width, height), c);
            sp.Draw(box, new Rectangle(x + width, y, w - width * 2, height), new Rectangle(width, 0, box.Width - width * 2, height), c);
            sp.Draw(box, new Rectangle((x + w) - width, y, width, height), new Rectangle(box.Width - width, 0, width, height), c);
            sp.Draw(box, new Rectangle(x, y + height, width, h - height * 2), new Rectangle(0, height, width, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle(x + width, y + height, w - width * 2, h - height * 2), new Rectangle(width, height, box.Width - width * 2, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle((x + w) - width, y + height, width, h - height * 2), new Rectangle(box.Width - width, height, width, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle(x, (y + h) - height, width, height), new Rectangle(0, box.Height - height, width, height), c);
            sp.Draw(box, new Rectangle(x + width, (y + h) - height, w - width * 2, height), new Rectangle(width, box.Height - height, box.Width - width * 2, height), c);
            sp.Draw(box, new Rectangle((x + w) - width, (y + h) - height, width, height), new Rectangle(box.Width - width, box.Height - height, width, height), c);
        }
    }
    class EntrustText  : UIElement
    {

        string Text ;
        float scale;
        Color Co;
        public EntrustText(string DText,float Sc=1,Color color = default)
        {
            Text = DText;
            scale = Sc;
            Co = color;
            if (color == default)
            {
                Co = Color.White;
            }
        }
        
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Point position = new Point((int)GetInnerDimensions().X, (int)GetInnerDimensions().Y);
            Color color = Co * 0.6F;
            if (EntrustTextUI.mouse)
            {
                color = Co;
            }
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            TextDisplayCache textDisplay = new TextDisplayCache();
            textDisplay.PrepareCache(Text, font, 300);
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
                Vector2 vector = new Vector2(0, 27 * (A));
                ChatManager.DrawColorCodedStringWithShadow(
                    spriteBatch,
                    font, textLines[A], new Vector2(position.X, position.Y)+ vector, color, 0, Vector2.Zero, new Vector2(scale), 300);
            }
        }
        public void DrawAdvBox(SpriteBatch sp, int x, int y, int w, int h, Color c, Texture2D img, Vector2 size4, float scale = 1f)
        {
            var box = img;
            var nw = (int)(w * scale);
            var nh = (int)(h * scale);
            x += (w - nw) / 2;
            y += (h - nh) / 2;
            w = nw;
            h = nh;
            var width = (int)size4.X;
            var height = (int)size4.Y;
            if (w < size4.X)
            {
                w = width;
            }
            if (h < size4.Y)
            {
                h = width;
            }
            sp.Draw(box, new Rectangle(x, y, width, height), new Rectangle(0, 0, width, height), c);
            sp.Draw(box, new Rectangle(x + width, y, w - width * 2, height), new Rectangle(width, 0, box.Width - width * 2, height), c);
            sp.Draw(box, new Rectangle((x + w) - width, y, width, height), new Rectangle(box.Width - width, 0, width, height), c);
            sp.Draw(box, new Rectangle(x, y + height, width, h - height * 2), new Rectangle(0, height, width, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle(x + width, y + height, w - width * 2, h - height * 2), new Rectangle(width, height, box.Width - width * 2, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle((x + w) - width, y + height, width, h - height * 2), new Rectangle(box.Width - width, height, width, box.Height - height * 2), c);
            sp.Draw(box, new Rectangle(x, (y + h) - height, width, height), new Rectangle(0, box.Height - height, width, height), c);
            sp.Draw(box, new Rectangle(x + width, (y + h) - height, w - width * 2, height), new Rectangle(width, box.Height - height, box.Width - width * 2, height), c);
            sp.Draw(box, new Rectangle((x + w) - width, (y + h) - height, width, height), new Rectangle(box.Width - width, box.Height - height, width, height), c);
        }
    }
    public class EntrustPlayer : ModPlayer
    {
        /// <summary>
        /// 委托
        /// </summary>
        public Entrust[] entrust = new Entrust[10];
        public static Entrust[] entrusts;
        public List<int> EntrustNPCTypes { get; set; } = new List<int>();

        public void AddEntrust(int newEntrust)
        {
            return;
            EntrustNPCTypes.Add(newEntrust);
            DDmod.SyncData(DDType.PlayersEntrust, Player.whoAmI, -1, Player.whoAmI);
        }
        public void RemoveEntrust(int newEntrust)
        {
            return;
            EntrustNPCTypes.Remove(newEntrust);
            DDmod.SyncData(DDType.PlayersEntrust, Player.whoAmI, -1, Player.whoAmI);

        }
        /// <summary>
        /// 最大委托任务数量
        /// </summary>
        public int MaxEntrust = 5;
        /// <summary>
        /// 冒险等级
        /// </summary>
        public int AdventurerLevel;

        /// <summary>
        /// 玩家等级
        /// </summary>
        public int Level = 0;
        public int MaxLevel=>120;
        public int PreLevel;
        /// <summary>
        /// 经验
        /// </summary>
        public int Experience;
        public int MaxExperience;
        public bool mouse;
        public bool mouseX;
        public bool EnterWorld;

        public Vector2 UIPo;
        public override void Load()
        {
            if (Main.LocalPlayer == Player)
            {
                if(entrust.Length<10)
                {
                    entrust = new Entrust[10];
                }
                for (int r = 0; r < entrust.Length; r++)
                {
                    if (entrust[r] == null)
                        entrust[r] = new Entrust();
                }
            }
        }
        public override void PreUpdate()
        {
            if (Main.LocalPlayer == Player)
            {
                if (entrust.Length < 10)
                {
                    entrust = new Entrust[10];
                }
                for (int r = 0; r < entrust.Length; r++)
                {
                    if (entrust[r] == null)
                        entrust[r] = new Entrust();
                    entrust[r].Update();
                }
            }
            while (npc.Count > 0)
            {
                if (npc[0].SpawnedFromStatue)
                {
                    npc.RemoveAt(0);
                    break;
                }
                for (int I = 0; I < entrust.Length; I++)
                {
                    if (entrust[I] !=null&& entrust[I].EntrustNPC != null && entrust[I].Accept && entrust[I].EntrustNPCStack < entrust[I].MaxEntrustNPCStack && entrust[I].EntrustNPCType(npc[0]) == entrust[I].EntrustNPC.type)
                    {
                        entrust[I].EntrustNPCStack++;
                        if (Player.whoAmI == Main.myPlayer && entrust[I].EntrustNPCStack == entrust[I].MaxEntrustNPCStack)
                        {
                            string Name = entrust[I].EntrustNPC.FullName;
                            NPC[] npc = new NPC[3] { new NPC(), new NPC(), new NPC(), };
                            if (entrust[I].EntrustNPC.type == 1)
                            {
                                Name = Language.GetTextValue("Mods.DDmod.Entrust.史莱姆");
                            }
                            if (entrust[I].EntrustNPC.type == 63)
                            {
                                Name = Language.GetTextValue("Mods.DDmod.Entrust.水母");
                            }
                            if (entrust[I].EntrustNPC.type == 635)
                            {
                                npc[0].SetDefaults(635);
                                npc[1].SetDefaults(254);
                                Name = npc[0].FullName + Language.GetTextValue("Mods.DDmod.Entrust.或") + npc[1].FullName;
                            }
                            string Dtext = Language.GetTextValue("Mods.DDmod.Entrust.每日委托") + ":" + Language.GetTextValue("Mods.DDmod.Entrust.击杀") + Name + "X" + entrust[I].MaxEntrustNPCStack + Language.GetTextValue("Mods.DDmod.Entrust.已完成");
                            Main.NewText(Dtext, new Color(100, 255, 100));
                            RemoveEntrust(entrust[I].EntrustNPC.type);
                        }
                        break;
                    }
                }
                npc.RemoveAt(0);
            }
        }
        public List<NPC> npc = new List<NPC>();
        public override void UpdateEquips()
        {
            Player.statLifeMax2 += Level * 2;
        }
        public override void OnEnterWorld()
        {
            EnterWorld = true;
        }
        public override void PostUpdateEquips()
        {
            EnterWorld = true;
            if (this.Level < 0)
            {
                this.Level = 0;
            }
            if (this.Level > MaxLevel)
            {
                this.Level = MaxLevel;
            }
            if(Level!= DeveloperMode.Instance.Level && DeveloperMode.Instance.Level >=0)
            {
                if (DDWorld.开发者模式)
                {
                    Experience = 0;
                    Level = DeveloperMode.Instance.Level;
                    UP(Level - 1);
                }
                DeveloperMode.Instance.Level = -1;
            }
            if (!DeveloperMode.Instance.lockLevel)
            {
                if (Experience >= MaxExperience || PreLevel != Level)
                {
                    UP(Level);
                }
            }
            else
            {
                if (!DDWorld.开发者模式)
                {
                    DeveloperMode.Instance.lockLevel = false;
                }
            }
        }
        //升级经验
        public void UP(int Level)
        {
            if (Level == 0)
            {
                MaxExperience = 20;
            }
            else
            {
                MaxExperience = 0;
            }
            int EXP = 50;
            for (int a = 0; a <= MaxLevel; a++)
            {
                if (a < Level)
                {
                    MaxExperience += EXP;

                    if (a < 50)
                    {
                        EXP = (int)(EXP * 1.1f);
                    }
                    else
                    {
                        EXP = (int)(EXP * 1.05f);
                    }
                }
                if (a == 5)
                {
                    EXP = 100;
                }
                if (a == 10)
                {
                    EXP = 200;
                }
                if (a == 20)
                {
                    EXP = 600;
                }
                if (a == 30)
                {
                    EXP = 1800;
                }
                if (a == 40)
                {
                    EXP = 5400;
                }
                if (a == 50)
                {
                    EXP = 16200;
                }
            }
            if (Level >= MaxLevel)
            {
                Experience = 1;
                MaxExperience = 1;
            }
            else if (Player == Main.LocalPlayer && MaxExperience > 0 && Experience >= MaxExperience)
            {
                Experience -= MaxExperience;
                for (int a = 0; a < 100; a++)
                {
                    Vector2 vector = new Vector2(Main.rand.NextFloat(-53, 53), Main.rand.NextFloat(-18, 18));
                    Vector2 vector2 = new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-6, 2));
                    UIDustDraw.NewDust(UIPo + vector, 4, new Color(255, 188, 35, 0), vector2, 0.4F);
                }
                NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<UPProj>(), 0, 0, -1);
                this.Level++;
                PreLevel = this.Level;
                UP(this.Level);
                if (Player.whoAmI == Main.myPlayer)
                    DDmod.SyncData(DDType.PlayerData, Player.whoAmI, -1, Player.whoAmI);
            }
        }
        public override void PreSavePlayer()
        {
            base.PreSavePlayer();
        }
        public override void SaveData(TagCompound tag)
        {
            for (int r = 0; r < entrust.Length; r++)
            {
                if (entrust[r] == null)
                    entrust[r] = new Entrust();
                entrust[r].SaveData(r, tag);
            }
            tag.Add("PlayerLevel", Level);
            tag.Add("PlayerMaxLevel", PreLevel);
            tag.Add("PlayerExperience", Experience);
            tag.Add("PlayerMaxExperience", MaxExperience);
            tag.Add("UIPo", UIPo);


            tag.Add("EntrustLevel", AdventurerLevel);
            EnterWorld = false;
        }
        public override void LoadData(TagCompound tag)
        {
            EntrustNPCTypes.Clear();
            entrust = new Entrust[10];
            for (int r = 0; r < entrust.Length; r++)
            {
                if (entrust[r] == null)
                    entrust[r] = new Entrust();
                entrust[r].LoadData(r, tag);
                if (entrust[r].Accept && entrust[r].EntrustNPCStack < entrust[r].MaxEntrustNPCStack)
                {
                    AddEntrust(entrust[r].EntrustNPC.type);
                }
            }
            Level = tag.GetInt("PlayerLevel");
            PreLevel = tag.GetInt("PlayerMaxLevel");
            Experience = tag.GetInt("PlayerExperience");
            MaxExperience = tag.GetInt("PlayerMaxExperience");
            UIPo = tag.Get<Vector2>("UIPo");
            AdventurerLevel = tag.GetInt("EntrustLevel");
            EnterWorld = false;
        }
        public override void ModifyScreenPosition()
        {
        }
        public void Sync()
        {
            if (Main.netMode == 1)
            {
                Mod mod = DDmod.Instance;
                ModPacket packet = mod.GetPacket(256);
                packet.Write((byte)DDType.PlayersEntrust);
                packet.Write(Player.whoAmI);
                packet.Write(EntrustNPCTypes.Count);
                for(int A=0;A< EntrustNPCTypes.Count;A++)
                {
                    packet.Write(EntrustNPCTypes[A]);
                }
                packet.Send(-1, Player.whoAmI);
            }
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
        }
        public override void ModifyDrawLayerOrdering(IDictionary<PlayerDrawLayer, PlayerDrawLayer.Position> positions)
        {
            base.ModifyDrawLayerOrdering(positions);
        }
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
        }
    }
}