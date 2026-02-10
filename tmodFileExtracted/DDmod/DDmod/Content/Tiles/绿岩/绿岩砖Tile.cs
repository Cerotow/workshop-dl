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
	public class 绿岩砖Tile : DDBlocks
    {
        public override int Sound =>0;
        public override int Dust => ModContent.DustType<绿岩粒子>();
        public override Color Color => new Color(200, 255, 200);
        public override void SetDefaults()
        {
            Main.tileMerge[ModContent.TileType<绿岩格网块Tile>()][Type] = true;
            Main.tileMerge[ModContent.TileType<绿岩Tile>()][Type] = true;
            DDGlobalTile.ForbidSpawn[Type] = true;
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if ( !NPCDowned.绿岩之视 && j > DDWorld.GreenRockLab.Y + 60)
            {
                fail = true;
            }
        }
        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
        {
            return !(!NPCDowned.绿岩之视 && j > DDWorld.GreenRockLab.Y + 60);
        }
        public override bool CanPlace(int i, int j)
        {
            return !(!NPCDowned.绿岩之视 && j > DDWorld.GreenRockLab.Y + 60);
        }
        public override bool Slope(int i, int j)
        {
            return !(!NPCDowned.绿岩之视 && j > DDWorld.GreenRockLab.Y + 60);
        }
    }
}