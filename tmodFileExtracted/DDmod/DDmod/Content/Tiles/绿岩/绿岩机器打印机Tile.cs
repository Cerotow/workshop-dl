
using DDmod.Content.Items.Tiles.晶凝;
using DDmod.Content.Items.Tiles.花岗岩基地;
using DDmod.UI.PlaystationUI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using DDmod.Content.NPCs.IittleMonster;
using DDmod.Content.Dusts;
using DDmod.Content.NPCs.IittleMonster.绿岩;
using DDmod.Content.Projectiles.Hostile;
using DDmod.Content.Tiles.杂物块;
using DDmod.Content.Items.Melee.Sword;
using DDmod.Worlds;
using DDmod.Content.Items.Tiles.绿岩;

namespace DDmod.Content.Tiles.绿岩
{
    public class 绿岩机器打印机Tile : ModTile
    {
        public const int NextStyleHeight = 56;
        public override void Load()
        {
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            if (tepowerCellFactory != null)
            {
                if (tepowerCellFactory.Initiate)
                {
                    r = 0.1f;
                    g = 0.3f;
                    b = 0.1f;
                }
            }
        }
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            MinPick = 100;

            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩机器打印机", out int GG);
            Main.tileGlowMask[Type] = (short)GG;
            DustType = ModContent.DustType<绿岩粒子>();

            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());

            TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
            TileObjectData.newTile.Origin = new Point16(2, 4);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            NPCSpawnTE advancedEntity = ModContent.GetInstance<NPCSpawnTE>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(advancedEntity.Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            if (tepowerCellFactory != null)
            {
                return !tepowerCellFactory.SpawnRunning;
            }
            return base.CanKillTile(i, j, ref blockDamaged);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<NPCSpawnTE>().Kill(i, j);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!NPCDowned.绿岩之视)
            {
                fail = true;
            }
            if (!fail)
            {
                if (Main.rand.NextBool(2))
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, j * 16 + 8), ModContent.ItemType<绿岩砖>(), Main.rand.Next(4, 8));
                }
                else
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, j * 16 + 8), ModContent.ItemType<绿岩格网块>(), Main.rand.Next(4, 8));

                }
            }
        }
        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
        {
            return !(!NPCDowned.绿岩之视);
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile t = Main.tile[i, j];
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            if (tepowerCellFactory == null)
            {
                if (Main.netMode != 2)
                {
                    ModContent.GetInstance<NPCSpawnTE>().Hook_AfterPlacement(i, j, Type, 0, 1, 0);
                }
            }
            else
            {
                if (Main.netMode == 1)
                {
                    Main.LocalPlayer.Dplayer().AddTE(tepowerCellFactory.Position);
                }
            }
        }
        int SpawnTime = 1200;


        public override void HitWire(int i, int j)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            Tile Wiredens = Main.tile[DDSystem.Wiredens];
            if(tepowerCellFactory == null)
            {
                return;
            }
            if (((Main.player[tepowerCellFactory.player].position - new Vector2(i, j) * 16).Length() > 600 || !tepowerCellFactory.Initiate || tepowerCellFactory.Wire))
            {
                if (!tepowerCellFactory.SpawnRunning)
                {
                    if (tepowerCellFactory.Wire && tepowerCellFactory.WireTime == 0)
                    {
                        if (Wiredens.TileType == ModContent.TileType<绿岩操作台Tile>())
                        {
                            if (Wiredens.TileFrameY == 0)
                            {
                                tepowerCellFactory.Initiate = false;
                            }
                            else
                            {
                                tepowerCellFactory.Initiate = true;
                            }
                        }
                        tepowerCellFactory.WireTime = 3;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.TileEntitySharing, number: tepowerCellFactory.ID, number2: tepowerCellFactory.Position.X, number3: tepowerCellFactory.Position.Y);
                        }
                    }
                    tepowerCellFactory.Wire = true;
                    return;
                }
                else
                {
                    if (Wiredens.TileType == ModContent.TileType<绿岩操作台Tile>())
                    {
                        if (Wiredens.TileFrameY == 0)
                        {
                            Wiredens.TileFrameY = 162;
                            tepowerCellFactory.Initiate = true;
                        }
                    }
                }
            }
                tepowerCellFactory.Wire = true;
            tepowerCellFactory.Canspawn = true;
            //到达关键帧需要同步

            if ((tepowerCellFactory.Time % SpawnTime) == 105)
            {
                tepowerCellFactory.SpawnRunning = false;
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.TileEntitySharing, number: tepowerCellFactory.ID, number2: tepowerCellFactory.Position.X, number3: tepowerCellFactory.Position.Y);
            }
            if (tepowerCellFactory.Time % SpawnTime == 0)
            {
                //if (DNPC.CountNPCS(ModContent.NPCType<绿岩侦察机>(), new Vector2(i+2.5F, j+2.5F) * 16, 16*20, out int Max) < 3 && Max < 40)
                if (DNPC.TileCountNPCS(ModContent.NPCType<绿岩侦察机>(), tepowerCellFactory.Position)<3 && NPCDowned.绿岩刷怪)
                {
                    tepowerCellFactory.SpawnRunning = true;
                    if (Main.netMode != 1)
                    {
                        Tile tile = Main.tile[i, j];
                        if (tile.TileFrameX < 90)
                        {
                            int A= NewProjectile(new EntitySource_TileEntity(tepowerCellFactory), new Vector2(i, j + 2) * 16 - new Vector2(4, -3), Vector2.Zero, ModContent.ProjectileType<绿岩打印>(), 0, 0, -1, 0, 0, 0);
                            Main.projectile[A].DProj().vector[0].X = tepowerCellFactory.Position.X;
                            Main.projectile[A].DProj().vector[0].Y = tepowerCellFactory.Position.Y;
                        }
                        else
                        {
                            int A = NewProjectile(new EntitySource_TileEntity(tepowerCellFactory), new Vector2(i + 5, j + 2) * 16 - new Vector2(4, -3), Vector2.Zero, ModContent.ProjectileType<绿岩打印>(), 0, 0, -1, 0, 0, 1);
                            Main.projectile[A].DProj().vector[0].X = tepowerCellFactory.Position.X;
                            Main.projectile[A].DProj().vector[0].Y = tepowerCellFactory.Position.Y;
                        }
                        if (Main.netMode == 2)
                            NetMessage.SendData(MessageID.TileEntitySharing, number: tepowerCellFactory.ID, number2: tepowerCellFactory.Position.X, number3: tepowerCellFactory.Position.Y);
                    }
                }
                else
                {
                    tepowerCellFactory.Time += 105;
                    tepowerCellFactory.SpawnRunning = false;
                    if (Main.netMode == 2)
                        NetMessage.SendData(MessageID.TileEntitySharing, number: tepowerCellFactory.ID, number2: tepowerCellFactory.Position.X, number3: tepowerCellFactory.Position.Y);
                }
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture = TextureAssets.Tile[tile.TileType].Value;
            Texture2D Glow = TextureAssets.GlowMask[Main.tileGlowMask[Type]].Value;
            if (tile.TileColor > 0)
            {
                texture = Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady(tile.TileType, 0, tile.TileColor);
            }
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = 18;
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            if (tepowerCellFactory != null)
            {
                if (NPCDowned.绿岩刷怪 && (tepowerCellFactory.Initiate || tepowerCellFactory.SpawnRunning))
                {
                    if ((Main.player[tepowerCellFactory.player].position - new Vector2(tepowerCellFactory.Position.X, tepowerCellFactory.Position.Y) * 16).Length() <= 600 || tepowerCellFactory.SpawnRunning)
                    {
                        Point16 point = new Point16(270, 90 * 6);
                        if (tile.TileFrameX < 90)
                        {
                            if (tepowerCellFactory.Time % SpawnTime >= 75 && tepowerCellFactory.Time % SpawnTime < 105)
                            {
                                point = new Point16(270, 90 * (((tepowerCellFactory.Time % SpawnTime - 75) / 5)));
                            }
                            if (tepowerCellFactory.Time % SpawnTime < 75)
                            {
                                int F = (tepowerCellFactory.Time / 5) % 15;
                                point = new Point16(180, 90 * F);
                            }

                            point += new Point16(tile.TileFrameX, tile.TileFrameY);
                        }
                        else
                        {
                            point = new Point16(450, 90 * 6);
                            if (tepowerCellFactory.Time % SpawnTime >= 75 && tepowerCellFactory.Time % SpawnTime < 105)
                            {
                                point = new Point16(450, 90 * (((tepowerCellFactory.Time % SpawnTime - 75) / 5)));
                            }
                            if (tepowerCellFactory.Time % SpawnTime < 75)
                            {
                                int F = (tepowerCellFactory.Time / 5) % 15;
                                point = new Point16(360, 90 * F);
                            }

                            point += new Point16(tile.TileFrameX - 90, tile.TileFrameY);
                        }
                        spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle(point.X, point.Y, 16, height), Lighting.GetColor(new Point(i, j)), 0f, Vector2.Zero, 1f, 0, 0f);
                        spriteBatch.Draw(Glow, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle(point.X, point.Y, 16, height), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
                    }
                    else
                    {
                         spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Lighting.GetColor(new Point(i, j)), 0f, Vector2.Zero, 1f, 0, 0f);
                         spriteBatch.Draw(Glow, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
                    }
                }
                else
                {
                    spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Lighting.GetColor(new Point(i, j)), 0f, Vector2.Zero, 1f, 0, 0f);
                    spriteBatch.Draw(Glow, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
                }
                return false;
            }
            return true;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
        }
    }
}
