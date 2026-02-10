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
using DDmod.ModLinkage.BossChecklist;
using static DDmod.Players.DDPlayer;
using DDmod.UI.PlaystationUI;
using Humanizer;

namespace DDmod.UI
{

    public class 绿岩测验机UI
    {
        public static bool Visible;
        /// <summary>
        /// 门外竖着
        /// </summary>
        public bool Vertical;
        public bool Start;
        public Asset<Texture2D> texture;
        public Asset<Texture2D> texture2;
        public List<Asset<Texture2D>> assets = new List<Asset<Texture2D>>();
        public Point16 Size = new Point16(23, 14);
        public 绿岩测验机UI()
        {
            Load();
        }

        public void Load()
        {
            texture = ModContent.Request<Texture2D>("DDmod/UI/绿岩测验机UI");
            texture2 = ModContent.Request<Texture2D>("DDmod/UI/豆子");
            assets.Add(ModContent.Request<Texture2D>("DDmod/UI/障碍"));
            assets.Add(ModContent.Request<Texture2D>("DDmod/UI/障碍2"));
            assets.Add(ModContent.Request<Texture2D>("DDmod/UI/障碍3"));
            assets.Add(ModContent.Request<Texture2D>("DDmod/UI/障碍4"));
            assets.Add(ModContent.Request<Texture2D>("DDmod/UI/障碍5"));
            FlushedSnake();
        }
        public Vector2 SnakePosition;
        public List<Vector2> Position = new List<Vector2>();
        public byte[] TrueBarrier;
        public byte[] Barrier;
        public byte[] Barrier2;
        public byte[] Barrier3;
        public byte[] Barrier4;
        public byte[] Barrier5;
        public Vector2 velocity;

        public Point16 Bean;
        public byte Difficulty = 0;

        public byte Time;
        /// <summary>
        /// 记录操作
        /// </summary>
        public byte C = 0;
        public void Update(GameTime gameTime)
        {
            if (Main.gamePaused)
            {
                return;
            }
            Mini_game game = Main.LocalPlayer.Dplayer().Mini_game_shortcuts;

            game.Enable = true;
            if (game.Exit|| Position.Count>=23)
            {
                if (Position.Count >= 23)
                {
                    //Main.LocalPlayer.Dplayer().Bossperspective();
                    PlaySound(DDHelper.SoundStyle(1, "解锁成功"));
                    Wiring.HitSwitch(Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().point.X, Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().point.Y);
                    NetMessage.SendData(59, -1, -1, null, Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().point.X, Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().point.Y);
                    FlushedSnake();
                }
                Time = 0;
                Start = false;
                Visible = false;
                Main.LocalPlayer.GetModPlayer<PlaystationPlayer>().point = Point16.Zero;
                DDmod.SyncData(DDType.PlayersGame, Main.LocalPlayer.whoAmI, -1, Main.LocalPlayer.whoAmI);
                return;
            }
            if (!Start)
            {
                if (Time == 0)
                {
                    FlushedSnake();
                }
            }
            byte T = 5;
            if (Main.masterMode)
            {
                T += 3;
            }
            if (game.Jump)
            {
                T += 20;
            }
            Time += T;
            if (!Start)
            {
                if(Time==0)
                {
                    FlushedSnake();
                }
                if (Time > 240)
                {
                    Time = 240;
                    if (game.Up || game.Down || game.Right || game.Left || game.Jump)
                    {
                        Start = true;
                        Time = 0;
                    }
                }
                return;
            }
            bool R = true;

            Vector2 vector = velocity;
            bool B = Vertical;
            if (C == 0)
            {
                if (velocity.X == 0)
                {
                    if (game.Left)
                    {
                        velocity.Y = 0;
                        velocity.X = -1;
                        C = 100;
                    }
                    if (game.Right)
                    {
                        velocity.Y = 0;
                        velocity.X = 1;
                        C = 100;
                    }
                }
                else
                {
                    if (game.Up)
                    {
                        velocity.Y = -1;
                        velocity.X = 0;
                        C = 100;
                    }
                    if (game.Down)
                    {
                        velocity.Y = 1;
                        velocity.X = 0;
                        C = 100;
                    }
                }
            }
            if (PreFailure())
            {
                velocity = vector;
                Vertical = B;
            }
            if (Time >= 90)
            {
                Time -= 90;
                if (velocity != Vector2.Zero)
                {
                    for (int i = Position.Count - 1; i > 0; i--)
                    {
                        Position[i] = Position[i - 1];
                    }
                    V();
                    /*
                    if (R)
                    {
                        for (int i = Position.Count - 1; i > 0; i--)
                        {
                            Position[i] = Position[i - 1];
                        }
                    }*/
                }
                C = 0;
            }
            void V()
            {
                SnakePosition += velocity;
                Failure(ref R);
                Position[0] = SnakePosition;
            }

            Rectangle rectangle = new Rectangle((int)SnakePosition.X, (int)SnakePosition.Y, 1, 1);
            Rectangle Beanrectangle = new Rectangle((int)Bean.X, (int)Bean.Y, 1, 1);
            if (Position.Count > 1 && rectangle.Intersects(Beanrectangle))
            {
                Position.Add(Position[Position.Count - 1]);
                FlushedBean();
                PlaySound(DDHelper.SoundStyle(1, "吃豆"));
            }
        }
        public bool PreFailure()
        {
            SnakePosition += velocity;
            if (velocity.X != 0)
            {
                if (SnakePosition.X < 0 || SnakePosition.X > 23)
                {
                    SnakePosition -= velocity;
                    velocity.X = 0;
                    return true;
                }
            }

            if (velocity.Y != 0)
            {
                if (SnakePosition.Y < 0 || SnakePosition.Y > 14)
                {
                    SnakePosition -= velocity;
                    velocity.Y = 0;
                    return true;
                }
            }
            Rectangle rectangle = new Rectangle((int)SnakePosition.X, (int)SnakePosition.Y, 1, 1);
            /*
            for (int B = 1; B < Position.Count; B++)
            {
                Rectangle rectangle2 = new Rectangle((int)Position[B].X, (int)Position[B].Y, 1, 1);
                if (rectangle.Intersects(rectangle2))
                {
                    SnakePosition -= velocity;
                    return true;
                }
            }*/
            if (TrueBarrier != null && TrueBarrier.Length > 0)
            {
                for (int I = 0; I < TrueBarrier.Length; I++)
                {
                    if (TrueBarrier[I] == 1)
                    {
                        int P = I / 24;
                        Rectangle rectangle2 = new Rectangle(I % 24, P, 1, 1);
                        if (rectangle.Intersects(rectangle2))
                        {
                            SnakePosition -= velocity;
                            return true;
                        }
                    }
                }
            }
            SnakePosition -= velocity;
            return false;
        }
        public bool Failure(ref bool R)
        {
            if (velocity.X != 0)
            {
                if (SnakePosition.X < 0 || SnakePosition.X > 23)
                {
                    SnakePosition -= velocity;
                    velocity.X = 0;
                    R = false;
                    FlushedSnake();
                    return true;
                }
            }

            if (velocity.Y != 0)
            {
                if (SnakePosition.Y < 0 || SnakePosition.Y > 14)
                {
                    SnakePosition -= velocity;
                    velocity.Y = 0;
                    R = false;
                    FlushedSnake();
                    return true;
                }
            }
            Rectangle rectangle = new Rectangle((int)SnakePosition.X, (int)SnakePosition.Y, 1, 1);

            for (int B = 1; B < Position.Count; B++)
            {
                Rectangle rectangle2 = new Rectangle((int)Position[B].X, (int)Position[B].Y, 1, 1);
                if (rectangle.Intersects(rectangle2))
                {
                    SnakePosition -= velocity;
                    FlushedSnake();
                    return true;
                }
            }
            if (TrueBarrier != null && TrueBarrier.Length > 0)
            {
                for (int I = 0; I < TrueBarrier.Length; I++)
                {
                    if (TrueBarrier[I] == 1)
                    {
                        int P = I / 24;
                        Rectangle rectangle2 = new Rectangle(I % 24, P, 1, 1);
                        if (rectangle.Intersects(rectangle2))
                        {
                            SnakePosition -= velocity;
                            FlushedSnake();
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public void FlushedBean()
        {
            List<Point16> points = new List<Point16>();
            for (int A = 0; A <= 23; A++)
            {
                for (int B = 0; B <= 14; B++)
                {
                    points.Add(new Point16(A, B));
                }
            }
            for (int B = 0; B < Position.Count; B++)
            {
                points.Remove(new Point16((int)Position[B].X, (int)Position[B].Y));
            }
            if (TrueBarrier != null && TrueBarrier.Length > 0)
            {
                for (int I = 0; I < TrueBarrier.Length; I++)
                {
                    if (TrueBarrier[I] == 1)
                    {
                        int P = I / 24;
                        points.Remove(new Point16(I%24, P));
                    }
                }
            }
            C = 0;
            Bean = Main.rand.Next(points);
        }
        public void FlushedSnake()
        {
            Time = 0;
            Start = false;
            SnakePosition = new Vector2(8, 7);
            Position = new List<Vector2>();
            Position.Add(new Vector2(8, 7));
            Position.Add(new Vector2(7, 7));
            Position.Add(new Vector2(6, 7));
            velocity.Y = 0;
            velocity.X = 1;
            Vertical = false;
            if (Barrier == null || Barrier.Length == 0)
            {
                Barrier = DDHelper.Getbytes(assets[0].Value);
                Barrier2 = DDHelper.Getbytes(assets[1].Value);
                Barrier3 = DDHelper.Getbytes(assets[2].Value);
                Barrier4 = DDHelper.Getbytes(assets[3].Value);
                Barrier5 = DDHelper.Getbytes(assets[4].Value);
            }
            if (Main.expertMode)
            {
                TrueBarrier = Main.rand.Next(new byte[][] { Barrier, Barrier2, Barrier3, Barrier4, Barrier5 });
                if (TrueBarrier == Barrier4)
                {
                    SnakePosition = new Vector2(6, 7);
                    Position = new List<Vector2>();
                    Position.Add(new Vector2(6, 7));
                    Position.Add(new Vector2(5, 7));
                    Position.Add(new Vector2(4, 7));
                }
                if (TrueBarrier == Barrier5)
                {
                    SnakePosition = new Vector2(12, 7);
                    Position = new List<Vector2>();
                    Position.Add(new Vector2(12, 7));
                    Position.Add(new Vector2(11, 7));
                    Position.Add(new Vector2(10, 7));
                }
            }
            else
            {
                Difficulty = 0;
                TrueBarrier = [];
            }
            FlushedBean();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = this.texture.Value;
            Vector2 vector = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            spriteBatch.Draw(texture, vector, null, Color.White, 0, texture.Size() / 2, 1, 0, 0);
            if (Start || Time >= 240)
            {
                for (int A = 0; A < Position.Count; A++)
                {
                    Vector2 Po = Position[A] * 8;
                    spriteBatch.Draw(DDTextures.WhitePng.Value, vector - (texture.Size() / 2 - new Vector2(24)) + Po, null, new Color(0, 100, 0, 255), 0, Vector2.Zero, 4, 0, 0);
                }
                spriteBatch.Draw(texture2.Value, vector - (texture.Size() / 2 - new Vector2(24)) + new Vector2(Bean.X, Bean.Y) * 8, null, new Color(0, 100, 0, 255), 0, Vector2.Zero, 1, 0, 0);
                if (TrueBarrier != null && TrueBarrier.Length > 0)
                {
                    for (int I = 0; I < TrueBarrier.Length; I++)
                    {
                        if (TrueBarrier[I] == 1)
                        {
                            int P = I / 24;
                            spriteBatch.Draw(DDTextures.WhitePng.Value, vector - (texture.Size() / 2 - new Vector2(24)) + new Vector2(I % 24, P) * 8, null, new Color(0, 10, 0, 255), 0, Vector2.Zero, 4, 0, 0);
                        }
                    }
                }
            }
            else
            {

                byte[] B = new byte[] {
                    0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,1,1,0,0,
                    0,0,0,0,1,0,1,0,0,0,1,0,0,1,0,1,0,0,1,0,
                    1,0,0,0,0,1,0,0,0,0,0,0,1,0,0,1,0,0,1,0,
                    0,0,0,0,1,0,1,0,0,0,0,1,0,0,0,1,0,0,1,0,
                    0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,1,1,0,0,
                };
                for (int I = 0; I < B.Length; I++)
                {
                    if (B[I] == 1)
                    {
                        int P = I / 20;
                        spriteBatch.Draw(DDTextures.WhitePng.Value, vector - (texture.Size() / 2 - new Vector2(24)) + new Vector2(I % 20 + 2, P + 5) * 8, null, new Color(0, 100, 0, 255), 0, Vector2.Zero, 4, 0, 0);
                    }
                }
            }
        }
    }
}