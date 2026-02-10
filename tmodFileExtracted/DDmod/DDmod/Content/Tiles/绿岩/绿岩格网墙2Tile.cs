using DDmod.Content.Dusts;
using DDmod.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩格网墙2Tile : 绿岩格网墙Tile
	{
        public override string Texture => (GetType().Namespace + ".绿岩格网墙Tile").Replace('.', '/');
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(50, 105,50));
            DustType = ModContent.DustType<绿岩粒子>();

            HitSound = SoundID.Tink;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{	return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
		}
		public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void KillWall(int i, int j, ref bool fail)
        {
        }
    }
}