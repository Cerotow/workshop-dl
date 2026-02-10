
using Microsoft.Xna.Framework.Graphics;

namespace DDmod.ModLinkage.BossChecklist
{
	public partial class BProj
	{
		public Vector2 velocity;
		public Vector2 position;
		public int width;
		public int height;
		public int type;
		public int damage;
		public int penetrate;
		public int Bosstype;
		public float rotating;
		/// <summary>
		/// 是不是敌对
		/// </summary>
		public bool Hostile;
		public bool active;
		public float Time;
		public Vector2 Centre => position + new Vector2(width, height) / 2;
		public Rectangle Rectangle => new Rectangle((int)position.X, (int)position.Y, width, height);
		public int[] ImmuneFrames = new int[200];
		public void UpdateProj(int Proj)
		{
			position += velocity;
			for (int a = 0; a < 200; a++)
			{
				if(active && ImmuneFrames[a]>0)
                {
					ImmuneFrames[a]--;
				}
				if (active && ChecklistHelper.NPC[a].active && ChecklistHelper.NPC[a].Rectangle.Intersects(Rectangle)&& penetrate>0&& ImmuneFrames[a]<=0&& ChecklistHelper.NPC[a].Hostile!=Hostile)
				{
					ChecklistHelper.NPC[a].Hit(damage);
					ImmuneFrames[a] = 20;
					penetrate--;
				}
            }
            if (Time < velocity.Length()/4)
            {
                Time += velocity.Length() / 12;
            }
            else
            {
                Time = velocity.Length()/4;
            }
            if (penetrate == 0)
			{
				active = false;
			}
		}
		public void Dawn(SpriteBatch sb,Rectangle rect,int Type)
		{
			if (!active || type == 0||Type!=Bosstype)
			{
				return;
            }
            Texture2D texture = ProjTexture[type].Value;
			if (type == 1)
            {
				float Scale = 0.5f;
                Vector2 vector = new Vector2(texture.Width / 2, 10);
                Rectangle rectangle = new Rectangle(0, 0, texture.Width, 14);
                Color color = new Color(255, 255, 255, 20);
                sb.Draw(texture, Centre + rect.TopLeft(), rectangle, color,0, vector, Scale ,0, 0f);

                vector = new Vector2(texture.Width / 2, 0);
                rectangle = new Rectangle(0, 14, texture.Width, 2);
                sb.Draw(texture, Centre + rect.TopLeft() - velocity.PerfectNormalize() * 4 * Scale, rectangle, color, 0, vector, new Vector2(Scale, velocity.Length() / 2 * Time), 0, 0f);

                vector = new Vector2(texture.Width / 2, 0);
                rectangle = new Rectangle(0, 16, texture.Width, 14);
                sb.Draw(texture, Centre + rect.TopLeft() - (velocity.PerfectNormalize() * 4 * Scale ) - velocity * Time, rectangle, color, 0, vector, Scale , 0, 0f);
            }
		}
		public Asset<Texture2D>[] ProjTexture = new Asset<Texture2D>[100];
		public void SetDefault(int type)
		{
			ProjTexture = new Asset<Texture2D>[100];
			if (type != 0)
				ProjTexture[type] = ModContent.Request<Texture2D>("DDmod/Content/Projectiles/Boss/Boss机械橙激光");
			if (type == 1)
			{
				width = 7;
				height = 60;
				penetrate = 1;
			}
		}
		public static void NewProj(Vector2 position, Vector2 velocity, int type, int damage, int Bosstype)
		{
			for (int k = 0; k < 1000; k++)
			{
				if (!ChecklistHelper.Proj[k].active)
				{
					ChecklistHelper.Proj[k] = new BProj();
					ChecklistHelper.Proj[k].SetDefault(type);
					ChecklistHelper.Proj[k].position = position;
					ChecklistHelper.Proj[k].velocity = velocity;
					ChecklistHelper.Proj[k].type = type;
					ChecklistHelper.Proj[k].Bosstype = Bosstype;
					ChecklistHelper.Proj[k].damage = damage;
					ChecklistHelper.Proj[k].active = true;
					break;
				}
			}
		}
	}
}