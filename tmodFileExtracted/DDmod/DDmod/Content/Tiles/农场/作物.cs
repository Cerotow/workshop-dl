using Microsoft.Xna.Framework;
using Terraria;
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
    public abstract class 作物 : ModTile
    {
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<菜TE>().Kill(i, j);
        }
        public 菜TE Entity(int i,int j)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Main.tile[i,j].TileType, 0, 0);
            
            return playerHelper.FindTileEntity2<菜TE>(i, j, tileData.Width, tileData.Height, 18);
        }
        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
        }
        public void MouseText(int i, int j, int Time, int Max, bool Right = false)
        {
            Player player = Main.LocalPlayer;
            bool flag18 = player.position.X / 16f - Player.tileRangeX <= Player.tileTargetX && (player.position.X + player.width) / 16f + Player.tileRangeX - 1f >= Player.tileTargetX && player.position.Y / 16f - Player.tileRangeY <= Player.tileTargetY && (player.position.Y + player.height) / 16f + Player.tileRangeY - 2f >= Player.tileTargetY;
            if(!flag18)
            {
                return;
            }
            if (Entity(i, j) == null)
            {
                player.cursorItemIconID = -1;
                player.cursorItemIconText = Language.GetTextValue("Mods.DDmod.TilesText.未知");
                return;
            }
            int A = Entity(i, j).Time / Time;

            player.cursorItemIconID = -1;
            if (A >= Max)
            {
                if (Right)
                {
                    player.cursorItemIconText = Language.GetTextValue("Mods.DDmod.TilesText.成熟2");

                    if (!Main.gamePaused)
                    {
                        TextureAssets.Cursors[0] = DDTextures.Nullpng;
                        TextureAssets.Cursors[1] = DDTextures.Nullpng;
                        TextureAssets.CursorRadial = DDTextures.Nullpng;
                        TextureAssets.Cursors[11] = DDTextures.Nullpng;
                        TextureAssets.Cursors[12] = DDTextures.Nullpng;
                        TextureAssets.LockOnCursor = DDTextures.Nullpng;
                        DDItemTextures.MouseTime = 2;
                    }
                }
                else
                {
                    player.cursorItemIconText = Language.GetTextValue("Mods.DDmod.TilesText.成熟");
                }
            }
            else
            {
                player.cursorItemIconText = Language.GetTextValue("Mods.DDmod.TilesText.阶段")+":" + (A+1) + "(" + (((float)Entity(i, j).Time / Time - A) * 100).ToString("F0") + "%)";
            }
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }
        public void Draw(int i, int j, SpriteBatch spriteBatch, int Time, int Max,bool variation = false,bool Wind = true)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Main.tile[i, j].TileType, 0, 0);
            if (Main.tile[i, j].TileFrameX % (18 * tileData.Width) == 0 && Main.tile[i, j].TileFrameY % (18 * tileData.Height) == 0)
            {
                bool R = true;
                for (int A = 0; A < DDTileDawnSystem.SpecialDraw.Count; A++)
                {
                    if (DDTileDawnSystem.SpecialDraw[A].tiles == new Point(i, j))
                    {
                        R = false;
                    }
                }
                if(R)
                DDTileDawnSystem.SpecialDraw.Add(new DDTileDawn(i, j, Main.tile[i, j].TileType, tileData.Width, tileData.Height, Time, Max, variation, Wind));
            }
            return;
            /*
            TileObjectData tileData = TileObjectData.GetTileData(Main.tile[i, j].TileType, 0, 0);
            Tile tile = Main.tile[i, j];
            if (Entity(i, j) == null)
            {
                return;
            }
            Vector2 zero = new Vector2(Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = (tile.TileFrameY == 36) ? 18 : 16;
            int frameXPos = (int)tile.TileFrameX;
            int frameYPos = (int)tile.TileFrameY;
            int A = Entity(i, j).Time / Time;
            if (A > Max)
            {
                A = Max;
            }
            if (variation && Entity(i, j).variation)
            {
                A++;
            }
            frameXPos += 18 * tileData.Width * A;
            if (Entity(i, j).direction < 0)
            {
                frameYPos += 18 * tileData.Height;
            }
            Color color = Lighting.GetColor(i, j, Color.White);
            spriteBatch.Draw(TextureAssets.Tile[Type].Value, new Vector2((i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero + new Vector2(8, 10), new Rectangle?(new Rectangle(frameXPos, frameYPos, 16, height)), color, 0f, new Vector2(8, 8), 1f, 0, 0f);
            */
        }
    }
}