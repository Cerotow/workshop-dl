using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Tiles.花岗岩基地.Walls
{
	public class 花岗岩科技墙3Tile : ModWall
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
			/*Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Vector2 drawOffset = new Vector2((float)(i * 16) - Main.screenPosition.X, (float)(j * 16) - Main.screenPosition.Y) + zero;
			spriteBatch.Draw(mod.GetTexture("Walls/科技花岗岩墙"), drawOffset, new Rectangle?(new Rectangle(Main.tile[i, j].wallFrameX(), Main.tile[i, j].wallFrameY(), 32, 32)), GetWallColour(i, j), 0f, new Vector2(8,8), 1f, 0, 0f);
			for (int W = 0; W <= Slime.instance.numPlayer; W++)
			{
				Player player = Main.player[W];
				float G = (player.Center - new Vector2(i * 16, j * 16)).Length() / 16;
				float I = (player.Center - new Vector2(i * 16, j * 16)).Length() - G;
				if (player.Distance(new Vector2(i * 16, j * 16)) < 100)
				{
					spriteBatch.Draw(mod.GetTexture("Walls/科技花岗岩墙光效"), drawOffset, new Rectangle?(new Rectangle(Main.tile[i, j].wallFrameX(), Main.tile[i, j].wallFrameY(), 32, 32)), Color.White * ((1 - I * 0.015F) * 1.25F), 0f, new Vector2(8, 8), 1f, 0, 0f);
				}
			}
			spriteBatch.Draw(mod.GetTexture("Walls/科技花岗岩墙光效"), drawOffset, new Rectangle?(new Rectangle(Main.tile[i, j].wallFrameX(), Main.tile[i, j].wallFrameY(), 32, 32)), Color.White *0.01F, 0f, new Vector2(8, 8), 1f, 0, 0f);*/
			return true;
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
			fail = true;
		}
		private Color GetWallColour(int i, int j)
		{
			byte b = Main.tile[i, j].WallColor;
			Color paintCol = WorldGen.paintColor((int)b);
			if (b < 13)
			{
				paintCol.R = (byte)((float)paintCol.R / 2f + 128f);
				paintCol.G = (byte)((float)paintCol.G / 2f + 128f);
				paintCol.B = (byte)((float)paintCol.B / 2f + 128f);
			}
			if (b == 29)
			{
				paintCol = Color.Black;
			}
			Color col = Lighting.GetColor(i, j);
			col.R = (byte)((float)paintCol.R / 255f * (float)col.R);
			col.G = (byte)((float)paintCol.G / 255f * (float)col.G);
			col.B = (byte)((float)paintCol.B / 255f * (float)col.B);
			return col;
		}
	}
}