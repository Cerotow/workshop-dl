using DDmod.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩墙Tile : ModWall
	{
		public override void SetStaticDefaults()
		{
            Main.wallHouse[Type] = true;
            AddMapEntry(new Color(100, 155, 100));
            DustType = ModContent.DustType<绿岩粒子>();

            HitSound = SoundID.Tink;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{return true;
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