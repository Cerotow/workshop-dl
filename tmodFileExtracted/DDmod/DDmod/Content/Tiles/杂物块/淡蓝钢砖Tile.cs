using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;
using DDmod.Content.Dusts;

namespace DDmod.Content.Tiles.杂物块
{
	public class 淡蓝钢砖Tile : DDBlocks
    {
        public override int Sound => -1;
        public override int Dust => 268;
        public override Color Color => new Color(155, 185, 192);
        public override void SetDefaults()
        {
            Main.tileMergeDirt[(int)Type] = false;
            HitSound = SoundID.NPCHit4;
            MinPick = 110;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return base.CreateDust(i, j, ref type);
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