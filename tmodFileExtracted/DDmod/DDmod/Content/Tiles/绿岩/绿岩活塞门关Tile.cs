using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Content.Tiles.杂物块;
using FullSerializer.Internal;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
	public class 绿岩活塞门关Tile : ModTile
	{
		public const int NextStyleHeight = 38;

		public override void SetStaticDefaults()
		{
            DDGlobalTile.LeftInvincible[Type] = true;
            DDGlobalTile.RightInvincible[Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;
			TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩活塞门关", out int GG);
			Main.tileGlowMask[Type] = (short)GG;


			DustType = ModContent.DustType<绿岩粒子>();

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.DrawYOffset = 0;
            TileObjectData.newTile.Origin = new Point16(0, 2);

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(0, 255, 0), CreateMapEntryName());
        }
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num =3;
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }
        public override void HitWire(int i, int j)
        {
            Tile Wiredens = Main.tile[DDSystem.Wiredens];
            if (Wiredens.TileType == ModContent.TileType<门禁按钮Tile>() && Wiredens.TileFrameY == 18)
            {
                Tile tile = Main.tile[i, j];
                int left = i - (int)tile.TileFrameX % (6 * 18) / 18;
                int top = j - (int)tile.TileFrameY % (2 * 18) / 18;

                for (int a = 0; a < 3; a++)
                {
                    tile = Main.tile[left, top + a];
                    tile.TileType = (ushort)ModContent.TileType<绿岩活塞门开Tile>();
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, left, top, 1, 3);
                }

                if (Wiring.running)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        Wiring.SkipWire(left, top + n);
                    }
                }
            }
        }

        public override bool RightClick(int i, int j)
        {
            return false;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = (tile.TileFrameY == 36 || tile.TileFrameY == 92) ? 18 : 16;
            Color color = Lighting.GetColor(i,j,Color.White);
            Main.spriteBatch.Draw(TextureAssets.Tile[Type].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), color, 0f, Vector2.Zero, 1f, 0, 0f);

            Main.spriteBatch.Draw(TextureAssets.GlowMask[Main.tileGlowMask[Type]].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
            /*
            if (Main.InSmartCursorHighlightArea(i, j, out var actuallySelected))
            {
                int num = (color.R + color.G + color.B) / 3;
                if (num > 10)
                {
                    Main.spriteBatch.Draw(TextureAssets.HighlightMask[Type].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), Colors.GetSelectionGlowColor(actuallySelected, num), 0f, Vector2.Zero, 1f, 0, 0f);
                }
            }*/


            return false;
        }
    }
}
