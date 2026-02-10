
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace DDmod.ModLinkage.BossChecklist
{
	public partial class BGore
	{
		public Vector2 velocity;
		public Vector2 position;
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
		public bool gravity = true;
		public Color color = Color.White;
		public Vector2 Centre => position + new Vector2(width, height) / 2;
		public Rectangle Rectangle => new Rectangle((int)position.X, (int)position.Y, width, height);

		public void UpdateGore(int Proj)
		{
			if (!active || type == 0 || Proj == 0)
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
			position += velocity;
        }

		public void Dawn(SpriteBatch sb, Rectangle rect,int Type)
        {
            if (!active || type == 0 || WhoamI == 0||Type!=Bosstype)
			{
				return;
			}
			Texture2D texture = TextureAssets.Gore[type].Value;
			Main.instance.LoadGore(type);
			if (Norotating)
			{
				Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);

				sb.Draw(texture, position + rect.TopLeft(), new Rectangle?(rectangle), color, rotating, Vector2.Zero, scale, 0, 0);
			}
			else
			{
				sb.Draw(texture, Centre + rect.TopLeft(), new Rectangle?(new Rectangle(0, 0, texture.Width, (int)(texture.Height))), color, rotating, new Vector2(texture.Width, texture.Height) / 2, scale, 0, 0);
			}
		}
		public void SetDefault(int type)
		{
		}
		public static int NewGore(Vector2 position, Vector2 velocity, int type,float Scale = 1,int post =0,int Bosstype=0)
		{
			int Type = 0;
			for (int k = 1; k < ChecklistHelper.Gore.Length; k++)
			{
				if (ChecklistHelper.Gore[k] == null)
				{
					ChecklistHelper.Gore[k] = new BGore();
				}
				if (!ChecklistHelper.Gore[k].active)
				{
					ChecklistHelper.Gore[k] = new BGore();
					ChecklistHelper.Gore[k].SetDefault(type);
					ChecklistHelper.Gore[k].position = position;
					ChecklistHelper.Gore[k].velocity = velocity;
					ChecklistHelper.Gore[k].type = type;
					ChecklistHelper.Gore[k].scale = Scale;
					ChecklistHelper.Gore[k].post = post;
					ChecklistHelper.Gore[k].Bosstype = Bosstype;
					ChecklistHelper.Gore[k].active = true;
					ChecklistHelper.Gore[k].WhoamI = k;
					Type = k;
					break;
				}
			}
			return Type;
		}
	}
}