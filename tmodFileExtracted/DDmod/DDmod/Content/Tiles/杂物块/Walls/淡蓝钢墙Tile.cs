using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Tiles.杂物块.Walls
{
	public class 淡蓝钢墙Tile : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(77, 92, 96));
			DustType = 268;

            HitSound = SoundID.NPCHit4;
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
			//fail = true;
		}
	}
}