using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩砖墙Tile : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(100, 155, 100));
			DustType = ModContent.DustType<绿岩粒子>();
            RegisterItemDrop(ModContent.ItemType<绿岩砖墙>());

            HitSound = SoundID.Tink;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			/*Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Vector2 drawOffset = new Vector2((float)(i * 16) - Main.screenPosition.X, (float)(j * 16) - Main.screenPosition.Y) + zero;
			spriteBatch.Draw(mod.GetTexture("Walls/科技花岗岩墙"), drawOffset, new Rectangle?(new Rectangle(Main.tile[i, j].wallFrameX(), Main.tile[i, j].wallFrameY(), 32, 32)), GetWallColour(i, j), 0f, new Vector2(8,8), 1f, 0, 0f);
			for (int W = 0; W <= Slime.instance.numPlayer; W++)
			{
				Player player = Main.player[W];
				float G = (player.Center - new Vector2(i * 16, j * 16)).Length() / 16;
				float I = (player.Center - new Vector2(i * 16, j * 16)).Length() - G;
				if (player.Distance(new Vector2(i * 16, j * 16)) < 100)
				{
					spriteBatch.Draw(mod.GetTexture("Walls/科技花岗岩墙光效"), drawOffset, new Rectangle?(new Rectangle(Main.tile[i, j].wallFrameX(), Main.tile[i, j].wallFrameY(), 32, 32)), Color.White * ((1 - I * 0.015F) * 1.25F), 0f, new Vector2(8, 8), 1f, 0, 0f);
				}
			}
			spriteBatch.Draw(mod.GetTexture("Walls/科技花岗岩墙光效"), drawOffset, new Rectangle?(new Rectangle(Main.tile[i, j].wallFrameX(), Main.tile[i, j].wallFrameY(), 32, 32)), Color.White *0.01F, 0f, new Vector2(8, 8), 1f, 0, 0f);*/
			return true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			/*for (int W = 0; W <= Slime.instance.numPlayer; W++)
			{
				Player player = Main.player[W];
				if (player.Distance(new Vector2(i * 16, j * 16)) < 100)
				{
					r = 0;
					g = 0.08f;
					b = 0.08f;
				}
			}*/
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void KillWall(int i, int j, ref bool fail)
        {
            if (!Main.hardMode && !NPCDowned.绿岩之视 && j > DDWorld.GreenRockLab.Y + 60)
            {
                fail = true;
            }
        }
	}
}