
using DDmod.Content.Dusts;
using DDmod.Helper;
using Terraria;

namespace DDmod.ModLinkage.BossChecklist
{
	public partial class BDust
	{
		public Vector2 velocity;
		public Vector2 position;
		public int width;
		public int height;
		public int type;
		public int Bosstype;
		public float rotation;
		public int alpha;
		public bool active;
		public bool gravity;
		public float scale = 1;
		public Color color;
		public object customData;


        public float Extraspeed = 1;
		public Vector2 Centre => position + new Vector2(width, height) / 2;
		public Rectangle Rectangle => new Rectangle((int)position.X, (int)position.Y, width, height);
		public void UpdateDust(int Proj)
		{
			if (!active || type == 0)
			{
				return;
			}
			if (type == ModContent.DustType<光球粒子>())
			{
				position += velocity;
				scale -= 0.03F * (Math.Abs(Extraspeed) % 1000);
				if (scale < 0.01F)
				{
					active = false;
				}
				return;
            }
			if (type == ModContent.DustType<光圈粒子>())
			{
				if (customData == null)
				{
					customData = new Vector4(1, 1, 1, 1);
				}
				if (customData is int)
				{
					float A = (int)customData;
					customData = new object();
					customData = new Vector4(A, A, 0, 0);
				}
				if (customData is float)
				{
					float A = (float)customData;
					customData = new object();
					customData = new Vector4(A, A, 0, 0);
				}
				if (customData is Vector3)
				{
					Vector3 vector = (Vector3)customData;
					customData = new object();
					customData = new Vector4(vector, 0);
				}
				if (customData is Vector4)
				{
					Vector4 vector = (Vector4)customData;
					if (vector.Z <= 0)
					{
						scale += 0.1F * vector.X;
						scale += 0.2F * vector.X;
						if (alpha >= 0)
						{
							alpha += (int)(1 * vector.Y);
						}
						else
						{
							alpha++;
						}
						if (alpha > 255)
						{
							active = false;
						}
					}
					else
					{
						vector.Z--;
						customData = vector;
					}
				}
				return;
			}
			rotation += velocity.X * 0.03F;
			velocity *= 0.98F;
			scale -= 0.03F * (Math.Abs(Extraspeed) % 1000);
			if (scale < 0)
			{
				active = false;
			}
			if (gravity)
			{
				if (velocity.Y < 10)
				{
					velocity.Y += 0.2F;
				}
			}
			position += velocity;
		}
		public void Dawn(SpriteBatch sb, Rectangle rect,int Type)
		{
			if (!active || type == 0|| Type!= Bosstype)
			{
				return;
			}
			Texture2D texture = TextureAssets.Dust.Value;
			int H = type / 100;
			H *= 3;

			ModDust modDust = DustLoader.GetDust(type);
			if (modDust == null)
			{
				sb.Draw(texture, Centre + rect.TopLeft(), new Rectangle?(new Rectangle(texture.Width / 100 * type, texture.Height / 12 * H, texture.Width / 100, texture.Height / 12)), Color.White, rotation, new Vector2(texture.Width / 100, texture.Height / 12) / 2, scale, 0, 0);
			}

			//火焰粒子
			if (type == 6)
			{
				Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
				Color color = new Color(253, 62, 3);
				color.A = 0;

				Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, position + rect.TopLeft(), new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color, rotation, vector, scale / 4, 0, 0f);
			}
			//光球粒子
			if (type == ModContent.DustType<光球粒子>())
			{
				Vector2 vector = new Vector2(DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height()) / 2;
				if (alpha < 0)
				{
					alpha = 0;
				}
				Color color2 = color * (1 - alpha / 255F);
				color2.A = 0;
				Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, position + rect.TopLeft(), new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color2, rotation, vector, scale / 2, 0, 0f);
				if (Extraspeed > 0)
				{
					color2 = color2.Opposite();
					color2.A = 0;
					Main.spriteBatch.Draw(DDTextures.MiniVoidStar.Value, position + rect.TopLeft(), new Rectangle?(new Rectangle(0, 0, DDTextures.MiniVoidStar.Width(), DDTextures.MiniVoidStar.Height())), color2, rotation, vector, scale / 4, 0, 0f);
				}
			}
            //光圈粒子
            if (type == ModContent.DustType<光圈粒子>())
            {
                Texture2D Round = DDTextures.Round3.Value;
                Vector2 origin = Round.Size() / 2;
                float A = (1F - alpha / 255F);
                if (A < 0 || alpha < 0)
                {
                    A = 0;
                }
                if (A > 1)
                {
                    A = 1;
                }
                Vector4 vector = (Vector4)customData;
                Main.spriteBatch.Draw(Round, position + rect.TopLeft(), null, color * A * vector.W, rotation, origin, scale, 0, 0);
            }
        }
        public void SetDefault(int type)
        {
        }
        /// <summary>
        /// 0,陨石歼灭者,1,蘑菇
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="type"></param>
        /// <param name="scale"></param>
        /// <param name=""></param>
        /// <returns></returns>
		public static int NewDust(Vector2 position, Vector2 velocity, int type, float scale,int Bosstype)
		{
			for (int k = 0; k < 1000; k++)
			{
				if (!ChecklistHelper.Dust[k].active)
				{
					ChecklistHelper.Dust[k] = new BDust();
					ChecklistHelper.Dust[k].SetDefault(type);
					ChecklistHelper.Dust[k].scale = scale;
					ChecklistHelper.Dust[k].position = position;
					ChecklistHelper.Dust[k].velocity = velocity;
					ChecklistHelper.Dust[k].type = type;
					ChecklistHelper.Dust[k].Bosstype = Bosstype;
					ChecklistHelper.Dust[k].color = new Color(255, 255, 255, 255);
					ChecklistHelper.Dust[k].Extraspeed = 1;
					ChecklistHelper.Dust[k].alpha = 0;
					ChecklistHelper.Dust[k].active = true;
					return k;
				}
			}
			return 0;
		}
	}
}