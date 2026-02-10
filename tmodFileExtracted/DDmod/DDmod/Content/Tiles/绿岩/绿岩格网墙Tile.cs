using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩格网墙Tile : ModWall
	{
		public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(50, 105,50));
            DustType = ModContent.DustType<绿岩粒子>();
            RegisterItemDrop(ModContent.ItemType<绿岩格网墙>());

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
            if ( !NPCDowned.绿岩之视 && j > DDWorld.GreenRockLab.Y + 60)
            {
                fail = true;
            }
        }
	}
}