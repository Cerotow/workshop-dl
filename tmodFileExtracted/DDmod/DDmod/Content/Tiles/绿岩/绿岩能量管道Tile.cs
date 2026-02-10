using System;
using DDmod.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩能量管道Tile : DDSimplifyBlocks
    {
        public override Vector3 LightColor => new Color(100,255,100).ToVector3()*1.5F;
        public override Color Color => new Color(100,255,100);
        public override int Glow => ModContent.TileType<绿岩能量管道Tile_Glow>();
        public override int Sound => 0;
        public override int Dust => ModContent.DustType<绿岩电光粒子>();
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 3;
        }
        public override void SetDefaults()
        {
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            Main.tileSolid[Type] = false;
            MinPick = 100;
        }
        public override bool Slope(int i, int j)=>false;
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return true;
        }

        public override bool CanExplode(int i, int j) => false;
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
        }
	}
}
