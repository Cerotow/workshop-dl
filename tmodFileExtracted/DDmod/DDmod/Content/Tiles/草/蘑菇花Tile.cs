using System;
using DDmod.Content.Items.Sundries;
using DDmod.Content.Items.农场.食物;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.草
{
	public class 蘑菇花Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileLighted[Type] = true;
			TileID.Sets.SwaysInWindBasic[Type] = false;
			HitSound = SoundID.Grass;
			AddMapEntry(new Color(33, 204, 249));
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,18
			};
			TileObjectData.newTile.DrawYOffset = 0;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				70
			};
			TileObjectData.addTile(Type);
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<蘑菇花>());
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 33;
            g = 204;
            b = 249;
            r *= 0.005F;
			g *= 0.005F;
			b *= 0.005F;
		}
    }
}
