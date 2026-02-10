
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.UI;

namespace DDmod.UI.PlaystationUI.Game.MeteorsPlane
{
	public class MeteorsGore
    {
		public int GameType;
        public MeteorsGore(int GameType)
        {
            this.GameType = GameType;
        }
        public Vector2 velocity;
		public Vector2 Position;
		public int width;
		public int height;
		public int type;
		public int Bosstype;
		public int WhoamI;
		public float rotating;
		public bool Norotating;
		public float scale;
		public bool active;
		public int post;
		public bool gravity = false;
		public Color color = new Color(50,30,50,Main.rand.Next(200));
		public Vector2 Centre => Position + new Vector2(width, height) / 2;
		public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, width, height);

		public void Update()
		{
			if (!active || type == 0)
			{
				return;
			}
			if (!Norotating)
			{
				rotating += velocity.X * 0.03F;
			}
			if (velocity.Y < 10&& gravity)
			{
				velocity.Y += 0.3F;
            }
            Position += velocity;
            if (Position.X < -500 ||
                Position.X > PlaystationSystem.ScreenSize.X + 500 ||
               Position.Y < -500 ||
              Position.Y > PlaystationSystem.ScreenSize.Y + 500)
            {
                active = false;
            }
        }

		public void Dawn(SpriteBatch sb, Vector2 ScreenPos)
        {
            if (!active || type == 0 || WhoamI == 0)
			{
				return;
			}
			Texture2D texture = TextureAssets.Gore[type].Value;
			Main.instance.LoadGore(type);
			if (Norotating)
			{
				Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);

				sb.Draw(texture, Centre + ScreenPos, new Rectangle?(rectangle), color, rotating, new Vector2(texture.Width, texture.Height) / 2, scale, 0, 0);
			}
			else
			{
				sb.Draw(texture, Centre + ScreenPos, new Rectangle?(new Rectangle(0, 0, texture.Width, (int)(texture.Height))), color, rotating, new Vector2(texture.Width, texture.Height) / 2, scale, 0, 0);
			}
		}
		public void SetDefault(int type)
		{
		}
		public static int NewGore(int GameType, Vector2 position, Vector2 velocity, int type,float Scale = 1,int post =0)
		{
			int Type = 0;
			for (int k = 1; k < PlaystationSystem.Playstation[GameType].gore.Length; k++)
			{
				if (PlaystationSystem.Playstation[GameType].gore[k] == null)
				{
					PlaystationSystem.Playstation[GameType].gore[k] = new MeteorsGore(GameType);
				}
				if (!PlaystationSystem.Playstation[GameType].gore[k].active)
				{
					PlaystationSystem.Playstation[GameType].gore[k] = new MeteorsGore(GameType);
					PlaystationSystem.Playstation[GameType].gore[k].SetDefault(type);
					PlaystationSystem.Playstation[GameType].gore[k].Position = position;
					PlaystationSystem.Playstation[GameType].gore[k].velocity = velocity;
					PlaystationSystem.Playstation[GameType].gore[k].type = type;
					PlaystationSystem.Playstation[GameType].gore[k].scale = Scale;
					PlaystationSystem.Playstation[GameType].gore[k].post = post;
					PlaystationSystem.Playstation[GameType].gore[k].active = true;
					PlaystationSystem.Playstation[GameType].gore[k].WhoamI = k;
					Type = k;
					break;
				}
			}
			return Type;
		}
	}
}