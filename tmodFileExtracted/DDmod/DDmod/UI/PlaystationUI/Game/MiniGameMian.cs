using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Sundries;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Modkey;
using DDmod.ModLinkage.BossChecklist;
using DDmod.Players;
using DDmod.UI.HunterQuests;
using DDmod.UI.ItemUI;
using DDmod.UI.ItemUI.背包;
using DDmod.UI.PlaystationUI.Game.MeteorsPlane;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using Terraria.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using static DDmod.Players.DDPlayer;

namespace DDmod.UI.PlaystationUI.Game
{
    public class MiniGameMian
    {
        public bool Active;
        //我是哪一款游戏
        public int GameType;
        //开始游戏
        public bool Start;
        //暂停游戏
        public bool Timeout;
        public MeteorsPlayer[] player = new MeteorsPlayer[255];
        public MeteorsProj[] proj;
        public MeteorsNPC[] npc;
        public MeteorsDust[] dust;
        public MeteorsGore[] gore;
        public string Text = "DDmod/UI/PlaystationUI/Game_";
        public static Asset<Texture2D>[] Texture;
        public MiniGameMian()
        {
            GameType = 0;
            Load(GameType);
        }
        public void Load(int GameType)
        {
            proj = new MeteorsProj[1000];
            for (int a = 0; a < proj.Length; a++)
            {
                proj[a] = new MeteorsProj(GameType);
            }
            npc = new MeteorsNPC[200];
            for (int a = 0; a < npc.Length; a++)
            {
                npc[a] = new MeteorsNPC(GameType);
            }
            dust = new MeteorsDust[2000];
            for (int a = 0; a < dust.Length; a++)
            {
                dust[a] = new MeteorsDust(GameType);
            }
            gore = new MeteorsGore[400];
            for (int a = 0; a < gore.Length; a++)
            {
                gore[a] = new MeteorsGore(GameType);
            }
            int T = 1;
            Texture = new Asset<Texture2D>[T + 1];
            for (int a = 0; a < Texture.Length; a++)
            {
                Texture[a] = ModContent.Request<Texture2D>(Text + a);
            }

        }
        public void CheckActive()
        {
            if (!Start)
            {
                Timeout = false;
            }
            Active = false;
            for (int a = 0; a < 255; a++)
            {
                if (Main.player[a].active && Main.player[a].GetModPlayer<PlaystationPlayer>().Start == GameType)
                {
                    Active = true;
                }
            }
            if (!Active)
            {
                Timeout = true;
            }
        }
        public void UpdateProj()
        {
            if (!Timeout)
            {
                for (int a = 0; a < proj.Length; a++)
                {
                    if (proj[a] != null && proj[a].Active)
                    {
                        proj[a].Update();
                    }
                }
            }
        }
        public void UpdateNPC()
        {
            if (!Timeout)
            {
                for (int a = 0; a < npc.Length; a++)
                {
                    if (npc[a] != null && npc[a].Active)
                    {
                        npc[a].Update();
                    }
                }
            }
        }
        public void UpdateGore()
        {
            if (!Timeout)
            {
                for (int a = 0; a < gore.Length; a++)
                {
                    if (gore[a] != null && gore[a].active)
                    {
                        gore[a].Update();
                    }
                }
            }
        }
        public void UpdateDust()
        {
            if (!Timeout)
            {
                for (int a = 0; a < dust.Length; a++)
                {
                    if (dust[a] != null && dust[a].active)
                    {
                        dust[a].Update(a);
                    }
                }
            }
        }
        public virtual void Update()
        {
        }
        public virtual void ControlPlayer(Player Player)
        {
            T++;
            if (PlaystationSystem.Playstation[GameType].player[Player.whoAmI] == null)
            {
                PlaystationSystem.Playstation[GameType].player[Player.whoAmI] = new MeteorsPlayer(GameType);
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
                    Player.GetModPlayer<PlaystationPlayer>().Start = -1;
                    Player.GetModPlayer<PlaystationPlayer>().StartCDTime = 10;
                    Player.GetModPlayer<PlaystationPlayer>().point = Point16.Zero;
                    DDmod.SyncData(DDType.PlayersGame, Player.whoAmI, -1, Player.whoAmI);
                }
            }
            操作(out bool Left, out bool Right, out bool Up, out bool Down);
        }
        int T = 0;
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 ScreenPos)
        {
            int Time;
            Texture2D Background = TextureAssets.Background[0].Value;
            spriteBatch.Draw(Background, ScreenPos - new Vector2(50), null, Color.White, 0, Vector2.Zero, new Vector2(10, 0.4F), 0, 0);
            Background = TextureAssets.Background[7].Value;
            Main.instance.LoadBackground(7);
            Time = (T / 4) % Background.Width;
            spriteBatch.Draw(Background, ScreenPos - new Vector2(50, -100 - Background.Height) + new Vector2(-Time + 0, 0), new Rectangle(0, 0, Background.Width, Background.Height), Color.White, 0, new Vector2(0, Background.Height), new Vector2(1), 0, 0);
            spriteBatch.Draw(Background, ScreenPos - new Vector2(50, -100 - Background.Height) + new Vector2(-Time + Background.Width, 0), new Rectangle(0, 0, Background.Width, Background.Height), Color.White, 0, new Vector2(0, Background.Height), new Vector2(1), 0, 0);
            Background = TextureAssets.Background[92].Value;
            Main.instance.LoadBackground(92);
            Time = (T / 2) % Background.Width;
            spriteBatch.Draw(Background, ScreenPos + new Vector2(-50, 100 + Background.Height) + new Vector2(-Time + 0, 0), new Rectangle(0, 0, Background.Width, Background.Height), Color.White, 0, new Vector2(0, Background.Height), new Vector2(1), 0, 0);
            spriteBatch.Draw(Background, ScreenPos + new Vector2(-50, 100 + Background.Height) + new Vector2(-Time + Background.Width, 0), new Rectangle(0, 0, Background.Width, Background.Height), Color.White, 0, new Vector2(0, Background.Height), new Vector2(1), 0, 0);
            Background = TextureAssets.Background[279].Value;
            Main.instance.LoadBackground(279);
            Time = (T) % Background.Width;
            spriteBatch.Draw(Background, ScreenPos + new Vector2(-50, 200 + Background.Height) + new Vector2(-Time + 0, 0), new Rectangle(0, 0, Background.Width, Background.Height), Color.White, 0, new Vector2(0, Background.Height), new Vector2(1), 0, 0);
            spriteBatch.Draw(Background, ScreenPos + new Vector2(-50, 200 + Background.Height) + new Vector2(-Time + Background.Width, 0), new Rectangle(0, 0, Background.Width, Background.Height), Color.White, 0, new Vector2(0, Background.Height), new Vector2(1), 0, 0);
            Main.spriteBatch.End();
            RasterizerState state = new RasterizerState()
            {
                ScissorTestEnable = true,
            };

            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, state, null, Main.UIScaleMatrix);
            Vector2 vector = ScreenPos + new Vector2(40, 20);
            for (int a = 1; a < Texture.Length; a++)
            {
                spriteBatch.Draw(Texture[a].Value, vector, null, Color.White, 0, Vector2.Zero, new Vector2(1F), 0, 0);
                if (new Rectangle(Main.mouseX, Main.mouseY, 1, 1).Intersects(new Rectangle((int)vector.X, (int)vector.Y, Texture[a].Width(), Texture[a].Height())))
                {
                    spriteBatch.Draw(Texture[0].Value, vector, null, new Color(255, 255, 0), 0, Vector2.Zero, new Vector2(1F), 0, 0);
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                        Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().Start = a;
                }

                string Name = "王牌星战";
                float Nsc = 1;
                if (ChatManager.GetStringSize(FontAssets.MouseText.Value, Name, new Vector2(1)).X > 100)
                {
                    Nsc = 100 / ChatManager.GetStringSize(FontAssets.MouseText.Value, Name, new Vector2(1)).X;
                }
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Name, vector.X + Texture[a].Width() / 2, vector.Y + 76, Color.White, Color.Black, new Vector2(ChatManager.GetStringSize(FontAssets.MouseText.Value, Name, new Vector2(1)).X / 2, 0), Nsc);

            }
        }
    }
}