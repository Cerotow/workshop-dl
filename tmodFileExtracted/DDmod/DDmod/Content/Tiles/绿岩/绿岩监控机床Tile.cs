
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
using DDmod.Worlds;
using DDmod.Content.Items.Tiles.绿岩;

namespace DDmod.Content.Tiles.绿岩
{
    public class 绿岩监控机床Tile : ModTile
    {
        public const int NextStyleHeight = 56;
        public override void Load()
        {
        }

        public override void SetStaticDefaults()
        {
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;

            DustType = ModContent.DustType<绿岩粒子>();
            MinPick = 100;
            DDSystem.Instance.DDEquipGlow.TryGetValue("绿岩监控机床", out int GG);
            Main.tileGlowMask[Type] = (short)GG;

            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.Width = 4;

            TileObjectData.newTile.CoordinatePaddingFix = new Point16(2, 2);
            NPCSpawnTE advancedEntity = ModContent.GetInstance<NPCSpawnTE>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(advancedEntity.Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.StyleHorizontal = true;
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
                    ModContent.GetInstance<NPCSpawnTE>().Hook_AfterPlacement(i, j, Type, 0, 0, 0);
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
        int SpawnTime = 120;


        public override void HitWire(int i, int j)
        {
            SpawnTime = 120;
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            Tile Wiredens = Main.tile[DDSystem.Wiredens];
            if (tepowerCellFactory == null)
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
            if (DNPC.TileCountNPCS(ModContent.NPCType<绿岩监控无人机>(), tepowerCellFactory.Position) ==0 && NPCDowned.绿岩刷怪)
            {
                //到达关键帧需要同步
                if ((tepowerCellFactory.Time % SpawnTime) == SpawnTime - 5 * 4)
                {
                    tepowerCellFactory.SpawnRunning = true;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.TileEntitySharing, number: tepowerCellFactory.ID, number2: tepowerCellFactory.Position.X, number3: tepowerCellFactory.Position.Y);
                }
                else if ((tepowerCellFactory.Time % SpawnTime) == 5 * 4)
                {
                    tepowerCellFactory.SpawnRunning = false;
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.TileEntitySharing, number: tepowerCellFactory.ID, number2: tepowerCellFactory.Position.X, number3: tepowerCellFactory.Position.Y);
                }
                if (tepowerCellFactory.Time % SpawnTime == 0)
                {
                    if (Main.netMode != 1)
                    {
                        int A = NewNPCs(new EntitySource_TileEntity(tepowerCellFactory), new Vector2(i +2, j + 2) * 16, ModContent.NPCType<绿岩监控无人机>(), 1);
                        Main.npc[A].velocity.X = (Main.rand.NextFloat(-1, 1));
                        Main.npc[A].velocity.Y =-2;
                        Main.npc[A].Dnpc().TETile = tepowerCellFactory.Position;
                        Main.npc[A].netUpdate = true;
                        tepowerCellFactory.Dust = true;

                        if (Main.netMode == 2)
                            NetMessage.SendData(MessageID.TileEntitySharing, number: tepowerCellFactory.ID, number2: tepowerCellFactory.Position.X, number3: tepowerCellFactory.Position.Y);
                    }
                }
            }
            else
            {
                if ((tepowerCellFactory.Time % SpawnTime) == SpawnTime - 5 * 4)
                {
                    tepowerCellFactory.Time = 20;
                }
                tepowerCellFactory.SpawnRunning = false;
            }
        }
        public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            if (Main.netMode != 2 && tepowerCellFactory.Dust)
            {
                for (int A = 0; A < 50; A++)
                {
                    NewDust(new Vector2(i + 2, j + 2) * 16 - new Vector2(4), 32, 5, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-3, 3), -Main.rand.NextFloat(4, 8), Scale: Main.rand.NextFloat(0.5F, 1.5F));
                }

                tepowerCellFactory.Dust = false;
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
                EmitParticles(i, j, tile, tile.TileFrameX, tile.TileFrameY, Lighting.GetColor(new Point(i, j)), true);
                if (NPCDowned.绿岩刷怪 && (tepowerCellFactory.Initiate || tepowerCellFactory.SpawnRunning))
                {
                    if ((Main.player[tepowerCellFactory.player].position - new Vector2(tepowerCellFactory.Position.X, tepowerCellFactory.Position.Y) * 16).Length() <= 600 || tepowerCellFactory.SpawnRunning)
                    {
                        //开机
                        Point16 point = new Point16(72, 38 * ((tepowerCellFactory.Time / 5) % 6));
                        //上餐中
                        if (tepowerCellFactory.Time % SpawnTime > SpawnTime - 5 * 4)
                        {
                            int F = (tepowerCellFactory.Time % SpawnTime) - (SpawnTime - 5 * 4);
                            F /= 5;
                            point = new Point16(144, 38 * F);
                        }
                        if (tepowerCellFactory.Time % SpawnTime < 5 * 4)
                        {
                            int F = (tepowerCellFactory.Time % SpawnTime);
                            F /= 5;
                            point = new Point16(144, 38 * (F + 3));
                        }

                        point += new Point16(tile.TileFrameX, tile.TileFrameY);
                        spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle(point.X, point.Y, 16, height), Lighting.GetColor(new Point(i, j)), 0f, Vector2.Zero, 1f, 0, 0f);
                        spriteBatch.Draw(Glow, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle(point.X, point.Y, 16, height), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
                    }
                    else
                    {
                        spriteBatch.Draw(texture, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle(tile.TileFrameX + 72, tile.TileFrameY, 16, height), Lighting.GetColor(new Point(i, j)), 0f, Vector2.Zero, 1f, 0, 0f);
                        spriteBatch.Draw(Glow, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle(tile.TileFrameX + 72, tile.TileFrameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
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
