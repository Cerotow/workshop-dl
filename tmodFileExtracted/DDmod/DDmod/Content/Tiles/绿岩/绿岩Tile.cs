using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using DDmod.Worlds;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;
using DDmod.Content.Items.Series.Heart;
using DDmod.Content.Items.Tiles.晶凝;
using DDmod.Content.Dusts;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩Tile  : DDBlocks
    {
        public override int Sound => 0;
        public override int Dust => ModContent.DustType<绿岩粒子>();
        public override Color Color => new Color(40, 105, 40);
        public override void SetDefaults()
        {
            Main.tileMerge[ModContent.TileType<绿岩格网块Tile>()][Type] = true;
            Main.tileMerge[ModContent.TileType<绿岩砖Tile>()][Type] = true;
        }
        public override void RandomUpdate(int i, int j)
        {
            if (TileLoader.CanPlace(i, j, ModContent.TileType<绿岩晶块Tile>()))
            {
                if (Main.rand.NextBool(80) && !Main.tile[i, j - 1].HasTile)
                {
                    WorldGen.PlaceTile(i, j - 1, ModContent.TileType<绿岩晶块Tile>(), true);
                }

                if (Main.rand.NextBool(80) && !Main.tile[i, j + 1].HasTile)
                {
                    WorldGen.PlaceTile(i, j + 1, ModContent.TileType<绿岩晶块Tile>(), true);
                }

                if (Main.rand.NextBool(80) && !Main.tile[i - 1, j].HasTile)
                {
                    WorldGen.PlaceTile(i - 1, j, ModContent.TileType<绿岩晶块Tile>(), true);
                }

                if (Main.rand.NextBool(80) && !Main.tile[i + 1, j].HasTile)
                {
                    WorldGen.PlaceTile(i + 1, j, ModContent.TileType<绿岩晶块Tile>(), true);
                }
            }
        }
    }
}