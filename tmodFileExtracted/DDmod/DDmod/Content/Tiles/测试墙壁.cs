using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Tiles
{
	public class 测试墙壁 : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(0, 0, 110));
			DustType = 226;

            HitSound = SoundID.Tink;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Texture2D bgTexture = ModContent.Request<Texture2D>("DDmod/Content/Biome/绿岩实验室_Background").Value;

            float scrollOffsetX = Main.screenPosition.X/ bgTexture.Width/2;
            float scrollOffsetY = Main.screenPosition.Y/ bgTexture.Height/2;
            scrollOffsetX = 0;
            scrollOffsetY = 0;

            Vector2 drawPos = new Vector2(i * 16, j * 16);
            Rectangle sourceRect = new Rectangle(
                (int)(drawPos.X + scrollOffsetX * bgTexture.Width) % bgTexture.Width,
                (int)(drawPos.Y + scrollOffsetY * bgTexture.Height) % bgTexture.Height,
                16, 16
            );

            Color lightColor = Lighting.GetColor(i, j);

            // 检查边界
            bool crossX = sourceRect.X + 16 > bgTexture.Width;
            bool crossY = sourceRect.Y + 16 > bgTexture.Height;

            if (!crossX && !crossY)
            {
                // 正常绘制
                spriteBatch.Draw(bgTexture, drawPos + zero - Main.screenPosition, sourceRect, lightColor);
            }
            else if (crossX && !crossY)
            {
                // X方向边界
                int leftWidth = bgTexture.Width - sourceRect.X;
                int rightWidth = 16 - leftWidth;

                // 左边部分
                spriteBatch.Draw(bgTexture, drawPos + zero - Main.screenPosition,
                    new Rectangle(sourceRect.X, sourceRect.Y, leftWidth, 16), lightColor);
                // 右边部分  
                spriteBatch.Draw(bgTexture, drawPos + new Vector2(leftWidth, 0) + zero - Main.screenPosition,
                    new Rectangle(0, sourceRect.Y, rightWidth, 16), lightColor);
            }
            else if (!crossX && crossY)
            {
                // Y方向边界
                int topHeight = bgTexture.Height - sourceRect.Y;
                int bottomHeight = 16 - topHeight;

                // 上边部分
                spriteBatch.Draw(bgTexture, drawPos + zero - Main.screenPosition,
                    new Rectangle(sourceRect.X, sourceRect.Y, 16, topHeight), lightColor);
                // 下边部分
                spriteBatch.Draw(bgTexture, drawPos + new Vector2(0, topHeight) + zero - Main.screenPosition,
                    new Rectangle(sourceRect.X, 0, 16, bottomHeight), lightColor);
            }
            else
            {
                // XY角
                int leftWidth = bgTexture.Width - sourceRect.X;
                int topHeight = bgTexture.Height - sourceRect.Y;
                int rightWidth = 16 - leftWidth;
                int bottomHeight = 16 - topHeight;

                // 左上
                spriteBatch.Draw(bgTexture, drawPos + zero - Main.screenPosition,
                    new Rectangle(sourceRect.X, sourceRect.Y, leftWidth, topHeight), lightColor);
                // 右上
                spriteBatch.Draw(bgTexture, drawPos + new Vector2(leftWidth, 0) + zero - Main.screenPosition,
                    new Rectangle(0, sourceRect.Y, rightWidth, topHeight), lightColor);
                // 左下
                spriteBatch.Draw(bgTexture, drawPos + new Vector2(0, topHeight) + zero - Main.screenPosition,
                    new Rectangle(sourceRect.X, 0, leftWidth, bottomHeight), lightColor);
                // 右下
                spriteBatch.Draw(bgTexture, drawPos + new Vector2(leftWidth, topHeight) + zero - Main.screenPosition,
                    new Rectangle(0, 0, rightWidth, bottomHeight), lightColor);
            }
            return true;
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			/*for (int W = 0; W <= Slime.instance.numPlayer; W++)
			{
				Player player = Main.player[W];
				if (player.Distance(new Vector2(i * 16, j * 16)) < 100)
				{
					r = 0;
					g = 0.08f;
					b = 0.08f;
				}
			}*/
		}
		public override bool CanExplode(int i, int j)
        {
            return false;
		}
		public override void KillWall(int i, int j, ref bool fail)
		{
			//fail = true;
		}
	}
}