using DDmod.NoContent.Config;
using System.Linq;
using Terraria.Graphics.Shaders;

namespace DDmod
{
	public partial class UIDustDraw
	{
		public Vector2 velocity;
		public Vector2 position;
		public Color color;
		public int type;
		public float rotating;
		public float scale;
		public int Time;
		public bool active;
		public bool Gravity;
		public float ScaleSpeed;
		/// <summary>
		/// 特殊粒子
		/// </summary>
		public bool Special;
		/// <summary>
		/// 透明度
		/// </summary>
		public float Al = 1;
		public void Update()
		{
			if (position.X < -40)
			{
				active = false;

			}
			else if (position.X> Main.screenWidth+40)
			{
				active = false;

			}
			if (position.Y < -40)
            {

				active = false;
			}
			else if (position.Y> Main.screenHeight+40)
			{
				active = false;

			}
			if(Gravity)
            {
				if (velocity.Y < 10)
				{
					velocity.Y += 0.4F;
				}
			}
			position += velocity;
			if (type == 1)
			{
				scale -= ScaleSpeed;
				if (scale < 0)
				{
					active = false;
				}
			}
			if (type == 2)
			{
				scale -= ScaleSpeed;
				if (scale < 0)
				{
					active = false;
				}
			}
			if (type == 3)
			{
				scale -= ScaleSpeed;
				if (velocity.Y < 10)
				{
					velocity.Y += 0.4F;
				}
				if (scale < 0)
                {
					active = false;
                }
			}
			if (type == 4)
			{
				scale -= ScaleSpeed;
				velocity *= 0.98F;
				if (scale < 0)
                {
					active = false;
                }
			}
			if (type == 5)
			{
				scale -= ScaleSpeed;
				velocity *= 0.98F;
				if (scale < 0)
                {
					active = false;
                }
				Gravity = false;

            }
		}
		public void SetDefaults(int type)
        {
			Al = 1;
			ScaleSpeed = 0.01F;

		}
		public void Draw(SpriteBatch spriteBatch)
		{
			if (type == 1)
			{
				spriteBatch.Draw(DDTextures.WhitePng.Value, position, null, color * Al, 0, DDTextures.WhitePng.Size() / 2, scale, 0, 0);
			}
			if (type == 2)
			{
				spriteBatch.Draw(DDTextures.WhitePng.Value, position, null, color * Al, 0, DDTextures.WhitePng.Size() / 2, scale, 0, 0);
			}
			if (type == 3)
			{
				spriteBatch.Draw(DDTextures.VoidStar.Value, position, null, color * Al, 0, DDTextures.VoidStar.Size() / 2, scale, 0, 0);
				spriteBatch.Draw(DDTextures.VoidStar.Value, position, null, color.Opposite() * 0.5f * Al, 0, DDTextures.VoidStar.Size() / 2, scale / 2, 0, 0);
			}
			if (type == 4)
			{
				spriteBatch.Draw(DDTextures.VoidStar.Value, position, null, color * Al, 0, DDTextures.VoidStar.Size() / 2, scale, 0, 0);
				spriteBatch.Draw(DDTextures.VoidStar.Value, position, null, color.Opposite() * 0.5f * Al, 0, DDTextures.VoidStar.Size() / 2, scale / 2, 0, 0);
			}
			if (type == 5)
			{
				Texture2D texture = TextureAssets.Projectile[454].Value;
                spriteBatch.Draw(texture, position, new Rectangle(0,0, texture.Width, texture.Height/2), color * Al, 0, new Vector2(texture.Width, texture.Height / 2)/ 2, 20, 0, 0);
			}
		}
		public static int NewDust(Vector2 position, int type, Color color= default, Vector2 vector = default, float scale = 1, float rotating = 0)
		{
			for (int k = 0; k < DustSystem.UIDustDraws.Length; k++)
			{
				if (DustSystem.UIDustDraws[k] !=null&& !DustSystem.UIDustDraws[k].active)
				{
					DustSystem.UIDustDraws[k].SetDefaults(k);
					DustSystem.UIDustDraws[k].position = position;
					DustSystem.UIDustDraws[k].type = type;
					DustSystem.UIDustDraws[k].color = color;
					DustSystem.UIDustDraws[k].active = true;
					DustSystem.UIDustDraws[k].velocity = vector;
					DustSystem.UIDustDraws[k].scale = scale;
					DustSystem.UIDustDraws[k].rotating = rotating;
					DustSystem.UIDustDraws[k].Gravity = true;
					DustSystem.UIDustDraws[k].Special = false;
					return k;
				}
			}
			return -1;
		}
	}
	public class DustSystem : ModSystem
	{
		public static UIDustDraw[] UIDustDraws = new UIDustDraw[5000];
		public override void Load()
		{
			for (int a = 0; a < UIDustDraws.Length; a++)
			{
				UIDustDraws[a] = new UIDustDraw();
			}

		}
        public override void PreUpdateTime()
		{
			for (int a = 0; a < UIDustDraws.Length; a++)
			{
				if (UIDustDraws[a].active && !UIDustDraws[a].Special)
				{
					UIDustDraws[a].Update();
				}
			}
		}
        public override void PostDrawInterface(SpriteBatch spriteBatch)
		{
		}
	}
}