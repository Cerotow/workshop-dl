using System;
using DDmod.Content.Items.Ammo;
using DDmod.Content.Items.Sundries;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Content.Items.农场.食物;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩罐子Tile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileLighted[Type] = true;
			HitSound = SoundID.NPCHit4;
			DustType = -1;
			AddMapEntry(new Color(33, 204, 249));
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,16
			};
			TileObjectData.newTile.DrawYOffset = 0;
			TileObjectData.newTile.RandomStyleRange = 3;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.Table | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			return base.TileFrame(i, j, ref resetFrame, ref noBreak);
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Vector2 vector = new Vector2(i, j + 1) * 16;

			if (Main.netMode != NetmodeID.Server)
			{
				if (frameX < 36)
				{
					int GoreType = Mod.Find<ModGore>("绿岩罐1").Type;
					Gore.NewGore(new EntitySource_TileBreak(i,j), vector, new Vector2(Main.rand.NextFloat(-2, 2), -Main.rand.NextFloat(0, 2)), GoreType, 1);
					GoreType = Mod.Find<ModGore>("绿岩罐2").Type;
					Gore.NewGore(new EntitySource_TileBreak(i,j), vector, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), GoreType, 1);
					GoreType = Mod.Find<ModGore>("绿岩罐3").Type;
					Gore.NewGore(new EntitySource_TileBreak(i,j), vector, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(0, 2)), GoreType, 1);
				}
				else if (frameX < 72)
                {
                    int GoreType = Mod.Find<ModGore>("绿岩罐4").Type;
                    Gore.NewGore(new EntitySource_TileBreak(i, j), vector, new Vector2(Main.rand.NextFloat(-2, 2), -Main.rand.NextFloat(0, 2)), GoreType, 1);
                    GoreType = Mod.Find<ModGore>("绿岩罐5").Type;
                    Gore.NewGore(new EntitySource_TileBreak(i, j), vector, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), GoreType, 1);
                    GoreType = Mod.Find<ModGore>("绿岩罐6").Type;
                    Gore.NewGore(new EntitySource_TileBreak(i, j), vector, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), GoreType, 1);
                    GoreType = Mod.Find<ModGore>("绿岩罐7").Type;
                    Gore.NewGore(new EntitySource_TileBreak(i, j), vector, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(0, 2)), GoreType, 1);
                }
				else
				{

                    int GoreType = Mod.Find<ModGore>("绿岩罐8").Type;
                    Gore.NewGore(new EntitySource_TileBreak(i, j), vector, new Vector2(-Main.rand.NextFloat(0, 2), -Main.rand.NextFloat(0, 2)), GoreType, 1);
                    GoreType = Mod.Find<ModGore>("绿岩罐9").Type;
                    Gore.NewGore(new EntitySource_TileBreak(i, j), vector, new Vector2(Main.rand.NextFloat(0, 2), -Main.rand.NextFloat(0, 2)), GoreType, 1);
                    GoreType = Mod.Find<ModGore>("绿岩罐10").Type;
                    Gore.NewGore(new EntitySource_TileBreak(i, j), vector, new Vector2(-Main.rand.NextFloat(0, 2), Main.rand.NextFloat(0, 2)), GoreType, 1);
                   GoreType = Mod.Find<ModGore>("绿岩罐11").Type;
                    Gore.NewGore(new EntitySource_TileBreak(i, j), vector, new Vector2(-Main.rand.NextFloat(0, 2), Main.rand.NextFloat(0, 2)), GoreType, 1);
                }
			}
		}
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
			if(fail)
			{

			}
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
		{
			int a = Main.rand.Next(40, 400);
			if (Main.rand.NextBool(20))
			{
				a = Main.rand.Next(1000, 10000);
			}
			if (Main.rand.NextBool(40))
			{
				a = Main.rand.Next(10000, 80000);
			}
			if (a >= 10000)
			{
				yield return new Item(73, a / 10000);
			}
			a %= 10000;
			if (a >= 100)
			{
				yield return new Item(72, a / 100);
			}
			a %= 100;
			if (a > 0)
			{
				yield return new Item(71, a);
			}
			if (Main.rand.NextBool(3))
			{
                yield return new Item(ModContent.ItemType<绿岩箭>(), Main.rand.Next(30, 100));
            }
			if (Main.rand.NextBool(2))
			{
				yield return new Item(ModContent.ItemType<绿岩砖>(), Main.rand.Next(5, 30));
			}
			else
			{
                yield return new Item(ModContent.ItemType<绿岩格网块>(), Main.rand.Next(5, 30));
            }
		}
	}
}
