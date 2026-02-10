using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Sundries;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using Microsoft.CodeAnalysis.Differencing;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using static DDmod.Players.DDPlayer;

namespace DDmod.UI.PlaystationUI.Game.MeteorsPlane
{
    public class Star
    {
        public Vector2 Position;
        public Vector2 Scale;
        public Color color;
        public Star(Vector2 Position, Vector2 Scale, Color color)
        {
            this.Position = Position;
            this.Scale = Scale;
            this.color = color;
        }
    }
    public class MeteorsPlaneMian : MiniGameMian
    {
        public List<Star> stars;
        //死亡倒计时
        public int KillTime = 0;
        public Vector2 ScreenSize = new Vector2(364, 342);
        public MeteorsPlaneMian(int GameType)
        {
            this.GameType = GameType;
            Load(GameType);
            StarLaod();
        }
        public void StarLaod()
        {
            stars = new List<Star>();
            stars.Add(new Star(new Vector2(78, 50), new Vector2(1), new Color(255, 255, 255, 155)));
            stars.Add(new Star(new Vector2(166, 370), new Vector2(0.8F), new Color(255, 255, 255, 55)));
            stars.Add(new Star(new Vector2(307, 247), new Vector2(0.5F), new Color(255, 255, 255, 205)));
            stars.Add(new Star(new Vector2(68, 254), new Vector2(0.6F), new Color(255, 255, 255, 120)));
            stars.Add(new Star(new Vector2(116, 46), new Vector2(0.65F), new Color(255, 255, 255, 85)));
            stars.Add(new Star(new Vector2(114, 348), new Vector2(0.78F), new Color(255, 255, 255, 145)));
            stars.Add(new Star(new Vector2(233, 329), new Vector2(0.54F), new Color(255, 255, 255, 235)));
            stars.Add(new Star(new Vector2(276, 62), new Vector2(0.45F), new Color(255, 255, 255, 125)));
            stars.Add(new Star(new Vector2(168, 121), new Vector2(0.36F), new Color(255, 255, 255, 185)));
            stars.Add(new Star(new Vector2(190, 19), new Vector2(0.46F), new Color(255, 255, 255, 35)));
            stars.Add(new Star(new Vector2(113, 112), new Vector2(0.34F), new Color(255, 255, 255, 15)));
            stars.Add(new Star(new Vector2(317, 94), new Vector2(0.75F), new Color(255, 255, 255, 75)));
            stars.Add(new Star(new Vector2(237, 287), new Vector2(0.86F), new Color(255, 255, 255, 95)));
            stars.Add(new Star(new Vector2(113, 355), new Vector2(0.94F), new Color(255, 255, 255, 35)));
            stars.Add(new Star(new Vector2(230, 220), new Vector2(0.24F), new Color(255, 255, 255, 175)));
            stars.Add(new Star(new Vector2(182, 393), new Vector2(0.53F), new Color(255, 255, 255, 205)));
            stars.Add(new Star(new Vector2(36, 71), new Vector2(0.64F), new Color(255, 255, 255, 125)));
            stars.Add(new Star(new Vector2(220, 291), new Vector2(0.73F), new Color(255, 255, 255, 175)));
            stars.Add(new Star(new Vector2(147, 205), new Vector2(0.24F), new Color(255, 255, 255, 195)));
            stars.Add(new Star(new Vector2(208, 180), new Vector2(0.64F), new Color(255, 255, 255, 145)));
            stars.Add(new Star(new Vector2(356, 9), new Vector2(0.46F), new Color(255, 255, 255, 75)));
            stars.Add(new Star(new Vector2(21, 138), new Vector2(0.211F), new Color(255, 255, 255, 45)));
            stars.Add(new Star(new Vector2(151, 197), new Vector2(0.254F), new Color(255, 255, 255, 85)));
            stars.Add(new Star(new Vector2(236, 26), new Vector2(0.463F), new Color(255, 255, 255, 35)));
            stars.Add(new Star(new Vector2(297, 402), new Vector2(0.746F), new Color(255, 255, 255, 95)));
            stars.Add(new Star(new Vector2(8, 46), new Vector2(0.463F), new Color(255, 255, 255, 185)));
            stars.Add(new Star(new Vector2(123, 376), new Vector2(0.332F), new Color(255, 255, 255, 235)));
            stars.Add(new Star(new Vector2(338, 247), new Vector2(0.253F), new Color(255, 255, 255, 165)));
            stars.Add(new Star(new Vector2(291, 95), new Vector2(0.457F), new Color(255, 255, 255, 245)));
            stars.Add(new Star(new Vector2(14, 336), new Vector2(0.657F), new Color(255, 255, 255, 195)));
        }
        public int Time;
        public override void Update()
        {
            if (!Main.dedServ) Main.LocalPlayer.Dplayer().Music = DDSystem.Music(0,"王牌星战");
            bool Kill = true;
            if (KillTime>0)
            {
                KillTime--;
                if(KillTime==0)
                {
                    for (int a = 0; a < proj.Length; a++)
                    {
                        if (proj[a].Active)
                        {
                            proj[a].Kill();
                        }
                    }
                    for (int a = 0; a < npc.Length; a++)
                    {
                        if (npc[a].Active)
                        {
                            npc[a].Active = false;
                        }
                    }
                    for (int a = 0; a < 255; a++)
                    {
                        Player player = Main.player[a];
                        if (player != null && player.active)
                        {
                            PlaystationPlayer playstation = player.GetModPlayer<PlaystationPlayer>();
                            if (this.player[a] != null)
                            {
                                MeteorsPlayer Mplayer = this.player[a];
                                Mplayer.MaxLife = 10;
                                Mplayer.Life = Mplayer.MaxLife;
                            }
                        }
                    }
                    Start = false;
                }
            }
            else
            {
                for (int a = 0; a < 255; a++)
                {
                    Player player = Main.player[a];
                    if (player != null && player.active)
                    {
                        PlaystationPlayer playstation = player.GetModPlayer<PlaystationPlayer>();
                        if (this.player[a] != null)
                        {
                            MeteorsPlayer Mplayer = this.player[a];
                            if (Mplayer.Life > 0)
                            {
                                Kill = false;
                            }
                        }
                    }
                }
            }
            if (Kill && KillTime == 0 && Start)
            {
                KillTime = 300;
            }
            if (Main.rand.NextBool(20))
            {
                int GoreType = DDSystem.Instance.Mod.Find<ModGore>("Cloud_" + Main.rand.Next(22)).Type;
                float Sc = Main.rand.NextFloat(0.1F, 0.5F);
                int Post = Main.rand.Next(2);
                if (Post == 1)
                {
                    Sc = Main.rand.NextFloat(0.75F, 1.25F);
                }
                MeteorsGore.NewGore(GameType, new Vector2(Main.rand.NextFloat(40, 524), -200), new Vector2(0, 2) * Sc, GoreType, Sc, Post);
            }
            if (!Timeout && Start)
            {
                Time++; 
                Spawning();
            }
            else
            {
                if (!Start)
                    Time = 0;
            }
        }
        public void Spawning()
        {
            if (Time >= 10)
            {
                MeteorsNPC.NewNPC(GameType, new Vector2(Main.rand.NextFloat(40, 524), 0), new Vector2(0, Main.rand.NextFloat(1, 3)), Main.rand.Next(1, 3));
                Time = 0;
            }
        }
        public override void ControlPlayer(Player Player)
        {
            if (player[Player.whoAmI] == null)
            {
                player[Player.whoAmI] = new MeteorsPlayer(GameType);
            }
            
            void 操作(out bool Left, out bool Right, out bool Up, out bool Down)
            {
                Mini_game game = Player.Dplayer().Mini_game_shortcuts;
                game.Enable = true;
                Left = game.Left;
                Right = game.Right;
                Up = game.Up;
                Down = game.Down;
                if (game.Exit)
                {
                    Player.GetModPlayer<PlaystationPlayer>().Start = 0;
                    Player.GetModPlayer<PlaystationPlayer>().StartCDTime = 10;
                }
                if (!Start && game.Jump)
                {
                    player[Player.whoAmI].Initialize();
                    Start = true;
                }
                if (Timeout && game.Jump)
                {
                    Timeout = false;
                }
            }
            操作(out bool Left, out bool Right, out bool Up, out bool Down);

            if (!Timeout)
            {
                player[Player.whoAmI].控制飞机(Left, Right, Up, Down);
                player[Player.whoAmI].碰撞();
                player[Player.whoAmI].发射();
            }
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 ScreenPos)
        {
            Texture2D Background = TextureAssets.Background[0].Value;
            spriteBatch.Draw(Background, ScreenPos - new Vector2(50), null, new Color(20, 0, 20, 0), 0, Vector2.Zero, new Vector2(10, 0.4F), 0, 0);
            for (int a = 0; a < stars.Count; a++)
            {
                spriteBatch.Draw(DDTextures.VoidStar.Value, ScreenPos + stars[a].Position, null, stars[a].color, 0, DDTextures.VoidStar.Size() / 2, stars[a].Scale * 0.1F, 0, 0);
            }
            Main.spriteBatch.End();
            RasterizerState state = new RasterizerState()
            {
                ScissorTestEnable = true,
            };
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, state, null, Main.UIScaleMatrix);
            for (int a = 0; a < gore.Length; a++)
            {
                if (gore != null && gore[a].active && gore[a].post == 0)
                {
                    gore[a].Dawn(spriteBatch, ScreenPos);
                }
            }
            for (int a = 0; a < 255; a++)
            {
                Player player = Main.player[a];
                if (player != null && player.active)
                {
                    PlaystationPlayer playstation = player.GetModPlayer<PlaystationPlayer>();
                    if (this.player[a] != null && this.player[a].Life > 0)
                    {
                        MeteorsPlayer Mplayer = this.player[a];
                        if (Mplayer.HitTime2 == 0|| Mplayer.HitTime2%6<=3)
                        {
                            NPC npc = playstation.npc;
                            if (npc.type != ModContent.NPCType<MeteorAnnihilator>())
                            {
                                npc.SetDefaults(ModContent.NPCType<MeteorAnnihilator>());
                                npc.scale = 0.5F;
                                npc.width = 10;
                                npc.height = 10;
                            }
                            npc.position = Mplayer.Position + ScreenPos;
                            npc.velocity = Mplayer.velocity;
                            NPCLoader.PreDraw(npc, spriteBatch, new Vector2(0, 4), Color.White);
                            NPCLoader.PostDraw(npc, spriteBatch, new Vector2(0, 4), Color.White);
                            Texture2D VoidStar = DDTextures.GlowEffect.Value;
                            Texture2D Starlight = DDTextures.Starlight.Value;
                            Main.EntitySpriteDraw(Starlight, npc.Center, null, new Color(20, 205, 20, 0) * 0.5F, 0, Starlight.Size() / 2, 0.3F, 0, 0);
                            Main.EntitySpriteDraw(Starlight, npc.Center, null, new Color(235, 50, 235, 0) * 0.5F, 0, Starlight.Size() / 2, 0.15F, 0, 0);
                            Main.EntitySpriteDraw(VoidStar, npc.Center, null, new Color(20, 205, 20, 0) * 0.4F, 0, VoidStar.Size() / 2, 0.15F, 0, 0);
                            Main.EntitySpriteDraw(VoidStar, npc.Center, null, new Color(20, 205, 20, 0) * 0.4F, 0, VoidStar.Size() / 2, 0.15F, 0, 0);
                            Main.EntitySpriteDraw(VoidStar, npc.Center, null, new Color(20, 205, 20, 0) * 0.4F, 0, VoidStar.Size() / 2, 0.15F, 0, 0);
                        }
                    }
                }
            }
            for (int a = 0; a < npc.Length; a++)
            {
                if (npc != null && npc[a].Active)
                {

                    npc[a].Draw(spriteBatch, ScreenPos);
                }
            }
            for (int a = 0; a < proj.Length; a++)
            {
                if (proj != null && proj[a].Active)
                {

                    proj[a].Draw(spriteBatch, ScreenPos);
                }
            }
            for (int a = 0; a < dust.Length; a++)
            {
                if (dust != null && dust[a].active)
                {

                    dust[a].Dawn(spriteBatch, new Rectangle((int)ScreenPos.X, (int)ScreenPos.Y, 0, 0));
                }
            }
            for (int a = 0; a < gore.Length; a++)
            {
                if (gore != null && gore[a].active && gore[a].post == 1)
                {
                    gore[a].Dawn(spriteBatch, ScreenPos);
                }
            }
            spriteBatch.Draw(DDTextures.WhitePng.Value, ScreenPos + new Vector2(0, 320), null, new Color(50, 50, 50, 240), 0, Vector2.Zero, new Vector2(PlaystationSystem.ScreenSize.X / 2, 60), 0, 0);
            if (player[Main.myPlayer].Life<=0)
            {
                string Name = "你的战机已被击落!!!";
                float Nsc = 1;
                if (ChatManager.GetStringSize(FontAssets.DeathText.Value, Name, new Vector2(1)).X > 200)
                {
                    Nsc = 200 / ChatManager.GetStringSize(FontAssets.DeathText.Value, Name, new Vector2(1)).X;
                }

                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.DeathText.Value, Name, ScreenPos.X + PlaystationSystem.ScreenSize.X / 2, ScreenPos.Y + PlaystationSystem.ScreenSize.Y / 2, new Color(255, 0, 0, 0), Color.Black, new Vector2(ChatManager.GetStringSize(FontAssets.DeathText.Value, Name, new Vector2(1)).X / 2, 0), Nsc);

            }
            else
            {
                Vector2 vector= new Vector2(30, 325);
                string Name = "HP:";
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.DeathText.Value, Name, ScreenPos.X + vector.X-14, ScreenPos.Y + vector.Y+2, new Color(255, 255, 255, 255), Color.Black, new Vector2(ChatManager.GetStringSize(FontAssets.DeathText.Value, Name, new Vector2(1)).X / 2, 0), 0.3F);
                for (int a = 0; a < player[Main.myPlayer].Life; a++)
                {
                    spriteBatch.Draw(TextureAssets.Heart.Value, ScreenPos + vector, null, Color.White, 0, Vector2.Zero, new Vector2(1), 0, 0);
                    vector.X += 10;
                } 
                if (!Start && Main.LocalPlayer.Dplayer().PlayerTimes % 30 < 20)
                {
                    string Text = "";
                    if (PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Jump"].Count > 0)
                    {
                        Text = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Jump"][0];
                    }
                    if (!DDSystem.English && Text == "Space")
                    {
                        Text = "空格";
                    }
                    if (Text == "")
                    {
                        Text = Lang.menu[195].Value;
                    }
                    else
                    {
                        Text = "<" + Text + ">";
                    }
                    Name = "-按" + Text + "开始游戏-";
                    float Nsc = 1;
                    if (ChatManager.GetStringSize(FontAssets.DeathText.Value, Name, new Vector2(1)).X > 200)
                    {
                        Nsc = 200 / ChatManager.GetStringSize(FontAssets.DeathText.Value, Name, new Vector2(1)).X;
                    }

                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.DeathText.Value, Name, ScreenPos.X + PlaystationSystem.ScreenSize.X / 2, ScreenPos.Y + PlaystationSystem.ScreenSize.Y / 2, new Color(255, 255, 255, 0), Color.Black, new Vector2(ChatManager.GetStringSize(FontAssets.DeathText.Value, Name, new Vector2(1)).X / 2, 0), Nsc);

                }
                if (Timeout && Main.LocalPlayer.Dplayer().PlayerTimes % 30 < 20)
                {
                    string Text = "";
                    if (PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Jump"].Count > 0)
                    {
                        Text = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Jump"][0];
                    }
                    if (!DDSystem.English && Text == "Space")
                    {
                        Text = "空格";
                    }
                    if (Text == "")
                    {
                        Text = Lang.menu[195].Value;
                    }
                    else
                    {
                        Text = "<" + Text + ">";
                    }
                    Name = "-已暂停,按" + Text + "继续游戏-";
                    float Nsc = 1;
                    if (ChatManager.GetStringSize(FontAssets.DeathText.Value, Name, new Vector2(1)).X > 200)
                    {
                        Nsc = 200 / ChatManager.GetStringSize(FontAssets.DeathText.Value, Name, new Vector2(1)).X;
                    }
                    Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.DeathText.Value, Name, ScreenPos.X + PlaystationSystem.ScreenSize.X / 2, ScreenPos.Y + PlaystationSystem.ScreenSize.Y / 2, new Color(255, 255, 255, 0), Color.Black, new Vector2(ChatManager.GetStringSize(FontAssets.DeathText.Value, Name, new Vector2(1)).X / 2, 0), Nsc);
                }
            }
        }
    }
}