using DDmod.Content.Items.Melee.Sword;
using DDmod.Content.Items.农场.种子;
using DDmod.Content.NPCs.Boss.狱火蛇;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.Tiles.农场;
using DDmod.Content.Tiles.农场.果树;
using DDmod.Content.Tiles.绿岩;
using DDmod.Content.Tiles.绿岩.家具;
using DDmod.Content.Tiles.草;
using DDmod.Worlds;
using Terraria;
using Terraria.Enums;
using Terraria.IO;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles
{
    public class DDGlobalWall : GlobalWall
    {
        public override bool CanExplode(int i, int j, int type)
        {
            if (DDGlobalTile.WallInvincible[Main.tile[i,j].TileType])
            {
                return false;
            }
            return base.CanExplode(i, j, type);
        }
        public override void KillWall(int i, int j, int type, ref bool fail)
        {
            if (DDGlobalTile.WallInvincible[Main.tile[i, j].TileType])
            {
                fail = true;
            }
        }
    }

    public class DDGlobalTile : GlobalTile
    {
        /// <summary>
        /// 头顶无敌
        /// </summary>
        public static bool[] TopInvincible = TileID.Sets.Factory.CreateBoolSet(false);
        /// <summary>
        /// 左边无敌
        /// </summary>
        public static bool[] LeftInvincible = TileID.Sets.Factory.CreateBoolSet(false);
        /// <summary>
        /// 右边无敌
        /// </summary>
        public static bool[] RightInvincible = TileID.Sets.Factory.CreateBoolSet(false);
        /// <summary>
        /// 墙无敌
        /// </summary>
        public static bool[] WallInvincible = TileID.Sets.Factory.CreateBoolSet(false);
        /// <summary>
        /// 禁止刷怪
        /// </summary>
        public static bool[] ForbidSpawn = TileID.Sets.Factory.CreateBoolSet(false);
        public override void Load()
        {
        }
        public override void Drop(int i, int j, int type)
        {
            WorldGen.GetTreeBottom(i, j, out var x, out var y);
            TreeTypes treeType = WorldGen.GetTreeType(Main.tile[x, y].TileType);
            if (type == TileID.Trees && Main.rand.NextBool(18) && treeType == TreeTypes.Forest)
            {
                NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, ModContent.NPCType<AcornSpirit>(), 0, 0, 0, 1);
            }
            if ((type == 3 || type == 73) && Main.rand.NextBool(30) && Main.tile[i, j].TileFrameX <= 96)
            {
                if (Main.rand.NextBool(2))
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 8, 8), ModContent.ItemType<胡萝卜种子>());
                }
                else
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Rectangle(i * 16, j * 16, 8, 8), ModContent.ItemType<生菜种子>());
                }
            }
        }
        public static bool Invincible(int i, int j, int type)
        {
            if(Main.tileCut[type] || !Main.tileSolid[type] || TileObjectData.GetTileData(type, 0, 0) != null)
            {
                return false;
            }
            
            if (TopInvincible[Main.tile[i, j + 1].TileType] && Main.tile[i, j + 1].TileType != type)
            {
                return true;
            }
            if (LeftInvincible[Main.tile[i + 1, j].TileType] && Main.tile[i + 1, j].TileType != type)
            {
                return true;
            }
            if (RightInvincible[Main.tile[i - 1, j].TileType] && Main.tile[i - 1, j].TileType != type)
            {
                return true;
            }
            return false;
        }
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (Main.tile[i, j - 1].HasTile && Main.tile[i, j + 1].HasTile && Main.tile[i + 1, j].HasTile && Main.tile[i - 1, j].HasTile)
            {
                //return false;
            }
            if (Invincible(i, j, type))
            {
                blockDamaged = false;
                return false;
            }
            return base.CanKillTile(i, j, type, ref blockDamaged);
        }
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Invincible(i, j, type))
            {
                //fail = true;
                effectOnly = true;
            }
            if(type == ModContent.TileType<绿岩门禁Tile>())
            {
                fail = true;
            }
        }
        public override bool Slope(int i, int j, int type)
        {
            if (Main.tile[i, j - 1].HasTile && Main.tile[i, j + 1].HasTile && Main.tile[i + 1, j].HasTile && Main.tile[i - 1, j].HasTile)
            {
                //return false;
            }
            if (Invincible(i, j, type))
            {
                return false;
            }
            return base.Slope(i, j, type);
        }
        public override bool CanExplode(int i, int j, int type)
        {
            if (Invincible(i, j, type))
            {
                return false;
            }
            return base.CanExplode(i, j, type);
        }
        public override bool CanPlace(int i, int j, int type)
        {
            return base.CanPlace(i, j, type);
            if (Main.tile[i,j+1].TileType == ModContent.TileType<绿岩井关Tile>()|| Main.tile[i, j+1].TileType == ModContent.TileType<绿岩井锁Tile>())
            {
                if (Main.tileSolid[type])
                {
                    return false;
                }
            }
        }
        public override bool PreDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            return true;
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = (tile.TileFrameY == 36 || tile.TileFrameY == 92) ? 18 : 16;

            for (float A = 1; A > 0; A -= 0.075F)
            {
                Color color = Lighting.GetColor(i, j, new Color(255, 255, 255)*(1-A/4)); 
                Main.spriteBatch.Draw(TextureAssets.Tile[type].Value, new Vector2((float)(i * 16 + A*16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), color, 0f, Vector2.Zero, 1f, 0, 0f);
            }
            return true;
        }
        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
        }
        public override void RandomUpdate(int i, int j, int type)
        {
            Tile tile = Main.tile[i, j];
            if (type == 0)
            {
                if (Main.rand.NextBool(40) && j > Main.worldSurface && !Main.tile[i, j - 1].HasTile)
                {
                    WorldGen.PlaceObject(i, j - 1, ModContent.TileType<洞穴萝卜Tile>(), true, 0, 0, -1, -1);
                }
            }
            if (type == 109)
            {
                if (Main.rand.NextBool(80) && !Main.tile[i, j - 1].HasTile)
                {
                    int style = Main.rand.Next(0, 3);
                    WorldGen.PlaceObject(i, j - 1, ModContent.TileType<荧光花>(), true, style, 0, -1, -1);
                }
            }
            if (type == 70&&NPCDowned.觉醒星心双子)
            {
                
                if (Main.rand.NextBool(150) && j > Main.worldSurface)
                {
                    bool r = true;
                    for (int a = 0; a < 2; a++)
                    {
                        for (int b = 0; b < 2; b++)
                        {
                            if (!Main.tile[i + b, j - a - 1].HasTile || (Main.tile[i + b, j - a - 1].TileType == 71||Main.tile[i + b, j - a - 1].TileType == 519||Main.tile[i + b, j - a - 1].TileType == 528))
                                { }
                            else
                            {
                                r = false; break;
                            }
                        }
                    }
                    if(!Main.tile[i + 1, j].HasTile)
                    {
                        r = false;
                    }
                    if (r)
                    {
                        Tile tile2 = Main.tile[i+1, j];
                        tile.Slope = 0;
                        tile2.Slope = 0;
                        for (int a = 0; a < 2; a++)
                        {
                            for (int b = 0; b < 2; b++)
                            {
                                if (Main.tile[i + b, j - a-1].HasTile)
                                {
                                    WorldGen.KillTile(i + b, j - a - 1);
                                }
                            }
                        }
                        int style = Main.rand.Next(0, 3);
                        bool w = true;
                        for(int a= -20;a<=20;a++)
                        {
                            for (int b = -20; b <= 20; b++)
                            {
                                if (Main.tile[i + a, j + b].HasTile && Main.tile[i + a, j + b].TileType == ModContent.TileType<蘑菇花Tile>())
                                {
                                    w = false;
                                }
                            }

                        }
                        if(w)
                        WorldGen.PlaceObject(i, j - 2, ModContent.TileType<蘑菇花Tile>(), true, style, 0, -1, -1);
                    }
                }
            }
        }
    }
}