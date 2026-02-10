using DDmod.Content.Dusts;
using DDmod.Content.Items.Tiles.晶凝;
using DDmod.Content.Items.Tiles.绿岩;
using DDmod.UI.PlaystationUI;
using DDmod.Worlds;
using Humanizer;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace DDmod.Content.Tiles.绿岩
{
    public class 绿岩存储仓Tile : DDChest
    {
        public override int Icon => 0;
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public static int AfterPlacement_Hook(int x, int y, int type = 21, int style = 0, int direction = 1, int alternate = 0)
        {
            Point16 baseCoords = new Point16(x, y);
            TileObjectData.OriginToTopLeft(type, style, ref baseCoords);
            int num = Chest.FindEmptyChest(baseCoords.X, baseCoords.Y);
            if (num == -1)
                return -1;

            if (Main.netMode != 1)
            {
                Chest chest = new Chest();
                chest.x = baseCoords.X;
                chest.y = baseCoords.Y;
                for (int i = 0; i < 40; i++)
                {
                    chest.item[i] = new Item();
                }

                Main.chest[num] = chest;
            }
            else
            {
                NetMessage.SendData(34, -1, -1, null, 100, x, y, style, 0, type, 0);
            }

            return num;
        }
        public override Color Color => new Color(100,255,100);
        public override void SetStaticDefaults()
        {
            if (LightColor != Vector3.Zero)
            {
                Main.tileLighted[Type] = true;
            }
            DDGlobalTile.WallInvincible[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileContainer[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileOreFinderPriority[Type] = 500;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.BasicChest[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;

            DustType = ModContent.DustType<绿岩粒子>();

            AddMapEntry(Color, Language.GetText("Mods.DDmod.Tiles.绿岩存储仓Tile.MapEntry"), 绿岩存储仓Tile.MapName);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };

            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(AfterPlacement_Hook, -1, 0, false);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorWall = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData();
            TileObjectData.newTile.Origin = new Point16(2, 2);
            SetDefaults();
            TileObjectData.addTile(Type);
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            left -= (tile.TileFrameX % 72) / 18;
            top -= (tile.TileFrameY % 72) / 18;
            int chest = Chest.FindChest(left, top);
            for (int A = 0; A < Main.chest[chest].item.Length; A++)
            {
                if (Main.chest[chest].item[A].type > 0)
                {
                    return false;
                }
            }
            return base.CanKillTile(i, j, ref blockDamaged);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
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
        public override LocalizedText DefaultContainerName(int frameX, int frameY)
        {
            return Language.GetText("Mods.DDmod.Tiles.绿岩存储仓Tile.MapEntry");
        }
        public override ushort GetMapOption(int i, int j)
        {
            return 0;
        }
        public override bool IsLockedChest(int i, int j)
        {
            return Main.tile[i, j].TileFrameX / 72 == 1;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override void HitWire(int i, int j)
        {
            Tile Wiredens = Main.tile[DDSystem.Wiredens];
            if (Wiredens.TileType == ModContent.TileType<绿岩测验机Tile>())
            {
                Tile tile = Main.tile[i, j];
                int left = i - (int)tile.TileFrameX % (4 * 18) / 18;
                int top = j - (int)tile.TileFrameY % (4 * 18) / 18;

                for (int a = 0; a < 4; a++)
                {
                    for (int b = 0; b < 4; b++)
                    {
                        tile = Main.tile[left + a, top + b];
                        if (tile.TileFrameX >= 72)
                        {
                            tile.TileFrameX -= 72;
                        }
                    }
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, left, top, 4, 4);
                }

                if (Wiring.running)
                {
                    for (int a = 0; a < 4; a++)
                    {
                        for (int b = 0; b < 4; b++)
                        {
                            Wiring.SkipWire(left+a, top + b);
                        }
                    }
                }
            }
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            NewDust(new Vector2(i, j) * 16, 16, 16, ModContent.DustType<绿岩电光粒子>(), Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), Scale: Main.rand.NextFloat(0.75F, 1.25F));
            return base.CreateDust(i, j, ref type);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }
        public static string MapName(string name, int i, int j)
        {
            int left = i;
            int top = j;
            Tile tile = Main.tile[i, j];
            left -= (tile.TileFrameX % 72) / 18;
            top -= (tile.TileFrameY % 72) / 18;

            int chest = Chest.FindChest(left, top);
            if (chest < 0)
            {
                return Language.GetTextValue("LegacyChestType.0");
            }

            if (Main.chest[chest].name == "")
            {
                return name;
            }

            return name + ": " + Main.chest[chest].name;
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            if (Math.Abs(i - (int)Main.LocalPlayer.Center.X / 16) <= Player.tileRangeX - 1 && Math.Abs(j - (int)Main.LocalPlayer.Center.Y / 16) <= Player.tileRangeY - 1)
            {
                Tile tile = Main.tile[i, j];
                Main.mouseRightRelease = false;
                int left = i;
                int top = j;
                left -= (tile.TileFrameX % 72) / 18;
                top -= (tile.TileFrameY % 72) / 18;

                player.CloseSign();
                player.SetTalkNPC(-1);
                Main.npcChatCornerItem = 0;
                Main.npcChatText = "";
                if (Main.editChest)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    Main.editChest = false;
                    Main.npcChatText = string.Empty;
                }

                if (player.editedChestName)
                {
                    NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
                    player.editedChestName = false;
                }

                bool isLocked = Chest.IsLocked(left, top);
                if (Main.netMode == NetmodeID.MultiplayerClient && !isLocked)
                {
                    if (left == player.chestX && top == player.chestY && player.chest >= 0)
                    {
                        player.chest = -1;
                        Recipe.FindRecipes();
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    else
                    {
                        NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
                        Main.stackSplit = 600;
                    }
                }
                else
                {
                    if (isLocked)
                    {
                        int key = ModContent.ItemType<晶凝箱>();
                        if (player.ConsumeItem(key) && Chest.Unlock(left, top))
                        {
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                NetMessage.SendData(MessageID.LockAndUnlock, -1, -1, null, player.whoAmI, 1f, left, top);
                            }
                        }
                    }
                    else
                    {
                        int chest = Chest.FindChest(left, top);
                        if (chest >= 0)
                        {
                            Main.stackSplit = 600;
                            if (chest == player.chest)
                            {
                                player.chest = -1;
                                SoundEngine.PlaySound(SoundID.MenuClose);
                            }
                            else
                            {
                                SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
                                player.OpenChest(left, top, chest);
                            }

                            Recipe.FindRecipes();
                        }
                    }
                }

                return true;
            }
            return false;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int left = i;
            int top = j;
            left -= (tile.TileFrameX % 72) / 18;
            top -= (tile.TileFrameY % 72) / 18;

            int chest = Chest.FindChest(left, top);
            player.cursorItemIconID = -1;
            if (chest < 0)
            {
                player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
            }
            else
            {
                string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY);
                player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : defaultName;
                if (player.cursorItemIconText == defaultName)
                {
                    if (Icon > 0)
                        player.cursorItemIconID = Icon;

                    player.cursorItemIconText = "";
                }
            }


            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }
        public Asset<Texture2D> TileTexture;
        public Asset<Texture2D> TileTexture2;

        public string T => "DDmod/Content/Tiles/绿岩/绿岩存储仓Tile_Glow";
        public string T2 => "DDmod/Content/Tiles/绿岩/绿岩存储仓2Tile";

        public override void Load()
        {
            if (!Main.dedServ)
            {
                TileTexture = ModContent.Request<Texture2D>(T);
            }
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
        }
        public bool InSmartCursorHighlightArea(int x, int y, out bool actuallySelected)
        {
            actuallySelected = Main.SmartInteractTileCoordsSelected.Contains(new Microsoft.Xna.Framework.Point(x, y));

            if (Collision.InTileBounds(x, y, Main.TileInteractionLX, Main.TileInteractionLY, Main.TileInteractionHX, Main.TileInteractionHY))
            {
                return true;
            }
            return false;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Color color = Lighting.GetColor(i, j, Color.White);
            int left = i;
            int top = j;
            left -= (tile.TileFrameX % 72) / 18;
            top -= (tile.TileFrameY % 72) / 18;
            int chest = Chest.FindChest(left, top);

            Vector2 zero = new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            int height = 16;
            if (chest > 0)
            {
                Main.spriteBatch.Draw(TextureAssets.Tile[Type].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY + 72 * Main.chest[chest].frame, 16, height)), color, 0f, Vector2.Zero, 1f, 0, 0f);

                Main.spriteBatch.Draw(TileTexture.Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY + 72 * Main.chest[chest].frame, 16, height)), Color.White, 0f, Vector2.Zero, 1f, 0, 0f);

                if (Main.InSmartCursorHighlightArea(i, j, out var actuallySelected))
                {
                    int num = (color.R + color.G + color.B) / 3;
                    if (num > 10)
                    {
                        if (tile.TileFrameX < 72)
                        {
                            Main.spriteBatch.Draw(TextureAssets.HighlightMask[Type].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY + 72 * Main.chest[chest].frame, 16, height)), Colors.GetSelectionGlowColor(actuallySelected, num), 0f, Vector2.Zero, 1f, 0, 0f);
                        }
                        else
                        {

                            Main.spriteBatch.Draw(TextureAssets.HighlightMask[Type].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY + 72 * Main.chest[chest].frame, 16, height)), Lighting.GetColor(i, j, new Color(255, 100, 100)), 0f, Vector2.Zero, 1f, 0, 0f);

                        }
                    }
                }
                else if (!Collision.InTileBounds(i, j, Main.TileInteractionLX, Main.TileInteractionLY, Main.TileInteractionHX, Main.TileInteractionHY))
                {
                    left = (int)(Main.LocalPlayer.Dplayer().MouseWorld.X / 16);
                    top = (int)(Main.LocalPlayer.Dplayer().MouseWorld.Y / 16);
                    if (left > 0 && left < Main.maxTilesX && top > 0 && top < Main.maxTilesY && Math.Abs(left - (int)Main.LocalPlayer.Center.X / 16) <= Player.tileRangeX - 1 && Math.Abs(top - (int)Main.LocalPlayer.Center.Y / 16) <= Player.tileRangeY - 1)
                    {
                        Tile tile2 = Main.tile[left, top];
                        left -= (tile2.TileFrameX % 72) / 18;
                        top -= (tile2.TileFrameY % 72) / 18;
                        if (chest == Chest.FindChest(left, top))
                        {
                            color = Lighting.GetColor(i, j, new Color(100, 255, 100));
                            if (tile.TileFrameX>=72)
                            {
                                color = Lighting.GetColor(i, j, new Color(255, 100, 100));
                            }
                            Main.spriteBatch.Draw(TextureAssets.HighlightMask[Type].Value, new Vector2((float)(i * 16 - (int)Main.screenPosition.X), (float)(j * 16 - (int)Main.screenPosition.Y)) + zero, new Rectangle?(new Rectangle((int)tile.TileFrameX, (int)tile.TileFrameY + 72 * Main.chest[chest].frame, 16, height)), color, 0f, Vector2.Zero, 1f, 0, 0f);
                        }

                    }
                }
            }
            return false;
        }
    }
}