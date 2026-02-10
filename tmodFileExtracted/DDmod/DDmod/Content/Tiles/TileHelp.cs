using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.Tiles.杂物块;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Tiles.草;
using DDmod.Players;
using Terraria.GameContent.Drawing;
using static Terraria.GameContent.Animations.Actions.Sprites;

namespace DDmod.Content.Tiles
{
    public static class TileHelp
    {
        public static void HitSwitch(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            ushort type = tile.TileType;
            if (type == ModContent.TileType<门禁按钮Tile>())
            {
                if (tile.TileFrameY == 0)
                {
                    tile.TileFrameY = 18;
                }
                else
                {
                    tile.TileFrameY = 0;
                }
                if (Main.netMode == 2)
                {
                    NetMessage.SendTileSquare(-1, i, j, 1, 1);
                }
            }
            if (type == ModContent.TileType<绿岩门禁Tile>())
            {
                tile.TileType = (ushort)ModContent.TileType<门禁按钮Tile>();
                tile.TileFrameY = 18;
                if (Main.netMode == 2)
                {
                    NetMessage.SendTileSquare(-1, i, j, 1, 1);
                }
            }
            if (type == ModContent.TileType<绿岩操作台Tile>())
            {
                if (tile.TileFrameY == 162)
                {
                    for (int a = 0; a < 4; a++)
                    {
                        for (int b = 0; b < 3; b++)
                        {
                            tile = Main.tile[i + a, j + b];
                            tile.TileFrameY = (short)(18 * b);
                        }
                    }
                }
                else
                {
                    for (int a = 0; a < 4; a++)
                    {
                        for (int b = 0; b < 3; b++)
                        {
                            tile = Main.tile[i + a, j + b];
                            tile.TileFrameY = (short)(162 + 18 * b);
                        }
                    }
                }
                if (Main.netMode == 2)
                {
                    NetMessage.SendTileSquare(-1, i, j, 4, 3);
                }
            }
        }
        public static void DrawSlope(this Tile tile, Texture2D TileTexture, SpriteBatch spriteBatch, Vector2 Position, Color color)
        {
            if (tile.Slope == 0 && !tile.IsHalfBlock)
            {
                spriteBatch.Draw(TileTexture, Position, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, 16)), color, 0f, Vector2.Zero, 1f, 0, 0f);
            }
            else
            if (tile.IsHalfBlock)
            {
                Rectangle rectangle = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 8);
                Main.spriteBatch.Draw(TileTexture, Position + new Vector2(0, 8), rectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            else
            {

                int num12 = (int)tile.Slope;
                int num13 = 2;
                for (int I = 0; I < 8; I++)
                {
                    int num14 = I * -2;
                    int num15 = 16 - I * 2;
                    int num16 = 16 - num15;
                    int num17;
                    switch (num12)
                    {
                        case 1:
                            num14 = 0;
                            num17 = I * 2;
                            num15 = 14 - I * 2;
                            num16 = 0;
                            break;
                        case 2:
                            num14 = 0;
                            num17 = 16 - I * 2 - 2;
                            num15 = 14 - I * 2;
                            num16 = 0;
                            break;
                        case 3:
                            num17 = I * 2;
                            break;
                        default:
                            num17 = 16 - I * 2 - 2;
                            break;
                    }

                    Main.spriteBatch.Draw(TileTexture, Position + new Vector2(num17, I * num13 + num14), new Rectangle(tile.TileFrameX + num17, tile.TileFrameY + num16, num13, num15), color, 0f, Vector2.Zero, 1f, 0, 0f);
                }

                int num18 = ((num12 <= 2) ? 14 : 0);
                Main.spriteBatch.Draw(TileTexture, Position + new Vector2(0f, num18), new Rectangle(tile.TileFrameX, tile.TileFrameY + num18, 16, 2), color, 0f, Vector2.Zero, 1f, 0, 0f);
            }
        }
        public static void Tileswitch(int type, int i, int j, int tileX, int tileY)
        {
            int x = i - Main.tile[i, j].TileFrameX / 18 % tileX;
            int y = j - Main.tile[i, j].TileFrameY / 18 % tileY;
            for (int k = x; k < x + tileX; k++)
            {
                for (int l = y; l < y + tileY; l++)
                {
                    if (Main.tile[k, l].HasTile && Main.tile[k, l].TileType == type)
                    {
                        if (Main.tile[k, l].TileFrameX < 18 * tileX)
                        {
                            Main.tile[k, l].TileFrameX += (short)(18 * tileX);
                        }
                        else
                        {
                            Main.tile[k, l].TileFrameX -= (short)(18 * tileX);
                        }
                    }
                }
            }
            if (Wiring.running)
            {
                for (int m = 0; m < tileX; m++)
                {
                    for (int n = 0; n < tileY; n++)
                    {
                        Wiring.SkipWire(x + m, y + n);
                    }
                }
            }
        }
        public static bool GetMerge(Tile myTile, Tile mergeTile)
        {
            return mergeTile.HasTile && (mergeTile.TileType == myTile.TileType || Main.tileMerge[(int)(myTile.TileType)][(int)(mergeTile.TileType)]);
        }
        public static void GetAdjacentTiles(int x, int y, out bool up, out bool down, out bool left, out bool right, out bool upLeft, out bool upRight, out bool downLeft, out bool downRight)
        {
            Tile tile = Main.tile[x, y];
            Tile north = Main.tile[x, y - 1];
            Tile south = Main.tile[x, y + 1];
            Tile west = Main.tile[x - 1, y];
            Tile east = Main.tile[x + 1, y];
            Tile southwest = Main.tile[x - 1, y + 1];
            Tile southeast = Main.tile[x + 1, y + 1];
            Tile northwest = Main.tile[x - 1, y - 1];
            Tile northeast = Main.tile[x + 1, y - 1];
            left = false;
            right = false;
            up = false;
            down = false;
            upLeft = false;
            upRight = false;
            downLeft = false;
            downRight = false;
            if (GetMerge(tile, north) && ((north.Slope == 0) || north.Slope == SlopeType.SlopeDownLeft || north.Slope == SlopeType.SlopeDownRight))
            {
                up = true;
                if (tile.IsHalfBlock || (tile.Slope == SlopeType.SlopeDownLeft || tile.Slope == SlopeType.SlopeDownRight))
                {
                    up = false;

                }
            }
            if (GetMerge(tile, south) && ((south.Slope == 0) || south.Slope == SlopeType.SlopeUpLeft || south.Slope == SlopeType.SlopeUpRight))
            {
                down = true;
                if ((south.Slope == SlopeType.SlopeDownLeft || south.Slope == SlopeType.SlopeDownRight) || south.IsHalfBlock || tile.Slope == SlopeType.SlopeUpLeft || tile.Slope == SlopeType.SlopeUpRight)
                {

                    down = false;
                }
            }
            if (GetMerge(tile, west) && ((west.Slope == 0) || west.Slope == SlopeType.SlopeDownRight || west.Slope == SlopeType.SlopeUpRight))
            {
                left = true;
                if (tile.Slope == SlopeType.SlopeDownRight || tile.Slope == SlopeType.SlopeUpRight)
                {

                    left = false;
                }
            }
            if (GetMerge(tile, east) && ((east.Slope == 0) || east.Slope == SlopeType.SlopeDownLeft || east.Slope == SlopeType.SlopeUpLeft))
            {
                right = true;
                if (tile.Slope == SlopeType.SlopeDownLeft || tile.Slope == SlopeType.SlopeUpLeft)
                {
                    right = false;
                }
            }
            if (GetMerge(tile, north) && GetMerge(tile, west) && GetMerge(tile, northwest) && (northwest.Slope == SlopeType.Solid || northwest.Slope == SlopeType.SlopeDownRight) && (north.Slope == SlopeType.Solid || north.Slope == SlopeType.SlopeDownLeft || north.Slope == SlopeType.SlopeUpLeft) && (west.Slope == SlopeType.Solid || west.Slope == SlopeType.SlopeUpLeft || west.Slope == SlopeType.SlopeUpRight))
            {
                upLeft = true;
            }
            if (GetMerge(tile, north) && GetMerge(tile, east) && GetMerge(tile, northeast) && (northeast.Slope == SlopeType.Solid || northeast.Slope == SlopeType.SlopeDownLeft) && (north.Slope == SlopeType.Solid || north.Slope == SlopeType.SlopeDownRight || north.Slope == SlopeType.SlopeUpRight) && (east.Slope == SlopeType.Solid || east.Slope == SlopeType.SlopeUpLeft || east.Slope == SlopeType.SlopeUpRight))
            {
                upRight = true;
            }
            if (GetMerge(tile, south) && GetMerge(tile, west) && GetMerge(tile, southwest) && !southwest.IsHalfBlock && (southwest.Slope == SlopeType.Solid || southwest.Slope == SlopeType.SlopeUpRight) && (south.Slope == SlopeType.Solid || south.Slope == SlopeType.SlopeDownLeft || south.Slope == SlopeType.SlopeUpLeft) && (west.Slope == SlopeType.Solid || west.Slope == SlopeType.SlopeDownLeft || west.Slope == SlopeType.SlopeDownRight))
            {
                downLeft = true;
            }
            if (GetMerge(tile, south) && GetMerge(tile, east) && GetMerge(tile, southeast) && !southeast.IsHalfBlock && (southeast.Slope == SlopeType.Solid || southeast.Slope == SlopeType.SlopeUpLeft) && (south.Slope == SlopeType.Solid || south.Slope == SlopeType.SlopeDownRight || south.Slope == SlopeType.SlopeUpRight) && (east.Slope == SlopeType.Solid || east.Slope == SlopeType.SlopeDownLeft || east.Slope == SlopeType.SlopeDownRight))
            {
                downRight = true;
            }
        }

        public static bool TileDrawCrystals(int i, int j, SpriteBatch spriteBatch, Color color, Texture2D texture = default, int TileGlow = -1, float offset = 2)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = Vector2.Zero;
            if (tile.TileFrameY == 0)
            {
                zero += new Vector2(0, offset);
            }
            if (tile.TileFrameY == 18)
            {
                zero += new Vector2(0, -offset);
            }
            if (tile.TileFrameY == 36)
            {

                zero += new Vector2(offset, 0);
            }
            if (tile.TileFrameY == 54)
            {

                zero += new Vector2(-offset, 0);
            }
            TileDrawCrystals(i, j, spriteBatch, color, zero, texture, TileGlow);
            return false;
        }
        public static bool TileDrawCrystals(int i, int j, SpriteBatch spriteBatch, Color color, Vector2 offset, Texture2D texture = default, int TileGlow = -1)
        {
            Tile tile = Main.tile[i, j];
            if (texture == default)
            {
                texture = TextureAssets.Tile[tile.TileType].Value;
                if (TileGlow == -1)
                {
                    Texture2D texture2 = Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady(tile.TileType, 0, tile.TileColor);
                    if (texture2 != null)
                    {
                        texture = texture2;
                    }
                }
                else
                {
                    Texture2D texture2 = Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady(TileGlow, 0, tile.TileColor);
                    if (texture2 != null)
                    {
                        texture = texture2;
                    }
                }
            }
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            zero += offset;
            spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, 16)), color, 0f, Vector2.Zero, 1f, 0, 0f);
            return false;
        }

        public static bool TileDraw(int i, int j, SpriteBatch spriteBatch, Color color, Texture2D texture = default, int TileGlow = -1)
        {
            Tile tile = Main.tile[i, j];
            if (texture == default)
            {
                texture = TextureAssets.Tile[tile.TileType].Value;
                if (TileGlow == -1)
                {
                    Texture2D texture2 = Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady(tile.TileType, 0, tile.TileColor);
                    if (texture2 != null)
                    {
                        texture = texture2;
                    }
                }
                else
                {
                    Texture2D texture2 = Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady(TileGlow, 0, tile.TileColor);
                    if (texture2 != null)
                    {
                        texture = texture2;
                    }
                }
            }
            //上方物块
            bool up;
            //下方物块
            bool down;
            //左边物块
            bool left;
            //右边物块
            bool right;
            //左上
            bool upLeft;
            //右上
            bool upRight;
            //左下
            bool downLeft;
            //右下
            bool downRight;
            TileHelp.GetAdjacentTiles(i, j, out up, out down, out left, out right, out upLeft, out upRight, out downLeft, out downRight);
            int Frame = tile.TileFrameY / 54;
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = (tile.TileFrameY == 36) ? 18 : 16;
            int OffsetX = 0;
            int OffsetY = 0;
            TileLoader.SetAnimationFrame(tile.TileType, i, j, ref OffsetX, ref OffsetY);
            if (tile.Slope == 0 && tile.IsHalfBlock)
            {

                //上方物块
                bool up2;
                //下方物块
                bool down2;
                //左边物块
                bool left2;
                //右边物块
                bool right2;
                //左上
                bool upLeft2;
                //右上
                bool upRight2;
                //左下
                bool downLeft2;
                //右下
                bool downRight2;
                spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y + 12)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX + OffsetX, (int)tile.TileFrameY + OffsetY + 12, 16, 4)), color, 0f, Vector2.Zero, 1f, 0, 0f);
                spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y + 8)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX + OffsetX, (int)tile.TileFrameY + OffsetY, 16, 4)), color, 0f, Vector2.Zero, 1f, 0, 0f);

                Point point = new Point(54, 18 + 54 * Frame);
                if (left)
                {
                    TileHelp.GetAdjacentTiles(i - 1, j, out up2, out down2, out left2, out right2, out upLeft2, out upRight2, out downLeft2, out downRight2);
                    if (!Main.tile[i - 1, j].IsHalfBlock || Main.tile[i - 1, j].Slope == SlopeType.SlopeDownRight || Main.tile[i - 1, j].Slope == SlopeType.SlopeUpRight)
                    {
                        if (!up2)
                        {
                            point = new Point(54, 0 + 54 * Frame);
                            spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle(point.X + OffsetX, point.Y + OffsetY, 2, 8)), color, 0f, Vector2.Zero, 1f, 0, 0f);

                        }
                        else
                        {
                            point = new Point(54, 18 + 54 * Frame);
                            spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle(point.X + OffsetX, point.Y + OffsetY, 2, 8)), color, 0f, Vector2.Zero, 1f, 0, 0f);

                        }
                    }
                }
                if (right)
                {
                    TileHelp.GetAdjacentTiles(i + 1, j, out up2, out down2, out left2, out right2, out upLeft2, out upRight2, out downLeft2, out downRight2);
                    if (!Main.tile[i + 1, j].IsHalfBlock || Main.tile[i + 1, j].Slope == SlopeType.SlopeDownLeft || Main.tile[i + 1, j].Slope == SlopeType.SlopeUpLeft)
                    {
                        if (!up2)
                        {
                            point = new Point(54, 0 + 54 * Frame);

                            spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X + 14), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle(point.X + OffsetX + 14, point.Y + OffsetY, 2, 8)), color, 0f, Vector2.Zero, 1f, 0, 0f);

                        }
                        else
                        {
                            point = new Point(54, 18 + 54 * Frame);

                            spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X + 14), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle(point.X + OffsetX + 14, point.Y + OffsetY, 2, 8)), color, 0f, Vector2.Zero, 1f, 0, 0f);

                        }
                    }
                }

                return false;
            }

            if ((down && !left && !right))
            {
                if (tile.Slope == SlopeType.SlopeDownLeft)
                {
                    for (int a = 0; a <= 16; a += 2)
                    {
                        spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X + a), (float)(j * 16 - (int)Main.screenPosition.Y + a)) + zero, new Rectangle?(new Rectangle(tile.TileFrameX + OffsetX + a, (int)tile.TileFrameY + OffsetY, 2, 16 - a)), color, 0f, Vector2.Zero, 1f, 0, 0f);

                    }
                    return false;
                }
                if (tile.Slope == SlopeType.SlopeDownRight)
                {
                    for (int a = 0; a <= 16; a += 2)
                    {
                        spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X + a - 2), (float)(j * 16 - (int)Main.screenPosition.Y + (16 - a))) + zero, new Rectangle?(new Rectangle(tile.TileFrameX + OffsetX + (16 - a), (int)tile.TileFrameY + OffsetY, 2, a)), color, 0f, Vector2.Zero, 1f, 0, 0f);
                    }
                    return false;
                }
            }
            if (tile.Slope == 0 && !tile.IsHalfBlock)
            {

                if (!up && !down && !left && !right)
                {
                    spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX + OffsetX, (int)tile.TileFrameY + OffsetY, 16, 16)), color, 0f, Vector2.Zero, 1f, 0, 0f);
                }
                else
                {
                    spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX + OffsetX, (int)tile.TileFrameY + OffsetY, 16, 16)), color, 0f, Vector2.Zero, 1f, 0, 0f);


                    if (Main.tile[i - 1, j].HasTile && Main.tile[i + 1, j].IsHalfBlock && Main.tile[i + 1, j].Slope == 0 && Main.tile[i - 1, j].TileType == Main.tile[i, j].TileType)
                    {
                        spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX + OffsetX, (int)tile.TileFrameY + OffsetY, 16, 16)), color, 0f, Vector2.Zero, 1f, 0, 0f);
                    }
                }
                return false;
            }
            return true;
        }
        public static bool TileFrame(int x, int y, bool resetFrame)
        {
            if (x < 0 || x >= Main.maxTilesX)
            {
                return false;
            }
            if (y < 0 || y >= Main.maxTilesY)
            {
                return false;
            }
            Tile tile = Main.tile[x, y];
            if (tile.Slope > 0 && TileID.Sets.HasSlopeFrames[(int)(tile.TileType)])
            {
                return true;
            }
            //上方物块
            bool up;
            //下方物块
            bool down;
            //左边物块
            bool left;
            //右边物块
            bool right;
            //左上
            bool upLeft;
            //右上
            bool upRight;
            //左下
            bool downLeft;
            //右下
            bool downRight;
            GetAdjacentTiles(x, y, out up, out down, out left, out right, out upLeft, out upRight, out downLeft, out downRight);
            /*
            int randomFrame;
            if (resetFrame)
            {
                randomFrame = WorldGen.genRand.Next(1);
                Main.tile[x, y].Get<TileWallWireStateData>().TileFrameNumber = randomFrame;
            }
            else
            {
                randomFrame = Main.tile[x, y].TileFrameNumber;
            }*/
            if (!up && !down && !left && !right)
            {
                tile.TileFrameX = 90;
                tile.TileFrameY = 36;
                return false;
            }
            if (!up && !down && left && !right)
            {
                tile.TileFrameX = 108;
                tile.TileFrameY = 0;
                return false;
            }
            if (!up && !down && !left && right)
            {
                tile.TileFrameX = 72;
                tile.TileFrameY = 0;
                return false;
            }
            if (!up && !down && left && right)
            {
                tile.TileFrameX = 90;
                tile.TileFrameY = 0;
                return false;
            }
            if (!up && down && !left && !right)
            {
                tile.TileFrameX = 54;
                tile.TileFrameY = 0;
                return false;
            }
            if (up && !down && !left && !right)
            {
                tile.TileFrameX = 54;
                tile.TileFrameY = 36;
                return false;
            }
            if (up && !down && left && right)
            {
                tile.TileFrameX = 18;
                tile.TileFrameY = 36;
                return false;
            }
            if (up && down && left && right)
            {
                tile.TileFrameX = 18;
                tile.TileFrameY = 18;
                return false;
            }
            if (!up && down && !left && right)
            {
                tile.TileFrameX = 0;
                tile.TileFrameY = 0;
                return false;
            }
            if (!up && down && left && right)
            {
                tile.TileFrameX = 18;
                tile.TileFrameY = 0;
                return false;
            }
            if (!up && down && left && !right)
            {
                tile.TileFrameX = 36;
                tile.TileFrameY = 0;
                return false;
            }
            if (up && down && !left && right)
            {
                tile.TileFrameX = 0;
                tile.TileFrameY = 18;
                return false;
            }
            if (up && down && left && !right)
            {
                tile.TileFrameX = 36;
                tile.TileFrameY = 18;
                return false;
            }
            if (up && !down && !left && right)
            {
                tile.TileFrameX = 0;
                tile.TileFrameY = 36;
                return false;
            }
            if (up && !down && left && !right)
            {
                tile.TileFrameX = 36;
                tile.TileFrameY = 36;
                return false;
            }
            if (up && down && !left && !right)
            {
                tile.TileFrameX = 54;
                tile.TileFrameY = 18;
                return false;
            }
            return true;
        }
        public static void GlowAdd(this Dictionary<string, int> DDGlow, string path, string name, ref int A)
        {
            Array.Resize(ref TextureAssets.GlowMask, TextureAssets.GlowMask.Length + 1);
            TextureAssets.GlowMask[TextureAssets.GlowMask.Length - 1] = ModContent.Request<Texture2D>(path);
            DDGlow.Add(name, TextureAssets.GlowMask.Length - 1);
            A++;
        }
        public static void TileGlow(Dictionary<string, int> DDGlow, out int A)
        {
            A = DDEquipGlowMask.A;
            A += Greenstone(DDGlow, A);
        }
        public static int Greenstone(Dictionary<string, int> DDGlow, int A)
        {
            string G = "Tile_Glow";
            string Text = "绿岩故障机器人";
            string path = "DDmod/Content/Tiles/绿岩/";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩吞噬者";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "报废的绿岩机器人";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩之视凹槽";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩锭预览图";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩电弧炉";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩井关";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩井锁";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩门禁";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩活塞门锁";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩活塞门关";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩操作台";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩监控机床";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩胶化舱";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩机器打印机";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩干扰器";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            path = "DDmod/Content/Tiles/绿岩/家具/";
            Text = "绿岩电子钟";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩吊灯";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩钢琴";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩工程灯";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩工作台";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩挂顶灯";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩门开";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩门关";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "上锁绿岩门";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩能量灯";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩平台";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩梳妆台";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩水槽";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩咖啡机";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩箱";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩小灯管";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩信息终端";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩椅";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩浴缸";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩桌";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩电脑";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩伺服器";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "绿岩书架";
            GlowAdd(DDGlow, path + Text + G, Text, ref A);

            Text = "门禁按钮";
            GlowAdd(DDGlow, "DDmod/Content/Tiles/杂物块/" + Text + G, Text, ref A);

            return A;
        }
    }
}