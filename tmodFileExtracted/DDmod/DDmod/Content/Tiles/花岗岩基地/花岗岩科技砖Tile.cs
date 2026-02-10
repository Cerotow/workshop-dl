using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;
using DDmod.Content.Dusts;

namespace DDmod.Content.Tiles.花岗岩基地
{
	public class 花岗岩科技砖Tile : DDBlocks
    {
        public override int Sound => 0;
        public override int Dust => ModContent.DustType<花岗岩电光粒子>();
        public override Color Color => new Color(20, 55, 155, 0);
        public override void SetDefaults()
        {
            MinPick = 350;
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
        }
        public override bool Slope(int i, int j)
        {
            return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return true;
        }
        public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}