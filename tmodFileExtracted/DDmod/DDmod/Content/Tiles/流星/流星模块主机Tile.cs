
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
using DDmod.Content.Items.Tiles.流星;

namespace DDmod.Content.Tiles.流星
{
    public class 流星模块主机Tile : ModTile
    {
        public const int NextStyleHeight = 56;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeSatOnForNPCs[Type] = true;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);

            DustType = 6;
            AdjTiles = new int[] { TileID.Chairs };

            AddMapEntry(new Color(200, 20, 20), Language.GetText("MapObject.Chair"));

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            NPCSpawnTE advancedEntity = ModContent.GetInstance<NPCSpawnTE>();
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(advancedEntity.Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
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

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, 60);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<NPCSpawnTE>().Kill(i, j);
        }
        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            /*
			if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance)) { 
				player.GamepadEnableGrappleCooldown();
				player.sitting.SitDown(player, i, j);
			}
			*/
            Tile tile = Main.tile[i, j];
            /*
            int left = i - (int)tile.TileFrameX % (2 * 16) / 16;
            int top = j - (int)tile.TileFrameY % (3 * 16) / 16;
            for (int k = left; k < left+2; k++)
            {
                for (int l = top; l < top + 3; l++)
                {
                    if (Main.tile[k, l].HasTile && Main.tile[k, l].TileType == tile.TileType)
                    {
                        if (Main.tile[k, l].TileFrameY < 18 * 3)
                        {
                            Main.tile[k, l].TileFrameY += (short)(18 * 3);
                        }
                        else
                        {
                            Main.tile[k, l].TileFrameY -= (short)(18 * 3);
                        }
                    }
                }
            }*/
            if (!player.IsWithinSnappngRangeToTile(i, j, 60))
            {
                return false;
            }

            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            tepowerCellFactory.Initiate = !tepowerCellFactory.Initiate;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = DDmod.Instance.GetPacket(256);
                //写入要发的包
                packet.Write((byte)DDType.NPCTE);
                packet.WriteVector2(new Vector2(tepowerCellFactory.Position.X, tepowerCellFactory.Position.Y));
                packet.Write(tepowerCellFactory.Initiate);
                //发出去
                packet.Send(-1, Main.myPlayer);
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //NetMessage.SendTileSquare(Main.myPlayer, left, top, 2, 3);
            }

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            if (!player.IsWithinSnappngRangeToTile(i, j, 60))
            {
                return;
            }

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<流星模块主机>();

            if (Main.tile[i, j].TileFrameX / 35 < 1)
            {
                player.cursorItemIconReversed = true;
            }
        }
        public Asset<Texture2D> TileTexture;
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
        public string T => "DDmod/Content/Tiles/流星/流星模块主机Tile_Glow";
        public override void HitWire(int i, int j)
        {
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            if ((Main.player[Player.FindClosest(new Vector2(i, j) * 16, 1, 1)].position - new Vector2(i, j) * 16).Length() > 600 || tepowerCellFactory == null || !tepowerCellFactory.Initiate)
            {
                return;
            }
            tepowerCellFactory.Canspawn = true;
            if (Main.netMode != 1)
            {
                if (tepowerCellFactory.Time % 300 == 0 && NPC.CountNPCS(ModContent.NPCType<自律工程模块>()) < 5)
                {
                    NewNPCs(new EntitySource_TileEntity(tepowerCellFactory), new Vector2(i + 1, j + 2) * 16, ModContent.NPCType<自律工程模块>(), 1, 0, 0, 255);
                }
            }
        }
        public override void Load()
        {
            if (!Main.dedServ)
            {
                TileTexture = ModContent.Request<Texture2D>(T);
            }
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = (tile.TileFrameY == 36) ? 18 : 16;
            TileObjectData tileData = TileObjectData.GetTileData(Type, 0, 0);
            NPCSpawnTE tepowerCellFactory = playerHelper.FindTileEntity2<NPCSpawnTE>(i, j, tileData.Width, tileData.Height, 18);
            if (tepowerCellFactory != null && tepowerCellFactory.Initiate)
            {
                Main.spriteBatch.Draw(TileTexture.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);
            }
        }
    }
}
