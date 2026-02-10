using DDmod.Content.Items.Melee.Sword;
using DDmod.NoContent.Config;
using DDmod.Players;
using System.Linq;
using Terraria.Graphics.Shaders;

namespace DDmod
{
	public partial class UITextDraw
	{
		public Vector2 velocity;
		public Vector2 position;
		public Color color;
		public string Text;
		public float rotating;
		public float scale;
		public int Time;
		public bool active;
		public int type;
		/// <summary>
		/// 透明度
		/// </summary>
		public float Al;
		public void Update()
		{
			position += velocity;
			if (type == 1)
			{
				Time--;
				if (Time <= 30)
				{
					Al = (float)Time / 30;
					velocity = new Vector2(0, -2);
				}
				else
				{
					Al = 1;
				}
				if (Time <= 0)
				{
					active = false;
				}
			}
			if (type == 2)
			{
				Time--;
				if (velocity.Y < 4)
					velocity.Y += 0.4F;
				if (Time <= 30)
				{
					Al = (float)Time / 30;
				}
				else
				{
					Al = 1;
				}
				if (Time <= 0)
				{
					active = false;
				}
			}
		}
		public static void NewText(Vector2 position, string Text, int T = 180, float scale = 1, float rotating = 0,Color color = default,Vector2 vector = default, int type = 1)
		{
			if(color == default)
            {
				color = Color.White;
            }
			for (int k = 0; k < TextSystem.UITextDraw.Length; k++)
			{
				if (!TextSystem.UITextDraw[k].active)
				{
					TextSystem.UITextDraw[k].position = position;
					TextSystem.UITextDraw[k].Text = Text;
					TextSystem.UITextDraw[k].active = true;
					TextSystem.UITextDraw[k].Time = T;
					TextSystem.UITextDraw[k].scale = scale;
					TextSystem.UITextDraw[k].rotating = rotating;
					TextSystem.UITextDraw[k].type = type;
					TextSystem.UITextDraw[k].color = color;
					TextSystem.UITextDraw[k].velocity = vector;
					break;
				}
			}
		}
	}
    public class TextSystem : ModSystem
    {
		public static UITextDraw[] UITextDraw = new UITextDraw[50];
        public override void Load()
        {
			for (int a = 0; a < UITextDraw.Length; a++)
			{
				UITextDraw[a] = new UITextDraw();
			}
					
		}
		public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
        }
    }

}