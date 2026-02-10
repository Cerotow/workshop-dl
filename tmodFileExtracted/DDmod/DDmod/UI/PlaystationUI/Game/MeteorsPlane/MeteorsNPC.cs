using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.Sundries;
using DDmod.Content.NPCs.Boss.MeteorAnnihilator;
using DDmod.Modkey;
using DDmod.ModLinkage.BossChecklist;
using DDmod.Players;
using DDmod.Sync;
using DDmod.UI.HunterQuests;
using DDmod.UI.ItemUI;
using DDmod.UI.ItemUI.背包;
using System.Collections;
using Terraria.Chat;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;
using static DDmod.Players.DDPlayer;
using static System.Net.Mime.MediaTypeNames;

namespace DDmod.UI.PlaystationUI.Game.MeteorsPlane
{
    public class MeteorsNPC
    {
        public int GameType;
        public string Text = "DDmod/UI/PlaystationUI/Game/MeteorsPlane/NPC/NPC_";
        public static Asset<Texture2D>[] Texture;
        public MeteorsNPC(int GameType)
        {
            this.GameType = GameType;
            Load();
        }
        public void Load()
        {
            int T = 2;
            Texture = new Asset<Texture2D>[T + 1];
            for (int a = 0; a < Texture.Length; a++)
            {
                Texture[a] = ModContent.Request<Texture2D>(Text + a);
            }
        }
        public Vector2 velocity;
        public Vector2 Position;
        public Vector2 Size;
        public int Damage;
        public bool Active;
        public bool Hostile;
        public bool Friendly;
        public int WhoamI;
        public int MaxLife;
        public int Life;
        public int Penetrate = 1;
        public int Type = 0;
        public int MaxFrame = 0;
        public int Frame = 0;
        public int FrameTime = 0;
        public int HitTime = 0;
        public Vector2 Scale = new Vector2(0.5F);
        public Rectangle Rect => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        public void Setdefault()
        {
            //眼球
            if (Type == 1)
            {
                Damage = 1;
                MaxLife = 40;
                Size = new Vector2(22);
                Hostile = true;
                MaxFrame = 2;
            }
            //小眼球
            if (Type == 2)
            {
                Damage = 1;
                MaxLife = 20;
                Size = new Vector2(20);
                Hostile = true;
                MaxFrame = 2;
            }
            Life = MaxLife;
            Size *= Scale;
        }
        public void Collide(MeteorsProj proj)
        {
            if(proj.invinc==null || proj.invinc[WhoamI]>0)
            {
                return;
            }
            if(proj.Active&&Life>0&& proj.Collide(PlaystationSystem.Playstation[GameType].npc[WhoamI]))
            {
                Hit(proj.Damage);
                proj.invinc[WhoamI] += 20;
                proj.Penetrate--;
                if(proj.Penetrate==0)
                {
                    proj.Kill();
                }
            }
        }
        public void Hit(int Damage)
        {
            HitTime = 12;
            Life -= Damage;
            if(Life<=0)
            {
                Kill();
            }
        }
        public void Kill()
        {
            for (int a=0; a<30;a++)
            {
                MeteorsDust.NewDust(GameType,Position+Size/2,new Vector2(Main.rand.NextFloat(0,2),0).RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)),5, Main.rand.NextFloat(1F, 1.3F));
            }
                Active = false;
        }
        public void Update()
        {
            Position += velocity;
            for(int a=0;a < PlaystationSystem.Playstation[GameType].proj.Length;a++ )
            {
                Collide(PlaystationSystem.Playstation[GameType].proj[a]);
            }
            //眼球
            if (Type == 1)
            {
                FrameTime++;
                if(FrameTime%6==0)
                {
                    Frame++;
                }
                if(Frame>1)
                {
                    Frame = 0;
                }
            }
            //小眼球
            if (Type == 2)
            {
                FrameTime++;
                if (FrameTime % 6 == 0)
                {
                    Frame++;
                }
                if (Frame > 1)
                {
                    Frame = 0;
                }
            }
            if(HitTime>0)
            {
                HitTime--;
            }
            if (Position.X < -500 ||
                Position.X > PlaystationSystem.ScreenSize.X + 500 ||
               Position.Y < -500 ||
              Position.Y > PlaystationSystem.ScreenSize.Y + 500)
            {
                Active = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 ScreenPos)
        {
            Texture2D texture = Texture[0].Value;
            if (Type < Texture.Length)
            {
                texture = Texture[Type].Value;
            }
            if(MaxFrame<=0)
            {
                MaxFrame = 1;
            }
            Rectangle rectangle = new Rectangle(0, texture.Height / MaxFrame * Frame, texture.Width, texture.Height / MaxFrame);
            Color color = Color.White;
            if (HitTime > 0)
            {
                color = new Color(255, 0, 0);
            }

            if (Type == 1)
            {
                spriteBatch.Draw(texture, Position+Size/2 + ScreenPos, rectangle, color, 0, rectangle.Size()/2, Scale, 0, 0);
            }
            if (Type == 2)
            {
                spriteBatch.Draw(texture, Position+Size/2 + ScreenPos, new Rectangle(0, texture.Height / 2 * Frame, texture.Width, texture.Height / 2), color, 0, rectangle.Size()/2, Scale, 0, 0);

            }

        }
        public static int NewNPC(int GameType,Vector2 Position, Vector2 velocity, int Type)
        {
            for (int a = 0; a < PlaystationSystem.Playstation[GameType].npc.Length; a++)
            {
                if (PlaystationSystem.Playstation[GameType].npc[a] == null)
                {
                    PlaystationSystem.Playstation[GameType].npc[a] = new MeteorsNPC(GameType);
                }
                if (!PlaystationSystem.Playstation[GameType].npc[a].Active)
                {
                    PlaystationSystem.Playstation[GameType].npc[a] = new MeteorsNPC(GameType);
                    PlaystationSystem.Playstation[GameType].npc[a].Type = Type;
                    PlaystationSystem.Playstation[GameType].npc[a].Setdefault();
                    PlaystationSystem.Playstation[GameType].npc[a].Position = Position - PlaystationSystem.Playstation[GameType].npc[a].Size / 2;
                    PlaystationSystem.Playstation[GameType].npc[a].velocity = velocity;
                    PlaystationSystem.Playstation[GameType].npc[a].WhoamI = a;
                    PlaystationSystem.Playstation[GameType].npc[a].Active = true;
                    return a;
                }
            }
            return 0;
        }
    }
}