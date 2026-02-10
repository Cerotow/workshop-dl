using System;
using DDmod.Content.Dusts;
using DDmod.Content.Items.Boss.StarGuardItems;
using DDmod.Content.Items.Tiles;
using DDmod.Content.Items.农场.种子;
using DDmod.Content.Items.农场.食物;
using DDmod.UI.ItemUI;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.农场.果树
{
    public class 森林果树 : 作物
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.IsATreeTrunk[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            Main.tileAxe[Type] = true;
            Main.tileSolidTop[(int)Type] = false;
            Main.tileFrameImportant[(int)Type] = true;
            Main.tileNoAttach[(int)Type] = true;
            Main.tileLavaDeath[(int)Type] = true;
            Main.tileWaterDeath[(int)Type] = false;
            Main.tileLighted[Type] = true;
            HitSound = SoundID.Dig;
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16};
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Origin = new Point16(1, 4);
            TileObjectData.newTile.DrawYOffset = 2;
            菜TE advancedEntity = ModContent.GetInstance<菜TE>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(advancedEntity.Hook_AfterPlacement, -1, 0, false);
			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				2,477
			};
            TileObjectData.newTile.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(85, 158, 37));
        }
        int Time = DDHelper.Second(12);
        int Max = 5;
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            菜TE tepowerCellFactory = playerHelper.FindTileEntity2<菜TE>(i, j, tileData.Width, tileData.Height, 18);
            int A = tepowerCellFactory.Time / Time;
            if (A > Max)
            {
                A = Max;
            }
            if (A == Max)
            {
                yield return new Item(4009, 1);
                yield return new Item(4293, 1);
                yield return new Item(4282, 1);
                yield return new Item(4290, 1);
                yield return new Item(4291, 1);
                yield return new Item(ModContent.ItemType<果树种子>(),1);
            }
            if (A > 0)
            {
                if (A > Max - 1)
                {
                    A = Max - 1;
                }
                yield return new Item(9, Main.rand.Next(3, 8) * A);
            }
            yield return new Item(ModContent.ItemType<果树种子>());

        }
        public override bool KillSound(int i, int j, bool fail)
        {
            return CanKillTile(i, j, ref fail);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            /*
            case 2:
            case 477:
                passStyle = 910;
                break;
            case 60:
                passStyle = 914;
                break;
            case 70:
                passStyle = 912;
                break;
            case 23:
            case 112:
                passStyle = 915;
                break;
            case 199:
            case 234:
                passStyle = 916;
                break;
            case 53:
                passStyle = 911;
                break;
            case 116:
                passStyle = 919;
                break;
            case 147:
                passStyle = 913;
                break;
            case 633:
                passStyle = 1278;
                break;*/
                TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            菜TE tepowerCellFactory = playerHelper.FindTileEntity2<菜TE>(i, j, tileData.Width, tileData.Height, 18);
            Tile t = Main.tile[i, j];
            int left = i - t.TileFrameX % (tileData.Width * 18) / 18;
            int top = j - t.TileFrameY % (tileData.Height * 18) / 18;

            int A = tepowerCellFactory.Time / Time;
            if (A > Max)
            {
                A = Max;
            }
            if (!fail && Main.netMode != 2&& left==i&&top ==j)
            {
                if (A == 5 || A == 4)
                {
                    for (int L = 0; L < 4; L++)
                    {
                        for (int T = 0; T < 3; T++)
                        {
                            for (int a = 0; a < 4; a++)
                            {
                                Gore.NewGore(new EntitySource_TileEntity(tepowerCellFactory), new Vector2((float)left + L, (float)top + T) * 16f, Vector2.Zero, 910);
                            }
                        }
                    }
                }
                else if (A == 3)
                {
                    for (int L = 1; L < 3; L++)
                    {
                        for (int T = 1; T < 3; T++)
                        {
                            for (int a = 0; a < 4; a++)
                            {
                                Gore.NewGore(new EntitySource_TileEntity(tepowerCellFactory), new Vector2((float)left + L, (float)top + T) * 16f, Vector2.Zero, 910);
                            }
                        }
                    }
                }
                else
                {
                    for (int a = 0; a < 4; a++)
                    {
                        Gore.NewGore(new EntitySource_TileEntity(tepowerCellFactory), new Vector2((float)left + 2, (float)top + 3) * 16f, Vector2.Zero, 910);
                    }
                }
            }
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            菜TE tepowerCellFactory = playerHelper.FindTileEntity2<菜TE>(i, j, tileData.Width, tileData.Height, 18);
            菜TE tepowerCellFactory2 = playerHelper.FindTileEntity2<菜TE>(i-1, j, tileData.Width, tileData.Height, 18);
            菜TE tepowerCellFactory3 = playerHelper.FindTileEntity2<菜TE>(i+1, j, tileData.Width, tileData.Height, 18);

            if (Main.tile[i, j + 1].TileType != Type && tepowerCellFactory== tepowerCellFactory2 && tepowerCellFactory== tepowerCellFactory3)
            {
                return true;
            }
            return false;
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            for (int a = 0; a < 5; a++)
            {
                Dust.NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, 7, 0f, 0f, 1);
            }
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 0;
            if (CanKillTile(i, j, ref fail))
            {
                num = (fail ? 1 : 3);
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Draw(i, j, spriteBatch, Time, Max,false,false);
            return false;
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
        }
        public override void MouseOver(int i, int j)
        {
            MouseText(i, j, Time, Max,true);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY == 0 && tile.TileFrameX == 0)
            {
            }
        }
        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            菜TE tepowerCellFactory = playerHelper.FindTileEntity2<菜TE>(i, j, tileData.Width, tileData.Height, 18);

            int A = tepowerCellFactory.Time / Time;
            if (A > Max)
            {
                A = Max;
            }
            if (A == Max)
            {
                Main.LocalPlayer.QuickSpawnItem(new EntitySource_TileEntity(tepowerCellFactory), 4009, 1);
                Main.LocalPlayer.QuickSpawnItem(new EntitySource_TileEntity(tepowerCellFactory), 4293, 1);
                Main.LocalPlayer.QuickSpawnItem(new EntitySource_TileEntity(tepowerCellFactory), 4282, 1);
                Main.LocalPlayer.QuickSpawnItem(new EntitySource_TileEntity(tepowerCellFactory), 4290, 1);
                Main.LocalPlayer.QuickSpawnItem(new EntitySource_TileEntity(tepowerCellFactory), 4291, 1);
                tepowerCellFactory.Time = Time * 4;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = DDmod.Instance.GetPacket(256);
                    //写入要发的包
                    packet.Write((byte)DDType.菜TE);
                    packet.WriteVector2(new Vector2(tepowerCellFactory.Position.X, tepowerCellFactory.Position.Y));
                    packet.Write(Time * 4);
                    //发出去
                    packet.Send(-1, Main.myPlayer);
                }
            }
            return true;
        }
    }
}