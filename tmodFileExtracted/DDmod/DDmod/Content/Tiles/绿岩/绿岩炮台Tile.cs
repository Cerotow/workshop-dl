
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
using DDmod.UI;
using Humanizer;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.Worlds;

namespace DDmod.Content.Tiles.绿岩
{
    public class 绿岩炮台Tile : ModTile
    {
        public const int NextStyleHeight = 56;
        public static Asset<Texture2D> asset;
        public override void Load()
        {
            if(Main.netMode!=2)
            {
                asset = ModContent.Request<Texture2D>("DDmod/Content/Tiles/绿岩/绿岩炮口Tile");
            }
        }

        public override void SetStaticDefaults()
        {
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileHammeringIfOnTopOfIt[Type] = true;

            DustType = ModContent.DustType<绿岩粒子>();
            MinPick = 100;
            AddMapEntry(new Color(100, 255, 100), CreateMapEntryName());

            TileObjectData.newTile.CopyFrom(TileObjectData.Style6x3);
            TileObjectData.newTile.Width =2;
            TileObjectData.newTile.Height =2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.newTile.Origin = new Point16(1,1);
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
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, j * 16 + 8), ModContent.ItemType<绿岩砖>(), Main.rand.Next(2, 4));
                }
                else
                {
                    Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16 + 8, j * 16 + 8), ModContent.ItemType<绿岩格网块>(), Main.rand.Next(2, 4));

                }
            }
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
        }
        int SpawnTime = 75;


        public override void HitWire(int i, int j)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            if (tepowerCellFactory == null || (Main.player[tepowerCellFactory.player].position - new Vector2(i, j) * 16).Length() > 1500 || !tepowerCellFactory.Initiate || tepowerCellFactory.Wire)
            {
                if(tepowerCellFactory != null)
                    tepowerCellFactory.Canspawn = true;
                return;
            }

            tepowerCellFactory.Wire = true;
            tepowerCellFactory.Canspawn = true;

            //到达关键帧需要同步
            Tile tile = Main.tile[i, j];
            if(tepowerCellFactory.SpawnTime==0)
            {
                return;
            }
            if (tepowerCellFactory.Time % (tepowerCellFactory.SpawnTime) == 0)
            {
                if (Main.netMode != 1)
                {
                    if (tile.TileFrameX == 0)
                    {
                        NewProjectile(new EntitySource_TileEntity(tepowerCellFactory), new Vector2(tepowerCellFactory.Position.X+1, tepowerCellFactory.Position.Y+1)*16 - new Vector2(-12, 8), new Vector2(2,0), ModContent.ProjectileType<绿岩激光>(), 20, 0, -1, 0, 1);
                    }
                    else
                    {
                        NewProjectile(new EntitySource_TileEntity(tepowerCellFactory), new Vector2(tepowerCellFactory.Position.X+1, tepowerCellFactory.Position.Y+1)*16 - new Vector2(10, 8), new Vector2(-2, 0), ModContent.ProjectileType<绿岩激光>(), 20, 0, -1, 0, -1);
                    }
                }
            }
        }
        public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
        {
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D texture = TextureAssets.Tile[tile.TileType].Value;
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
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity3<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18,18);
            if (tepowerCellFactory != null)
            {

                if (tile.TileFrameX >= 36)
                {
                    spriteBatch.Draw(texture, new Vector2(i, j) * 16 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX-36, tile.TileFrameY, 16, height), Lighting.GetColor(new Point(i, j)), 0f, Vector2.Zero, 1f, 0, 0f);
                }
                else
                {

                    spriteBatch.Draw(texture, new Vector2(i, j) * 16 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), Lighting.GetColor(new Point(i, j)), 0f, Vector2.Zero, 1f, 0, 0f);

                }
                if (tile.TileFrameX%36==18 && tile.TileFrameY == 18)
                {
                    if (tile.TileFrameX==18)
                    {
                        spriteBatch.Draw(asset.Value, new Vector2(i, j) * 16 - new Vector2(8, 8) - Main.screenPosition + zero, null, Lighting.GetColor(new Point(i, j)), 0f, asset.Size() / 2, 1f, 0, 0f);
                    }
                    else
                    {
                        spriteBatch.Draw(asset.Value, new Vector2(i, j) * 16 - new Vector2(-8, 8) - Main.screenPosition + zero, null, Lighting.GetColor(new Point(i, j)), 0f, asset.Size() / 2, 1f, SpriteEffects.FlipHorizontally, 0f);
                    }
                }
            }
            return false;
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            //发射频率
            short T = 0;
            if (Main.LocalPlayer.direction!=1)
            {
                //转向
                T += 36;
            }
            Tile tile = Main.tile[i, j];
            tile.TileFrameX += (short)(T);
            tile = Main.tile[i-1, j];
            tile.TileFrameX += T;
            tile = Main.tile[i, j-1];
            tile.TileFrameX += (short)(T);
            tile = Main.tile[i-1, j-1];
            tile.TileFrameX += T;
            NetMessage.SendTileSquare(Main.myPlayer, i-1, j-1, 2, 2);

        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
        }
    }
}
