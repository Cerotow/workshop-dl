using System;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Series.绿岩;
using DDmod.Content.Items.Tiles;
using DDmod.Content.Tiles.绿岩;
using DDmod.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.杂物块
{
	public class 门禁按钮Tile : DDCrystals
    {
        public override int Sound => 2;
        public override int Dust => 0;
        public override Color Color => new Color(100, 100, 100, 0);
        public override int Glow => -1;
        public override int Style => 1;
        public override bool Switch => true;
        public override void SetDefaults()
        {
            RegisterItemDrop(ModContent.ItemType<门禁按钮>());
            DDSystem.Instance.DDEquipGlow.TryGetValue("门禁按钮", out int GG);
            Main.tileGlowMask[Type] = (short)GG;
            TileID.Sets.HasOutlines[Type] = true;
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }
        public override bool KillSound(int i, int j, bool fail)
        {
            return false;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = Vector2.Zero;
            if (tile.TileFrameX == 0)
            {
                zero += new Vector2(2,0);
            }
            if (tile.TileFrameX == 36)
            {
                zero -= new Vector2(2, 0);
            }
            Color color = Lighting.GetColor(i, j, Color.White);
            if (Main.tile[i, j].TileColor != 0)
            {
                TileHelp.TileDrawCrystals(i, j, spriteBatch, Lighting.GetColor(i, j, WorldGen.paintColor(Main.tile[i, j].TileColor)), zero);
                TileHelp.TileDrawCrystals(i, j, spriteBatch, Color.White, zero, TextureAssets.GlowMask[Main.tileGlowMask[Type]].Value);
            }
            else
            {
                TileHelp.TileDrawCrystals(i, j, spriteBatch, color, zero);
                TileHelp.TileDrawCrystals(i, j, spriteBatch, Color.White, zero, TextureAssets.GlowMask[Main.tileGlowMask[Type]].Value);
            }
            zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            if (tile.TileFrameX == 0)
            {
                zero += new Vector2(2, 0);
            }
            if (tile.TileFrameX == 36)
            {
                zero -= new Vector2(2, 0);
            }
            if (Main.InSmartCursorHighlightArea(i, j, out var actuallySelected))
            {
                int num = (color.R + color.G + color.B) / 3;
                if (num > 10)
                {
                    Main.spriteBatch.Draw(TextureAssets.HighlightMask[Type].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, 16)), Colors.GetSelectionGlowColor(actuallySelected, num), 0f, Vector2.Zero, 1f, 0, 0f);
                }
            }

            return false;
        }
        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            Wiring.HitSwitch(i, j);
            NetMessage.SendData(59, -1, -1, null, i, j);
            PlaySound(DDHelper.SoundStyle(1,"解锁成功"),new Vector2(i,j)*16);

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<门禁按钮>();
        }

    }
}
