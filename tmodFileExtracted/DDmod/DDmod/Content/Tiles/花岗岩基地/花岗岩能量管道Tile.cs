using System;
using DDmod.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DDmod.Content.Tiles.花岗岩基地
{
	public class 花岗岩能量管道Tile : DDSimplifyBlocks
    {
        public override Vector3 LightColor => new Color(0,200,255).ToVector3()*1.5F;
        public override int Glow => ModContent.TileType<花岗岩能量管道Tile_Glow>();
        public override int Sound => 0;
        public override int Dust => 226;
        public override void SetDefaults()
        {
            MinPick = 350;
            Main.tileSolid[Type] = false;
        }
        public override bool Slope(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool KillSound(int i, int j, bool fail)
        {
            if (!fail)
            {
                PlaySound(SoundID.Shatter, new Vector2(i, j) * 16);
            }
            return base.KillSound(i, j, fail);
        }
        public override void NearbyEffects(int i, int j, bool closer)
		{
			Tile tile = Main.tile[i, j];
        }
	}
}
