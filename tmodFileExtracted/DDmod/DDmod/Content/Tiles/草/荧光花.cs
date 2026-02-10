using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.草
{
	public class 荧光花 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileLighted[Type] = true;
			TileID.Sets.SwaysInWindBasic[Type] = true;
			DustType = 47;
			HitSound = SoundID.Grass;
			AddMapEntry(new Color(125, 180, 185));
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Height = 1;
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				20
			};
			TileObjectData.newTile.DrawYOffset = 0;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				109
			};
			TileObjectData.addTile(Type);
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			if(Main.tile[i,j].TileFrameX ==0)
            {
				r = 231;
				g = 43;
				b = 245;
			}
			else
			if(Main.tile[i,j].TileFrameX ==18)
            {
				r = 98;
				g = 53;
				b = 231;
			}
			else
            {
				r = 255;
				g = 139;
				b = 0;
			}
			r *= 0.003F;
			g *= 0.003F;
			b *= 0.003F;
		}
		public override void AnimateTile(ref int frame, ref int frameCounter)
        {
			DDHelper.BackAndForth(0.2F, 0.5f, 0.01F, ref a, ref b);
		}

		float a = 0;
		bool b = false;
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Color color = new Color(255, 139, 0, 0);
			if (Main.tile[i, j].TileFrameX == 0)
			{
				color = new Color(231, 43, 245, 0);
			}
			else
			if (Main.tile[i, j].TileFrameX == 18)
			{
				color = new Color(98, 43, 231, 0);
			}
			Main.spriteBatch.Draw(DDTextures.光晕2.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero+new Vector2(8,8), null, color* (1f-a), (float)((float)Main.time/1000), new Vector2(DDTextures.光晕2.Width(), DDTextures.光晕2.Height())/2, 0.05f, 0, 0f);
			return base.PreDraw(i, j, spriteBatch);
        }
    }
}
