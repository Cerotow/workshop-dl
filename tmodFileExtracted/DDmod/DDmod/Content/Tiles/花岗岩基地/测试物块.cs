using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using DDmod.Content.Items.Melee.Sword;
using static Terraria.GameContent.Animations.Actions.Sprites;

namespace DDmod.Content.Tiles.花岗岩基地
{
	public class 测试物块 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[(int)Type] = true;
			Main.tileMergeDirt[(int)Type] = false;
			Main.tileLighted[(int)Type] =false;
			DustType = 226;
			MinPick = 300;
			MineResist = 3f;
			HitSound = SoundID.Tink;
			CreateMapEntryName();
			AddMapEntry(new Color(45, 45, 180));
		    
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
        public override bool CanPlace(int i, int j)
        {
            return base.CanPlace(i, j);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
            /*Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int height = (tile.frameY == 36) ? 18 : 16;
			for (int W = 0; W <= Slime.instance.numPlayer; W++)
			{
				Player player = Main.player[W];
				float G = (player.Center - new Vector2(i * 16, j * 16)).Length() / 16;
				float I = (player.Center - new Vector2(i * 16, j * 16)).Length() - G;
				if (player.Distance(new Vector2(i * 16, j * 16)) < 100)
				{
					Main.spriteBatch.Draw(mod.GetTexture("Tiles/花岗岩基地/科技花岗岩光效"), new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.frameX, (int)tile.frameY, 16, height)), Color.White * ((1 - I * 0.015F) * 1.25F), 0f, Vector2.Zero, 1f, 0, 0f);
				}
			}
			Main.spriteBatch.Draw(mod.GetTexture("Tiles/花岗岩基地/科技花岗岩光效"), new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.frameX, (int)tile.frameY, 16, height)), Color.White * 0.05f, 0f, Vector2.Zero, 1f, 0, 0f);*/

        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
			return TileHelp.TileDraw(i,j,spriteBatch, Lighting.GetColor(new Point(i, j)));
        }
        public override bool Slope(int i, int j)
        {
            return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            Tile tile = Main.tile[i, j];
            
            return true;
        }
		
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return TileHelp.TileFrame(i, j, false); ;
        }
        public override void PostTileFrame(int i, int j, int up, int down, int left, int right, int upLeft, int upRight, int downLeft, int downRight)
        {
			/*
            Tile tile = Main.tile[i, j];
            if((tile.TileFrameX == 18 || tile.TileFrameX == 36|| tile.TileFrameX == 54) &&tile.TileFrameY==0)
            {
                tile.TileFrameX = 18;
                tile.TileFrameY = 0;

            }
            //TileHelp.TileFrame(i, j,false);*/
        }
        public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}