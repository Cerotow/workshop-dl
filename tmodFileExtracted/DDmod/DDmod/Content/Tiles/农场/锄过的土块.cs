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

namespace DDmod.Content.Tiles.农场
{
	public class 锄过的土块 : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMerge[2][ModContent.TileType<锄过的土块>()] = true;
			Main.tileMerge[ModContent.TileType<锄过的土块>()][2] = true;
			Main.tileMerge[ModContent.TileType<锄过的土块>()][0] = true;
			Main.tileMerge[0][ModContent.TileType<锄过的土块>()] = true;
			Main.tileMerge[477][ModContent.TileType<锄过的土块>()] = true;
            Main.tileMerge[ModContent.TileType<锄过的土块>()][477] = true;
            Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			DustType = 0;
			HitSound = SoundID.Dig;
            LocalizedText modTranslation = CreateMapEntryName();

			AddMapEntry(new Color(151, 107, 75), modTranslation);
		}
		public static Asset<Texture2D> asset;
		public override void Load()
		{
			asset = ModContent.Request<Texture2D>("DDmod/Content/Tiles/农场/锄过的湿润土块");

		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
		public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			yield return new Item(2);

        }
        public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
        {
            WorldGen.TileMergeAttempt(Type, 2, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (锄土.FindFirstTile(new Point16(i, j), out int type) >= 0)
			{
				DDWorld.土[type].tiles = new Point16(0, 0);
			}
		}
		public override bool KillSound(int i, int j, bool fail)
		{
			if (!fail)
			{
			}
			return base.KillSound(i, j, fail);
		}
		public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile tile = Main.tile[i, j];
			if (!锄土.FindFirstTileActive(new Point16(i, j)))
			{
				if(DDWorld.土==null|| DDWorld.土.Length<1000)
				{
                    DDWorld.土 = new 锄土[1000];
                }

				int T = 0;
				for (int a = 0; a < DDWorld.土.Length; a++)
				{
					if (DDWorld.土[a] == null)
					{
						DDWorld.土[a] = new 锄土(i, j);
					}
					if(DDWorld.土[a].tiles.X<0|| DDWorld.土[a].tiles.Y<0|| DDWorld.土[a].tiles.X>Main.maxTilesX|| DDWorld.土[a].tiles.Y>Main.maxTilesY)
					{
                        DDWorld.土[a].tiles = Point16.Zero;
                        continue;
					}
					if (Main.tile[DDWorld.土[a].tiles.X, DDWorld.土[a].tiles.Y].TileType == ModContent.TileType<锄过的土块>())
					{
						T++;
					}
				}
				if (T < 1000)
				{
					锄土.New(new Point16(i, j));
				}
				else
				{
					tile.TileType = 0;
                    WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
                    NetMessage.SendTileSquare(Main.LocalPlayer.whoAmI, Player.tileTargetX, Player.tileTargetY, 1);
                }
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
				Tile tile = Main.tile[i, j];
			if (锄土.FindFirstTile(new Point16(i, j), out int type) >= 0 && DDWorld.土[type].DampTime > 0)
			{
				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen)
				{
					zero = Vector2.Zero;
				}
				int height = (tile.TileFrameY == 36) ? 18 : 16;
				Main.spriteBatch.Draw(asset.Value, new Vector2((i * 16) - (int)Main.screenPosition.X, (j * 16) - (int)Main.screenPosition.Y) + zero, new Rectangle?(new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height)), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, 0, 0f);
				return false;
			}
			return true;
		}
	}
}